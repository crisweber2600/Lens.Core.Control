---
feature: lens-dev-new-codebase-complete
doc_type: story-file
story_id: CP-7
status: ready-for-dev
goal: "Audit and eliminate all final-summary.md references to lock summary.md as the only canonical archive summary name"
depends_on: [CP-5]
updated_at: 2026-04-27T22:55:00Z
---

# Story CP-7 - Lock Summary Artifact Naming

## User Story

As a maintainer, I want the summary artifact name to be unambiguous so every reader, test, and help text refers to the same file.

## Acceptance Criteria

- A systematic search finds and resolves every `final-summary.md` reference in: `complete-ops.py`, `test-complete-ops.py`, `bmad-lens-complete/SKILL.md`, any related prompt stubs, and any help text surfaces.
- After the story lands, the only archive summary filename used anywhere in the complete feature implementation is `summary.md`.
- If any `final-summary` reference is found in adjacent features that consume the `complete` command output, it is flagged as a follow-on task (not silently removed from those features' docs).
- This story's completion is required before CP-5 (atomic archive writes) acceptance criteria can be signed off.

## Implementation Notes

- Search scope: all files under `_bmad/lens-work/skills/bmad-lens-complete/`, plus module help CSV, plus any prompt stubs.
- Do not rename `summary.md` files in existing archived governance feature directories — those are historical artifacts already committed.
- Document the reconciliation result: how many references found, what was changed, what was flagged for adjacent features.

## Validation Targets

- Grep for `final-summary` in the complete skill path returns zero matches.
- `test-complete-ops.py` contains no `final-summary` references.
- CP-5 is unblocked: status changes from `blocked-by-CP-7` to `ready-for-dev`.
