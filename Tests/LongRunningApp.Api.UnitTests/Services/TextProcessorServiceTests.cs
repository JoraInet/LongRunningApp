using LongRunningApp.Api.Hubs.v1;
using LongRunningApp.Api.Services;
using LongRunningApp.Application.Managers;
using LongRunningApp.Application.Services;
using LongRunningApp.Application.UnitTests;
using LongRunningApp.Infrastructure.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongRunningApp.Api.UnitTests.Services;
[TestClass]
public sealed class TextProcessorServiceTests
{
    private Mock<ILogger<TextProcessorService>> _loggerMock;
    private Mock<ITextProcessingService> _textProcessingServiceMock;
    private Mock<IHubContext<TextProcessorHub>> _processingTextHubContextMock;
    private Mock<IProcessingTasksManager<ITextProcessorData>> _tasksManagerMock;
    private Mock<ICacheService> _cacheMock;

    [TestInitialize]
    public void TestInitialize()
    {
        _loggerMock = new Mock<ILogger<TextProcessorService>>();
        _textProcessingServiceMock = new Mock<ITextProcessingService>();
        _processingTextHubContextMock = new Mock<IHubContext<TextProcessorHub>>();
        _tasksManagerMock = new Mock<IProcessingTasksManager<ITextProcessorData>>();
        _cacheMock = new Mock<ICacheService>();
    }

    [TestMethod]
    public async Task StartProcessingTextAsync_ConnectionIdNull_ShouldLogError()
    {
        //Arrange
        var service = CreateNewService();

        //Act
        var taskId = await service.StartProcessingTextAsync("", "text");

        //Assert
        Assert.AreEqual(Guid.Empty, taskId);
        _loggerMock.VerifyLoggerHasExactMessage(LogLevel.Error, "'connectionId' cannot be null or whitespace.");
    }

    [TestMethod]
    public async Task StartProcessingTextAsync_TextNull_ShouldLogError()
    {
        //Arrange
        var service = CreateNewService();

        //Act
        var taskId = await service.StartProcessingTextAsync("Id", "");

        //Assert
        Assert.AreEqual(Guid.Empty, taskId);
        _loggerMock.VerifyLoggerHasExactMessage(LogLevel.Error, "'text' cannot be null or whitespace.");
    }

    [TestMethod]
    public async Task StartProcessingTextAsync_CacheHasValue_ShouldReturnCacheValue()
    {
        //Arrange
        var service = CreateNewService();
        _processingTextHubContextMock.Setup(x => x.Clients.Client("Id")).Returns(Mock.Of<ISingleClientProxy>());
        _cacheMock.Setup(x => x.ReadFromCacheAsync("text")).ReturnsAsync("ResultFromCache");

        //Act
        var taskId = await service.StartProcessingTextAsync("Id", "text");

        //Assert
        Assert.AreEqual(Guid.Empty, taskId);
        _loggerMock.VerifyLoggerHasExactMessage(LogLevel.Trace,
            "Result [ResultFromCache] for text [text] read from cache and has already sent to connection id [Id].");
    }

    [TestMethod]
    public async Task StartProcessingTextAsync_CacheHasntValue_ShouldCallAddTask()
    {
        //Arrange
        var service = CreateNewService();
        var taskIdFake = Guid.NewGuid();
        _tasksManagerMock.Setup(x => x.AddTaskAsync(It.IsAny<ITextProcessorData>(),
                                                    It.IsAny<Func<ITextProcessorData, IProgress<int>, CancellationToken, Task>>()))
            .ReturnsAsync(taskIdFake);

        //Act
        var taskId = await service.StartProcessingTextAsync("Id", "text");

        //Assert
        Assert.IsTrue(taskId == taskIdFake);
        _tasksManagerMock.Verify(x => x.AddTaskAsync(It.IsAny<ITextProcessorData>(),
                                                     It.IsAny<Func<ITextProcessorData, IProgress<int>, CancellationToken, Task>>()), Times.Once());
        _loggerMock.VerifyLoggerHasExactMessage(LogLevel.Trace, $"Added new task for processing with taskId:[{taskId}].");
    }

    [TestMethod]
    public async Task CancelProcessingTextAsync_WithProcessingTaskId_ShouldCallRemoveTask()
    {
        //Arrange
        var service = CreateNewService();
        var taskIdFake = Guid.NewGuid();

        //Act
        await service.CancelProcessingTextAsync(taskIdFake);

        //Assert
        _tasksManagerMock.Verify(x => x.RemoveTaskAsync(taskIdFake), Times.Once());
        _loggerMock.VerifyLoggerContainsMessage(LogLevel.Trace, $"Finished cancellation text processing task with id:[{taskIdFake}].");
    }

    [TestMethod]
    public void CheckProgressProcessingTextAsync_WithProcessingTaskId_ShouldCallGetTaskProgress()
    {
        //Arrange
        var service = CreateNewService();
        var taskIdFake = Guid.NewGuid();
        _tasksManagerMock.Setup(x => x.GetTaskProgress(taskIdFake)).Returns(40);

        //Act
        var progress = service.CheckProgressProcessingText(taskIdFake);

        //Assert
        Assert.AreEqual(40, progress);
        _tasksManagerMock.Verify(x => x.GetTaskProgress(taskIdFake), Times.Once());
        _loggerMock.VerifyLoggerHasExactMessage(LogLevel.Trace,
            $"Finished check progress text processing task with id:[{taskIdFake}]. Current progress:[{progress}].");
    }

    private TextProcessorService CreateNewService() => new(
        _loggerMock.Object,
        _textProcessingServiceMock.Object,
        _processingTextHubContextMock.Object,
        _tasksManagerMock.Object,
        _cacheMock.Object);
}
