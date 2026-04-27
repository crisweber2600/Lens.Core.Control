---
feature: lens-dev-new-codebase-complete
doc_type: story-file
story_id: CP-4
status: ready-for-dev
goal: "Restore completable-phase gating so /complete blocks planning-phase features with clear output"
depends_on: [CP-1]
updated_at: 2026-04-27T22:55:00Z
---

# Story CP-4 - Preserve Completable-Phase Checks

## User Story

As a feature author, I want `check-preconditions` to tell me clearly if my feature is not ready to close so I never archive work that is still in planning.

## Acceptance Criteria

- `check-preconditions` returns `fail` for features in planning phases: `preplan`, `businessplan`, `techplan`, `finalizeplan`, `expressplan`, `expressplan-complete`.
- `check-preconditions` returns `fail` for features already in `complete` or `abandoned` phase.
- `check-preconditions` returns `warn` for features in `dev` or `dev-complete` phase that are missing a retrospective record.
- `check-preconditions` returns `pass` for features in `dev` or `dev-complete` with all preconditions satisfied.
- Each result includes a message that tells the operator what to do next, not just what is wrong.
- Dry-run mode surfaces all precondition issues before any write.

## Implementation Notes

- Implement as a subcommand of `complete-ops.py`.
- Read from `feature.yaml` and the governance feature directory for retrospective evidence.
- Do not write any file in `check-preconditions` mode.

## Validation Targets

- `test-complete-ops.py::test_check_preconditions_blocks_planning_phases` passes.
- `test-complete-ops.py::test_check_preconditions_warns_missing_retrospective` passes.
- `test-complete-ops.py::test_check_preconditions_passes_ready_feature` passes.
