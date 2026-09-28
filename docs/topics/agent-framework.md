# Topic 6: Microsoft Agent Framework

Category: Agentic & Protocols
Status: Planned (not built yet, will live in `src/FieldGuide.Core/Agents/`)

## What it is

An agent is a program built around a Large Language Model (LLM) that can plan multiple steps,
call tools, and use the result of one step to decide the next one, rather than answering in a
single pass the way Topic 1's tool calling does. The Microsoft Agent Framework
(`Microsoft.Agents.AI` on NuGet, version 1.22.0 confirmed 2026-09-18) is Microsoft's .NET
library for building these, supporting multi-agent patterns (sequential, concurrent, group
chat, handoff) and graph-based workflows with checkpointing.

## How it's used in practice

Where Topic 1 is "one call, the model may use one tool, done," an agent can decide to call a
tool, look at the result, decide it needs another tool, and keep going until the task is
actually finished, planning and re-planning across steps.

## Best use cases, and when to use it

| Use an agent when | Use plain tool calling when |
|---|---|
| The task needs multiple steps where each depends on the last | One tool call answers the question |
| Multiple specialized agents should hand off work between each other | A single model with a few tools is enough |

## Where it's implemented here

Not yet. `Microsoft.Agents.AI` depends on `Microsoft.Extensions.AI`, so this topic builds
directly on Topic 1's `IChatClient` plumbing rather than starting fresh.

## Gotchas actually hit

None yet, nothing built.

## Exercise

Not applicable until built.
