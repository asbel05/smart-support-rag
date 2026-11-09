using Microsoft.AspNetCore.Mvc;
using SmartSupportRag.Api.DTOs.Policies;
using SmartSupportRag.Api.Services;

namespace SmartSupportRag.Api.Controllers;

[ApiController]
[Route("api/policies")]
public class PoliciesController : ControllerBase
{
    private readonly IPolicyService _policyService;

    public PoliciesController(IPolicyService policyService)
    {
        _policyService = policyService;
    }

    [HttpPost("ingest")]
    public async Task<ActionResult<IngestPolicyResponse>> Ingest(
        [FromBody] IngestPolicyRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest("El título y el contenido de la política son obligatorios.");
        }

        var response = await _policyService.IngestAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("seed")]
    public async Task<ActionResult<SeedPoliciesResponse>> Seed(CancellationToken cancellationToken)
    {
        var response = await _policyService.SeedAsync(cancellationToken);
        return Ok(response);
    }
}
