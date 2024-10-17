
namespace LongRunningApp.Api.Services;

public interface ITextProcessorService
{
    Task<Guid> StartProcessingTextAsync(string connectionId, string text);
    Task CancelProcessingTextAsync(Guid processingTaskId);
    int CheckProgressProcessingText(Guid processingTaskId);

}