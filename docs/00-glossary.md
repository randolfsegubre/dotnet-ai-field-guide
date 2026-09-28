# Glossary

Every term used in this repository, in Full Word Format (full words first, abbreviation after)
on first appearance in each entry.

- **Large Language Model (LLM)** — a model trained on huge amounts of text to predict and
  generate language. Knows nothing past its training cutoff date and knows nothing about
  your private data unless you give it that data in the prompt.
- **Retrieval-Augmented Generation (RAG)** — look up the most relevant pieces of your own
  documents at question time, and hand them to the LLM along with the question, so it
  answers from real text instead of guessing.
- **Embedding** — a list of numbers (a vector) that represents the meaning of a piece of
  text. Two chunks of text about similar things end up as two vectors that point in a
  similar direction.
- **Cosine similarity** — a number from -1 to 1 measuring how similar two vectors' directions
  are. 1 means "point the same way" (very similar meaning), 0 means unrelated, -1 means opposite.
- **Chunking** — splitting a long document into smaller pieces before embedding them, because
  embedding the whole document at once loses precision and most embedding models have a
  maximum input length anyway.
- **Vector store** — where embeddings are kept so they can be searched. Ranges from "a list in
  memory" (fine for a few hundred chunks) to a dedicated vector database.
- **Tool calling** (also called **function calling**) — a model, instead of only writing text,
  can ask your code to run a specific function and give it the result, so it can act on
  live or private information it doesn't otherwise have.
- **Model Context Protocol (MCP)** — a standard way for an AI assistant to connect to external
  tools and data sources, so the same server works across different AI clients.
- **Agent** — a program built around an LLM that can plan multiple steps, call tools, and use
  the results of one step to decide the next one, rather than answering in a single pass.
- **IChatClient** — the interface in `Microsoft.Extensions.AI` that represents "something you
  can send chat messages to and get a response from," regardless of which underlying provider
  (Ollama, OpenAI, Azure OpenAI, and so on) is behind it.
- **Hallucination** — when a model states something false with the same confidence as
  something true, because it is predicting plausible-sounding text, not looking anything up.
- **Fine-tuning** — retraining a model on your own examples to change its style or behaviour.
  Different problem from RAG: fine-tuning changes *how* a model talks, RAG changes *what*
  facts it has access to.
- **OpenID Connect (OIDC)** — an identity layer built on top of OAuth 2.0 that lets an
  application confirm who a user is (not just what they're allowed to do) via a trusted
  identity provider.
- **OAuth 2.0** — a standard that lets an application get limited access to a user's data or
  actions on another service, without ever seeing that user's password.
- **Application Programming Interface (API)** — a defined way for one piece of software to
  ask another piece of software to do something or return data.

_This file grows as each topic is filled in. Every abbreviation used anywhere else in this
repository should have an entry here._
