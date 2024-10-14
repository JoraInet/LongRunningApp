using LongRunningApp.Application.Models;
using LongRunningApp.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;

namespace LongRunningApp.Application.UnitTests;
[TestClass]
public sealed class SetupApplicationTests
{
    private Mock<IServiceCollection> _serviceCollectionMock;
    private Mock<IConfigurationManager> _configurationManagerMock;

    [TestInitialize]
    public void TestInitialize()
    {
        _serviceCollectionMock = new Mock<IServiceCollection>();
        _configurationManagerMock = new Mock<IConfigurationManager>();
    }

    [TestMethod]
    public void ConfigureApplicationLayer_WithDefaults_ShouldAddedRequired()
    {
        //Arrange
        var configSectionMock = new Mock<IConfigurationSection>();

        _configurationManagerMock.Setup(x => x.GetSection(nameof(AppLayerSettings))).Returns(configSectionMock.Object);

        var builderMock = new Mock<IHostApplicationBuilder>();
        builderMock.Setup(x => x.Configuration).Returns(_configurationManagerMock.Object);
        builderMock.Setup(x => x.Services).Returns(_serviceCollectionMock.Object);

        //Act
        builderMock.Object.ConfigureApplicationLayer();

        //Assert
        Assert.IsTrue(IsServiceAdded(typeof(ITextProcessingService), typeof(TextProcessingService), ServiceLifetime.Singleton));
        Assert.IsTrue(IsConfigureOptionsAdded(typeof(IConfigureOptions<AppLayerSettings>)));
    }

    private bool IsServiceAdded(Type serviceType, Type implementationType, ServiceLifetime lifetime)
        => _serviceCollectionMock.Invocations.Any(x => x.Method.Name == nameof(IServiceCollection.Add)
                                                        && x.Arguments.OfType<ServiceDescriptor>().Any(
                                                            y => y.Lifetime == lifetime && y.ServiceType == serviceType
                                                                && y.ImplementationType == implementationType));

    private bool IsConfigureOptionsAdded(Type serviceType)
        => _serviceCollectionMock.Invocations.Any(x => x.Method.Name == nameof(IServiceCollection.Add)
                                                        && x.Arguments.OfType<ServiceDescriptor>().Any(
                                                            y => y.Lifetime == ServiceLifetime.Singleton
                                                                && y.ServiceType == serviceType));
}
