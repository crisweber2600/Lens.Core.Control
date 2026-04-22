---
feature: lens-dev-new-codebase-baseline
story_id: "5.2"
story_key: "5-2-rewrite-complete"
epic: "5"
title: "Rewrite complete Command"
status: re-evaluate
priority: must
story_points: 5
depends_on: [5-1-rewrite-dev]
updated_at: 2026-04-22T00:00:00Z
---

# Story 5.2: Rewrite complete Command

Status: re-evaluate

## Story

As a Lens user,
I want complete to preserve retrospective, documentation, and archive ordering with terminal archive semantics,
so that closed features stay auditable and immutable.

## Acceptance Criteria

1. Retrospective runs before documentation.
2. Documentation completes before archive.
3. Archive is atomic and prevents future lifecycle mutation.

## Tasks / Subtasks

- [ ] Preserve retrospective-first workflow ordering.
  - [ ] Keep documentation after retrospective and before archive.
- [ ] Preserve terminal archive semantics.
  - [ ] Prevent post-archive lifecycle mutation.
- [ ] Preserve complete/archive atomicity coverage.
  - [ ] Verify archive cannot partially apply.

## Dev Notes

- The closeout flow is ordering-sensitive and should remain simple.
- Keep archive state changes in governance, not local-only session state.
- Preserve the retrospective-before-archive rule from the old lifecycle.

### References

- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/epics.md
- TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/dependency-mapping.md
- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/implementation-readiness.md

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
