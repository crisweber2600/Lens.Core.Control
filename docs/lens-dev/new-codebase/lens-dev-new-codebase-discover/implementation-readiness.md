---
feature: lens-dev-new-codebase-discover
doc_type: implementation-readiness
status: approved
goal: "Assess readiness for dev execution of the discover command rewrite across 8 stories"
key_decisions:
  - Readiness verdict: READY — proceed to /dev
  - Story 5.4.9 integration test is a hard dev-complete gate; does not block sprint start
  - No-remote edge case and config key stability deferred; not blocking
depends_on: [epics, stories, finalizeplan-review]
blocks: []
updated_at: 2026-04-29T00:00:00Z
---

# Implementation Readiness — Discover Command

**Feature:** `lens-dev-new-codebase-discover`
**Author:** FinalizePlan bundle (lens-finalizeplan)
**Date:** 2026-04-29
**Verdict:** **READY** — proceed to `/dev`

---

## Readiness Assessment

### Planning Artifact Completeness

| Artifact | Status | Notes |
|---|---|---|
| `business-plan.md` | ✓ Approved | Goals, scope, stakeholders defined |
| `tech-plan.md` | ✓ Approved | Architecture decisions, test matrix, script interface |
| `sprint-plan.md` | ✓ Approved | 7 original stories; Story 5.4.9 added via OQ-FP3 |
| `expressplan-adversarial-review.md` | ✓ Pass-with-warnings | 5 medium + 1 high finding; all mapped to pre-work |
| `finalizeplan-review.md` | ✓ Approved | OQ-FP1/2 deferred; OQ-FP3 resolved as Story 5.4.9 |
| `epics.md` | ✓ Complete | Single epic; 8 stories listed |
| `stories.md` | ✓ Complete | Full ACs; Story 5.4.9 added |

### Prerequisites for Dev Start

| Check | Status | Detail |
|---|---|---|
| Baseline SKILL.md stub exists | Assumed ✓ | `lens.core/_bmad/lens-work/skills/bmad-lens-discover/SKILL.md` — to verify in Story 5.4.1 |
| `discover-ops.py` baseline script exists | Assumed ✓ | From baseline rewrite; to verify before 5.4.3 |
| Test file scaffold exists | Assumed ✓ | `scripts/tests/test-discover-ops.py` from baseline |
| Feature branches set up | ✓ Confirmed | `lens-dev-new-codebase-discover` and `-plan` branches exist |
| Planning PR merged | ✓ Confirmed | PR #20 merged |
| Governance docs mirror current | ✓ Confirmed | business-plan, tech-plan, sprint-plan, expressplan-adversarial-review published |

### Story 0 Pre-Work Items (address before Story 5.4.2 coding starts)

These are carried from the expressplan adversarial review as pre-work items and must be resolved during Story 5.4.1 before coding begins on Story 5.4.2:

| Finding | Resolution Target | Status |
|---|---|---|
| H1 — Story 5.4.1 scope unbounded without pre-assessment | Story 5.4.1: add bounded assessment step as first AC | Pre-work item |
| M1 — Hash comparison ownership ambiguous | Story 5.4.2 before coding: confirm inline-skill vs script-subcommand | Pre-work item |
| M3 — No-remote edge case unhandled | Deferred (OQ-FP2 — E). Note in SKILL.md out-of-scope section | Deferred |
| M4 — Dry-run regression test absent | Story 5.4.4 scope: add T-dry-run if feasible within story points | Pre-work item |
| M5 — T8 validates scan not commit absence | Story 5.4.5 scope: T8 must explicitly verify no-commit path | Pre-work item |
| M2 — SC-5 vs Story 5.4.7 conflict | Story 5.4.7 scope: confirm audit-only vs remediation | Pre-work item |

---

## Risk Assessment

### High Risks

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Story 5.4.1 scope expansion (H1) | Medium | High | Bounded assessment step as first AC in 5.4.1; escalate if scope change exceeds 1 session |
| Baseline files missing or incomplete | Low | High | Story 5.4.1 explicitly audits SKILL.md; 5.4.3 explicitly audits script — surface gaps early |

### Medium Risks

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Hash comparison ownership ambiguity (M1) | Medium | Medium | Resolve in 5.4.1 pre-work before 5.4.2 coding starts |
| Path resolution bugs on Windows (M4-adjacent) | Low | Medium | Story 5.4.3 explicit AC; tests must pass on both POSIX and Windows |
| Story 5.4.9 integration test complexity | Medium | Medium | Use local bare-git repos; no network required; allocate extra story points |

### Low Risks / Deferred

| Risk | Status |
|---|---|
| No-remote edge case | Deferred (OQ-FP2 E) |
| Config key stability (upgrade dependency) | Deferred (OQ-FP1 E) — note against upgrade TechPlan |
| Integration test scope creep (5.4.9) | Story points set to 3; escalate if additional scenarios required |

---

## Sequencing Recommendation

```
Sprint 1 start
  ↓
Story 5.4.1 (SKILL.md finalize) — first; unblocks everything downstream
  ├── → Story 5.4.2 (conditional commit guard) — depends on 5.4.1
  │       └── → Story 5.4.5 (no-op tests) — depends on 5.4.2
  │       └── → Story 5.4.9 (integration smoke) — depends on 5.4.1 + 5.4.2
  ├── → Story 5.4.3 (path resolve) — parallel with 5.4.2
  │       └── → Story 5.4.4 (missing-disk tests) — depends on 5.4.3
  ├── → Story 5.4.6 (prompt chain) — independent; any time after 5.4.1
  └── → Story 5.4.7 (isolation audit) — independent; run last
  ↓
dev-complete: all stories done + 5.4.9 integration test passes
```

---

## Readiness Verdict

**READY — no blocking conditions identified.**

All planning artifacts are complete. Pre-work items are documented and mapped to specific stories. Deferred items (OQ-FP1, OQ-FP2) are recorded but not blocking. Story 5.4.9 is a hard dev-complete gate, not a sprint-start blocker.

**Recommended next action:** `/dev` — start with Story 5.4.1.
