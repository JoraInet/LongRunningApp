namespace LongRunningApp.Api.Models.v1.Hubs;

public sealed record ProcessingTextResponse
{
    public static ProcessingTextResponse EmptyWith100Percentage => new() { Text = string.Empty, ProgressPercentage = 100 };
    public required int ProgressPercentage { get; set; }
    public required string Text { get; set; }
}
