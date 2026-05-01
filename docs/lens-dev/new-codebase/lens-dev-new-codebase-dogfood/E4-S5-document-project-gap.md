---
feature: lens-dev-new-codebase-dogfood
epic: 4
story_id: E4-S5
sprint_story_id: S4.5
title: Document discover and document-project capability gaps
type: fix
points: M
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E1-S3, E3-S3]
blocks: [E5-S5]
target_repo: lens.core.src
target_branch: develop
---

# E4-S5 — Document discover and document-project capability gaps

## Context

The `lens-discover` and `lens-document-project` command surfaces exist in the parity map but have under-specified implementations in the new codebase. Rather than silently inheriting broken behavior, this story establishes explicit capability boundaries: what works, what is deferred, and what requires further research.

## Implementation Steps

1. Load the old-codebase `lens-discover` and `lens-document-project` implementations.
2. Compare against the new-codebase stubs; identify capability gaps.
3. For each gap: classify as (a) implemented, (b) deferred with known scope, or (c) requires further research.
4. Write a capability-gap report as `docs/lens-dev/new-codebase/lens-dev-new-codebase-dogfood/capability-gaps.md`.
5. Update `module.yaml` comments to surface deferred gaps without removing the registrations.

## Acceptance Criteria

- [ ] Capability gap report `capability-gaps.md` exists with each gap classified.
- [ ] Every deferred gap includes a scope note.
- [ ] `module.yaml` comments reflect gap classifications.
- [ ] No silent capability regressions: every gap is explicit.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill).

## Governance Coordination Note

This story reproduces reference behavior for clean-room parity testing. First-class Dev command authoring belongs to `lens-dev-new-codebase-dev`; any overlap with that feature's eventual PRD must be surfaced as explicit acceptance criteria alignment before implementation proceeds.

## Dev Notes

- Reference: tech-plan, parity map from E1-S1, old-codebase at `TargetProjects/lens-dev/old-codebase/`.

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
