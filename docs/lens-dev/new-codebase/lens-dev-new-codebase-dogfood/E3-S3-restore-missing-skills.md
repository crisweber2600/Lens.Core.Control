---
feature: lens-dev-new-codebase-dogfood
epic: 3
story_id: E3-S3
sprint_story_id: S3.3
title: Restore missing skills — dev, split-feature, upgrade, constitution
type: new
points: L
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E3-S1]
blocks: [E4-S1]
target_repo: lens.core.src
target_branch: develop
---

# E3-S3 — Restore missing skills: dev, split-feature, upgrade, constitution

## Context

Four SKILL.md files are missing from the target module: `bmad-lens-dev`, `bmad-lens-split-feature`, `bmad-lens-upgrade`, and `bmad-lens-constitution`. All four are referenced in `module.yaml` and in the lifecycle contract, meaning callers encounter a missing-skill error at runtime.

## Implementation Steps

1. Create `_bmad/lens-work/skills/bmad-lens-dev/SKILL.md` — conductor for the Dev phase lifecycle.
2. Create `_bmad/lens-work/skills/bmad-lens-split-feature/SKILL.md` — splits one feature into two.
3. Create `_bmad/lens-work/skills/bmad-lens-upgrade/SKILL.md` — handles module version upgrade path.
4. Create `_bmad/lens-work/skills/bmad-lens-constitution/SKILL.md` — wraps constitution resolution and track resolution.
5. Each SKILL.md must declare its input contracts, output contracts, error behaviors, and test hooks.
6. Write focused tests for each new skill's input/output contract.

## Acceptance Criteria

- [ ] `bmad-lens-dev/SKILL.md` exists with full input/output contract.
- [ ] `bmad-lens-split-feature/SKILL.md` exists with full input/output contract.
- [ ] `bmad-lens-upgrade/SKILL.md` exists with full input/output contract.
- [ ] `bmad-lens-constitution/SKILL.md` exists with full input/output contract.
- [ ] All four SKILL.md files registered in `module.yaml` without duplicates.
- [ ] Focused tests cover input/output contracts for each.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill) for all four new SKILL.md files.

Load the BMB documentation index at `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md` before authoring.

## Dev Notes

- Reference: tech-plan parity map command surface (17 commands), lifecycle contract in E1-S2.
- `bmad-lens-constitution` replaces the broken inline constitution resolver; see Defect 1 context from E1-S2.
- `bmad-lens-dev` scope is limited to phase lifecycle orchestration — full dev implementation is E4.

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
