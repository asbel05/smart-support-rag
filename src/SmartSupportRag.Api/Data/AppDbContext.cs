using Microsoft.EntityFrameworkCore;
using SmartSupportRag.Api.Entities;

namespace SmartSupportRag.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<PolicyDocument> PolicyDocuments => Set<PolicyDocument>();
    public DbSet<PolicyChunk> PolicyChunks => Set<PolicyChunk>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("vector");

        modelBuilder.Entity<PolicyDocument>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Category).HasConversion<string>().HasMaxLength(50);
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasMany(e => e.Chunks)
                  .WithOne(c => c.Document)
                  .HasForeignKey(c => c.DocumentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PolicyChunk>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Category).HasConversion<string>().HasMaxLength(50);
            entity.Property(e => e.Content).IsRequired();

            entity.Property(e => e.Embedding)
                  .HasColumnType("vector(1536)");

            entity.HasIndex(e => e.Category)
                  .HasDatabaseName("IX_PolicyChunks_Category");

            entity.HasIndex(e => e.Embedding)
                  .HasMethod("hnsw")
                  .HasOperators("vector_cosine_ops")
                  .HasDatabaseName("IX_PolicyChunks_Embedding_Hnsw");
        });
    }
}

