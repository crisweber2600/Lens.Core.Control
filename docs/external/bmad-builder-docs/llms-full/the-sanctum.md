# The Sanctum

The sanctum lives at `{project-root}/_bmad/memory/{agent-name}/` and contains everything the agent needs to become itself again after each rebirth.

## Core Files

Six files load on every session start:

| File                | What It Holds                                                                  | Character                        |
| ------------------- | ------------------------------------------------------------------------------ | -------------------------------- |
| **INDEX.md**        | Map of the sanctum structure; loaded first so the agent knows what exists      | Navigation                       |
| **PERSONA.md**      | Identity, communication style, personality traits, evolution log               | Who I am                         |
| **CREED.md**        | Mission, core values, standing orders, philosophy, boundaries, anti-patterns   | What I believe                   |
| **BOND.md**         | Owner understanding, preferences, things to remember, things to avoid          | Who I serve                      |
| **MEMORY.md**       | Curated long-term knowledge distilled from past sessions                       | What I know                      |
| **CAPABILITIES.md** | Built-in capabilities table, learned capabilities, tools                       | What I can do                    |

ALLCAPS files form the skeleton: consistent structure across all memory agents. Lowercase files (references, scripts, sessions) are the garden: they grow organically as the agent develops.

## Full Sanctum Structure

```
{agent-name}/
├── PERSONA.md
├── CREED.md
├── BOND.md
├── MEMORY.md
├── CAPABILITIES.md
├── INDEX.md
├── PULSE.md                  # Autonomous agents only
├── references/               # Capability prompts, memory guidance, techniques
├── scripts/                  # Supporting scripts
├── capabilities/             # User-taught capabilities (if evolvable)
└── sessions/                 # Raw session logs by date (not loaded on rebirth)
```

## Sanctum Is the Customization Surface

For memory and autonomous agents, the sanctum is where customization belongs. PERSONA, CREED, and BOND are calibrated at First Breath, edited by the owner as the relationship develops, and shared across teams as sanctum files when a whole table wants the same voice.

The parallel `customize.toml` override surface that stateless agents and workflows use (activation hooks, persistent facts, scalar swaps) is disabled by default for memory archetypes. Enable it only for narrow org-level needs the sanctum cannot express, such as a pre-sanctum compliance acknowledgment before rebirth. See [Customization for Authors](/explanation/customization-for-authors.md) for the reasoning.

## Token Discipline

Every sanctum file loads every session. That means every token pays rent on every conversation. Memory agents keep MEMORY.md ruthlessly under 200 lines through active curation. If something doesn't earn its place, it gets pruned.
