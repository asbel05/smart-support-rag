using System.ComponentModel.DataAnnotations;
using SmartSupportRag.Api.Entities;

namespace SmartSupportRag.Api.DTOs.Policies;

public class IngestPolicyRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public PolicyCategory Category { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;
}
