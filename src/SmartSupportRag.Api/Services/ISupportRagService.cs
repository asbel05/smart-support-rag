using SmartSupportRag.Api.DTOs.Support;

namespace SmartSupportRag.Api.Services;

public interface ISupportRagService
{
    Task<AskSupportResponse> AskAsync(AskSupportRequest request, CancellationToken cancellationToken = default);
}

