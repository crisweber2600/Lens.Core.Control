---
feature: lens-dev-new-codebase-dogfood
epic: 1
story_id: E1-S5
sprint_story_id: S1.5
title: Implement read-only git-state operations
type: new
points: M
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E1-S1, E1-S3]
blocks: [E2-S1, E2-S4]
target_repo: lens.core.src
target_branch: develop
---

# E1-S5 — Implement read-only git-state operations

## Context

The target module has no `bmad-lens-git-state` skill. Phase conductors and cross-feature context loading depend on the ability to inspect branch topology, active features, and git-vs-yaml discrepancies without performing any writes.

## Implementation Steps

1. Create `_bmad/lens-work/skills/bmad-lens-git-state/SKILL.md` in the target.
2. Implement branch topology reporting: current branch, all feature branches present, which features have plan/dev branches open.
3. Implement active feature reporting: reads governance feature-index, reports phase per feature.
4. Implement discrepancy reporting: compares `feature.yaml` phase to branch state and flags mismatches.
5. Enforce strict read-only constraint: no git writes, no file mutations.
6. Write focused tests for branch detection, discrepancy reporting, and read-only constraint.

## Acceptance Criteria

- [ ] `bmad-lens-git-state` skill exists in target `_bmad/lens-work/skills/`.
- [ ] Reports: current branch, all feature branches, which features have plan/dev branches open.
- [ ] Reports: active features from governance feature-index with phase.
- [ ] Reports: git-vs-yaml discrepancies.
- [ ] Strictly read-only: no writes, no mutations.
- [ ] Focused tests cover all reporting modes and read-only constraint.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill).

Load the BMB documentation index at `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md` before authoring.

## Dev Notes

- Reference: tech-plan Git State Contract section.
- Read-only constraint is strict: if any code path could mutate the repo, it must be rejected at design time.
- Discrepancy reporting should surface the specific field mismatch (e.g., `feature.yaml.phase=dev` but no `{featureId}-dev` branch exists).

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
