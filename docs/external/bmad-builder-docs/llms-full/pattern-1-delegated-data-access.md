# Pattern 1: Delegated Data Access

The simplest pattern. Subagents read sources and return only distilled summaries. The parent never touches raw data.

| Aspect               | Detail                                                                                                                       |
| -------------------- | ---------------------------------------------------------------------------------------------------------------------------- |
| **How it works**     | Parent spawns readers in parallel; each reads a source and returns a compact summary; parent synthesizes from summaries only |
| **Critical rule**    | Parent must delegate _before_ touching any source material. If it reads first, the tokens are already spent                 |
| **When to use**      | 5+ documents, web research, large codebase exploration                                                                       |
| **Not worth it for** | 1-2 files where the overhead exceeds the savings                                                                             |
| **Token savings**    | ~99%. Five docs at 15K tokens each = 75K raw vs ~350 tokens in summaries                                                     |
