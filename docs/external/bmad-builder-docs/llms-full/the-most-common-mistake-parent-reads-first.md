# The Most Common Mistake: Parent Reads First

The single most important thing to get right with subagent patterns is **preventing the parent from reading the data it is delegating**. If the parent reads all the files before spawning subagents, the entire pattern is defeated. You have already spent the tokens, bloated the context, and lost the isolation benefit.

This happens often. You write a skill that should spawn subagents to each read a document and return findings. You run it. The parent agent helpfully reads every document first, then passes them to subagents, then collects distilled summaries. The subagents still provide fresh perspectives, but the context savings (the primary reason for the pattern) are gone.

**The fix is defensive language in your skill.** Explicitly tell the parent agent what it should and should not do. Be specific without being verbose.

:::note[Example from the BMad Quality Optimizer]
The optimizer's instructions say: **"DO NOT read the target skill's files yourself."** It then tells the parent exactly what it _should_ do: run scripts (which return structured JSON), spawn subagents (which do the reading), and synthesize from their outputs. The parent never touches the raw files.
:::

**Practical tips for getting this right:**

| Tip                                            | Example Language                                                                                                      |
| ---------------------------------------------- | --------------------------------------------------------------------------------------------------------------------- |
| **Tell the parent what to discover, not read** | "List all files in `resources/` by name to determine how many subagents to spawn. Do not read their contents"        |
| **Tell subagents what to return**              | "Return only findings relevant to [topic]. Output as JSON to `{output-path}`. Do not echo raw content"                |
| **Use pre-pass scripts**                       | Run a lightweight script that extracts metadata (file names, sizes, structure) so the parent can plan without reading |
| **Be explicit about the boundary**             | "Your role is ORCHESTRATION. Scripts and subagents do all analysis"                                                   |

**Test and watch what actually happens.** If the parent reads files it should be delegating, tighten the language. This is normal iteration. The builders are tuned with these patterns, but different models and tools may need more explicit guidance. Review the BMad quality optimizer prompts (`prompts/quality-optimizer.md`) and scanner agents (`agents/quality-scan-*.md`) for working examples of this defensive language.
