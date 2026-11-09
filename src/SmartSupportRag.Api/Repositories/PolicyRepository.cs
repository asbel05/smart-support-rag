using Microsoft.EntityFrameworkCore;
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
        return await _dbContext.PolicyDocuments.AsNoTracking().Select(d => d.Title).ToListAsync(cancellationToken);
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
}

