using LongRunningApp.Api.Services;
using Microsoft.AspNetCore.SignalR;

namespace LongRunningApp.Api.Hubs.v1;

public sealed class TextProcessorHub(
    ILogger<TextProcessorHub> logger,
    ITextProcessorService textProcessorService) : Hub
{

    [HubMethodName(HubNames.ProcessTextRequest)]
    public async Task ProcessText(string text)
    {
        logger.LogInformation($"Receive text [{text}] from connection id [{Context.ConnectionId}]");
        await textProcessorService.StartProcessingTextAsync(Context.ConnectionId, text);
    }

    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }
}
