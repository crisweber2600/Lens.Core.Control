# lens.core.src — Development Guide

**Feature:** lens-dev-old-codebase-discovery
**Generated:** 2026-04-21

---

## Overview

This guide is for developers working **in** the `lens.core.src` source repository — authoring new skills, updating prompts, modifying scripts, or fixing bugs. This is the upstream source of the LENS Workbench module. All changes made here propagate to the release distribution through the promotion pipeline.

> **Consumer of LENS?** If you are setting up a new project to use Lens (not contributing to it), see the release distribution guide (`lens.core.release`) instead.

---

## Prerequisites

| Requirement | Minimum Version | Check |
| --- | --- | --- |
| Git | 2.28+ | `git --version` |
| Python | 3.10+ | `python3 --version` |
| `uv` (Python tool runner) | Latest | `uv --version` |
| Node.js | 18+ (for installer.js) | `node --version` |
| `gh` CLI | Latest (for PR creation) | `gh --version` |
| GitHub PAT | Read/write scopes | `echo $GITHUB_TOKEN` |

---

## Quick Start — Working With the Source

```bash
# Navigate to the source project in the control repo
cd TargetProjects/lens-dev/old-codebase/lens.core.src

# Verify the registry is consistent (skills + prompts match module.yaml)
uv run _bmad/lens-work/scripts/validate-lens-bmad-registry.py

# Run the full test suite
uv run --with pytest pytest _bmad/lens-work/scripts/tests -q

# Run a specific test module
uv run --with pytest pytest _bmad/lens-work/scripts/tests/test-install.py -q

# Dry-run the installer to verify adapter generation
uv run _bmad/lens-work/scripts/install.py --dry-run
```

---

## Project Structure for Development

The only directory you need to edit is `_bmad/lens-work/`. All module content lives here.

```
_bmad/lens-work/
├── module.yaml          ← Registry of all skills and prompts
├── lifecycle.yaml       ← Lifecycle contract and phase model
├── module-help.csv      ← Command discovery registry
├── agents/              ← Agent shell menu (update when adding commands)
├── prompts/             ← 57 entry-point prompts (edit these directly)
├── skills/              ← 41 skill directories (edit SKILL.md files directly)
├── scripts/             ← 19 operational scripts (edit Python files directly)
├── assets/templates/    ← 11 authoring templates
└── docs/                ← 24 embedded reference docs
```

---

## Adding a New Skill

Follow this checklist when adding a skill to the module:

### 1. Create the skill directory

```bash
mkdir -p _bmad/lens-work/skills/bmad-lens-{name}/scripts/tests
```

Create `_bmad/lens-work/skills/bmad-lens-{name}/SKILL.md` with the skill implementation.

### 2. Create the corresponding prompt

```bash
touch _bmad/lens-work/prompts/lens-{name}.prompt.md
```

Prompts follow the two-step pattern:
1. Run `light-preflight.py`
2. Load SKILL.md and follow instructions

### 3. Register the skill in module.yaml

```yaml
skills:
  - id: bmad-lens-{name}
    name: "Lens Name Skill"
    command: /lens-{name}
    prompt: lens-{name}.prompt.md
```

### 4. Add to module-help.csv

```csv
/lens-{name},Short description of what the skill does,bmad-lens-{name}
```

### 5. Update the agent shell menu

Edit `_bmad/lens-work/agents/lens.agent.md` to add the command to the compact menu if it is user-facing.

> **See repo memory:** "Lens command discovery surfaces" — all three surfaces (module-help.csv, help-topics.yaml, lens.agent.md) must be updated in sync when adding or removing a command.

### 6. Update the help topics

Edit `_bmad/lens-work/skills/bmad-lens-help/assets/help-topics.yaml` to add the new command entry.

### 7. Write a test (if script is included)

If the skill has a script (`scripts/{name}-ops.py`), write a corresponding test:

```bash
# Script convention — inline PEP 723 dependencies
# At the top of {name}-ops.py:
# /// script
# dependencies = ["pyyaml>=6.0"]
# ///

# Test file
touch _bmad/lens-work/skills/bmad-lens-{name}/scripts/tests/test-{name}-ops.py
```

### 8. Validate

```bash
# Validate registry consistency
uv run _bmad/lens-work/scripts/validate-lens-bmad-registry.py

# Run tests
uv run --with pytest pytest _bmad/lens-work/scripts/tests -q
```

---

## Modifying an Existing Skill

1. Edit `_bmad/lens-work/skills/bmad-lens-{name}/SKILL.md` directly.
2. If the skill has a script, edit `scripts/{name}-ops.py` and update its test file.
3. Run the relevant test module:

```bash
uv run --with pytest pytest _bmad/lens-work/skills/bmad-lens-{name}/scripts/tests -q
```

---

## Modifying Prompts

Prompts live at `_bmad/lens-work/prompts/lens-{name}.prompt.md`. Edit them directly.

> **Important:** IDE adapter stubs in `.github/prompts/`, `.claude/commands/`, etc. are **generated** by `install.py`. Do not edit the stubs — edit the source prompts. The stubs are regenerated when `install.py --update` is run.

---

## Modifying the Lifecycle Contract

The lifecycle contract lives at `_bmad/lens-work/lifecycle.yaml`. Key considerations:

- **Adding a new phase:** Add to `phases:` list with all required fields. Update `feature-yaml-ops.py` VALID_PHASES.
- **Adding a new track:** Add to `tracks:` list and update TRACK_TRANSITIONS dict.
- **Schema-breaking change:** Increment `schema:` in `module.yaml`. Write a `migrate-ops.py` migration path. See existing migrations for pattern.
- **Non-breaking change:** Update `module_version:` in `module.yaml` only.

---

## Running Preflight Checks

```bash
# Full preflight (validates git, Python, schema, governance connectivity)
uv run _bmad/lens-work/scripts/preflight.py --caller check

# Light preflight (subset, faster — used by prompts)
uv run _bmad/lens-work/scripts/run-preflight-cached.py
```

> **Gotcha:** When run from `TargetProjects/lens-dev/old-codebase/lens.core.src`, the preflight script resolves `project_root` to the nearest ancestor containing a `lens.core` directory, which resolves to `TargetProjects/` — not the workspace root. Preflight then looks for `lifecycle.yaml` at `TargetProjects/lens.core/_bmad/lens-work/lifecycle.yaml`. Use targeted test commands for source-only validation.

---

## Targeted Test Commands

For common validation tasks, use these focused commands from the source root:

```bash
# Registry validation
uv run _bmad/lens-work/scripts/validate-lens-bmad-registry.py

# Init-feature logic
uv run --script _bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/test-init-feature-ops.py

# Git orchestration logic
uv run --with pytest --with pyyaml pytest _bmad/lens-work/skills/bmad-lens-git-orchestration/scripts/tests/test-git-orchestration-ops.py -q

# Next-action routing
uv run --with pytest --with pyyaml pytest _bmad/lens-work/skills/bmad-lens-next/scripts/tests/test-next-ops.py -q

# Adapter generation
uv run --with pytest pytest _bmad/lens-work/scripts/tests/test-install.py -q

# Setup-control-repo
uv run --with pytest pytest _bmad/lens-work/scripts/tests/test-setup-control-repo.py -q
```

---

## Parity Check (Source vs Release)

Before merging changes to `main`, verify parity between source and release:

```bash
bash _bmad/lens-work/assets/lens-bmad-parity-check.sh
```

This check runs automatically in CI on PRs to `main`.

---

## Promotion to Release

Once changes are merged to `main` in `lens.core.src`, the promotion pipeline:

1. Runs parity validation (validates source vs release consistency)
2. Copies `_bmad/lens-work/` content to `lens.core.release`
3. Runs the installer to regenerate pre-bundled adapter stubs
4. Pushes to `lens.core.release` `beta` branch

The `beta` branch is the stable release. Consumers run:

```bash
git -C TargetProjects/lens-dev/release/lens.core.release pull origin beta
```

---

_Generated by lens-bmad-document-project for feature lens-dev-old-codebase-discovery._
