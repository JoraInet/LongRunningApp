using LongRunningApp.Application.Managers;
using Microsoft.Extensions.Logging;

namespace LongRunningApp.Application.UnitTests.Managers;
[TestClass]
public sealed class ProcessingTasksManagerTests
{
    private Mock<ILogger<ProcessingTasksManager<ITestData>>> _loggerMock;
    private readonly int taskDelay = 500;

    [TestInitialize]
    public void TestInitialize()
    {
        _loggerMock = new Mock<ILogger<ProcessingTasksManager<ITestData>>>();
    }

    [TestMethod]
    public async Task AddTaskAsync_ArgumentDataNull_ShouldThrowArgumentNullException()
    {
        //Arrange
        var manager = new ProcessingTasksManager<ITestData>(_loggerMock.Object);
        var hasException = false;

        //Act
        try
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            await manager.AddTaskAsync(null, LongRunningTask);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
        catch (Exception ex)
        {
            hasException = ex is ArgumentNullException;
        }

        //Assert
        Assert.IsTrue(hasException);
    }


    [TestMethod]
    public async Task AddTaskAsync_ArgumentTaskNull_ShouldThrowArgumentNullException()
    {
        //Arrange
        var manager = new ProcessingTasksManager<ITestData>(_loggerMock.Object);
        var hasException = false;

        //Act
        try
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            await manager.AddTaskAsync(new TestData(), null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
        catch (Exception ex)
        {
            hasException = ex is ArgumentNullException;
        }

        //Assert
        Assert.IsTrue(hasException);
    }

    [TestMethod]
    public async Task RemoveTaskAsync_WithEmptyGuid_ShouldLogWarning()
    {
        //Arrange
        var manager = new ProcessingTasksManager<ITestData>(_loggerMock.Object);

        //Act
        await manager.RemoveTaskAsync(Guid.Empty);

        //Assert
        _loggerMock.VerifyLoggerHasExactMessage(LogLevel.Warning, "Trying to remove task with empty GUID as 'taskId'");
    }

    [TestMethod]
    public async Task AddTaskAsync_WithTask_ShouldCreateProcessingTaskAndReturnTaskIdWithoutException()
    {
        //Arrange
        var manager = new ProcessingTasksManager<ITestData>(_loggerMock.Object);
        var hasException = false;
        var taskId = Guid.Empty;
        //Act
        try
        {
            taskId = await manager.AddTaskAsync(new TestData() { TaskDelay = 0 }, LongRunningTask);
        }
        catch (Exception)
        {
            hasException = true;
        }

        //Assert
        Assert.IsFalse(hasException);
        Assert.IsTrue(taskId != Guid.Empty);
        _loggerMock.VerifyLoggerHasExactMessage(LogLevel.Trace, $"Added task with id:[{taskId}]");
    }

    [TestMethod]
    public async Task AddTaskAsync_WithTwoTasks_ShouldAddTasksAndRemoveThemAfterFinished()
    {
        //Arrange
        var manager = new ProcessingTasksManager<ITestData>(_loggerMock.Object);

        //Act
        var taskId1 = await manager.AddTaskAsync(new TestData(), LongRunningTask);
        var taskId2 = await manager.AddTaskAsync(new TestData(), LongRunningTask);

        await Task.Delay(4000);

        //Assert
        Assert.IsTrue(taskId1 != Guid.Empty);
        Assert.IsTrue(taskId2 != Guid.Empty);
        _loggerMock.VerifyLoggerHasExactMessage(LogLevel.Trace, $"Added task with id:[{taskId1}]");
        _loggerMock.VerifyLoggerHasExactMessage(LogLevel.Trace, $"Added task with id:[{taskId2}]");
        _loggerMock.VerifyLoggerHasExactMessage(LogLevel.Trace, $"Removed task with id:[{taskId1}]");
        _loggerMock.VerifyLoggerHasExactMessage(LogLevel.Trace, $"Removed task with id:[{taskId2}]");
    }

    [TestMethod]
    public void GetTaskProgress_WithEmptyGuid_ShouldLogWarning()
    {
        //Arrange
        var manager = new ProcessingTasksManager<ITestData>(_loggerMock.Object);

        //Act
        manager.GetTaskProgress(Guid.Empty);

        //Assert
        _loggerMock.VerifyLoggerHasExactMessage(LogLevel.Warning, "Trying to get task progress with empty GUID as 'taskId'.");

    }

    [TestMethod]
    public void GetTaskProgress_WithWrongGuid_ShouldLogWarning()
    {
        //Arrange
        var manager = new ProcessingTasksManager<ITestData>(_loggerMock.Object);

        //Act
        manager.GetTaskProgress(Guid.NewGuid());

        //Assert
        _loggerMock.VerifyLoggerContainsMessage(LogLevel.Warning, "Trying to get progress for task that not exist with id:");
    }

    [TestMethod]
    public async Task GetTaskProgress_WithValidId_ShouldReturnTaskProgress()
    {
        //Arrange
        var manager = new ProcessingTasksManager<ITestData>(_loggerMock.Object);

        //Act
        var taskId = await manager.AddTaskAsync(new TestData(), LongRunningTask);
        await Task.Delay(1500);
        var progress = manager.GetTaskProgress(taskId);

        //Assert
        Assert.IsTrue(taskId != Guid.Empty);
        Assert.IsTrue(progress > 40 && progress < 100);
    }

    [TestMethod]
    public async Task RemoveTaskAsync_WithValidId_ShouldCancelTask()
    {
        //Arrange
        var manager = new ProcessingTasksManager<ITestData>(_loggerMock.Object);
        var taskId1 = await manager.AddTaskAsync(new TestData(), LongRunningTask);
        var taskId2 = await manager.AddTaskAsync(new TestData(), LongRunningTask);
        var taskId3 = await manager.AddTaskAsync(new TestData(), LongRunningTask);

        //Act
        await manager.RemoveTaskAsync(taskId2);

        await Task.Delay(4000);

        //Assert
        _loggerMock.VerifyLoggerHasExactMessage(LogLevel.Trace, $"Cancel task with id:[{taskId2}]");
        _loggerMock.VerifyLoggerHasExactMessage(LogLevel.Trace, $"Added task with id:[{taskId1}]");
        _loggerMock.VerifyLoggerHasExactMessage(LogLevel.Trace, $"Added task with id:[{taskId2}]");
        _loggerMock.VerifyLoggerHasExactMessage(LogLevel.Trace, $"Added task with id:[{taskId3}]");
        _loggerMock.VerifyLoggerHasExactMessage(LogLevel.Trace, $"Removed task with id:[{taskId1}]");
        _loggerMock.VerifyLoggerHasExactMessage(LogLevel.Trace, $"Removed task with id:[{taskId2}]");
        _loggerMock.VerifyLoggerHasExactMessage(LogLevel.Trace, $"Removed task with id:[{taskId3}]");
    }

    public interface ITestData : IProcessingData
    {
        public int TaskDelay { get; set; }
    }

    private record TestData : ITestData
    {
        public int TaskDelay { get; set; } = 500;
    }

    private async Task LongRunningTask(ITestData data, IProgress<int> progress, CancellationToken cancellationToken)
    {
        for (int i = 0; i < 5; i++)
        {
            progress.Report((i + 1) * 100 / 5);
            await Task.Delay(taskDelay);
        }
    }
}
