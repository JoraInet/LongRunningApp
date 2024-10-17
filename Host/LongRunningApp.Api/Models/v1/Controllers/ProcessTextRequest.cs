using LongRunningApp.Api.Models.Versionless;

namespace LongRunningApp.Api.Models.v1.Controllers;

public sealed record ProcessTextRequest : RequestBase
{
    public string Text { get; set; }
}
