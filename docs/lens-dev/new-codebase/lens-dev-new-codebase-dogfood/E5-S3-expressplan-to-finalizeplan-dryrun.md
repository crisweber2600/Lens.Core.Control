---
feature: lens-dev-new-codebase-dogfood
epic: 5
story_id: E5-S3
sprint_story_id: S5.3
title: Dry-run ExpressPlan to FinalizePlan phase transition
type: confirm
points: M
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E2-S6, E5-S2]
blocks: [E5-S4, E5-S5]
target_repo: lens.core.src
target_branch: develop
---

# E5-S3 — Dry-run ExpressPlan to FinalizePlan phase transition

## Context

A complete dry-run of the ExpressPlan → FinalizePlan transition on a test feature verifies that: artifact routing is correct, publish artifacts include the full set (E2-S6), Windows paths normalize (E2-S6), plan PR creation and merge sequence works, and the FinalizePlan step 3 bundle is written to the correct branch.

## Implementation Steps

1. Create a scratch feature `lens-dev-new-codebase-dogfood-dryrun-1` in the control repo (or a throwaway branch).
2. Run ExpressPlan artifact creation → publish → plan PR merge → FinalizePlan entry → step 3 bundle.
3. At each step, capture log output and confirm it matches the expected routing and artifact checklist.
4. Commit dry-run results to `docs/lens-dev/new-codebase/lens-dev-new-codebase-dogfood/dryrun-expressplan-to-finalizeplan.md`.
5. Delete throwaway branch after report committed.

## Acceptance Criteria

- [ ] Full ExpressPlan → FinalizePlan transition completes without error on a test feature.
- [ ] Artifact routing confirmed: each artifact on the correct branch.
- [ ] Publish includes all four ExpressPlan artifact types (E2-S6 AC).
- [ ] Windows path normalization applied without failures.
- [ ] Dry-run results committed to `dryrun-expressplan-to-finalizeplan.md`.
- [ ] Throwaway branch deleted after report committed.

## Dev Notes

- Reference: tech-plan phase contracts, E2-S6, E2-S2.

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
