---
feature: lens-dev-new-codebase-dogfood
epic: 3
story_id: E3-S2
sprint_story_id: S3.2
title: Reconcile module.yaml duplicate lens-expressplan entries
type: fix
points: M
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E1-S3]
blocks: [E3-S4]
target_repo: lens.core.src
target_branch: develop
---

# E3-S2 — Reconcile module.yaml duplicate lens-expressplan entries

## Context

`module.yaml` contains duplicate registrations for the `lens-expressplan` command surface. Duplicate entries cause resolution ambiguity and manifest as non-deterministic routing failures. The clean-room parity map from E1-S1 identifies the canonical entry; the duplicate must be removed.

## Implementation Steps

1. Load the parity map from E1-S1 to identify canonical vs. duplicate `lens-expressplan` entries in `module.yaml`.
2. Remove the duplicate entry, keeping the canonical one.
3. Verify the remaining entry resolves to the correct skill/prompt path.
4. Write a test: parse `module.yaml`, assert each command name appears exactly once.

## Acceptance Criteria

- [ ] `module.yaml` has exactly one `lens-expressplan` entry.
- [ ] Remaining entry resolves to the correct skill/prompt path.
- [ ] Test: each command name in `module.yaml` appears exactly once → passes.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill).

Load the BMB documentation index at `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md` before authoring.

## Dev Notes

- Reference: tech-plan parity map, module.yaml audit section.
- If there are other duplicate entries beyond `lens-expressplan`, address them in the same story.

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
