---
feature: lens-dev-new-codebase-complete
doc_type: story-file
story_id: CP-8
status: ready-for-dev
goal: "Restore retrospective-first sequencing in the conductor skill so retrospective always precedes archive"
depends_on: [CP-5]
updated_at: 2026-04-27T22:55:00Z
---

# Story CP-8 - Preserve Retrospective-First Orchestration

## User Story

As a feature author, I want the conductor skill to run retrospective before archive so I cannot accidentally archive a feature with no closure narrative.

## Acceptance Criteria

- The skill flow is: `check-preconditions → confirm → retrospective → document-project → finalize`.
- If the user explicitly skips retrospective, the skill requires a separate confirmation for the skip action before proceeding.
- The archive summary (`summary.md`) records whether retrospective was completed or explicitly skipped — it is never silent about the skip.
- The skill calls `check-preconditions` before presenting any confirmation gate to the user.

## Implementation Notes

- Retrospective delegation goes through `bmad-lens-retrospective` — do not inline retrospective logic in the complete skill.
- Keep the skip-retrospective confirmation distinct from the main finalize confirmation.
- The record of a skipped retrospective should appear in a field on `summary.md`, not just in transient skill output.

## Validation Targets

- Skill flow spec or test confirms retrospective precedes finalize.
- Skill spec confirms a skipped retrospective is recorded in the archive summary.
- `bmad-lens-retrospective` delegation is referenced, not inlined.
