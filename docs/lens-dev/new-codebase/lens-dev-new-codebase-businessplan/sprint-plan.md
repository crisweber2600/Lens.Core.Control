---
feature: lens-dev-new-codebase-businessplan
doc_type: sprint-plan
status: draft
goal: "Organize businessplan command rewrite into sequenced implementation stories for a single focused sprint."
key_decisions:
  - Businessplan rewrite is the sole implementation story in this sprint (techplan deferred to lens-dev-new-codebase-techplan feature)
  - Regression gate is a final blocking story before merge
  - Discovery surface verification (module-help, agent manifest) is minimal overhead — done in the same PR pass as regression
  - Stories are sized at 3–5 points each; total sprint capacity 8 points
open_questions: []
depends_on:
  - business-plan.md (this feature)
  - tech-plan.md (this feature)
blocks: []
updated_at: 2026-04-28T00:00:00Z
---

# Sprint Plan — Rewrite businessplan Command

**Feature:** lens-dev-new-codebase-businessplan  
**Author:** crisweber2600  
**Date:** 2026-04-28  
**Total Estimate:** 8 story points  

---

## Pre-Sprint Checklist

Before any story in this sprint starts, verify the following are `done` in `lens.core.src/develop`:

- [ ] Story 1.4 — `publish-to-governance` entry hook (shared CLI with `--phase` arg)
- [ ] Story 3.1 — constitution partial hierarchy fix
- [ ] Story 4.1 — preplan rewrite (produces the staged preplan artifacts that businessplan will publish)

**Blocking policy:** Do not start BP-1 until all three checks pass.

**Verification command:** Run `#prompt:lens-preflight.prompt.md` against `lens-dev-new-codebase-businessplan` and confirm no blocking dependency gaps are reported before beginning BP-1.

---

## Sprint Stories

---

### BP-1 — Rewrite businessplan command as thin conductor

**Type:** Implementation  
**Points:** 5  
**Priority:** P0  
**Depends On:** pre-sprint checklist (1.4, 3.1, 4.1 done)  
**Blocks:** BP-3

#### Goal

Rewrite `bmad-lens-businessplan` SKILL.md and `lens-businessplan.prompt.md` as a thin conductor that delegates all shared patterns — batch, review-ready, publish-before-author — to the canonical shared implementations.

#### Scope

- Rewrite `lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/SKILL.md` via `bmad-module-builder`
- Rewrite `lens.core/_bmad/lens-work/prompts/lens-businessplan.prompt.md` via `bmad-workflow-builder`
- Verify `.github/prompts/lens-businessplan.prompt.md` stub chain is intact

#### Acceptance Criteria

1. `publish-to-governance --phase preplan` is invoked before any PRD or UX authoring; no direct governance writes anywhere in the SKILL.md flow
2. Batch pass 1 delegates to `bmad-lens-batch --target businessplan`, writes `businessplan-batch-input.md`, and stops; pass 2 resumes with pre-approved context
3. Review-ready fast path invokes `validate-phase-artifacts.py --phase businessplan --contract review-ready`; on `status=pass` it jumps to adversarial review without re-running the authoring delegation
4. PRD authoring routes to `bmad-lens-bmad-skill --skill bmad-create-prd`
5. UX design authoring routes to `bmad-lens-bmad-skill --skill bmad-create-ux-design`
6. Adversarial review gate blocks `businessplan-complete` transition on `fail` verdict
7. SKILL.md has no inline batch logic, no inline artifact existence checks, and no direct governance file writes

#### Implementation Notes

```
Channel:  bmad-module-builder for SKILL.md, bmad-workflow-builder for prompt
Branch:   feature/businessplan-conductor in lens.core.src
Test target:  wrapper-equivalence + governance-audit regression categories (see tech-plan.md §5)
```

> **FP-2 (FinalizePlan Review):** Preflight must confirm target-repo _merge_ state for baseline
> stories 1.4, 3.1, and 4.1 — not just feature-index phase status — before BP-1 begins.
> Run `#prompt:lens-preflight.prompt.md` against `lens-dev-new-codebase-businessplan` and
> verify all three stories show merged in `lens.core.src/develop`.

---

### BP-3 — Regression gate and discovery surface verification

**Type:** Quality / Validation  
**Points:** 2  
**Priority:** P0  
**Depends On:** BP-1 merged  
**Blocks:** merge to develop

#### Goal

Run all required regression categories defined in tech-plan.md §5 and verify the 17-command discovery surface (module-help, agent manifest) is unchanged for businessplan.

#### Scope

- Run wrapper-equivalence regression for businessplan (batch 2-pass, review-ready fast path, delegation routes)
- Run governance-audit regression for businessplan (no direct writes, publish-before-author timing)
- Verify `module-help.csv` contains correct entry for `businessplan` command
- Verify `agents/lens.agent.md` 17-command surface unchanged (`businessplan` still listed)

#### Acceptance Criteria

1. Both regression categories green for businessplan: wrapper-equivalence, governance-audit
2. `module-help.csv` has matching entry to old codebase for `businessplan`
3. `agents/lens.agent.md` command surface unchanged

#### Implementation Notes

```
Location:  TargetProjects/lens-dev/new-codebase/lens.core.src/
Command:   uv run --with pytest pytest lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/ -q
On fail:   Block merge; return to BP-1 for fix
```

**Note:** Wrapper-equivalence tests must explicitly cover the `/next` auto-delegation path — verify that when businessplan is invoked via `/next`, no redundant run-confirmation prompt appears and phase entry proceeds immediately.

> **FP-4 (FinalizePlan Review — scope corrected by Correct Course):** Architecture-reference regression category is not applicable to this feature (techplan deferred). This test category belongs to `lens-dev-new-codebase-techplan`.

> **FP-4 (FinalizePlan Review):** Architecture-reference regression applies to full-track
> techplan invocations only. Express-track does not produce `prd.md`; this test category
> is skipped for express-track test scenarios.

> **FP-5 (FinalizePlan Review):** On BP-3 green, this unblocks baseline stories 4.4
> (finalizeplan rewrite) and 4.5 (expressplan rewrite) in `lens.core.src`. Signal the
> baseline feature lead when BP-3 passes.

---

### BP-4 — Feature closeout and governance phase advance

**Type:** Governance / Closeout  
**Points:** 1  
**Priority:** P0  
**Depends On:** BP-3 green  

#### Goal

Commit all planning artifacts to the control repo plan branch, update governance with the feature phase transition, and advance to `expressplan-complete`.

#### Scope

- Commit `docs/lens-dev/new-codebase/lens-dev-new-codebase-businessplan/` artifacts to `lens-dev-new-codebase-businessplan-plan`
- Update `feature.yaml` phase to `expressplan-complete` with phase transition record
- Commit governance changes to `lens-governance/main`
- Mirror docs artifacts to governance docs path
- Report: ready for finalizeplan

#### Acceptance Criteria

1. Control repo `lens-dev-new-codebase-businessplan-plan` branch contains all three QuickPlan artifacts: `business-plan.md`, `tech-plan.md`, `sprint-plan.md`
2. `feature.yaml` phase is `expressplan-complete`
3. Governance mirror `features/lens-dev/new-codebase/lens-dev-new-codebase-businessplan/docs/` matches control repo docs path
4. Both commits pushed (control repo plan branch + governance main)

---

## Sprint Summary

| Story | Title | Points | Status |
|-------|-------|--------|--------|
| Pre-sprint | Dependency verification (1.4, 3.1, 4.1) | — | Not started |
| BP-1 | Rewrite businessplan command as thin conductor | 5 | Not started |
| BP-3 | Regression gate and discovery surface verification | 2 | Not started |
| BP-4 | Feature closeout and governance phase advance | 1 | Not started |
| **Total** | | **8** | |

---

## Risk Register

| Risk | Story | Mitigation |
|------|-------|-----------|
| BP-1 clean-room rewrite misses batch pass-2 context loading | BP-1 | Explicit batch 2-pass equivalence test in regression gate |
| Old codebase behavior consultation causes inadvertent copy | BP-1 | BMB-first channel enforced; all SKILL.md content freshly authored via bmad-module-builder |
| Regression tests missing the publish-timing assertion | BP-3 | Governance-audit test explicitly checks sequence: publish-before-any-authoring |
