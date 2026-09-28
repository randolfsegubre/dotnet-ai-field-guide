# .NET AI Field Guide

A personal, working reference for .NET AI Engineering: real running code, tests that hit a
real local model, and documentation written to be understood again months later, not just
once while building it.

This is not a tutorial repo you read once. It's built to be revisited: when you're stuck on a
real problem later ("how did I wire up tool calling again?"), open [`docs/index.html`](docs/index.html)
and jump straight to the exact function, at the exact line.

## What's actually in here right now

Topics are grouped by category in [`docs/index.html`](docs/index.html), so you can go straight
to, say, "the RAG section" and get what it is, when to use it, and exactly where it lives here.

| Category | Topic | Status |
|---|---|---|
| Core Chat & Tools | 1. `IChatClient` basics and tool calling | ✅ Built, tested against a real local Ollama model |
| Retrieval-Augmented Generation (RAG) | 2. Embeddings and vector similarity | Planned |
| Retrieval-Augmented Generation (RAG) | 3. Chunking strategies | Planned |
| Retrieval-Augmented Generation (RAG) | 4. End-to-end RAG pipeline | Planned |
| Agentic & Protocols | 5. Model Context Protocol (MCP) from the .NET side | Planned |
| Agentic & Protocols | 6. Microsoft Agent Framework | Planned |
| Security & Identity | 7. OpenID Connect (OIDC) and OAuth 2.0 primer | Planned (concept only) |

Check [`docs/index.html`](docs/index.html) for the current, authoritative list; this table is
a summary and can drift. Every topic doc under `docs/topics/` follows the same fixed shape:
what it is, how it's used in practice, best use cases, where it's implemented here, gotchas
actually hit, and an exercise.

## Stack

| Piece | Technology | Why |
|---|---|---|
| AI abstraction | `Microsoft.Extensions.AI` (`IChatClient`) | Swap Ollama for OpenAI/Azure OpenAI later with a one-line change |
| Local model runtime | [Ollama](https://ollama.com) | Free, runs on your own machine, no API key or cloud cost |
| .NET ↔ Ollama bridge | `OllamaSharp` | Implements `IChatClient` and the embedding generator directly |
| Backend | ASP.NET Core Web API (.NET 10) | Same stack most real .NET jobs ask for |
| Database | SQL Server (EF Core) | Real persistence, not an in-memory toy; SQL Server 2025's native `VECTOR` type is the planned home for the RAG topic |
| Frontend | React + TypeScript (Vite) | Full-stack, not just a console app |
| Tests | xUnit | Tests here call the real local model on purpose, no mocks, to prove the whole pipeline actually works |

Nothing in this stack costs money. Ollama and every model used here run locally.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js](https://nodejs.org) 20+ (built and tested against Node 24)
- [Ollama](https://ollama.com), installed and running (`ollama serve`, or it runs automatically after install)
- SQL Server (any edition, including a free local instance) reachable with Windows Authentication or a connection string you control
- Pull the models this repo uses:
  ```bash
  ollama pull llama3.1:8b        # fast, used as the interactive default
  ollama pull nomic-embed-text   # small, for the upcoming embeddings topic
  ```
  Any tool-capable model works; check with `ollama show <model>` and look for `tools` under
  Capabilities before relying on one.

## Running it

```bash
# 1. Restore and build the whole solution
dotnet build

# 2. Point the API at your own SQL Server instance
#    Edit src/FieldGuide.Api/appsettings.json -> ConnectionStrings:FieldGuide
#    Default assumes a local default instance with Windows Authentication.

# 3. Create the database (first time only)
dotnet tool install --global dotnet-ef   # if you don't have it already
dotnet ef database update --project src/FieldGuide.Data --startup-project src/FieldGuide.Api

# 4. Run the API
dotnet run --project src/FieldGuide.Api
# Note the HTTPS port it prints (see src/FieldGuide.Api/Properties/launchSettings.json);
# update src/FieldGuide.Web/src/api/chatDemo.ts if it differs from 7185.

# 5. In a second terminal, run the frontend
cd src/FieldGuide.Web
npm install
npm run dev
```

Open the URL Vite prints (usually `http://localhost:5173`).

## Running the tests

```bash
dotnet test tests/FieldGuide.Core.Tests
```

These tests call a real local model, not a mock. A cold first call can take over a minute
while the model loads into memory; that's expected, not a bug (see
[`docs/topics/ichatclient-basics.md`](docs/topics/ichatclient-basics.md) for the exact timeout
issue this surfaced and how it was fixed).

## How this repo is organized

```
docs/                     — the actual explanations: glossary, cheatsheet, architecture guide,
                             one file per topic, and index.html (the navigator)
src/FieldGuide.Core/      — the real logic, one folder per topic, framework-agnostic
src/FieldGuide.Api/       — ASP.NET Core Web API exposing Core over HTTP, persists to SQL Server
src/FieldGuide.Data/      — EF Core DbContext and entities
src/FieldGuide.Web/       — React + TypeScript frontend
tests/FieldGuide.Core.Tests/ — xUnit tests, several run against a real local model on purpose
```

See [`docs/02-architecture-guide.md`](docs/02-architecture-guide.md) for the reasoning behind
this split and a decision table for RAG vs. fine-tuning vs. plain prompting.

## Using this repo to refresh your memory later

1. Open [`docs/index.html`](docs/index.html).
2. Find the topic that matches the problem you're stuck on.
3. Click through to the doc for the plain-language explanation and the gotcha that was
   actually hit while building it.
4. Click a symbol's "Open in VS Code" link (or "View on GitHub" once this repo is pushed) to
   land on the exact working code, not a simplified snippet.

The first time you open `docs/index.html` on a new machine or after moving this repo, edit the
two constants at the top of its `<script>` block (`LOCAL_REPO_ROOT` and `GITHUB_REPO_BASE`);
every link on the page is generated from those two values.

## Honest scope note

This repo exists to build real, demonstrable evidence for .NET AI engineering skills, not to
claim experience that doesn't exist yet. Each topic is marked "planned" until it has working,
tested code behind it. See the individual topic docs for what's proven versus what's still ahead.
