---
feature: lens-dev-new-codebase-expressplan
doc_type: story-file
story_id: 3.1
status: ready-for-dev
goal: "Hand off expressplan into FinalizePlan-owned bundle generation without duplicating downstream logic"
depends_on:
  - 2.2
updated_at: 2026-04-27T22:50:00Z
---

# Story 3.1 - FinalizePlan Handoff and Phase Advance

## User Story

As a maintainer, I want expressplan to reuse FinalizePlan for the downstream planning bundle so only one implementation owns epics, stories, readiness, sprint status, and story files.

## Acceptance Criteria

- On pass or pass-with-warnings, expressplan signals handoff into FinalizePlan.
- Expressplan does not generate downstream bundle artifacts directly.
- Auto-advance messaging stays aligned with lifecycle metadata.
- Any phase mutation remains behind existing Lens state tooling rather than ad hoc file edits.

## Implementation Notes

- The handoff seam is more important than cosmetic messaging.
- Keep governance publication and PR topology in the FinalizePlan or shared script boundary.
- Make the separation obvious enough that a future maintainer cannot merge the flows accidentally.

## Validation Targets

- Handoff contract test
- No duplicate bundle-generation path
- Auto-advance message check