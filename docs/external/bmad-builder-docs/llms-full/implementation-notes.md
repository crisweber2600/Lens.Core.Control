# Implementation Notes

- Force **strict JSON schemas** on every subagent output for reliable parent parsing
- Use **git worktrees** or per-agent directories to prevent crosstalk
- Start small: one orchestrator that reads `plan.md` and spawns the first wave
- Patterns compose: use Delegated Access for data gathering, Persona-Driven for analysis, Temp File Assembly for the final report
- Always include **graceful degradation**. If subagents are unavailable, the main agent performs the work sequentially
</document>

<document path="explanation/what-are-bmad-agents.md">
BMad Agents are AI skills that combine a **persona**, **capabilities**, and optionally **persistent memory** into a conversational partner. They range from focused, stateless experts to evolving companions that remember you across sessions.
