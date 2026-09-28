# Topic 2: Embeddings and vector similarity

Category: Retrieval-Augmented Generation (RAG)
Status: Planned (not built yet, will live in `src/FieldGuide.Core/Embeddings/`)

## What it is

An embedding is a list of numbers (a vector) that represents the *meaning* of a piece of text.
Two chunks of text about similar things end up as two vectors that point in a similar
direction. **Cosine similarity** measures how similar two vectors' directions are, a number
from -1 to 1, where 1 means "point the same way" (very similar meaning), 0 means unrelated,
and -1 means opposite.

## How it's used in practice

1. Every chunk of a document gets embedded once, up front, and stored.
2. At question time, the question itself gets embedded the same way.
3. Compare the question's vector against every stored chunk's vector with cosine similarity.
4. Take the top few matches, those are the chunks most likely to answer the question.

```csharp
// The shape of it, verified against Microsoft.Extensions.AI's IEmbeddingGenerator interface
var chunkEmbeddings = await embedder.GenerateAsync(myChunks);
var questionEmbedding = (await embedder.GenerateAsync([question]))[0];
var topChunks = chunkEmbeddings
    .Select((e, i) => (Chunk: myChunks[i], Score: CosineSimilarity(e.Vector, questionEmbedding.Vector)))
    .OrderByDescending(x => x.Score)
    .Take(3);
```

## Best use cases, and when to use it

| Use case | Fit |
|---|---|
| Semantic search ("find text about X" even if it doesn't use the word X) | Exactly what embeddings are for |
| Exact keyword or code-identifier search | A plain text/full-text search is often better; consider combining both (hybrid search) |
| Small chunk counts (a few hundred) | Brute-force cosine similarity in a C# loop, no infrastructure needed |
| Large chunk counts (thousands+) | A dedicated index, SQL Server 2025's native `VECTOR` type with DiskANN indexing, generally available since November 2025 (Microsoft's own Azure SQL Dev Corner blog, confirmed 2026-09-28), is the planned target here since it's already installed locally |

## Where it's implemented here

Not yet. Planned: `nomic-embed-text` via Ollama (already pulled locally, 274 MB, confirmed via
`ollama.com/library/nomic-embed-text`), exposed through `OllamaSharp`'s
`IEmbeddingGenerator<string, Embedding<float>>` implementation, the same pattern as
[`OllamaChatClientFactory.cs`](../../src/FieldGuide.Core/Chat/OllamaChatClientFactory.cs) uses
for chat.

## Gotchas actually hit

None yet, nothing built.

## Exercise

Not applicable until built.
