---
feature: lens-dev-new-codebase-baseline
story_id: "2.4"
story_key: "2-4-rewrite-new-feature"
epic: "2"
title: "Rewrite new-feature Command"
status: re-evaluate
priority: must
story_points: 8
depends_on: [2-2-rewrite-new-domain, 2-3-rewrite-new-service]
updated_at: 2026-04-22T00:00:00Z
---

# Story 2.4: Rewrite new-feature Command

Status: re-evaluate

## Story

As a Lens user,
I want new-feature to keep the canonical featureId formula, feature-index registration, and 2-branch topology intact,
so that new work stays governance-compatible with the existing lifecycle model.

## Acceptance Criteria

1. The canonical featureId `{domain}-{service}-{featureSlug}` is written to feature.yaml.
2. feature-index.yaml records both featureId and featureSlug.
3. The control repo creates exactly `{featureId}` and `{featureId}-plan`.

## Tasks / Subtasks

- [ ] Preserve canonical feature identity creation.
  - [ ] Write both featureId and featureSlug consistently.
- [ ] Preserve feature-index registration and summary stub creation.
  - [ ] Keep duplicate detection in place.
- [ ] Preserve two-branch creation and regression coverage.
  - [ ] Keep parity with init-feature and git-orchestration tests.

## Dev Notes

- This story is the identity root for downstream switch, next, and planning conductors.
- Keep the split between canonical control-repo identity and short target-repo branch naming.
- Do not republish the deprecated init-feature stub.

### References

- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/epics.md
- TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/dependency-mapping.md
- lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
