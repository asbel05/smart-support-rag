using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SmartSupportRag.Api.DTOs.Policies;
using SmartSupportRag.Api.Entities;
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

    [HttpGet]
    public async Task<ActionResult<PagedPoliciesResponse>> GetAll(
        [FromQuery] PolicyCategory? category = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 5,
        CancellationToken cancellationToken = default)
    {
        var response = await _policyService.GetPagedPoliciesAsync(
            category,
            pageNumber,
            pageSize,
            cancellationToken);

        return Ok(response);
    }

    [HttpPost("ingest")]
    [EnableRateLimiting("PerIpPolicy")]
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

    [HttpPost("search")]
    [EnableRateLimiting("PerIpPolicy")]
    public async Task<ActionResult<SearchPoliciesResponse>> Search(
        [FromBody] SearchPoliciesRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
        {
            return BadRequest("La consulta de búsqueda no puede estar vacía.");
        }

        var response = await _policyService.SearchAsync(request, cancellationToken);
        return Ok(response);
    }
}
