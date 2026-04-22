---
feature: lens-dev-new-codebase-baseline
story_id: "2.5"
story_key: "2-5-rewrite-switch"
epic: "2"
title: "Rewrite switch Command"
status: re-evaluate
priority: must
story_points: 3
depends_on: [2-4-rewrite-new-feature]
updated_at: 2026-04-22T00:00:00Z
---

# Story 2.5: Rewrite switch Command

Status: re-evaluate

## Story

As a Lens user,
I want switch to update active feature context without mutating lifecycle state,
so that moving between features is safe.

## Acceptance Criteria

1. Session context changes to the selected feature.
2. No governance or lifecycle artifact is written during a switch.
3. Invalid feature selection fails cleanly.

## Tasks / Subtasks

- [ ] Rebuild switch around feature-index driven context loading.
  - [ ] Preserve read-only behavior end to end.
- [ ] Surface missing-feature errors clearly.
  - [ ] Avoid silent fallback behavior.
- [ ] Preserve the no-write regression anchor.
  - [ ] Verify feature.yaml and feature-index.yaml stay untouched.

## Dev Notes

- switch must remain a pure context-selection command.
- Preserve related-doc loading without adding lifecycle mutation.
- Keep compatibility with later next/dev routing.

### References

- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/epics.md
- TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/dependency-mapping.md
- TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/component-inventory.md

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
