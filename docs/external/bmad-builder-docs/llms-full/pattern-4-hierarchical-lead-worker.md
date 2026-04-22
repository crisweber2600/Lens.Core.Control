# Pattern 4: Hierarchical Lead-Worker

A lead subagent analyzes the task once and writes a breakdown. The parent spawns workers from that plan. Mid-level sub-orchestrators can handle complex subtasks.

| Aspect           | Detail                                                                                                                                               |
| ---------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------- |
| **How it works** | Lead agent writes `plan.json` with task breakdown; parent reads plan and spawns workers in parallel; complex subtasks get their own sub-orchestrator |
| **When to use**  | Tasks that need analysis before decomposition, or where the parent cannot predict the work structure upfront                                         |
| **Variant**      | Master-clone: spawn near-identical agents with slight persona tweaks exploring different branches of the same problem                               |
