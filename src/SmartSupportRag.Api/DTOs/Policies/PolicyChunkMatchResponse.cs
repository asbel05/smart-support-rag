using SmartSupportRag.Api.Entities;

namespace SmartSupportRag.Api.DTOs.Policies;

public class PolicyChunkMatchResponse
{
    public Guid ChunkId { get; set; }
    public Guid DocumentId { get; set; }
    public string DocumentTitle { get; set; } = string.Empty;
    public PolicyCategory Category { get; set; }
    public int ChunkIndex { get; set; }
    public string Content { get; set; } = string.Empty;
    public double CosineDistance { get; set; }
    public double SimilarityScore { get; set; }
}
