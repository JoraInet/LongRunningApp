
namespace LongRunningApp.Application.Managers;
public interface IProcessingTasksManager<TData> 
    where TData : IProcessingData
{
    Task<Guid> AddTaskAsync(TData data, Func<TData, IProgress<int>, CancellationToken, Task> func);
    int GetTaskProgress(Guid taskId);
    Task RemoveTaskAsync(Guid taskId);
}