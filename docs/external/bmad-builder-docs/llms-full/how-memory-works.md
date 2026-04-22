# How Memory Works

Memory agents store their persistent state in a **sanctum** at `_bmad/memory/<agent-name>/`. The sanctum contains six core files that load on every session:

| File                | Purpose                                                     |
| ------------------- | ----------------------------------------------------------- |
| **PERSONA.md**      | Identity, communication style, traits, evolution log        |
| **CREED.md**        | Mission, values, standing orders, philosophy, boundaries    |
| **BOND.md**         | Owner understanding, preferences, things to remember/avoid  |
| **MEMORY.md**       | Curated long-term knowledge (kept under 200 lines)          |
| **CAPABILITIES.md** | Built-in + learned capabilities registry                    |
| **INDEX.md**        | Map of the sanctum structure (loaded first on every rebirth)|

:::tip[Memory Lives Outside the Skill]
Agent memory is stored in your project, not inside the skill folder. This keeps agents from modifying their own instructions and makes your data portable. The same agent can be used across different projects, each generating its own memory space.
:::

Sanctum architecture, First Breath, PULSE, and the two-tier memory system are covered in **[Agent Memory and Personalization](/explanation/agent-memory-and-personalization.md)**.
