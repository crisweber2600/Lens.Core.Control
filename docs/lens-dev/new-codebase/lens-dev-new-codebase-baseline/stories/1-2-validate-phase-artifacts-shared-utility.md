---
feature: lens-dev-new-codebase-baseline
story_id: "1.2"
story_key: "1-2-validate-phase-artifacts-shared-utility"
epic: "1"
title: "Implement validate-phase-artifacts.py Shared Utility"
status: ready-for-dev
priority: must
story_points: 5
depends_on: [1-1-scaffold-published-surface]
updated_at: 2026-04-22T00:00:00Z
---

# Story 1.2: Implement validate-phase-artifacts.py Shared Utility

Status: ready-for-dev

## Story

As a lens-work maintainer,
I want a single review-ready validation script shared by all planning conductors,
so that gate logic is not duplicated across lifecycle phases.

## Acceptance Criteria

1. Reviewed, complete phase artifacts return success with a clear confirmation.
2. Missing or unreviewed artifacts return specific failures.
3. preplan, businessplan, techplan, and finalizeplan delegate gate validation to the shared script.

## Tasks / Subtasks

- [ ] Extend validate-phase-artifacts.py to cover the expected planning-phase contract states.
  - [ ] Preserve clear JSON output for pass and fail cases.
- [ ] Replace inline gate checks in planning conductors with shared-script delegation.
  - [ ] Verify docs-root resolution still comes from feature.yaml.docs.path.
- [ ] Add or preserve focused regression coverage.
  - [ ] Cover happy path, missing artifact, and unreviewed artifact cases.

## Dev Notes

- This utility is a shared dependency for all planning conductor rewrites.
- Keep behavior parity with the legacy validation flow surfaced in the old dependency map.
- Do not hardcode feature docs paths inside conductors.

### References

- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/epics.md
- lens.core/_bmad/lens-work/scripts/validate-phase-artifacts.py
- TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/dependency-mapping.md

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
