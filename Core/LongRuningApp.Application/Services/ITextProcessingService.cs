
namespace LongRunningApp.Application.Services;
public interface ITextProcessingService
{
    IAsyncEnumerable<string> ProcessText(string text, CancellationToken cancellation = default);
}