using SmartSupportRag.Api.Entities;

namespace SmartSupportRag.Api.DTOs.Support;

public class SupportSourceDto
{
    public Guid ChunkId { get; set; }
    public Guid DocumentId { get; set; }
    public string DocumentTitle { get; set; } = string.Empty;
    public PolicyCategory Category { get; set; }
    public int ChunkIndex { get; set; }
    public string Content { get; set; } = string.Empty;
    public double SimilarityScore { get; set; }
}

