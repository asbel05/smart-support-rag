using SmartSupportRag.Api.Entities;

namespace SmartSupportRag.Api.DTOs.Support;

public class AskSupportResponse
{
    public string Question { get; set; } = string.Empty;
    public PolicyCategory? CategoryFilter { get; set; }
    public string Answer { get; set; } = string.Empty;
    public IReadOnlyList<SupportSourceDto> Sources { get; set; } = new List<SupportSourceDto>();
}

