using Asp.Versioning;
using LongRunningApp.Api.Hubs.v1;
using LongRunningApp.Api.Models.v1;
using LongRunningApp.Api.Services;
using LongRunningApp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LongRunningApp.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ControllerName(ApiEndpointNames.Controllers.TextProcessor)]
public class TextProcessorController : ControllerBase
{
    private readonly ILogger<TextProcessorController> _logger;
    private readonly ITextProcessorService _textProcessorService;
    public TextProcessorController(
        ILogger<TextProcessorController> logger,
        ITextProcessorService textProcessorService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _textProcessorService = textProcessorService ?? throw new ArgumentNullException(nameof(textProcessorService));
    }

    [HttpPost]
    [ActionName(ApiEndpointNames.ControllersActions.ProcessText)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProcessTextResponse>> ProcessText([FromBody] ProcessTextRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.ConnectionId))
        {
            return BadRequest(new ProcessTextResponse() { ErrorMessage = Resource.ConectionIdForProcessingIsEmpty });
        }

        if (string.IsNullOrEmpty(request.Text))
        {
            return BadRequest(new ProcessTextResponse() { ErrorMessage = Resource.TextForProcessingIsEmpty });
        }

        try
        {
            await _textProcessorService.PerformProcessing(request.ConnectionId, request.Text, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Internal error while processing text:{request.Text}");
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ProcessTextResponse() { ErrorMessage = Resource.ProcessingTextError });
        }

        return Ok();
    }
}
