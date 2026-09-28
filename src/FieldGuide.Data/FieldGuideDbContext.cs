using FieldGuide.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FieldGuide.Data;

public class FieldGuideDbContext(DbContextOptions<FieldGuideDbContext> options) : DbContext(options)
{
    public DbSet<ChatTranscript> ChatTranscripts => Set<ChatTranscript>();

    public DbSet<DocumentChunk> DocumentChunks => Set<DocumentChunk>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatTranscript>(e =>
        {
            e.Property(x => x.TopicSlug).HasMaxLength(200);
            e.Property(x => x.ModelId).HasMaxLength(200);
            e.Property(x => x.UserMessage).HasMaxLength(4000);
            e.Property(x => x.Answer).HasMaxLength(4000);
        });

        modelBuilder.Entity<DocumentChunk>(e =>
        {
            e.Property(x => x.SourceTitle).HasMaxLength(400);
        });
    }
}
