
using LongRunningApp.Application.Models;

namespace LongRunningApp.Application.Services;
public interface ITextProcessingService
{
    IAsyncEnumerable<ITextProcessingResult> ProcessText(ITextProcessingRequest request,
                                                        IProgress<int> progress,
                                                        CancellationToken cancellation = default);
}