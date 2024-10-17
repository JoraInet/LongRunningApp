namespace LongRunningApp.Api.Services;
public record class TextProcessorData : ITextProcessorData
{
    public required string Text { get; set; }
    public required string ConnectionId { get; set; }
}
