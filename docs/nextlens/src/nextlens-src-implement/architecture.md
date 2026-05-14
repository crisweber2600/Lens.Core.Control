---
feature: nextlens-src-implement
doc_type: architecture
status: draft
goal: "Define a deterministic, idempotent architecture for NextLens v1 that emits one implementation-ready packet with verifiable evidence and correction routing."
key_decisions:
  - Keep a single command spine with strict stage boundaries and deterministic transitions.
  - Treat stable-ID landscape state as authoritative and derived graph state as rebuildable projection.
  - Enforce idempotency for mutating operations with replay-safe request handling.
  - Emit doctor checks as non-mutating JSONL and keep correction routing as a separate mutating stage.
  - Use a canonical packet schema with explicit required fields and version marker.
open_questions:
  - Exact numeric threshold values for candidate confidence bands.
  - Long-term schema evolution policy for packet version upgrades.
depends_on:
  - prd.md
  - ux-design.md
  - businessplan-adversarial-review.md
blocks: []
updated_at: 2026-05-14T00:00:00Z
---

# Architecture Decision Document - nextlens-src-implement

## 1. Context and Scope

NextLens v1 must convert ambiguous context into one selected feature packet through a deterministic command flow. The architecture must satisfy three hard constraints:

1. deterministic one-packet output,
2. replay-safe mutation behavior,
3. auditable evidence and correction routing.

The scope is limited to the first implementation slice and excludes multi-packet orchestration.

## 2. System Model

### 2.1 Logical Components

1. Context Intake Engine
2. Candidate Ranker
3. Selection Gate
4. Authoritative State Writer
5. Derived Projection Builder
6. Packet Emitter
7. Doctor Validator
8. Correction Router

### 2.2 Stage Pipeline

1. intake -> 2. rank -> 3. confirm -> 4. write authoritative -> 5. rebuild projection -> 6. emit packet -> 7. validate -> 8. route corrections

The pipeline is linear in v1 and emits exactly one packet on success.

## 3. Data Contracts

### 3.1 Canonical Packet Schema (v1)

Required fields:

- `schemaVersion`
- `packetId`
- `featureId`
- `selectionRationale`
- `sourceContextRefs`
- `authoritativeStateRef`
- `doctorSummary`
- `correctionPlan`
- `createdAt`

Optional fields:

- `alternates`
- `confidence`
- `notes`

Validation rules:

1. `schemaVersion` must be `nextlens.packet.v1`.
2. `packetId` must be unique per run.
3. `featureId` must resolve to one selected candidate only.
4. `selectionRationale` must include deterministic tie-break evidence when needed.

### 3.2 Authoritative vs Derived

- Authoritative artifacts are written once per successful mutation stage.
- Derived graph projection is always regenerated from authoritative data.
- No downstream stage may mutate authoritative state except the designated write stage.

## 4. Determinism and Idempotency

### 4.1 Deterministic Ranking

Ranking input order, scoring factors, and tie-break sequence are fixed. Tie-break sequence:

1. highest normalized score,
2. fewer unresolved blockers,
3. earliest stable creation timestamp,
4. lexical feature ID fallback.

### 4.2 Idempotent Mutation Contract

- Mutating requests require an idempotency token.
- Token replay returns the original result envelope, not a second write.
- Evidence bundle records replay outcome.

## 5. Validation and Correction Architecture

### 5.1 Doctor Output

- Doctor is non-mutating and emits JSONL.
- Severity classes: blocking, advisory, informational.
- Blocking findings stop packet emission if detected before confirmation.

### 5.2 Correction Routing

- Findings are mapped into correction classes.
- Events are deduplicated by deterministic fingerprint.
- Routing result must report one of: created, merged, duplicate-ignored.

## 6. Failure Handling

1. If ranking fails, stop before selection gate.
2. If confirmation is declined, no writes occur.
3. If authoritative write fails, projection rebuild and packet emission are skipped.
4. If projection rebuild fails, packet emission is blocked and rollback guidance is returned.
5. If correction routing fails, packet remains emitted but status is warning with actionable follow-up.

## 7. Observability and Evidence

Each run produces:

1. packet artifact,
2. doctor JSONL report,
3. evidence bundle containing stage outcomes, idempotency decision, and correction routing summary.

## 8. Security and Boundary Controls

- No direct governance or release-clone writes in the execution flow.
- Writes are restricted to approved control docs and implementation target paths.
- Evidence references are immutable once emitted.

## 9. Acceptance Mapping

1. Exactly one packet emitted per successful run.
2. Replay of identical idempotency token creates no duplicate side effects.
3. Doctor reports are JSONL and non-mutating.
4. Correction deduplication outcome is explicit and reproducible.
5. Derived projection is consistent with latest authoritative state.

## 10. Implementation Notes for Next Phase

TechPlan hardens the contracts needed for implementation start:

1. packet schema is now explicit,
2. deterministic tie-break policy is fixed,
3. idempotency and correction contracts are architecture-level requirements.