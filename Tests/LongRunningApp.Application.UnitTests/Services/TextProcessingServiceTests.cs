using LongRunningApp.Application.Models;
using LongRunningApp.Application.Services;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text;

namespace LongRunningApp.Application.UnitTests.Services;

[TestClass]
public sealed class TextProcessingServiceTests
{
    private Mock<IOptions<AppLayerSettings>> _appLayerSettingsMock;
    private Mock<IProgress<int>> _progressMock;

    [TestInitialize]
    public void TestInitialize()
    {
        _appLayerSettingsMock = new Mock<IOptions<AppLayerSettings>>();
        _progressMock = new Mock<IProgress<int>>();
    }

    [TestMethod]
    [DataRow("", "", -1)]
    [DataRow(null, "", -1)]
    [DataRow(" ", "", -1)]
    [DataRow("Hello, World!", " 1!1,1H1W1d1e1l3o2r1/SGVsbG8sIFdvcmxkIQ==", 100)]
    public async Task ProcessText_WithText_ShouldReturnEmptyResult(string sourceText, string expectedResult, int expectedPercentage)
    {
        //Arrange
        _appLayerSettingsMock.Setup(x => x.Value).Returns(new AppLayerSettings() { DelayEmulationMaxSec = 0 });
        var service = new TextProcessingService(_appLayerSettingsMock.Object);
        var processedResult = new StringBuilder();
        var request = new TextProcessingRequest() { Text = sourceText };

        //Act
        await foreach (var currentProcessingResult in service.ProcessText(request, _progressMock.Object))
        {
            processedResult.Append(currentProcessingResult.Text);
        }

        //Assert
        Assert.AreEqual(expectedResult, processedResult.ToString());
        if (expectedPercentage < 0)
        {
            _progressMock.Verify(x => x.Report(It.IsAny<int>()), Times.Never());
        }
        else
        {
            _progressMock.Verify(x => x.Report(expectedPercentage), Times.Once());
        }
        
    }

    [TestMethod]
    public async Task ProcessText_WithTextAndDelay_ShouldHasDelay()
    {
        //Arrange
        _appLayerSettingsMock.Setup(x => x.Value).Returns(new AppLayerSettings() { DelayEmulationMaxSec = 1 });
        var service = new TextProcessingService(_appLayerSettingsMock.Object);
        var processedResult = new StringBuilder();
        var request = new TextProcessingRequest() { Text = "TM" };

        Stopwatch stopwatch = Stopwatch.StartNew();

        //Act
        stopwatch.Start();
        await foreach (var currentProcessingResult in service.ProcessText(request, _progressMock.Object))
        {
            processedResult.Append(currentProcessingResult.Text);
        }
        stopwatch.Stop();

        //Assert
        Assert.AreEqual("M1T1/VE0=", processedResult.ToString());
        Assert.IsTrue(stopwatch.Elapsed.TotalSeconds > 1);
    }

    [TestMethod]
    public async Task ProcessText_WithCancelation_ShouldCancelProcessing()
    {
        //Arrange
        _appLayerSettingsMock.Setup(x => x.Value).Returns(new AppLayerSettings() { DelayEmulationMaxSec = 0 });
        var service = new TextProcessingService(_appLayerSettingsMock.Object);
        var processedResult = new StringBuilder();
        var request = new TextProcessingRequest() { Text = "TestText" };
        var cts = new CancellationTokenSource();

        //Act
        await foreach (var currentProcessingResult in service.ProcessText(request, _progressMock.Object, cts.Token))
        {
            processedResult.Append(currentProcessingResult.Text);
            if (processedResult.ToString() == "T2e2s1")
            {
                cts.Cancel();
            }
        }

        //Assert
        Assert.IsTrue(cts.IsCancellationRequested);
        Assert.AreEqual("T2e2s1", processedResult.ToString());
        _progressMock.Verify(x => x.Report(26), Times.Once());
        _progressMock.Verify(x => x.Report(It.IsInRange(27, 100, Moq.Range.Inclusive)), Times.Never());
    }


    [TestMethod]
    public async Task ProcessText_WithLongText_FirstPercentageShouldEqual1()
    {
        //Arrange
        _appLayerSettingsMock.Setup(x => x.Value).Returns(new AppLayerSettings() { DelayEmulationMaxSec = 0 });
        var service = new TextProcessingService(_appLayerSettingsMock.Object);
        var processedResult = new StringBuilder();
        var request = new TextProcessingRequest() { Text = "1234567890QqWwEeRrTtYyUuIiOoPpAaSsDdFfGgHhJjKkLlZzXxCcVvBbNnMm" };
        var cts = new CancellationTokenSource();

        //Act
        await foreach (var currentProcessingResult in service.ProcessText(request, _progressMock.Object, cts.Token))
        {
            cts.Cancel();
        }

        //Assert
        Assert.IsTrue(cts.IsCancellationRequested);
        _progressMock.Verify(x => x.Report(1), Times.Once());
    }
}
