using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SmartSupportRag.Api.DTOs.Support;
using SmartSupportRag.Api.Services;

namespace SmartSupportRag.Api.Controllers;

[ApiController]
[Route("api/support")]
[EnableRateLimiting("PerIpPolicy")]
public class SupportController : ControllerBase
{
    private readonly ISupportRagService _supportRagService;

    public SupportController(ISupportRagService supportRagService)
    {
        _supportRagService = supportRagService;
    }

    [HttpPost("ask")]
    public async Task<ActionResult<AskSupportResponse>> Ask(
        [FromBody] AskSupportRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return BadRequest("La pregunta no puede estar vacía.");
        }

        var response = await _supportRagService.AskAsync(request, cancellationToken);
        return Ok(response);
    }
}
