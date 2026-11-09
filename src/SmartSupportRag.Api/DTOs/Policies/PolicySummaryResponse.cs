using SmartSupportRag.Api.Entities;

namespace SmartSupportRag.Api.DTOs.Policies;

public class PolicySummaryResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public PolicyCategory Category { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int ChunksCount { get; set; }
    public IReadOnlyList<PolicyChunkItemResponse> Chunks { get; set; } = new List<PolicyChunkItemResponse>();
}
