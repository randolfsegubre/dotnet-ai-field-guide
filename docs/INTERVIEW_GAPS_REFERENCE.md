# AI Gaps Reference — Codeless Platforms C#/.NET AI Developer

As of 2026-09-28. Six requirements from the posting (Reference ORB-DEV-OS-AI198) that are
currently gaps, in the posting's own order. Each section covers: what it is, how to use it,
when to use it, and an honest status check, so this document works both as study material and
as a check against saying anything untrue in the interview.

**Rule for using this document:** every "Say" line is something true today. If your status on
a gap changes (you build more of the RAG topic, for example), update the status line, don't
just update what you'd say.

---

## Gap 1: 2+ years of generative AI and LLM application development (Essential)

**What it actually measures.** Not "have you used AI tools," but "have you personally written
code that calls a Large Language Model (LLM) and built something with it," sustained over two
years. Daily use of Claude Code or GitHub Copilot as a developer tool doesn't count toward
this, that's using AI, not building AI applications.

**Your honest tally.**

| Kind of experience | Real duration | Counts toward "2 years of LLM application development"? |
|---|---|---|
| Daily AI-assisted coding (Claude Code, GitHub Copilot) | ~4 years | No, this is tool usage, not application development |
| MCP servers configured and used (Jira/git/.NET build, GitHub, Playwright) | Recent, ongoing | Partial, configuring is closer to integration than development |
| ai-tutor: a real .NET chat application (Semantic Kernel, streaming, multi-provider) | Personal project, built once | Real LLM application code, but not sustained over 2 years |
| dotnet-ai-field-guide: IChatClient + tool calling, tested end to end | Built 2026-09-28 | Real, current, but days old, not years |

**How to use / when to use this framing.** Use it when asked directly about years of
experience. Don't reach for it unprompted, since volunteering "I don't have 2 years" before
being asked undersells the real evidence you do have.

**Say:** "Not two years of building LLM applications specifically, no. I've used AI tools
professionally for about four years, and I've built real LLM application code, a chat app in
one personal project, and just this week a tested IChatClient integration with tool calling and
SQL Server persistence in another. What I bring instead is nine years of production C#/.NET
and the ability to pick up an AI stack fast, which I think this week is decent proof of."

**Status: Real gap.** No amount of framing changes the plain fact: it is not 2 years.

---

## Gap 2: Retrieval-Augmented Generation (RAG), vector databases, embeddings (Essential)

**What it is.** An open-book exam for an LLM. Instead of answering from memory alone (which
has a training cutoff and can hallucinate), the system looks up the most relevant pieces of
your own documents at question time and hands them to the model with the question.

**The pipeline (how to use it).**
1. Load documents.
2. Split into chunks (by meaning, not fixed character count, ideally).
3. Turn each chunk into an embedding (a vector representing its meaning).
4. Store the vectors.
5. At question time, embed the question, retrieve the closest chunks via cosine similarity.
6. Put those chunks into the prompt.
7. Generate the answer, ideally with citations and an "I don't know" fallback.

**When to use it vs. the alternatives.**

| Situation | Use |
|---|---|
| Data is large, private, or changes often | RAG |
| Data is small and static | Paste it directly into the prompt |
| Need to change how the model talks/behaves | Fine-tuning, a different problem entirely |
| Just need general knowledge | Plain prompting |

**Say (if asked to explain it):** "I understand the pipeline end to end: chunk, embed, store,
retrieve by cosine similarity, augment the prompt, generate with citations. I haven't shipped a
RAG system in production. I have the architecture planned in a repo I'm building this week, an
IChatClient and tool-calling layer is already working and tested, embeddings are the next piece."

**Status: Real gap, actively being closed.** Not built yet in `dotnet-ai-field-guide` (planned
location: `src/FieldGuide.Core/Embeddings/`, `Chunking/`, `Rag/`). The local embedding model
(`nomic-embed-text`) is already pulled and ready. If this gets built before the interview,
update the "Say" line to name it as done, not planned.

---

## Gap 3: Microsoft.Extensions.AI, IChatClient

**What it is.** `Microsoft.Extensions.AI`'s `IChatClient` is a provider-agnostic interface for
sending chat messages and getting a response, so swapping Ollama for OpenAI or Azure OpenAI
later is a one-line change, not a rewrite.

**How to use it (verified working code, from your own repo).**
```csharp
var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:11434"), Timeout = TimeSpan.FromMinutes(5) };
var ollama = new OllamaApiClient(httpClient, "llama3.1:8b");
IChatClient client = new ChatClientBuilder(ollama).UseFunctionInvocation().Build();

var chatOptions = new ChatOptions { Tools = [AIFunctionFactory.Create(myTool, name: "my_tool", description: "...")] };
ChatResponse response = await client.GetResponseAsync(history, chatOptions);
```

**When to use it.** Any time you're building a .NET app that talks to an LLM and you want to
stay provider-agnostic, or want automatic tool/function-call handling instead of writing that
loop by hand.

**Say:** "I've built and tested this directly. An ASP.NET Core API using IChatClient with
UseFunctionInvocation, one real tool the model can call, a React front end calling it, and the
whole exchange persisted to SQL Server. I verified it end to end on 2026-09-28: asked it the
current time in a specific time zone, it correctly called the tool instead of guessing, and the
answer was right."

**Status: Real, verified, built this week.** This is your strongest, most current, most
honestly demonstrable answer in this whole document. Lean on it. Package versions used:
`Microsoft.Extensions.AI` 10.10.0, `OllamaSharp` 5.4.30 (both confirmed against NuGet
2026-09-25).

---

## Gap 4: Microsoft Agent Framework

**What it is.** `Microsoft.Agents.AI`, Microsoft's .NET library for building agents: programs
that can plan multiple steps, call tools, and use one step's result to decide the next one,
rather than answering in a single pass the way plain tool calling (Gap 3) does. Supports
multi-agent patterns (sequential, concurrent, group chat, handoff) and depends directly on
`Microsoft.Extensions.AI`.

**How to use it / when to use it.** Reach for it when a task needs several dependent steps, or
when multiple specialized agents should hand work to each other. A single tool call is enough
when one lookup answers the question; an agent is for when the model needs to decide, act,
observe, and decide again.

**Say:** "I haven't used the Agent Framework yet. I understand it sits on top of the same
IChatClient abstraction I've already built with, so the step from tool calling to a full agent
is a smaller jump for me than starting from nothing. It's next on my list after the RAG piece."

**Status: Real gap, not started.** Version confirmed on NuGet: `Microsoft.Agents.AI` 1.22.0
(checked 2026-09-18).

---

## Gap 5: OpenAI-compatible APIs

**What it is.** Many providers (Ollama, GitHub Models, Groq, and others) expose an HTTP API
shaped the same way as OpenAI's own Chat Completions API (same request/response JSON shape,
usually at a `/v1` path), so one client library can talk to many providers with only the base
URL and API key changing.

**How to use it.** Point an OpenAI-compatible client at the provider's base URL instead of
OpenAI's. No code change beyond configuration in the well-behaved case.

**When to use it.** When you want to switch providers (cost, latency, capability, or privacy
reasons) without rewriting your integration layer.

**Say:** "In my own project, ai-tutor, I built exactly this: one client pointed at local Ollama
through its OpenAI-compatible /v1 endpoint, and the same code also worked against GitHub
Models, Groq, and Anthropic's compatible endpoint, with API keys stored encrypted per user."

**Status: Real personal-project evidence, genuinely usable.** This is a real, honest win, not
just theory, cite it plainly as "in my own project," not as production experience.

---

## Gap 6: Single Sign-On (SSO), OpenID Connect (OIDC), OAuth 2.0 (Essential)

**What each term means.**
- **OAuth 2.0**: lets an app get limited access to a user's data or actions on another service,
  without ever seeing the user's password.
- **OpenID Connect (OIDC)**: an identity layer built on top of OAuth 2.0, confirms *who* a user
  is (not just what they can do), via a trusted identity provider.
- **Single Sign-On (SSO)**: the user experience outcome, log in once at an identity provider,
  reuse that login across multiple applications. Usually implemented with OIDC underneath.

**How the flow works.** The user logs in at the identity provider, not your app. Your app
receives a code, exchanges it for tokens, and those tokens prove identity (OIDC) and/or
authorization (OAuth 2.0).

**When to use it vs. plain JSON Web Token (JWT) auth.** Use OIDC/SSO when users should log in
via an external identity provider (Microsoft Entra ID, Google, a company's own SSO). Plain JWT,
issued and validated entirely by your own app, is enough when there's no external identity
provider involved at all.

**Say:** "I've built JSON Web Token authentication with token refresh in my own projects. I
haven't implemented OpenID Connect or Single Sign-On in production. I understand the flow, the
user authenticates at the identity provider, the app exchanges a code for tokens, and I'd pick
up the implementation details quickly given the JWT groundwork I already have."

**Status: Real gap, concept understood.** Do not say "OIDC" or "SSO" as something you've
implemented. JWT is the honest, separate claim.

---

## Study priority, given limited time before the interview

1. **Gap 3 (IChatClient)** — already done, just rehearse saying it clearly and specifically.
2. **Gap 2 (RAG/embeddings)** — highest-value gap to close further if you have any more time;
   even a basic working embeddings demo changes your honest answer meaningfully.
3. **Gap 5 (OpenAI-compatible APIs)** — no new work needed, just remember to cite ai-tutor.
4. **Gaps 1, 4, 6** — accept these stay open gaps for this interview; the honest "Say" lines
   above are the whole plan for them, don't try to fake progress you don't have.

This document lives at `docs/INTERVIEW_GAPS_REFERENCE.md` in the `dotnet-ai-field-guide` repo,
next to the actual code each "Say" line refers to.
