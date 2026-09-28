# Topic 3: Chunking strategies

Category: Retrieval-Augmented Generation (RAG)
Status: Planned (not built yet, will live in `src/FieldGuide.Core/Chunking/`)

## What it is

Splitting a long document into smaller pieces before embedding them. Necessary because most
embedding models have a maximum input length, and because embedding an entire document at once
loses precision, the resulting single vector has to represent everything the document talks
about at once, which blurs the specific answer a question needs.

## How it's used in practice

Bad chunking is the single most common cause of a RAG system giving wrong answers, because the
sentence that actually answers the question can get cut in half across two chunks. The rule of
thumb: **chunk by meaning, not by fixed character count.** Split at headings or paragraphs
first; only fall back to a fixed size (roughly 300-500 tokens) if a section is too long.

## Best use cases, and when to use it

| Situation | Approach |
|---|---|
| Well-structured docs (Markdown, headed sections, this repo's own `docs/topics/*.md`) | Split at headings first |
| Unstructured prose (a long article, a transcript) | Split at paragraph boundaries, fall back to a fixed token window only when a paragraph itself is too long |
| Code files | Split at function/class boundaries, not by line count, so a function's signature and body stay together |

## Where it's implemented here

Not yet. Planned: a chunker that splits this repo's own `docs/topics/*.md` files at headings,
so the field guide can eventually answer questions about itself, a deliberately self-referential
test case once Topic 4 (the RAG pipeline) exists.

## Gotchas actually hit

None yet, nothing built.

## Exercise

Not applicable until built.
