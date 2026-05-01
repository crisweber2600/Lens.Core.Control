---
feature: lens-dev-new-codebase-dogfood
epic: 2
story_id: E2-S1
sprint_story_id: S2.1
title: Implement 3-branch control topology
type: fix
points: L
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E1-S3, E1-S5]
blocks: [E2-S2, E2-S4, E2-S5]
target_repo: lens.core.src
target_branch: develop
---

# E2-S1 — Implement 3-branch control topology

## Context

The current control topology uses only 2 branches (`{featureId}` and `{featureId}-plan`). The tech-plan (ADR-2) and business-plan require a 3-branch model: `{featureId}`, `{featureId}-plan`, and `{featureId}-dev`. This is BF-1 (dev branch behavior). The target-project branch strategy must remain independent and configurable separately.

## Implementation Steps

1. Update `bmad-lens-git-orchestration` to include `create-feature-branches` operation that creates all three control branches.
2. Add validation: each branch is verified before the phase operation that requires it.
3. Add structured error: missing branch routes back to init-feature, not silent fallback.
4. Keep target-project branch strategy separate: `flat`, `feature/{featureStub}`, or `feature/{featureStub}-{username}`.
5. Write focused tests for creation, validation, missing-branch error, and target-project strategy.

## Acceptance Criteria

- [ ] `create-feature-branches` creates `{featureId}`, `{featureId}-plan`, and `{featureId}-dev` from the correct base.
- [ ] Each branch is validated for existence before phase operations.
- [ ] Missing branch triggers a structured error routing to init-feature, not silent fallback.
- [ ] Target-project branch strategy is independent and configurable.
- [ ] Focused tests cover all scenarios.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill) for SKILL.md; branch operations are implementation code in the target repo.

Load the BMB documentation index at `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md` before authoring.

## Dev Notes

- Reference: tech-plan ADR-2, Git Orchestration Contract section, BF-1 bugfix.
- BF-2 (username persistence): ensure `{featureId}-dev` branch uses the configured `github_username` when needed.
- Do not confuse control repo topology (3-branch) with target-project branch strategy (separate, configurable).

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
