using LongRunningApp.Application.Managers;

namespace LongRunningApp.Api.Services;
public interface ITextProcessorData : IProcessingData
{
    public string Text { get; set; }
    public string ConnectionId { get; set; }
}
