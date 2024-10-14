using LongRunningApp.Api.Hubs.v1;
using LongRunningApp.Application.Services;
using LongRunningApp.Infrastructure.Services;
using Microsoft.AspNetCore.SignalR;
using System.Text;

namespace LongRunningApp.Api.Services;
public sealed class TextProcessorService : ITextProcessorService
{
    private readonly ILogger<TextProcessorService> _logger;
    private readonly ITextProcessingService _textProcessingService;
    private readonly IHubContext<TextProcessorHub> _processingTextHubContext;
    private readonly ICacheService _cache;

    public TextProcessorService(
        ILogger<TextProcessorService> logger,
        ITextProcessingService textProcessingService,
        IHubContext<TextProcessorHub> processingTextHubContext,
        ICacheService cache)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _textProcessingService = textProcessingService ?? throw new ArgumentNullException(nameof(textProcessingService));
        _processingTextHubContext = processingTextHubContext ?? throw new ArgumentNullException(nameof(processingTextHubContext));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task PerformProcessing(string connectionId, string text, CancellationToken cancellation)
    {
        if (string.IsNullOrWhiteSpace(connectionId))
        {
            throw new ArgumentException($"'{nameof(connectionId)}' cannot be null or whitespace.", nameof(connectionId));
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException($"'{nameof(text)}' cannot be null or whitespace.", nameof(text));
        }

        _logger.LogInformation($"Receive text [{text}] for processing from connection id [{connectionId}].");

        var cacheValue = await _cache.ReadFromCacheAsync(text);
        if (!string.IsNullOrEmpty(cacheValue))
        {
            await _processingTextHubContext.Clients.Client(connectionId).SendAsync(HubNames.ProcessTextResponse, cacheValue);
            _logger.LogInformation($"Result [{cacheValue}] for text [{text}] read from cache and has already sent to connection id [{connectionId}] .");
            return;
        }

        var processedResult = new StringBuilder();
        await foreach (var processedPart in _textProcessingService.ProcessText(text, cancellation))
        {
            processedResult.Append(processedPart);
            await _processingTextHubContext.Clients.Client(connectionId).SendAsync(HubNames.ProcessTextResponse, processedPart);

            if (cancellation.IsCancellationRequested)
            {
                _logger.LogInformation($"Connection id [{connectionId}] request cancel processing text [{text}] with result [{processedResult}].");
                return;
            }
        }

        await _cache.WriteToCacheAsync(text, processedResult.ToString());

        _logger.LogInformation($"Result [{processedResult}] for text [{text}] has already sent to connection id [{connectionId}].");
    }

}
