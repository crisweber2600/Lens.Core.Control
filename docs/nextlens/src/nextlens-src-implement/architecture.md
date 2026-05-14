---
feature: nextlens-src-implement
doc_type: architecture
status: draft
goal: "Define a deterministic, idempotent architecture for NextLens v1 that emits one top-down implementation-ready Feature packet with verifiable evidence and correction routing."
key_decisions:
  - Keep a single command spine with strict stage boundaries and deterministic transitions.
  - Treat stable-ID landscape state as authoritative and derived graph state as rebuildable projection.
  - Enforce idempotency for mutating operations with replay-safe request handling.
  - Emit doctor checks as non-mutating JSONL and keep correction routing as a separate mutating stage.
  - Use a canonical top-down Feature packet schema with explicit required fields and version marker.
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

NextLens v1 must convert discovered top-down LENS context into one selected Feature packet through a deterministic command flow. The architecture must satisfy three hard constraints:

1. deterministic one-packet output,
2. replay-safe mutation behavior,
3. auditable evidence and correction routing.

The scope is limited to the first selected Feature packet and excludes full discovery implementation, bottom-up mode, and multi-Feature orchestration.

## 2. System Model

### 2.1 Logical Components

1. Context Intake Engine
2. Context Sufficiency Gate
3. Candidate Ranker
4. Selection Gate
5. Authoritative State Writer
6. Derived Projection Builder
7. Packet Emitter
8. Doctor Validator
9. Correction Router

### 2.2 Stage Pipeline

1. intake -> 2. context sufficiency -> 3. rank -> 4. confirm -> 5. write authoritative -> 6. rebuild projection -> 7. emit packet -> 8. validate -> 9. route corrections

The pipeline is linear in v1 and emits exactly one packet on success.

### 2.3 Top-Down Bridge Boundary

LENS owns top-down context, traceability, Feature selection, Doctor validation, and Salmon correction routing.

BMAD owns product brief, PRD, UX, architecture, epics, stories, implementation, code review, and correct-course.

NextLens v1 emits the Feature packet that bridges those two systems. The packet carries enough top-down context for BMAD to understand why the selected Feature matters, but it constrains BMAD to the selected Feature scope only. BMAD must not expand into adjacent journeys, future Features, full platform architecture, or unrelated outcomes unless Salmon or correct-course explicitly changes scope.

### 2.4 Feature Selection Does Not Create Full Architecture

The selected Feature packet must not cause BMAD to plan the whole system. It may include top-down traceability and rationale, but the selected Feature scope and explicit out-of-scope list define the planning boundary.

## 3. Data Contracts

### 3.1 Top-Down Context Contract

NextLens v1 consumes the upstream discovery output when available; it does not create the full discovery epoch. Expected input shape:

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

If upstream context is incomplete, the command must either block, warn, or request confirmation before emitting a Feature packet.

### 3.2 Context Sufficiency Gate

Before ranking, the gate validates:

1. system thesis present,
2. at least one role present,
3. at least one outcome present,
4. at least one journey or journey hypothesis present,
5. candidate Features trace to journey or outcome,
6. risks and open questions captured,
7. BMAD consumer context available.

Output contract:

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

### 3.3 Canonical Feature Packet Schema (v1)

Required fields:

```yaml
feature_packet:
  schemaVersion: nextlens.feature-packet.v1
  packetId:
  featureId:
  sourceMode: top_down
  selectedFeature:
    id:
    name:
    goal:
    includedScope: []
    explicitOutOfScope: []
  trace:
    systemId:
    discoveryEpochId:
    roleIds: []
    outcomeIds: []
    operatingLoopIds: []
    journeyIds: []
    relationshipRefs: []
  selectionRationale:
    score:
    tieBreakEvidence:
    whyThisFeature:
    whyNow:
    rejectedAlternates: []
  sourceContextRefs: []
  authoritativeStateRef:
  derivedGraphRef:
  doctorSummary:
  salmonRoutingSummary:
  bmadConsumerHints:
    prdInput:
    uxInput:
    architectureInput:
    epicStoryInput:
    readinessInput:
  evidenceBundleRef:
  createdAt:
```

Validation rules:

1. `schemaVersion` must be `nextlens.feature-packet.v1`.
2. `packetId` must be unique per run.
3. `featureId` must resolve to one selected candidate only.
4. `sourceMode` must be `top_down`.
5. `selectionRationale` must include deterministic tie-break evidence when needed.
6. `trace` must preserve system, role, outcome, journey, and Feature lineage when context is ready.
7. `selectedFeature.explicitOutOfScope` must constrain adjacent journeys, future Features, full platform architecture, and unrelated outcomes.

### 3.4 Authoritative vs Derived

- Work Archive stores discovery sessions, extraction outputs, Feature packet runs, Doctor reports, evidence bundles, and Salmon signals.
- Living Landscape stores current system thesis, role map, outcome map, journey ledgers, Feature ledgers, decisions, and risks.
- Derived Graph rebuilds machine-readable relationships from Work Archive and Living Landscape. It is never authoritative.
- Authoritative artifacts are written once per successful mutation stage.
- Derived graph projection is always regenerated from authoritative data.
- No downstream stage may mutate authoritative state except the designated write stage.

## 4. Determinism and Idempotency

### 4.1 Deterministic Top-Down Ranking

Ranking does not rank arbitrary Features. It ranks candidate Features by their ability to prove part of the discovered system.

Scoring factors:

1. outcome alignment,
2. journey criticality,
3. role value,
4. risk reduction,
5. dependency readiness,
6. implementation boundedness,
7. BMAD readiness,
8. evidence clarity,
9. open-question severity.

Ranking input order, scoring factors, and tie-break sequence are fixed. Tie-break sequence:

1. highest outcome alignment,
2. highest journey criticality,
3. fewest unresolved blockers,
4. clearest acceptance evidence,
5. earliest stable candidate timestamp,
6. lexical Feature ID fallback.

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

Salmon impact levels:

```yaml
salmon_impact_levels:
  - local_feature_note
  - feature_scope_change
  - journey_assumption_change
  - outcome_reframe
  - role_or_stakeholder_change
  - operating_loop_change
  - capability_or_landscape_update
  - bmad_correct_course_required
```

Salmon signal shape:

```yaml
salmon_signal:
  id:
  raisedFrom:
  source:
    type: human | doctor | review | implementation
  discovery:
  impactedNodes:
    features: []
    journeys: []
    outcomes: []
    roles: []
    operatingLoops: []
    capabilities: []
    bmadArtifacts: []
  severity:
  recommendedAction:
    type: local_note | landscape_update | block_packet | correct_course
  dedupFingerprint:
  routingResult:
    status: created | merged | duplicate_ignored
```

## 6. Failure Handling

1. If ranking fails, stop before selection gate.
2. If context sufficiency is blocked, stop before ranking and recommend return to discovery.
3. If confirmation is declined, no writes occur.
4. If authoritative write fails, projection rebuild and packet emission are skipped.
5. If projection rebuild fails, packet emission is blocked and rollback guidance is returned.
6. If correction routing fails, packet remains emitted but status is warning with actionable follow-up.

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
2. Complete top-down context ranks candidate Features, requires confirmation, emits exactly one Feature packet, and preserves traceability to system -> Role -> Outcome -> Journey -> Feature.
3. Insufficient context with no outcome or journey does not emit a Feature packet; it produces a context sufficiency report and recommends return to discovery.
4. Candidate Feature scope that includes adjacent journeys or future Features is flagged by Doctor, and packet emission is blocked or marked ready_with_warnings according to severity.
5. Implementation-discovered journey assumption errors route through Salmon to impacted Feature, Journey, Outcome, and BMAD correct-course recommendation.
6. Replay of identical idempotency token creates no duplicate side effects.
7. Doctor reports are JSONL and non-mutating.
8. Correction deduplication outcome is explicit and reproducible.
9. Derived projection is consistent with latest authoritative state.

## 10. Implementation Notes for Next Phase

TechPlan hardens the contracts needed for implementation start:

1. top-down context and packet schemas are now explicit,
2. deterministic top-down ranking and tie-break policy are fixed,
3. idempotency and correction contracts are architecture-level requirements,
4. BMAD scope containment is required for every emitted packet.

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

To support BMAD module delivery, the next selected Feature must add these work packages:

1. Define module identity and configuration variables in `module.yaml`
2. Define capability catalog and ordering entries in `module-help.csv`
3. Add/verify setup skill behavior for multi-skill registration (or self-registration path for standalone)
4. Create and validate `.claude-plugin/marketplace.json`
5. Add packaging validation checks to CI or release checklist (CM and VM gates)
