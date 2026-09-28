namespace FieldGuide.Data.Entities;

/// <summary>
/// Placeholder for the embeddings/RAG topic (not wired up yet, Topic 1 only covers chat
/// and tool calling). The embedding is stored as a JSON array of floats for now.
/// </summary>
/// <remarks>
/// SQL Server 2025 (which is what's installed on this machine) has a native <c>VECTOR</c>
/// data type plus a <c>VECTOR_SEARCH</c> function and DiskANN indexing, generally available
/// since the November 2025 RTM (Microsoft's Azure SQL Dev Corner blog, confirmed 2026-09-28).
/// That is the real target for this table once the embeddings topic is built: store the
/// embedding natively and let SQL Server do the similarity search in T-SQL, instead of
/// pulling every row into memory and computing cosine similarity in C#. Entity Framework
/// Core does not yet have first-class mapping for <c>VECTOR</c>, so that step will likely
/// need a raw SQL query or a value converter. Left as a JSON string here until that topic.
/// </remarks>
public class DocumentChunk
{
    public int Id { get; set; }

    public required string SourceTitle { get; set; }

    public required string Content { get; set; }

    /// <summary>JSON array of floats, e.g. "[0.012, -0.44, ...]". Placeholder, see remarks.</summary>
    public string? EmbeddingJson { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}
