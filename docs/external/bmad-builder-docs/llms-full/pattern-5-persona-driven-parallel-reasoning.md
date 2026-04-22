# Pattern 5: Persona-Driven Parallel Reasoning

The most powerful pattern for quality. Spawn diverse specialists in parallel, producing genuinely independent thinking from isolated contexts.

| Aspect           | Detail                                                                                                                                                                                        |
| ---------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **How it works** | Parent spawns 3-6 agents with distinct personas (Architect, Red Teamer, Pragmatist, Innovator); each writes findings independently; an evaluator subagent scores and merges the best elements |
| **When to use**  | Design decisions, code review, strategy, any task where diverse perspectives improve quality                                                                                                  |
| **Key**          | Heavy persona injection gives genuinely different outputs, not just paraphrases of the same analysis                                                                                          |

**Useful diversity packs:**

| Persona           | Perspective                                       |
| ----------------- | ------------------------------------------------- |
| **Architect**     | Scale and elegance above all                      |
| **Red Teamer**    | Break this. What fails?                           |
| **Pragmatist**    | Ship it Friday. What is the minimum?              |
| **Innovator**     | What if we approached this entirely differently?  |
| **User Advocate** | How does the end user actually experience this?   |
| **Future-Self**   | With 5 years of hindsight, what would you change? |

**Sub-patterns:**

| Sub-Pattern                | How It Works                                                                                              |
| -------------------------- | --------------------------------------------------------------------------------------------------------- |
| **Multi-Path Exploration** | Same task, different personas. Each writes to `/explorations/path_N/`. Parent prunes or merges best paths |
| **Debate & Critique**      | Round 1: parallel proposals. Round 2: critics attack proposals. Round 3: refinement                       |
| **Ensemble Voting**        | Same subtask K times with persona variations. Evaluator scores. Weighted merge of winners                 |
