---
feature: lens-dev-new-codebase-complete
doc_type: story-file
story_id: CP-9
status: ready-for-dev
goal: "Ensure bmad-lens-document-project completes before finalize runs so the archive includes fresh documentation"
depends_on: [CP-8]
updated_at: 2026-04-27T22:55:00Z
---

# Story CP-9 - Preserve Document-Before-Archive Ordering

## User Story

As a feature author, I want documentation captured before archival so my archive record reflects what was actually built and not just the planning state.

## Acceptance Criteria

- The conductor skill flow explicitly positions `bmad-lens-document-project` after `retrospective` and before `finalize`.
- If `document-project` errors or is skipped, the skill requires an explicit confirmation (with clear explanation) before proceeding to `finalize`.
- The archive summary (`summary.md`) records whether documentation was completed or explicitly skipped.
- `finalize` does not call `document-project` internally — the delegation is handled at the conductor skill level.

## Implementation Notes

- Cross-reference with CP-8: the full expected order is `check-preconditions → confirm → retrospective → document-project → finalize`.
- Reference `bmad-lens-document-project` SKILL.md for expected delegation contract.
- Document-project completion should be verified (ideally via a result field or artifact existence) before finalize is invoked.

## Validation Targets

- Skill flow spec or test confirms document-project precedes finalize.
- Skill spec confirms a skipped documentation step is recorded in the archive summary.
- `bmad-lens-document-project` delegation is referenced, not inlined.
