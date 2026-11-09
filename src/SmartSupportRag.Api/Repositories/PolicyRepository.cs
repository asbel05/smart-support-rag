using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using SmartSupportRag.Api.Data;
using SmartSupportRag.Api.Entities;

namespace SmartSupportRag.Api.Repositories;

public class PolicyRepository : IPolicyRepository
{
    private readonly AppDbContext _dbContext;

    public PolicyRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<string>> GetExistingTitlesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.PolicyDocuments
            .AsNoTracking()
            .Select(d => d.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task AddDocumentAsync(PolicyDocument document, CancellationToken cancellationToken = default)
    {
        _dbContext.PolicyDocuments.Add(document);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddDocumentsRangeAsync(IEnumerable<PolicyDocument> documents, CancellationToken cancellationToken = default)
    {
        _dbContext.PolicyDocuments.AddRange(documents);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<PolicyDocumentSummaryResult> Items, int TotalCount)> GetPagedDocumentsAsync(
        PolicyCategory? category,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.PolicyDocuments.AsNoTracking();

        if (category.HasValue)
        {
            query = query.Where(d => d.Category == category.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(d => d.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new PolicyDocumentSummaryResult
            {
                Id = d.Id,
                Title = d.Title,
                Category = d.Category,
                CreatedAt = d.CreatedAt,
                ChunksCount = d.Chunks.Count,
                Chunks = d.Chunks
                    .OrderBy(c => c.ChunkIndex)
                    .Select(c => new PolicyChunkSummaryResult
                    {
                        Id = c.Id,
                        ChunkIndex = c.ChunkIndex,
                        Content = c.Content
                    })
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<PolicyChunkSearchResult>> SearchSimilarChunksAsync(
        Vector queryVector,
        PolicyCategory? category,
        int topK,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.PolicyChunks
            .AsNoTracking()
            .Where(c => c.Embedding != null);

        if (category.HasValue)
        {
            query = query.Where(c => c.Category == category.Value);
        }

        return await query
            .OrderBy(c => c.Embedding!.CosineDistance(queryVector))
            .Take(topK)
            .Select(c => new PolicyChunkSearchResult
            {
                Chunk = c,
                DocumentTitle = c.Document != null ? c.Document.Title : string.Empty,
                CosineDistance = c.Embedding!.CosineDistance(queryVector)
            })
            .ToListAsync(cancellationToken);
    }
}
