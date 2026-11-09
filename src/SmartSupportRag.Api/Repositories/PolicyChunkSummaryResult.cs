namespace SmartSupportRag.Api.Repositories;

public class PolicyChunkSummaryResult
{
    public Guid Id { get; set; }
    public int ChunkIndex { get; set; }
    public string Content { get; set; } = string.Empty;
}

