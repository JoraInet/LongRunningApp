
namespace LongRunningApp.Application.Models;
public sealed record TextProcessingRequest : ITextProcessingRequest
{
    public required string Text { get; set; }
}
