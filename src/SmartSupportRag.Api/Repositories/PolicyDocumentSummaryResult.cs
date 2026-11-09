using SmartSupportRag.Api.Entities;

namespace SmartSupportRag.Api.Repositories;

public class PolicyDocumentSummaryResult
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public PolicyCategory Category { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int ChunksCount { get; set; }
    public IReadOnlyList<PolicyChunkSummaryResult> Chunks { get; set; } = new List<PolicyChunkSummaryResult>();
}
