# lens.core.src — Contribution Guide

**Feature:** lens-dev-old-codebase-discovery
**Generated:** 2026-04-21

---

## Overview

`lens.core.src` is the **upstream source** of the LENS Workbench module. Contributions happen here — this is the right place to author new skills, fix bugs, update prompts, improve scripts, and evolve the lifecycle contract.

> **Read-only distribution:** Changes are promoted from here to `lens.core.release` through the CI promotion pipeline. Do not submit PRs to `lens.core.release` directly.

---

## Quick Contribution Path

1. Create a feature branch:

```bash
git checkout main && git pull origin main
git checkout -b fix/my-fix-description
```

2. Make your changes in `_bmad/lens-work/`
3. Validate:

```bash
cd TargetProjects/lens-dev/old-codebase/lens.core.src

uv run _bmad/lens-work/scripts/validate-lens-bmad-registry.py
uv run --with pytest pytest _bmad/lens-work/scripts/tests -q
```

4. Open a PR to `main`. CI runs parity check + tests automatically.

---

## Conventions

### Adding a New Skill

All three command discovery surfaces must be updated in sync when adding or removing a skill:

| Surface | Location | What to Add |
| --- | --- | --- |
| Registry | `module.yaml` | `skills:` entry with `id`, `name`, `command`, `prompt` |
| Help CSV | `module-help.csv` | One CSV row: command, description, skill-id |
| Agent menu | `agents/lens.agent.md` | Entry in compact command menu |
| Help topics | `skills/bmad-lens-help/assets/help-topics.yaml` | Help topic entry |
| Prompt | `prompts/lens-{name}.prompt.md` | Two-step prompt file |
| Skill dir | `skills/bmad-lens-{name}/SKILL.md` | Canonical skill implementation |

Missing any of these surfaces will break command discovery for the new skill.

### Adding a Skill Script

If the skill has an operational script (`{name}-ops.py`):

```python
# /// script
# dependencies = ["pyyaml>=6.0"]
# ///
```

Inline PEP 723 dependency declarations are required at the top of all scripts. This allows `uv run --script` execution without a separate requirements file.

Write a corresponding test file:

```
skills/bmad-lens-{name}/scripts/tests/test-{name}-ops.py
```

Use the existing test files as templates (`test-init-feature-ops.py`, `test-git-orchestration-ops.py`, etc.).

### Modifying Existing Prompts

Prompts follow the two-step pattern. Do not break this:

```markdown
1. Run `run-preflight-cached.py`
2. Read `SKILL.md` from `skills/bmad-lens-{name}/` and follow instructions
```

Edit the source prompt at `_bmad/lens-work/prompts/lens-{name}.prompt.md`. Do not edit the generated adapter stubs (`.github/prompts/`, etc.).

### Lifecycle Contract Changes

- **Non-breaking** (new field, new option): Increment `module_version` in `module.yaml` only.
- **Breaking** (removed field, renamed field, changed valid values): Increment `schema` in `module.yaml`. Add a migration path to `lifecycle.yaml` under `migrations:`. Write a `migrate-ops.py` implementation.

Before shipping a schema increment, verify:

```bash
uv run _bmad/lens-work/skills/bmad-lens-migrate/scripts/migrate-ops.py --dry-run
```

### Script Conventions

- All scripts use `uv run` with inline PEP 723 headers — never import top-level without declaring the dependency
- Scripts that write files should have dry-run mode (`--dry-run` flag)
- Scripts that interact with git should validate the working directory before making changes
- Scripts should emit clear exit codes (0 = success, 1 = validation error, 2 = unrecoverable error)

---

## What Needs Testing

| Change Type | Test Required |
| --- | --- |
| New operational script | Corresponding `test-{name}-ops.py` |
| Modified script | Update existing test to cover the change |
| New skill (prompt + SKILL.md only) | Registry validation pass (`validate-lens-bmad-registry.py`) |
| Lifecycle contract change | Migrate dry-run pass + any affected script tests |
| Prompt change | Registry validation pass |
| Template change | Registry validation pass |

---

## CI Checks on PRs

Every PR to `main` automatically runs:

1. **Registry validation** — `validate-lens-bmad-registry.py`
2. **Full test suite** — `pytest _bmad/lens-work/scripts/tests -q`
3. **Parity check** — `validate-lens-core-parity.sh` (source vs release)
4. **Installer dry-run** — `install.py --dry-run`

All four must pass for a PR to merge.

---

## Issue Reporting

File issues against this repository (`lens.core.src`). Before filing:

1. Pull the latest `main` and confirm the issue still exists
2. Check open issues for duplicates
3. If the issue is in a distributed/consumer control repo — verify it is a module issue (not a workspace configuration issue) before filing here

When filing:
- Specify the `module_version` from `_bmad/lens-work/module.yaml`
- Specify the `schema` version
- Specify the IDE and adapter surface affected (e.g., GitHub Copilot, Claude, Cursor)
- Include the exact command used and the unexpected output

---

## Promotion to Release

After your PR merges to `main`, the promotion pipeline automatically:

1. Validates registry + tests
2. Syncs `_bmad/lens-work/` to `lens.core.release`
3. Regenerates bundled adapter stubs in release
4. Pushes to `lens.core.release` `main`

A maintainer then fast-forwards `lens.core.release` `beta` after spot-verifying the release. Contributors do not need to take any action after their PR merges.

---

_Generated by lens-bmad-document-project for feature lens-dev-old-codebase-discovery._
