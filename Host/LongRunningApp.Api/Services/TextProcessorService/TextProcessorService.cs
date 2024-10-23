using LongRunningApp.Api.Hubs.v1;
using LongRunningApp.Api.Models.v1.Hubs;
using LongRunningApp.Application.Managers;
using LongRunningApp.Application.Models;
using LongRunningApp.Application.Services;
using LongRunningApp.Infrastructure.Services;
using LongRunningApp.Shared.Extensions;
using Microsoft.AspNetCore.SignalR;
using System.Text;

namespace LongRunningApp.Api.Services;
public sealed class TextProcessorService(
    ILogger<TextProcessorService> logger,
    ITextProcessingService textProcessingService,
    IHubContext<TextProcessorHub> processingTextHubContext,
    IProcessingTasksManager<ITextProcessorData> tasksManager,
    ICacheService cache) : ITextProcessorService
{
    public async Task<Guid> StartProcessingTextAsync(string connectionId, string text)
    {
        if (logger.LogErrorIfNullOrWhiteSpace(connectionId, nameof(connectionId))
            || logger.LogErrorIfNullOrWhiteSpace(text, nameof(text)))
        {
            return Guid.Empty;
        }

        logger.LogTrace($"Receive text [{text}] for processing from connection id [{connectionId}].");

        //if we have result in the Cache, then send it immediately
        var cacheValue = await cache.ReadFromCacheAsync(text);
        if (!string.IsNullOrEmpty(cacheValue))
        {
            var response = new ProcessingTextResponse()
            {
                Text = cacheValue,
                ProgressPercentage = 100
            };
            await processingTextHubContext.Clients.Client(connectionId).SendAsync(HubNames.ProcessTextResponse, response);
            logger.LogTrace($"Result [{cacheValue}] for text [{text}] read from cache and has already sent to connection id [{connectionId}].");
            return Guid.Empty;
        }

        //run processing task and return taskId
        var data = new TextProcessorData() { Text = text, ConnectionId = connectionId };

        var processingTaskId = await tasksManager.AddTaskAsync(data, ProcessingTextAsync);

        logger.LogTrace($"Added new task for processing with taskId:[{processingTaskId}].");

        return processingTaskId;
    }

    public async Task CancelProcessingTextAsync(Guid processingTaskId)
    {
        logger.LogTrace($"Starting cancellation text processing task with id:[{processingTaskId}]");

        var progress = tasksManager.GetTaskProgress(processingTaskId);
        await tasksManager.RemoveTaskAsync(processingTaskId);

        logger.LogTrace($"Finished cancellation text processing task with id:[{processingTaskId}]. Progress was:[{progress}].");
    }

    public int CheckProgressProcessingText(Guid processingTaskId)
    {
        logger.LogTrace($"Starting check progress text processing task with id:[{processingTaskId}]");

        var progress = tasksManager.GetTaskProgress(processingTaskId);

        logger.LogTrace($"Finished check progress text processing task with id:[{processingTaskId}]. Current progress:[{progress}].");

        return progress;
    }

    private async Task ProcessingTextAsync(ITextProcessorData data, IProgress<int> progress, CancellationToken cancellationToken)
    {
        var processedResult = new StringBuilder();
        var processedRequest = new TextProcessingRequest() { Text = data.Text };
        await foreach (var processedPart in textProcessingService.ProcessText(processedRequest, progress, cancellationToken))
        {
            processedResult.Append(processedPart.Text);

            var response = new ProcessingTextResponse() 
            { 
                Text = processedPart.Text,
                ProgressPercentage = processedPart.ProgressPercentage
            };
            await processingTextHubContext.Clients.Client(data.ConnectionId).SendAsync(HubNames.ProcessTextResponse, response);

            if (cancellationToken.IsCancellationRequested)
            {
                logger.LogTrace($"Connection id [{data.ConnectionId}] request cancel processing text [{data.Text}] with result [{processedResult}].");
                return;
            }
        }

        await processingTextHubContext.Clients.Client(data.ConnectionId).SendAsync(HubNames.ProcessTextResponse, ProcessingTextResponse.EmptyWith100Percentage);

        await cache.WriteToCacheAsync(data.Text, processedResult.ToString());

        logger.LogTrace($"Result [{processedResult}] for text [{data.Text}] has already sent to connection id [{data.ConnectionId}].");
    }

}
