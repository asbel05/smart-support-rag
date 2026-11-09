namespace SmartSupportRag.Api.DTOs.Policies;

public record SeedPoliciesResponse(
    int DocumentsCreated,
    int TotalChunksCreated,
    string Message);

