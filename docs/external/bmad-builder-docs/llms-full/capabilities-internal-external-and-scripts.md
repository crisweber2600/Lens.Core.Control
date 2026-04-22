# Capabilities: Internal, External, and Scripts

| Type                  | Description                                                 | Example                                                       |
| --------------------- | ----------------------------------------------------------- | ------------------------------------------------------------- |
| **Internal commands** | Prompt-driven actions defined inside the agent's skill file | A Dream Agent's "Dream Capture" command                       |
| **External skills**   | Standalone skills or workflows the agent can invoke         | Calling the `create-prd` workflow via a PM agent              |
| **Scripts**           | Deterministic operations offloaded from the LLM             | Validation, data processing, file operations                  |

You choose the mix when you design the agent. Internal commands keep everything self-contained. External skills let you compose agents from shared building blocks, and scripts handle operations where determinism matters more than judgment.

## Evolvable Capabilities

Memory agents can optionally support **evolvable capabilities**. When enabled, the agent gets a capability-authoring reference and a "Learned" section in its capability registry. Users can teach the agent new prompt-based, script-based, or multi-file capabilities that it absorbs into its repertoire over time.
