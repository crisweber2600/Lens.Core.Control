---
feature: lens-dev-new-codebase-complete
doc_type: story-file
story_id: CP-10
status: ready-for-dev
goal: "Restore the explicit irreversibility confirmation gate so finalize cannot run without operator acknowledgment"
depends_on: [CP-5]
updated_at: 2026-04-27T22:55:00Z
---

# Story CP-10 - Preserve Explicit Confirmation Gate

## User Story

As a feature author, I want a clear irreversibility warning before archive writes so I cannot accidentally close a feature I meant to keep active.

## Acceptance Criteria

- `finalize` displays a warning that the operation is irreversible and lists the three files that will be written.
- `finalize` requires an affirmative response before writing any file.
- In dry-run mode, the confirmation prompt is shown but no writes occur after an affirmative response.
- In non-interactive mode (CI / `--yes` flag), the confirmation requirement is documented in help text, not silently bypassed.
- `test-complete-ops.py::test_finalize_requires_confirmation` passes.

## Implementation Notes

- The confirmation gate belongs at the `finalize` subcommand level, not at the conductor skill level.
- Use `--yes` flag for CI scenarios — never embed blanket bypass in the script logic.
- The done condition for this story is named explicitly: `test-complete-ops.py::test_finalize_requires_confirmation`.

## Validation Targets

- `test-complete-ops.py::test_finalize_requires_confirmation` passes.
- Manual test: running `finalize` without `--yes` waits for input; running with `--yes` proceeds without prompt.
- Dry-run mode shows confirmation output but makes no writes.
