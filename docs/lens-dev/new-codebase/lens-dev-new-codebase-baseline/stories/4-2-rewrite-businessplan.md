---
feature: lens-dev-new-codebase-baseline
story_id: "4.2"
story_key: "4-2-rewrite-businessplan"
epic: "4"
title: "Rewrite businessplan Command"
status: re-evaluate
priority: must
story_points: 5
depends_on: [1-4-publish-to-governance-entry-hook, 3-1-fix-constitution-partial-hierarchy, 4-1-rewrite-preplan]
updated_at: 2026-04-22T00:00:00Z
---

# Story 4.2: Rewrite businessplan Command

Status: re-evaluate

## Story

As a Lens user,
I want businessplan to publish reviewed predecessors before PRD and UX authoring, with no direct governance writes,
so that publish-before-author ordering is preserved.

## Acceptance Criteria

1. publish-to-governance runs before BMAD authoring delegation.
2. Governance write audits show no direct writes from businessplan.
3. Wrapper-equivalence and governance-audit regressions stay green.

## Tasks / Subtasks

- [ ] Wire the shared publish entry hook into businessplan.
  - [ ] Publish reviewed predecessor docs before PRD or UX authoring starts.
- [ ] Preserve BMAD delegation for PRD and UX outputs.
  - [ ] Keep the conductor itself out of authoring decisions.
- [ ] Maintain governance write audit coverage.
  - [ ] Fail if businessplan writes to governance outside publish-to-governance.

## Dev Notes

- Keep the feature docs path as the authoritative staging root.
- Use the governance mirror only as a published snapshot for consumers.
- Preserve old-codebase wrapper routing for PRD and UX delegation.

### References

- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/epics.md
- TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/dependency-mapping.md
- lens.core/_bmad/lens-work/skills/bmad-lens-git-orchestration/references/publish-to-governance.md

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
