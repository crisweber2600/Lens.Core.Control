---
doc_type: business-plan
feature: coffee-store-service-mvp
status: draft
updated_at: '2026-05-07T00:00:00Z'
---

# Business Plan — coffee-store-service-mvp

## Problem Statement

Coffee stores operate under conditions that punish ambiguity: short order cycle times (typically 3–8 minutes), limited concurrent station capacity, high interruption rates from rushes, remakes, and walk-ins, and staff who cannot afford to pause work to resolve coordination confusion. Today, in-store order execution is frequently coordinated through a combination of ad hoc verbal calls, customer-facing display screens that lag behind floor reality, and disconnected point-of-sale signals that convey customer intent but not operational state.

The result is predictable and costly:

- **Queue drift**: the sequence visible to baristas diverges from what the floor actually produced, causing out-of-order completions and missed SLAs.
- **Phantom orders**: cancelled or voided orders remain visible in production queues until manually cleared, triggering unnecessary remakes.
- **Invisible rush pressure**: a shift lead designates an order as rush, but that designation is carried verbally or via workarounds, reaching some stations and not others.
- **Handoff uncertainty**: the expediter does not know whether an order is completed, remade, or still in-progress when the customer is at the counter — every handoff involves a moment of floor-scanning and verbal confirmation.
- **Lost audit trails**: when a cancellation happens mid-production, neither the cause nor the stage context is preserved, making service recovery, coaching, and trend analysis impossible.

The core question that remains unanswered at critical moments is: *"What should the store do next, with which order, and what is the true state of that work right now?"*

`StoreOperationsService` is built to answer that question with authority and precision, at all times, for every order active in the store.

---

## Value Proposition

`StoreOperationsService` establishes a single authoritative operational record for every order from the moment it is accepted and assigned to a store through final handoff or cancellation. The following measurable improvements are the direct output of replacing ad hoc coordination with an event-sourced operational truth layer:

| Outcome | Current State | Target State | Measurement Signal |
|---|---|---|---|
| Queue visibility | Latency varies; floor state often 30–90s behind display | All active orders visible in ≤3 seconds | p95 queue-sync latency |
| Priority accuracy | Rush designation verbal/informal; propagation incomplete | Rush orders surface correctly ≥99% of transitions | Rush-state divergence events per shift |
| Cancellation auditability | Stage context lost at cancellation time | 100% of cancellations preserve reason code and production stage | Audit record completeness rate |
| Remake rate (queue-cause) | Baseline unmeasured; estimated 8–12% of orders affected | ≥15% reduction in remakes attributable to incorrect queue state | Remake event correlation to queue-state divergence |
| Handoff confidence | Expediter confirmation verbal; ~15 seconds per order | Deterministic ready-state confirmation; zero verbal confirmation required | Handoff resolution time |

Beyond individual metrics, the broader value proposition is **operational data quality**. When every transition carries a timestamp, actor, stage, and reason code, the store gains a coaching and staffing instrument that was not previously possible. The Store Manager gains reliable signal on rush spike frequency, cancellation cause distribution, stall conditions by station, and throughput patterns — not as a reporting add-on but as an emergent property of operational truth capture.

The service does not replace or extend the customer experience, pricing logic, inventory management, or enterprise reporting definitions. Its value is narrow and deep: make the floor's actual state knowable to everyone on the floor, in real time, with full history.

---

## Target Personas

The following personas are drawn directly from the product brief and represent the complete set of store-floor roles whose work is directly affected by `StoreOperationsService`.

### 1. Shift Lead

**Role context:** The shift lead owns queue health, throughput, and exception handling for the shift. They make real-time tradeoffs between fairness, speed, labor allocation, and customer recovery.

**What they need from this service:**
- Real-time visibility into the full active backlog: count, age, station assignment, current state
- Rush designation with immediate, confirmed propagation to all stations
- Stall and block signals when an order has not progressed past a stage within expected time
- Cancellation authority with captured reason, enabling shift-end review without reconstructing from memory

**Impact of the current state:** Without a reliable operational record, the shift lead manages queue health through constant floor presence and verbal check-ins. Rush treatment is informal and inconsistently applied. Cancellation decisions leave no trace. Recovery from service failures requires reconstruction rather than review.

---

### 2. Barista / Production Operator

**Role context:** The barista executes production work at a station. Their cognitive load is highest during rush windows; unnecessary context switching is a direct throughput and quality risk.

**What they need from this service:**
- Clear, deterministic next-order signal for their station
- Visible and persistent rush markers that do not require verbal confirmation
- Confidence that completed or cancelled work will not reappear in their queue
- No interruption to the production flow from display lag or queue ghost items

**Impact of the current state:** Queue inconsistency forces baristas to make judgment calls about which order is actually next, particularly when rush designations arrive mid-flow or cancellations are not immediately propagated. Each judgment call is a potential mistake and a distraction from production quality.

---

### 3. Expediter / Handoff Operator

**Role context:** The expediter is the last internal checkpoint before the customer receives their order. They operate at the intersection of floor truth and customer expectation.

**What they need from this service:**
- Deterministic order state at handoff: in-progress, ready, remade, or cancelled — with no ambiguity
- Ability to confirm ready state without verbal floor confirmation
- Visibility into whether a remake is in progress and at what stage
- Certainty that the customer-visible status reflects actual floor state

**Impact of the current state:** The expediter absorbs the cost of every coordination failure upstream. When a customer arrives and the expediter cannot confirm order state without scanning the floor, the service failure is already in progress. The expediter currently carries the highest cognitive load relative to their role's intended scope.

---

### 4. Store Manager

**Role context:** The store manager reviews operational patterns across shifts: rush spike frequency, cancellation cause distributions, staffing gaps, bottlenecks, and service recovery trends. Their decisions are downstream of data quality.

**What they need from this service:**
- Reliable operational event history, not cosmetic status snapshots
- Rush spike timing and frequency by shift and day-part
- Cancellation reason breakdown by stage and cause
- Stall frequency by station and shift
- Throughput comparison across staffing configurations

**Impact of the current state:** Without structured operational capture, the store manager relies on end-of-shift verbal summaries, incomplete POS data, and customer complaint signals. Coaching conversations are reconstructive rather than evidence-based. Staffing decisions are made on intuition rather than quantified bottleneck patterns.

---

## Revenue and Cost Assumptions

### Build Cost Estimate

The MVP is scoped as a focused .NET 9 / C# microservice implemented over a structured sprint sequence following the epics defined in `epics.md`. Cost assumptions are based on a small dedicated team executing against a well-scoped story backlog.

| Cost Category | Assumption | Estimated Cost |
|---|---|---|
| Engineering (backend) | 1 senior engineer × 6 sprints × 2 weeks | Primary build cost |
| Engineering (integration) | Shared time with CustomerService and OrderService teams for contract alignment | Incremental cost |
| Infrastructure | .NET Aspire shared AppHost; existing CI/CD pipeline; no new cloud services at MVP | Low incremental cost |
| Event store / outbox | Implemented via existing patterns in the shared infrastructure; no new tooling purchase | Included in engineering |
| QA and observability | Integrated into sprint cadence; no dedicated QA headcount at MVP | Included in engineering |

Total MVP build is expected to fall within a standard feature-team sprint budget. No new vendor licensing, external tooling purchases, or infrastructure provisioning is required at MVP scope.

### Expected ROI and Operational Metrics

This service does not generate direct revenue. Its business case rests on **operational cost avoidance** and **service quality improvement** at the store level.

**Remake cost avoidance:** If the current remake rate attributable to queue-state errors is estimated at 8–12% of orders, and the service achieves the target ≥15% reduction in that subset, the per-store cost avoidance (waste materials, labor, customer recovery) is meaningful at scale. Across a multi-store deployment, even conservative remake reduction yields a positive return within the first operational quarter.

**Labor efficiency:** Eliminating verbal confirmation loops at handoff and reducing the shift lead's floor-scanning overhead recovers meaningful labor minutes per shift. At high-volume stores, this compounds across shifts and headcount.

**Service recovery speed:** Cancellation reason capture enables faster, evidence-based service recovery decisions. Reduced time-to-resolution for escalated orders reduces comp and refund costs.

**Data quality for staffing decisions:** Once the Store Manager has reliable operational history, staffing optimization decisions (shift composition, station assignment, rush period coverage) become quantifiable. Even a single staffing optimization that reduces overtime or covers a rush gap yields return well in excess of the MVP build cost.

**Break-even horizon:** Based on remake rate reduction alone at a single mid-volume store, the MVP is expected to reach operational break-even within 8–12 weeks of go-live, assuming normal ramp time for staff adoption.

---

## Business Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| **Adoption failure — staff bypass the system** if the queue UI is not intuitive or fast enough for floor conditions, staff will revert to verbal coordination, leaving the service underutilized and its data untrustworthy. | High | High | UX validation with actual shift staff before go-live. Hard gate: shift lead and barista persona reps must sign off on queue interaction flow before Epic 3 (operational readiness). Fast-path interaction for common actions (next order, mark complete, mark rush) must require ≤2 taps. |
| **Integration contract instability — CustomerService handoff contract not yet frozen.** If the order-accepted event schema changes after `StoreOperationsService` is built against it, the order receipt flow breaks silently or noisily. | Medium | High | Define and version the integration contract as a shared artifact before Epic 1 implementation begins. Require change-notification protocol from CustomerService team. Add contract validation tests that run in CI on both sides. |
| **Event sourcing complexity — transactional outbox failure causes duplicate or lost operational state.** The event sourcing + outbox pattern is correct for this domain but introduces failure modes (duplicate dispatch, missed events, replay inconsistency) that require explicit handling. | Medium | High | Enforce idempotency on all event handlers. Include chaos/fault injection tests for outbox failure scenarios in Epic 2. Document replay and reconciliation procedure before go-live. |
| **Rush authority inflation — no formal rule on who can designate rush.** If any actor can mark any order as rush without constraint, priority inflation degrades the signal value and creates floor conflict. | Medium | Medium | Define rush-authority policy in domain constitution before implementation. Encode allowed actors (shift lead only at MVP) in the command handler. Log every rush designation with actor ID for auditability. |
| **Scope creep — multi-store routing flagged as potentially in MVP scope.** If multi-store routing is added mid-sprint, the data model, event schema, and API surface expand significantly, threatening MVP delivery timeline and focus. | Low | High | Formal scope freeze decision required before Epic 1 starts: multi-store routing is explicitly deferred to post-MVP unless a signed business requirement is added to the PRD. Any scope addition requires PRD amendment and sprint replanning. |
