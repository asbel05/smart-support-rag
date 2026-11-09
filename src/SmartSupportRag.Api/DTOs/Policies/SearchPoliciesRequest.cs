using System.ComponentModel.DataAnnotations;
using SmartSupportRag.Api.Entities;

namespace SmartSupportRag.Api.DTOs.Policies;

public class SearchPoliciesRequest
{
    [Required]
    public string Query { get; set; } = string.Empty;

    public PolicyCategory? Category { get; set; }

    public int TopK { get; set; } = 3;
}
