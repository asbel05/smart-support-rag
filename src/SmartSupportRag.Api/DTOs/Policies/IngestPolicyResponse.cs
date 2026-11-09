using SmartSupportRag.Api.Entities;

namespace SmartSupportRag.Api.DTOs.Policies;

public class IngestPolicyResponse
{
    public Guid DocumentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public PolicyCategory Category { get; set; }
    public int ChunksCreated { get; set; }
}
