---
feature: lens-dev-new-codebase-dogfood
epic: 2
story_id: E2-S4
sprint_story_id: S2.4
title: Resolve BF-4 and BF-5 phase-start and base-branch validation
type: fix
points: M
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E1-S2, E1-S5, E2-S1]
blocks: [E3-S1]
target_repo: lens.core.src
target_branch: develop
---

# E2-S4 — Resolve BF-4 and BF-5 phase-start and base-branch validation

## Context

BF-4 (phase-start branch verification) and BF-5 (base-branch validation) mean that phases can silently begin from the wrong branch or without the required branches existing. This also covers the Defect 1 fix (constitution resolver express-track false negative) at the phase-start gate.

## Implementation Steps

1. Add phase-start check: verifies `{featureId}` and `{featureId}-plan` exist before any phase writing begins.
2. Add base-branch validation: confirms the current branch is the intended base; fails explicitly on mismatch.
3. At phase-start, verify the constitution resolver accepts the feature's track; reject with a structured error on false negative (Defect 1 gate).
4. Write focused tests: missing branch, wrong base, and constitution resolver express-track pass scenarios.

## Acceptance Criteria

- [ ] Phase-start check verifies required branches exist; fails on missing branches.
- [ ] Base-branch validation confirms correct branch; fails explicitly on mismatch.
- [ ] Constitution resolver accepts `express` track for `lens-dev/new-codebase`; false-negative from Defect 1 does not recur.
- [ ] Focused tests cover missing branch, wrong base, and express-track pass.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill).

## Dev Notes

- Reference: tech-plan, BF-4, BF-5, Defect 1.
- The phase-start check should run before any artifact write or branch creation — it is a pre-condition gate.
- Constitution resolver fix at this gate is a runtime check; the underlying fix is in E1-S2 (lifecycle contract).

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
