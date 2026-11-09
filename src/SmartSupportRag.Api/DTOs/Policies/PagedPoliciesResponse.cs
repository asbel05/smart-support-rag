namespace SmartSupportRag.Api.DTOs.Policies;

public class PagedPoliciesResponse
{
    public IReadOnlyList<PolicySummaryResponse> Items { get; set; } = new List<PolicySummaryResponse>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}

