---
feature: lens-dev-new-codebase-complete
doc_type: story-file
story_id: CP-6
status: ready-for-dev
goal: "Restore the read-only archive-status query for terminal-state recognition without any writes"
depends_on: [CP-4]
updated_at: 2026-04-27T22:55:00Z
---

# Story CP-6 - Preserve Archive-Status Query

## User Story

As a downstream command owner, I want `archive-status` to let me check whether a feature is archived without mutating anything so readers can stay safe.

## Acceptance Criteria

- `archive-status` reads `feature.yaml` and `feature-index.yaml` and returns a structured result (JSON or YAML).
- `archive-status` makes no writes to any file under any condition.
- Features with `phase: complete` and `status: archived` are recognized as terminal and reported as archived.
- Features with any other phase/status are reported as not-archived with a clear reason.
- Features that do not exist in the registry return a structured not-found result rather than an unhandled error.

## Implementation Notes

- This is a read-only subcommand — never accept file paths as output targets.
- Structured output format should be consistent with `check-preconditions` for composability.
- Reference the existing archived features (`new-service`, `switch`, `new-domain`) for fixture data.

## Validation Targets

- `test-complete-ops.py::test_archive_status_recognizes_complete_phase` passes.
- `test-complete-ops.py::test_archive_status_returns_not_archived_for_active_feature` passes.
- `test-complete-ops.py::test_archive_status_makes_no_writes` passes.
