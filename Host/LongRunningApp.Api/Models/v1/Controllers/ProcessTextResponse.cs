using LongRunningApp.Api.Models.Versionless;

namespace LongRunningApp.Api.Models.v1.Controllers;
public sealed record ProcessTextResponse : ResponseBase
{
    public static ProcessTextResponse Empty => new() { ProcessId = Guid.Empty };
    public static ProcessTextResponse EmptyWithErrorMessage(string errorMessage) => new() { ProcessId = Guid.Empty, ErrorMessage = errorMessage };
    public required Guid ProcessId { get; set; }
}
