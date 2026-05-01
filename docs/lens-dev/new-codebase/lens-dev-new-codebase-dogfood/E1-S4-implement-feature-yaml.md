---
feature: lens-dev-new-codebase-dogfood
epic: 1
story_id: E1-S4
sprint_story_id: S1.4
title: Implement feature-yaml state operations
type: new
points: L
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E1-S1, E1-S3]
blocks: [E2-S3, E2-S4, E3-S4]
target_repo: lens.core.src
target_branch: develop
---

# E1-S4 — Implement feature-yaml state operations

## Context

The target module has no `bmad-lens-feature-yaml` skill. Phase conductors cannot advance features, update docs paths, or register target repos without this skill. This is also where Defect 6 (dirty governance state blocking phase advancement) is resolved: rather than blocking on uncommitted changes, the skill must detect dirty state and commit/push before continuing.

## Implementation Steps

1. Create `_bmad/lens-work/skills/bmad-lens-feature-yaml/SKILL.md` in the target.
2. Implement the read operation: loads `feature.yaml`, returns all v4 fields.
3. Implement the validate operation: rejects invalid transitions, warns on missing `target_repos` for implementation-impacting features.
4. Implement the surgical update operation: updates `phase`, `docs.path`, `docs.governance_docs_path`, `target_repos`, milestones; preserves all other fields.
5. Implement dirty-state handling: pull, stage relevant changes, commit, push, report SHA — does not block on uncommitted changes (Defect 6).
6. Write focused tests for read/validate/update/dirty-state scenarios.

## Acceptance Criteria

- [ ] `bmad-lens-feature-yaml` skill exists in target `_bmad/lens-work/skills/`.
- [ ] Read: loads feature.yaml and returns identity fields, phase, track, docs paths, target_repos, dependencies, transition history.
- [ ] Validate: rejects invalid phase transitions; warns on missing target_repos.
- [ ] Update: surgically updates specified fields; preserves all others unchanged.
- [ ] Dirty-state handling: pull, stage, commit, push, report SHA (Defect 6 regression passes).
- [ ] Focused tests cover all four operation modes.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill).

Load the BMB documentation index at `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md` before authoring.

## Dev Notes

- Reference: tech-plan Feature State Contract section and Defect 6.
- Surgical update means: read the YAML, modify only the specified keys, write back — do not regenerate the entire file.
- The M1 response (register `lens.core.src` in `feature.yaml.target_repos` for the dogfood feature) must be completed using this skill after it is implemented.

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
