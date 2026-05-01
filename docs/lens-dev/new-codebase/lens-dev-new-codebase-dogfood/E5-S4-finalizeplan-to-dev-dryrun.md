---
feature: lens-dev-new-codebase-dogfood
epic: 5
story_id: E5-S4
sprint_story_id: S5.4
title: Dry-run FinalizePlan to Dev phase transition
type: confirm
points: M
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E4-S1, E4-S2, E4-S3, E5-S3]
blocks: [E5-S5]
target_repo: lens.core.src
target_branch: develop
---

# E5-S4 — Dry-run FinalizePlan to Dev phase transition

## Context

A complete dry-run of the FinalizePlan → Dev transition on a test feature verifies that: the Dev conductor (E4-S1) enters correctly, target-repo branch prep (E4-S3) runs, a mock story executes end-to-end, dev-session.yaml is created, and sprint-status is updated. This is the final integration verification before parity reporting.

## Implementation Steps

1. Create a scratch feature `lens-dev-new-codebase-dogfood-dryrun-2` in the control repo (or a throwaway branch).
2. Seed a minimal sprint-status.yaml with one story.
3. Run FinalizePlan → Dev conductor → story execution (mock dev agent).
4. Capture log output and confirm: Dev entry, branch prep, dev-session.yaml created, sprint-status updated.
5. Commit dry-run results to `docs/lens-dev/new-codebase/lens-dev-new-codebase-dogfood/dryrun-finalizeplan-to-dev.md`.
6. Delete throwaway branch after report committed.

## Acceptance Criteria

- [ ] Dev conductor enters from `finalizeplan-complete` feature.yaml phase.
- [ ] Target-repo branch prep runs and completes.
- [ ] Mock story execution creates story-scoped `dev-session.yaml`.
- [ ] Sprint-status updated after mock story.
- [ ] Dry-run results committed to `dryrun-finalizeplan-to-dev.md`.
- [ ] Throwaway branch deleted after report committed.

## Dev Notes

- Reference: tech-plan Dev phase contract, E4-S1, E4-S3.

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
