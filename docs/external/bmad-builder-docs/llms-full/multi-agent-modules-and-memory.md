# Multi-Agent Modules and Memory

Modules with multiple agents introduce a memory architecture decision. BMad agents exist on a spectrum from stateless (no memory) through memory agents (personal sanctum) to autonomous agents (sanctum + PULSE). In a multi-agent module, you choose both the agent type for each skill and whether agents should share memory across the module.

| Pattern                              | When It Fits                                                                            |
| ------------------------------------ | --------------------------------------------------------------------------------------- |
| **Personal memory only**                | Agents have distinct domains with minimal overlap                                       |
| **Personal + shared module memory**     | Agents have their own context but also learn shared things about the user or project    |
| **Shared memory only**                  | All agents serve the same domain; consider whether a single agent is the better design |
| **Mixed types**                         | Some agents need memory (coaches, companions) while others are stateless (formatters, validators) |

**Example:** A social creative module with a podcast expert, a viral video expert, and a blog expert. Each memory agent maintains its own sanctum with what it has done with the user (episode topics, video formats, blog themes). But they all also contribute to a module-level memory folder that captures the user's communication style, favorite catchphrases, content preferences, and brand voice.

Each agent should still be self-contained with its own capabilities, even if this means duplicating some common functionality. A podcast expert that can independently handle a full session without needing the blog expert is better than one that depends on shared state to function.

See **[What Are BMad Agents](/explanation/what-are-bmad-agents.md)** for the three agent types, and **[Agent Memory and Personalization](/explanation/agent-memory-and-personalization.md)** for details on how the sanctum architecture works.
