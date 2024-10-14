namespace LongRunningApp.Api.Models.v1;
public class ResponseBase
{
    public string ErrorMessage { get; set; } = string.Empty;
    public string ErrorDetails { get; set; } = string.Empty;
}
