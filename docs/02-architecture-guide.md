# Architecture guide: when to use what

## The decision table

| Situation | Best fit | Why |
|---|---|---|
| Data is small, static, fits in a few pages | Paste it directly into the prompt | Simplest thing that works; no retrieval infrastructure needed |
| Data is large, private, or updates often | Retrieval-Augmented Generation (RAG) | Only the relevant pieces get pulled in per question, so it scales |
| You need a consistent tone, format, or behaviour | Fine-tuning | Changes *how* the model talks; RAG only changes *what facts* it has |
| The question is general knowledge the model already knows well | Plain prompting | Fastest, cheapest, no extra moving parts |
| The model needs to take an action, not just answer | Tool/function calling | Lets code run on the model's behalf (look something up, send something, calculate something) |
| Multiple steps needed, each depending on the last | An agent (Microsoft Agent Framework) | Plans and re-plans across steps; a single RAG call is one-shot |

## The RAG pipeline, plain words

1. **Load** your documents.
2. **Split** them into chunks (see `docs/topics/chunking-strategies.md`).
3. **Embed** each chunk into a vector.
4. **Store** the vectors (in memory, SQLite, or a dedicated vector store).
5. **Retrieve** the closest chunks to a question via cosine similarity.
6. **Augment** the prompt with those chunks.
7. **Generate** the answer, ideally citing which chunk it used.

## Where each project piece fits

```
FieldGuide.Core/
├── Chat/          -> Topic: IChatClient basics, tool calling
├── Embeddings/     -> Topic: embeddings & vector similarity
├── Chunking/       -> Topic: chunking strategies
├── Rag/            -> Topic: the end-to-end RAG pipeline (uses Chat + Embeddings + Chunking)
├── Mcp/            -> Topic: Model Context Protocol from the .NET side
└── Agents/         -> Topic: Microsoft Agent Framework (uses Chat + Rag as tools)
```

`FieldGuide.Web` is a thin layer on top: it renders the `docs/` Markdown and calls straight
into `FieldGuide.Core` for the live "Try it" demos. All the real logic lives in `Core` so it
stays testable without a browser.

## Security note carried over from RAG design

Retrieved content is untrusted input. If a document your pipeline ingests (a public web page,
a user upload) contains text trying to instruct the model, that text must never be allowed to
override the system prompt's rules. This matters more once ingestion is automated (see
`docs/topics/security-prompt-injection.md`, planned).
