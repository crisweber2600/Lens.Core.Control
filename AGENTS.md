# AGENTS.md — Lens.Core.Control

## What This Repo Is

This is a **LENS Workbench control repo**. It is a local workspace container — not a source repo itself. It orchestrates planning, documentation, and lifecycle operations for the LENS project.

## Critical Rules

| Rule | Detail |
|------|--------|
| **Never write to `lens.core/`** | It is a read-only clone of the Lens.Core.Release module. Changes there are overwritten on every pull. |
| **All code changes go in `TargetProjects/`** | The real source repos live here. The primary source for the lens-work module is `TargetProjects/lens-dev/new-codebase/lens.core.src/`. |
| **All Lens documentation goes in `docs/`** | Planning artifacts, phase reports, and sprint files are written under `docs/lens-dev/new-codebase/<feature-folder>/`. |
| **Never generate code in the control repo root** | Only `docs/`, `setup.py`, and `.lens/` (non-personal) are tracked by this repo. |

## Directory Layout

```
lens.core/                          # READ-ONLY — local clone of Lens.Core.Release
  _bmad/lens-work/                  # Reference copy of the LENS workbench module
TargetProjects/
  lens-dev/new-codebase/
    lens.core.src/                  # PRIMARY SOURCE — lens-work module development
      _bmad/lens-work/              # Edit here, not in lens.core/
    lens.core.ghactions/            # GitHub Actions source
    lens.core.release/              # Release packaging
  lens/
    lens-governance/                # Governance repo (constitutions, feature index, artifacts)
docs/
  lens-dev/new-codebase/            # All planning docs for active features
    lens-dev-new-codebase-<name>/   # One folder per feature
.lens/
  personal/context.yaml             # Active feature context (domain + service) — not committed
  governance-setup.yaml             # Governance repo path config
setup.py                            # Local workspace bootstrap script
```

## Active Context

Current active context (`.lens/personal/context.yaml`):
- **domain**: `lens-dev`
- **service**: `new-codebase`

Use `/lens-switch` to change the active feature context.

## Lens Lifecycle Phases

**Full track**: `preplan → businessplan → techplan → finalizeplan → dev → complete`  
**Express track**: `expressplan → finalizeplan → dev → complete`

Each phase produces docs in `docs/lens-dev/new-codebase/<feature-folder>/`. See [lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml) for full phase definitions.

## Feature Folder & Artifact Conventions

- Feature doc folders: `lens-dev-new-codebase-<featureId>/`
- Planning artifacts use YAML frontmatter with `status: approved|draft|reviewed`
- `sprint-status.yaml` is a **single YAML document** (not a multi-doc stream)
- ExpressPlan review report: `expressplan-adversarial-review.md` (legacy alias: `expressplan-review.md`)
- PrePlan review: `preplan-adversarial-review.md`; TechPlan review: `techplan-adversarial-review.md`
- FinalizePlan review: `finalizeplan-review.md`

## Making Changes to the Lens Module

1. Edit files in `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/`
2. Commit and push to the source repo (branch strategy: `feature/<featureStub>`)
3. Merge to `main` or `develop` — the [promote-to-release workflow](.github/workflows/promote-to-release.yml) builds the full BMAD release and pushes to Lens.Core.Release

## Key Reference Files

- [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml) — phase definitions and artifact contracts
- [lens.core/_bmad/lens-work/bmadconfig.yaml](lens.core/_bmad/lens-work/bmadconfig.yaml) — module configuration and topology
- [lens.core/_bmad/lens-work/module-help.csv](lens.core/_bmad/lens-work/module-help.csv) — full Lens command catalog
- [lens.core/_bmad/lens-work/README.md](lens.core/_bmad/lens-work/README.md) — workbench overview
- [docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/research.md](docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/research.md) — baseline lifecycle contract reference

## Common Terminal Errors & Fixes

> When a recurring terminal error is encountered, record the fix here.

<!-- Example format:
**Error**: `<error message>`  
**Cause**: `<root cause>`  
**Fix**: `<resolution>`
-->

**Error**: `rg: command not found` (or similar)
**Cause**: `ripgrep (rg) is not installed in this environment`
**Fix**: Use `grep` instead of `rg` for all text searches

**Error**: Prompt files contain literal `\r\n` text after bulk replace
**Cause**: PowerShell heredoc replacement does not expand `\r\n` as newlines
**Fix**: Use Python for multi-file text replacement. Never use PowerShell `-Command` regex replacements for prompt files. Example:
```python
from pathlib import Path

for p in Path(".github/prompts").glob("*.prompt.md"):
  content = p.read_text(encoding="utf-8")
  content = content.replace("OLD", "NEW")
  p.write_text(content, encoding="utf-8")
```

**Error**: `gh pr create` fails with no common ancestor / no shared history
**Cause**: Branch was created from `develop` (or another non-main base) but `--base main` was passed
**Fix**: Use the merge-base timestamp comparison in `create-pr` of `git-orchestration-ops.py`. Do not call `gh pr create --base main` directly.
