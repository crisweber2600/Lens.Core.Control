---
feature: lens-dev-new-codebase-baseline
story_id: "4.5"
story_key: "4-5-rewrite-expressplan"
epic: "4"
title: "Rewrite expressplan Command"
status: re-evaluate
priority: must
story_points: 5
depends_on: [1-3-batch-two-pass-contract, 3-1-fix-constitution-partial-hierarchy, 4-4-rewrite-finalizeplan]
updated_at: 2026-04-22T00:00:00Z
---

# Story 4.5: Rewrite expressplan Command

Status: re-evaluate

## Story

As a Lens user,
I want expressplan to run only for express-eligible features, delegate to QuickPlan, and stop on an adversarial-review failure,
so that compressed planning stays controlled.

## Acceptance Criteria

1. Non-express features are blocked from entering expressplan.
2. Express-eligible features delegate to bmad-lens-quickplan.
3. The hard-stop adversarial review completes before finalize bundling.

## Tasks / Subtasks

- [ ] Preserve express-eligibility gating.
  - [ ] Keep non-express tracks on the full planning path.
- [ ] Preserve delegation to the internal QuickPlan skill.
  - [ ] Avoid replacing it with a planning conductor rewrite.
- [ ] Preserve hard-stop adversarial review behavior.
  - [ ] Do not allow finalize bundling after a failed review.

## Dev Notes

- This workflow is defined by routing and gating, not by new document structure.
- Preserve QuickPlan as an internal dependency even if public surfaces change.
- Use the old dependency map to confirm expressplan still delegates through QuickPlan.

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
