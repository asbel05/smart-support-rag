using SmartSupportRag.Api.DTOs.Support;
using SmartSupportRag.Api.Repositories;

namespace SmartSupportRag.Api.Mapper;

public static class SupportMapper
{
    public static SupportSourceDto ToSupportSourceDto(this PolicyChunkSearchResult result)
    {
        var similarity = Math.Round(1.0 - result.CosineDistance, 4);

        return new SupportSourceDto
        {
            ChunkId = result.Chunk.Id,
            DocumentId = result.Chunk.DocumentId,
            DocumentTitle = result.DocumentTitle,
            Category = result.Chunk.Category,
            ChunkIndex = result.Chunk.ChunkIndex,
            Content = result.Chunk.Content,
            SimilarityScore = similarity
        };
    }

    public static AskSupportResponse ToAskSupportResponse(
        this IReadOnlyList<PolicyChunkSearchResult> results,
        AskSupportRequest request,
        string answer)
    {
        var sources = results
            .Select(r => r.ToSupportSourceDto())
            .ToList();

        return new AskSupportResponse
        {
            Question = request.Question,
            CategoryFilter = request.Category,
            Answer = answer,
            Sources = sources
        };
    }
}

