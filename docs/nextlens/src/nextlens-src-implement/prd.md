---
feature: nextlens-src-implement
doc_type: prd
status: draft
goal: "Deliver a deterministic v1 NextLens path from ambiguous context to one implementation-ready feature packet with auditable evidence and safe correction loops."
key_decisions:
  - Ship a single command spine for v1 that ingests context inline and selects exactly one feature packet.
  - Keep stable-ID landscape artifacts authoritative and rebuild derived graph projections eagerly after successful writes.
  - Enforce idempotent mutating operations and non-mutating JSONL doctor checks for validation.
  - Route corrections through deduplicated Salmon events with strict control/target write boundaries.
open_questions:
  - Minimum required packet metadata and acceptance schema for downstream BMAD consumers.
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

This PRD defines the first implementation slice for NextLens: a governed, deterministic flow that converts ambiguous product context into one selected, implementation-ready feature packet. The v1 scope is intentionally narrow and optimized for reliability, auditability, and correction safety.

The solution must preserve Lens lifecycle boundaries while proving end-to-end operability. In one run, operators should be able to ingest context, select one packet, validate outcomes via doctor checks, and route corrections through a deduplicated signaling path.

## Problem Statement

Current planning assets describe desired behavior but do not provide a constrained operational path that consistently produces one validated packet from uncertain input. This creates repeat interpretation work, drift between authoritative and derived artifacts, and inconsistent correction handling.

Without a deterministic first path, planning outputs remain informative but not reliably executable.

## Users and Stakeholders

- Lens feature operators who need a repeatable command path from context to a single packet.
- BMAD downstream authors who need bounded, implementation-ready input.
- Governance and review stakeholders who require traceable evidence and strict write-boundary guarantees.

## Product Goals

1. Deterministic first path: one command flow from ambiguous context to one selected packet.
2. Authoritative state discipline: stable-ID landscape is source-of-truth; graph is derived projection.
3. Retry safety and validation clarity: mutating ops are idempotent; doctor checks are machine-readable and non-mutating.
4. Correction integrity: multi-source corrections route through deduplicated Salmon signaling.
5. Boundary safety: no direct governance or release writes from implementation flow.

## Non-Goals

- Multi-packet generation in v1.
- Full workflow automation beyond minimum proof path.
- Rewiring lifecycle phase semantics outside current planning completion boundaries.
- Replacing existing governance publish contracts.

## Functional Requirements

1. The system shall provide one interactive command flow that ingests context and selects exactly one packet.
2. The system shall require explicit confirmation before final packet emission.
3. The system shall persist authoritative landscape state with stable IDs.
4. The system shall rebuild derived graph projection deterministically after successful authoritative writes.
5. The system shall expose doctor checks as JSONL output suitable for CI and tooling.
6. The system shall ensure doctor checks are non-mutating.
7. The system shall enforce idempotency for mutating operations, including replay-safe behavior for duplicate requests.
8. The system shall route correction signals from human, doctor, and review inputs through a deduplicated Salmon event path.
9. The system shall produce an evidence bundle describing selected packet, validation output, and correction routing decisions.

## Non-Functional Requirements

1. Determinism: identical inputs and state produce identical packet selection.
2. Traceability: each run emits machine-consumable evidence artifacts.
3. Safety: implementation flow must not write directly to governance or release clones.
4. Reliability: duplicate submission retries must not create duplicate side effects.
5. Maintainability: authoritative and derived artifacts remain clearly separated.

## Success Criteria

1. A single run produces one selected packet and one evidence bundle.
2. Doctor output is valid JSONL and remains non-mutating across runs.
3. Duplicate requests replay without duplicate writes.
4. Derived graph remains consistent with latest authoritative writes.
5. At least one correction route is observed end-to-end through deduplicated Salmon signaling.
6. No direct governance or release writes occur during execution.

## Constraints

- Writes are limited to approved control docs and implementation target paths.
- Governance publication remains phase-owned and outside this implementation flow.
- Scope remains v1-minimal and must prioritize deterministic behavior over breadth.

## Risks and Mitigations

- Vocabulary drift across feature/slice terms.
  - Mitigation: normalization mapping and doctor drift checks.
- Status drift across planning/execution artifacts.
  - Mitigation: cross-surface reconciliation checks.
- Retry side effects under transient failures.
  - Mitigation: idempotency token store and replay semantics.
- Multi-source correction noise.
  - Mitigation: fingerprint-based deduplication and deterministic routing classes.

## Out of Scope Follow-Ups

- Multi-feature packet generation.
- Historical semantic-ID migration tooling.
- Extended automation packs outside first selected packet flow.