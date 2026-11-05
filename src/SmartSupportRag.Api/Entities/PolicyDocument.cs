namespace SmartSupportRag.Api.Entities;

public class PolicyDocument
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public PolicyCategory Category { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public ICollection<PolicyChunk> Chunks { get; set; } = new List<PolicyChunk>();
}

