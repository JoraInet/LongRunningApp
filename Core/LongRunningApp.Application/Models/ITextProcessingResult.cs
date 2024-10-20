namespace LongRunningApp.Application.Models;
public interface ITextProcessingResult
{
    int ProgressPercentage { get; set; }
    string Text { get; set; }
}