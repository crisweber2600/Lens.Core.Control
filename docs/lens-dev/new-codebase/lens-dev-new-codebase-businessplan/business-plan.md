---
feature: lens-dev-new-codebase-businessplan
doc_type: business-plan
status: draft
goal: "Rewrite businessplan and techplan commands as thin conductors using shared utilities, achieving output parity with the old codebase while eliminating direct governance writes and copy-pasted inline patterns."
key_decisions:
  - Both businessplan and techplan rewrites are scoped in this feature given their hard dependency chain
  - Shared utilities (publish-to-governance, validate-phase-artifacts.py, bmad-lens-batch) replace inline copy-pasted patterns in both skills
  - BMB-first authoring channel enforced for all SKILL.md and release prompt changes
  - Clean-room re-implementation: old codebase consulted as behavioral reference only; new implementations authored from scratch against the architecture contract
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline (story 1.4 — publish-to-governance entry hook)
  - lens-dev-new-codebase-baseline (story 3.1 — constitution partial hierarchy fix)
  - lens-dev-new-codebase-baseline (story 4.1 — rewrite preplan)
blocks:
  - lens-dev-new-codebase-baseline (story 4.4 — rewrite finalizeplan)
  - lens-dev-new-codebase-baseline (story 4.5 — rewrite expressplan)
updated_at: 2026-04-28T00:00:00Z
---

# Business Plan — Rewrite businessplan and techplan Commands

**Feature:** lens-dev-new-codebase-businessplan  
**Author:** crisweber2600  
**Date:** 2026-04-28  
**Track:** express  

---

## 1. Executive Summary

The `businessplan` and `techplan` commands are the two middle phases of the full-track planning pipeline. They are currently implemented as monolithic conductors that embed copy-pasted shared logic for batch intake, review-ready gating, and governance publication. This feature rewrites both commands as thin conductors that delegate those shared patterns to the single canonical implementations introduced in the lens-work rewrite baseline.

The goal is behavioral preservation with structural simplification. Users running the full planning track should experience identical outcomes before and after this rewrite. Maintainers will see a dramatically reduced per-skill footprint — SKILL.md files that orchestrate via delegation calls rather than inline implementations.

This is a clean-room implementation. The old codebase is consulted as a behavioral reference to confirm what must be preserved, but no code or content is copied. The rewrite is authored fresh against the architecture contract defined in `lens-dev-new-codebase-baseline`.

---

## 2. Problem Statement

The old-codebase businessplan and techplan skills each contain:

1. **Inline batch intake logic** — if/else blocks for pass 1 / pass 2 detection, duplicating the `bmad-lens-batch` contract that is now the canonical implementation.
2. **Inline review-ready logic** — per-skill artifact existence checks that are now consolidated into `validate-phase-artifacts.py`.
3. **Direct governance file writes** — `businessplan` in the old codebase writes governance artifacts directly in some paths, violating the publish-before-author boundary introduced by story 1.4.
4. **Ad-hoc PRD-reference enforcement** — `techplan` enforces the PRD dependency rule via inline logic rather than a lifecycle-declared contract.

These patterns increase maintenance surface, create silent-break risk when shared contracts change, and make governance write audits harder to enforce automatically.

---

## 3. Stakeholders

| Stakeholder | Role | Interest |
|---|---|---|
| Lens users (full-track features) | Primary users | BusinessPlan and TechPlan phases work identically to the old codebase |
| Lens maintainers | Internal | Reduced skill footprint, governance audit integrity, testable delegation |
| Downstream phase skills (finalizeplan, expressplan) | Consumers | Clean artifact staging and governance publication handoffs |

---

## 4. Success Criteria

### Businessplan Command

| Criterion | Acceptance Test |
|---|---|
| Publish-before-author hook runs before any PRD or UX authoring | governance-audit regression: no businessplan direct writes |
| Batch 2-pass contract delegated to `bmad-lens-batch` | batch pass 1 stops, pass 2 resumes; no inline if/else in SKILL.md |
| Review-ready fast path delegated to `validate-phase-artifacts.py` | wrapper-equivalence regression: validate-phase-artifacts.py invoked before delegation |
| BMAD delegation preserved for PRD (`bmad-create-prd`) and UX (`bmad-create-ux-design`) | wrapper-equivalence regression: both routes produce same artifacts as old codebase |
| Adversarial review gate blocks phase completion on `fail` verdict | review-gate regression: fail blocks businessplan-complete transition |
| `businessplan-complete` phase transition committed to feature.yaml | feature-yaml regression: phase field updated correctly |

### TechPlan Command

| Criterion | Acceptance Test |
|---|---|
| Publish-before-author hook runs before architecture authoring | governance-audit regression: no techplan direct writes |
| PRD reference enforced in architecture generation | architecture-reference regression: architecture.md references prd.md |
| Shared batch and review-ready patterns delegated | wrapper-equivalence regression: no inline duplication in SKILL.md |
| Architecture delegation via `bmad-create-architecture` through `bmad-lens-bmad-skill` | wrapper-equivalence regression: architecture output matches old codebase |
| Adversarial review gate blocks phase completion on `fail` verdict | review-gate regression: fail blocks techplan-complete transition |
| `techplan-complete` phase transition committed to feature.yaml | feature-yaml regression: phase field updated correctly |

---

## 5. Scope

### In Scope

- Rewrite `skills/bmad-lens-businessplan/SKILL.md` — thin conductor
- Rewrite `skills/bmad-lens-techplan/SKILL.md` — thin conductor
- Rewrite `lens.core/_bmad/lens-work/prompts/lens-businessplan.prompt.md` — release prompt
- Rewrite `lens.core/_bmad/lens-work/prompts/lens-techplan.prompt.md` — release prompt
- Update `.github/prompts/lens-businessplan.prompt.md` — stub (verify chain integrity)
- Update `.github/prompts/lens-techplan.prompt.md` — stub (verify chain integrity)
- Regression coverage: wrapper-equivalence, governance-audit, architecture-reference tests

### Out of Scope

- Changes to `bmad-lens-batch`, `validate-phase-artifacts.py`, or `git-orchestration-ops.py` — those shared implementations are already delivered by dependent stories
- Changes to `bmad-create-prd`, `bmad-create-ux-design`, or `bmad-create-architecture` — those are authoring skills, not conductors
- FinalizePlan or ExpressPlan rewrites — those are separate features (4.4, 4.5)
- New businessplan or techplan capabilities beyond parity

---

## 6. Business Value and Risk

### Value

- Eliminates governance write audit gaps in two of the most-used planning commands
- Reduces per-skill maintenance surface from ~150+ lines of inline logic to ~30-line conductor delegation chains
- Makes the shared utility contract changes (batch, review-ready, publish-hook) effective across the full planning pipeline
- Produces a reuse proof-point: two skills that independently demonstrate the same conductor pattern can be updated atomically when shared contracts evolve

### Risks

| Risk | Likelihood | Mitigation |
|---|---|---|
| Regression in PRD reference enforcement during clean-room rewrite | Medium | Architecture-reference regression check required before merge |
| Batch 2-pass context loss if delegation boundary is placed incorrectly | Medium | Explicit pass 1 / pass 2 equivalence test against old codebase behavior |
| Publish-before-author hook not triggered when phase is invoked via `/next` | Low | Wrapper-equivalence test covers the `/next` delegated-invocation path |
| techplan skips publish hook when businessplan artifacts are partial | Low | Publish-to-governance contract validates source artifact presence before writing |

---

## 7. Dependencies

### Prerequisites (must be complete before BP-1)

| Story | Description |
|---|---|
| 1.4 — publish-to-governance entry hook | Shared publish CLI with `--phase` argument available |
| 3.1 — constitution partial hierarchy fix | Constitution resolution stable for businessplan/techplan validation |
| 4.1 — rewrite preplan | PrePlan artifacts staged and reviewable as businessplan publish source |

### What This Unblocks

| Story | Description |
|---|---|
| 4.4 — rewrite finalizeplan | Requires businessplan and techplan to produce correctly staged and governed artifacts |
| 4.5 — rewrite expressplan | Requires stable businessplan and techplan rewrite contracts as reference |
