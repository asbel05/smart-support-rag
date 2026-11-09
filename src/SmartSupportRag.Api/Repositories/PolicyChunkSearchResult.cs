using SmartSupportRag.Api.Entities;

namespace SmartSupportRag.Api.Repositories;

public class PolicyChunkSearchResult
{
    public PolicyChunk Chunk { get; set; } = null!;
    public string DocumentTitle { get; set; } = string.Empty;
    public double CosineDistance { get; set; }
}
