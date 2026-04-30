---
feature: lens-dev-new-codebase-businessplan
doc_type: implementation-readiness
status: approved
goal: "Assess readiness of lens-dev-new-codebase-businessplan to proceed to /dev."
key_decisions:
  - Feature is ready for dev with known pre-sprint dependency checks required before BP-1 starts
  - No blocking open questions; all FinalizePlan review findings resolved or accepted
open_questions: []
depends_on:
  - finalizeplan-review.md
  - epics.md
  - stories.md
blocks: []
updated_at: 2026-04-28T02:00:00Z
---

# Implementation Readiness — lens-dev-new-codebase-businessplan

**Feature:** lens-dev-new-codebase-businessplan  
**Track:** express  
**Phase Gate:** finalizeplan → dev-ready  
**Assessment Date:** 2026-04-28  
**Assessor:** lens-finalizeplan  

---

## Readiness Assessment

**Overall Verdict: READY (with pre-sprint dependency gate)**

The feature is fully planned and reviewed. All planning artifacts are present, reviewed with a `pass-with-warnings` verdict, and the planning PR has been merged. One conditional gate must pass before BP-1 begins: the three baseline dependency stories must be confirmed merged in `lens.core.src/develop` via preflight.

### Planning Completeness

| Artifact | Present | Reviewed | Status |
|----------|---------|----------|--------|
| business-plan.md | ✓ | ✓ (expressplan-review) | Approved |
| tech-plan.md | ✓ | ✓ (expressplan-review) | Approved |
| sprint-plan.md | ✓ | ✓ (expressplan-review) | Approved |
| expressplan-review.md | ✓ | N/A (is the review) | pass-with-warnings |
| finalizeplan-review.md | ✓ | N/A (is the review) | pass-with-warnings |
| epics.md | ✓ | — | Draft approved |
| stories.md | ✓ | — | Draft approved |
| sprint-status.yaml | ✓ | — | Draft |

### Story Readiness

| Story | Dev Notes Complete | Acceptance Criteria | Test Target Defined |
|-------|-------------------|---------------------|---------------------|
| BP-1 | ✓ | 8 criteria (all testable) | ✓ (wrapper-equivalence, governance-audit) |
| BP-3 | ✓ | 3 criteria | ✓ (pytest target defined) |
| BP-4 | ✓ | 4 criteria | N/A (governance closeout) |

### FinalizePlan Review Findings Status

All findings from `finalizeplan-review.md` are resolved or have accepted risk decisions:

| Finding | Resolution | Status |
|---------|-----------|--------|
| FP-1 — techplan scope overlap | D: Split BP-2 to techplan feature; techplan reactivated | ✓ Resolved (Correct Course applied) |
| FP-2 — dependency verification gap | B: Preflight confirms merge state; note added to BP-1 | ✓ Applied |
| FP-3 — feature-index.yaml stale | A: feature.yaml + feature-index updated | ✓ Applied |
| FP-4 — architecture-reference regression scope | A: Clarifying note added to BP-3 | ✓ Applied |
| FP-5 — unblocking signal undefined | B: Unblocking note added to BP-3 | ✓ Applied |

---

## Risk Assessment

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|------------|
| Baseline dependency stories not merged when BP-1 starts | Medium | High — BP-1 would block mid-implementation | Pre-sprint checklist + preflight merge-state check (mandatory, not advisory) |
| BP-1 clean-room rewrite misses batch pass-2 context loading | Low | Medium — silent regression | Explicit batch 2-pass equivalence test in BP-3 regression gate |
| BP-3 regression not covering `/next` auto-delegation path | Low | Medium — `/next` behavior regression | `/next` path explicitly called out in BP-3 notes |
| techplan scope (BP-2) resurfaces in this feature | Low | Low — governance documents split decision | FP-1 resolved by Correct Course; techplan reactivated in governance |

---

## Pre-Sprint Gate

**Gate:** Before BP-1 starts, the following baseline stories must be confirmed merged in `lens.core.src/develop`:

1. Story 1.4 — `publish-to-governance` shared CLI entry hook
2. Story 3.1 — constitution partial hierarchy fix
3. Story 4.1 — preplan rewrite

**Verification:** Run `#prompt:lens-preflight.prompt.md` against `lens-dev-new-codebase-businessplan` and confirm all three stories show `merged` in `lens.core.src/develop` (not just feature-index phase status).

---

## Dev Signal

All planning conditions are satisfied. Proceed to `/dev` after the pre-sprint gate passes.

- **Story sequencing:** BP-1 → BP-3 → BP-4 (linear, no parallel execution)
- **Target repo:** `TargetProjects/lens-dev/new-codebase/lens.core.src/`
- **Branch convention:** `feature/businessplan-conductor` in `lens.core.src`
- **Unblocking signal:** BP-3 green unblocks `lens-dev-new-codebase-baseline` stories 4.4 and 4.5
