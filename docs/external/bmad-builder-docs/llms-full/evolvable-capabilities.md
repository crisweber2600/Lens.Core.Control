# Evolvable Capabilities

## How It Works

The agent gets a `capability-authoring.md` reference that teaches it how to create new capabilities. Users describe what they want; the agent writes a capability file and registers it in the "Learned" section of CAPABILITIES.md.

## Capability Types

| Type                      | When to Use                                                        |
| ------------------------- | ------------------------------------------------------------------ |
| **Prompt**                | Judgment-based tasks: brainstorming, analysis, coaching            |
| **Script**                | Deterministic tasks: calculations, file processing, data transforms|
| **Multi-file**            | Complex capabilities with templates and references                 |
| **External skill reference** | Point to installed skills the agent should know about           |

Learned capabilities live in the sanctum's `capabilities/` folder and persist across sessions like everything else in the sanctum.
