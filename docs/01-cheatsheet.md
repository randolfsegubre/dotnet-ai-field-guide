# Cheatsheet

Quick syntax reference. Grows as each topic is built. Every snippet here is copied from code
that actually compiled and ran in this repo, not written fresh for this page.

## Build an IChatClient (local Ollama)

```csharp
var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:11434"), Timeout = TimeSpan.FromMinutes(5) };
var ollama = new OllamaApiClient(httpClient, "llama3.1:8b");
IChatClient client = new ChatClientBuilder(ollama).UseFunctionInvocation().Build();
```

Source: [`OllamaChatClientFactory.cs`](topics/../../src/FieldGuide.Core/Chat/OllamaChatClientFactory.cs)

## Register a tool and call it

```csharp
var chatOptions = new ChatOptions
{
    Tools = [AIFunctionFactory.Create(myMethod, name: "my_tool", description: "What it does, plainly.")]
};

List<ChatMessage> history = [new(ChatRole.System, "..."), new(ChatRole.User, userMessage)];
ChatResponse response = await client.GetResponseAsync(history, chatOptions);
```

Verified against Microsoft's own quickstart:
learn.microsoft.com/dotnet/ai/quickstarts/use-function-calling (fetched 2026-09-28).

## Check which tools actually fired

```csharp
var toolCallNames = response.Messages
    .SelectMany(m => m.Contents)
    .OfType<FunctionCallContent>()
    .Select(c => c.Name);
```

## Check a local model's capabilities before relying on it

```bash
ollama show <model>
```

Look for `tools` under Capabilities. If it's missing, `ChatOptions.Tools` will silently never
get invoked, the model just answers in plain text instead.

## EF Core + SQL Server, minimal registration

```csharp
builder.Services.AddDbContext<FieldGuideDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FieldGuide")));
```

Connection string for a local instance with Windows Authentication:
`Server=localhost;Database=FieldGuide;Trusted_Connection=True;TrustServerCertificate=True;`

_(More entries land here as embeddings, RAG, MCP, and Agent Framework topics are built.)_
