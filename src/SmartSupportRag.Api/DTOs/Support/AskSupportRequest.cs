using System.ComponentModel.DataAnnotations;
using SmartSupportRag.Api.Entities;

namespace SmartSupportRag.Api.DTOs.Support;

public class AskSupportRequest
{
    [Required]
    public string Question { get; set; } = string.Empty;

    public PolicyCategory? Category { get; set; }

    public int TopK { get; set; } = 3;
}

