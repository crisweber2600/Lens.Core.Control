---
feature: lens-dev-new-codebase-reopen-feature
doc_type: business-plan
status: draft
goal: "Enable a governed reopen workflow so archived/complete features can be safely returned to express planning when work resumes."
key_decisions:
  - Add a sanctioned CLI operation in lens-feature-yaml instead of manual governance edits.
  - Limit reopen to terminal features and require a non-terminal target phase.
  - Default reopen target phase to expressplan for this feature's intended workflow.
  - Keep feature-index synchronization in the same operation so lens-switch visibility is immediately correct.
open_questions:
  - Should a dedicated /lens-reopen command be added later, or is lens-feature-yaml reopen sufficient?
depends_on:
  - TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-feature-yaml/scripts/feature-yaml-ops.py
blocks:
  - Archived features cannot re-enter planning through approved tooling until reopen exists.
updated_at: '2026-05-08T00:00:00Z'
---

# Business Plan - Reopen Archived Features

## Outcome

Lens operators need a safe and auditable way to resume work on features that were finalized and archived. The desired business outcome is restoring planning velocity without bypassing governance boundaries.

## Problem

Current lifecycle tools enforce forward-only transitions. Once a feature reaches `complete`/`archived`, the sanctioned transition path cannot move it back to `expressplan`, forcing ad hoc edits or new feature creation even when the original feature should continue.

## Scope

In scope:
- Add governed reopen operation to `lens-feature-yaml`.
- Keep behavior deterministic and script-backed.
- Ensure `feature-index.yaml` status is unarchived as part of reopen.

Out of scope:
- New UI or interactive menu command.
- Changes to finalize/archive contract.
- Bulk reopen operations.

## Success Criteria

1. Reopen command works for terminal features and updates both `feature.yaml` and `feature-index.yaml`.
2. Reopen command rejects non-terminal features.
3. Reopened features appear in active planning flows by status/phase.
4. Focused regression tests cover success and failure paths.
