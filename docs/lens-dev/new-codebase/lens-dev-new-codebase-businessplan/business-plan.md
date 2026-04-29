---
feature: lens-dev-new-codebase-businessplan
doc_type: business-plan
status: draft
goal: "Rewrite businessplan command as a thin conductor using shared utilities, achieving output parity with the old codebase while eliminating direct governance writes and copy-pasted inline patterns. TechPlan rewrite deferred to lens-dev-new-codebase-techplan."
key_decisions:
  - Businessplan rewrite only — techplan deferred to lens-dev-new-codebase-techplan feature (dependency chain rationale does not require co-scoping)
  - Shared utilities (publish-to-governance, validate-phase-artifacts.py, bmad-lens-batch) replace inline copy-pasted patterns in businessplan
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

# Business Plan — Rewrite businessplan Command

**Feature:** lens-dev-new-codebase-businessplan  
**Author:** crisweber2600  
**Date:** 2026-04-28  
**Track:** express  

---

## 1. Executive Summary

The `businessplan` command is the first of the two middle phases in the full-track planning pipeline. It is currently implemented as a monolithic conductor that embeds copy-pasted shared logic for batch intake, review-ready gating, and governance publication. This feature rewrites the businessplan command as a thin conductor that delegates those shared patterns to the single canonical implementations introduced in the lens-work rewrite baseline.

The techplan command rewrite is deferred to the `lens-dev-new-codebase-techplan` feature, which is the canonical scope owner for that deliverable.

The goal is behavioral preservation with structural simplification. Users running the full planning track should experience identical businessplan outcomes before and after this rewrite. Maintainers will see a dramatically reduced per-skill footprint — a SKILL.md that orchestrates via delegation calls rather than inline implementations.

This is a clean-room implementation. The old codebase is consulted as a behavioral reference to confirm what must be preserved, but no code or content is copied. The rewrite is authored fresh against the architecture contract defined in `lens-dev-new-codebase-baseline`.

---

## 2. Problem Statement

The old-codebase businessplan skill contains:

1. **Inline batch intake logic** — if/else blocks for pass 1 / pass 2 detection, duplicating the `bmad-lens-batch` contract that is now the canonical implementation.
2. **Inline review-ready logic** — per-skill artifact existence checks that are now consolidated into `validate-phase-artifacts.py`.
3. **Direct governance file writes** — `businessplan` in the old codebase writes governance artifacts directly in some paths, violating the publish-before-author boundary introduced by story 1.4.

These patterns increase maintenance surface, create silent-break risk when shared contracts change, and make governance write audits harder to enforce automatically.

---

## 3. Stakeholders

| Stakeholder | Role | Interest |
|---|---|---|
| Lens users (full-track features) | Primary users | BusinessPlan phase works identically to the old codebase |
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

---

## 5. Scope

### In Scope

- Rewrite `skills/bmad-lens-businessplan/SKILL.md` — thin conductor
- Rewrite `lens.core/_bmad/lens-work/prompts/lens-businessplan.prompt.md` — release prompt
- Update `.github/prompts/lens-businessplan.prompt.md` — stub (verify chain integrity)
- Regression coverage: wrapper-equivalence, governance-audit tests

### Out of Scope

- TechPlan command rewrite — deferred to `lens-dev-new-codebase-techplan` feature
- Changes to `bmad-lens-batch`, `validate-phase-artifacts.py`, or `git-orchestration-ops.py` — those shared implementations are already delivered by dependent stories
- Changes to `bmad-create-prd`, `bmad-create-ux-design`, or `bmad-create-architecture` — those are authoring skills, not conductors
- FinalizePlan or ExpressPlan rewrites — those are separate features (4.4, 4.5)
- New businessplan capabilities beyond parity

---

## 6. Business Value and Risk

### Value

- Eliminates governance write audit gaps in the businessplan command
- Reduces businessplan's per-skill maintenance surface from ~150+ lines of inline logic to a ~30-line conductor delegation chain
- Makes the shared utility contract changes (batch, review-ready, publish-hook) effective for the businessplan phase of the planning pipeline
- Produces a reuse proof-point: a skill that demonstrates the conductor pattern and can be updated atomically when shared contracts evolve

### Risks

| Risk | Likelihood | Mitigation |
|---|---|---|
| Batch 2-pass context loss if delegation boundary is placed incorrectly | Medium | Explicit pass 1 / pass 2 equivalence test against old codebase behavior |
| Publish-before-author hook not triggered when phase is invoked via `/next` | Low | Wrapper-equivalence test covers the `/next` delegated-invocation path |

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
| 4.4 — rewrite finalizeplan | Requires businessplan to produce correctly staged and governed artifacts (techplan tracked separately in `lens-dev-new-codebase-techplan`) |
| 4.5 — rewrite expressplan | Requires stable businessplan rewrite contract as reference (techplan planned separately) |
