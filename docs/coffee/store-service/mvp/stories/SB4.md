---
storyId: SB4
title: "Close 6 open design questions from PRD (M1)"
epic: E6
feature: coffee-store-service-mvp
type: spike
priority: HIGH
points: 3
sprint: 0
status: not-started
owner: TBD
blocked_reason: "None — this story unblocks S1.5, S1.6, S3.4"
blocks:
  - "S1.5 — Q2: StoreOrderCompleted event"
  - "S1.6 — Q3: post-prep cancellation, Q5: reason codes"
  - "S3.4 — Q2: StoreOrderCompleted event schema"
---

# SB4 — Close 6 Open Design Questions from PRD

## Context

`prd.md` contains an `open_questions` section with 6 unresolved questions. These questions appear as conditional blockers in S1.5, S1.6, S3.4, and S2.3. Stories can be partially implemented but cannot be closed until each relevant question is answered.

This SPIKE's job is to gather the decisions, document them with named decision owners, and update `prd.md` so the open_questions list is closed.

## User Story

**As a** developer implementing the lifecycle state machine and event schema,  
**I want** all 6 open product questions from the PRD answered and documented,  
**so that** ambiguous acceptance criteria in S1.5, S1.6, S3.4, and other stories are resolved.

## Questions to Resolve

### Q1 — Rush Qualification
> Is rush designation system-driven (e.g., loyalty tier, order value) or human-initiated (Shift Lead action)?

**Why it matters:** Determines whether the rush trigger is automatic (background service) or manual (API endpoint with authority check).  
**Blocking:** S2.2 (partially — see also SB3 authority matrix).

---

### Q2 — Completion Event
> Is there a `StoreOrderCompleted` domain event published when `Ready → Completed` occurs?  
> Does CustomerService receive a callback, or is `Completed` a silent internal state flip?

**Why it matters:** If there is no completion event, S1.5 and S3.4 do not need to define the event type. If CustomerService receives a callback, E4 integration scope expands.  
**Blocking:** S1.5 (event write in same transaction), S3.4 (event schema placeholder).

---

### Q3 — Post-Preparation Cancellation Classification
> If an order is cancelled after transitioning to `in-progress`, is it:
> (a) a standard cancellation (same `cancelled` state as pre-prep),
> (b) a separate `wasted` / `voided` terminal state, or
> (c) same state with a distinct reason code?

**Why it matters:** If (b), the state machine needs an additional terminal state, which is a breaking change to AD-1. If (a) or (c), S1.6 can use a single cancellation path with metadata.  
**Blocking:** S1.6 (state machine design), potentially E1 and E3 event schema.

---

### Q4 — SLA Threshold Values
> What are the concrete elapsed-time thresholds (in minutes) for triggering at-risk escalation?  
> Is it a single threshold, or per-order-type (drinks, food, add-ons)?

**Why it matters:** S2.3 at-risk escalation needs a concrete threshold value to configure.  
**Blocking:** S2.3 (also in SB3 scope).

---

### Q5 — Cancellation Reason Codes
> What reason codes are valid for a cancellation?  
> Is this a fixed enum (e.g., `customer-request`, `item-unavailable`, `order-timeout`, `operational`) or an open-ended string?

**Why it matters:** S1.6 validation logic depends on whether reason codes are validated at the domain boundary or accepted as free text.  
**Blocking:** S1.6 acceptance criteria (reason code validation).

---

### Q6 — Multi-Store Routing
> Is store disambiguation (routing an order to a specific store instance) in scope for MVP?  
> Or does the MVP assume a single store context?

**Why it matters:** If multi-store is in scope, the aggregate needs a `StoreId` dimension and queue queries must be store-scoped. This is an architectural expansion that could affect E1, E2, and E4.  
**Blocking:** Scope definition for E1 aggregate and E2 queue.

---

## Acceptance Criteria

- [ ] All 6 questions answered in writing with a named decision owner
- [ ] `prd.md` `open_questions` section updated — each question status changed from `open` to `closed`, with a one-line resolution summary and decision owner
- [ ] If Q3 resolution is (b) — additional terminal state — an ADR is authored and `architecture.md` updated before S1.6 begins
- [ ] Decision document committed and pushed to `mvp` branch (can be appended to `prd.md` or authored as `decisions.md`)

## Definition of Done

- `prd.md` or `decisions.md` updated with all 6 resolutions
- S1.5, S1.6, S3.4 conditionals unblocked (or explicitly accepted as conditionals with placeholder paths)
- This story closed in `sprint-status.yaml` with status `done`

## Notes

Q4 overlaps with SB3 (SLA thresholds). These two SPIKEs can be run concurrently and the output cross-referenced.  
If Q3 resolution requires a new terminal state, escalate to a full architecture review before S1.6 starts — this could add 3–5 story points to E1.
