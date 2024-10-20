using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace LongRunningApp.Application.Managers;

public sealed class ProcessingTasksManager<TData>(ILogger<ProcessingTasksManager<TData>> logger) : IProcessingTasksManager<TData>
    where TData : IProcessingData
{
    private readonly ConcurrentDictionary<Guid, IProcessingTask> _processingTasks = new();
    private readonly ConcurrentDictionary<Guid, int> _progressTasks = new();

    public async Task<Guid> AddTaskAsync(TData data, Func<TData, IProgress<int>, CancellationToken, Task> func)
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentNullException.ThrowIfNull(func);

        var taskId = Guid.NewGuid();

        _processingTasks[taskId] = GetNewProcessingTask(taskId, data, func);

        await Task.CompletedTask;

        logger.LogTrace($"Added task with id:[{taskId}]");

        return taskId;
    }

    public async Task RemoveTaskAsync(Guid taskId)
    {
        if (taskId == Guid.Empty)
        {
            logger.LogWarning("Trying to remove task with empty GUID as 'taskId'.");
            return;
        }

        try
        {
            if (_processingTasks.TryRemove(taskId, out var removedTask))
            {
                if (!removedTask.TaskInProgress.IsCompleted)
                {
                    removedTask.CancellationTokenSource.Cancel();
                    logger.LogTrace($"Cancel task with id:[{taskId}]");
                }
                await removedTask.TaskInProgress;
                removedTask.CancellationTokenSource.Dispose();
                removedTask.TaskInProgress.Dispose();
                _progressTasks.TryRemove(taskId, out _);
                logger.LogTrace($"Removed task with id:[{taskId}]");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error while remove task with id:[{taskId}]");
        }
    }

    public int GetTaskProgress(Guid taskId)
    {
        if (taskId == Guid.Empty)
        {
            logger.LogWarning("Trying to get task progress with empty GUID as 'taskId'.");
            return 0;
        }

        if (!_progressTasks.Keys.Contains(taskId))
        {
            logger.LogWarning($"Trying to get progress for task that not exist with id:[{taskId}]");
            return 0;
        }

        return _progressTasks.TryGetValue(taskId, out var progress) ? progress : 0;
    }

    private IProcessingTask GetNewProcessingTask(
        Guid taskId, 
        TData data, 
        Func<TData, IProgress<int>, CancellationToken, Task> func)
    {
        var cts = new CancellationTokenSource();
        var progress = new Progress<int>(x =>
        {
            _progressTasks[taskId] = x;
        });
        return new ProcessingTask()
        {
            CancellationTokenSource = cts,
            ProgressPercentage = progress,
            TaskInProgress = Task.Run(() => func(data, progress, cts.Token))
                .ContinueWith(async t =>
                {
                    await RemoveTaskAsync(taskId);
                })
        };
    }

}
