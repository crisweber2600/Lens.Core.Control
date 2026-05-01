---
feature: lens-dev-new-codebase-dogfood
epic: 1
story_id: E1-S4
sprint_story_id: S1.4
title: Implement feature-yaml state operations
type: new
points: L
status: done
phase: dev
updated_at: '2026-05-01T17:58:19Z'
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

- [x] `bmad-lens-feature-yaml` skill exists in target `_bmad/lens-work/skills/`.
- [x] Read: loads feature.yaml and returns identity fields, phase, track, docs paths, target_repos, dependencies, transition history.
- [x] Validate: rejects invalid phase transitions; warns on missing target_repos.
- [x] Update: surgically updates specified fields; preserves all others unchanged.
- [x] Dirty-state handling: pull, stage, commit, push, report SHA (Defect 6 regression passes).
- [x] Focused tests cover all four operation modes.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill).

Load the BMB documentation index at `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md` before authoring.

## Dev Notes

- Reference: tech-plan Feature State Contract section and Defect 6.
- Surgical update means: read the YAML, modify only the specified keys, write back — do not regenerate the entire file.
- The M1 response (register `lens.core.src` in `feature.yaml.target_repos` for the dogfood feature) must be completed using this skill after it is implemented.

## Dev Agent Record

### Agent Model Used
GitHub Copilot

### Debug Log References
- Loaded BMB builder documentation index from `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md` before checkpointing the skill build.
- `uv run --with pytest --with pyyaml python -m pytest _bmad/lens-work/skills/bmad-lens-feature-yaml/scripts/tests/test-feature-yaml-ops.py` -> 5 passed.
- `uv run --with pyyaml python _bmad/lens-work/skills/bmad-lens-feature-yaml/scripts/feature-yaml-ops.py read --governance-repo <governance> --feature-id lens-dev-new-codebase-dogfood` -> pass.
- `uv run --with pyyaml python _bmad/lens-work/skills/bmad-lens-feature-yaml/scripts/feature-yaml-ops.py validate --governance-repo <governance> --feature-id lens-dev-new-codebase-dogfood --to-phase dev` -> pass.
- `uv run --with pytest --with pyyaml python -m pytest _bmad/lens-work/scripts/tests _bmad/lens-work/skills/bmad-lens-feature-yaml/scripts/tests` -> 26 passed.
- E1-S4 adversarial review verdict: PASS. See `code-review-E1-S4.md`.

### Completion Notes List
- Added the clean-room `bmad-lens-feature-yaml` skill with CLI-backed read, validate, update, and commit-dirty operations.
- Implemented v4 feature state payloads covering identity, phase, track, docs paths, target repos, dependencies, milestones, and transition history.
- Added phase-transition validation and target-repo warnings for implementation-impacting features.
- Added surgical YAML updates for phase, docs paths, target repos, and milestones while preserving untouched fields.
- Added dirty governance-state persistence that pulls with autostash, stages relevant paths, commits, pushes, and reports the resulting SHA.
- Review noted one non-blocking Windows path formatting observation for relative git paths; no action required.

### File List
- `_bmad/lens-work/skills/bmad-lens-feature-yaml/SKILL.md`
- `_bmad/lens-work/skills/bmad-lens-feature-yaml/scripts/feature-yaml-ops.py`
- `_bmad/lens-work/skills/bmad-lens-feature-yaml/scripts/tests/test-feature-yaml-ops.py`

### Change Log

- 2026-05-01: Completed E1-S4 implementation and review gate on target branch `feature/dogfood`.
