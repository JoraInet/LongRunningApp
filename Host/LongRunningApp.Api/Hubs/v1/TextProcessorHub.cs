using Microsoft.AspNetCore.SignalR;

namespace LongRunningApp.Api.Hubs.v1;

public sealed class TextProcessorHub : Hub
{
    private readonly ILogger<TextProcessorHub> _logger;

    public TextProcessorHub(ILogger<TextProcessorHub> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HubMethodName(HubNames.ProcessTextRequest)]
    public async Task ProcessText(string connectionId, string text)
    {
        _logger.LogInformation($"Receive text [{text}] from connection id [{connectionId}]");
        await Clients.Client(connectionId).SendAsync(HubNames.ProcessTextResponse, text);
    }

    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }

}
