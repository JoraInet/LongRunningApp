namespace LongRunningApp.Application.Models;

public sealed record TextProcessingResult : ITextProcessingResult
{
    public static TextProcessingResult Empty = new() { Text = string.Empty, ProgressPercentage = 0 };
    public required int ProgressPercentage { get; set; }
    public required string Text { get; set; }
}
