using Microsoft.Extensions.AI;
using Pgvector;
using SmartSupportRag.Api.DTOs.Policies;
using SmartSupportRag.Api.Entities;
using SmartSupportRag.Api.Repositories;

namespace SmartSupportRag.Api.Mapper;

public static class PolicyMapper
{
    public static PolicyDocument ToEntity(
        this IngestPolicyRequest request,
        IReadOnlyList<string> textChunks,
        GeneratedEmbeddings<Embedding<float>> embeddings)
    {
        var document = new PolicyDocument
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            Category = request.Category,
            CreatedAt = DateTimeOffset.UtcNow
        };

        for (var i = 0; i < textChunks.Count; i++)
        {
            document.Chunks.Add(new PolicyChunk
            {
                Id = Guid.NewGuid(),
                DocumentId = document.Id,
                Category = request.Category,
                ChunkIndex = i,
                Content = textChunks[i],
                Embedding = new Vector(embeddings[i].Vector)
            });
        }

        return document;
    }

    public static IngestPolicyResponse ToIngestResponse(this PolicyDocument document)
    {
        return new IngestPolicyResponse
        {
            DocumentId = document.Id,
            Title = document.Title,
            Category = document.Category,
            ChunksCreated = document.Chunks.Count
        };
    }

    public static PolicyChunkMatchResponse ToMatchResponse(this PolicyChunkSearchResult result)
    {
        var distance = Math.Round(result.CosineDistance, 4);
        var similarity = Math.Round(1.0 - result.CosineDistance, 4);

        return new PolicyChunkMatchResponse
        {
            ChunkId = result.Chunk.Id,
            DocumentId = result.Chunk.DocumentId,
            DocumentTitle = result.DocumentTitle,
            Category = result.Chunk.Category,
            ChunkIndex = result.Chunk.ChunkIndex,
            Content = result.Chunk.Content,
            CosineDistance = distance,
            SimilarityScore = similarity
        };
    }

    public static SearchPoliciesResponse ToSearchResponse(
        this IReadOnlyList<PolicyChunkSearchResult> results,
        SearchPoliciesRequest request)
    {
        var matches = results
            .Select(r => r.ToMatchResponse())
            .ToList();

        return new SearchPoliciesResponse
        {
            Query = request.Query,
            CategoryFilter = request.Category,
            TotalMatches = matches.Count,
            Matches = matches
        };
    }
}
