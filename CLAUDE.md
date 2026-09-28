# dotnet-ai-field-guide — Claude Code Instructions

Read [`README.md`](README.md) first, then [`docs/02-architecture-guide.md`](docs/02-architecture-guide.md)
if the task touches how the pieces fit together.

## What this project is

Randolf's personal, living reference for .NET AI engineering: real running code (not just
snippets), tested against a real local Ollama model, documented so it can be understood again
months later without re-deriving it. It doubles as honest portfolio evidence, so accuracy
matters more than breadth: a topic marked "planned" in `docs/index.html` must stay that way
until real, tested code backs it, not just a doc describing what it would look like.

## Ground rules specific to this project

- **Verify, don't guess, package names, versions, and API signatures.** Several bugs in this
  repo's history came from assumed APIs (see git history / commit messages). Check NuGet, the
  Microsoft Learn quickstarts, or the library's own XML docs (`~/.nuget/packages/<pkg>/<ver>/lib/*/*.xml`)
  before writing a call you haven't confirmed.
- **Free and local first.** Ollama, not a paid API, unless Randolf explicitly asks for a cloud
  provider. Say clearly if something would cost money.
- **No AI attribution in commits** on this repo (standing preference, applies to all of
  Randolf's personal projects).
- **Full Word Format** for abbreviations in anything written here (docs, comments, commit
  messages): full words first, abbreviation in brackets, on first use per file.
- **In-code documentation**: XML `<summary>` blocks plus numbered `STEP N of M` comments for
  any non-trivial method, matching Randolf's standard elsewhere.
- Keep `docs/index.html` in sync when adding or moving a symbol worth linking to: update its
  `data-file` / `data-line` attributes, don't let it silently go stale.

## Layout

See the table in `README.md`. `FieldGuide.Core` holds the real logic; `FieldGuide.Api` and
`FieldGuide.Web` are thin layers on top; `FieldGuide.Data` is EF Core against SQL Server.
