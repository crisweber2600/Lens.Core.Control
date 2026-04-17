---
feature: coffee-store-service-mvp
doc_type: finalizeplan-review
status: in-review
goal: "Final cross-artifact review and governance cross-check before bundled planning execution and PR handoff."
key_decisions:
  - "Treat product-brief + prd + architecture as the combined TechPlan artifact set per user override."
  - "Two hard-gate constitutional blockers (business-plan, aspire-plan) must be resolved before dev promotion."
open_questions:
  - "Will a dedicated business-plan.md be authored, or will the constitution be amended to accept prd as the equivalent?"
  - "Which AppHost resources and Aspire integrations does store-service MVP require for the aspire-plan?"
  - "What qualifies an order as rush, and who may set or revoke that flag?"
  - "What store event counts as completed: handoff-ready, handoff-confirmed, or customer-collected?"
  - "What SLA threshold auto-elevates a waiting order to the at-risk queue band?"
  - "Which cancellation reason codes require finance or customer recovery semantics?"
depends_on:
  - "CustomerService"
  - "CorporateReportingService"
blocks:
  - "Produce business-plan.md (hard gate — coffee domain constitution)"
  - "Produce aspire-plan.md (hard gate — coffee Aspire Mandate)"
  - "Resolve rush-authority matrix and at-risk escalation thresholds before dev"
  - "Resolve cancellation reason taxonomy before downstream contract publication"
updated_at: 2026-04-17T00:00:00Z
---

# FinalizePlan Review — StoreOperationsService MVP

`[adversarial-review:finalizeplan] source=manual-rerun`

## Review Scope

**Artifact set reviewed:**
- `docs/coffee/store-service/mvp/product-brief.md` — operational problem analysis and workflow mapping
- `docs/coffee/store-service/mvp/prd.md` — service requirements, ownership boundaries, and order-state lifecycle
- `docs/coffee/store-service/mvp/architecture.md` — technical design decisions, aggregate model, and downstream contracts

**Predecessor context:** Phase-gate override applied — no separate PrePlan / BusinessPlan / TechPlan phase history exists; existing staged docs treated as combined TechPlan output.

**Constitution applied:** `coffee` domain (hard gate) — service-level constitution for `store-service` does not yet exist.

---

## Verdict

> **CONDITIONAL PASS** — Planning artifact quality is high. Two hard-gate constitutional blockers must be resolved before dev promotion. The downstream planning bundle may proceed but must carry both blockers as explicit pre-dev-promotion action items.

---

## Findings by Severity

### CRITICAL — Hard Gate Blockers (block dev promotion)

#### C1 · Missing `business-plan` artifact

**Finding:** The coffee domain constitution (`TargetProjects/lens/lens-governance/constitutions/coffee/constitution.md`) declares `gate_mode: hard` and lists `business-plan` as a required planning artifact before dev promotion. The staged planning set contains `product-brief.md`, `prd.md`, and `architecture.md`, but no `business-plan.md`.

The product brief captures operational problem analysis and the PRD captures behavioral requirements, but neither is registered as a `business-plan` under the current constitution. Downstream story acceptance and the constitution-gated dev-ready checkpoint will fail without this artifact.

**Impact:** Blocks `dev-ready` checkpoint (constitution hard gate).

**Required action:**
1. **Option A:** Author a `business-plan.md` that establishes the business case, value proposition, success metrics, and cost/risk posture for StoreOperationsService MVP.
2. **Option B:** Open a constitution amendment PR that explicitly accepts `prd` as the satisfying artifact for the `business-plan` gate in the coffee domain.

---

#### C2 · Missing `aspire-plan` artifact

**Finding:** The coffee domain constitution includes an Aspire Mandate — a hard gate requiring an `aspire-plan` document for any feature that adds, modifies, or removes service infrastructure. StoreOperationsService is a **new service** being introduced into the coffee workstream and must be registered as an Aspire resource in the shared AppHost. No `aspire-plan.md` exists in the staged docs.

The architecture doc references a `Queue and Priority Engine`, an `Outbox Publisher`, and a relational data store — all are infrastructure concerns requiring Aspire registration. Without an `aspire-plan`, the Aspire Mandate will block dev-ready.

**Impact:** Blocks `dev-ready` checkpoint (coffee Aspire Mandate hard gate).

**Required action:** Author `aspire-plan.md` covering:
- Which existing AppHost project registers `store-service` as a resource
- Any new Aspire integrations required (DB, cache, messaging — e.g., `aspire add postgresql`, `aspire add rabbitmq`)
- How the feature was validated using `aspire start` and `aspire describe`
- Which other coffee services are co-registered and whether the shared AppHost is the same as the one used by customer-service and order-service

---

### MAJOR — Planning Gaps (block story authoring or implementation without a stated resolution)

#### M1 · Open questions remain unresolved across all three docs

The following open questions appear in the staged artifacts without stated resolutions or deferral decisions. Story authoring will hit ambiguity on each:

| Source | Open Question | Risk If Unresolved |
|--------|--------------|-------------------|
| product-brief, prd | What qualifies an order as rush, and who may set or revoke it? | `in-progress -> ready` stories can't set acceptance criteria for priority escalation |
| product-brief, prd | What event marks completion: handoff-ready, handoff-confirmed, or pickup? | Terminal state story acceptance criteria are undefined |
| product-brief | How are cancellations after preparation started classified and reported? | `in-progress -> cancelled` stories lack classification semantics |
| architecture | What SLA threshold auto-elevates a waiting order to at-risk? | Queue ordering stories are unwritable without this value |
| architecture | Which cancellation reason codes require finance/CustomerService semantics? | Downstream event contract will be incomplete at story authoring time |

**Required action:** Resolve each question with a stated decision and owner, or explicitly defer with a spike story and a stated fallback assumption.

---

#### M2 · Architecture approval blockers not resolved

The architecture.md `blocks:` frontmatter carries two items that are stated prerequisites for implementation:
- *"Approve the rush-authority matrix and at-risk escalation thresholds before implementation starts."*
- *"Approve the canonical cancellation reason taxonomy before downstream contract publication."*

These are not tagged as spike work or deferred decisions. Without resolution, the architecture is self-stated as not ready for implementation.

**Required action:** Either record the approved decisions in the architecture document, or convert each to an explicit spike story with a time-box and a fallback.

---

#### M3 · Queue ordering rules absent

The architecture introduces a `Queue and Priority Engine` as a logical component but never defines the ordering algorithm applied to the active store queue. Stories cannot be acceptance-tested without a stated queue ordering contract:
- Is within-priority ordering FIFO by intake timestamp?
- Does promised timing override queue position?
- How do rush orders interact with in-progress orders at adjacent stations?

**Required action:** Add a queue ordering specification to the architecture (a decision appendix is sufficient) before sprint planning assigns story points to queue-related stories.

---

#### M4 · No service-level constitution for `store-service`

The governance repo contains constitutions for `coffee/customer-service` and `coffee/order-service`, but not for `coffee/store-service`. This means store-service has no service-level governance constraints and inherits only the domain constitution.

**Required action:** Create `TargetProjects/lens/lens-governance/constitutions/coffee/store-service/constitution.md` before the feature PR is merged, even if it delegates all rules to the domain level. This ensures the governance inventory is complete and future changes have the right surface to extend.

---

### MINOR — Quality Gaps (addressable in stories or a future iteration)

#### m1 · Outbox retry, dead-letter, and idempotency contracts absent

The architecture correctly calls for a transactional outbox but does not define:
- Maximum delivery attempts before DLQ routing
- DLQ consumer responsibilities
- Downstream consumer idempotency requirements (event dedup key, at-least-once vs exactly-once expectations)

These are solvable in a dedicated infrastructure story but need to be present in the sprint plan.

---

#### m2 · Multi-store order routing edge case unaddressed

If CustomerService contains a bug that routes the same order to two stores simultaneously, the architecture's dedup logic at intake is the only defense. No circuit-breaker, conflict-detection, or escalation path is described. At MVP scale this is low probability but the absence is a blind spot for store operations support teams.

---

#### m3 · `in-progress -> cancelled` compensation workflow not scoped

The PRD permits `in-progress -> cancelled` as a valid transition but the product-brief notes that cancellations after preparation has started have different operational and reporting consequences. Neither document specifies a remake/waste accounting story or a reporting event shape for this case.

---

#### m4 · Downstream event versioning scheme absent

The architecture specifies "versioned snapshot events" but does not define the versioning scheme, the schema evolution contract, or the deprecation lifecycle. This is a minor gap for MVP but will become a breaking risk when CustomerService or CorporateReportingService need to consume a second event version.

---

## Governance Impact Findings

| Service / Concern | Impact | Action |
|------------------|--------|--------|
| `CustomerService` | Depends on StoreOperationsService for operational state display and customer recovery triggers. Cancellation reason taxonomy (M1, M2) directly affects CustomerService's recovery workflow design. | Align on cancellation taxonomy before CustomerService story authoring |
| `CorporateReportingService` | Consumes domain events as its sole operational data source. Versioning scheme absence (m4) creates downstream schema risk. | Define event versioning contract before dev promotion |
| `coffee AppHost` | StoreOperationsService must be registered as an Aspire resource — the AppHost project must exist or be created as part of this feature. | Covered by C2 aspire-plan action |
| `coffee/store-service constitution` | No service-level constitution exists — governance inventory is incomplete. | Covered by M4 action |

---

## Party-Mode Blind-Spot Challenge

The following three challenge voices were run after adversarial findings were written. Capture any answers as additions to this review document or as new open questions in the affected artifacts.

---

**Voice 1 — Blind Hunter (what the authors may have overlooked):**

> "The architecture defines `ready -> cancelled` as a valid transition, but the PRD states CustomerService handles post-completion customer recovery. If an order reaches `ready` and a cancellation arrives simultaneously — a race condition between the customer canceling and the handoff operator confirming — who wins? The architecture's optimistic concurrency model resolves the transaction winner, but does the loser get a compensating event, or does the race result in a silent final state that CustomerService infers from a missing `completed` event? This boundary is not stated."

---

**Voice 2 — Edge Case Hunter (boundary conditions):**

> "The Outbox Publisher commits after the aggregate transaction succeeds. What is the delivery guarantee when the publisher service restarts mid-batch? Without a stated at-least-once delivery contract and a downstream consumer dedup key, a restart could either lose events or replay them. Neither `CorporateReportingService` nor `CustomerService` has a stated dedup contract in these docs. The outbox guarantees write durability but downstream dedup is a separate contract that must exist before the first integration story is written."

---

**Voice 3 — Acceptance Auditor (will stories actually be testable?):**

> "The coffee domain constitution requires `tdd-red-evidence` and `reqnroll-feature-files` for dev promotion. SpecFlow/Reqnroll `.feature` files for queue ordering, rush escalation, and the cancellation taxonomy cannot be written until M1 (open questions) and M2 (architecture blockers) are resolved. If FinalizePlan proceeds with these gaps open, QA will author feature files against assumed behavior that is later overridden by implementation decisions — producing mismatched red-phase evidence that fails the constitution gate retroactively. Resolving M1 and M2 before story creation is the safest sequencing."

---

## Action Item Summary

| ID | Severity | Action | Owner |
|----|----------|--------|-------|
| C1 | Critical | Author `business-plan.md` OR amend coffee constitution | cweber |
| C2 | Critical | Author `aspire-plan.md` with AppHost registration details | cweber |
| M1 | Major | Resolve all six open questions with decisions or explicit spike deferral | cweber |
| M2 | Major | Resolve rush-authority matrix and cancellation taxonomy in architecture | cweber |
| M3 | Major | Add queue ordering specification to architecture | cweber |
| M4 | Major | Create `coffee/store-service/constitution.md` in governance | cweber |
| m1 | Minor | Add Outbox retry/DLQ/idempotency story to sprint plan | cweber |
| m2 | Minor | Add multi-store routing edge case to risk register | cweber |
| m3 | Minor | Add `in-progress -> cancelled` compensation story | cweber |
| m4 | Minor | Define event versioning scheme before second consumer onboards | cweber |

---

## Merge Readiness Assessment

| Check | Status | Notes |
|-------|--------|-------|
| Both `mvp` and `mvp-plan` branches exist | ✓ Pass | Confirmed |
| Staged planning docs present | ✓ Pass | product-brief, prd, architecture |
| No unresolved merge conflicts | ✓ Pass | Branch SHA matches main (no diverging commits yet) |
| Constitution hard gates satisfied | ✗ Fail | business-plan and aspire-plan missing (C1, C2) |
| Open questions resolved or deferred | ✗ Fail | Six questions remain without stated disposition |
| Architecture approval blockers cleared | ✗ Fail | Rush matrix and cancellation taxonomy unresolved |
| Service-level constitution exists | ✗ Fail | store-service constitution not created |

**Overall:** Conditional pass. The planning bundle (epics, stories, readiness, sprint) may proceed. Dev promotion requires C1 and C2 resolved at minimum before the `dev-ready` constitution gate runs.
