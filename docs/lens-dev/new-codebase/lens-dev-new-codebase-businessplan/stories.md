---
feature: lens-dev-new-codebase-businessplan
doc_type: stories
status: approved
goal: "Implementation stories for the businessplan thin conductor rewrite — ordered by dependency chain."
key_decisions:
  - Three stories total: BP-1 (rewrite), BP-3 (regression gate), BP-4 (closeout)
  - Linear execution — no parallelism; BP-3 blocks BP-4, BP-1 blocks BP-3
  - Clean-room implementation channel: bmad-module-builder for SKILL.md, bmad-workflow-builder for prompt
open_questions: []
depends_on:
  - epics.md (EP-1)
blocks: []
updated_at: 2026-04-28T02:00:00Z
---

# Stories — Rewrite businessplan Command

**Feature:** lens-dev-new-codebase-businessplan  
**Epic:** EP-1 — Businessplan Conductor Rewrite  
**Track:** express  
**Author:** crisweber2600  
**Date:** 2026-04-28  

---

## Story List

---

### BP-1 — Rewrite businessplan command as thin conductor

**Epic:** EP-1  
**Type:** Implementation  
**Story Points:** 5  
**Priority:** P0  
**Status:** Not started  
**Depends On:** Pre-sprint checklist (stories 1.4, 3.1, 4.1 merged in `lens.core.src/develop`)  
**Blocks:** BP-3  

#### Story

As a Lens maintainer,  
I want the businessplan command rewritten as a thin conductor that delegates shared patterns to canonical shared utilities,  
so that businessplan behavior is preserved while eliminating inline copy-pasted batch logic, review-ready logic, and direct governance writes.

#### Acceptance Criteria

1. `publish-to-governance --phase preplan` is invoked before any PRD or UX authoring; no direct governance writes anywhere in the SKILL.md flow
2. Batch pass 1 delegates to `bmad-lens-batch --target businessplan`, writes `businessplan-batch-input.md`, and stops; pass 2 resumes with pre-approved context
3. Review-ready fast path invokes `validate-phase-artifacts.py --phase businessplan --contract review-ready`; on `status=pass` it jumps to adversarial review without re-running the authoring delegation
4. PRD authoring routes to `bmad-lens-bmad-skill --skill bmad-create-prd`
5. UX design authoring routes to `bmad-lens-bmad-skill --skill bmad-create-ux-design`
6. Adversarial review gate blocks `businessplan-complete` transition on `fail` verdict
7. SKILL.md has no inline batch logic, no inline artifact existence checks, and no direct governance file writes
8. When businessplan is invoked via `/next`, no redundant run-confirmation prompt appears and phase entry proceeds immediately

#### Implementation Notes

- **Channel:** `bmad-module-builder` for SKILL.md, `bmad-workflow-builder` for prompt
- **Branch:** `feature/businessplan-conductor` in `lens.core.src`
- **Test target:** wrapper-equivalence + governance-audit regression categories (see tech-plan.md §5)
- **Preflight note (FP-2):** Preflight must confirm target-repo *merge* state for baseline stories 1.4, 3.1, and 4.1 — not just feature-index phase status — before BP-1 begins.

---

### BP-3 — Regression gate and discovery surface verification

**Epic:** EP-1  
**Type:** Quality / Validation  
**Story Points:** 2  
**Priority:** P0  
**Status:** Not started  
**Depends On:** BP-1 merged  
**Blocks:** BP-4 (merge to develop)  

#### Story

As a Lens maintainer,  
I want regression tests to confirm businessplan wrapper-equivalence and governance-audit compliance,  
so that the thin conductor rewrite does not introduce silent behavioral regressions or governance write violations.

#### Acceptance Criteria

1. Both regression categories green for businessplan: wrapper-equivalence, governance-audit
2. `module-help.csv` has matching entry to old codebase for `businessplan`
3. `agents/lens.agent.md` command surface unchanged (17-command surface, `businessplan` still listed)

#### Implementation Notes

- **Location:** `TargetProjects/lens-dev/new-codebase/lens.core.src/`
- **Command:** `uv run --with pytest pytest lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/ -q`
- **On fail:** Block merge; return to BP-1 for fix
- **Scope note (FP-4):** Architecture-reference regression applies to full-track techplan invocations only. Express-track does not produce `prd.md`; this test category is skipped for express-track test scenarios. Architecture-reference regression belongs to `lens-dev-new-codebase-techplan`.
- **Unblocking signal (FP-5):** On BP-3 green, this unblocks baseline stories 4.4 (finalizeplan rewrite) and 4.5 (expressplan rewrite) in `lens.core.src`. Signal the baseline feature lead when BP-3 passes.
- **`/next` path (expressplan-review BP-1):** Wrapper-equivalence tests must explicitly cover the `/next` auto-delegation path — verify that when businessplan is invoked via `/next`, no redundant run-confirmation prompt appears and phase entry proceeds immediately.

---

### BP-4 — Feature closeout and governance phase advance

**Epic:** EP-1  
**Type:** Governance / Closeout  
**Story Points:** 1  
**Priority:** P0  
**Status:** Not started  
**Depends On:** BP-3 green  
**Blocks:** Nothing (terminal story)  

#### Story

As a Lens feature lead,  
I want all planning artifacts committed to the control repo and governance updated to `finalizeplan-complete`,  
so that the feature is fully closed out and downstream features (4.4, 4.5) can be unblocked via governance state.

#### Acceptance Criteria

1. Control repo `lens-dev-new-codebase-businessplan` branch contains all five planning artifacts: `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, `expressplan-review.md`, `finalizeplan-review.md`
2. `feature.yaml` phase is `finalizeplan-complete` with phase transition record
3. Governance mirror `features/lens-dev/new-codebase/lens-dev-new-codebase-businessplan/docs/` matches control repo docs path
4. Both commits pushed (control repo main + governance main)

#### Implementation Notes

- Update `feature.yaml` via `bmad-lens-feature-yaml` after BP-3 green
- Mirror docs via `publish-to-governance --phase finalizeplan`

---

## Story Summary

| Story | Title | Points | Priority | Status | Dependencies |
|-------|-------|--------|----------|--------|--------------|
| BP-1 | Rewrite businessplan command as thin conductor | 5 | P0 | Not started | Pre-sprint checklist |
| BP-3 | Regression gate and discovery surface verification | 2 | P0 | Not started | BP-1 merged |
| BP-4 | Feature closeout and governance phase advance | 1 | P0 | Not started | BP-3 green |
| **Total** | | **8** | | | |
