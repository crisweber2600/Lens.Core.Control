---
feature: lens-dev-new-codebase-baseline
story_id: "5.4"
story_key: "5-4-rewrite-discover"
epic: "5"
title: "Rewrite discover Command"
status: re-evaluate
priority: must
story_points: 5
depends_on: [1-4-publish-to-governance-entry-hook]
updated_at: 2026-04-22T00:00:00Z
---

# Story 5.4: Rewrite discover Command

Status: re-evaluate

## Story

As a maintainer,
I want discover to preserve bidirectional inventory sync and the governance-main auto-commit exception,
so that repository inventory remains current without manual reconciliation.

## Acceptance Criteria

1. Governance-to-local and local-to-governance inventory sync both execute.
2. Governance-main auto-commit occurs only when inventory changes exist.
3. No-op runs report cleanly without creating a commit.

## Tasks / Subtasks

- [ ] Preserve bidirectional inventory synchronization.
  - [ ] Keep both directions visible in results.
- [ ] Preserve the governance-main auto-commit exception.
  - [ ] Do not spread this behavior to other planning commands.
- [ ] Preserve no-op reporting and regression coverage.
  - [ ] Confirm unchanged inventories do not commit.

## Dev Notes

- discover is the only allowed auto-commit exception in this planning ecosystem.
- Preserve repo inventory semantics and local clone synchronization rules.
- Keep behavior aligned with the old dependency map and inventory flow.

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
