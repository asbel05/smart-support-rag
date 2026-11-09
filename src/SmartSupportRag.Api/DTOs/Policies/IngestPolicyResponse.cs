using SmartSupportRag.Api.Entities;

namespace SmartSupportRag.Api.DTOs.Policies;

public record IngestPolicyResponse(
    Guid DocumentId,
    string Title,
    PolicyCategory Category,
    int ChunksCreated);

