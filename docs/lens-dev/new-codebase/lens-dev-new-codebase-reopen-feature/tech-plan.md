---
feature: lens-dev-new-codebase-reopen-feature
doc_type: tech-plan
status: approved
goal: "Implement terminal-feature reopen support in lens-feature-yaml with strict guards and index synchronization."
key_decisions:
  - Implement as a new `reopen` subcommand in feature-yaml-ops.py.
  - Validate target phase against track phase set and reject terminal reopen targets.
  - Append a phase transition record with actor and timestamp for auditability.
  - Clear `completed_at` and normalize status from archived/complete to active.
depends_on:
  - business-plan.md
  - TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-feature-yaml/scripts/tests/test-feature-yaml-ops.py
blocks: []
updated_at: '2026-05-08T00:00:00Z'
---

# Tech Plan - Reopen Operation

## Files

- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-feature-yaml/scripts/feature-yaml-ops.py`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-feature-yaml/scripts/tests/test-feature-yaml-ops.py`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-feature-yaml/SKILL.md`

## Command Contract

Add subcommand:

- `reopen --feature-id/--feature-path --governance-repo --to-phase <phase=expressplan> [--actor <label>]`

Behavior:

1. Resolve feature and governance paths via existing helpers.
2. Allow reopen only when phase/status is terminal (`complete`/`archived`).
- Validate `--to-phase` is valid for track (treat null/missing track as bypass-allowed with warning) and non-terminal.
4. Update `feature.yaml`:
   - `phase = to-phase`
   - `status = active` when previously archived/complete
   - remove `completed_at` (always present for terminal features; removing unconditionally is safe)
   - append `phase_transitions` entry
5. Sync `feature-index.yaml` via existing sync helper.

## Validation

- Success test: complete+archived feature reopens to expressplan and index status is set to `active`.
- Guard test: non-terminal feature returns `reopen_not_allowed`.
- Index sync assertion: after reopen, `feature-index.yaml` status field equals `active` (not just that sync ran).
- Use a temp-dir fake `feature.yaml` fixture so tests do not depend on any live feature being archived.
