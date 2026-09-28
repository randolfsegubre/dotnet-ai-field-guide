using System.Reflection;
using FieldGuide.Core.Chat.Tools;
using Microsoft.Extensions.AI;

namespace FieldGuide.Core.Chat;

/// <summary>
/// Topic 1 end to end: a chat call with one real tool the model can invoke.
/// This is the functional implementation the Web "Try it" page and the xUnit tests
/// both call into, so there is exactly one place the logic lives.
/// </summary>
public sealed class ChatDemoService
{
    private readonly IChatClient _client;

    public ChatDemoService(IChatClient client) => _client = client;

    /// <summary>
    /// Sends one user message to the model with the date/time tool registered, and returns
    /// the final answer plus which tool calls (if any) actually fired.
    /// </summary>
    public async Task<ChatDemoResult> AskAsync(string userMessage, CancellationToken cancellationToken = default)
    {
        // STEP 1 of 3: describe the one tool the model is allowed to use here.
        var chatOptions = new ChatOptions
        {
            Tools = [AIFunctionFactory.Create(
                (Func<string, string>)DateTimeTools.GetCurrentDateTime,
                name: "get_current_date_time",
                description: "Gets the current date and time in a specific IANA time zone, for example 'Asia/Manila' or 'UTC'.")]
        };

        // STEP 2 of 3: a short system prompt keeps the model from inventing a date on its own.
        List<ChatMessage> history =
        [
            new(ChatRole.System,
                "You are a precise assistant. If the user asks about the current date or time, " +
                "always call the get_current_date_time tool instead of guessing. Never state a date " +
                "or time you did not get from the tool."),
            new(ChatRole.User, userMessage)
        ];

        // STEP 3 of 3: UseFunctionInvocation (wired in OllamaChatClientFactory) makes this one
        // call run the whole loop: model asks for the tool, the library runs it, the result goes
        // back to the model, and we get the final answer here.
        ChatResponse response = await _client.GetResponseAsync(history, chatOptions, cancellationToken);

        var toolCallNames = response.Messages
            .SelectMany(m => m.Contents)
            .OfType<FunctionCallContent>()
            .Select(c => c.Name)
            .ToList();

        return new ChatDemoResult(response.Text, toolCallNames);
    }
}

/// <param name="Answer">The model's final natural-language answer.</param>
/// <param name="ToolCallsInvoked">Names of any tools the model actually called to produce that answer.</param>
public sealed record ChatDemoResult(string Answer, IReadOnlyList<string> ToolCallsInvoked);
