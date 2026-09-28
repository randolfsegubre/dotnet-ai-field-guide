using FieldGuide.Core.Chat;
using Xunit;

namespace FieldGuide.Core.Tests;

/// <summary>
/// Topic 1 proof: these tests hit a real local Ollama server. There is no mock chat client
/// here on purpose, this is the "does it actually work end to end" check, not a fast unit test.
/// Requires: `ollama serve` running, and the model below already pulled (`ollama list`).
/// </summary>
public class ChatDemoServiceTests
{
    // Change this if you're using a different local model than gemma4:26b.
    private const string ModelId = "gemma4:26b";

    private static ChatDemoService CreateService()
    {
        var client = OllamaChatClientFactory.Create(ModelId);
        return new ChatDemoService(client);
    }

    [Fact]
    public async Task AskAsync_CurrentTimeQuestion_InvokesDateTimeTool()
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.AskAsync("What time is it right now in Asia/Manila?");

        // Assert: the point of this test isn't the exact wall-clock time (that would be
        // flaky), it's proving the model called the tool instead of inventing an answer.
        Assert.Contains("get_current_date_time", result.ToolCallsInvoked);
        Assert.False(string.IsNullOrWhiteSpace(result.Answer));
    }

    [Fact]
    public async Task AskAsync_UnrelatedQuestion_DoesNotInvokeDateTimeTool()
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.AskAsync("In one word, what is the capital of France?");

        // Assert: a well-behaved tool-calling setup should NOT call a tool it doesn't need.
        Assert.DoesNotContain("get_current_date_time", result.ToolCallsInvoked);
    }
}
