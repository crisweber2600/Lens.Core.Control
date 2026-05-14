---
feature: nextlens-src-implement
doc_type: epics
status: approved
stepsCompleted: [validate-prerequisites, design-epics, map-coverage]
inputDocuments:
  - product-brief.md
  - prd.md
  - architecture.md
  - ux-design.md
  - research.md
  - brainstorm.md
updated_at: 2026-05-14T00:00:00Z
---

# NextLens v1 - Epic Breakdown

## Overview

This document provides the complete epic and story breakdown for NextLens v1, decomposing the top-down Feature packet bridge requirements into implementable stories. The solution delivers a deterministic command-driven path from discovered system context to one selected Feature packet with auditable evidence and correction routing.

## Requirements Inventory

### Functional Requirements (FRs)

- **FR1:** The system shall provide one interactive command flow that consumes top-down context and selects exactly one Feature packet.
- **FR2:** The system shall require explicit confirmation before final packet emission.
- **FR3:** The system shall run a context sufficiency gate before ranking candidate Features.
- **FR4:** The system shall rank candidate Features by their ability to prove part of the discovered system using deterministic scoring factors (outcome alignment, journey criticality, role value, risk reduction, dependency readiness, implementation boundedness, BMAD readiness, evidence clarity, open-question severity).
- **FR5:** The system shall persist authoritative landscape state with stable IDs (system, roles, outcomes, journeys, operating loops, capabilities, decisions, risks).
- **FR6:** The system shall rebuild derived graph projection deterministically after successful authoritative writes.
- **FR7:** The system shall expose doctor checks as JSONL output suitable for CI and tooling.
- **FR8:** The system shall ensure doctor checks are non-mutating read-only operations.
- **FR9:** The system shall enforce idempotency for mutating operations, including replay-safe behavior for duplicate requests via idempotency tokens.
- **FR10:** The system shall route correction signals from human, doctor, review, and implementation inputs through a deduplicated Salmon event path using fingerprint-based deduplication.
- **FR11:** The system shall produce an evidence bundle describing selected packet, validation output, and correction routing decisions.
- **FR12:** The system shall validate and emit Feature packet conforming to nextlens.feature-packet.v1 schema with required fields (schemaVersion, packetId, featureId, sourceMode, selectedFeature, trace, selectionRationale, doctorSummary, salmonRoutingSummary, bmadConsumerHints).
- **FR13:** The system shall block packet emission when context sufficiency check returns blocked status; recommend returning to discovery.
- **FR14:** The system shall block packet emission when Doctor finds blocking-severity findings before confirmation gate.
- **FR15:** The system shall mark packet as ready_with_warnings when Doctor finds advisory findings and require explicit operator confirmation.
- **FR16:** The system shall provide stage-based interaction model with clear status headers [stage:name], pass/warning/fail outcome signals, and blocking-reason display.
- **FR17:** The system shall preserve strict write boundaries enforcing zero direct governance or release mutations from implementation flow.

### Non-Functional Requirements (NFRs)

- **NFR1:** Determinism: identical inputs and state produce identical packet selection.
- **NFR2:** Traceability: each run emits machine-consumable evidence artifacts preserving system -> Role -> Outcome -> Journey -> Feature lineage.
- **NFR3:** Safety: implementation flow must not write directly to governance or release clones; writes restricted to approved control docs and implementation target paths.
- **NFR4:** Reliability: duplicate submission retries must not create duplicate side effects; idempotency token store required.
- **NFR5:** Maintainability: authoritative (Work Archive, Living Landscape, packet selection state) and derived artifacts (Derived Graph) remain clearly separated.
- **NFR6:** Replay Safety: identical idempotency token replays return original response envelope without creating duplicate writes.
- **NFR7:** Accessibility: consistent command vocabulary across all stages; Feature used as official operational unit; legacy "slice" terminology referenced only when necessary.
- **NFR8:** Consistency: derived projection must always be consistent with latest authoritative writes; eager rebuild required after successful mutations.
- **NFR9:** Machine Readability: doctor JSONL output with stable check IDs, severity levels, target paths, remediation hints suitable for shell tools and test harnesses.
- **NFR10:** Module Compliance: must ship as BMAD module with module.yaml, module-help.csv, and .claude-plugin/marketplace.json registration artifacts.

### Architecture Design Requirements

- **AR1:** Command spine design: single interactive wizard entry point with deterministic stage pipeline (intake → sufficiency → rank → confirm → write → rebuild → emit → validate → route).
- **AR2:** State model: Work Archive stores discovery sessions and Feature packet runs; Living Landscape stores current system state with stable IDs; Derived Graph is query projection rebuilt eagerly after writes.
- **AR3:** Top-down context contract: schemaVersion: lens.topdown-context.v1 with required fields (system, discoveryEpoch, roles, stakeholders, outcomes, operatingLoops, journeys, candidateFeatures, openQuestions, risks, decisions, relationshipRefs).
- **AR4:** Idempotency model: mutating requests require idempotency token (UUID/KSUID); token store persists (status, result, replay); duplicate token returns original envelope.
- **AR5:** Deterministic ranking: fixed tie-break sequence (highest outcome alignment → highest journey criticality → fewest blockers → clearest evidence → earliest timestamp → lexical fallback).
- **AR6:** Salmon correction model: eight impact levels (local_feature_note, feature_scope_change, journey_assumption_change, outcome_reframe, role_or_stakeholder_change, operating_loop_change, capability_or_landscape_update, bmad_correct_course_required) with multi-source evidence deduplication by fingerprint.
- **AR7:** Doctor severity model: three levels (blocking, advisory, informational) with non-mutating checks run pre-flight before all writes; blocking findings prevent packet emission.
- **AR8:** BMAD scoping: selected Feature packet constrains BMAD to selected Feature scope only; must not expand into adjacent journeys, future Features, full platform architecture, or unrelated outcomes without explicit Salmon scope-change signal.
- **AR9:** BMAD module packaging: must include module.yaml with identity and config, module-help.csv for discoverability, .claude-plugin/marketplace.json for installer compatibility; Create Module (CM) and Validate Module (VM) gates required before release.

### UX Design Requirements

- **UX-DR1:** Design stage-framing pattern: every stage starts with status header [stage:name], displays deterministic output, ends with pass/warning/fail, prints blocking reasons and next-valid-action on fail.
- **UX-DR2:** Design context sufficiency presentation: display in fixed order (system thesis, role, outcome, journey/hypothesis, candidate Feature trace, risks/open questions, BMAD consumer context); if blocked status do not show confirmation, recommend return to discovery; if ready_with_warnings require explicit confirmation.
- **UX-DR3:** Design candidate presentation: show exactly one selected candidate and up to two alternates; display score rationale fields in fixed order; explain that ranking is by ability to prove discovered system, not arbitrary backlog.
- **UX-DR4:** Design confirmation gate: require explicit operator confirmation before packet emission; provide safe cancel path with no writes; preserve diagnostic context for resume.
- **UX-DR5:** Design doctor results presentation: display concise severity table (blocking/advisory/informational); always provide JSONL path; distinguish non-mutating checks from mutating actions; flag scope spillage (adjacent journeys, future Features, platform architecture, unrelated outcomes).
- **UX-DR6:** Design correction routing: map each finding to one Salmon impact level with deterministic routing logic; deduplicate using fingerprint; surface whether correction was created/merged/ignored-as-duplicate.
- **UX-DR7:** Design success state: message confirms packet ID, evidence bundle path, doctor status, includes reminder that governance publish is phase-owned elsewhere.
- **UX-DR8:** Design partial success state: packet generated successfully but advisory findings exist; message includes recommended correction route and confidence level.
- **UX-DR9:** Design failure state: no packet emitted when blocking conditions fail before confirmation; show rollback or idempotency replay result on write attempt failure.
- **UX-DR10:** Design vocabulary consistency: use Feature as official public operational unit; only mention legacy "slice" terminology when referencing historical discussion; keep lines scannable, avoid long prose in live command output; ensure all critical results readable in plain text without color reliance.

### Requirements Coverage Map

| Epic | FR Coverage | NFR Coverage | AR Coverage | UX Coverage |
|------|-------------|--------------|-------------|-------------|
| EP1: Command Spine | FR1, FR16 | NFR7 | AR1 | UX-DR1, UX-DR10 |
| EP2: Context Sufficiency | FR3, FR13 | NFR2 | AR3 | UX-DR2 |
| EP3: Landscape State | FR5 | NFR3, NFR5 | AR2 | - |
| EP4: Feature Ranking | FR4 | NFR1 | AR5 | UX-DR3, UX-DR10 |
| EP5: Idempotency & Writes | FR9 | NFR4, NFR6 | AR4 | - |
| EP6: Graph Projection | FR6 | NFR5, NFR8 | AR2 | - |
| EP7: Doctor Validation | FR7, FR8, FR14, FR15 | NFR9 | AR7 | UX-DR5 |
| EP8: Packet Schema & Emission | FR12, FR2 | NFR2 | AR3, AR8 | UX-DR4, UX-DR7, UX-DR8, UX-DR9 |
| EP9: Salmon Correction | FR10 | NFR2 | AR6 | UX-DR6 |
| EP10: Evidence & Auditability | FR11 | NFR2, NFR3 | - | - |
| EP11: BMAD Module | FR17 | NFR10 | AR9 | - |

## Epic List

1. **EP1: Command Spine & Interactive Entry Point** - Implement single interactive wizard entry point with stage pipeline
2. **EP2: Context Sufficiency Gate** - Implement context validation and sufficiency evaluation before ranking
3. **EP3: Landscape State Management** - Implement authoritative state storage with stable IDs
4. **EP4: Feature Ranking & Selection** - Implement deterministic candidate Feature ranking logic
5. **EP5: Idempotent Writes & Replay Safety** - Implement idempotency tokens and replay-safe mutation operations
6. **EP6: Derived Graph Projection** - Implement eager graph rebuild on authoritative writes
7. **EP7: Doctor Checks & Validation** - Implement JSONL doctor reporting and pre-flight validation
8. **EP8: Feature Packet Schema & Emission** - Implement Feature packet generation and emission with confirmation gate
9. **EP9: Salmon Correction Routing** - Implement multi-source deduplication and deterministic correction routing
10. **EP10: Evidence Bundle & Traceability** - Implement evidence bundle generation and audit trail
11. **EP11: BMAD Module Packaging & Registration** - Implement BMAD module artifacts and packaging requirements

---

## EP1: Command Spine & Interactive Entry Point

**Goal:** Establish the main command entry point with stage-based wizard flow that guides operators deterministically from startup through context intake to packet selection.

**Scope:** Command-line interface, argument parsing, stage transition logic, status output formatting.

**Dependencies:** None (entry point)

**Blocks:** All other epics depend on working command spine.

### Story 1.1: Implement Command Entry Point and Argument Parser

As a **NextLens operator**, I want the system to **accept command-line arguments for mode (new, doctor, salmon), context input source, and optional config overrides**, so that **I can flexibly invoke different operational paths**.

**Acceptance Criteria:**

- **Given** the user runs `nextlens new --context path/to/context.yaml`
- **When** the command is parsed
- **Then** the mode is `new`, context source is resolved to `path/to/context.yaml`, and next stage begins

- **Given** the user runs `nextlens doctor --packet path/to/packet.json`
- **When** the command is parsed
- **Then** the mode is `doctor`, packet source is `path/to/packet.json`, doctor validation stage starts

- **Given** the user runs `nextlens salmon --findings path/to/findings.jsonl`
- **When** the command is parsed
- **Then** the mode is `salmon`, findings source is resolved, correction routing stage starts

- **Given** invalid or missing required arguments
- **When** the command is parsed
- **Then** usage/help message is printed with next valid action

### Story 1.2: Implement Stage Pipeline Orchestration

As a **NextLens system architect**, I want the command spine **to transition deterministically through stages (intake → sufficiency → rank → confirm → write → rebuild → emit → validate → route)**, so that **operators always know which stage they're in and what comes next**.

**Acceptance Criteria:**

- **Given** command enters `new` mode
- **When** stages execute in sequence
- **Then** each stage prints [stage:name] header, processes input, prints outcome (pass/warning/fail), transitions to next stage or blocks with reason

- **Given** a stage fails with blocking reason
- **When** the stage prints fail outcome
- **Then** the next valid action is displayed (return to discovery, fix input, contact support) and execution stops

- **Given** a stage completes successfully
- **When** the next stage is ready to start
- **Then** stage transition is logged in evidence bundle with timestamp

### Story 1.3: Implement Status Output Formatting and Progress Display

As a **NextLens operator**, I want the system to **display clear, scannable status messages at each stage without reliance on color or animation**, so that **I can follow progress in CI logs and terminal environments**.

**Acceptance Criteria:**

- **Given** each stage completes
- **When** status line is printed
- **Then** format is `[stage:stageName] status=pass|warning|fail; message; next_action=...` with line breaks for readability

- **Given** warning-level findings exist
- **When** status is printed
- **Then** warnings are listed with remediation hints, not hidden behind color or indentation only

- **Given** failure occurs before confirmation
- **When** status is printed
- **Then** rollback action (if any) is shown, and diagnostic context is preserved for resume

---

## EP2: Context Sufficiency Gate

**Goal:** Validate that top-down discovery output contains minimum required information before ranking proceeds; block packet emission if context is insufficient.

**Scope:** Sufficiency validation logic, context schema parsing, gate decision rules, reporting.

**Dependencies:** EP1 (command spine), input documents (top-down context)

**Blocks:** EP4 (ranking cannot proceed without sufficiency validation)

### Story 2.1: Implement Context Parsing and Schema Validation

As a **NextLens system**, I want to **parse and validate incoming top-down context against the lens.topdown-context.v1 schema**, so that **I can detect missing or malformed input early**.

**Acceptance Criteria:**

- **Given** top-down context is provided in YAML format
- **When** context is parsed
- **Then** required root fields are present (schemaVersion, system, discoveryEpoch, roles, outcomes, journeys, candidateFeatures)

- **Given** schemaVersion is not `lens.topdown-context.v1`
- **When** parsing completes
- **Then** a warning is logged and parsing continues with version-mismatch flag

- **Given** malformed YAML or missing required fields
- **When** parsing is attempted
- **Then** a detailed error message identifies the missing field and parsing stops

### Story 2.2: Implement Context Sufficiency Check Logic

As a **NextLens system**, I want to **evaluate context completeness against the seven mandatory gates**, so that **I prevent silent failures due to incomplete discovery**.

**Acceptance Criteria:**

- **Given** context sufficiency check runs
- **When** evaluating gates
- **Then** the system checks for:
  - system thesis is present and non-empty
  - at least one role is present
  - at least one outcome is present
  - at least one journey or journey hypothesis is present
  - candidate Features trace to a journey or outcome
  - risks and open questions are captured
  - BMAD consumer context is available (prdInput, uxInput, architectureInput hints)

- **Given** all seven gates pass
- **When** sufficiency status is set
- **Then** status is `ready`

- **Given** one or more gates fail
- **When** sufficiency status is set
- **Then** status is `blocked`, missingRequired array lists all failed gates, recommendation is `return_to_discovery`

- **Given** gates pass but advisory conditions exist (e.g., sparse open questions, ambiguous role hierarchy)
- **When** sufficiency status is set
- **Then** status is `ready_with_warnings`, warnings array lists conditions, recommendation is `ask_for_confirmation`

### Story 2.3: Implement Context Sufficiency Report Output

As a **NextLens operator**, I want the system to **display sufficiency status in a clear, actionable format**, so that **I can immediately understand what's missing and what to do next**.

**Acceptance Criteria:**

- **Given** sufficiency check completes
- **When** report is printed
- **Then** format is:
  ```
  [stage:context-sufficiency] 
  system_thesis: [✓|✗] [value or missing]
  role_coverage: [✓|✗] [count] roles
  outcome_coverage: [✓|✗] [count] outcomes
  journey_coverage: [✓|✗] [count] journeys
  candidate_traceability: [✓|✗] [count] candidates traceable
  risks_captured: [✓|✗] [count] risks
  bmad_hints: [✓|✗] [present/missing hints]
  
  status: ready|ready_with_warnings|blocked
  recommendation: continue|ask_for_confirmation|return_to_discovery
  ```

- **Given** status is `blocked`
- **When** report is printed
- **Then** no confirmation prompt is shown; next valid action is "return to discovery"

- **Given** status is `ready_with_warnings`
- **When** report is printed
- **Then** warnings are listed with context; operator is prompted for explicit confirmation

---

## EP3: Landscape State Management

**Goal:** Establish authoritative storage model for stable-ID landscape state (system, roles, outcomes, journeys, operating loops, capabilities, decisions, risks) with deterministic persistence and reconstruction guarantees.

**Scope:** State schema, persistence layer (file-based in control repo), stable ID generation/validation, state reconstruction logic.

**Dependencies:** Feature context (docs path), authorization (write boundary validation)

**Blocks:** EP6 (derived graph depends on authoritative state)

### Story 3.1: Implement Stable ID Generation and Validation

As a **NextLens system**, I want to **generate deterministic stable IDs for all landscape entities** and **validate that IDs are unique and immutable**, so that **I can reliably reference and trace entities across multiple runs**.

**Acceptance Criteria:**

- **Given** a new landscape entity (system, role, outcome, journey, operating loop) is created
- **When** ID generation runs
- **Then** a semantic ID is generated (human-visible, normalized from entity name) and an opaque ID is generated (immutable machine reference)

- **Given** an entity with stable ID already exists
- **When** reconstruction runs
- **Then** the same ID is recovered and used for all subsequent operations

- **Given** two entities with identical normalized names
- **When** ID generation runs
- **Then** both receive unique IDs (suffix-based disambiguation applied)

- **Given** ID immutability is violated (entity renamed, ID changed)
- **When** doctor check runs
- **Then** a warning is reported; evidence bundle records rename event

### Story 3.2: Implement Landscape State Persistence

As a **NextLens system**, I want to **persist landscape state in the control repo at {docs_path}/landscape/**, so that **state is version-controlled, auditable, and shareable**.

**Acceptance Criteria:**

- **Given** authoritative state is ready to write
- **When** write stage executes
- **Then** state is written to {docs_path}/landscape/{entity-type}/{stable-id}.yaml with:
  - entity identity (semantic ID, opaque ID, name)
  - state snapshot (role coverage, outcome statements, journey steps, operating loop cycles)
  - relationships (parent/child, cross-entity references)
  - metadata (createdAt, updatedAt, source, author)

- **Given** write completes successfully
- **When** file system is checked
- **Then** file is readable, parseable, and matches schema

- **Given** write fails (permission, disk space, etc.)
- **When** failure occurs
- **Then** error is logged, transaction is rolled back, status is fail, and packet emission is blocked

### Story 3.3: Implement State Reconstruction and Query

As a **NextLens system**, I want to **reconstruct landscape state deterministically from persisted files**, so that **replay runs produce identical results**.

**Acceptance Criteria:**

- **Given** existing landscape files at {docs_path}/landscape/
- **When** state reconstruction runs
- **Then** all entities are loaded in dependency order (systems → roles/outcomes → journeys/operating loops → capabilities)

- **Given** entity relationships are recorded in files
- **When** state is reconstructed
- **Then** all parent/child and cross-entity references are validated and resolved

- **Given** state is fully reconstructed
- **When** query runs for a specific entity
- **Then** the entity and all its relationships are returned without additional I/O

---

## EP4: Feature Ranking & Selection

**Goal:** Implement deterministic candidate Feature ranking using fixed scoring factors and tie-break logic; present ranked candidates to operator and require explicit selection confirmation.

**Scope:** Ranking algorithm, scoring factor calculation, tie-break sequence, candidate presentation, selection confirmation.

**Dependencies:** EP2 (context sufficiency must pass first), EP3 (landscape state provides entity context), FR4, AR5

**Blocks:** EP8 (packet emission depends on selection outcome)

### Story 4.1: Implement Deterministic Scoring Algorithm

As a **NextLens system**, I want to **score candidate Features using nine fixed factors in deterministic order**, so that **ranking is reproducible and explainable**.

**Acceptance Criteria:**

- **Given** candidate Features with context (outcomes, journeys, roles, risks, open questions)
- **When** scoring runs
- **Then** each candidate receives scores for:
  1. Outcome alignment (count of covered outcomes, weight by criticality)
  2. Journey criticality (count of journeys covered, weight by role dependency)
  3. Role value (count of roles impacted, weight by stakeholder importance)
  4. Risk reduction (count of mitigated risks, weight by severity)
  5. Dependency readiness (count of satisfied upstream dependencies)
  6. Implementation boundedness (scope clarity, absence of adjacent/future Feature spillage)
  7. BMAD readiness (availability of PRD, UX, architecture inputs for scope)
  8. Evidence clarity (traceability back to system thesis, completeness of rationale)
  9. Open-question severity (count and severity of unresolved questions)

- **Given** scoring completes
- **When** candidates are ranked
- **Then** highest composite score ranks first

### Story 4.2: Implement Tie-Break Sequence

As a **NextLens system**, I want to **apply deterministic tie-break logic when candidates have equal composite scores**, so that **tie-break is always the same given identical inputs**.

**Acceptance Criteria:**

- **Given** two candidates with equal composite score
- **When** tie-break runs
- **Then** tie-break sequence is applied in order:
  1. Highest outcome alignment
  2. Highest journey criticality
  3. Fewest unresolved blockers
  4. Clearest acceptance evidence
  5. Earliest stable candidate timestamp (first discovered)
  6. Lexical Feature ID fallback (A before Z)

- **Given** all tie-break factors are equal
- **When** final tie-break runs
- **Then** lexical ordering of Feature ID is applied

### Story 4.3: Implement Candidate Presentation and Selection

As a **NextLens operator**, I want to **see the top-ranked candidate with up to two alternatives displayed**, so that **I understand why the system selected the top candidate and can override if needed**.

**Acceptance Criteria:**

- **Given** candidates are ranked
- **When** presentation stage runs
- **Then** format is:
  ```
  [stage:candidate-selection]
  
  Selected Candidate (Rank 1):
  - Name: [feature name]
  - Goal: [feature goal]
  - Score: [composite score]
  - Rationale:
    - Outcome alignment: [value + top outcomes covered]
    - Journey criticality: [value + top journeys]
    - Role value: [impacted roles]
    - Risk reduction: [mitigated risks]
    - Evidence: [traceability summary]
  
  Alternative (Rank 2):
  - Name: [feature name]
  - Score: [score]
  - Why not selected: [score delta, missing coverage]
  
  Alternative (Rank 3):
  ...
  
  Confirm selection? [Y/n]
  ```

- **Given** operator reviews candidates
- **When** operator confirms (Y)
- **Then** selectedFeature is locked, proceed to confirmation gate

- **Given** operator declines (n)
- **When** operator is prompted
- **Then** offer options: (1) select different candidate, (2) cancel flow with no writes, (3) return to context with modifications

---

## EP5: Idempotent Writes & Replay Safety

**Goal:** Implement idempotency token store and replay semantics to ensure duplicate requests do not create duplicate side effects; support safe retry and recovery flows.

**Scope:** Idempotency token generation, token store persistence, request deduplication, response replay logic.

**Dependencies:** EP3 (landscape state writer depends on idempotency)

**Blocks:** Packet emission must succeed idempotency check

### Story 5.1: Implement Idempotency Token Generation and Storage

As a **NextLens system**, I want to **generate and store idempotency tokens for all mutating requests**, so that **retry attempts are safe and deterministic**.

**Acceptance Criteria:**

- **Given** a mutating operation (write landscape, emit packet, route correction) is about to start
- **When** token generation runs
- **Then** a unique idempotency token is created (UUID v4 or KSUID) with:
  - token value (40-char random)
  - operation type (write-landscape, emit-packet, route-correction)
  - timestamp (request issued at)
  - request digest (hash of operation parameters)

- **Given** token is generated
- **When** token storage runs
- **Then** token record is persisted at {docs_path}/.idempotency/tokens/{token-value}.yaml with:
  - status: pending | completed | failed
  - createdAt
  - completedAt (if applicable)
  - result (if completed)

- **Given** token is persisted
- **When** token lifecycle runs
- **Then** TTL is set to 24 hours; tokens older than TTL can be archived

### Story 5.2: Implement Request Deduplication

As a **NextLens system**, I want to **detect and deduplicate duplicate requests using idempotency tokens**, so that **reruns with the same token never create side effects**.

**Acceptance Criteria:**

- **Given** a request arrives with idempotency token
- **When** deduplication check runs
- **Then** system checks if token exists in store

- **Given** token does not exist
- **When** check completes
- **Then** operation is marked as "new" and proceeds to execution

- **Given** token exists with status=completed
- **When** check completes
- **Then** operation is skipped; original result is returned (replay)

- **Given** token exists with status=pending
- **When** check completes
- **Then** system waits for completion or returns error with retry-after suggestion

- **Given** token exists with status=failed
- **When** check completes
- **Then** system returns error and suggests contact support or manual recovery

### Story 5.3: Implement Response Replay Semantics

As a **NextLens system**, I want to **return original response envelopes for replayed requests**, so that **operators experience consistent results across retries**.

**Acceptance Criteria:**

- **Given** idempotency token is marked completed with result stored
- **When** duplicate request with same token arrives
- **Then** original response envelope is returned without re-executing operation

- **Given** response envelope includes packet reference, evidence bundle path, and doctor status
- **When** response is replayed
- **Then** evidence bundle records that response was replayed, not freshly generated

- **Given** operator runs same command twice with different output configuration
- **When** idempotency token is identical
- **Then** the same result is returned, not regenerated with new configuration

---

## EP6: Derived Graph Projection

**Goal:** Implement eager derived graph rebuild logic that reconstructs machine-readable relationship projections deterministically after each authoritative landscape write; ensure derived graph is always consistent with latest state.

**Scope:** Graph schema, eager rebuild logic, projection generation, consistency validation.

**Dependencies:** EP3 (landscape state), EP5 (idempotency for write operations)

**Blocks:** EP8 (packet emission depends on graph consistency check)

### Story 6.1: Implement Derived Graph Schema

As a **NextLens system**, I want to **define a canonical derived graph schema that captures all landscape entity relationships**, so that **graph queries are fast and consistent**.

**Acceptance Criteria:**

- **Given** landscape state is complete
- **When** graph schema is defined
- **Then** schema includes nodes for:
  - system (root)
  - roles (stakeholders, capabilities)
  - outcomes (desired states, metrics)
  - journeys (user stories, operating loops, scenarios)
  - operating loops (business cycles, feedback mechanisms)
  - capabilities (system features, platform services)
  - decisions (architecture, policy, design)
  - risks (known issues, mitigations)

- **Given** schema is defined
- **When** edges are defined
- **Then** edges capture:
  - system → role (system requires role participation)
  - role → outcome (role achieves outcome)
  - outcome → journey (outcome fulfilled by journey)
  - journey → operating loop (journey contains operating loop)
  - operating loop → capability (loop uses capability)
  - capability → decision (decision governs capability)
  - feature → journey/outcome (feature proves journey/outcome)

### Story 6.2: Implement Eager Graph Rebuild

As a **NextLens system**, I want to **rebuild derived graph immediately after every successful authoritative write**, so that **graph is always consistent with latest state**.

**Acceptance Criteria:**

- **Given** landscape write completes successfully
- **When** post-write stage runs
- **Then** graph rebuild is triggered automatically (not optional)

- **Given** graph rebuild runs
- **When** rebuilding edges
- **Then** all entity references are resolved, broken references are reported as warnings, orphaned nodes are flagged

- **Given** graph rebuild completes
- **When** output is written
- **Then** graph is persisted at {docs_path}/derived/graph.json with:
  - nodes array (id, type, label, metadata)
  - edges array (source, target, type, label)
  - metadata (generatedAt, sourceStateRef, consistency checksum)

- **Given** consistency checksum is generated
- **When** graph validation runs
- **Then** checksum is deterministic (same state → same checksum)

### Story 6.3: Implement Graph Consistency Validation

As a **NextLens system**, I want to **validate that derived graph is consistent with authoritative state before packet emission**, so that **packet never references stale relationships**.

**Acceptance Criteria:**

- **Given** packet emission is about to start
- **When** consistency check runs
- **Then** system verifies:
  - all entities in graph exist in landscape state
  - all edges reference valid nodes
  - consistency checksum matches current state
  - graph has no broken references or orphaned nodes

- **Given** consistency check passes
- **When** status is set
- **Then** packet emission proceeds

- **Given** consistency check finds inconsistencies
- **When** inconsistency is detected
- **Then** status is fail, error message identifies inconsistency, packet emission is blocked, landscape state is checked for corruption

---

## EP7: Doctor Checks & Validation

**Goal:** Implement comprehensive non-mutating doctor framework that validates pre-flight conditions, detects issues, and emits machine-readable JSONL reports; enforce pre-flight checks as mandatory gates before packet emission.

**Scope:** Check framework, check catalog, JSONL report generation, severity classification, pre-flight integration.

**Dependencies:** EP3 (landscape state), EP6 (derived graph)

**Blocks:** EP8 (packet emission depends on doctor validation)

### Story 7.1: Implement Doctor Check Framework

As a **NextLens system**, I want to **provide a pluggable framework for non-mutating doctor checks**, so that **I can add new checks without modifying core emission logic**.

**Acceptance Criteria:**

- **Given** doctor framework is initialized
- **When** framework starts
- **Then** framework provides:
  - registration method for new checks
  - execution method to run all registered checks
  - result collection and reporting
  - (no side effects, read-only)

- **Given** check is registered
- **When** check metadata is provided
- **Then** metadata includes:
  - check_id (stable, immutable identifier)
  - name (human-readable)
  - category (schema, scope, traceability, readiness)
  - severity (blocking, advisory, informational)
  - description
  - remediation hint

- **Given** check is executed
- **When** check runs
- **Then** check accepts (landscape state, derived graph, packet candidate, selected feature) as inputs and returns (status, message, references)

### Story 7.2: Implement Core Doctor Checks

As a **NextLens system**, I want to **provide essential doctor checks for common failure modes**, so that **operators can catch issues before packet emission**.

**Acceptance Criteria:**

- **Given** landscape state is available
- **When** check `schema-validity` runs
- **Then** check verifies:
  - all entity files match schema
  - required fields are present
  - no orphaned files exist
  - status: pass/warn/fail

- **Given** candidate Feature is selected
- **When** check `feature-scope` runs
- **Then** check verifies:
  - selectedFeature.includedScope does not contain adjacent journeys
  - selectedFeature.includedScope does not contain future Features
  - selectedFeature.includedScope does not contain unrelated platform architecture
  - selectedFeature.explicitOutOfScope is populated
  - severity: blocking if spillage detected

- **Given** Feature packet is prepared
- **When** check `traceability` runs
- **Then** check verifies:
  - packet.trace.systemId is valid
  - packet.trace.roleIds all resolve to landscape roles
  - packet.trace.outcomeIds all resolve to landscape outcomes
  - packet.trace.journeyIds all resolve to landscape journeys
  - packet.selectionRationale is non-empty
  - severity: advisory if any reference broken

- **Given** context sufficiency passed
- **When** check `context-readiness` runs
- **Then** check verifies:
  - BMAD consumer hints (prdInput, uxInput, architectureInput) are populated
  - top-down context includes system thesis, roles, outcomes, journeys
  - open questions and risks are captured
  - severity: advisory if missing hints

- **Given** all pre-conditions checked
- **When** check `write-boundary` runs
- **Then** check verifies:
  - write targets are within {docs_path}
  - write targets are not in governance repo
  - write targets are not in release clone paths
  - severity: blocking if boundary violated

- **Given** derived graph is available
- **When** check `graph-consistency` runs
- **Then** check verifies:
  - derived graph is consistent with landscape state
  - consistency checksum matches current state
  - no broken references or orphaned nodes
  - severity: advisory if inconsistency detected

### Story 7.3: Implement JSONL Doctor Report Generation

As a **NextLens system**, I want to **emit doctor report as JSONL with one check result per line**, so that **operators can parse results with shell tools and CI systems**.

**Acceptance Criteria:**

- **Given** all doctor checks complete
- **When** report generation runs
- **Then** report is written to {docs_path}/.nextlens/doctor-{run_id}.jsonl with format:
  ```json
  {"timestamp": "2026-05-14T12:34:56Z", "check_id": "schema-validity", "status": "pass", "severity": "info", "message": "All landscape files valid", "references": []}
  {"timestamp": "2026-05-14T12:34:56Z", "check_id": "feature-scope", "status": "fail", "severity": "blocking", "message": "Feature scope includes adjacent journey J1", "references": ["journey:J1"], "remediation": "Update explicitOutOfScope or reduce includedScope"}
  ...
  {"timestamp": "2026-05-14T12:34:56Z", "run_summary": "checks_run": 6, "passed": 4, "blocked": 1, "advisory": 1, "overall_status": "blocked"}
  ```

- **Given** report is generated
- **When** report is written
- **Then** each line is valid JSON, readable as JSONL, and references are dereferenceable

### Story 7.4: Implement Pre-Flight Doctor Integration

As a **NextLens system**, I want to **run all doctor checks before packet emission and block emission if blocking findings exist**, so that **invalid packets never reach BMAD**.

**Acceptance Criteria:**

- **Given** packet emission is about to start
- **When** pre-flight stage runs
- **Then** all doctor checks execute automatically (user not prompted to skip)

- **Given** doctor finds blocking-severity findings
- **When** pre-flight completes
- **Then** packet emission is blocked, status is fail, findings are displayed, operator must fix issues or cancel

- **Given** doctor finds advisory-severity findings
- **When** pre-flight completes
- **Then** packet emission can proceed, but operator is prompted for explicit confirmation ("Proceed with advisory findings?" [Y/n])

- **Given** operator declines to proceed with advisory findings
- **When** user selects no
- **Then** execution stops, no writes occur, packet is not emitted

---

## EP8: Feature Packet Schema & Emission

**Goal:** Implement Feature packet generation conforming to nextlens.feature-packet.v1 schema; implement confirmation gate before emission; ensure packet carries sufficient top-down context for BMAD while constraining scope.

**Scope:** Packet schema validation, packet composition, confirmation gate, emission logic, scope documentation.

**Dependencies:** EP1 (command spine), EP4 (feature ranking), EP5 (idempotency), EP6 (graph consistency), EP7 (doctor validation)

**Blocks:** EP9 (salmon routing depends on packet state), EP10 (evidence bundle documents packet emission)

### Story 8.1: Implement Feature Packet Schema Validation

As a **NextLens system**, I want to **validate Feature packet against nextlens.feature-packet.v1 schema**, so that **all emitted packets have required fields and correct types**.

**Acceptance Criteria:**

- **Given** Feature packet is being composed
- **When** schema validation runs
- **Then** packet must include:
  - schemaVersion: "nextlens.feature-packet.v1"
  - packetId: unique ID (UUID)
  - featureId: references exactly one selected candidate Feature
  - sourceMode: "top_down"
  - selectedFeature: {id, name, goal, includedScope, explicitOutOfScope}
  - trace: {systemId, discoveryEpochId, roleIds, outcomeIds, journeyIds, operatingLoopIds, relationshipRefs}
  - selectionRationale: {score, tieBreakEvidence, whyThisFeature, whyNow, rejectedAlternates}
  - sourceContextRefs: list of input document references
  - authoritativeStateRef: path to landscape state used
  - derivedGraphRef: path to derived graph used
  - doctorSummary: {status, blocking_count, advisory_count, informational_count}
  - salmonRoutingSummary: {status, routed_count}
  - bmadConsumerHints: {prdInput, uxInput, architectureInput, epicStoryInput, readinessInput}
  - evidenceBundleRef: path to evidence bundle
  - createdAt: timestamp

- **Given** required field is missing or wrong type
- **When** validation runs
- **Then** validation fails with clear error identifying field

### Story 8.2: Implement Packet Composition Logic

As a **NextLens system**, I want to **assemble Feature packet from ranking results, landscape state, and context**, so that **packet contains all required information for BMAD downstream**.

**Acceptance Criteria:**

- **Given** Feature is selected and confirmed
- **When** packet composition runs
- **Then** system assembles:
  - schemaVersion from constant
  - packetId from unique generator
  - featureId from selected candidate
  - sourceMode = "top_down"
  - selectedFeature from ranking result (name, goal, includedScope from Feature definition)
  - explicitOutOfScope populated with adjacent journeys, future Features, platform architecture, unrelated outcomes
  - trace from context sufficiency output and landscape state
  - selectionRationale from ranking scores and tie-break sequence
  - sourceContextRefs = {product-brief.md, prd.md, ux-design.md, architecture.md, research.md, brainstorm.md}
  - authoritativeStateRef = path to landscape directory
  - derivedGraphRef = path to derived graph
  - doctorSummary from doctor report
  - bmadConsumerHints from context (PRD goal, UX patterns, architecture decisions, expected epic count estimate, expected story count estimate)
  - createdAt = current timestamp

- **Given** packet composition completes
- **When** schema validation runs
- **Then** all required fields are present and valid

### Story 8.3: Implement Confirmation Gate

As a **NextLens operator**, I want to **review the selected Feature and explicitly confirm before packet emission**, so that **I have final control and can cancel if needed**.

**Acceptance Criteria:**

- **Given** Feature is ranked and confirmed, doctor validation passed, packet is composed
- **When** confirmation gate stage runs
- **Then** display format is:
  ```
  [stage:final-confirmation]
  
  About to emit Feature packet:
  - Packet ID: [packetId]
  - Feature: [name]
  - Goal: [goal]
  - Scope: [includedScope summary]
  - Out of Scope: [explicitOutOfScope summary]
  - Evidence: [traceability summary]
  - Doctor Status: [pass|advisory|blocked]
  
  Emit packet? [Y/n]
  ```

- **Given** operator confirms (Y)
- **When** confirmation is recorded
- **Then** proceed to emission stage, record confirmation in evidence bundle

- **Given** operator declines (n)
- **When** cancellation is recorded
- **Then** stop execution, no writes occur, no packet emitted, preserve diagnostic context

### Story 8.4: Implement Packet Emission and File Writing

As a **NextLens system**, I want to **emit Feature packet to {docs_path}/.nextlens/ with deterministic naming**, so that **BMAD can locate and consume the packet**.

**Acceptance Criteria:**

- **Given** confirmation gate passes
- **When** emission stage runs
- **Then** packet is written to {docs_path}/.nextlens/packet-{packetId}.json with:
  - JSON formatting (indented, readable)
  - file permissions (readable by control repo members)
  - atomic write (no partial files on failure)

- **Given** packet is written
- **When** write completes
- **Then** status is pass, packet path is printed, evidence bundle records write result

- **Given** write fails
- **When** failure occurs
- **Then** status is fail, rollback guidance is printed, no evidence of partial write remains

### Story 8.5: Implement BMAD Scope Containment Enforcement

As a **NextLens system**, I want to **document and enforce constraints that BMAD must respect**, so that **BMAD does not expand scope beyond selected Feature**.

**Acceptance Criteria:**

- **Given** Feature packet is emitted
- **When** bmadConsumerHints are populated
- **Then** hints include:
  - warning: "This packet represents one selected Feature from top-down discovery. Do not expand into adjacent journeys, future Features, platform architecture, or unrelated outcomes unless Salmon or correct-course signals scope change."
  - selectedFeature: {goal, includedScope, explicitOutOfScope}
  - architecture constraints: references to key architecture.md constraints affecting this Feature
  - traceability: system → role → outcome → journey → Feature lineage

- **Given** packet is ready for BMAD consumption
- **When** downstream phase begins
- **Then** BMAD receives packet with scope constraints clearly visible

---

## EP9: Salmon Correction Routing

**Goal:** Implement multi-source correction event model with fingerprint-based deduplication and deterministic routing to impacted landscape nodes and BMAD correct-course; ensure correction signals flow safely upstream without duplicate side effects.

**Scope:** Salmon event model, deduplication logic, routing classes, impact classification, event persistence.

**Dependencies:** EP3 (landscape state), EP5 (idempotency for writes), EP7 (doctor findings source)

**Blocks:** Evidence bundle documents corrections

### Story 9.1: Implement Salmon Event Model

As a **NextLens system**, I want to **define canonical Salmon event schema that captures correction signals from multiple sources**, so that **corrections are traceable and deduplicated**.

**Acceptance Criteria:**

- **Given** correction source (human, doctor, review, implementation) identifies issue
- **When** Salmon event is created
- **Then** event includes:
  - id: unique event ID
  - raisedFrom: source system (human, doctor check, reviewer, implementation)
  - source: {type: human|doctor|review|implementation, sourceId: check_id|reviewer_id|etc}
  - discovery: {issueDescription, impactedFeature, impactLevel}
  - impactedNodes: {features[], journeys[], outcomes[], roles[], operatingLoops[], capabilities[], bmadArtifacts[]}
  - severity: blocking|advisory|informational
  - recommendedAction: {type: local_note|landscape_update|block_packet|correct_course, details}
  - dedupFingerprint: deterministic hash of (normalized issue class, target stable ID, canonical path)
  - createdAt: timestamp
  - routingResult: {status: created|merged|duplicate_ignored, targetRef: path or entity ID}

- **Given** event schema is defined
- **When** validation runs
- **Then** all required fields are present and correctly typed

### Story 9.2: Implement Fingerprint-Based Deduplication

As a **NextLens system**, I want to **deduplicate Salmon events using deterministic fingerprints**, so that **duplicate reports do not create duplicate events**.

**Acceptance Criteria:**

- **Given** new correction signal arrives
- **When** fingerprint is generated
- **Then** fingerprint is calculated as:
  - normalized_issue_class (standardized category name)
  - target_stable_id (feature ID, journey ID, etc.)
  - canonical_path (normalized reference to affected node)
  - message_hash (SHA256 of normalized issue description)
  - fingerprint = SHA256(class + id + path + hash)

- **Given** fingerprint is generated
- **When** deduplication check runs
- **Then** system checks if fingerprint exists in {docs_path}/.nextlens/salmon/

- **Given** fingerprint does not exist
- **When** check completes
- **Then** event is marked as "new", routed to target, and persisted

- **Given** fingerprint exists with same event recorded
- **When** check completes
- **Then** event is marked as "duplicate_ignored", evidence records provenance attachment

- **Given** fingerprint exists but new source provides additional evidence
- **When** check completes
- **Then** event is marked as "merged", new source is appended to event, merged event is persisted

### Story 9.3: Implement Impact Classification and Routing

As a **NextLens system**, I want to **classify corrections by impact level and route to appropriate targets**, so that **corrections affect only intended scope**.

**Acceptance Criteria:**

- **Given** correction event is created
- **When** impact classification runs
- **Then** system classifies into one of eight levels:
  1. local_feature_note: affects only Feature notes, no state change
  2. feature_scope_change: changes Feature includedScope or explicitOutOfScope
  3. journey_assumption_change: affects Journey steps or preconditions
  4. outcome_reframe: redefines or reprioritizes Outcome
  5. role_or_stakeholder_change: adds/removes/redefines Role
  6. operating_loop_change: affects Operating Loop cycles
  7. capability_or_landscape_update: adds/updates system Capabilities
  8. bmad_correct_course_required: requires BMAD to rescope or replan

- **Given** impact level is determined
- **When** routing logic runs
- **Then** target is selected:
  - local_feature_note → route to Feature notes file
  - feature_scope_change → route to Feature definition + landscape update
  - journey_assumption_change → route to Journey file + BMAD if active
  - outcome_reframe → route to Outcome file + downstream Features
  - role_or_stakeholder_change → route to Role file + all impacted Features/Journeys
  - operating_loop_change → route to OperatingLoop file + impacted Journeys
  - capability_or_landscape_update → route to Capability file + Landscape updates
  - bmad_correct_course_required → route to Feature packet + BMAD correct-course record

- **Given** routing decision is made
- **When** event is persisted
- **Then** event is written to {docs_path}/.nextlens/salmon/{packetId}/{dedupFingerprint}.yaml

### Story 9.4: Implement Correction Event Persistence and Summary

As a **NextLens system**, I want to **persist all Salmon events with audit trail**, so that **corrections are traceable and reproducible**.

**Acceptance Criteria:**

- **Given** correction event is routed
- **When** persistence runs
- **Then** event YAML is written atomically to {docs_path}/.nextlens/salmon/{packetId}/

- **Given** one or more events are created per run
- **When** run completes
- **Then** summary is written to {docs_path}/.nextlens/salmon-summary-{packetId}.yaml with:
  - run_id
  - packet_id
  - events_created: count
  - events_merged: count
  - duplicates_ignored: count
  - impact_distribution: {local_feature_note: count, ...}
  - routing_results: {created, merged, ignored}

---

## EP10: Evidence Bundle & Traceability

**Goal:** Generate comprehensive evidence bundle capturing all stage outcomes, idempotency decisions, doctor findings, and correction routing for post-run review and audit.

**Scope:** Evidence collection, bundle schema, manifest generation, path references.

**Dependencies:** All prior epics (evidence collects from all stages)

**Blocks:** None (terminal artifact)

### Story 10.1: Implement Evidence Collection Framework

As a **NextLens system**, I want to **collect evidence from all stages in a structured, traceable format**, so that **operators and reviewers can reconstruct the run**.

**Acceptance Criteria:**

- **Given** each stage completes
- **When** stage result is available
- **Then** evidence collector records:
  - stage_name
  - timestamp
  - status (pass/warning/fail)
  - input summary (context schema, candidate count, scoring factors applied)
  - output summary (selections made, confirmations given, blocks encountered)
  - any warnings or diagnostic output

- **Given** all stages complete
- **When** evidence collection finishes
- **Then** evidence manifest is available for bundle assembly

### Story 10.2: Implement Evidence Bundle Generation

As a **NextLens system**, I want to **emit evidence bundle as structured YAML with all stage records**, so that **bundle serves as complete run audit trail**.

**Acceptance Criteria:**

- **Given** run completes (success or failure)
- **When** bundle generation runs
- **Then** bundle is written to {docs_path}/.nextlens/evidence-{packetId}.yaml with:
  ```yaml
  run_id: [generated UUID]
  packet_id: [packet ID if emitted, null if not]
  started_at: [timestamp]
  completed_at: [timestamp]
  duration_seconds: [elapsed]
  
  context_intake:
    status: pass|fail
    context_loaded_from: [path]
    schema_version_detected: [version]
  
  context_sufficiency:
    status: ready|ready_with_warnings|blocked
    checks:
      - gate_name: system_thesis
        status: pass|fail
      - gate_name: role_coverage
        status: pass|fail
        value: [count]
      ...
    warnings: [...]
  
  landscape_state:
    status: pass|fail
    entities_loaded: {system: count, role: count, outcome: count, ...}
    write_attempted: yes|no
    write_status: success|failed|skipped
    write_path: [path if written]
  
  feature_ranking:
    status: pass|fail
    candidates_evaluated: [count]
    top_candidate_selected: {id, name, score}
    tie_break_applied: yes|no
    tie_break_sequence: [factor1, factor2, ...]
    confirmation_given: yes|no
  
  doctor_validation:
    status: pass|fail
    checks_run: [count]
    blocking_findings: [count]
    advisory_findings: [count]
    informational_findings: [count]
    doctor_report_path: [path]
  
  graph_consistency:
    status: pass|fail
    consistency_checksum: [hash]
    nodes_validated: [count]
    edges_validated: [count]
  
  packet_emission:
    status: success|skipped|failed
    packet_id: [id if emitted]
    packet_path: [path if written]
    idempotency_token: [token used]
    idempotency_decision: new|replayed
  
  salmon_routing:
    events_created: [count]
    events_merged: [count]
    duplicates_ignored: [count]
    event_summary_path: [path]
  
  artifacts:
    packet: [path or null]
    doctor_report: [path]
    evidence_bundle: [path of this file]
    salmon_events: [directory path]
    landscape_state: [directory path]
    derived_graph: [path]
  
  operator_confirmations:
    - stage: candidate_selection
      confirmation: yes|no|timeout
      timestamp: [timestamp]
    - stage: final_confirmation
      confirmation: yes|no
      timestamp: [timestamp]
  
  errors: [error logs if any]
  warnings: [warning logs]
  ```

- **Given** bundle is complete
- **When** bundle is written
- **Then** bundle is persisted atomically to {docs_path}/.nextlens/evidence-{packetId}.yaml

### Story 10.3: Implement Run Traceability Artifacts

As a **NextLens system**, I want to **generate a traceability index that links packet to evidence and source context**, so that **reviewers can quickly navigate audit artifacts**.

**Acceptance Criteria:**

- **Given** packet is emitted and evidence is collected
- **When** traceability index is generated
- **Then** index is written to {docs_path}/.nextlens/traceability-index.md with:
  - table of recent runs: [packetId, timestamp, Feature selected, doctor status, status (success/advisory/blocked)]
  - quick links to evidence bundle, packet YAML, doctor report, Salmon events
  - lineage: system → role → outcome → journey → Feature → packet
  - key decisions: context sufficiency decision, ranking scores, tie-break applied, operator confirmations

---

## EP11: BMAD Module Packaging & Registration

**Goal:** Package NextLens as a distributable BMAD module with proper registration artifacts, configuration schema, and quality gates (Create Module, Validate Module); enable installation and discoverability through BMAD ecosystem.

**Scope:** Module identity, configuration, registration artifacts (module.yaml, module-help.csv, .claude-plugin/marketplace.json), setup flow, quality validation.

**Dependencies:** All implementation epics (module packages complete implementation)

**Blocks:** None (packaging and distribution)

### Story 11.1: Implement Module Identity and Configuration Schema

As a **NextLens module designer**, I want to **define module.yaml with NextLens identity and configuration variables**, so that **BMAD can discover and configure module capabilities**.

**Acceptance Criteria:**

- **Given** NextLens implementation is complete
- **When** module.yaml is created
- **Then** module.yaml includes:
  ```yaml
  module_id: nextlens-src
  module_name: NextLens Top-Down Bridge
  module_version: 1.0.0
  description: "Deterministic v1 top-down Feature packet bridge with validation and correction routing"
  
  capabilities:
    - command: nextlens-new
      description: "Create one Feature packet from top-down discovery context"
      entry_point: commands/new.ts
      skill_type: command
    
    - command: nextlens-doctor
      description: "Run non-mutating validation checks on packet or landscape"
      entry_point: commands/doctor.ts
      skill_type: command
    
    - command: nextlens-salmon
      description: "Route correction signals through deduplication and impact classification"
      entry_point: commands/salmon.ts
      skill_type: command
  
  configuration:
    - variable: NEXTLENS_DOCS_PATH
      type: string
      required: true
      description: "Path to feature docs directory (resolved from feature.yaml)"
    
    - variable: NEXTLENS_LANDSCAPE_STORE
      type: string
      default: "{docs_path}/landscape"
      description: "Landscape state storage directory"
    
    - variable: NEXTLENS_IDEMPOTENCY_TTL_HOURS
      type: number
      default: 24
      description: "Idempotency token time-to-live in hours"
  
  dependencies:
    - feature-yaml-resolver
    - bmad-constitution-resolver
  
  author: "NextLens Team"
  license: "MIT"
  ```

- **Given** module.yaml is complete
- **When** validation runs
- **Then** all required fields are present and correctly typed

### Story 11.2: Implement Module Help Registration

As a **NextLens module designer**, I want to **register capabilities in module-help.csv for BMAD discoverability**, so that **bmad-help can list and guide users**.

**Acceptance Criteria:**

- **Given** module capabilities are defined
- **When** module-help.csv is created
- **Then** CSV includes:
  ```
  command,category,description,entry_point,trigger_keywords
  nextlens-new,command,"Create one Feature packet from top-down discovery context",commands/new.ts,"nextlens new,top-down bridge,feature packet,deterministic selection"
  nextlens-doctor,command,"Run non-mutating validation checks on packet or landscape",commands/doctor.ts,"nextlens doctor,validate packet,check landscape,doctor validation"
  nextlens-salmon,command,"Route correction signals through deduplication and impact classification",commands/salmon.ts,"nextlens salmon,route correction,deduplicate events,correction routing"
  ```

- **Given** module-help.csv is complete
- **When** bmad-help loads the module
- **Then** capabilities appear in help output with correct descriptions and keywords

### Story 11.3: Implement Marketplace Manifest

As a **NextLens module designer**, I want to **create .claude-plugin/marketplace.json for installer and distribution compatibility**, so that **NextLens can be installed from package sources**.

**Acceptance Criteria:**

- **Given** module packaging is ready
- **When** marketplace.json is created
- **Then** manifest includes:
  ```json
  {
    "name": "NextLens Top-Down Bridge",
    "version": "1.0.0",
    "description": "Deterministic v1 top-down Feature packet bridge with validation and correction routing",
    "author": "NextLens Team",
    "repository": "https://github.com/[owner]/lens/tree/main/TargetProjects/nextlens/src",
    "plugins": [
      {
        "id": "nextlens-new",
        "name": "NextLens New Packet",
        "description": "Create one Feature packet from top-down discovery context",
        "skills": ["commands/new.ts"]
      },
      {
        "id": "nextlens-doctor",
        "name": "NextLens Doctor",
        "description": "Run non-mutating validation checks on packet or landscape",
        "skills": ["commands/doctor.ts"]
      },
      {
        "id": "nextlens-salmon",
        "name": "NextLens Salmon",
        "description": "Route correction signals through deduplication and impact classification",
        "skills": ["commands/salmon.ts"]
      }
    ],
    "keywords": ["nextlens", "top-down", "feature-packet", "bmad-module"],
    "license": "MIT"
  }
  ```

- **Given** manifest is complete
- **When** installer validates it
- **Then** manifest is accepted for distribution

### Story 11.4: Implement Create Module and Validate Module Gates

As a **NextLens implementation team**, I want to **run Create Module (CM) and Validate Module (VM) gates before release**, so that **packaged output is verified correct**.

**Acceptance Criteria:**

- **Given** implementation is ready for packaging
- **When** Create Module (CM) command runs
- **Then** CM:
  - regenerates module.yaml from current capabilities
  - regenerates module-help.csv from current skills
  - regenerates .claude-plugin/marketplace.json from current skills
  - verifies all referenced skill files exist
  - reports results (success/failure)

- **Given** CM completes successfully
- **When** Validate Module (VM) command runs
- **Then** VM:
  - validates module.yaml schema
  - validates module-help.csv format and consistency
  - validates marketplace.json completeness
  - checks that all skills in module-help.csv exist in manifest
  - checks that all manifest skills exist in repository
  - validates semantic versioning (major.minor.patch)
  - reports detailed findings or pass status

- **Given** VM finds no issues
- **When** validation completes
- **Then** module is approved for distribution

- **Given** VM finds structural issues
- **When** validation completes
- **Then** VM reports issues with remediation guidance; release is blocked until resolved

---

## Summary and Dependencies

**Implementation Sequence:**

1. **EP1** - Command Spine (entry point, enables all others)
2. **EP2** - Context Sufficiency (blocks ranking if not ready)
3. **EP3** - Landscape State (authoritative store)
4. **EP4** - Feature Ranking (depends on sufficiency and landscape)
5. **EP5** - Idempotency (dependency for writes)
6. **EP6** - Derived Graph (depends on landscape and idempotency)
7. **EP7** - Doctor Checks (validation gate before emission)
8. **EP8** - Packet Schema (depends on ranking, idempotency, doctor, graph)
9. **EP9** - Salmon Routing (depends on landscape, idempotency)
10. **EP10** - Evidence Bundle (collects from all prior stages)
11. **EP11** - BMAD Module (packages complete implementation)

**Total Stories:** 41

**Estimated Effort:** ~8-12 weeks for full v1 implementation with core functionality complete by week 6.
