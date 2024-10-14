
namespace LongRunningApp.Api.Services
{
    public interface ITextProcessorService
    {
        Task PerformProcessing(string connectionId, string text, CancellationToken cancellation);
    }
}