using Microsoft.Extensions.AI;
using Pgvector;
using SmartSupportRag.Api.Data;
using SmartSupportRag.Api.DTOs.Policies;
using SmartSupportRag.Api.Entities;
using SmartSupportRag.Api.Mapper;
using SmartSupportRag.Api.Repositories;

namespace SmartSupportRag.Api.Services;

public class PolicyService : IPolicyService
{
    private static readonly SemaphoreSlim SeedLock = new(1, 1);

    private readonly IPolicyRepository _policyRepository;
    private readonly TextChunker _textChunker;
    private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;

    public PolicyService(
        IPolicyRepository policyRepository,
        TextChunker textChunker,
        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
    {
        _policyRepository = policyRepository;
        _textChunker = textChunker;
        _embeddingGenerator = embeddingGenerator;
    }

    public async Task<PagedPoliciesResponse> GetPagedPoliciesAsync(
        PolicyCategory? category,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = Math.Max(1, pageNumber);
        var normalizedSize = Math.Clamp(pageSize, 1, 50);

        var (items, totalCount) = await _policyRepository.GetPagedDocumentsAsync(
            category,
            normalizedPage,
            normalizedSize,
            cancellationToken);

        return items.ToPagedResponse(normalizedPage, normalizedSize, totalCount);
    }

    public async Task<IngestPolicyResponse> IngestAsync(
        IngestPolicyRequest request,
        CancellationToken cancellationToken = default)
    {
        var document = await BuildDocumentWithEmbeddingsAsync(request, cancellationToken);
        await _policyRepository.AddDocumentAsync(document, cancellationToken);

        return document.ToIngestResponse();
    }

    public async Task<SeedPoliciesResponse> SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedLock.WaitAsync(cancellationToken);
        try
        {
            var initialPolicies = SeedData.GetInitialPolicies();
            var existingTitles = await _policyRepository.GetExistingTitlesAsync(cancellationToken);

            var policiesToInsert = initialPolicies
                .Where(p => !existingTitles.Contains(p.Title))
                .ToList();

            if (policiesToInsert.Count == 0)
            {
                return new SeedPoliciesResponse
                {
                    DocumentsCreated = 0,
                    TotalChunksCreated = 0,
                    Message = "Las políticas base ya se encuentran cargadas en la base de datos."
                };
            }

            var documents = new List<PolicyDocument>(policiesToInsert.Count);
            var totalChunks = 0;

            foreach (var policy in policiesToInsert)
            {
                var document = await BuildDocumentWithEmbeddingsAsync(policy, cancellationToken);
                documents.Add(document);
                totalChunks += document.Chunks.Count;
            }

            await _policyRepository.AddDocumentsRangeAsync(documents, cancellationToken);

            return new SeedPoliciesResponse
            {
                DocumentsCreated = documents.Count,
                TotalChunksCreated = totalChunks,
                Message = "Políticas iniciales cargadas y vectorizadas correctamente."
            };
        }
        finally
        {
            SeedLock.Release();
        }
    }

    public async Task<SearchPoliciesResponse> SearchAsync(
        SearchPoliciesRequest request,
        CancellationToken cancellationToken = default)
    {
        var topK = Math.Clamp(request.TopK, 1, 20);
        var generatedEmbeddings = await _embeddingGenerator.GenerateAsync(
            [request.Query.Trim()],
            cancellationToken: cancellationToken);

        var queryVector = new Vector(generatedEmbeddings[0].Vector);
        var results = await _policyRepository.SearchSimilarChunksAsync(
            queryVector,
            request.Category,
            topK,
            cancellationToken);

        return results.ToSearchResponse(request);
    }

    private async Task<PolicyDocument> BuildDocumentWithEmbeddingsAsync(
        IngestPolicyRequest request,
        CancellationToken cancellationToken)
    {
        var textChunks = _textChunker.ChunkText(request.Content);
        var generatedEmbeddings = await _embeddingGenerator.GenerateAsync(
            textChunks,
            cancellationToken: cancellationToken);

        return request.ToEntity(textChunks, generatedEmbeddings);
    }
}
