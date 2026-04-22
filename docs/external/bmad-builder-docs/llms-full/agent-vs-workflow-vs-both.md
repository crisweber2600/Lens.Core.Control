# Agent vs. Workflow vs. Both

The first architecture decision when planning a module is whether to use a single agent, multiple workflows, or a combination.

| Architecture                       | When It Fits                                                                 | Trade-offs                                                                                                    |
| ---------------------------------- | ---------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------- |
| **Single agent with capabilities** | All capabilities serve the same user journey and benefit from shared context | Simpler to maintain, better memory continuity, seamless UX. Can feel monolithic if capabilities are unrelated |
| **Multiple workflows**             | Capabilities serve different user journeys or require different tools        | Each workflow is focused and composable. Users switch between skills explicitly                               |
| **Hybrid**                         | Some capabilities need persistent persona/memory while others are procedural | Best of both worlds but more skills to build and maintain                                                     |

:::tip[Agent-First Thinking]
Many users default to building multiple single-purpose agents. Consider whether one agent with rich internal capabilities and routing would serve users better. A single agent accumulates context, maintains memory across interactions, and provides a smoother experience.
:::
