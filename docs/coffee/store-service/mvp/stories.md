---
feature: coffee-store-service-mvp
doc_type: stories
status: draft
goal: "Complete story list for StoreOperationsService MVP — covers all epics including constitution compliance SPIKEs"
key_decisions:
  - "C1 and C2 are BLOCKER stories in Sprint 0 — dev promotion is gated on both"
  - "E2 stories are deferred until Sprint 0 SPIKEs resolve open questions and authority matrix"
  - "E5 service scaffolding begins Sprint 1 but S5.2 (Aspire registration) is blocked by C2"
open_questions: []
depends_on: []
blocks: []
updated_at: "2025-01-15T00:00:00Z"
---

# Stories — StoreOperationsService MVP

## Notation

- 🔴 **BLOCKER** — cannot start until a SPIKE (E6) closes the dependency
- ⚠️ **CONDITIONAL** — can start, but acceptance criteria include a dependency condition that must be resolved before PR merge
- Story points use Fibonacci: 1, 2, 3, 5, 8, 13

---

## E6 — Constitution Compliance SPIKEs (Sprint 0)

> Must all close before dev-ready promotion. Treat as sprint-blocking stories.

---

### SB1 — Author `business-plan.md` (C1 Hard Gate)

**Type:** SPIKE  
**Points:** 5  
**Priority:** BLOCKER  
**Sprint:** 0

**As a** coffee domain stakeholder,  
**I want** a `business-plan.md` authored for the store-service/mvp milestone,  
**so that** the coffee domain constitution hard gate (`required_planning_artifacts.business-plan`) is satisfied and the milestone can advance to `dev-ready`.

**Acceptance Criteria:**
- `docs/coffee/store-service/mvp/business-plan.md` exists and passes frontmatter schema validation
- `doc_type: business-plan` in frontmatter
- Covers: problem statement, value proposition, target personas, revenue/cost assumptions, risk register
- Published into governance mirror at `milestones/coffee/store-service/mvp/docs/`
- `milestone.yaml` reflects the artifact as present

**Blockers:** None — this is itself the blocker.

---

### SB2 — Author `aspire-plan.md` (C2 Aspire Mandate Hard Gate)

**Type:** SPIKE  
**Points:** 5  
**Priority:** BLOCKER  
**Sprint:** 0

**As a** coffee Aspire Mandate owner,  
**I want** an `aspire-plan.md` authored for the store-service/mvp milestone,  
**so that** the Aspire Mandate gate is satisfied (all coffee services MUST use shared .NET Aspire AppHost) and the milestone can advance to `dev-ready`.

**Acceptance Criteria:**
- `docs/coffee/store-service/mvp/aspire-plan.md` exists and passes frontmatter schema validation
- `doc_type: aspire-plan` in frontmatter
- Covers: AppHost project reference, service registration pattern, shared resource naming, local dev validation (`aspire start` smoke test)
- Answers which existing AppHost project StoreOperationsService registers into
- Published into governance mirror
- Unblocks S5.2

**Blockers:** None — this is itself the blocker.

---

### SB3 — Resolve Rush-Authority Matrix and Queue Ordering Rules (M2 + M3)

**Type:** SPIKE  
**Points:** 3  
**Priority:** HIGH  
**Sprint:** 0

**As a** developer implementing E2 Queue and Priority Engine,  
**I want** a written decision on the rush-authority matrix and queue ordering algorithm,  
**so that** S2.1, S2.2, and S2.3 can be implemented with a clear specification.

**Acceptance Criteria:**
- Decision appended to `architecture.md` under a "Resolved Decisions" section or a separate `queue-ordering-adr.md`
- Rush authority: who may designate an order as rush (system rule, Shift Lead, external signal)?
- Escalation thresholds: at what elapsed wait time does an order become at-risk?
- Queue ordering algorithm within a priority band documented (FIFO by received timestamp? Time-promise deadline-driven?)
- Reviewed and approved by at least one domain stakeholder
- Unblocks S2.1, S2.2, S2.3

**Blockers:** None.

---

### SB4 — Close Open Design Questions (M1)

**Type:** SPIKE  
**Points:** 3  
**Priority:** HIGH  
**Sprint:** 0

**As a** developer implementing the lifecycle state machine and cancellation path,  
**I want** answers to the six open product questions from the PRD,  
**so that** ambiguous acceptance criteria in S1.x stories are resolved before implementation.

**Acceptance Criteria:**
Answers documented in `prd.md` (close the `open_questions` section) or a separate `decisions.md`:
1. **Rush qualification** — system-driven or human-initiated? Criteria documented.
2. **Completion event** — is there a `StoreOrderCompleted` domain event, or is `ready` → `completed` a silent state flip? Does CustomerService receive a callback?
3. **Cancellation classification post-prep** — if an order is cancelled after `in-progress`, is it a standard cancellation or a separate `voided`/`wasted` state? How does it appear in reporting?
4. **SLA threshold values** — concrete elapsed-time thresholds for at-risk escalation
5. **Reason codes** — what reason codes are valid for cancellations? Open vs. fixed enumeration?
6. **Multi-store routing** — is store disambiguation in scope for MVP?

- All six questions have a written resolution with a named decision owner
- `prd.md` `open_questions` list updated to reflect closed status

**Blockers:** None.

---

## E5 — Service Foundation (Sprint 1)

---

### S5.1 — Scaffold StoreOperationsService .NET Project

**Points:** 3  
**Priority:** HIGH  
**Sprint:** 1

**As a** developer,  
**I want** a new `StoreOperationsService` .NET project scaffolded in the coffee solution,  
**so that** other stories have a valid build target and I can run the service locally.

**Acceptance Criteria:**
- `TargetProjects/coffee/store-service/` contains a valid .NET 8+ project (or 9+)
- Project builds cleanly (`dotnet build`) with no warnings beyond nullable annotation noise
- Project referenced in solution file
- Basic health-check endpoint returns 200 for local smoke test
- No business logic yet — scaffold only

**Blockers:** None.

---

### S5.2 — Register StoreOperationsService in Aspire AppHost

**Points:** 3  
**Priority:** HIGH  
**Sprint:** 1  
**Status:** 🔴 BLOCKED by C2 (aspire-plan.md)

**As a** developer,  
**I want** StoreOperationsService registered in the shared Aspire AppHost,  
**so that** `aspire start` launches the service alongside CustomerService and CorporateReportingService in the local dev environment.

**Acceptance Criteria:**
- AppHost project reference to StoreOperationsService added
- Service appears in `aspire start` dashboard
- Connection strings and resource bindings wired per `aspire-plan.md` decisions
- `aspire start` smoke test passes (service health-check reachable from dashboard)

**Blockers:** 🔴 C2 — `aspire-plan.md` required to determine which AppHost and resource binding pattern to use.

---

### S5.3 — EF Core DbContext and Initial Migration

**Points:** 3  
**Priority:** HIGH  
**Sprint:** 1

**As a** developer,  
**I want** an EF Core DbContext with the initial schema migration created,  
**so that** E1 aggregate persistence stories have a database target.

**Acceptance Criteria:**
- `StoreOperationsDbContext` class with `StoreOrders`, `StoreOrderLog`, and `OutboxMessages` tables
- Initial migration generated and applies cleanly via `dotnet ef database update`
- Connection string configurable via environment variable / Aspire resource binding
- Tables correctly reflect the aggregate schema from AD-1

**Blockers:** None (S5.2 not required to generate migration, only to run with Aspire).

---

## E1 — Order State Machine (Sprint 1–2)

---

### S1.1 — Define StoreOrder Aggregate and Value Objects

**Points:** 5  
**Priority:** HIGH  
**Sprint:** 1

**As a** developer,  
**I want** a `StoreOrder` aggregate root and its lifecycle-state value object defined,  
**so that** all lifecycle stories build on a consistent domain model.

**Acceptance Criteria:**
- `StoreOrder` aggregate root class with `OrderId`, `LifecycleState`, `OperationalModifiers`, `CreatedAt`, `UpdatedAt`
- `OrderLifecycleState` value object or enum: `Received`, `Queued`, `InProgress`, `Ready`, `Completed`, `Cancelled`
- Transition table unit-tested: all valid transitions return success; invalid transitions throw `InvalidTransitionException`
- `Completed → Cancelled` specifically tested and throws (AD-4)
- No persistence yet — domain model only

**Blockers:** None.

---

### S1.2 — Persist Lifecycle State Snapshot

**Points:** 3  
**Priority:** HIGH  
**Sprint:** 1

**As a** developer,  
**I want** the current lifecycle state persisted to the relational `StoreOrders` table on every state change,  
**so that** queries for current order state are fast and do not require log replay.

**Acceptance Criteria:**
- `StoreOrderRepository` writes aggregate state to `StoreOrders` table inside the transaction that also appends the log entry (S1.3)
- Optimistic concurrency via row version / `xmin` token
- `dotnet test` includes a round-trip test: transition aggregate → save → reload → verify state

**Blockers:** S5.3 (DbContext must exist).

---

### S1.3 — Append Lifecycle Transitions to Append-Only Log

**Points:** 3  
**Priority:** HIGH  
**Sprint:** 1

**As a** developer,  
**I want** every lifecycle transition appended as an immutable row to `StoreOrderLog`,  
**so that** the full transition history is preserved for audit and reporting.

**Acceptance Criteria:**
- `StoreOrderLog` row created in the same DB transaction as the current-state update (S1.2)
- Row includes: `OrderId`, `FromState`, `ToState`, `OccurredAt`, `InitiatedBy`, `Metadata` (JSON)
- Log rows are never updated or deleted
- Unit test verifies log entries accumulate correctly across a multi-step lifecycle

**Blockers:** S5.3, S1.2.

---

### S1.4 — Enforce Invalid Transition Guard

**Points:** 2  
**Priority:** MEDIUM  
**Sprint:** 1

**As a** developer,  
**I want** all invalid lifecycle transitions to throw a typed domain exception at the aggregate boundary,  
**so that** callers cannot accidentally put an order into an impossible state.

**Acceptance Criteria:**
- `InvalidTransitionException` thrown for any transition not in the valid-transition table
- `Completed → Cancelled` specifically tested
- Exception message includes `fromState`, `toState`, and `orderId`
- No state is written to the database if an invalid transition is attempted

**Blockers:** S1.1.

---

### S1.5 — Implement Ready → Completed (Handoff Confirmation) Path

**Points:** 3  
**Priority:** HIGH  
**Sprint:** 2

**As a** barista or expediter,  
**I want** to confirm handoff on a ready order and transition it to `completed`,  
**so that** the order lifecycle closes correctly when the customer receives their order.

**Acceptance Criteria:**
- `StoreOrder.ConfirmHandoff()` transitions `Ready → Completed`
- `CompletedAt` timestamp recorded
- Outbox entry for `StoreOrderCompleted` event written in same transaction (pending SB4 decision on whether the event exists)
- ⚠️ CONDITIONAL: Event schema depends on SB4 Q2 resolution

**Blockers:** S1.1, S1.3. SB4 must be resolved before PR merge for event schema.

---

### S1.6 — Implement Cancellation Path

**Points:** 5  
**Priority:** HIGH  
**Sprint:** 2

**As a** store operator,  
**I want** to cancel an order from `received`, `queued`, or `in-progress` states,  
**so that** orders that cannot be fulfilled are removed from the queue with the correct audit trail.

**Acceptance Criteria:**
- `StoreOrder.Cancel(reasonCode, cancelledBy)` allowed from `received`, `queued`, `in-progress`
- `CancelledAt`, `CancellationReason`, and `CancelledBy` recorded
- `Completed → Cancelled` still throws
- Log entry written
- Outbox entry for `StoreOrderCancelled` snapshot event written in same transaction
- ⚠️ CONDITIONAL: Reason code enumeration and post-prep cancellation classification depend on SB4 resolution

**Blockers:** S1.1, S1.3. SB4 must be resolved for final acceptance criteria.

---

## E3 — Transactional Outbox + Events (Sprint 1–2, parallel with E1)

---

### S3.1 — Create Outbox Table and Persistence Contract

**Points:** 2  
**Priority:** HIGH  
**Sprint:** 1

**As a** developer,  
**I want** the `OutboxMessages` table and its EF Core entity defined,  
**so that** aggregate state changes can atomically enqueue events for reliable delivery.

**Acceptance Criteria:**
- `OutboxMessages` table: `MessageId`, `EventType`, `Payload` (JSON), `CreatedAt`, `ScheduledAt`, `DeliveredAt`, `DeliveryAttempts`, `Status`
- EF Core entity and migration created
- `StoreOrderRepository` writes outbox entries in the same DB transaction as state changes (AD-1)
- Unit test: aggregate transition + outbox write fail together on rollback

**Blockers:** S5.3.

---

### S3.2 — Implement Outbox Publisher

**Points:** 5  
**Priority:** HIGH  
**Sprint:** 2

**As a** developer,  
**I want** an Outbox Publisher background service that polls for undelivered messages and dispatches them to the event bus,  
**so that** aggregate state changes are reliably propagated even if the message broker is temporarily unavailable.

**Acceptance Criteria:**
- `OutboxPublisherService` polls `OutboxMessages` where `Status = Pending`
- Marks message as `Delivered` only after bus acknowledgement
- Increments `DeliveryAttempts` on failure; routes to dead-letter after configurable max attempts
- Idempotent: duplicate delivery of the same `MessageId` does not cause duplicate processing downstream (event consumers must handle at-least-once)
- Configurable poll interval via `appsettings.json`
- Integration test: inject failing bus adapter → verify retry and DLQ behaviour

**Blockers:** S3.1, S3.3.

---

### S3.3 — Implement Event Bus Adapter

**Points:** 3  
**Priority:** HIGH  
**Sprint:** 2

**As a** developer,  
**I want** an `IEventBusAdapter` abstraction and a concrete implementation,  
**so that** the Outbox Publisher is decoupled from the specific message broker (Azure Service Bus, RabbitMQ, etc.).

**Acceptance Criteria:**
- `IEventBusAdapter` interface with `PublishAsync(MessageEnvelope)` method
- At least one concrete adapter (in-process for local; cloud adapter TBD in `aspire-plan.md`)
- Adapter registered in DI container
- Unit tests use a fake/mock adapter

**Blockers:** None.

---

### S3.4 — Define Versioned Snapshot Event Schema

**Points:** 3  
**Priority:** HIGH  
**Sprint:** 1

**As a** external service consumer (CorporateReportingService),  
**I want** versioned snapshot event contracts for StoreOperationsService lifecycle events,  
**so that** I can consume event payloads without tight coupling to internal aggregate fields.

**Acceptance Criteria:**
- Event types defined: `StoreOrderReceived`, `StoreOrderQueued`, `StoreOrderInProgress`, `StoreOrderReady`, `StoreOrderCancelled` (and `StoreOrderCompleted` per SB4 Q2)
- Each event has a `schema_version` field (starting at `v1`)
- Snapshot payload includes: `orderId`, `lifecycleState`, `occurredAt`, `schemaVersion`, plus state-specific fields
- Schema documented in `docs/coffee/store-service/mvp/event-schema.md` (can be authored as part of this story)
- Unit test: serialize/deserialize round-trip for each event type

**Blockers:** SB4 Q2 for `StoreOrderCompleted` event — include a placeholder if unresolved.

---

## E2 — Queue and Priority Engine (Sprint 3 — deferred pending SB3 + SB4)

---

### S2.1 — Queue Ingestion and Ordering Algorithm

**Points:** 5  
**Priority:** MEDIUM  
**Sprint:** 3  
**Status:** 🔴 BLOCKED by SB3 (queue ordering rules M3)

**As a** barista,  
**I want** orders automatically enqueued and ordered when they transition to `queued`,  
**so that** I see a prioritised list of work to fulfill.

**Acceptance Criteria:**
- On `Received → Queued` transition, order appears in the queue read model
- Orders sorted per the algorithm resolved in SB3 (FIFO-within-band or deadline-driven)
- Priority band assigned at ingestion
- Queue-state read model updated atomically with lifecycle transition

**Blockers:** 🔴 SB3 (ordering algorithm), E1 (lifecycle).

---

### S2.2 — Rush Designation

**Points:** 3  
**Priority:** MEDIUM  
**Sprint:** 3  
**Status:** 🔴 BLOCKED by SB3 (rush-authority matrix M2)

**As a** shift lead,  
**I want** to designate an order as rush,  
**so that** it moves to the front of the in-band queue for immediate fulfillment.

**Acceptance Criteria:**
- `StoreOrder.DesignateRush(designatedBy)` only allowed by principals matching the authority matrix from SB3
- Order priority band updated; queue position recalculated
- `OperationalModifiers.IsRush = true`
- Audit log entry written

**Blockers:** 🔴 SB3 (authority matrix), S2.1.

---

### S2.3 — At-Risk Escalation

**Points:** 3  
**Priority:** MEDIUM  
**Sprint:** 3  
**Status:** 🔴 BLOCKED by SB3 (SLA threshold M1)

**As an** expediter,  
**I want** orders waiting beyond the SLA threshold to be automatically escalated to at-risk,  
**so that** I can intervene before a customer-facing miss.

**Acceptance Criteria:**
- Background timer evaluates orders in `queued` or `in-progress` against the SLA threshold from SB3
- `OperationalModifiers.IsAtRisk = true` set when threshold exceeded
- Queue read model shows at-risk indicator
- Threshold configurable via `appsettings.json`

**Blockers:** 🔴 SB3 (SLA threshold values), S2.1.

---

### S2.4 — Queue-State Read Model API

**Points:** 3  
**Priority:** MEDIUM  
**Sprint:** 3

**As a** barista or shift lead,  
**I want** an API endpoint that returns the current queue state,  
**so that** operational UIs can display the prioritised order list.

**Acceptance Criteria:**
- `GET /store-operations/queue` returns ordered list of active orders with priority band and at-risk indicator
- Response paginates at 100 orders
- Secured (API key or service-to-service auth per `aspire-plan.md`)
- Integration test using test double for the database

**Blockers:** S2.1, S5.2 (for auth wiring).

---

## E4 — External Service Integration (Sprint 2–3)

---

### S4.1 — Inbound Order Intake from CustomerService

**Points:** 5  
**Priority:** HIGH  
**Sprint:** 2

**As a** StoreOperationsService,  
**I want** to consume `OrderCreated` events from CustomerService and create a `StoreOrder` in `received` state,  
**so that** the lifecycle begins at the moment CustomerService confirms an order.

**Acceptance Criteria:**
- Consumer subscribes to CustomerService `OrderCreated` events via the Event Bus Adapter
- `StoreOrder` created in `received` state; `orderId` mapped from CustomerService event
- Idempotent: duplicate `OrderCreated` events do not create duplicate orders
- Error handling: failed persistence → message nacked for redelivery

**Blockers:** S3.3 (Event Bus Adapter), S1.1.

---

### S4.2 — Outbound Ready Notification to CustomerService

**Points:** 3  
**Priority:** HIGH  
**Sprint:** 2

**As a** CustomerService,  
**I want** a notification when a store order reaches `ready`,  
**so that** I can notify the customer their order is available for pickup.

**Acceptance Criteria:**
- `StoreOrderReady` snapshot event published via outbox when `InProgress → Ready` transition occurs
- CustomerService integration contract documented in event schema (S3.4)
- CustomerService consumer can correlate via `orderId`

**Blockers:** S3.4 (event schema), S1.1.

---

### S4.3 — Outbound Event Fan-out to CorporateReportingService

**Points:** 3  
**Priority:** MEDIUM  
**Sprint:** 3

**As a** CorporateReportingService,  
**I want** to receive versioned lifecycle snapshot events for all StoreOperationsService state changes,  
**so that** I can compute store-level throughput and queue metrics.

**Acceptance Criteria:**
- All lifecycle events (`StoreOrderReceived`, `StoreOrderQueued`, `StoreOrderInProgress`, `StoreOrderReady`, `StoreOrderCompleted`, `StoreOrderCancelled`) published via outbox
- Events consumed by CorporateReportingService integration (stubbed for MVP if CorporateReportingService does not yet exist)
- Schema version `v1` for all events

**Blockers:** S3.2, S3.4.

---

## Story Point Summary

| Epic | Stories | Total Points | Sprint |
|------|---------|-------------|--------|
| E6 (SPIKEs) | SB1, SB2, SB3, SB4 | 16 | 0 |
| E5 Foundation | S5.1, S5.2, S5.3 | 9 | 1 |
| E1 State Machine | S1.1–S1.6 | 21 | 1–2 |
| E3 Outbox + Events | S3.1–S3.4 | 13 | 1–2 |
| E4 Integration | S4.1–S4.3 | 11 | 2–3 |
| E2 Queue Engine | S2.1–S2.4 | 14 | 3 |
| **Total** | **23 stories** | **84 pts** | **0–3** |
