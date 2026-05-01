---
feature: lens-dev-new-codebase-dogfood
epic: 1
story_id: E1-S3
sprint_story_id: S1.3
title: Add target module config and user config contract
type: new
points: M
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E1-S1]
blocks: [E1-S4, E1-S5, E2-S1]
target_repo: lens.core.src
target_branch: develop
---

# E1-S3 — Add target module config and user config contract

## Context

The target module has no `bmadconfig.yaml` and no documented user config contract. This means governance repo path, control topology, target-project branch strategy, username, and output paths are either hardcoded or undefined. Two additional bugs live here:

- **Defect 2:** Command and config discovery fails when `rg` or editor glob search is unavailable.
- **Defect 3:** Git Bash `/d/...` path forms cause file writes to `C:/d/...` on Windows.

## Implementation Steps

1. Create `_bmad/lens-work/bmadconfig.yaml` in the target with required fields.
2. Document the user config contract: which fields are overridable via `config.user.yaml`.
3. Add deterministic repository and config discovery helpers that do not depend on `rg` or a specific editor search provider.
4. Add OS-normalized absolute path utility for all file write and publish operations.
5. Write focused tests for: config loading, fallback discovery (no `rg`), and Windows path normalization.

## Acceptance Criteria

- [ ] Target has `_bmad/lens-work/bmadconfig.yaml` with `governance_repo_path`, `control_topology: 3-branch`, `target_projects_path`, `default_git_remote`, and `lifecycle_contract` fields.
- [ ] User config contract documents user-overridable fields (at minimum: `github_username`, `default_branch`, `target_branch_strategy`).
- [ ] Feature and config discovery works without `rg` or any specific editor search provider; fallback path is tested (Defect 2).
- [ ] File writes and publish operations use OS-normalized absolute paths; no artifact written to `C:/d/...` on Windows (Defect 3).
- [ ] Focused tests cover config loading, fallback discovery, and Windows path normalization.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill) — `bmadconfig.yaml` and config helpers are core `lens-work` artifacts.

Load the BMB documentation index at `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md` before authoring.

## Dev Notes

- Reference: tech-plan ADR-2 (topology rules), Data Model Changes table.
- Defect 2: deterministic discovery must use Python stdlib (os, pathlib, glob) rather than subprocess rg or VS Code search API.
- Defect 3: use `pathlib.Path.resolve()` or equivalent before all file writes and publish calls.
- The open question about which config file is the single source for `github_username` should be resolved here and recorded in completion notes.

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
