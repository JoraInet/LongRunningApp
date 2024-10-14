using LongRunningApp.Application.Models;
using LongRunningApp.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LongRunningApp.Application;

public static class SetupApplication
{
    public static IHostApplicationBuilder ConfigureApplicationLayer(this IHostApplicationBuilder builder)
    {
        AddAppLayerConfiguration(builder);

        builder.Services.AddSingleton<ITextProcessingService, TextProcessingService>();

        return builder;
    }

    private static void AddAppLayerConfiguration(IHostApplicationBuilder builder)
    {
        var assemblyDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        try
        {
            builder.Configuration.AddJsonFile($"{assemblyDirectory}//Properties//app_layer_settings.json", optional: false, reloadOnChange: true);
            builder.Services.Configure<AppLayerSettings>(builder.Configuration.GetSection(nameof(AppLayerSettings)));
        }
        catch (Exception)
        {
            throw;
        }
    }
}
