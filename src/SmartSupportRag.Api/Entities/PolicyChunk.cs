using Pgvector;

namespace SmartSupportRag.Api.Entities;

public class PolicyChunk
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid DocumentId { get; set; }
    public PolicyDocument? Document { get; set; }
    public PolicyCategory Category { get; set; }
    public int ChunkIndex { get; set; }
    public string Content { get; set; } = string.Empty;
    public Vector? Embedding { get; set; }
}

