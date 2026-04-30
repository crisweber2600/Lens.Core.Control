---
feature: lens-dev-new-codebase-complete
doc_type: story-file
story_id: CP-12
status: ready-for-dev
goal: "Check that adjacent commands and archive state readers still work correctly after the complete rewrite"
depends_on: [CP-11]
updated_at: 2026-04-27T22:55:00Z
---

# Story CP-12 - Run Adjacent Command Checks

## User Story

As a feature operator, I want to know that the complete implementation has not broken adjacent commands that read archive state so I can rely on the full command surface.

## Acceptance Criteria

- `bmad-lens-retrospective` SKILL.md or its delegation contract loads without errors and can be invoked from the complete conductor skill.
- `bmad-lens-document-project` SKILL.md or its delegation contract loads without errors and can be invoked from the complete conductor skill.
- `archive-status` correctly reads governance state written by `finalize` in the focused regression suite.
- Any command that reads `feature.yaml` for phase/status still handles the `complete` phase gracefully.
- Smoke check is documented in the handoff notes (CP-13).

## Implementation Notes

- This is a smoke-check story, not a full regression of adjacent skills.
- The goal is to surface breakage, not to add coverage for adjacent command internals.
- If a real breakage is found, file a follow-on ticket and document the finding in CP-13.

## Validation Targets

- Manual smoke run of `bmad-lens-retrospective` invocation path succeeds (no load error).
- Manual smoke run of `bmad-lens-document-project` invocation path succeeds (no load error).
- `archive-status` integration test against `finalize` output passes.
