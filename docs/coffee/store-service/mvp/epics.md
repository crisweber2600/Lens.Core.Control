---
feature: coffee-store-service-mvp
doc_type: epics
status: draft
goal: "Deliver StoreOperationsService — a new service that owns the store-side order lifecycle from receipt through handoff and cancellation"
key_decisions:
  - "Single store-order aggregate with relational current-state + append-only log + transactional outbox"
  - "Lifecycle state separate from operational modifiers (rush, at-risk)"
  - "completed = handoff_confirmed (operational, not CustomerService-driven)"
  - "completed → cancelled is an invalid transition"
  - "Versioned snapshot events for external consumers"
open_questions:
  - "Rush qualification criteria and authority (blocks E2 queue engine work — see M1, M2)"
  - "Queue ordering algorithm within a priority band (blocks E2 — see M3)"
  - "SLA threshold for at-risk escalation (blocks S2.3 — see M1)"
  - "Aspire AppHost registration design (blocks E5 — see C2)"
depends_on:
  - "CustomerService: order creation events feed StoreOperationsService intake"
  - "CorporateReportingService: receives store-level lifecycle event fan-out"
blocks:
  - "C1: business-plan.md required (coffee constitution hard gate — blocks dev-ready)"
  - "C2: aspire-plan.md required (Aspire Mandate hard gate — blocks dev-ready and E5)"
updated_at: "2025-01-15T00:00:00Z"
---

# Epics — StoreOperationsService MVP

## Milestone Context

| Field | Value |
|-------|-------|
| Workstream | coffee |
| Project | store-service |
| Milestone | mvp |
| Track | full |
| Planning Verdict | CONDITIONAL PASS (see finalizeplan-review.md) |

## Pre-Dev Blockers

> These two items are **constitution hard gates**. The `dev-ready` phase cannot begin until both are resolved. They are tracked as SPIKE stories in E6.

| ID | Blocker | Constitution Gate | Blocks |
|----|---------|-------------------|--------|
| C1 | `business-plan.md` missing | coffee hard gate: `required_planning_artifacts.business-plan` | dev-ready promotion |
| C2 | `aspire-plan.md` missing | coffee Aspire Mandate hard gate | dev-ready promotion + E5 stories |

---

## E1 — Order State Machine

**Goal:** Implement the `StoreOrder` aggregate with the full six-state lifecycle, persistence layer, and transition guards.

**Scope in:**
- Lifecycle states: `received → queued → in-progress → ready → completed → cancelled`
- Transition guard: `completed → cancelled` throws (invalid by AD-4)
- `completed` = `handoff_confirmed` (AD-3)
- Current-state snapshot persisted to relational store
- Lifecycle transitions appended to append-only log
- Aggregate entry into transactional outbox on every state change

**Scope out:**
- Queue priority ordering (E2)
- Outbox publisher/delivery (E3)
- CustomerService event intake (E4)

**Key Architecture Decisions Implemented:** AD-1, AD-2, AD-3, AD-4

**Dependencies:** None — foundational epic.

**Blockers:** None.

**Estimated Stories:** S1.1 – S1.6

---

## E2 — Queue and Priority Engine

**Goal:** Implement the store-side queue with priority banding, rush designation, and at-risk escalation.

**Scope in:**
- Queue ingestion when an order transitions to `queued`
- Priority band assignment (standard, rush, at-risk)
- At-risk escalation for waiting orders that exceed the SLA threshold
- Queue-state read model for operational display (barista, shift-lead, expediter surfaces)

**Scope out:**
- Order creation or intake (E4)
- Queue persistence beyond the current-state snapshot (E1)

**Key Architecture Decisions Implemented:** AD-2 (operational modifiers separate from lifecycle state)

**Dependencies:** E1 (lifecycle state machine)

**Blockers:**
- 🔴 **M3**: Queue ordering algorithm not specified — `S2.1` cannot be implemented without the ordering contract (FIFO-within-band? Time-promise hybrid?)
- 🔴 **M2**: Rush-authority matrix not approved — `S2.2` (rush designation) blocked until escalation thresholds decided
- 🔴 **M1**: SLA threshold for at-risk escalation unresolved — `S2.3` blocked

**Estimated Stories:** S2.1 – S2.4

---

## E3 — Transactional Outbox and Event Publication

**Goal:** Implement the transactional outbox pattern for reliable, exactly-once-effective event delivery.

**Scope in:**
- Outbox table schema and EF Core persistence contract
- Outbox Publisher: polling mechanism, mark-delivered, retry policy
- Event Bus Adapter (abstraction over actual message broker)
- Versioned snapshot event contracts (AD-5)
- Dead-letter routing and max-delivery-attempts policy

**Scope out:**
- Which events are published (driven by E1 lifecycle transitions)
- Downstream consumer integration (E4)

**Key Architecture Decisions Implemented:** AD-1 (outbox as part of aggregate persistence), AD-5 (versioned snapshot events)

**Dependencies:** E1 (aggregate commits produce outbox entries)

**Blockers:** None — can start in parallel with E1 after project scaffolding (E5).

**Estimated Stories:** S3.1 – S3.4

---

## E4 — External Service Integration

**Goal:** Implement the CustomerService inbound integration and the CorporateReportingService outbound fan-out.

**Scope in:**
- Inbound: consume CustomerService order-created events → `received` state in aggregate
- Outbound ready signal: notify CustomerService when order reaches `ready`
- Outbound reporting: fan-out lifecycle snapshot events to CorporateReportingService via outbox
- Integration contracts versioned per AD-5

**Scope out:**
- Internal lifecycle management (E1)
- Outbox infrastructure (E3)

**Dependencies:** E1, E3

**Blockers:** None.

**Estimated Stories:** S4.1 – S4.3

---

## E5 — Service Foundation

**Goal:** Scaffold StoreOperationsService in the solution, register it in the Aspire AppHost, and configure data persistence.

**Scope in:**
- .NET project scaffold (StoreOperationsService project, solution reference)
- Aspire AppHost service registration and resource wiring
- EF Core DbContext, initial schema migration, connection string configuration
- Docker/dev-container baseline for local Aspire `aspire start` validation

**Scope out:**
- Application logic (E1–E4)

**Dependencies:** None technically — but blocks all other epics running locally.

**Blockers:**
- 🔴 **C2**: `aspire-plan.md` not authored — AppHost registration design decisions required by Aspire Mandate before `S5.2` proceeds

**Estimated Stories:** S5.1 – S5.3

---

## E6 — Constitution Compliance and Design Resolution (SPIKEs)

**Goal:** Resolve all pre-dev-promotion blockers: author the two required governance artifacts and close the open design questions that block story implementation.

**Scope:** SPIKE stories only — no production code. All outputs are governance artifacts or written decisions appended to existing planning docs.

**Blockers:** These stories ARE the unblocking actions for other epics and for the `dev-ready` gate. Must be completed in Sprint 0 before Sprint 1 story work starts.

**Estimated Stories:** SB1 – SB4

---

## Epic Summary

| Epic | Title | Depends On | Blocker Count | Stories |
|------|-------|-----------|---------------|---------|
| E1 | Order State Machine | — | 0 | S1.1–S1.6 |
| E2 | Queue and Priority Engine | E1 | 3 (M1, M2, M3) | S2.1–S2.4 |
| E3 | Transactional Outbox + Events | E1 | 0 | S3.1–S3.4 |
| E4 | External Service Integration | E1, E3 | 0 | S4.1–S4.3 |
| E5 | Service Foundation | — (C2 blocks S5.2) | 1 (C2) | S5.1–S5.3 |
| E6 | Constitution Compliance SPIKEs | — | 0 (is the fix) | SB1–SB4 |
