# Project Documentation Index: bmad-lens-switch

**Feature:** lens-dev-new-codebase-switch
**Generated:** 2026-04-27
**Project Type:** Python CLI/Library (Lens module skill)
**Scan Level:** Deep

---

## Documentation Artifacts

| Artifact | Path | Description |
|----------|------|-------------|
| Project Overview | [project-overview.md](./project-overview.md) | Architecture, components, data flow, API reference |
| SKILL.md | [../../stories/SW-11.md](../../stories/SW-11.md) | Full skill contract, JSON response shapes |
| Planning Artifacts | [../](../) | Epics, stories, tech plan, sprint status |
| Retrospective | [retrospective (governance)](../../../../../../../TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-switch/retrospective.md) | Post-delivery analysis |

---

## Implementation Summary

The `bmad-lens-switch` skill provides feature context switching for Lens agent sessions. It consists of:

- **`switch-ops.py`** — core Python script (single-file, uv-runnable) implementing `list` and `switch` subcommands
- **`SKILL.md`** — skill API contract and documentation
- **`references/`** — operation-specific guidance for `switch-feature` and `list-features`
- **`tests/test-switch-ops.py`** — pytest regression suite (18 cases)

---

## Source Paths

All implementation lives in the target repo:

```
TargetProjects/lens-dev/new-codebase/lens.core.src/
└── _bmad/lens-work/skills/bmad-lens-switch/
    ├── SKILL.md
    ├── scripts/
    │   ├── switch-ops.py
    │   └── tests/
    │       └── test-switch-ops.py
    └── references/
        ├── switch-feature.md
        └── list-features.md
```

Discovery surfaces modified:
- `_bmad/lens-work/module-help.csv`
- `_bmad/lens-work/bmad-lens-work-setup/assets/module-help.csv`
- `_bmad/lens-work/prompts/lens-switch.prompt.md`
- `_bmad/lens-work/agents/lens.agent.md`

---

## Test Execution

```bash
# Focused regression (18 cases)
uv run --with pytest pytest _bmad/lens-work/skills/bmad-lens-switch/scripts/tests/test-switch-ops.py -q

# Full lens-work suite (35 cases)
uv run --with pytest --with pyyaml python -m pytest _bmad/lens-work -q
```

Run from: `TargetProjects/lens-dev/new-codebase/lens.core.src`
