using SmartSupportRag.Api.DTOs.Policies;

namespace SmartSupportRag.Api.Services;

public interface IPolicyService
{
    Task<IngestPolicyResponse> IngestAsync(IngestPolicyRequest request, CancellationToken cancellationToken = default);
    Task<SeedPoliciesResponse> SeedAsync(CancellationToken cancellationToken = default);
    Task<SearchPoliciesResponse> SearchAsync(SearchPoliciesRequest request, CancellationToken cancellationToken = default);
}
