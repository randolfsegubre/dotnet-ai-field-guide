using FieldGuide.Core.Chat;
using FieldGuide.Data;
using FieldGuide.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FieldGuide.Api.Controllers;

/// <summary>
/// Backs the Topic 1 "Try it" panel in the React app: send a question, get an answer,
/// see which tool (if any) the model called, and have the whole exchange saved to SQL Server.
/// </summary>
[ApiController]
[Route("api/chat")]
public class ChatDemoController(ChatDemoService chatDemoService, FieldGuideDbContext db, IConfiguration config) : ControllerBase
{
    public record AskRequest(string Message);

    public record AskResponse(string Answer, IReadOnlyList<string> ToolCallsInvoked, int TranscriptId);

    [HttpPost("demo")]
    public async Task<ActionResult<AskResponse>> Ask([FromBody] AskRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest("Message is required.");
        }

        // STEP 1 of 2: run the real Topic 1 pipeline (chat + tool calling).
        var result = await chatDemoService.AskAsync(request.Message, cancellationToken);

        // STEP 2 of 2: persist the round trip, the full-stack proof: React -> API -> Ollama
        // -> SQL Server, not just an in-memory demo that forgets everything on refresh.
        var transcript = new ChatTranscript
        {
            TopicSlug = "ichatclient-basics",
            ModelId = config["Ollama:ChatModelId"] ?? "llama3.1:8b",
            UserMessage = request.Message,
            Answer = result.Answer,
            ToolCallsInvoked = string.Join(",", result.ToolCallsInvoked)
        };
        db.ChatTranscripts.Add(transcript);
        await db.SaveChangesAsync(cancellationToken);

        return Ok(new AskResponse(result.Answer, result.ToolCallsInvoked, transcript.Id));
    }

    [HttpGet("history")]
    public async Task<ActionResult<IEnumerable<ChatTranscript>>> History(CancellationToken cancellationToken)
    {
        var history = await db.ChatTranscripts
            .OrderByDescending(t => t.CreatedAtUtc)
            .Take(20)
            .ToListAsync(cancellationToken);
        return Ok(history);
    }
}
