using LongRunningApp.Application.Models;
using LongRunningApp.Application.Services;
using Microsoft.Extensions.Options;
using Moq;
using System.Diagnostics;

namespace LongRunningApp.Application.UnitTests.Services;

[TestClass]
public sealed class TextProcessingServiceTests
{
    private Mock<IOptions<AppLayerSettings>> _appLayerSettingsMock;

    [TestInitialize]
    public void TestInitialize()
    {
        _appLayerSettingsMock = new Mock<IOptions<AppLayerSettings>>();
    }

    [TestMethod]
    [DataRow("", "")]
    [DataRow("Hello, World!", " 1,1!1d1e1H1r1W1o2l3")]
    public async Task ProcessText_WithText_ShouldReturnEmptyResult(string sourceText, string expectedResult)
    {
        //Arrange
        _appLayerSettingsMock.Setup(x => x.Value).Returns(new AppLayerSettings() { DelayEmulationMaxSec = 0 });
        var service = new TextProcessingService(_appLayerSettingsMock.Object);
        string? processedResult = null;

        //Act
        await foreach (var currentProcessingResult in service.ProcessText(sourceText))
        {
            processedResult += currentProcessingResult;
        }

        //Assert
        Assert.AreEqual(expectedResult, processedResult);
    }

    [TestMethod]
    public async Task ProcessText_WithTextAndDelay_ShouldHasDelay()
    {
        //Arrange
        _appLayerSettingsMock.Setup(x => x.Value).Returns(new AppLayerSettings() { DelayEmulationMaxSec = 1 });
        var service = new TextProcessingService(_appLayerSettingsMock.Object);
        string? processedResult = null;

        Stopwatch stopwatch = Stopwatch.StartNew();

        //Act
        stopwatch.Start();
        await foreach (var currentProcessingResult in service.ProcessText("TestText"))
        {
            processedResult += currentProcessingResult;
        }
        stopwatch.Stop();

        //Assert
        Assert.AreEqual("s1x1e2t2T2", processedResult);
        Assert.IsTrue(stopwatch.Elapsed.TotalSeconds > 1);
    }

    [TestMethod]
    public async Task ProcessText_WithCancelation_ShouldCancelProcessing()
    {
        //Arrange
        _appLayerSettingsMock.Setup(x => x.Value).Returns(new AppLayerSettings() { DelayEmulationMaxSec = 0 });
        var service = new TextProcessingService(_appLayerSettingsMock.Object);
        string? processedResult = null;
        var cts = new CancellationTokenSource();

        //Act
        await foreach (var currentProcessingResult in service.ProcessText("TestText", cts.Token))
        {
            processedResult += currentProcessingResult;
            if (processedResult == "s1x1e")
            {
                cts.Cancel();
            }
        }

        //Assert
        Assert.IsTrue(cts.IsCancellationRequested);
        Assert.AreEqual("s1x1e", processedResult);
    }
}
