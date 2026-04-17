---
feature: coffee-store-service-mvp
doc_type: architecture
status: draft
goal: "Translate StoreOperationsService requirements into a technical design that standardizes queueing, fulfillment state, completion, cancellation, and downstream contracts before implementation begins."
key_decisions:
  - "Use a single store-order aggregate with a relational current-state record, append-only transition log, and transactional outbox."
  - "Model lifecycle state separately from operational modifiers so rush, blocked, and remake behavior do not fragment core state semantics."
  - "Standardize completed as handoff-confirmed, not merely prepared, and forbid completed-to-cancelled transitions."
  - "Emit versioned domain events with full current-state snapshots so downstream services do not infer lifecycle meaning from partial deltas."
open_questions:
  - "What exact SLA threshold should auto-elevate a waiting standard order into the at-risk queue band?"
  - "Which cancellation reason codes need finance or customer recovery semantics in CustomerService?"
depends_on:
  - "CustomerService"
  - "CorporateReportingService"
blocks:
  - "Approve the rush-authority matrix and at-risk escalation thresholds before implementation starts."
  - "Approve the canonical cancellation reason taxonomy before downstream contract publication."
updated_at: 2026-04-17T00:00:00Z
---

# StoreOperationsService Solutioning / Architecture

## Document Intent

This document turns the approved StoreOperationsService product brief and PRD into an implementation-ready technical design. It standardizes the service architecture, state model, queue behavior, contracts, and failure handling so different teams or AI agents do not make incompatible choices during delivery.

## Solutioning Summary

StoreOperationsService becomes the single writer for operational truth after CustomerService hands off an actionable order to a store. The service owns:

- the authoritative current store-order state
- the auditable transition history for that state
- the queue ordering rules that decide what the store should work next
- the externally visible contracts that downstream services depend on

The architecture is intentionally designed around one rule: queue state, lifecycle state, and published downstream facts must all come from the same accepted transition, or downstream systems will drift.

## Architecture Decisions

### Decision 1: Authoritative Write Model

Use a single store-order aggregate per actionable order, backed by:

- a relational current-state record for fast reads and operational queries
- an append-only transition log for auditability and replay
- a transactional outbox for guaranteed downstream publication

This avoids two common failure patterns:

- a queue projection that moves independently from the true lifecycle state
- downstream events that publish from a different code path than the one that updated the order

### Decision 2: Separate Lifecycle State From Operational Modifiers

The primary lifecycle remains exactly the six states approved in the PRD:

- `received`
- `queued`
- `in-progress`
- `ready`
- `completed`
- `cancelled`

Rush, blocked, reprioritized, and remake handling are represented as operational modifiers on the order, not as additional lifecycle states.

This keeps downstream meaning stable while still allowing the floor to handle urgency and recovery conditions.

### Decision 3: Completion Means Handoff Confirmed

`ready` means production is finished and the order is waiting for handoff.

`completed` is standardized as `handoff_confirmed` for MVP. It is not inferred from preparation finishing, and it is not delayed until post-store customer behavior that StoreOperationsService does not own.

An order is only `completed` when a store actor or approved handoff integration explicitly confirms the handoff event.

### Decision 4: Completed Orders Cannot Become Cancelled

`completed -> cancelled` is not a valid lifecycle transition.

If a cancellation request arrives after completion:

- StoreOperationsService keeps the lifecycle state as `completed`
- the cancel command is rejected with a domain conflict
- an audit event records the rejection
- CustomerService handles any follow-on customer recovery, refund, or compensation workflow outside the store-order lifecycle

This prevents stale or contradictory downstream truth such as an order appearing both completed and cancelled.

### Decision 5: Versioned Snapshot Events Are the Canonical Downstream Contract

Every accepted lifecycle or modifier transition emits a versioned event that includes the full current-state snapshot, not only the delta.

This is required because downstream consumers need:

- monotonic ordering per order
- separate `ready_at`, `completed_at`, and `cancelled_at` timestamps
- cancellation stage and reason context
- current priority and rush context
- correlation and tracing identifiers

Without the full snapshot, CustomerService and CorporateReportingService will reconstruct different meanings from partial data.

## Service Architecture

### Logical Components

1. Intake Contract Adapter
   Validates actionable order intent from CustomerService, deduplicates upstream retries, and rejects incomplete orders before they enter store execution.

2. Store Order Aggregate Service
   Enforces lifecycle rules, modifier rules, optimistic concurrency, and idempotent command handling.

3. Queue and Priority Engine
   Builds the active store queue from accepted aggregate transitions and applies the standardized ordering rules.

4. Operator Command API
   Accepts store-floor actions such as start, ready, complete, cancel, apply rush, block, and resume.

5. Query and Projection Layer
   Serves the store queue, active work views, order detail, and transition history without allowing direct mutation.

6. Outbox Publisher
   Publishes domain events to downstream consumers only after the authoritative state transaction commits.

7. Reconciliation and Replay Interface
   Allows downstream consumers to reconcile missed or out-of-order updates using the latest aggregate snapshot.

### System Context

```text
CustomerService
  -> actionable order handoff
  -> optional customer recovery workflow

StoreOperationsService
  -> intake validation and dedupe
  -> authoritative order aggregate and transition log
  -> queue and priority projection
  -> operator command/query APIs
  -> outbox-based event publication

CorporateReportingService
  <- immutable operational events and milestone timestamps
```

### Data Ownership Boundary

CustomerService owns customer intent, pricing, payment, and customer-facing recovery.

StoreOperationsService owns store execution truth after actionable handoff.

CorporateReportingService owns aggregation and analytics derived from StoreOperationsService facts, but not the facts themselves.

## Canonical Data Model

### Core Aggregate Fields

Each store-order aggregate must include at minimum:

- `store_order_id`
- `customer_order_id`
- `store_id`
- `aggregate_version`
- `lifecycle_state`
- `priority_band`
- `priority_source`
- `manual_override_rank`
- `blocked_flag`
- `blocked_reason_code`
- `remake_flag`
- `received_at`
- `queued_at`
- `started_at`
- `ready_at`
- `completed_at`
- `completed_by`
- `completion_mode`
- `cancelled_at`
- `cancelled_stage`
- `cancellation_reason_code`
- `cancellation_actor_type`
- `cancellation_actor_id`
- `last_command_id`
- `last_transition_id`
- `correlation_id`
- `trace_id`

### Supporting Tables or Collections

The implementation should standardize equivalent persistence structures for:

- `store_orders` for the current aggregate snapshot
- `store_order_transitions` for immutable lifecycle and modifier transitions
- `store_queue_projection` for the current ordered queue view per store
- `processed_source_messages` for intake dedupe
- `outbox_events` for downstream publication

The queue projection is a derived view. It is never a separate source of lifecycle truth.

## State Model and Transitions

### Primary Lifecycle Model

| State | Entry Condition | Exit Condition | Required Timestamps | Notes |
| --- | --- | --- | --- | --- |
| `received` | Valid actionable order intent accepted | Auto-transition to `queued` in same transaction once queue entry is created | `received_at` | Exists to preserve the operational handoff moment |
| `queued` | Order is active, not yet started, and visible to the store queue | `in-progress` or `cancelled` | `queued_at` | Participates in dispatch ordering |
| `in-progress` | Fulfillment has actually started | `ready` or `cancelled` | `started_at` | Indicates committed work has begun |
| `ready` | Production finished, waiting for handoff | `completed` or `cancelled` | `ready_at` | Not terminal |
| `completed` | Handoff confirmed | none | `completed_at` | Terminal success |
| `cancelled` | Order withdrawn before completion | none | `cancelled_at` | Terminal cancellation |

### Transition Rules

1. `received -> queued` is automatic after successful intake validation and queue insertion.
2. `queued -> in-progress` requires an explicit operator or system command that work has started.
3. `in-progress -> ready` requires an explicit operator or system command that preparation is finished.
4. `ready -> completed` requires explicit handoff confirmation.
5. `cancelled` is valid from `received`, `queued`, `in-progress`, or `ready`, but not from `completed`.
6. Every accepted transition increments `aggregate_version` and writes a transition-log record.
7. Every rejected transition must return a domain error and write an audit record with the reason for rejection.

### Operational Modifiers

| Modifier | Applies To | Purpose | Downstream Visibility |
| --- | --- | --- | --- |
| `priority_band` | `queued` orders | Controls queue ordering without changing lifecycle | Always visible |
| `blocked_flag` | `queued`, `in-progress` | Exposes inability to proceed without pretending cancellation or completion | Always visible to reporting; visible to CustomerService when customer impact threshold is met |
| `manual_override_rank` | `queued` orders | Lets authorized actors reorder within the queue model | Always visible |
| `remake_flag` | `queued`, `in-progress`, `ready` | Marks recovery handling without creating a second lifecycle | Visible to reporting; visible to CustomerService only if it affects promised experience |

### Completion Representation

Completion is represented by all of the following being true:

- `lifecycle_state = completed`
- `completion_mode = handoff_confirmed`
- `completed_at` is populated
- `ready_at` remains preserved as the earlier milestone
- `completed_by` identifies the actor or approved integration that confirmed handoff

This makes `ready` and `completed` analytically and operationally distinct.

### Cancellation Representation

Valid cancellation is represented by:

- `lifecycle_state = cancelled`
- `cancelled_at` populated
- `cancelled_stage` set to one of `received`, `queued`, `in-progress`, or `ready`
- `cancellation_reason_code` populated from the canonical taxonomy
- `cancellation_actor_type` and `cancellation_actor_id` captured

Cancellation after completion is represented as a rejected cancellation command, not a lifecycle mutation.

## Queue and Prioritization Model

### Queue Views

StoreOperationsService maintains separate views for different kinds of work:

- `dispatch queue`: orders in `queued` that are eligible to be started
- `active work view`: orders in `in-progress`
- `handoff view`: orders in `ready`
- `terminal history`: orders in `completed` or `cancelled`

Only the dispatch queue is subject to next-up ordering logic.

### Standardized Priority Bands

Use exactly three queue priority bands in MVP:

- `rush`
- `at-risk`
- `standard`

`rush` is explicit operational prioritization.

`at-risk` is an automated fairness band for non-rush orders that have waited too long relative to their promise or configured store threshold.

`standard` is the default band.

### Rush Prioritization Rules

Rush handling must be explicit and auditable.

The standardized dispatch ordering for `queued` orders is:

1. exclude blocked orders from startable dispatch ordering
2. order by `priority_band` in this sequence: `rush`, `at-risk`, `standard`
3. within a band, order by `manual_override_rank` when present
4. then by `promise_due_at`
5. then by `queue_entered_at`
6. then by stable `store_order_id` tiebreaker

Additional rush rules:

- rush may be applied by CustomerService at intake or by an authorized store actor later
- the source of rush must be preserved as `priority_source`
- a rush order moves to the head of the not-started dispatch queue, but it does not automatically preempt work already in `in-progress`
- removing rush returns the order to its computed non-rush band with a recorded audit transition
- repeated rush application by unauthorized actors is rejected and audited

This is explicit enough to prevent one implementation from inserting rush orders at the front of every lane while another only adds a visual badge.

### Fairness and Starvation Control

Rush cannot erase the existence of older standard work.

For that reason, the service must automatically evaluate standard orders for promotion into `at-risk` when they cross an approved threshold. The threshold is a business-controlled configuration, but the behavior is standardized in the architecture.

### Blocked Order Handling

Blocked orders:

- remain operationally visible
- do not silently disappear
- are excluded from startable dispatch ranking while blocked
- preserve `blocked_reason_code`, `blocked_at`, and `resumed_at`
- emit blocked and resumed events so reporting can explain abnormal cycle time

## APIs and Events

### Command API

The operator-facing and service-to-service write surface should use explicit commands rather than open-ended partial updates.

| Command | Purpose | Required Inputs |
| --- | --- | --- |
| `POST /store-orders/intake` | Fallback synchronous intake path if event-based handoff is unavailable | `commandId`, `sourceEventId`, order payload, store payload |
| `POST /store-orders/{id}/start` | Move `queued` to `in-progress` | `commandId`, `expectedVersion`, actor context |
| `POST /store-orders/{id}/ready` | Move `in-progress` to `ready` | `commandId`, `expectedVersion`, actor context |
| `POST /store-orders/{id}/complete` | Confirm handoff and move `ready` to `completed` | `commandId`, `expectedVersion`, actor context, `completionMode=handoff_confirmed` |
| `POST /store-orders/{id}/cancel` | Cancel from a valid non-terminal state | `commandId`, `expectedVersion`, actor context, `reasonCode` |
| `POST /store-orders/{id}/priority` | Apply or remove rush or override rank | `commandId`, `expectedVersion`, actor context, priority payload |
| `POST /store-orders/{id}/block` | Mark the order as blocked | `commandId`, `expectedVersion`, actor context, `blockedReasonCode` |
| `POST /store-orders/{id}/resume` | Remove blocked status | `commandId`, `expectedVersion`, actor context |

Every command must be idempotent on `commandId` and concurrency-checked on `expectedVersion`.

### Query API

| Query | Purpose |
| --- | --- |
| `GET /stores/{storeId}/queue` | Current dispatch queue, active work, and handoff view |
| `GET /store-orders/{id}` | Current full aggregate snapshot including version and milestone timestamps |
| `GET /store-orders/{id}/history` | Transition log for audit and troubleshooting |
| `GET /stores/{storeId}/exceptions` | Blocked, cancelled, and at-risk operational views |

### Integration Events

The event set must be versioned and explicit:

- `store.order.received.v1`
- `store.order.queued.v1`
- `store.order.priority.changed.v1`
- `store.order.fulfillment.started.v1`
- `store.order.blocked.v1`
- `store.order.resumed.v1`
- `store.order.ready.v1`
- `store.order.completed.v1`
- `store.order.cancelled.v1`
- `store.order.intake.rejected.v1`
- `store.order.cancellation.rejected.v1`

### Event Envelope Requirements

Every published event must include:

- `event_id`
- `event_type`
- `occurred_at`
- `published_at`
- `store_order_id`
- `customer_order_id`
- `store_id`
- `aggregate_version`
- `lifecycle_state`
- `priority_band`
- `priority_source`
- `blocked_flag`
- `remake_flag`
- `received_at`
- `queued_at`
- `started_at`
- `ready_at`
- `completed_at`
- `completion_mode`
- `cancelled_at`
- `cancelled_stage`
- `cancellation_reason_code`
- `actor_type`
- `actor_id`
- `correlation_id`
- `causation_id`
- `trace_id`

The event payload is intentionally snapshot-based. Downstream systems should not be forced to infer current truth by replaying only partial deltas.

## Downstream Contract Requirements

### What CustomerService Must Receive

CustomerService needs enough information to avoid stale or misleading customer-facing updates. It must receive:

- the current `lifecycle_state`
- separate milestone timestamps for `ready` and `completed`
- current rush and priority context
- blocked or resumed signals when the condition affects promised service
- authoritative cancellation context including stage and reason
- rejected cancellation information when a customer attempts cancellation after completion

CustomerService must process events by `aggregate_version` and ignore any older version than the one it already holds.

If CustomerService detects a version gap or replay issue, it must reconcile through `GET /store-orders/{id}` rather than inferring missing lifecycle state.

### What CorporateReportingService Must Receive

CorporateReportingService needs the full milestone set and modifier history to avoid manufacturing incorrect operational analytics. It must receive:

- all lifecycle events
- priority changes and their source
- blocked and resumed intervals
- cancellation reason and stage
- the distinction between `ready` and `completed`
- immutable transition ordering via `aggregate_version`

Reporting must derive metrics from authoritative milestone timestamps, not from message arrival time.

### Stale-State Prevention Standards

To prevent downstream drift, all consumers must follow these standards:

1. treat `aggregate_version` as the source of event ordering per order
2. treat `ready` and `completed` as distinct milestones with different meanings
3. never infer cancellation stage from current state; use `cancelled_stage`
4. never infer rush from UI labels; use `priority_band` and `priority_source`
5. reconcile from the query API when an event gap or out-of-order arrival is detected

## Cancellation Propagation Design

### Valid Cancellation Flow

When a valid cancellation command is accepted:

1. validate that the order is not already `completed` or `cancelled`
2. capture the current lifecycle state as `cancelled_stage`
3. remove the order from dispatch, active work, or handoff projections immediately
4. persist the `cancelled` transition and cancellation metadata
5. increment `aggregate_version`
6. publish `store.order.cancelled.v1` with the full current-state snapshot

### Cancellation After Ready

Cancellation after `ready` but before `completed` is valid in MVP and is represented as:

- `lifecycle_state = cancelled`
- `cancelled_stage = ready`
- preserved `ready_at`
- downstream visibility that work had already reached handoff-ready state

This is important because it distinguishes abandoned prepared work from cancellation before production.

### Cancellation After Completion

Cancellation after `completed` is not represented as `cancelled`.

It is represented as:

- a rejected cancel command with a domain-conflict result
- an audit record and `store.order.cancellation.rejected.v1` event
- no change to `lifecycle_state`, timestamps, or queue state
- optional downstream customer recovery workflow handled outside StoreOperationsService

This is the architecture decision that prevents one team from silently changing terminal state while another team treats completion as final.

## Error Handling Strategy

### Domain Error Policy

Use explicit domain errors rather than generic transport failures for invalid state changes.

Standard categories:

- `invalid_transition`
- `stale_version`
- `duplicate_command`
- `unauthorized_priority_change`
- `intake_validation_failed`
- `already_completed`
- `already_cancelled`

Every domain error must be:

- returned to the caller with a stable machine-readable code
- logged with `store_order_id`, `commandId`, and `trace_id`
- auditable in the transition or command log

### Hidden Runtime Failure Paths Made Explicit

| Failure Path | Design Response | Observable Signal |
| --- | --- | --- |
| Incomplete or contradictory intake payload | Reject intake before creating active work; publish intake rejection event | Intake rejection metric and audit log |
| Duplicate upstream order handoff | Dedupe on `sourceEventId` and `customer_order_id` | Dedupe hit metric |
| Concurrent complete and cancel commands | Guard with `expectedVersion`; one wins, the other receives `stale_version` or `already_completed` | Transition rejection metric |
| Event publish failure after state commit | Preserve state, retry from transactional outbox, alert on lag | Outbox lag metric and alert |
| Queue projection lagging behind aggregate | Rebuild projection from transition log; expose projection age | Queue projection freshness metric |
| Unauthorized rush override | Reject command and audit actor identity | Authorization failure metric |
| Blocked order forgotten indefinitely | Emit blocked duration metrics and age-based alerts | Blocked age dashboard and alert |
| Clock skew from client devices | Use server-side canonical timestamps and keep client assertion time separately if needed | Timestamp skew warning metric |

### Reliability Patterns

The implementation must standardize these reliability controls:

- optimistic concurrency on every write command
- idempotency by `commandId` and upstream `sourceEventId`
- transactional outbox for publication
- dead-letter handling for repeatedly failing publications
- replay and reconciliation support from transition history

## Observability Requirements

### Metrics

At minimum, the service must expose:

- queue depth by store and priority band
- active work count by store
- ready backlog count by store
- lifecycle transition counts by type
- time from `received` to `started`
- time from `started` to `ready`
- time from `ready` to `completed`
- cancellation rate by stage and reason
- blocked duration by reason
- rush assignment and removal counts
- outbox lag and publish failure counts
- transition rejection counts by error code

### Structured Logging

Every accepted or rejected command should log:

- `store_order_id`
- `customer_order_id`
- `store_id`
- `commandId`
- `aggregate_version`
- `lifecycle_state`
- `actor_type`
- `actor_id`
- `trace_id`
- `correlation_id`

### Alerting

Alerts should trigger for:

- outbox publication lag above threshold
- blocked orders aging beyond threshold
- excessive `stale_version` or `invalid_transition` failures
- missing completion confirmations causing ready backlog growth
- sustained rush-band starvation of standard work

## Distributed Tracing Expectations

Use W3C trace-context propagation across all synchronous and asynchronous boundaries.

The standard is:

- CustomerService creates or forwards the incoming trace context
- StoreOperationsService preserves the same `trace_id` through intake, state mutation, and event publication
- every command starts a span that links back to the original order-intake correlation when possible
- published events include `trace_id`, `correlation_id`, and `causation_id`
- CorporateReportingService logs and stores the originating trace identifiers for end-to-end troubleshooting

Tracing must make these questions answerable from a single trace:

- where the order entered store ownership
- which actor or command changed its state
- when the queue decision changed
- whether downstream publication lagged behind the accepted state change

## Technical Decisions That Must Be Standardized Before Implementation Begins

The following items are mandatory standardization points. Implementation should not begin until they are fixed in shared contracts and documentation.

1. Completion semantic: `completed` means `handoff_confirmed`.
2. Terminal rule: `completed -> cancelled` is forbidden.
3. Priority bands and dispatch ordering must be identical across UI, API, and queue projection logic.
4. Canonical event envelope fields and event names must be versioned and frozen for MVP.
5. Cancellation reason codes and actor taxonomy must be fixed before downstream contract publication.
6. `aggregate_version`, `commandId`, and `sourceEventId` must be mandatory for concurrency and idempotency.
7. The service must use server-authored canonical timestamps in UTC ISO-8601 format.
8. Blocked and remake handling must remain modifiers, not alternate lifecycle states.
9. MVP does not support partial fulfillment or split completion. One store order resolves to one terminal lifecycle outcome.
10. Remake stays on the same store-order record as a recovery modifier in MVP rather than spawning a second operational lifecycle.

## Epic Breakdown

### Epic 1: Authoritative Order Intake and State Foundation

Goal: establish the single-writer aggregate, transition log, and intake validation boundary.

Deliverables:

- actionable-intent validation
- authoritative store-order aggregate
- transition log
- outbox-backed persistence contract

### Epic 2: Queueing, Rush Prioritization, and Exception Visibility

Goal: make the store queue deterministic, auditable, and operationally useful under rush and blocked conditions.

Deliverables:

- dispatch queue projection
- rush and override handling
- at-risk fairness logic
- blocked and resumed visibility

### Epic 3: Fulfillment Lifecycle and Operator Commands

Goal: implement explicit commands for start, ready, complete, and invalid-transition protection.

Deliverables:

- operator command endpoints
- optimistic concurrency enforcement
- explicit ready and completion milestones
- handoff confirmation logic

### Epic 4: Cancellation and Downstream Contract Publication

Goal: standardize cancellation behavior by stage and publish stable downstream contracts.

Deliverables:

- cancellation by stage
- post-completion cancellation rejection
- versioned lifecycle events
- reconciliation API for downstream consumers

### Epic 5: Observability, Tracing, and Operational Hardening

Goal: make failure modes visible and give downstream teams confidence in service truth.

Deliverables:

- metrics and dashboards
- tracing propagation
- alerting on lag, drift, and queue health
- replay and contract verification support

## Story Breakdown

### Epic 1 Stories

#### Story 1.1: Define the canonical store-order schema

- create the aggregate snapshot model and transition-log model
- include milestone timestamps, priority fields, and cancellation context
- document the canonical enums and field meanings

#### Story 1.2: Ingest actionable order intent with validation and dedupe

- accept CustomerService handoff
- reject incomplete or contradictory payloads
- dedupe retries on upstream message identity

#### Story 1.3: Persist state, transition log, queue projection, and outbox atomically

- ensure one accepted transition updates all authoritative write-side artifacts in one transaction
- guarantee the queue cannot move without the state change that caused it

### Epic 2 Stories

#### Story 2.1: Build the store queue projection and queue query API

- expose dispatch, active work, and handoff views
- ensure the queue view is derived from authoritative state

#### Story 2.2: Implement rush and manual reprioritization controls

- apply and remove rush with authority checks and audit history
- preserve priority source and override rank

#### Story 2.3: Implement blocked, resumed, and at-risk queue behavior

- exclude blocked work from startable dispatch ordering
- surface blocked age and at-risk promotion without changing core lifecycle state

### Epic 3 Stories

#### Story 3.1: Implement `start` with optimistic concurrency and idempotency

- move `queued` to `in-progress`
- reject stale or duplicate commands safely

#### Story 3.2: Implement `ready` and `complete` as separate milestones

- preserve `ready_at`
- require explicit handoff confirmation to set `completed`

#### Story 3.3: Implement invalid-transition rejection and audit logging

- return stable domain error codes
- record rejected transitions for support and reporting

### Epic 4 Stories

#### Story 4.1: Implement cancellation before start, during work, and after ready

- set `cancelled_stage` correctly based on the last trusted lifecycle state
- remove cancelled work from active execution views immediately

#### Story 4.2: Publish versioned lifecycle and modifier events with full snapshots

- emit the canonical event set from the outbox
- include milestone timestamps, priority context, and cancellation context

#### Story 4.3: Reject post-completion cancellation and hand off recovery correctly

- keep completed orders terminal
- publish `store.order.cancellation.rejected.v1`
- expose the rejection to CustomerService for non-store recovery handling

#### Story 4.4: Provide reconciliation and history APIs for downstream consumers

- support snapshot fetch by order id
- support transition-history inspection for troubleshooting and replay

### Epic 5 Stories

#### Story 5.1: Instrument metrics, logs, and traces for all accepted and rejected commands

- track queue health, latency, lag, and rejection categories
- propagate trace and correlation identifiers end to end

#### Story 5.2: Add alerts and dashboards for operational drift and hidden failure paths

- alert on blocked age, ready backlog, outbox lag, and excessive transition failures

#### Story 5.3: Add contract verification and replay support

- verify event envelopes and state snapshots against canonical schemas
- support replay-driven rebuilding of queue projections and downstream reconciliation

## Recommended Implementation Order

1. Epic 1
2. Epic 2
3. Epic 3
4. Epic 4
5. Epic 5

This sequence ensures that queueing, completion, cancellation, and downstream publication are built on top of a single authoritative model instead of being stitched together after the fact.

## Implementation Readiness Assessment

This architecture is ready for implementation once the two listed blockers are approved:

- rush-authority and at-risk threshold signoff
- cancellation reason taxonomy signoff

Everything else required to keep StoreOperationsService technically consistent is now explicit: state semantics, queue behavior, completion meaning, cancellation propagation, downstream contracts, tracing, and the implementation sequence.