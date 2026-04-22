---
feature: lens-dev-new-codebase-baseline
story_id: "2.2"
story_key: "2-2-rewrite-new-domain"
epic: "2"
title: "Rewrite new-domain Command"
status: re-evaluate
priority: must
story_points: 3
depends_on: [1-1-scaffold-published-surface]
updated_at: 2026-04-22T00:00:00Z
---

# Story 2.2: Rewrite new-domain Command

Status: re-evaluate

## Story

As a Lens user,
I want new-domain to create governance markers and a constitution scaffold with the existing naming rules,
so that domain creation stays reproducible and governed.

## Acceptance Criteria

1. Governance domain markers are written under the approved naming convention.
2. The domain constitution scaffold lands in the expected location.
3. Duplicate-domain attempts fail without overwriting existing artifacts.

## Tasks / Subtasks

- [ ] Rebuild domain creation using the retained governance scaffolding rules.
  - [ ] Preserve marker and constitution locations.
- [ ] Detect duplicate domains before any write occurs.
  - [ ] Return clear conflict guidance.
- [ ] Preserve init-feature domain regression behavior.
  - [ ] Keep naming stable across both source and governance surfaces.

## Dev Notes

- This is a governance write path and must stay within the approved authority boundary.
- Use the existing constitution hierarchy conventions from the old codebase docs.
- Keep the feature bootstrap workflow compatible with the retained new-service and new-feature flows.

### References

- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/epics.md
- TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/source-tree-analysis.md
- TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/dependency-mapping.md

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
