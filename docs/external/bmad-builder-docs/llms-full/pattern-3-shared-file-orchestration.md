# Pattern 3: Shared-File Orchestration

Multiple subagents communicate through shared files, building on each other's work. The parent controls turn order.

| Aspect           | Detail                                                                                                                                        |
| ---------------- | --------------------------------------------------------------------------------------------------------------------------------------------- |
| **How it works** | Agent A writes to `shared.md`; Agent B reads it and adds; Agent A can be resumed to continue; the shared file grows incrementally             |
| **Variants**     | Shared file (multiple agents read/write a common file) or session resumption (reawaken a previous subagent to continue with its full context) |
| **When to use**  | Pipeline stages where later work depends on earlier work, but each agent's context stays small                                                |
