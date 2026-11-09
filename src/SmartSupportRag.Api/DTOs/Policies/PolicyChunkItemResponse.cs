namespace SmartSupportRag.Api.DTOs.Policies;

public class PolicyChunkItemResponse
{
    public Guid Id { get; set; }
    public int ChunkIndex { get; set; }
    public string Content { get; set; } = string.Empty;
}

