---
story_id: E1-S2
epic: E1
feature: lens-dev-new-codebase-expressplan
title: Validate SKILL.md Three-Step Contract
priority: High
size: S
status: not-started
updated_at: '2026-04-30T00:00:00Z'
---

# E1-S2 — Validate SKILL.md Three-Step Contract

## Context

`_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md` must implement the three-step
conductor contract:
1. Express-only eligibility gate
2. QuickPlan delegation via `bmad-lens-bmad-skill --skill bmad-lens-quickplan`
3. Adversarial review invocation with `--phase expressplan --source phase-complete` then
   phase advance to `expressplan-complete` on pass/pass-with-warnings

## Tasks

1. Open `SKILL.md` in the target source repo.
2. Verify Step 1: eligibility gate blocks non-express-track features before any other work.
3. Verify Step 2: delegation invokes `bmad-lens-bmad-skill --skill bmad-lens-quickplan`
   (or equivalent delegation phrase — check what pattern other conductor skills use).
4. Verify Step 3: adversarial review invocation includes `--phase expressplan` and
   `--source phase-complete` flags (or equivalent).
5. Verify Step 3b: phase advance to `expressplan-complete` occurs ONLY on
   `pass` or `pass-with-warnings` verdict.
6. Verify Step 3b negative: `fail` verdict does NOT trigger phase advance.
7. Note any divergence from the three-step contract as a finding.

## Acceptance Criteria

- [ ] Eligibility gate present and correctly gates express-only features.
- [ ] QuickPlan delegation present and uses correct skill reference.
- [ ] Adversarial review invocation present with correct phase and source flags.
- [ ] Phase advance gated on verdict (pass only).
- [ ] Any findings documented and triaged (must not be fail-level to proceed).
