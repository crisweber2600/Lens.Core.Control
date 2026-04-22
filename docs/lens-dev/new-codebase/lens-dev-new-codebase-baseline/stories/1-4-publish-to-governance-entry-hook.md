---
feature: lens-dev-new-codebase-baseline
story_id: "1.4"
story_key: "1-4-publish-to-governance-entry-hook"
epic: "1"
title: "Implement publish-to-governance Entry Hook"
status: re-evaluate
priority: must
story_points: 5
depends_on: [1-1-scaffold-published-surface]
updated_at: 2026-04-22T00:00:00Z
---

# Story 1.4: Implement publish-to-governance Entry Hook

Status: re-evaluate

## Story

As a lens-work maintainer,
I want a shared publish-before-author entry hook,
so that planning conductors mirror reviewed docs to governance without direct governance writes.

## Acceptance Criteria

1. businessplan, techplan, and finalizeplan invoke publish-to-governance before delegated authoring starts.
2. No-op entry runs cleanly when nothing is pending publication.
3. Governance write audits confirm the entry hook is the only planning-phase write path.

## Tasks / Subtasks

- [ ] Wire publish-to-governance into each planning-phase entry point.
  - [ ] Preserve feature docs path to governance docs mirror resolution.
- [ ] Support no-op and publish-needed cases with explicit reporting.
  - [ ] Return copied and missing artifacts clearly.
- [ ] Preserve governance write audit coverage.
  - [ ] Leave discover as the only explicit exception.

## Dev Notes

- This hook is the authority-boundary enforcement point for planning artifacts.
- Never replace the CLI-backed publish step with direct file copies in conductors.
- Keep governance writes on main and control-repo artifacts staged locally.

### References

- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/epics.md
- lens.core/_bmad/lens-work/skills/bmad-lens-git-orchestration/references/publish-to-governance.md
- TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/dependency-mapping.md

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
