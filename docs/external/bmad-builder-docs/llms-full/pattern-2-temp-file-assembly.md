# Pattern 2: Temp File Assembly

For large-scale operations with potentially a lot of relevant data across multiple sources. Subagents write results to temp files. A separate assembler subagent combines them into a cohesive deliverable.

| Aspect           | Detail                                                                                                                                                              |
| ---------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **How it works** | Parent spawns N worker subagents writing to `tmp/{n}.md`; after all complete, spawns an assembler subagent that reads all temp files and creates the final artifact |
| **When to use**  | When summaries are still too large to return inline, or when assembly needs a dedicated agent with fresh context                                                    |
| **Example**      | The BMad quality optimizer uses this: 5 parallel scanner subagents write temp JSON, then a report-creator subagent synthesizes them                                |
