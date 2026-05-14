---
feature: nextlens-src-implement
doc_type: prd
status: draft
goal: "Deliver a deterministic v1 NextLens top-down bridge from discovered system context to one implementation-ready Feature packet with auditable evidence and safe correction loops."
key_decisions:
  - Ship a single command spine for v1 that consumes top-down discovery output and selects exactly one Feature packet.
  - Keep stable-ID landscape artifacts authoritative and rebuild derived graph projections eagerly after successful writes.
  - Enforce idempotent mutating operations and non-mutating JSONL doctor checks for validation.
  - Route corrections through deduplicated Salmon events with strict control/target write boundaries.
open_questions:
  - Exact BMAD consumer hint fields required for downstream planning artifacts.
  - Escalation thresholds for automatic versus manual correction routing.
depends_on:
  - product-brief
  - brainstorm
  - research
blocks: []
updated_at: 2026-05-14T00:00:00Z
---

# Product Requirements Document - nextlens-src-implement

## Executive Summary

This PRD defines the first implementation Feature for NextLens: a governed, deterministic top-down bridge that converts discovered LENS system context into one selected, implementation-ready Feature packet. The v1 scope is intentionally narrow and optimized for reliability, auditability, and correction safety.

The solution must preserve Lens lifecycle boundaries while proving end-to-end operability. In one run, operators should be able to consume discovery output, check context sufficiency, rank candidate Features, select one packet, validate outcomes via doctor checks, and route corrections through a deduplicated signaling path.

## Problem Statement

Current planning assets describe desired behavior but do not provide a constrained operational path that consistently produces one validated packet from top-down discovery context. This creates repeat interpretation work, drift between authoritative and derived artifacts, and inconsistent correction handling.

Without a deterministic first path, planning outputs remain informative but not reliably executable.

## Users and Stakeholders

- Lens Feature operators who need a repeatable command path from discovered system context to a single packet.
- BMAD downstream authors who need bounded, implementation-ready input.
- Governance and review stakeholders who require traceable evidence and strict write-boundary guarantees.

## Product Goals

1. Deterministic top-down bridge: one command flow from discovered system context to one selected Feature packet.
2. Authoritative state discipline: stable-ID landscape is source-of-truth; graph is derived projection.
3. Retry safety and validation clarity: mutating ops are idempotent; doctor checks are machine-readable and non-mutating.
4. Correction integrity: multi-source corrections route through deduplicated Salmon signaling.
5. Boundary safety: no direct governance or release writes from implementation flow.

## Non-Goals

- Multi-packet generation in v1.
- Full discovery epoch implementation.
- Multi-Feature orchestration.
- Bottom-up standalone Feature mode.
- Automatic capability/domain promotion.
- Auspex dashboard implementation.
- Full landscape authoring UI.
- Full workflow automation beyond minimum proof path.
- Rewiring lifecycle phase semantics outside current planning completion boundaries.
- Replacing existing governance publish contracts.
- Replacing BMAD PRD, UX, architecture, epics, stories, implementation, code review, or correct-course.

## Top-Down Context Contract

NextLens v1 consumes, but does not create, the full discovery epoch. When upstream discovery output is available, the command expects the following shape:

```yaml
top_down_context:
  schemaVersion: lens.topdown-context.v1
  system:
    id:
    name:
    thesis:
    status:
    confidence:
  discoveryEpoch:
    id:
    status:
    sourceRefs: []
  roles: []
  stakeholders: []
  outcomes: []
  operatingLoops: []
  journeys: []
  candidateFeatures: []
  openQuestions: []
  risks: []
  decisions: []
  relationshipRefs: []
```

If upstream context is incomplete, the command must block, warn, or request confirmation before emitting a Feature packet. It must not invent missing discovery context or silently downgrade into bottom-up mode.

## Context Sufficiency Gate

Before ranking candidate Features, NextLens evaluates context sufficiency:

```yaml
context_sufficiency:
  status: ready | ready_with_warnings | blocked
  missingRequired:
    - ...
  warnings:
    - ...
  recommendation:
    - continue
    - ask_for_confirmation
    - return_to_discovery
```

The gate checks that the system thesis is present, at least one role is present, at least one outcome is present, at least one journey or journey hypothesis is present, candidate Features trace to a journey or outcome, risks and open questions are captured, and BMAD consumer context is available.

## Functional Requirements

1. The system shall provide one interactive command flow that consumes top-down context and selects exactly one Feature packet.
2. The system shall require explicit confirmation before final packet emission.
3. The system shall run a context sufficiency gate before ranking candidate Features.
4. The system shall rank candidate Features by their ability to prove part of the discovered system.
5. The system shall persist authoritative landscape state with stable IDs.
6. The system shall rebuild derived graph projection deterministically after successful authoritative writes.
7. The system shall expose doctor checks as JSONL output suitable for CI and tooling.
8. The system shall ensure doctor checks are non-mutating.
9. The system shall enforce idempotency for mutating operations, including replay-safe behavior for duplicate requests.
10. The system shall route correction signals from human, doctor, review, and implementation inputs through a deduplicated Salmon event path.
11. The system shall produce an evidence bundle describing selected packet, validation output, and correction routing decisions.

## Non-Functional Requirements

1. Determinism: identical inputs and state produce identical packet selection.
2. Traceability: each run emits machine-consumable evidence artifacts that preserve system -> Role -> Outcome -> Journey -> Feature links.
3. Safety: implementation flow must not write directly to governance or release clones.
4. Reliability: duplicate submission retries must not create duplicate side effects.
5. Maintainability: authoritative and derived artifacts remain clearly separated.

## Success Criteria

1. A single run produces one selected packet and one evidence bundle.
2. Complete top-down context ranks candidates, requires confirmation, emits exactly one Feature packet, and preserves traceability to system -> Role -> Outcome -> Journey -> Feature.
3. Ambiguous context with no outcome or journey does not emit a Feature packet; it produces a context sufficiency report and recommends returning to discovery.
4. Candidate Feature scope that includes adjacent journeys or future Features is flagged by Doctor, and packet emission is blocked or marked ready_with_warnings according to severity.
5. When implementation reveals a journey assumption is wrong, Salmon routes the signal to impacted Feature, Journey, Outcome, and BMAD correct-course recommendation.
6. Doctor output is valid JSONL and remains non-mutating across runs.
7. Duplicate requests replay without duplicate writes.
8. Derived graph remains consistent with latest authoritative writes.
9. At least one correction route is observed end-to-end through deduplicated Salmon signaling.
10. No direct governance or release writes occur during execution.

## Constraints

- Writes are limited to approved control docs and implementation target paths.
- Governance publication remains phase-owned and outside this implementation flow.
- Scope remains v1-minimal and must prioritize deterministic behavior over breadth.
- The selected Feature packet must contain enough top-down context for BMAD to understand why the Feature matters, but it must constrain BMAD to the selected Feature scope only. BMAD must not expand into adjacent journeys, future Features, full platform architecture, or unrelated outcomes unless Salmon or correct-course explicitly changes scope.

## Risks and Mitigations

- Vocabulary drift across Feature and legacy slice terms.
  - Mitigation: Feature is the official operational unit; only reference "slice" when describing legacy discussion.
- Status drift across planning/execution artifacts.
  - Mitigation: cross-surface reconciliation checks.
- Retry side effects under transient failures.
  - Mitigation: idempotency token store and replay semantics.
- Multi-source correction noise.
  - Mitigation: fingerprint-based deduplication and deterministic routing classes.
- BMAD over-expands from top-down context.
  - Mitigation: packet scope constraints and Doctor checks separate explanatory context from selected Feature scope.

## Out of Scope Follow-Ups

- Multi-Feature packet generation.
- Historical semantic-ID migration tooling.
- Extended automation packs outside first selected packet flow.
