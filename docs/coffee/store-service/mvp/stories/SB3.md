---
storyId: SB3
title: "Resolve rush-authority matrix and queue ordering rules (M2 + M3)"
epic: E6
feature: coffee-store-service-mvp
type: spike
priority: HIGH
points: 3
sprint: 0
status: not-started
owner: TBD
blocked_reason: "None — this story IS a blocker for Sprint 3 (E2)"
blocks:
  - "S2.1 — Queue ingestion and ordering algorithm"
  - "S2.2 — Rush designation"
  - "S2.3 — At-risk escalation"
---

# SB3 — Resolve Rush-Authority Matrix and Queue Ordering Rules

## Context

`architecture.md` declares the rush-authority matrix and queue cancellation taxonomy as **pre-implementation blockers**. Without a written decision on these two areas, Sprint 3 stories S2.1, S2.2, and S2.3 cannot be implemented to specification.

Two gaps from `finalizeplan-review.md` scope this SPIKE:

- **M2 (Rush-authority matrix)** — No specification for who may designate an order as rush; the queue engine cannot enforce access control without it.
- **M3 (Queue ordering rules)** — The queue ordering algorithm within a priority band is unspecified. FIFO, deadline-driven, and manual override are all possible; each produces different barista UX.

## User Story

**As a** developer implementing E2 Queue and Priority Engine,  
**I want** a written decision on the rush-authority matrix and queue ordering algorithm,  
**so that** S2.1, S2.2, and S2.3 can be implemented with a clear, approved specification.

## Key Questions to Resolve

### Rush-Authority Matrix (M2)
1. Which roles may designate an order as rush? (Barista only? Shift Lead only? Customer-service signal?)
2. Can rush designation be removed? If so, by whom?
3. Does rush designation change the queue position immediately, or does it take effect on next queue evaluation cycle?
4. Is there a limit on the number of concurrent rush-designated orders?

### Queue Ordering Algorithm (M3)
1. Default ordering within a priority band — FIFO by `received_at` timestamp, or by order-promise deadline?
2. How does rush designation interact with the priority banding? (Rush = new band? Or position override within existing band?)
3. At what granularity is queue position re-evaluated? Per transition? Background timer?

### At-Risk Escalation Thresholds (M1 / also SB4 Q4)
1. What elapsed wait time (in minutes) triggers an order transitioning to `at_risk` status?
2. Is the threshold a single global value or per-order-type (e.g., drinks vs. food)?
3. What is the escalation action? UI indicator only, or does it also send a notification?

## Acceptance Criteria

- [ ] Decision document produced — either appended to `docs/coffee/store-service/mvp/architecture.md` under a "Resolved Decisions" section, or a new `docs/coffee/store-service/mvp/queue-ordering-adr.md`
- [ ] Rush-authority matrix documented with named roles and conditions
- [ ] Queue ordering algorithm documented with explicit tie-breaking rules
- [ ] At-risk escalation threshold values documented (concrete numbers or configurable range)
- [ ] Decision reviewed and signed off by at least one named coffee stakeholder (Shift Lead persona representative)
- [ ] S2.1 acceptance criteria can be fully implemented from this decision alone (no further stakeholder input needed)
- [ ] S2.2 authority check can be fully implemented from the authority matrix alone

## Definition of Done

- Decision document committed and pushed to `mvp` branch
- S2.1, S2.2, S2.3 unblocked
- This story closed in `sprint-status.yaml` with status `done`

## Notes

The Shift Lead persona in `product-brief.md` is the primary decision-maker here.  
Consider a brief workshop or async document review with a Shift Lead representative rather than an engineering-only decision.
