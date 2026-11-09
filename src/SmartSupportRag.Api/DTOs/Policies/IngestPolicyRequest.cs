using System.ComponentModel.DataAnnotations;
using SmartSupportRag.Api.Entities;

namespace SmartSupportRag.Api.DTOs.Policies;

public record IngestPolicyRequest(
    [Required] string Title,
    [Required] PolicyCategory Category,
    [Required] string Content);

