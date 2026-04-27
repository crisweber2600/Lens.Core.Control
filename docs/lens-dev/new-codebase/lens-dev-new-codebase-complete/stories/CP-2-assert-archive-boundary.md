---
feature: lens-dev-new-codebase-complete
doc_type: story-file
story_id: CP-2
status: ready-for-dev
goal: "Prove that complete does not invent new lifecycle schemas or files beyond the three canonical archive outputs"
depends_on: [CP-1]
updated_at: 2026-04-27T22:55:00Z
---

# Story CP-2 - Assert Archive Boundary

## User Story

As a governance owner, I want the test suite to prove complete does not widen the archive mutation surface so future contributions stay bounded.

## Acceptance Criteria

- Tests assert `finalize` writes only `feature.yaml`, `feature-index.yaml`, and `summary.md` — no additional files.
- Tests assert no new YAML keys are introduced in `feature.yaml` or `feature-index.yaml` beyond existing fields.
- Tests assert `finalize` does not call any Git commands (no branch delete, no commit, no push).
- These boundary assertions run as part of the normal focused test pass alongside CP-1 tests.

## Implementation Notes

- Use a fixture-backed governance directory for all tests — no real repo writes.
- Keep assertions narrow: only check for unexpected file creation, not for exact file content (that is CP-5's job).
- Branch cleanup is explicitly out of scope for the archive script.

## Validation Targets

- `test-complete-ops.py` boundary assertion tests pass.
- No test mocks out file-write assertions to make them pass — the assertions must be real.
