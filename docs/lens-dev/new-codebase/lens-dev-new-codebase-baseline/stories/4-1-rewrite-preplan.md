---
feature: lens-dev-new-codebase-baseline
story_id: "4.1"
story_key: "4-1-rewrite-preplan"
epic: "4"
title: "Rewrite preplan Command"
status: ready-for-dev
priority: must
story_points: 8
depends_on: [1-2-validate-phase-artifacts-shared-utility, 1-3-batch-two-pass-contract, 3-1-fix-constitution-partial-hierarchy]
updated_at: 2026-04-22T00:00:00Z
---

# Story 4.1: Rewrite preplan Command

Status: ready-for-dev

## Story

As a Lens user,
I want preplan to preserve brainstorm-first behavior, shared batch semantics, and the review-ready fast path,
so that early planning behaves exactly as it does today.

## Acceptance Criteria

1. The Epic 3 prerequisite is enforced before work begins.
2. Non-batch runs author brainstorm before research or product brief.
3. Batch and review gates delegate to shared utilities instead of inline logic.

## Tasks / Subtasks

- [ ] Enforce the Story 3.1 prerequisite at workflow entry.
  - [ ] Prevent Epic 4 work from starting against a broken constitution layer.
- [ ] Rebuild preplan orchestration around brainstorm-first ordering.
  - [ ] Preserve non-batch and batch behavior separately.
- [ ] Delegate batch and gate checks to shared utilities.
  - [ ] Keep wrapper-equivalence and phase-gate regressions green.

## Dev Notes

- preplan must remain a thin conductor; downstream BMAD skills own document authorship.
- Preserve prompt-start sync, batch pass semantics, and review-ready routing.
- Use old-codebase dependency mapping to confirm delegation order.

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
