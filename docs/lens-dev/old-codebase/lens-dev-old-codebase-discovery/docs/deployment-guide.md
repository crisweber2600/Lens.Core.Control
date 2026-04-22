# lens.core.src — Deployment Guide

**Feature:** lens-dev-old-codebase-discovery
**Generated:** 2026-04-21

---

## Overview

`lens.core.src` is the **upstream source** repository for the LENS Workbench module. This guide describes how changes authored in the source are validated, promoted, and distributed to consumers through the release pipeline.

The source does not deploy itself — it promotes to a release distribution repo (`lens.core.release`) that consumers install from.

---

## Distribution Model

```
lens.core.src  (this repo — source of truth)
       │
       │  CI: parity check + registry validation
       │  Merge to main
       ▼
lens.core.release  (release distribution)
       │  beta branch = stable release
       │
       ├──► Consumer control repos  (via install.py)
       │        └── .github/, .claude/, .codex/, .cursor/
       │            adapters installed in each control repo
       │
       └──► Direct consumers
                └── git pull origin beta  (pulls latest stable release)
```

---

## Versioning

| Field | Location | When to Increment |
| --- | --- | --- |
| `module_version` | `_bmad/lens-work/module.yaml` | Any change to skills, prompts, scripts, or docs |
| `schema` | `_bmad/lens-work/module.yaml` | Breaking changes to `lifecycle.yaml` schema or `feature.yaml` shape |
| `lifecycle_version` | `_bmad/lens-work/lifecycle.yaml` | Changes to the lifecycle phase contract |

Versioning follows semantic intent:
- Non-breaking additions → increment minor part of `module_version`
- Breaking schema changes → increment `schema` AND write a `migrate-ops.py` migration path

---

## Release Branches

| Branch | Status | Description |
| --- | --- | --- |
| `main` | Stable source | All PRs merge here; parity check required |
| `{featureId}-plan` | In-flight | Planning artifacts for in-progress features |
| `{featureId}` | In-flight | Base for a given feature |

The release distribution has its own branch model:

| Branch (in `lens.core.release`) | Description |
| --- | --- |
| `beta` | Stable release — consumers pull from here |
| `main` | Release candidate — merged from source promotion |

---

## Pre-Promotion Checklist

Before a source change is promoted to release, the following checks must pass:

```bash
cd TargetProjects/lens-dev/old-codebase/lens.core.src

# 1. Registry validation — skills and prompts in module.yaml match files on disk
uv run _bmad/lens-work/scripts/validate-lens-bmad-registry.py

# 2. Full test suite
uv run --with pytest pytest _bmad/lens-work/scripts/tests -q

# 3. Parity check — source and release are in sync
bash _bmad/lens-work/assets/lens-bmad-parity-check.sh

# 4. Dry-run installer — verify adapter generation still works
uv run _bmad/lens-work/scripts/install.py --dry-run
```

All four checks run automatically in CI on PRs to `main`.

---

## Promotion Process

The promotion pipeline runs after a successful merge to `main` in `lens.core.src`:

### Step 1 — CI Validation

GitHub Actions workflow runs:
- `validate-lens-bmad-registry.py` — registry consistency
- `pytest _bmad/lens-work/scripts/tests -q` — full test suite
- `validate-lens-core-parity.sh` — source/release parity

Failure at any step blocks promotion.

### Step 2 — Sync to Release

The promotion job:
1. Checks out `lens.core.release`
2. Syncs `_bmad/lens-work/` content from `lens.core.src`
3. Runs `install.py` to regenerate bundled adapter stubs (`.github/`, `.claude/`, `.codex/`, `.cursor/`)
4. Bundles optional BMAD dependencies (`core`, and any configured optional modules)
5. Commits the sync to `lens.core.release` `main`

### Step 3 — Stable Release

After promotion to `lens.core.release` `main`:
1. QA check: spot-verification that adapters install correctly into a test control repo
2. If QA passes: merge or fast-forward `main` → `beta` in `lens.core.release`

The `beta` branch is the consumer-facing stable release.

### Step 4 — Consumer Update

Consumers update by pulling:

```bash
git -C TargetProjects/lens-dev/release/lens.core.release pull origin beta
```

Or reinstalling adapters if the IDE surfaces changed:

```bash
cd TargetProjects/lens-dev/release/lens.core.release
uv run _bmad/lens-work/scripts/install.py --ide github-copilot --update
```

---

## Schema Migration Releases

When a breaking schema change is released:

1. `schema` in `module.yaml` is incremented
2. A `migrate-ops.py` migration path is added declaring the old → new schema transition
3. Consumers run `/lens-migrate` (or `uv run _bmad/lens-work/skills/bmad-lens-migrate/scripts/migrate-ops.py --dry-run`) to plan migration before applying

See `_bmad/lens-work/lifecycle.yaml` for the `migrations:` block, which declares all schema migration paths.

---

## Rollback

If a release introduces a regression:

1. Identify the last good commit on `lens.core.release` `beta`
2. Revert `beta` to that commit: `git -C lens.core.release revert HEAD`
3. Consumers re-pull `beta`

For `lens.core.src` source rollback:
- Use standard git revert on `main`
- Re-run the promotion pipeline

---

## Adapter Installation (Consumer Side)

When a consumer control repo installs or updates the Lens module:

```bash
cd TargetProjects/lens-dev/release/lens.core.release

# Fresh install (one IDE)
uv run _bmad/lens-work/scripts/install.py --ide github-copilot

# Update existing install
uv run _bmad/lens-work/scripts/install.py --ide github-copilot --update

# Dry-run (see what would be installed)
uv run _bmad/lens-work/scripts/install.py --dry-run
```

The installer generates stubs in `.github/prompts/`, `.github/instructions/`, `.github/skills/`, etc. These stubs point back to the module's canonical `SKILL.md` files.

---

_Generated by lens-bmad-document-project for feature lens-dev-old-codebase-discovery._
