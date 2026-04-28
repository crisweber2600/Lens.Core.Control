---
feature: lens-dev-new-codebase-businessplan
doc_type: sprint-plan
status: draft
goal: "Organize businessplan and techplan command rewrites into sequenced implementation stories for a single focused sprint."
key_decisions:
  - Businessplan and techplan rewrites are in the same sprint (they share a dependency chain)
  - Regression gate is a final blocking story before merge
  - Discovery surface verification (module-help, agent manifest) is minimal overhead — done in the same PR pass as regression
  - Stories are sized at 3–5 points each; total sprint capacity 13 points
open_questions: []
depends_on:
  - business-plan.md (this feature)
  - tech-plan.md (this feature)
blocks: []
updated_at: 2026-04-28T00:00:00Z
---

# Sprint Plan — Rewrite businessplan and techplan Commands

**Feature:** lens-dev-new-codebase-businessplan  
**Author:** crisweber2600  
**Date:** 2026-04-28  
**Total Estimate:** 13 story points  

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
**Blocks:** BP-2

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

---

### BP-2 — Rewrite techplan command as thin conductor

**Type:** Implementation  
**Points:** 5  
**Priority:** P0  
**Depends On:** BP-1 merged and green  
**Blocks:** BP-3

#### Goal

Rewrite `bmad-lens-techplan` SKILL.md and `lens-techplan.prompt.md` as a thin conductor that delegates shared patterns and enforces the PRD-reference rule for architecture generation.

#### Scope

- Rewrite `lens.core/_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md` via `bmad-module-builder`
- Rewrite `lens.core/_bmad/lens-work/prompts/lens-techplan.prompt.md` via `bmad-workflow-builder`
- Verify `.github/prompts/lens-techplan.prompt.md` stub chain is intact

#### Acceptance Criteria

1. `publish-to-governance --phase businessplan` is invoked before architecture authoring; no direct governance writes in the SKILL.md flow
2. Architecture generation via `bmad-lens-bmad-skill --skill bmad-create-architecture` is preceded by loading the staged PRD artifact from the control-repo docs path; architecture must reference `prd.md` (lifecycle `artifact_validation`)
3. Batch pass 1 delegates to `bmad-lens-batch --target techplan`, writes `techplan-batch-input.md`, and stops
4. Review-ready fast path invokes `validate-phase-artifacts.py --phase techplan --contract review-ready`; fast-path to adversarial review on `status=pass`
5. Adversarial review gate blocks `techplan-complete` transition on `fail` verdict
6. SKILL.md has no inline batch logic, no inline artifact existence checks, and no direct governance file writes

#### Implementation Notes

```
Channel:  bmad-module-builder for SKILL.md, bmad-workflow-builder for prompt
Branch:   feature/techplan-conductor in lens.core.src (or same branch as BP-1 if atomic — 
          both follow the identical conductor pattern and may be combined into one branch/PR)
Test target:  wrapper-equivalence + governance-audit + architecture-reference regression categories (see tech-plan.md §5)
```

---

### BP-3 — Regression gate and discovery surface verification

**Type:** Quality / Validation  
**Points:** 2  
**Priority:** P0  
**Depends On:** BP-1 and BP-2 merged  
**Blocks:** merge to develop

#### Goal

Run all required regression categories defined in tech-plan.md §5 and verify the 17-command discovery surface (module-help, agent manifest) is unchanged.

#### Scope

- Run wrapper-equivalence regression for businessplan (batch 2-pass, review-ready fast path, delegation routes)
- Run wrapper-equivalence regression for techplan (same)
- Run governance-audit regression for both (no direct writes, publish-before-author timing)
- Run architecture-reference regression for techplan (PRD reference enforcement)
- Verify `module-help.csv` contains correct entries for both `businessplan` and `techplan` commands
- Verify `agents/lens.agent.md` 17-command surface unchanged (both commands still listed)

#### Acceptance Criteria

1. All three regression categories green for businessplan: wrapper-equivalence, governance-audit
2. All three regression categories green for techplan: wrapper-equivalence, governance-audit, architecture-reference
3. `module-help.csv` has matching entries to old codebase for both commands
4. `agents/lens.agent.md` command surface unchanged

#### Implementation Notes

```
Location:  TargetProjects/lens-dev/new-codebase/lens.core.src/
Command:   uv run --with pytest pytest lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/ -q
           uv run --with pytest pytest lens.core/_bmad/lens-work/skills/bmad-lens-techplan/ -q
On fail:   Block merge; return to BP-1 or BP-2 for fix
```

**Note:** Wrapper-equivalence tests must explicitly cover the `/next` auto-delegation path for both commands — verify that when businessplan or techplan is invoked via `/next`, no redundant run-confirmation prompt appears and phase entry proceeds immediately.

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
| BP-2 | Rewrite techplan command as thin conductor | 5 | Not started |
| BP-3 | Regression gate and discovery surface verification | 2 | Not started |
| BP-4 | Feature closeout and governance phase advance | 1 | Not started |
| **Total** | | **13** | |

---

## Risk Register

| Risk | Story | Mitigation |
|------|-------|-----------|
| BP-1 clean-room rewrite misses batch pass-2 context loading | BP-1 | Explicit batch 2-pass equivalence test in regression gate |
| PRD reference enforcement not triggered via `/next` delegation path | BP-2 | Wrapper-equivalence test must cover `/next`-delegated invocation |
| Old codebase behavior consultation causes inadvertent copy | BP-1, BP-2 | BMB-first channel enforced; all SKILL.md content freshly authored via bmad-module-builder |
| Regression tests missing the publish-timing assertion | BP-3 | Governance-audit test explicitly checks sequence: publish-before-any-authoring |
