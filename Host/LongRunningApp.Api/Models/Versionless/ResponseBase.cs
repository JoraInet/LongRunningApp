namespace LongRunningApp.Api.Models.Versionless;
public record ResponseBase
{
    public string ErrorMessage { get; set; }
    public string ErrorDetails { get; set; }
}
