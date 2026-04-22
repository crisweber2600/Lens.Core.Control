---
feature: lens-dev-new-codebase-baseline
story_id: "4.4"
story_key: "4-4-rewrite-finalizeplan"
epic: "4"
title: "Rewrite finalizeplan Command"
status: re-evaluate
priority: must
story_points: 8
depends_on: [1-2-validate-phase-artifacts-shared-utility, 1-3-batch-two-pass-contract, 1-4-publish-to-governance-entry-hook, 3-1-fix-constitution-partial-hierarchy, 4-3-rewrite-techplan]
updated_at: 2026-04-22T00:00:00Z
---

# Story 4.4: Rewrite finalizeplan Command

Status: re-evaluate

## Story

As a Lens user,
I want finalizeplan to preserve strict three-step ordering, plan PR topology, and downstream bundle generation,
so that planning consolidation remains deterministic and reviewable.

## Acceptance Criteria

1. Step 1 review completes before plan PR work begins.
2. Step 2 creates the correct `{featureId}-plan` to `{featureId}` PR.
3. Step 3 generates epics, stories, readiness, sprint status, and story files before the final PR is opened.

## Tasks / Subtasks

- [ ] Preserve explicit three-step ordering.
  - [ ] Do not allow step skipping or reordering.
- [ ] Preserve plan PR creation and readiness checks.
  - [ ] Keep the `{featureId}-plan` to `{featureId}` topology exact.
- [ ] Preserve downstream bundle generation and final PR sequencing.
  - [ ] Generate artifacts before opening the `{featureId}` to `main` PR.

## Dev Notes

- This is the conductor whose contract is being exercised by the current feature.
- Preserve adversarial review, governance re-check, merge-plan, and final PR sequencing.
- Keep the bundle staged locally under feature docs.path until /dev publishes it.

### References

- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/epics.md
- lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md
- TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/dependency-mapping.md

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
