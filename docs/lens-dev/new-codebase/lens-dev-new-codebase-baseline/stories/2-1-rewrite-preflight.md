---
feature: lens-dev-new-codebase-baseline
story_id: "2.1"
story_key: "2-1-rewrite-preflight"
epic: "2"
title: "Rewrite preflight Command"
status: re-evaluate
priority: must
story_points: 5
depends_on: [1-1-scaffold-published-surface]
updated_at: 2026-04-22T00:00:00Z
---

# Story 2.1: Rewrite preflight Command

Status: re-evaluate

## Story

As a Lens user,
I want preflight to preserve prompt-start sync and workspace validation exactly as it works today,
so that onboarding and resume flows are not disrupted.

## Acceptance Criteria

1. light-preflight.py runs before any workspace validation.
2. Correctly configured workspaces pass without lifecycle mutation.
3. Missing governance or install dependencies fail clearly and non-destructively.

## Tasks / Subtasks

- [ ] Preserve prompt-start sequencing for preflight entry.
  - [ ] Ensure light-preflight remains the first command-side gate.
- [ ] Rebuild workspace validation logic without lifecycle writes.
  - [ ] Keep missing-dependency diagnostics actionable.
- [ ] Keep regression parity with setup-control-repo coverage.
  - [ ] Remove onboard as a published surface while preserving behavior behind preflight.

## Dev Notes

- This story is parity-sensitive because every published prompt depends on prompt-start correctness.
- Follow the old codebase dependency map for onboard/preflight routing.
- Avoid introducing feature state mutation into preflight.

### References

- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/epics.md
- TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/dependency-mapping.md
- TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/project-overview.md

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
