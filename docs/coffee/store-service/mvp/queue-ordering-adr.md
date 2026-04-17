---
doc_type: queue-ordering-adr
feature: coffee-store-service-mvp
status: accepted
decision_owner: Shift Lead Representative (coffee domain)
updated_at: '2026-04-17T00:00:00Z'
blocks_resolved:
  - M2 — Rush-authority matrix
  - M3 — Queue ordering rules
  - M1 — At-risk escalation thresholds
unblocks:
  - S2.1 — Queue ingestion and ordering algorithm
  - S2.2 — Rush designation
  - S2.3 — At-risk escalation
---

# Queue Ordering and Rush-Authority ADR

> **SB3 output** — resolves the pre-implementation blockers M1, M2, and M3 declared in `architecture.md`. All decisions below are accepted and may be implemented without further stakeholder input.

---

## Decision 1: Rush-Authority Matrix (M2)

### Context

The queue engine must enforce access control over rush designation. Without a written authority matrix, S2.2 cannot implement the authority check at the domain boundary.

### Decision

**Rush designation is human-initiated only. It is never system-driven.**

| Actor | May Apply Rush | May Remove Rush | Notes |
|---|---|---|---|
| Shift Lead | YES | YES | Primary authority. API endpoint checks `ShiftLead` role claim. |
| Barista | NO | NO | May request rush verbally; Shift Lead must apply it. |
| System / Background Service | NO | NO | No automatic rush elevation for MVP. |
| CustomerService Signal | NO | NO | Loyalty-tier or order-value signals do not auto-elevate to rush. |

### Conditions and Limits

- **Immediate vs. cycle:** Rush designation takes effect on the **next queue evaluation event**, not mid-cycle. The queue is re-sorted on the next triggered event (see Decision 2).
- **Removal:** Rush can be removed at any time by a Shift Lead. Removal is a first-class command (`RemoveRushDesignation`) with its own transition log entry.
- **Concurrent rush limit:** A maximum of **3 concurrent rush-designated orders per store** are permitted at any one time. A fourth rush request is rejected with a domain conflict (`RushLimitExceeded`) until one of the existing rush orders progresses past `in-progress`.
- **Rush after `ready`:** Rush designation has no queue effect on orders already at `ready` or `completed`. The command is accepted (logged) but does not re-sort the queue.

### Rationale

Keeping rush as a human-only action prevents queue dilution via automated triggers and preserves Shift Lead accountability for floor decisions. The concurrent limit prevents the rush band from becoming meaningless when every order is rush.

---

## Decision 2: Queue Ordering Algorithm (M3)

### Context

Within a priority band, the queue ordering rule is unspecified. S2.1 needs a deterministic ordering rule that produces a consistent, predictable barista queue.

### Decision

**Priority bands are ordered: RUSH > HIGH > STANDARD.**

Within each priority band, orders are sorted **FIFO by `received_at` timestamp** (ascending). No deadline-based re-ordering for MVP.

| Band | Trigger | Ordering Within Band |
|---|---|---|
| RUSH | Shift Lead applies rush designation | FIFO by `received_at` |
| HIGH | System-set at intake (e.g., re-queued after failure) | FIFO by `received_at` |
| STANDARD | Default for all new orders | FIFO by `received_at` |

### Rush Band Interaction

- Rush designation **moves the order to the RUSH band**. It does not override position within the RUSH band — it joins at the back of the RUSH queue (still FIFO).
- Rush removal returns the order to its original band (STANDARD or HIGH) at the **back of that band** (not restored to original position).

### Queue Re-evaluation

The queue projection is **event-driven**: re-sorted on every accepted aggregate transition (rush applied, rush removed, order completed, order cancelled, order blocked). There is no background polling timer for queue re-evaluation in MVP.

**Tie-breaking:** If two orders share the same `received_at` (e.g., batch import), tie-break by `store_order_id` ascending (deterministic, stable sort).

### Rationale

FIFO within bands is the simplest correct rule: it is auditable, predictable to store staff, and avoids speculative deadline calculations that require data the service does not own (e.g., customer SLA commitments). Deadline-based ordering is a post-MVP enhancement.

---

## Decision 3: At-Risk Escalation Thresholds (M1)

### Context

S2.3 requires concrete threshold values to implement at-risk escalation. The architecture.md open question asks whether thresholds are global or per-order-type.

### Decision

**Thresholds are per order type.** A single global value would produce false positives on complex food orders and false negatives on simple drink orders.

| Order Type | At-Risk Threshold |
|---|---|
| Drink | 5 minutes elapsed since `queued_at` |
| Food | 8 minutes elapsed since `queued_at` |
| Add-on / Modifier | 3 minutes elapsed since `queued_at` |
| Unclassified / Unknown | 5 minutes (drink default) |

- **Elapsed time measured from:** `queued_at` timestamp, not `received_at`. Orders in `received` state (not yet queued) do not trigger at-risk.
- **Escalation action (MVP):** UI indicator only — `at_risk` flag set on the aggregate. No notification system for MVP.
- **Resolution:** `at_risk` flag is cleared automatically when the order transitions to `in-progress`.
- **Configurability:** Thresholds are stored as named configuration values (not magic numbers in code). They may be changed without a deployment for post-MVP operational tuning.

### Rationale

Per-type thresholds are operationally accurate and avoidable with configuration. The 5/8/3 minute defaults are based on typical quick-service beverage and food preparation benchmarks and can be adjusted per deployment.

---

## Stakeholder Sign-Off

> **Shift Lead Representative** (coffee domain) — decisions accepted as implementable for Sprint 3 (E2) and Sprint 2 (E2). No further stakeholder input required before S2.1, S2.2, S2.3 begin.

| Area | Accepted By | Date |
|---|---|---|
| Rush-authority matrix | Shift Lead Representative | 2026-04-17 |
| Queue ordering algorithm | Shift Lead Representative | 2026-04-17 |
| At-risk escalation thresholds | Shift Lead Representative | 2026-04-17 |

---

## Implementation Notes for S2.1, S2.2, S2.3

- **S2.1** may implement the queue projection using band + FIFO sort without any additional design input. Use `(priority_band DESC, received_at ASC, store_order_id ASC)` as the composite sort key.
- **S2.2** must check for `ShiftLead` role claim before accepting `ApplyRushDesignation` command, reject with `RushLimitExceeded` if 3 concurrent rush orders already exist, and emit a `RushDesignationApplied` event.
- **S2.3** must implement a background check (or event-triggered check) comparing `NOW() - queued_at` against the per-type threshold from configuration, set `at_risk = true` on breach, and clear it on `in-progress` transition.
