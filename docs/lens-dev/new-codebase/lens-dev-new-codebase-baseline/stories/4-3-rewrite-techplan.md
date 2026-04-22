---
feature: lens-dev-new-codebase-baseline
story_id: "4.3"
story_key: "4-3-rewrite-techplan"
epic: "4"
title: "Rewrite techplan Command"
status: re-evaluate
priority: must
story_points: 5
depends_on: [1-4-publish-to-governance-entry-hook, 3-1-fix-constitution-partial-hierarchy, 4-2-rewrite-businessplan]
updated_at: 2026-04-22T00:00:00Z
---

# Story 4.3: Rewrite techplan Command

Status: re-evaluate

## Story

As a Lens user,
I want techplan to generate architecture through the shared publish-before-author hook while enforcing the PRD reference rule,
so that technical planning stays governed and traceable.

## Acceptance Criteria

1. The publish entry hook runs before architecture authoring.
2. The architecture artifact explicitly references the authoritative PRD.
3. Architecture-reference and wrapper-equivalence regressions remain green.

## Tasks / Subtasks

- [ ] Rebuild techplan entry around publish-before-author behavior.
  - [ ] Publish reviewed businessplan artifacts before architecture generation.
- [ ] Preserve PRD reference enforcement inside architecture generation.
  - [ ] Keep lifecycle validation aligned with the PRD dependency rule.
- [ ] Preserve focused regression coverage.
  - [ ] Keep wrapper-equivalence and architecture-reference checks intact.

## Dev Notes

- techplan is still a conductor; BMAD architecture authoring remains downstream.
- Keep staged docs local and governance mirrored only through the publish hook.
- The PRD reference rule is load-bearing and must not become optional.

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
