# Topic 5: Model Context Protocol (MCP) from the .NET side

Category: Agentic & Protocols
Status: Planned (not built yet, will live in `src/FieldGuide.Core/Mcp/`)

## What it is

A standard way for an AI assistant (like Claude or an agent built on `Microsoft.Agents.AI`) to
connect to external tools and data sources, so the same server works across different AI
clients instead of writing a one-off integration per client.

## How it's used in practice

An MCP server exposes a set of tools (and optionally resources/prompts) over a defined
protocol. Any MCP-compatible client, Claude Code, an agent framework, another AI tool, can
connect to it and use those tools without custom glue code per client.

## Best use cases, and when to use it

| Use MCP when | Use plain tool calling (Topic 1) when |
|---|---|
| The same tool needs to work across multiple different AI clients/assistants | The tool only ever needs to be called from your own app's chat pipeline |
| You're exposing an existing system (a database, an internal API) as a reusable capability | It's a one-off function specific to a single feature |

## Where it's implemented here

Not yet. Given real prior experience already exists (three configured MCP servers used daily:
a custom Jira/git/.NET build server, GitHub, and Playwright), this topic focuses on the side
not yet done: writing an MCP server *in* .NET, not just configuring and using one.

## Gotchas actually hit

None yet, nothing built.

## Exercise

Not applicable until built.
