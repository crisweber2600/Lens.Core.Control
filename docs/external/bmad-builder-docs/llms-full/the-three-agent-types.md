# The Three Agent Types

Agents exist on a spectrum. The builder detects which type fits through natural conversation.

| Type           | Memory | First Breath | Autonomous | Build For                                                    |
| -------------- | ------ | ------------ | ---------- | ------------------------------------------------------------ |
| **Stateless**  | No     | No           | No         | Isolated sessions, focused experts (code formatter, diagram generator, meeting summarizer) |
| **Memory**     | Yes    | Yes          | No         | Ongoing relationships where remembering adds value (code coach, writing partner, domain advisor) |
| **Autonomous** | Yes    | Yes          | Yes        | Proactive value creation between sessions (idea incubation, project monitoring, content curation) |

## Stateless Agents

Everything lives in a single SKILL.md with supporting references. No memory directory, no initialization ceremony. The agent brings a persona and capabilities but treats every session as independent. Pick this type when prior session context wouldn't change the agent's behavior.

## Memory Agents

A lean bootloader SKILL.md (~30 lines) points to a **sanctum**: a set of persistent files the agent reads on every launch to become itself again. The sanctum holds the agent's identity, values, understanding of its owner, curated knowledge, and capability registry. On first launch, a **First Breath** conversation lets the agent discover who you are and calibrate itself to your needs.

Memory agents treat every session as a rebirth. They don't fake continuity; they read their sanctum files and become themselves again. If they don't remember something, they say so and check the files.

## Autonomous Agents

Everything a memory agent has, plus a PULSE file that defines what the agent does when no one's watching. Autonomous agents can wake on a schedule (cron, background task) and perform maintenance, from curating memory to checking on projects to running domain-specific tasks. With a human present, they're conversational. Headless, they work independently and exit.
