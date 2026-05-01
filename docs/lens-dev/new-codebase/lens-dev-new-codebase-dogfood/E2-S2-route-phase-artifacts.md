---
feature: lens-dev-new-codebase-dogfood
epic: 2
story_id: E2-S2
sprint_story_id: S2.2
title: Route phase artifacts to the correct control branch
type: fix
points: M
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E2-S1]
blocks: [E3-S4]
target_repo: lens.core.src
target_branch: develop
---

# E2-S2 — Route phase artifacts to the correct control branch

## Context

Phase artifacts must be routed to the correct control branch by phase step. Currently the routing may not enforce the separation between planning (plan branch), review (base branch), and implementation (dev branch or target repo). This is BF-6 (topology correction).

## Implementation Steps

1. Document the routing rules: planning before FinalizePlan → `{featureId}-plan`; FinalizePlan step 1/2 → `{featureId}-plan` then merged to `{featureId}`; FinalizePlan step 3 bundle → `{featureId}-dev`; Dev → target repo only.
2. Enforce routing in git-orchestration: each artifact commit validates the current branch against the expected phase branch.
3. Log routing decisions so deviations are detectable.
4. Reject writes that land on the wrong branch; surface a structured error.
5. Write focused tests for correct routing for each phase step.

## Acceptance Criteria

- [ ] Planning artifacts before FinalizePlan routed to `{featureId}-plan`.
- [ ] FinalizePlan step 1/2 artifacts routed to `{featureId}-plan`, merged to `{featureId}`.
- [ ] FinalizePlan step 3 bundle routed to `{featureId}-dev`.
- [ ] Dev implementation artifacts written only to the target repo.
- [ ] Routing decisions are logged.
- [ ] Focused tests cover correct branch routing for each phase step.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill).

Load the BMB documentation index at `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md` before authoring.

## Dev Notes

- Reference: tech-plan ADR-2, Git Orchestration Contract section, BF-6.
- Routing validation should happen before any write, not after.

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
