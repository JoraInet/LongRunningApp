using LongRunningApp.Api.Models.Versionless;
using System.Net;
using System.Text.Json;

namespace LongRunningApp.Api.Middleware;
public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var errorId = Guid.NewGuid();
            logger.LogError(ex, $"Id:{errorId}. Error while processing request:[{JsonSerializer.Serialize(context.Request)}]");
            await HandleExceptionResponseAsync(context, errorId);
        }
    }

    private static async Task HandleExceptionResponseAsync(HttpContext context, Guid errorId)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var response = new ResponseBase() 
        { 
            ErrorMessage = string.Format(Resource.UnexpectedServerErrorWithId, errorId) 
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        return;
    }
}
