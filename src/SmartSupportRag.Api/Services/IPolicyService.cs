using SmartSupportRag.Api.DTOs.Policies;
using SmartSupportRag.Api.Entities;

namespace SmartSupportRag.Api.Services;

public interface IPolicyService
{
    Task<PagedPoliciesResponse> GetPagedPoliciesAsync(
        PolicyCategory? category,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<IngestPolicyResponse> IngestAsync(IngestPolicyRequest request, CancellationToken cancellationToken = default);
    Task<SeedPoliciesResponse> SeedAsync(CancellationToken cancellationToken = default);
    Task<SearchPoliciesResponse> SearchAsync(SearchPoliciesRequest request, CancellationToken cancellationToken = default);
}
