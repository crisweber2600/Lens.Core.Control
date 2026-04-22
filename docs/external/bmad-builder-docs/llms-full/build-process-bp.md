# Build Process (BP)

The core creative path. Six phases of conversational discovery take you from a rough idea to a complete, tested skill folder.

## Input Types

Both builders accept any of these as a starting point.

| Input                             | What Happens                                              |
| --------------------------------- | --------------------------------------------------------- |
| A rough idea or description       | Guided discovery from scratch                             |
| An existing BMad skill path       | Edit mode. Analyze what exists, determine what to change  |
| A non-BMad skill, tool, or code   | Convert to BMad-compliant structure                       |
| Documentation, API specs, or code | Extract intent and requirements automatically             |

## Interaction Modes

| Mode           | Behavior                                                                                     | Best For                                     |
| -------------- | -------------------------------------------------------------------------------------------- | -------------------------------------------- |
| **Guided**     | The builder walks through decisions, clarifies ambiguities, ensures completeness             | Production skills, first-time builders       |
| **YOLO**       | Brain-dump your idea; the builder guesses its way to a finished skill with minimal questions | Quick prototypes, experienced builders       |
| **Autonomous** | Fully headless; no interactive prompts, proceeds with safe defaults                          | CI/CD, batch processing, orchestrated builds |

## Build Phases

| Phase | Agent Builder                                                                                    | Workflow Builder                                                                                      |
| ----- | ------------------------------------------------------------------------------------------------ | ----------------------------------------------------------------------------------------------------- |
| 1     | **Discover Intent**: understand the vision; detect agent type (stateless, memory, or autonomous) through natural questions | **Discover Intent**: understand the vision; accepts any input format                                  |
| 2     | **Capabilities Strategy**: internal commands, external skills, scripts; evolvable capability decision | **Classify Skill Type**: Simple Utility, Simple Workflow, or Complex Workflow; module membership      |
| 3     | **Gather Requirements**: identity, persona memory seeds, First Breath territories, PULSE behaviors, folder dominion | **Gather Requirements**: name, description, stages, config variables, output artifacts, dependencies  |
| 4     | **Draft & Refine**: present outline, iterate until ready                                         | **Draft & Refine**: present plan, clarify gaps, iterate until ready                                   |
| 5     | **Build**: generate skill structure per agent type, lint gate                                     | **Build**: generate skill structure, lint gate                                                        |
| 6     | **Summary**: present results, offer Quality Optimize                                             | **Summary**: present results, run unit tests if scripts exist, offer Quality Optimize                 |

## Agent Builder: Phase 1 Agent Type Detection

The builder determines the agent type through natural questions, not a menu:

| Question (asked naturally)                          | If No          | If Yes                     |
| --------------------------------------------------- | -------------- | -------------------------- |
| Does this agent need to remember between sessions?  | Stateless      | Memory or Autonomous       |
| Should the user be able to teach it new things?     | Fixed capabilities | Evolvable capabilities |
| Does it operate autonomously between sessions?      | Memory         | Autonomous                 |

For memory and autonomous agents, the builder also determines **relationship depth**: deep (calibration-style First Breath with open-ended discovery) or focused (configuration-style First Breath with guided questions).

## Agent Builder: Phase 2 Capabilities Strategy

Determines the mix of internal and external capabilities, plus script opportunities.

| Capability Type           | Description                                                                             |
| ------------------------- | --------------------------------------------------------------------------------------- |
| **Internal commands**     | Prompt-driven actions, each gets a file in `references/`                                |
| **External skills**       | Standalone skills the agent invokes by registered name                                  |
| **Scripts**               | Deterministic operations offloaded from the LLM (validation, data processing, file ops) |
| **Evolvable capabilities**| If enabled: user can teach the agent new capabilities over time via authoring reference  |

## Agent Builder: Phase 3 Requirements

Requirements differ by agent type. Stateless agents need identity and capabilities. Memory and autonomous agents need everything below.

**All agent types:**

| Requirement          | Description                                                                         |
| -------------------- | ----------------------------------------------------------------------------------- |
| **Identity**         | Who is this agent? Communication style, decision-making philosophy                  |
| **Capabilities**     | Internal commands, external skills, scripts                                         |
| **Folder dominion**  | Read boundaries, write boundaries, explicit deny zones                              |

**Memory and autonomous agents add:**

| Requirement                  | Description                                                                    |
| ---------------------------- | ------------------------------------------------------------------------------ |
| **Identity seed**            | 2-3 sentences of personality DNA for PERSONA.md                                |
| **Species-level mission**    | Domain-specific purpose statement for CREED.md                                 |
| **Core values**              | 3-5 values that guide behavior                                                 |
| **Standing orders**          | Surprise-and-delight + self-improvement, adapted to the domain with examples   |
| **CREED seeds**              | Philosophy, boundaries, anti-patterns (behavioral + operational)               |
| **BOND territories**         | Domain-specific areas to learn about the owner                                 |
| **First Breath territories** | Discovery questions beyond the universal set                                   |

**Autonomous agents add:**

| Requirement              | Description                                                                    |
| ------------------------ | ------------------------------------------------------------------------------ |
| **PULSE behaviors**      | Default wake behavior, domain-specific autonomous tasks                        |
| **Named task routing**   | Tasks invoked via `--headless {task-name}` or `-H {task-name}`                 |
| **Frequency & quiet hours** | How often to wake, when not to                                              |

## Workflow Builder: Phase 2-3 Details

**Skill type classification** determines template and structure.

| Type                 | Signals                                                                       | Structure                                                                           |
| -------------------- | ----------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- |
| **Simple Utility**   | Composable building block, clear input/output, usually mostly script-driven   | Single SKILL.md, scripts folder                                                     |
| **Simple Workflow**  | Fits in one SKILL.md, a few sequential steps, optional autonomous             | SKILL.md with inline steps, optional prompts and resources                          |
| **Complex Workflow** | Multiple stages, branching prompt flows, progressive disclosure, long-running | SKILL.md for routing, `prompts/` for stage details, `resources/` for reference data |

**Workflow-specific requirements** gathered in Phase 3:

| Requirement             | Simple Utility | Simple Workflow | Complex Workflow                         |
| ----------------------- | -------------- | --------------- | ---------------------------------------- |
| **Input/output format** | Yes            | -               | -                                        |
| **Composability**       | Yes            | -               | -                                        |
| **Steps**               | -              | Numbered steps  | Named stages with progression conditions |
| **Headless mode**       | -              | Optional        | Optional                                 |
| **Config variables**    | -              | Core + custom   | Core + module-specific                   |
| **Module sequencing**   | Optional       | Optional        | Recommended                              |

## Build Output

The output structure depends on the agent type.

**Stateless agents:**

```
{skill-name}/
├── SKILL.md              # Full identity + persona + capabilities
├── references/           # Capability prompts
├── agents/               # Subagent definitions (if needed)
├── scripts/              # Deterministic scripts
│   └── tests/            # Unit tests for scripts
└── assets/               # Templates (if needed)
```

**Memory and autonomous agents:**

```
{skill-name}/
├── SKILL.md              # Lean bootloader (~30 lines of content)
├── references/
│   ├── first-breath.md   # First Breath conversation guide
│   ├── memory-guidance.md          # Session close and curation practices
│   ├── capability-authoring.md     # If evolvable capabilities enabled
│   └── {capability}.md             # Outcome-focused capability prompts
├── assets/               # Sanctum seed templates
│   ├── INDEX-template.md
│   ├── PERSONA-template.md
│   ├── CREED-template.md
│   ├── BOND-template.md
│   ├── MEMORY-template.md
│   ├── CAPABILITIES-template.md
│   └── PULSE-template.md          # Autonomous agents only
├── agents/               # Subagent definitions (if needed)
└── scripts/
    ├── init-sanctum.py   # Creates sanctum folder, copies templates, generates CAPABILITIES.md
    └── tests/
```

The seed templates contain real content from the discovery phases, not placeholders. The init script is parameterized with the skill name, file lists, and evolvable flag.

**Workflow builder** output remains the same regardless of agent type:

```
{skill-name}/
├── SKILL.md              # Skill instructions
├── prompts/              # Stage prompts for complex workflows
├── resources/            # Reference data
├── agents/               # Subagent definitions for parallel processing
├── scripts/              # Deterministic scripts
│   └── tests/            # Unit tests for scripts
└── templates/            # Building blocks for generated output
```

## Lint Gate

Before completing the build, both builders run deterministic validation.

| Script                   | What It Checks                                                                            |
| ------------------------ | ----------------------------------------------------------------------------------------- |
| `scan-path-standards.py` | Path conventions: `{project-root}` for project-scope, `./` for same-folder references, bare paths for cross-directory skill-internal, no double-prefix        |
| `scan-scripts.py`        | Script portability, PEP 723 metadata, agentic design, unit test presence                  |

Critical issues block completion. Warnings are noted but don't block.
