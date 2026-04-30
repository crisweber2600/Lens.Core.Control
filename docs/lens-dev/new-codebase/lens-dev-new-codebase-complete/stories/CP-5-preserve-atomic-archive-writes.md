---
feature: lens-dev-new-codebase-complete
doc_type: story-file
story_id: CP-5
status: blocked-by-CP-7
goal: "Restore atomic archive writes to feature.yaml, feature-index.yaml, and summary.md as a consistent change set"
depends_on: [CP-4, CP-7]
updated_at: 2026-04-27T22:55:00Z
---

# Story CP-5 - Preserve Atomic Archive Writes

## User Story

As a governance owner, I want `finalize` to write the archive record atomically so a partial failure cannot leave the registry in an inconsistent state.

## Acceptance Criteria

- `finalize` updates `feature.yaml` (phase: complete), `feature-index.yaml` (status: archived), and `summary.md` as a consistent change set.
- If any of the three file writes fails, the failure is surfaced explicitly and the partial state is clearly documented in the output (not silently swallowed).
- `finalize` does NOT perform any Git operations (no branch cleanup, no commit, no push).
- `summary.md` is the only archive summary filename used — no `final-summary.md`.
- **Gate:** CP-7 (summary naming audit) must pass before this story can be accepted.

## Implementation Notes

- Write order: `feature.yaml` first, then `feature-index.yaml`, then `summary.md`. Surface failures at each step.
- Use the existing YAML write patterns from `feature-yaml-ops.py` or equivalent — do not invent new serializers.
- Reference `lens-dev-new-codebase-new-service` and `lens-dev-new-codebase-switch` governance dirs for expected output shape.
- Branch cleanup is explicitly excluded from the `finalize` subcommand.

## Validation Targets

- `test-complete-ops.py::test_finalize_writes_all_three_files` passes.
- `test-complete-ops.py::test_finalize_no_partial_write_on_failure` passes.
- `test-complete-ops.py::test_finalize_no_git_ops` passes.
- CP-7 audit completed: no `final-summary.md` references remain.
