using Asp.Versioning;
using LongRunningApp.Api.Models.v1.Controllers;
using LongRunningApp.Api.Services;
using LongRunningApp.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LongRunningApp.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ControllerName(ApiEndpointNames.Controllers.TextProcessor)]
public class TextProcessorController(
    ILogger<TextProcessorController> logger,
    ITextProcessorService textProcessorService) : ControllerBase
{

    [HttpPost]
    [ActionName(ApiEndpointNames.ControllersActions.StartProcess)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProcessTextResponse>> ProcessText([FromBody] ProcessTextRequest request)
    {
        if (logger.LogErrorIfNullOrWhiteSpace(request.ConnectionId, nameof(request.ConnectionId)))
        {
            return BadRequest(ProcessTextResponse.EmptyWithErrorMessage(Resource.ConectionIdForProcessingIsEmpty));
        }

        if (logger.LogErrorIfNullOrWhiteSpace(request.Text, nameof(request.Text)))
        {
            return BadRequest(ProcessTextResponse.EmptyWithErrorMessage(Resource.TextForProcessingIsEmpty));
        }

        logger.LogTrace("Requested run new text processing task. ConnectionId:[{request.ConnectionId}]; Text:[{request.Text}].",
                        request.ConnectionId, request.Text);

        Guid processId;
        try
        {
            processId = await textProcessorService.StartProcessingTextAsync(request.ConnectionId, request.Text);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Internal error while starting processing text:[{request.Text}]; ConnectionId:[{request.ConnectionId}].");
            return StatusCode(StatusCodes.Status500InternalServerError, ProcessTextResponse.EmptyWithErrorMessage(Resource.StartProcessingTextError));
        }
        logger.LogTrace($"New text processing task received Id:[{processId}]. ConnectionId:[{request.ConnectionId}]; Text:[{request.Text}].");

        return Accepted(new ProcessTextResponse() { ProcessId = processId });
    }

    [HttpDelete]
    [ActionName(ApiEndpointNames.ControllersActions.CancelProcess)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CancelProcessText([FromQuery] Guid processId)
    {
        logger.LogTrace($"Requested cancel process id:[{processId}]");
        try
        {
            await textProcessorService.CancelProcessingTextAsync(processId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Internal error while cancelling processing text. ConnectionId:[{processId}]");
            return StatusCode(StatusCodes.Status500InternalServerError, ProcessTextResponse.EmptyWithErrorMessage(Resource.CancelProcessingTextError));

        }
        logger.LogTrace($"Process with id:[{processId}] has cancelled.");
        return NoContent();
    }

}
