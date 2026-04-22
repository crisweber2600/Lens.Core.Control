---
feature: lens-dev-new-codebase-baseline
story_id: "1.3"
story_key: "1-3-batch-two-pass-contract"
epic: "1"
title: "Implement bmad-lens-batch Shared 2-Pass Contract"
status: re-evaluate
priority: must
story_points: 5
depends_on: [1-1-scaffold-published-surface]
updated_at: 2026-04-22T00:00:00Z
---

# Story 1.3: Implement bmad-lens-batch Shared 2-Pass Contract

Status: re-evaluate

## Story

As a lens-work maintainer,
I want a shared two-pass batch contract,
so that planning conductors handle intake and resume behavior consistently.

## Acceptance Criteria

1. Planning conductors delegate batch intake and resume behavior to bmad-lens-batch.
2. Pass 1 collects the required artifact inputs in order without mutation.
3. Pass 2 invokes downstream skills sequentially and stops cleanly on first failure.

## Tasks / Subtasks

- [ ] Normalize the shared batch input contract for all planning conductors.
  - [ ] Keep batch input files under the feature docs path.
- [ ] Implement ordered pass-1 collection and pass-2 execution semantics.
  - [ ] Halt on first downstream failure with a clear result.
- [ ] Preserve wrapper-equivalence coverage.
  - [ ] Confirm each planning conductor now uses the shared contract instead of copied logic.

## Dev Notes

- The batch contract is a lifecycle utility, not phase-specific behavior.
- Preserve the existing pass-1/pass-2 semantics documented in the retained planning flow.
- Keep feature docs path authority intact during resume.

### References

- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/epics.md
- lens.core/_bmad/lens-work/skills/bmad-lens-batch/SKILL.md
- TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/dependency-mapping.md

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
