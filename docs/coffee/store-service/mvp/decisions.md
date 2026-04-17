---
doc_type: decisions
feature: coffee-store-service-mvp
status: accepted
updated_at: '2026-04-17T00:00:00Z'
---

# Design Decisions — StoreOperationsService MVP

> **SB4 output** — closes all 6 open questions from `prd.md`. Each decision has a named owner. Stories S1.5, S1.6, S3.4, and S2.3 are unblocked.

---

## Q1 — Rush Qualification Rule

**Question:** Is rush designation system-driven (e.g., loyalty tier, order value) or human-initiated (Shift Lead action)?

**Decision:** Rush designation is **human-initiated only**. A Shift Lead must explicitly apply rush via the Operator Command API. No automated or system-driven rush elevation is in scope for MVP.

**Owner:** Product Owner (coffee domain)  
**Impact:** S2.2 implements a single `ApplyRushDesignation` command endpoint with a `ShiftLead` role authority check. No background service or rules engine needed for rush in MVP.

---

## Q2 — Completion Event and CustomerService Callback

**Question:** Is there a `StoreOrderCompleted` domain event published when `Ready → Completed` occurs? Does CustomerService receive a callback?

**Decision:** YES — a `StoreOrderCompleted` domain event **is published** via the outbox when `Ready → Completed` is accepted. CustomerService subscribes to this event to close its own order-confirmation workflow.

**Owner:** Product Owner (coffee domain)  
**Impact:**
- S1.5 must write the `StoreOrderCompleted` event to the outbox in the same transaction as the state transition.
- S3.4 must define the `StoreOrderCompleted` event schema (fields: `store_order_id`, `customer_order_id`, `store_id`, `completed_at`, `completed_by`, `completion_mode`, `aggregate_version`, `correlation_id`).
- E4 integration scope: CustomerService subscribes to `StoreOrderCompleted`. No synchronous callback. The integration is event-driven, not request-reply.

---

## Q3 — Post-Preparation Cancellation Classification

**Question:** If an order is cancelled after transitioning to `in-progress`, is it: (a) standard cancellation, (b) a separate `wasted`/`voided` terminal state, or (c) same state with a distinct reason code?

**Decision:** **(c) — same `cancelled` terminal state, with a distinct reason code.** No new terminal states are added.

**Owner:** Product Owner (coffee domain)  
**Impact:**
- S1.6 uses a single cancellation path. `cancelled_stage` field distinguishes `pre-start`, `in-progress`, and `post-ready` cancellations.
- Reason codes for post-preparation cancellation must include at least `operational-error` and `item-unavailable`.
- **Architecture.md Decision 1 (AD-1) is unchanged.** Six lifecycle states remain canonical.
- No ADR required.

---

## Q4 — SLA Threshold Values for At-Risk Escalation

**Question:** What are the concrete elapsed-time thresholds (in minutes) for triggering at-risk escalation? Single threshold or per-order-type?

**Decision:** Per-order-type thresholds. See `queue-ordering-adr.md` Decision 3 for the full threshold table (Drink: 5 min, Food: 8 min, Add-on: 3 min). Elapsed time measured from `queued_at`, not `received_at`.

**Owner:** Shift Lead Representative (coffee domain)  
**Impact:** S2.3 configures per-type thresholds from named configuration values. Threshold values are not hardcoded.

---

## Q5 — Cancellation Reason Codes

**Question:** What reason codes are valid for a cancellation? Fixed enum or open-ended string?

**Decision:** **Fixed enum.** The following reason codes are valid at the domain boundary. Unknown or unlisted values are rejected.

| Code | Description |
|---|---|
| `customer-request` | Customer requested cancellation before or during preparation |
| `item-unavailable` | Required item is out of stock or cannot be prepared |
| `order-timeout` | Order exceeded maximum wait threshold without progressing |
| `operational-error` | Internal error or equipment failure prevents fulfillment |
| `system-cancellation` | Automated cancellation by an upstream or orchestrating system |

**Owner:** Product Owner (coffee domain)  
**Impact:** S1.6 validates the incoming reason code against this enum at the domain boundary. Unrecognized codes return a domain validation error. The enum is the canonical list for downstream consumers (CustomerService, CorporateReportingService).

---

## Q6 — Multi-Store Routing Scope

**Question:** Is store disambiguation in scope for MVP? Or does MVP assume a single store context?

**Decision:** **Multi-store routing is in scope for MVP.** Every store-order aggregate carries a `store_id` field. Queue queries are always store-scoped. The MVP does not support single-store "convenience mode."

**Owner:** Product Owner (coffee domain)  
**Impact:**
- E1 aggregate must include `store_id` as a required field.
- E2 queue queries must include `WHERE store_id = @storeId` predicate.
- E4 integration events must include `store_id` in the payload.
- The `StoreId` dimension does not require a service-level router for MVP — the caller (CustomerService or Operator API) provides the target `store_id` on all commands.

---

## Summary Table

| Question | Decision | Owner | Stories Unblocked |
|---|---|---|---|
| Q1 — Rush qualification | Human-initiated only (Shift Lead) | Product Owner | S2.2 |
| Q2 — Completion event | `StoreOrderCompleted` published; CustomerService subscribes | Product Owner | S1.5, S3.4 |
| Q3 — Post-prep cancellation | Same `cancelled` state + `cancelled_stage` field | Product Owner | S1.6 |
| Q4 — SLA thresholds | Per-type (see queue-ordering-adr.md) | Shift Lead Rep | S2.3 |
| Q5 — Reason codes | Fixed enum (5 values) | Product Owner | S1.6 |
| Q6 — Multi-store scope | In scope; `store_id` required on all aggregates | Product Owner | E1, E2, E4 |
