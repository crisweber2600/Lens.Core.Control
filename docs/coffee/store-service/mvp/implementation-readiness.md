---
feature: coffee-store-service-mvp
doc_type: implementation-readiness
status: draft
goal: "Assess whether StoreOperationsService MVP is ready to advance to dev-ready and identify all blocking gates"
key_decisions:
  - "dev-ready gate requires C1 (business-plan) and C2 (aspire-plan) SPIKEs to be closed first"
  - "E2 stories are deferred to Sprint 3 pending design resolution"
  - "Architecture approval blockers from architecture.md must be resolved before pre-implementation spikes close"
open_questions: []
depends_on: []
blocks: []
updated_at: "2025-01-15T00:00:00Z"
---

# Implementation Readiness — StoreOperationsService MVP

## Overall Assessment

| Dimension | Status | Notes |
|-----------|--------|-------|
| Planning completeness | ⚠️ CONDITIONAL | 4 planning docs present; 2 required governance artifacts missing (C1, C2) |
| Architecture approval | ⚠️ CONDITIONAL | 2 pre-implementation blockers declared in `architecture.md` |
| Domain constitution compliance | 🔴 FAILING | coffee hard gates C1 + C2 not satisfied |
| Open design questions | ⚠️ PARTIAL | 6 open questions in PRD, 2 open questions in architecture (M1–M3) |
| Story coverage | ✅ COMPLETE | All 23 stories authored with acceptance criteria |
| Sprint plan | ✅ COMPLETE | 4-sprint plan with explicit blocker sequencing |
| Governance mirror | ⚠️ PENDING | TechPlan artifacts to be published after dev-ready promotion |

**Verdict: NOT READY for dev promotion.** Two constitution hard gates must be resolved (SB1, SB2) before `phase` can advance to `dev-ready`. All other planning work is complete and dev can begin on unblocked stories in parallel.

---

## Constitution Gates

### C1 — business-plan.md Required (FAILING)

| Field | Value |
|-------|-------|
| Constitution | coffee domain |
| Gate mode | hard |
| Required artifact | `business-plan.md` |
| Current state | MISSING |
| Blocking | dev-ready phase promotion |
| Resolution | Complete SB1 (Sprint 0) |

The coffee domain constitution (`constitutions/coffee/constitution.md`) lists `business-plan` as a required planning artifact with `gate_mode: hard`. No `business-plan.md` exists in `docs/coffee/store-service/mvp/`. Until SB1 is resolved, the milestone **cannot advance to `dev-ready`**.

---

### C2 — aspire-plan.md Required (FAILING)

| Field | Value |
|-------|-------|
| Constitution | coffee domain (Aspire Mandate) |
| Gate mode | hard |
| Required artifact | `aspire-plan.md` |
| Current state | MISSING |
| Blocking | dev-ready promotion + S5.2 (Aspire AppHost registration) |
| Resolution | Complete SB2 (Sprint 0) |

The Aspire Mandate clause states all coffee services MUST use the shared .NET Aspire AppHost. No `aspire-plan.md` documenting the AppHost integration exists. Until SB2 is resolved, S5.2 cannot proceed and the dev-ready gate cannot be satisfied.

---

## Architecture Approval Blockers

These are declared in `architecture.md` under `blocks:` frontmatter. They represent decisions the architecture flags as requiring stakeholder approval **before implementation begins** for the affected components.

### A1 — Rush-Authority Matrix (pre-implementation approval required)

| Field | Value |
|-------|-------|
| Source | `architecture.md` `blocks` frontmatter |
| Affected stories | S2.2 (rush designation), S2.3 (at-risk escalation) |
| Resolution | SB3 SPIKE |

The rush-authority matrix defines which principals may designate an order as rush, and what escalation thresholds trigger at-risk classification. Without this decision, S2.2 cannot be implemented per architecture intent.

### A2 — Cancellation Taxonomy (pre-implementation approval required)

| Field | Value |
|-------|-------|
| Source | `architecture.md` `blocks` frontmatter |
| Affected stories | S1.6 (cancellation path) |
| Resolution | SB4 SPIKE (Q3: post-prep classification) |

The cancellation taxonomy determines whether post-preparation-start cancellations have a distinct classification from pre-start cancellations, and whether this affects the event schema. S1.6 can be implemented with a conditional, but cannot be closed until SB4 Q3 is resolved.

---

## Open Design Questions Status

| # | Question | From | Sprint | Blocking | SPIKE |
|---|----------|------|--------|----------|-------|
| Q1 | Rush qualification criteria and authority | PRD M1 | 0 | S2.2, S2.3 | SB3 |
| Q2 | Completion event presence and CustomerService callback | PRD M1 | 0 | S1.5, S3.4 | SB4 |
| Q3 | Post-prep cancellation classification | PRD M1 | 0 | S1.6 | SB4 |
| Q4 | SLA threshold values for at-risk escalation | PRD M1 | 0 | S2.3 | SB3 |
| Q5 | Reason codes for cancellation | PRD M1 | 0 | S1.6 | SB4 |
| Q6 | Multi-store routing scope for MVP | PRD M1 | 0 | scope | SB4 |

---

## Risk Register

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|------------|
| SB1/SB2 SPIKEs take longer than Sprint 0 budget | Medium | High — blocks dev-ready | Assign dedicated owner to each SPIKE at sprint kick-off |
| Rush-authority matrix reveals conflicting stakeholder expectations (M2) | Medium | Medium — redesign S2.2 | SB3 workshop with Shift Lead persona representative |
| Queue ordering algorithm changes late in Sprint 3 | Low | High — S2.1 rework | Freeze ordering decision as part of SB3 output before S2.1 starts |
| CustomerService integration event schema not stable at time of S4.1 | Medium | Medium — S4.1 blocked | Define stub/contract-first consumer; agree schema version with CustomerService team |
| `completed → cancelled` guard has edge case with concurrent operations | Medium | Medium — data integrity | Use database-level serialisable transaction for lifecycle transitions |
| Outbox publisher deduplication gap (m1 minor) | Low | Low for MVP | Document at-least-once delivery contract; implement idempotent consumers in E4 |

---

## Unblocked Stories (Safe to Start Sprint 1)

The following stories have no blockers and can begin as soon as Sprint 0 SPIKEs are assigned (they do not need to be closed first):

| Story | Epic | Points | Note |
|-------|------|--------|------|
| S5.1 | E5 Foundation | 3 | Scaffold only; independent of SPIKEs |
| S5.3 | E5 Foundation | 3 | EF Core migration; independent of S5.2 |
| S1.1 | E1 State Machine | 5 | Domain model; no persistence dependency |
| S3.3 | E3 Outbox | 3 | Event Bus Adapter abstraction only |
| S3.4 | E3 Outbox | 3 | Event schema; SB4 Q2 conditional |

**Total immediately unblockable: 17 pts** across 5 stories.

---

## Readiness Checklist

| Check | Status | Owner |
|-------|--------|-------|
| Planning docs authored (prd, architecture, product-brief) | ✅ | finalizeplan |
| Adversarial review completed | ✅ | finalizeplan-review.md |
| Epics authored | ✅ | epics.md |
| Stories authored with acceptance criteria | ✅ | stories.md |
| Sprint plan produced | ✅ | sprint-status.yaml |
| Individual story files created (E6 + E1 prioritised) | ✅ | stories/ |
| `business-plan.md` authored (C1) | 🔴 MISSING | TBD |
| `aspire-plan.md` authored (C2) | 🔴 MISSING | TBD |
| Rush-authority matrix resolved (M2/SB3) | 🔴 MISSING | TBD |
| Open questions resolved (M1/SB4) | 🔴 MISSING | TBD |
| Store-service constitution authored (M4) | ⚠️ OPTIONAL | Post-MVP |
| Governance mirror updated | ⚠️ PENDING | Post dev-ready |

**Gate summary: 4 MISSING items must close before dev-ready. 0 items block Sprint 1 unblocked stories.**
