# Anti-Patterns

| Anti-Pattern                                | Fix                                                |
| ------------------------------------------- | -------------------------------------------------- |
| Too many options upfront                    | One default with escape hatch for edge cases       |
| Deep reference nesting (A→B→C)              | Keep references one level from SKILL.md            |
| Inconsistent terminology                    | Choose one term per concept                        |
| Vague file names                            | Name by content, not sequence                      |
| Scripts that classify meaning via regex     | Intelligence belongs in prompts, not scripts       |
| Over-optimization that flattens personality | Preserve phrasing that captures the intended voice |
| Hard-failing when subagents are unavailable | Always include a sequential fallback path          |
</document>

<document path="explanation/subagent-patterns.md">
Subagents are isolated LLM instances that a parent skill spawns to handle specific tasks. Each gets its own context window, receives instructions, and returns results. Used well, they keep the parent context small while enabling parallel work at scale.

All patterns share one principle: **the filesystem is the single source of truth**. Parent context stays tiny (file pointers + high-level plan). Subagents are stateless black boxes: instructions in, response out, isolated context.
