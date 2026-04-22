---
feature: lens-dev-new-codebase-baseline
story_id: "5.5"
story_key: "5-5-rewrite-upgrade-and-regression-gate"
epic: "5"
title: "Rewrite upgrade Command and Run E2E Regression Gate"
status: re-evaluate
priority: must
story_points: 8
depends_on: [2-1-rewrite-preflight, 2-4-rewrite-new-feature, 2-6-rewrite-next, 3-1-fix-constitution-partial-hierarchy, 4-4-rewrite-finalizeplan, 5-1-rewrite-dev, 5-3-rewrite-split-feature, 5-4-rewrite-discover]
updated_at: 2026-04-22T00:00:00Z
---

# Story 5.5: Rewrite upgrade Command and Run E2E Regression Gate

Status: re-evaluate

## Story

As a maintainer,
I want upgrade to stay a v4 no-op while the final regression gate proves the rewritten 17-command surface is release-ready,
so that release promotion is backed by explicit evidence.

## Acceptance Criteria

1. upgrade remains a no-op for v4 features and retains explicit migration paths for older versions.
2. The focused regression anchor set passes: init-feature, next, setup-control-repo, split-feature, upgrade, and git-orchestration.
3. Release readiness explicitly confirms 17 reachable prompts, end-to-end traceability, and zero broken active features.

## Tasks / Subtasks

- [ ] Preserve v4 no-op upgrade behavior.
  - [ ] Keep older-version migration routing explicit rather than implicit.
- [ ] Run and document the focused regression anchor suite.
  - [ ] Confirm all required command surfaces remain reachable.
- [ ] Produce the release-readiness gate artifact.
  - [ ] Record 17/17 prompt presence, zero schema changes, and green regression anchors.

## Dev Notes

- This is the release-promotion gate and should remain the last story executed.
- Preserve the split between migration routing and no-op v4 behavior.
- Keep the readiness report explicit rather than implied by passing tests alone.

### References

- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/epics.md
- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/stories.md
- TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/dependency-mapping.md

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
