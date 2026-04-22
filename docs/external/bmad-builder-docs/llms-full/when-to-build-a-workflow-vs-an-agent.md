# When to Build a Workflow vs. an Agent

| Choose a Workflow When                | Choose an Agent When                         |
| ------------------------------------- | -------------------------------------------- |
| The process has a clear start and end | The user will return to it across sessions   |
| No need to remember past interactions | Remembering context adds value               |
| All steps serve one cohesive goal     | Capabilities are loosely related             |
| You want a composable building block  | You want a persistent conversational partner |

Workflows are also excellent as the **internal capabilities** of an agent. Build the workflow first, then wrap it in an agent if you need persona and memory on top.
