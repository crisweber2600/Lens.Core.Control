---
feature: nextlens-src-implement
doc_type: product-brief
status: draft
goal: "Deliver the smallest useful NextLens implementation path from ambiguous product context to one selected feature packet with deterministic validation and correction loops"
key_decisions:
  - Keep a single interactive command spine with inline context ingestion for v1
  - Treat stable-ID landscape state as authoritative and derived graph as disposable projection
  - Enforce idempotent mutating operations and machine-readable non-mutating doctor checks
  - Route correction through deduplicated multi-source Salmon signals with strict write boundaries
open_questions:
  - What minimum product-brief metadata fields are mandatory in v1 packet schema?
  - What threshold should trigger automatic versus manual Salmon routing escalation?
depends_on: [brainstorm, research]
blocks: []
updated_at: 2026-05-14T00:00:00Z
---

# Product Brief - nextlens-src-implement

## 1. Vision

Create a governed command experience that turns uncertain product context into one implementation-ready feature packet with auditable evidence, while preserving Lens lifecycle boundaries and minimizing first-release complexity.

The product should prove, end-to-end, that NextLens can:

- capture intent,
- select one feature deterministically,
- emit a BMAD-ready packet,
- validate outcomes through doctor checks,
- and route corrections upstream without governance or release boundary violations.

## 2. Problem Statement

Current planning and implementation surfaces contain the right concepts but no constrained operational path that consistently converts ambiguous input into a single validated output packet. This causes:

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

### G1 - Deterministic First Path

Provide one command-driven flow from ambiguous context to one selected feature packet with explicit confirmation and reproducible outputs.

### G2 - Authoritative State Discipline

Keep stable-ID landscape artifacts authoritative and derived graph artifacts projection-only, with deterministic eager rebuild after successful writes.

### G3 - Retry Safety and Validation Clarity

Implement idempotent mutating operations and JSONL doctor reporting for CI and audit tooling.

### G4 - Upstream Correction Integrity

Support multi-source correction input (human, doctor, review) through deduplicated Salmon signaling with explicit routing classes.

### G5 - Boundary Safety

Enforce zero direct governance/release mutations from this flow; writes must stay within approved control docs and implementation target paths.

## 5. Non-Goals

- Full multi-feature packet generation in v1.
- Broad workflow automation installation beyond minimum evidence path.
- Lifecycle phase rewiring outside preplan artifact completion.
- Replacing existing governance publication contracts.

## 6. Scope

### In Scope

- Single interactive entry flow with inline ingestion.
- Stable-ID write model and eager projection rebuild.
- One selected packet with human confirmation gate.
- JSONL doctor report schema and pre-flight enforcement.
- Deduplicated Salmon correction event model.
- Evidence bundle generation for review traceability.

### Out of Scope

- Full automation rollout for all template workflows.
- Deep migration tools for historical semantic-ID renames.
- Additional package authoring beyond first selected packet.

## 7. Success Criteria

- One run reliably produces one selected packet and one evidence bundle.
- Doctor output is JSONL, non-mutating, and machine-parseable.
- Duplicate request replay does not create duplicate side effects.
- Derived graph is always consistent with latest authoritative writes.
- At least one correction path is demonstrably routed through Salmon with deduplication.
- No direct governance/release writes occur during execution.

## 8. Risks and Mitigations

- Vocabulary drift between feature and slice terms.
  Mitigation: normalization map with doctor drift checks.

- Status drift across planning and execution artifacts.
  Mitigation: cross-surface reconciliation checks in doctor.

- Retry side effects under transient failure.
  Mitigation: idempotency token store with response replay.

- Correction signal noise from multi-source reporting.
  Mitigation: fingerprint-based event deduplication.

## 9. Delivery Sequence

1. Implement command spine and inline ingestion.
2. Persist authoritative stable-ID landscape artifacts.
3. Rebuild derived graph eagerly after each successful write.
4. Rank and confirm one feature for packet creation.
5. Emit packet, doctor JSONL report, and evidence bundle.
6. Route corrections through Salmon with deduplication.

## 10. Expected Next Action

After this brief, proceed to businessplan with this product intent as the baseline for PRD and UX elaboration, while preserving the constrained v1 execution boundary established here.
