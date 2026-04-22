---
feature: lens-dev-new-codebase-baseline
story_id: "3.1"
story_key: "3-1-fix-constitution-partial-hierarchy"
epic: "3"
title: "Fix Org-Level Constitution Hard-Fail Bug and Add Parity Tests"
status: re-evaluate
priority: must
story_points: 5
depends_on: [1-1-scaffold-published-surface]
updated_at: 2026-04-22T00:00:00Z
---

# Story 3.1: Fix Org-Level Constitution Hard-Fail Bug and Add Parity Tests

Status: re-evaluate

## Story

As a Lens user working without an org-level constitution,
I want constitution resolution to work additively from whatever hierarchy levels are present,
so that partial-hierarchy environments do not fail or block downstream commands.

## Acceptance Criteria

1. Partial-hierarchy environments resolve constitution guidance without crashing.
2. Full hierarchy resolution remains additive and ordered.
3. The command stays read-only across all hierarchy combinations.

## Tasks / Subtasks

- [ ] Fix constitution resolution to tolerate missing org-level files.
  - [ ] Preserve additive merge order for levels that do exist.
- [ ] Preserve read-only behavior in all cases.
  - [ ] Return a no-rules-found result instead of hard-failing when all levels are absent.
- [ ] Add focused partial-hierarchy regression coverage.
  - [ ] Mark this story as the explicit blocker for Epic 4.

## Dev Notes

- Epic 4 may not start until this story is done.
- Keep resolution order explicit and avoid fabricating defaults for missing files.
- Preserve constitutional gate semantics for sensing and planning conductors.

### References

- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/epics.md
- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/architecture.md
- TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/dependency-mapping.md

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
