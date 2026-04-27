---
feature: lens-dev-new-codebase-complete
doc_type: story-file
story_id: CP-13
status: ready-for-dev
goal: "Capture the implementation handoff notes so future maintainers know exactly what was built and what is explicitly excluded"
depends_on: [CP-12]
updated_at: 2026-04-27T22:55:00Z
---

# Story CP-13 - Prepare Implementation Handoff Notes

## User Story

As a future maintainer, I want a concise handoff document that names all new files, states explicit non-goals, and points to behavioral reference examples so I can maintain the feature without re-reading the full planning history.

## Acceptance Criteria

- A `handoff.md` file (or equivalent section in `summary.md`) is written at story completion listing all new and modified files in the complete feature implementation.
- Non-goals are explicitly named: no branch cleanup, no git operations in `finalize`, no inline retrospective logic in the complete skill.
- The focused test command is documented verbatim: `uv run --with pytest pytest <path>/test-complete-ops.py -q`.
- Behavioral reference examples are cited: `lens-dev-new-codebase-new-service` and `lens-dev-new-codebase-switch` are named as reference governance dirs.
- CP-7 reconciliation result is summarized (how many `final-summary.md` references were found and resolved).
- Any follow-on issues discovered in CP-12 are listed.

## Implementation Notes

- This story is purely documentation — no code changes.
- Use `bmad-lens-document-project` as the delegation pattern for the handoff notes, not inline skill logic.
- Reference the finalized sprint-status.yaml to confirm all CP stories are at `done` before this story can close.

## Validation Targets

- `handoff.md` (or equivalent) exists in the feature docs directory.
- All explicit non-goals are documented.
- Focused test command is correct and runnable.
- Sprint-status shows all CP stories at `done` before handoff is marked complete.
