namespace LongRunningApp.Api.Controllers.v1;
public static class ApiEndpointNames
{
    public static class Controllers
    {
        public const string TextProcessor = "text-processor";
    }

    public static class ControllersActions
    {
        public const string StartProcess = "start-process";
        public const string CancelProcess = "cancel-process";
    }
}
