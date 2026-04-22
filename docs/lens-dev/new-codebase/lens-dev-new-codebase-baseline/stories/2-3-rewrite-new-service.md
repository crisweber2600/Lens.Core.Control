---
feature: lens-dev-new-codebase-baseline
story_id: "2.3"
story_key: "2-3-rewrite-new-service"
epic: "2"
title: "Rewrite new-service Command"
status: re-evaluate
priority: must
story_points: 3
depends_on: [2-2-rewrite-new-domain]
updated_at: 2026-04-22T00:00:00Z
---

# Story 2.3: Rewrite new-service Command

Status: re-evaluate

## Story

As a Lens user,
I want new-service to create service governance artifacts under an existing domain while preserving inherited rules,
so that service setup remains stable and predictable.

## Acceptance Criteria

1. Service markers use the correct location and naming rules.
2. Domain-to-service inheritance remains intact.
3. Missing parent domains fail fast with a clear message.

## Tasks / Subtasks

- [ ] Rebuild service scaffolding under an existing domain.
  - [ ] Preserve naming and location conventions.
- [ ] Carry domain-level inheritance into the service scaffold.
  - [ ] Keep constitution and metadata inheritance explicit.
- [ ] Preserve missing-domain guardrails and regressions.
  - [ ] Keep failure behavior non-destructive.

## Dev Notes

- This story depends on domain creation remaining stable.
- Keep inheritance additive and explicit rather than implicit.
- Preserve service-level compatibility with later new-feature initialization.

### References

- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/epics.md
- TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/dependency-mapping.md
- TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/component-inventory.md

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
