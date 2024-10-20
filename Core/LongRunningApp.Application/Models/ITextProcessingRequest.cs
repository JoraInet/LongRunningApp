using LongRunningApp.Application.Managers;

namespace LongRunningApp.Application.Models;
public interface ITextProcessingRequest : IProcessingData
{
    string Text { get; set; }
}