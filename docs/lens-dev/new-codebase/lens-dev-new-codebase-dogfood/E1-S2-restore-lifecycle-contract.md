---
feature: lens-dev-new-codebase-dogfood
epic: 1
story_id: E1-S2
sprint_story_id: S1.2
title: Restore v4-compatible lifecycle contract
type: new
points: M
status: done
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E1-S1]
blocks: [E2-S1, E2-S4, E3-S1]
target_repo: lens.core.src
target_branch: develop
---

# E1-S2 — Restore v4-compatible lifecycle contract

## Context

The target `lens.core.src` lacks `lifecycle.yaml`. All phase conductors and track validators derive their behavior from this contract. Without it, commands may appear present but fail at runtime when they try to resolve phases, tracks, artifact contracts, or review names.

This story also fixes Defect 1: the constitution resolver false negative that rejected `express` as a valid track even when governance files permitted it for `lens-dev/new-codebase`.

## Implementation Steps

1. Read the reference `lifecycle.yaml` from `lens.core/_bmad/lens-work/lifecycle.yaml` as a behavioral reference (do not copy; reproduce the structure from scratch using accepted phase/track definitions).
2. Create `_bmad/lens-work/lifecycle.yaml` in the target with v4-compatible schema.
3. Include all retained phases with their artifact contracts.
4. Include all retained tracks with phase arrays; ensure `express` track lists `finalizeplan`.
5. In the express track review artifact contract: name `expressplan-adversarial-review.md` as current and note `expressplan-review.md` as a recognized legacy alias.
6. Update the constitution resolver to permit `express` track for `lens-dev/new-codebase` (regression fixture for Defect 1).
7. Write focused regression test: constitution resolver returns permitted for `express` track on `lens-dev/new-codebase`.

## Acceptance Criteria

- [x] Target has `_bmad/lens-work/lifecycle.yaml` at v4-compatible schema.
- [x] Covers all retained phases: `preplan`, `businessplan`, `techplan`, `expressplan`, `finalizeplan`, `dev`, `complete`.
- [x] Covers all retained tracks: `standard`, `express`, `quickdev`, `hotfix-express`, `spike`.
- [x] `express` track lists `finalizeplan` in its phases array.
- [x] Express review artifact contract names `expressplan-adversarial-review.md` and recognizes `expressplan-review.md` as legacy alias.
- [x] Constitution resolver accepts `express` track for `lens-dev/new-codebase` (Defect 1 regression fixture passes).
- [x] Focused regression test is committed and passes.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill) — the lifecycle contract is a core `lens-work` artifact.

Load the BMB documentation index at `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md` before authoring.

## Dev Notes

- Reference: tech-plan ADR-5 (express review filename), tech-plan ADR-1 (foundations first).
- Defect 1 / BF regression: constitution resolver must not silently ignore express-track permissions from governance constitution files.
- Do not copy the reference `lifecycle.yaml` verbatim. Reproduce from the baseline docs behavioral contracts.

## Dev Agent Record

### Agent Model Used
GitHub Copilot

### Debug Log References
- `uv run --with pytest --with pyyaml python -m pytest _bmad/lens-work/scripts/tests/test-lifecycle-contract.py _bmad/lens-work/scripts/tests/test-validate-phase-artifacts.py _bmad/lens-work/skills/bmad-lens-constitution/scripts/tests/test-constitution-ops.py` -> 98 passed.
- `uv run _bmad/lens-work/skills/bmad-lens-constitution/scripts/constitution-ops.py progressive-display --governance-repo "D:/Lens.Core.Control - Copy/TargetProjects/lens/lens-governance" --domain lens-dev --service new-codebase --track express` -> `track_permitted: true`.
- E1-S2 adversarial review verdict: PASS. See `code-review-E1-S2.md`.

### Completion Notes List
- Restored the target clean-room lifecycle contract with retained phases, tracks, express review aliasing, and schema version 4.
- Updated artifact validation to recognize the legacy express review filename when validating the current express review contract.
- Updated constitution track handling so `express` is permitted for `lens-dev/new-codebase`, with focused regression coverage.

### File List
- `_bmad/lens-work/lifecycle.yaml`
- `_bmad/lens-work/scripts/validate-phase-artifacts.py`
- `_bmad/lens-work/scripts/tests/test-validate-phase-artifacts.py`
- `_bmad/lens-work/scripts/tests/test-lifecycle-contract.py`
- `_bmad/lens-work/skills/bmad-lens-constitution/scripts/constitution-ops.py`
- `_bmad/lens-work/skills/bmad-lens-constitution/scripts/tests/test-constitution-ops.py`

### Change Log

- 2026-05-01: Completed E1-S2 implementation and review gate on target branch `feature/dogfood`.
