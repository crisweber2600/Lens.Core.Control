---
feature: lens-dev-new-codebase-split-feature
epic: 2
story_id: E2-S1
title: Rewrite SKILL.md as thin conductor (BMB-first)
type: rewrite
points: 3
status: ready
phase: dev
updated_at: '2026-05-01T00:00:00Z'
depends_on: []
blocks: [E4-S2]
target_repo: lens.core.src
target_branch: develop
---

# E2-S1 — Rewrite SKILL.md as thin conductor (BMB-first)

## Context

The current `SKILL.md` (110 lines) delegates to `./references/` sub-documents for the
three main workflows (Validate Split, Split Scope, Split Stories). Per the baseline
architecture contract and tech-plan §2.1–§2.2, the new-codebase SKILL.md must be a
thin conductor where:
- All governance mutations are delegated to `split-feature-ops.py`
- No inline file writes exist anywhere in the SKILL.md
- The execution flow is explicit: load config → validate → confirm → dry-run → execute → report

The rewrite also needs to add two notes that were missing from the old-codebase version:
1. **Post-move scan note (L3):** After `move-stories` completes, the Lens agent should
   scan moved story files and report any with `feature:` frontmatter still pointing to
   the source feature ID
2. **Behavioral reference path (BS-2):** The old-codebase implementation is at
   `TargetProjects/lens-dev/old-codebase/lens.core.src/`

**Implementation channel:** BMB-first — use `bmad-module-builder` skill for SKILL.md rewrite.

**File:** `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/SKILL.md`

**Old-codebase reference:** `TargetProjects/lens-dev/old-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-split-feature/SKILL.md`

## Implementation Steps

### 1. Load old-codebase SKILL.md as behavioral reference

Read `TargetProjects/lens-dev/old-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-split-feature/SKILL.md`
and `./references/validate-split.md`, `./references/split-scope.md`, `./references/split-stories.md`
to understand the current behavior surface.

### 2. Rewrite SKILL.md following thin-conductor pattern

The new SKILL.md must include these sections:

**Identity:** "You are the split-feature thin conductor. You validate first, create then
move. You delegate all governance mutations to `split-feature-ops.py`."

**Principles (must retain):**
- `validate-first` — validate-split must pass before any create or move
- `new-feature-first-class` — complete governance setup for the new feature
- `atomic-split` — create before modify
- `user-decisions-required` — confirm split boundary before execution
- `dry-run-before-live` — show dry-run plan before any live execution

**On Activation:**
- Load config from `{project-root}/lens.core/_bmad/bmadconfig.yaml` and
  `{project-root}/lens.core/_bmad/config.user.yaml`
- Resolve `{governance_repo}` and `{username}`
- Load source feature context via `bmad-lens-feature-yaml`

**Execution Flow (thin conductor):**
```
[entry]
  → Load config + feature context
  → Prompt: split mode? [validate-only | scope | stories]
  → Run validate-split (hard gate — blocks if any story in-progress)
      ├── blocked → hard-stop; list blocked story IDs; no workaround
      └── pass → show split plan (both sides); wait for user confirm
  → [if scope] dry-run create-split-feature → confirm → execute
  → [if stories] dry-run create-split-feature → confirm → execute
                  dry-run move-stories → confirm → execute
                  → post-move scan: report any feature: frontmatter still pointing to source
  → Report: new feature path, modified files, moved stories (if any)
[done]
```

**Post-Move Scan Note (L3):** After `move-stories` completes, the Lens agent must
scan all moved story files and report any files whose `feature:` frontmatter field
still references the source feature ID. No automatic rewrite — surface results to
the user for manual resolution.

**Behavioral Reference Note (BS-2):** The old-codebase implementation is located at
`TargetProjects/lens-dev/old-codebase/lens.core.src/` in the control repo workspace.
The governance-registered source for old-codebase discovery artifacts is the
`lens-dev-old-codebase-discovery` feature. Consult this path for output parity testing.

**Script Reference section:** Retain the existing three-subcommand example block
(validate-split, create-split-feature, move-stories, --dry-run).

**Capabilities table:** Retain references to `./references/validate-split.md`,
`./references/split-scope.md`, `./references/split-stories.md`.

**Integration Points table:** Retain with `bmad-lens-feature-yaml`, `bmad-lens-init-feature`,
`bmad-lens-git-state`.

### 3. Verify no inline governance writes remain

Read the completed SKILL.md and confirm there are no lines that call file-creation
tools or write directly to governance paths. All governance mutations must be invoked
via `split-feature-ops.py` subcommand calls only.

## Acceptance Criteria Checklist

```
[ ] SKILL.md execution flow follows thin-conductor pattern (load → validate → confirm → dry-run → execute → report)
[ ] No inline governance file writes anywhere in SKILL.md
[ ] Post-move scan note present: after move-stories, scan moved files for stale feature: frontmatter
[ ] Behavioral reference path note present: TargetProjects/lens-dev/old-codebase/lens.core.src/
[ ] Dry-run-before-live principle is present (show plan before executing)
[ ] Script Reference section retained with all three subcommand examples
[ ] Capabilities table retained (Validate Split, Split Scope, Split Stories → references)
[ ] Integration Points table retained
[ ] On Activation loads from bmadconfig.yaml + config.user.yaml
```
