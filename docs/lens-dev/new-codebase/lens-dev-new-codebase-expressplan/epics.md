---
feature: lens-dev-new-codebase-expressplan
doc_type: epics
status: draft
goal: "Decompose expressplan command delivery into reviewable epics aligned with the three sprint slices."
updated_at: '2026-04-30T00:00:00Z'
---

# Epics — Expressplan Command

## Epic 1 — Foundation Validation

**Goal:** Confirm the expressplan infrastructure created in the previous session is complete
and correct.

**Scope:**
- Read and validate `.github/prompts/lens-expressplan.prompt.md` against the shared
  prompt-start preflight pattern.
- Read and validate `_bmad/lens-work/prompts/lens-expressplan.prompt.md` as a thin redirect.
- Read and validate `_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md` for:
  - Express-only eligibility gate
  - QuickPlan delegation via `bmad-lens-bmad-skill --skill bmad-lens-quickplan`
  - Adversarial review invocation `--phase expressplan --source phase-complete`
  - Phase-advance to `expressplan-complete` on pass verdict
  - Party-mode enforcement
- Confirm `_bmad/lens-work/module.yaml` lists `lens-expressplan.prompt.md` in the `prompts:`
  section following the same shape as other retained-command entries.
- Read `test-expressplan-ops.py` and confirm which regression expectations are already
  covered and which are missing.
- Commit all untracked infrastructure files to the target source repo.

**Exit Criteria:**
- All infrastructure files are read and confirmed valid.
- `module.yaml` registration shape is correct.
- Test file coverage gaps are documented.
- All infrastructure files are committed.

---

## Epic 2 — Discovery and Regressions

**Goal:** Register `lens-expressplan` in the new-codebase discovery surface and harden
regression coverage.

**Scope:**
- Identify the retained command discovery file used by `lens-techplan` and other retained
  commands in the new-codebase target project.
- Register `lens-expressplan` following the same pattern.
- Define and verify regression expectations for:
  - Express-only eligibility gate (non-express features blocked before QuickPlan delegation)
  - QuickPlan delegation route (`bmad-lens-bmad-skill --skill bmad-lens-quickplan`)
  - Adversarial review invocation (`--phase expressplan --source phase-complete`)
  - Phase-advance on `pass`/`pass-with-warnings` verdict
  - Phase-advance blocked on `fail` verdict
- Confirm shared skill prerequisites (`bmad-lens-quickplan`, `bmad-lens-bmad-skill`,
  `bmad-lens-adversarial-review`) exist in the target project; if absent, flag as a
  dependency gap.
- Verify `validate-phase-artifacts.py` is present for the review-ready fast path.

**Exit Criteria:**
- `lens-expressplan` registered in the discovery surface.
- All defined regression expectations pass.
- Prerequisite gaps are either confirmed present or flagged as tracked items.

---

## Epic 3 — Handoff Readiness

**Goal:** Confirm the feature is dev-ready and the planning PR can be merged.

**Scope:**
- Confirm planning PR #30 (`lens-dev-new-codebase-expressplan-plan` →
  `lens-dev-new-codebase-expressplan`) is ready to merge.
- Confirm no unresolved fail-level findings remain in `expressplan-review.md`
  or `finalizeplan-review.md`.
- Confirm all sprint plan exit criteria for Epics 1 and 2 are met.
- Signal `/dev` handoff.

**Exit Criteria:**
- Planning PR is merged.
- Feature phase is `finalizeplan-complete`.
- Dev handoff is signalled.
