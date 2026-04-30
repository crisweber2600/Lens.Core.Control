---
feature: lens-dev-new-codebase-complete
doc_type: story-file
story_id: CP-1
status: ready-for-dev
goal: "Lock the observable complete CLI contract before any archive mutations widen"
depends_on: []
updated_at: 2026-04-27T22:55:00Z
---

# Story CP-1 - Define Complete CLI Contract Tests

## User Story

As a maintainer, I want a focused regression harness locked before implementation so the archive contract cannot silently change.

## Acceptance Criteria

- `test-complete-ops.py` (or equivalent) exists in the skills/scripts/tests path with test stubs covering `check-preconditions`, `finalize`, `archive-status`, and dry-run behavior.
- All test stubs are runnable via `uv run --with pytest pytest <path>/test-complete-ops.py -q` from the source tree root.
- Tests use fixture data, not real governance repo state.
- The test file is committed to the source tree before any implementation story starts writing archive logic.

## Implementation Notes

- Mirror the pattern used by `test-git-orchestration-ops.py` for fixture-backed CLI testing.
- Red-phase stubs are acceptable for CP-1 — they just need to exist and be runnable.
- Do not write production code in CP-1; this story is test-first contract locking only.

## Validation Targets

- `uv run --with pytest pytest <path>/test-complete-ops.py -q` exits 0 (even if tests are currently failing red-phase).
- Test file contains at least one test per subcommand: `check-preconditions`, `finalize`, `archive-status`.
