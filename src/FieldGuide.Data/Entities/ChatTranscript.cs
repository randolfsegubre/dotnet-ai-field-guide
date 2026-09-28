namespace FieldGuide.Data.Entities;

/// <summary>
/// One saved run of a topic's "Try it" demo. This is the full-stack proof point: a React
/// page sends a question, the API calls the local model, and the round trip is persisted
/// to SQL Server, the same shape as any real production feature.
/// </summary>
public class ChatTranscript
{
    public int Id { get; set; }

    /// <summary>Which docs/topics page this came from, for example "ichatclient-basics".</summary>
    public required string TopicSlug { get; set; }

    public required string ModelId { get; set; }

    public required string UserMessage { get; set; }

    public required string Answer { get; set; }

    /// <summary>Comma-separated tool names the model actually invoked, empty if none.</summary>
    public string ToolCallsInvoked { get; set; } = string.Empty;

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}
