using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Testing.Platform.Builder;
using LongRunningApp.Infrastructure.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace LongRunningApp.Infrastructure.UnitTests;

[TestClass]
public sealed class SetupInfrastructureTests
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
    public void ConfigureInfrastructureLayer_WithDefaults_ShouldAddedRequired()
    {
        //Arrange
        var configSectionMock = new Mock<IConfigurationSection>();

        _configurationManagerMock.Setup(x => x.GetSection(nameof(InfrastructureLayerSettings))).Returns(configSectionMock.Object);

        var builderMock = new Mock<IHostApplicationBuilder>();
        builderMock.Setup(x => x.Configuration).Returns(_configurationManagerMock.Object);
        builderMock.Setup(x => x.Services).Returns(_serviceCollectionMock.Object);

        //Act
        builderMock.Object.ConfigureInfrastructureLayer();

        //Assert
        Assert.IsTrue(IsServiceAdded(typeof(IDistributedCache), ServiceLifetime.Singleton));
        Assert.IsTrue(IsConfigureOptionsAdded(typeof(IConfigureOptions<InfrastructureLayerSettings>)));
    }

    private bool IsServiceAdded(Type serviceType, ServiceLifetime lifetime)
    => _serviceCollectionMock.Invocations.Any(x => x.Method.Name == nameof(IServiceCollection.Add)
                                                    && x.Arguments.OfType<ServiceDescriptor>().Any(
                                                        y => y.Lifetime == lifetime && y.ServiceType == serviceType));

    private bool IsConfigureOptionsAdded(Type serviceType)
        => _serviceCollectionMock.Invocations.Any(x => x.Method.Name == nameof(IServiceCollection.Add)
                                                        && x.Arguments.OfType<ServiceDescriptor>().Any(
                                                            y => y.Lifetime == ServiceLifetime.Singleton
                                                                && y.ServiceType == serviceType));
}
