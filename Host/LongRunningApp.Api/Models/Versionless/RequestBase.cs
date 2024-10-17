namespace LongRunningApp.Api.Models.Versionless;
public record RequestBase
{
    public required string ConnectionId { get; set; }
}
