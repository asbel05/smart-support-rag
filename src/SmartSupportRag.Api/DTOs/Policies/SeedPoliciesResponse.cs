namespace SmartSupportRag.Api.DTOs.Policies;

public class SeedPoliciesResponse
{
    public int DocumentsCreated { get; set; }
    public int TotalChunksCreated { get; set; }
    public string Message { get; set; } = string.Empty;
}
