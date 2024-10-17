using LongRunningApp.Infrastructure.Models;
using LongRunningApp.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LongRunningApp.Infrastructure;
public static class SetupInfrastructure
{
    public static IHostApplicationBuilder ConfigureInfrastructureLayer(this IHostApplicationBuilder builder)
    {
        AddInfrastructureLayerConfiguration(builder);

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetSection("InfrastructureLayerSettings:RedisConnectionString").Value;
        });

        builder.Services.AddSingleton<ICacheService, CacheService>();

        return builder;
    }

    private static void AddInfrastructureLayerConfiguration(IHostApplicationBuilder builder)
    {
        var assemblyDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        try
        {
            builder.Configuration.AddJsonFile($"{assemblyDirectory}//Properties//infra_layer_settings.json", optional: false, reloadOnChange: true);
            builder.Services.Configure<InfrastructureLayerSettings>(builder.Configuration.GetSection(nameof(InfrastructureLayerSettings)));
        }
        catch (Exception)
        {
            throw;
        }
    }
}
