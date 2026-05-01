---
feature: lens-dev-new-codebase-dogfood
epic: 1
story_id: E1-S1
sprint_story_id: S1.1
title: Build retained-command parity map
type: new
points: S
status: done
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: []
blocks: [E1-S2, E1-S3, E1-S4, E1-S5]
target_repo: lens.core.src
target_branch: develop
---

# E1-S1 — Build retained-command parity map

## Context

The dogfood rebuild begins from a partial target codebase. Before any implementation sprint proceeds, there must be a machine-checkable inventory that maps the 17 retained commands to their current status in `lens.core.src`. This parity map provides the authoritative baseline for subsequent sprints and closes Defect 8 (clean-room traceability).

The clean-room rule must be documented here: no source files or prose are copied from `lens.core`; all behavior is reproduced from baseline acceptance criteria and observed command outputs.

## Implementation Steps

1. Read the baseline docs for the 17-command inventory.
2. For each retained command, record: public prompt path, release prompt path, owning skill path, and current target status (`present`, `partial`, `missing`).
3. Write the parity map document to `lens.core.src` under a known docs path.
4. Add a validation script or check that compares the parity map against the live file tree and reports drift.
5. Add the clean-room traceability statement to the document.

## Acceptance Criteria

- [x] A parity map document lists all 17 retained commands with their public stub path, release prompt path, owning skill, and current target status.
- [x] The clean-room traceability statement is explicit: no files were copied from `lens.core`.
- [x] The document is committed to `lens.core.src` under a known docs path.
- [x] A validation script or check can compare the parity map against the live file tree and report drift.

## Implementation Channel

This story does NOT touch `lens.core.src/_bmad/lens-work/` directly. The parity map is a documentation artifact committed to the target repo's docs path. No BMB path is required. Exception: if the validation script touches SKILL.md or manifest files, use `.github/skills/bmad-module-builder`.

## Dev Notes

- Reference: `docs/lens-dev/new-codebase/lens-dev-new-codebase-dogfood/tech-plan.md` — Architecture Overview table and Defect 8.
- The 17 retained commands: `preflight`, `new-domain`, `new-service`, `new-feature`, `switch`, `next`, `preplan`, `businessplan`, `techplan`, `finalizeplan`, `expressplan`, `dev`, `complete`, `split-feature`, `constitution`, `discover`, `upgrade`.
- Do NOT count QuickPlan as a public command in the parity map; it is internal-only (pending E3-S5 decision).

## Dev Agent Record

### Agent Model Used
GitHub Copilot

### Debug Log References
- `python docs/lens-dev/new-codebase/lens-dev-new-codebase-dogfood/validate-retained-command-parity.py` -> OK: retained-command parity map matches the live tree (17 commands: present=10, partial=4, missing=3).
- E1-S1 adversarial review verdict: PASS.

### Completion Notes List
- Added the retained-command parity map in the target repo docs path.
- Added a deterministic parity validator that compares the map against the live target file tree.
- Merged the latest target `origin/develop` into `feature/dogfood` before continuing downstream stories.

### File List
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-dogfood/retained-command-parity-map.md`
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-dogfood/validate-retained-command-parity.py`

### Change Log

- 2026-05-01: Completed E1-S1 implementation and review gate on target branch `feature/dogfood`.
