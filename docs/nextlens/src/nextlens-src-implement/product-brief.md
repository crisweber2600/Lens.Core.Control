---
feature: nextlens-src-implement
doc_type: product-brief
status: draft
goal: "Deliver the smallest useful top-down NextLens bridge from discovered system context to one selected Feature packet with deterministic validation and correction loops"
key_decisions:
  - Keep a single interactive command spine that consumes top-down discovery output for v1
  - Treat stable-ID landscape state as authoritative and derived graph as disposable projection
  - Enforce idempotent mutating operations and machine-readable non-mutating doctor checks
  - Route correction through deduplicated multi-source Salmon signals with strict write boundaries
open_questions:
  - What exact BMAD consumer hints are mandatory in the v1 packet schema?
  - What threshold should trigger automatic versus manual Salmon routing escalation?
depends_on: [brainstorm, research]
blocks: []
updated_at: 2026-05-14T00:00:00Z
---

# Product Brief - nextlens-src-implement

## 1. Vision

Create a governed command experience that turns top-down LENS discovery output into one implementation-ready Feature packet with auditable evidence, while preserving Lens lifecycle boundaries and minimizing first-release complexity.

The product should prove, end-to-end, that NextLens can:

- capture intent,
- select one Feature deterministically,
- emit a BMAD-ready packet,
- validate outcomes through doctor checks,
- and route corrections upstream without governance or release boundary violations.

NextLens v1 is the deterministic Feature-selection and BMAD-packet bridge for the larger top-down LENS flow. It is not the full discovery system; it consumes structured discovery output when available, checks whether that context is sufficient, ranks candidate Features, confirms exactly one selected Feature, and emits the packet BMAD needs to plan only that selected scope.

## 2. Problem Statement

Current planning and implementation surfaces contain the right concepts but no constrained operational path that consistently converts discovered system context into a single validated Feature packet. This causes:

- repeated manual interpretation,
- drift between authoritative and derived artifacts,
- weak retry safety for mutating flows,
- inconsistent correction handling across human and automated sources.

Without a smallest-useful path, preplan outcomes remain informative but not reliably executable.

## 3. Target Users

- Lens feature operators in control-repo workflows who need a deterministic first implementation path.
- BMAD downstream authors who need one clearly bounded packet rather than broad context dumps.
- Review and governance stakeholders who need machine-verifiable evidence and strict write-boundary behavior.

## 4. Goals

### G1 - Deterministic Top-Down Bridge

Provide one command-driven flow from top-down context to one selected Feature packet with explicit confirmation and reproducible outputs.

### G2 - Authoritative State Discipline

Keep stable-ID landscape artifacts authoritative and derived graph artifacts projection-only, with deterministic eager rebuild after successful writes.

### G3 - Retry Safety and Validation Clarity

Implement idempotent mutating operations and JSONL doctor reporting for CI and audit tooling.

### G4 - Upstream Correction Integrity

Support multi-source correction input (human, doctor, review, implementation) through deduplicated Salmon signaling with explicit routing classes across Feature, Journey, Outcome, Role, Operating Loop, landscape, and BMAD correct-course impacts.

### G5 - Boundary Safety

Enforce zero direct governance/release mutations from this flow; writes must stay within approved control docs and implementation target paths.

## 5. Non-Goals

- Full multi-Feature packet generation in v1.
- Full discovery epoch implementation.
- Bottom-up standalone Feature mode.
- Automatic capability/domain promotion.
- Auspex dashboard implementation.
- Full landscape authoring UI.
- Broad workflow automation installation beyond minimum evidence path.
- Lifecycle phase rewiring outside preplan artifact completion.
- Replacing existing governance publication contracts.
- Replacing BMAD PRD, UX, architecture, epics, stories, implementation, review, or correct-course.

## 6. Scope

### In Scope

- Single interactive entry flow with inline ingestion.
- Top-down context sufficiency gate for system thesis, roles, outcomes, journeys or hypotheses, candidate Feature traces, risks, open questions, and BMAD consumer context.
- Stable-ID write model and eager projection rebuild.
- One selected packet with human confirmation gate.
- JSONL doctor report schema and pre-flight enforcement.
- Deduplicated Salmon correction event model.
- Evidence bundle generation for review traceability.

### Out of Scope

- Full automation rollout for all template workflows.
- Deep migration tools for historical semantic-ID renames.
- Adjacent journey or future Feature expansion beyond the selected packet.

## 7. Success Criteria

- One run reliably produces one selected packet and one evidence bundle.
- Complete top-down context preserves traceability from system to Role, Outcome, Journey, and Feature.
- Insufficient context blocks packet emission and recommends return to discovery.
- Doctor output is JSONL, non-mutating, and machine-parseable.
- Duplicate request replay does not create duplicate side effects.
- Derived graph is always consistent with latest authoritative writes.
- At least one correction path is demonstrably routed through Salmon with deduplication.
- No direct governance/release writes occur during execution.

## 8. Risks and Mitigations

- Vocabulary drift between Feature and legacy slice terms.
  Mitigation: use Feature as the official public operational unit; only mention legacy "slice" terminology when explicitly referencing prior discussion.

- Status drift across planning and execution artifacts.
  Mitigation: cross-surface reconciliation checks in doctor.

- Retry side effects under transient failure.
  Mitigation: idempotency token store with response replay.

- Correction signal noise from multi-source reporting.
  Mitigation: fingerprint-based event deduplication.

## 9. Delivery Sequence

1. Implement command spine and top-down context intake.
2. Run context sufficiency gate before ranking.
3. Persist authoritative stable-ID landscape artifacts.
4. Rebuild derived graph eagerly after each successful write.
5. Rank and confirm one Feature for packet creation.
6. Emit packet, doctor JSONL report, and evidence bundle.
7. Route corrections through Salmon with deduplication.

## 10. Expected Next Action

After this brief, proceed to businessplan with this product intent as the baseline for PRD and UX elaboration, while preserving the constrained v1 execution boundary established here.
