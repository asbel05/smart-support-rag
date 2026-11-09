using SmartSupportRag.Api.Entities;

namespace SmartSupportRag.Api.DTOs.Policies;

public class SearchPoliciesResponse
{
    public string Query { get; set; } = string.Empty;
    public PolicyCategory? CategoryFilter { get; set; }
    public int TotalMatches { get; set; }
    public IReadOnlyList<PolicyChunkMatchResponse> Matches { get; set; } = new List<PolicyChunkMatchResponse>();
}
