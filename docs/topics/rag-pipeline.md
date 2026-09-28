# Topic 4: End-to-end Retrieval-Augmented Generation (RAG) pipeline

Category: Retrieval-Augmented Generation (RAG)
Status: Planned (not built yet, will live in `src/FieldGuide.Core/Rag/`)

## What it is

An open-book exam for a Large Language Model (LLM). Normally an LLM answers from memory alone,
which has three weaknesses: it stops learning at a training cutoff date, it has never seen your
private data, and it can state a wrong answer as confidently as a right one (a hallucination).
RAG fixes this by looking up the most relevant pieces of your own documents at question time,
and handing them to the model along with the question, so it answers from real text instead
of guessing.

It's a different tool from **fine-tuning** (which changes *how* a model talks or behaves, by
retraining it) and from **plain prompting** (fine for small, static, or general-knowledge
questions). RAG changes *what facts* the model has access to, on demand, per question.

## How it's used in practice

1. **Load** your documents.
2. **Split** them into chunks (see [chunking-strategies.md](chunking-strategies.md)).
3. **Embed** each chunk into a vector (see [embeddings-and-similarity.md](embeddings-and-similarity.md)).
4. **Store** the vectors.
5. **Retrieve** the closest chunks to a question via cosine similarity.
6. **Augment** the prompt with those chunks.
7. **Generate** the answer, ideally citing which chunk it used, and saying "I don't know" when
   nothing retrieved actually answers the question.

## Best use cases, and when to use it

| Use RAG when | Use something else when |
|---|---|
| Data is large, private, or updates often (your own project docs, a product catalog, a company knowledge base) | Data is small, static, and fits in a few pages, just paste it into the prompt |
| You need traceable answers ("which document said this?") | You need to change the model's tone or output format consistently, that's fine-tuning |
| The question depends on facts the model can't know (private data, anything after its training cutoff) | The question is general knowledge the model already answers well |

Real-world example seen during research for this repo: LiveChat's "LiveChat AI" product
(used by some travel sites, confirmed not proven enabled on Inghams' own site) ingests a
site's pages, PDFs, and Q&A lists and answers from them, RAG sold as a Software as a Service
(SaaS) feature rather than built in-house.

## Where it's implemented here

Not yet. Planned location: `src/FieldGuide.Core/Rag/`, reusing the `IChatClient` plumbing from
[Topic 1](ichatclient-basics.md) and the embedding generator from Topic 2. The intended storage
layer is SQL Server 2025's native `VECTOR` type and `VECTOR_SEARCH` function (generally
available since the November 2025 RTM, per Microsoft's Azure SQL Dev Corner blog, confirmed
2026-09-28), not a separate vector database, since it's already installed on this machine as
`FieldGuide.Data`'s `DocumentChunk` entity anticipates (see its XML doc remarks).

## Gotchas actually hit

None yet, nothing built. This section gets filled in honestly once the pipeline exists, not
before.

## Exercise

Not applicable until built.
