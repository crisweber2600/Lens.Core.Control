# Structure Patterns

## Simple Utility

```
bmad-my-utility/
├── SKILL.md              # Complete instructions, input/output spec
└── scripts/              # Core logic
    ├── process.py
    └── tests/
```

## Simple Workflow

```
bmad-my-workflow/
├── SKILL.md              # Steps inline, config loading, output spec
└── resources/            # Optional reference data
```

## Complex Workflow

```
bmad-my-complex-workflow/
├── SKILL.md              # Routing logic, dispatches to prompts/
├── prompts/              # Stage instructions
│   ├── 01-discovery.md
│   ├── 02-planning.md
│   ├── 03-execution.md
│   └── 04-review.md
├── resources/            # Reference data, templates, schemas
├── agents/               # Subagent definitions for parallel work
└── scripts/              # Deterministic operations
    └── tests/
```
