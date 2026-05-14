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
  - Package NextLens as a BMAD module with generated registration artifacts (`module.yaml`, `module-help.csv`) and manifest (`.claude-plugin/marketplace.json`).
  - Use Create Module (CM) and Validate Module (VM) as mandatory packaging and quality gates before distribution.
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

## 11. BMAD Module Alignment

This feature will ship as a BMAD module, so the architecture includes packaging, registration, and distribution constraints from the BMAD Builder reference at:

- https://bmad-builder-docs.bmad-method.org/llms-full.txt

### 11.1 Required Module Build Flow

For module readiness, the implementation must follow this sequence:

1. Plan module capability shape (IM)
2. Build skills/workflows (BA and BW)
3. Scaffold module package (CM)
4. Validate structure and quality (VM)

### 11.2 Packaging Strategy

Given this feature is expected to include multiple capabilities (selection pipeline, doctor validation, correction routing), use a multi-skill module layout by default.

If scope is reduced to one skill, standalone self-registering packaging is acceptable.

### 11.3 Mandatory Registration Artifacts

The packaged output must include:

1. `module.yaml` with module identity and config variables
2. `module-help.csv` with capability entries for `bmad-help` discoverability
3. `.claude-plugin/marketplace.json` manifest for installer/distribution compatibility

### 11.4 Repository and Manifest Constraints

Manifest and repository conventions are treated as architecture constraints:

1. Module name must be lowercase and hyphenated
2. Manifest `plugins[].skills` paths must match actual repository paths
3. Manifest versioning must be semantic (`major.minor.patch`) and bumped on each release
4. Single-module and multi-module repository layouts are both valid, but path consistency is mandatory

### 11.5 Module Quality Gates

Before any release or distribution:

1. Run CM to regenerate module registration files from current skills
2. Run VM and resolve structural and quality findings
3. Verify manifest integrity and skill path accuracy
4. Verify module help ordering and command discoverability in `bmad-help`

### 11.6 Operational Implications for NextLens

This module-oriented architecture adds non-functional obligations:

1. Discoverability: capabilities must be registered and orderable through module help metadata
2. Installability: module must install from a Git source through BMAD custom-source support
3. Maintainability: future capability additions must re-run CM/VM rather than hand-editing registration files
4. Portability: packaging must remain host-agnostic (GitHub, GitLab, Bitbucket, local path)

## 12. Module-Oriented Implementation Additions

To support BMAD module delivery, the next implementation slice must add these work packages:

1. Define module identity and configuration variables in `module.yaml`
2. Define capability catalog and ordering entries in `module-help.csv`
3. Add/verify setup skill behavior for multi-skill registration (or self-registration path for standalone)
4. Create and validate `.claude-plugin/marketplace.json`
5. Add packaging validation checks to CI or release checklist (CM and VM gates)