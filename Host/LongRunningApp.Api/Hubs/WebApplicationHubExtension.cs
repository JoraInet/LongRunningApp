using LongRunningApp.Api.Hubs.v1;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;

namespace LongRunningApp.Api.Hubs;
public static class WebApplicationHubExtension
{
    private static string HubsV1 = @"/hubs/v1/";

    public static WebApplication MapApplicationHubs(this WebApplication webApplication)
    {
        //v1
        webApplication.MapHub<v1.TextProcessorHub>($"{HubsV1}{HubNames.TextProcessorHub}");

        return webApplication;
    }

}
