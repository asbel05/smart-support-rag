using Pgvector;
using SmartSupportRag.Api.Entities;

namespace SmartSupportRag.Api.Repositories;

public interface IPolicyRepository
{
    Task<IReadOnlyList<string>> GetExistingTitlesAsync(CancellationToken cancellationToken = default);
    Task AddDocumentAsync(PolicyDocument document, CancellationToken cancellationToken = default);
    Task AddDocumentsRangeAsync(IEnumerable<PolicyDocument> documents, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PolicyChunkSearchResult>> SearchSimilarChunksAsync(
        Vector queryVector,PolicyCategory? category, int topK,CancellationToken cancellationToken = default);
}
