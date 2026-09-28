# Topic 1: IChatClient basics and tool calling

Category: Core Chat & Tools
Status: Built and tested against a real local model

## What it is

`IChatClient` is `Microsoft.Extensions.AI`'s universal interface for "something you can send
chat messages to and get a response from." Think of it as a universal phone jack: whether the
call goes to Ollama on your own machine, OpenAI, or Azure OpenAI, your code dials the same way.

Tool calling (also called function calling) lets the model, instead of only writing text, ask
your code to run a specific .NET method and hand back the result, so it can act on information
it doesn't have on its own, like today's real date.

## How it's used in practice

1. Build an `IChatClient` pointed at a model.
2. Describe one or more .NET methods as "tools" the model is allowed to call.
3. Send a message. If the model decides it needs a tool, `UseFunctionInvocation()` runs the
   .NET method automatically and feeds the result back to the model.
4. The model produces its final answer using that result.

No manual "check if the model asked for a tool, run it, send the result back" loop is written
by hand, the library does it.

## Best use cases, and when to use it

| Use tool calling when | Don't, when |
|---|---|
| The model needs live or private data it can't know (current time, a database lookup, an order status) | The answer is general knowledge the model already has |
| You want the model to trigger an action (book a meeting, send a message) | A human should always be in the loop before the action happens, add a confirmation step first |
| The task has a small, fixed set of well-described operations | The task needs many chained steps depending on each other's results, that's an **agent**, see Microsoft Agent Framework (planned) |

This is the same idea LiveChat AI calls "AI Actions" (booking a meeting via Calendly, checking
an order status), seen while researching how Inghams' live chat widget works.

## Where it's implemented here

| Step | File | What it does |
|---|---|---|
| 1 | [`OllamaChatClientFactory.cs`](../../src/FieldGuide.Core/Chat/OllamaChatClientFactory.cs) | Builds the `IChatClient` from a local Ollama model |
| 2 | [`DateTimeTools.cs`](../../src/FieldGuide.Core/Chat/Tools/DateTimeTools.cs) | The one tool the model is allowed to call |
| 3 | [`ChatDemoService.cs`](../../src/FieldGuide.Core/Chat/ChatDemoService.cs) | Wires the client, the tool, and a system prompt together |
| 4 | [`ChatDemoController.cs`](../../src/FieldGuide.Api/Controllers/ChatDemoController.cs) | Exposes it over HTTP and saves the exchange to SQL Server |
| 5 | [`App.tsx`](../../src/FieldGuide.Web/src/App.tsx) | The React "Try it" panel |

Verified working end to end on 2026-09-28: a real question through the React-facing API
returned `"The current time in Asia/Manila is 2026-09-28 13:02."` with
`toolCallsInvoked: ["get_current_date_time"]`, and the exchange was confirmed persisted in
SQL Server via `GET /api/chat/history`.

Use [`docs/index.html`](../index.html) to jump straight to any of these at the exact line.

## Gotchas actually hit

**A 26 billion parameter local model can take well over 100 seconds to answer on a first (cold)
call.** The default `HttpClient` timeout is 100 seconds, so the first test run of this topic
failed with a `TaskCanceledException`, not because the code was wrong, but because the model
was still loading. Fixed by:

- Passing an explicit `HttpClient` with a 5-minute timeout into `OllamaApiClient`
  (see `OllamaChatClientFactory.Create`).
- Using `llama3.1:8b` (much smaller, verified by Ollama's own model page to support tool use)
  as the default for interactive demos, keeping `gemma4:26b` available for slower, heavier use.

**Lesson**: local-model latency is not the same problem as cloud-API latency. A "the timeout
must mean my code is broken" instinct is often wrong here; check whether the model is still
loading before debugging the code.

## How to run it yourself

```bash
ollama list                                    # confirm the model is pulled
dotnet run --project src/FieldGuide.Api        # terminal 1
cd src/FieldGuide.Web && npm run dev           # terminal 2
```

## Exercise

Add a second tool: `GetRandomTravelDestination()` that returns a hardcoded list of a few
places. Ask the model "suggest me a place to travel" without mentioning the tool by name, and
see whether it decides to call it.
