---
feature: lens-dev-new-codebase-complete
doc_type: story-file
story_id: CP-11
status: ready-for-dev
goal: "Validate the focused complete regression suite passes end-to-end with no unintended test failures"
depends_on: [CP-10, CP-7]
updated_at: 2026-04-27T22:55:00Z
---

# Story CP-11 - Run Focused Complete Regressions

## User Story

As a maintainer, I want a single focused test command I can run to verify the complete implementation is stable so I can confidently promote it.

## Acceptance Criteria

- `uv run --with pytest pytest <path>/test-complete-ops.py -q` exits 0 from the source tree root.
- All tests implemented in CP-1 through CP-10 pass.
- No test passes via a mock that bypasses the assertion it was intended to verify.
- The focused test command is documented in `stories/CP-13-prepare-implementation-handoff-notes.md`.

## Implementation Notes

- This story is an integration checkpoint, not an implementation story. Run the suite, fix any failures surfaced by the full run.
- If a test reveals a gap from a prior story, the fix belongs in that story's scope and should be noted.
- Do not expand the test file with new coverage beyond what was specified in CP-1 through CP-10.

## Validation Targets

- `uv run --with pytest pytest <path>/test-complete-ops.py -q` exits 0.
- Test output shows all CP-specific test names passing.
- No skipped tests that were not explicitly approved in prior stories.
