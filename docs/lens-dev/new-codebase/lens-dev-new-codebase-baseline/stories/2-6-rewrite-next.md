---
feature: lens-dev-new-codebase-baseline
story_id: "2.6"
story_key: "2-6-rewrite-next"
epic: "2"
title: "Rewrite next Command"
status: re-evaluate
priority: must
story_points: 5
depends_on: [2-1-rewrite-preflight, 2-4-rewrite-new-feature, 2-5-rewrite-switch]
updated_at: 2026-04-22T00:00:00Z
---

# Story 2.6: Rewrite next Command

Status: re-evaluate

## Story

As a Lens user,
I want next to identify the one unblocked action and auto-delegate without redundant confirmation,
so that the lifecycle feels intentional and efficient.

## Acceptance Criteria

1. Exactly one unblocked next action is selected.
2. Delegation is pre-confirmed and does not re-ask to proceed.
3. Blockers stop delegation and are surfaced clearly.

## Tasks / Subtasks

- [ ] Preserve blocker-first routing logic.
  - [ ] Keep routing grounded in lifecycle.yaml and feature state.
- [ ] Preserve pre-confirmed handoff behavior.
  - [ ] Prevent delegated phase skills from re-asking the launch question.
- [ ] Keep the focused regression anchors green.
  - [ ] Verify single-choice routing and blocker handling.

## Dev Notes

- This story depends on stable identity and context-loading behavior.
- Avoid turning next into a menu or multi-choice recommender.
- Preserve the exact handoff contract that /next implies consent to enter the delegated phase.

### References

- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/epics.md
- TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/dependency-mapping.md
- lens.core/_bmad/lens-work/skills/bmad-lens-next

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
