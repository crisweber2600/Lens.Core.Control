---
feature: lens-dev-new-codebase-dogfood
epic: 3
story_id: E3-S1
sprint_story_id: S3.1
title: Reconcile missing prompt stubs
type: new
points: M
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E2-S3, E2-S4, E2-S5]
blocks: [E3-S3]
target_repo: lens.core.src
target_branch: develop
---

# E3-S1 — Reconcile missing prompt stubs

## Context

Defect 7: Several prompt stubs registered in `module.yaml` are missing from the filesystem. The clean-room parity target requires every prompt stub referenced in `module.yaml` to exist as an actual file, even if the file is a minimal scaffold. This story restores parity on prompt file presence.

## Implementation Steps

1. Run the parity map from E1-S1: enumerate prompt stubs registered in `module.yaml`.
2. For each stub, check whether the file exists in `_bmad/lens-work/prompts/`.
3. Create missing stubs as minimal scaffold files (frontmatter + placeholder body).
4. Do not alter existing prompts.
5. Update `module.yaml` if any stub references are incorrect.
6. Write a test that verifies every `module.yaml`-registered prompt has a corresponding file.

## Acceptance Criteria

- [ ] Every prompt stub registered in `module.yaml` has a corresponding file.
- [ ] Missing stubs created as minimal scaffolds; existing files not altered.
- [ ] `module.yaml` updated if any stub references are incorrect.
- [ ] Test: all `module.yaml`-registered prompts have corresponding files → passes.
- [ ] Defect 7 regression test passes.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill).

Load the BMB documentation index at `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md` before authoring.

## Dev Notes

- Reference: tech-plan Defect 7, parity map from E1-S1.
- "Minimal scaffold" = valid frontmatter + one-sentence description body; no functional logic required.

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
