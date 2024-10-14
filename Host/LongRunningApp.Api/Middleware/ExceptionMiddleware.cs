using LongRunningApp.Api.Models.v1;
using System.Net;
using System.Text.Json;

namespace LongRunningApp.Api.Middleware;
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var errorId = Guid.NewGuid();
            _logger.LogError(ex, $"Id:{errorId}. Error while processing request:[{JsonSerializer.Serialize(context.Request)}]");
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
