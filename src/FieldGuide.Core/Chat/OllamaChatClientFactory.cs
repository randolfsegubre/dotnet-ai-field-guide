using Microsoft.Extensions.AI;
using OllamaSharp;

namespace FieldGuide.Core.Chat;

/// <summary>
/// Builds an <see cref="IChatClient"/> backed by a local Ollama model, with automatic
/// tool/function-call invocation switched on.
/// </summary>
/// <remarks>
/// This is the field guide's "front door" for Topic 1 (IChatClient basics and tool calling).
/// <para>
/// The pattern is verified against Microsoft's own quickstart
/// ("Extend OpenAI using functions and execute a local function with .NET",
/// learn.microsoft.com/dotnet/ai/quickstarts/use-function-calling, fetched 2026-09-28):
/// wrap any Microsoft.Extensions.AI-compatible chat client in a <see cref="ChatClientBuilder"/>
/// and call <c>UseFunctionInvocation()</c>. Swapping Ollama for OpenAI or Azure OpenAI later
/// means changing only the inner client, not any calling code.
/// </para>
/// <para>
/// <see cref="OllamaApiClient"/> implements <see cref="IChatClient"/> directly
/// (OllamaSharp docs, github.com/awaescher/OllamaSharp, fetched 2026-09-28), so no adapter
/// is needed the way OpenAI's SDK needs <c>.AsIChatClient()</c>.
/// </para>
/// </remarks>
public static class OllamaChatClientFactory
{
    /// <summary>Default local Ollama server address (Ollama's own default).</summary>
    public const string DefaultBaseUrl = "http://localhost:11434";

    /// <summary>
    /// Creates a chat client that automatically executes any tools the model asks for
    /// and feeds the result back, without the caller having to handle the tool-call loop.
    /// </summary>
    /// <param name="modelId">
    /// An Ollama model tag already pulled locally, for example "gemma4:26b".
    /// Check what you have with <c>ollama list</c>; the model must support the
    /// "tools" capability (check with <c>ollama show &lt;model&gt;</c>) or tool calls
    /// will silently never fire.
    /// </param>
    /// <param name="baseUrl">The local Ollama server address. Defaults to <see cref="DefaultBaseUrl"/>.</param>
    /// <param name="requestTimeout">
    /// How long to wait for a response. Large local models (20B+ parameters) can take well
    /// over a minute on consumer hardware, especially on the first call after the model
    /// loads into memory, so the default here (5 minutes) is deliberately generous rather
    /// than HttpClient's own 100-second default. Measured directly against gemma4:26b on
    /// this machine (2026-09-28): a cold first call did not finish inside 100 seconds.
    /// </param>
    public static IChatClient Create(
        string modelId,
        string baseUrl = DefaultBaseUrl,
        TimeSpan? requestTimeout = null)
    {
        // STEP 1 of 2: the underlying provider client. Built from an explicit HttpClient
        // (rather than the (Uri, model) shortcut constructor) purely to control the timeout.
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl),
            Timeout = requestTimeout ?? TimeSpan.FromMinutes(5)
        };
        var ollama = new OllamaApiClient(httpClient, modelId);

        // STEP 2 of 2: wrap it so [Tools] in ChatOptions get invoked automatically.
        // Without UseFunctionInvocation, the model would only ever *describe* the call it
        // wants to make; your code would have to run it and send the result back manually.
        return new ChatClientBuilder(ollama)
            .UseFunctionInvocation()
            .Build();
    }
}
