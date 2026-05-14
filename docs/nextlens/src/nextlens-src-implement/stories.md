---
feature: nextlens-src-implement
doc_type: stories
status: approved
stepsCompleted: [validate-prerequisites, design-epics, map-coverage, generate-stories]
inputDocuments:
  - product-brief.md
  - prd.md
  - architecture.md
  - ux-design.md
  - research.md
  - brainstorm.md
updated_at: 2026-05-14T00:00:00Z
---

# NextLens v1 - User Stories

## Overview

This document lists all 38 user stories for NextLens v1 implementation, organized by epic. Each story includes:

- Story ID (Epic.Sequence)
- Story Title
- User Role and Motivation
- Acceptance Criteria
- Dependencies
- Estimated Complexity

Stories are sequenced for implementation following the epic dependency order.

---

## EP1: Command Spine & Interactive Entry Point

### Story 1.1: Implement Command Entry Point and Argument Parser

**Story ID:** 1.1  
**Epic:** EP1: Command Spine & Interactive Entry Point  
**Priority:** Critical (blocks all others)  
**Complexity:** Medium  

**User Story:**

As a **NextLens operator**, I want the system to **accept command-line arguments for mode (new, doctor, salmon), context input source, and optional config overrides**, so that **I can flexibly invoke different operational paths**.

**Acceptance Criteria:**

1. **Command Parsing - New Mode**
   - Given: `nextlens new --context path/to/context.yaml`
   - When: command is parsed
   - Then: mode=`new`, context_source resolves to `path/to/context.yaml`, command proceeds to intake stage

2. **Command Parsing - Doctor Mode**
   - Given: `nextlens doctor --packet path/to/packet.json`
   - When: command is parsed
   - Then: mode=`doctor`, packet_source resolves to `path/to/packet.json`, doctor validation stage starts

3. **Command Parsing - Salmon Mode**
   - Given: `nextlens salmon --findings path/to/findings.jsonl`
   - When: command is parsed
   - Then: mode=`salmon`, findings_source resolves, correction routing stage starts

4. **Invalid Arguments**
   - Given: invalid or missing required arguments
   - When: command is parsed
   - Then: usage/help message is printed with examples and valid next actions

5. **Config Overrides**
   - Given: `nextlens new --context path/to/context.yaml --docs-path /custom/path`
   - When: command is parsed
   - Then: override variables are recorded; defaults are preserved for unspecified options

6. **Help Text**
   - Given: `nextlens help` or `nextlens --help`
   - When: help is requested
   - Then: usage, modes, and example commands are printed

**Dependencies:** None (entry point)

**Definition of Done:**
- [ ] Command parser accepts all three modes (new, doctor, salmon)
- [ ] Invalid arguments produce helpful error messages
- [ ] Help text is clear and includes examples
- [ ] Config overrides are properly applied
- [ ] Unit tests cover all parsing paths

---

### Story 1.2: Implement Stage Pipeline Orchestration

**Story ID:** 1.2  
**Epic:** EP1: Command Spine & Interactive Entry Point  
**Priority:** Critical  
**Complexity:** High  

**User Story:**

As a **NextLens system architect**, I want the command spine **to transition deterministically through stages (intake → sufficiency → rank → confirm → write → rebuild → validate → emit → route)**, so that **operators always know which stage they're in and what comes next**.

**Acceptance Criteria:**

1. **Stage Pipeline Execution**
   - Given: command enters `new` mode
   - When: stages execute in sequence
   - Then: each stage prints `[stage:stageName]` header, processes input, prints outcome (pass/warning/fail)

2. **Stage Blocking**
   - Given: a stage fails with blocking reason
   - When: the stage prints fail outcome
   - Then: next valid action is displayed and execution stops; no further stages run

3. **Stage Transition Logging**
   - Given: a stage completes successfully
   - When: next stage is ready to start
   - Then: stage transition is logged with timestamp in evidence bundle

4. **Error Recovery Context**
   - Given: execution is blocked at a stage
   - When: diagnostic context is needed
   - Then: context is preserved for resume without re-running prior stages

5. **Stage State Persistence**
   - Given: pipeline is interrupted (user cancels, timeout, error)
   - When: interruption occurs
   - Then: pipeline state is saved to {docs_path}/.nextlens/pipeline-state.yaml for resume

**Dependencies:** Story 1.1 (argument parser)

**Definition of Done:**
- [ ] All 9 stages execute in correct sequence
- [ ] Stage transitions are logged with timestamps
- [ ] Blocking conditions stop further execution
- [ ] Evidence bundle records stage sequence
- [ ] Error recovery context is preserved
- [ ] Integration tests cover full pipeline

---

### Story 1.3: Implement Status Output Formatting and Progress Display

**Story ID:** 1.3  
**Epic:** EP1: Command Spine & Interactive Entry Point  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens operator**, I want the system to **display clear, scannable status messages at each stage without reliance on color or animation**, so that **I can follow progress in CI logs and terminal environments**.

**Acceptance Criteria:**

1. **Status Line Format**
   - Given: each stage completes
   - When: status line is printed
   - Then: format is `[stage:stageName] status=pass|warning|fail; message; next_action=...`

2. **Warning Display**
   - Given: warning-level findings exist
   - When: status is printed
   - Then: warnings are listed with remediation hints, not hidden by formatting

3. **Failure Display**
   - Given: failure occurs before confirmation
   - When: status is printed
   - Then: rollback action is shown, diagnostic context is printed, next valid action is specified

4. **No Color Dependency**
   - Given: output is displayed
   - When: terminal color is disabled
   - Then: all messages remain readable and understandable

5. **Progress Tracking**
   - Given: multi-stage execution runs
   - When: each stage completes
   - Then: operator can see progress (e.g., "Stage 3 of 9 complete")

6. **Line Wrapping**
   - Given: terminal width is narrow (80 columns)
   - When: output is rendered
   - Then: text wraps appropriately; no information is lost

**Dependencies:** Story 1.2 (stage pipeline)

**Definition of Done:**
- [ ] All status messages follow consistent format
- [ ] Messages are color-independent and readable
- [ ] Warning and failure messages include remediation hints
- [ ] Progress indicator shows current stage number
- [ ] Output works in CI logs (GitHub Actions, etc.)
- [ ] Manual QA on various terminal widths

---

## EP2: Context Sufficiency Gate

### Story 2.1: Implement Context Parsing and Schema Validation

**Story ID:** 2.1  
**Epic:** EP2: Context Sufficiency Gate  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **parse and validate incoming top-down context against the lens.topdown-context.v1 schema**, so that **I can detect missing or malformed input early**.

**Acceptance Criteria:**

1. **Schema Validation**
   - Given: top-down context is provided in YAML format
   - When: context is parsed
   - Then: required root fields are present: schemaVersion, system, discoveryEpoch, roles, outcomes, journeys, candidateFeatures

2. **Schema Version Check**
   - Given: schemaVersion is not `lens.topdown-context.v1`
   - When: parsing completes
   - Then: warning is logged, parsing continues with version-mismatch flag

3. **Malformed Input**
   - Given: malformed YAML or missing required fields
   - When: parsing is attempted
   - Then: detailed error message identifies missing field, parsing stops

4. **Nested Schema Validation**
   - Given: nested objects (system, roles, outcomes) are parsed
   - When: validation runs
   - Then: required nested fields are checked (system.id, role.id, outcome.id, etc.)

5. **Context Loading**
   - Given: context file path is specified
   - When: loading runs
   - Then: file is read, parsed as YAML, and validated

**Dependencies:** Story 1.1 (argument parser provides context path)

**Definition of Done:**
- [ ] Schema validation is implemented against lens.topdown-context.v1
- [ ] Malformed input produces clear error messages
- [ ] Version mismatch is warned but not fatal
- [ ] Nested fields are validated
- [ ] Unit tests cover valid and invalid schemas

---

### Story 2.2: Implement Context Sufficiency Check Logic

**Story ID:** 2.2  
**Epic:** EP2: Context Sufficiency Gate  
**Priority:** Critical  
**Complexity:** High  

**User Story:**

As a **NextLens system**, I want to **evaluate context completeness against the seven mandatory gates**, so that **I prevent silent failures due to incomplete discovery**.

**Acceptance Criteria:**

1. **Gate 1: System Thesis**
   - Given: context is checked
   - When: system.thesis is evaluated
   - Then: pass if thesis is present and non-empty (>0 chars); fail otherwise

2. **Gate 2: Role Coverage**
   - Given: context is checked
   - When: roles array is evaluated
   - Then: pass if at least one role is present; fail otherwise

3. **Gate 3: Outcome Coverage**
   - Given: context is checked
   - When: outcomes array is evaluated
   - Then: pass if at least one outcome is present; fail otherwise

4. **Gate 4: Journey/Hypothesis Coverage**
   - Given: context is checked
   - When: journeys array or journey hypotheses are evaluated
   - Then: pass if at least one journey or hypothesis is present; fail otherwise

5. **Gate 5: Candidate Traceability**
   - Given: context is checked
   - When: candidateFeatures are evaluated
   - Then: pass if each candidate traces to at least one journey or outcome; fail otherwise

6. **Gate 6: Risks and Open Questions**
   - Given: context is checked
   - When: risks and openQuestions arrays are evaluated
   - Then: pass if both present (may be empty); advisory if sparse (< 3 items each)

7. **Gate 7: BMAD Consumer Context**
   - Given: context is checked
   - When: bmadConsumerContext field is evaluated
   - Then: pass if provided; advisory if missing

8. **Status Determination**
   - Given: all gates are evaluated
   - When: status is determined
   - Then:
     - `ready` if all gates pass
     - `ready_with_warnings` if gates pass but advisories exist
     - `blocked` if any gate fails

9. **Missing Required Reporting**
   - Given: one or more gates fail
   - When: status is blocked
   - Then: missingRequired array lists all failed gates

10. **Warnings Reporting**
    - Given: gates pass but advisories exist
    - When: status is ready_with_warnings
    - Then: warnings array lists conditions

**Dependencies:** Story 2.1 (context parsing and validation)

**Definition of Done:**
- [ ] All seven gates are implemented and tested
- [ ] Status determination logic is correct
- [ ] Missing required and warnings arrays are populated accurately
- [ ] Unit tests cover pass, ready_with_warnings, and blocked scenarios

---

### Story 2.3: Implement Context Sufficiency Report Output

**Story ID:** 2.3  
**Epic:** EP2: Context Sufficiency Gate  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens operator**, I want the system to **display sufficiency status in a clear, actionable format**, so that **I can immediately understand what's missing and what to do next**.

**Acceptance Criteria:**

1. **Report Header**
   - Given: sufficiency check completes
   - When: report is printed
   - Then: header line is `[stage:context-sufficiency]`

2. **Gate Status Table**
   - Given: report is printed
   - When: gate results are displayed
   - Then: format shows:
     ```
     system_thesis: [✓|✗] [value or "missing"]
     role_coverage: [✓|✗] [count] roles
     outcome_coverage: [✓|✗] [count] outcomes
     journey_coverage: [✓|✗] [count] journeys
     candidate_traceability: [✓|✗] [count] candidates traceable
     risks_captured: [✓|✗] [count] risks
     bmad_hints: [✓|✗] [present/missing hints]
     ```

3. **Status and Recommendation**
   - Given: all gates are evaluated
   - When: report is printed
   - Then: status line shows: `status: ready|ready_with_warnings|blocked`
   - And: recommendation line shows: `recommendation: continue|ask_for_confirmation|return_to_discovery`

4. **Blocked Status Display**
   - Given: status is `blocked`
   - When: report is printed
   - Then: no confirmation prompt is shown; next valid action is "return to discovery"

5. **Ready with Warnings Display**
   - Given: status is `ready_with_warnings`
   - When: report is printed
   - Then: warnings are listed with context; operator is prompted for explicit confirmation

6. **Checkmark/X Symbols**
   - Given: status symbols are used
   - When: no color is available
   - Then: symbols (✓ and ✗) are readable in plain text

**Dependencies:** Story 2.2 (sufficiency check logic), Story 1.3 (status output formatting)

**Definition of Done:**
- [ ] Report format matches specification
- [ ] Blocked status prevents continuation
- [ ] Ready_with_warnings prompts for confirmation
- [ ] Report is readable without color
- [ ] Integration test verifies full output flow

---

## EP3: Landscape State Management

### Story 3.1: Implement Stable ID Generation and Validation

**Story ID:** 3.1  
**Epic:** EP3: Landscape State Management  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **generate deterministic stable IDs for all landscape entities** and **validate that IDs are unique and immutable**, so that **I can reliably reference and trace entities across multiple runs**.

**Acceptance Criteria:**

1. **Semantic ID Generation**
   - Given: a new landscape entity (system, role, outcome, journey, operating loop) is created
   - When: ID generation runs
   - Then: a semantic ID is generated (human-visible, normalized from entity name, e.g., "role-system-architect" from "System Architect")

2. **Opaque ID Generation**
   - Given: semantic ID is generated
   - When: opaque ID generation runs
   - Then: an immutable machine reference is generated (UUID or deterministic hash based on semantic ID)

3. **ID Immutability**
   - Given: an entity with stable ID already exists
   - When: reconstruction runs
   - Then: the same ID is recovered and used for all subsequent operations

4. **ID Uniqueness**
   - Given: two entities with identical normalized names
   - When: ID generation runs
   - Then: both receive unique IDs (suffix-based disambiguation applied, e.g., "role-1", "role-2")

5. **ID Validation**
   - Given: ID immutability is validated
   - When: doctor check runs
   - Then: if ID is changed, warning is reported; evidence bundle records rename event

6. **ID Format**
   - Given: semantic ID is generated
   - When: format is determined
   - Then: format is lowercase, hyphen-separated, max 50 chars

**Dependencies:** None

**Definition of Done:**
- [ ] Semantic ID generation is deterministic
- [ ] Opaque IDs are unique and immutable
- [ ] ID uniqueness collision handling is tested
- [ ] ID validation detects changes
- [ ] Unit tests cover all ID generation paths

---

### Story 3.2: Implement Landscape State Persistence

**Story ID:** 3.2  
**Epic:** EP3: Landscape State Management  
**Priority:** High  
**Complexity:** High  

**User Story:**

As a **NextLens system**, I want to **persist landscape state in the control repo at {docs_path}/landscape/**, so that **state is version-controlled, auditable, and shareable**.

**Acceptance Criteria:**

1. **Directory Structure**
   - Given: landscape state is ready to write
   - When: directory initialization runs
   - Then: directory structure is created:
     ```
     {docs_path}/landscape/
       system/
       role/
       outcome/
       journey/
       operating_loop/
       capability/
       decision/
       risk/
     ```

2. **Entity File Persistence**
   - Given: authoritative state is ready to write
   - When: write stage executes
   - Then: state is written to {docs_path}/landscape/{entity-type}/{stable-id}.yaml with:
     - entity identity (semantic ID, opaque ID, name)
     - state snapshot (content specific to entity type)
     - relationships (parent/child, cross-entity references)
     - metadata (createdAt, updatedAt, source, author)

3. **Write Success Validation**
   - Given: write completes successfully
   - When: file system is checked
   - Then: file is readable, parseable, and matches schema

4. **Write Failure Handling**
   - Given: write fails (permission, disk space, etc.)
   - When: failure occurs
   - Then: error is logged, transaction is rolled back, status is fail, packet emission is blocked

5. **Atomic Writes**
   - Given: landscape state is being written
   - When: write is executed
   - Then: writes are atomic (temp file → rename) to prevent partial writes on failure

6. **Permission Enforcement**
   - Given: landscape files are written
   - When: write completes
   - Then: file permissions are set to allow control repo members to read and write

**Dependencies:** Story 3.1 (stable ID generation)

**Definition of Done:**
- [ ] Directory structure is created automatically
- [ ] Entity files are written with correct schema
- [ ] Atomic writes are implemented
- [ ] Write failures are handled gracefully
- [ ] Integration test verifies persistence and recovery

---

### Story 3.3: Implement State Reconstruction and Query

**Story ID:** 3.3  
**Epic:** EP3: Landscape State Management  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **reconstruct landscape state deterministically from persisted files**, so that **replay runs produce identical results**.

**Acceptance Criteria:**

1. **Entity Loading**
   - Given: existing landscape files at {docs_path}/landscape/
   - When: state reconstruction runs
   - Then: all entities are loaded in dependency order (systems → roles/outcomes → journeys/operating loops → capabilities)

2. **Relationship Resolution**
   - Given: entity relationships are recorded in files
   - When: state is reconstructed
   - Then: all parent/child and cross-entity references are validated and resolved

3. **Query by ID**
   - Given: state is fully reconstructed
   - When: query runs for a specific entity by stable ID
   - Then: the entity and all its relationships are returned without additional I/O

4. **Query by Type**
   - Given: state is reconstructed
   - When: query runs for all entities of a type (e.g., all outcomes)
   - Then: all entities of that type are returned in deterministic order

5. **Relationship Queries**
   - Given: state is reconstructed
   - When: query runs for entities related to a specific entity (e.g., "all journeys for outcome X")
   - Then: related entities are returned with edge metadata

6. **Validation on Reconstruction**
   - Given: state is reconstructed
   - When: validation runs
   - Then: missing or broken references are reported as warnings; state is still usable

**Dependencies:** Story 3.2 (state persistence)

**Definition of Done:**
- [ ] Entity loading in correct dependency order
- [ ] Relationship resolution is complete
- [ ] Query API is performant (no unnecessary I/O)
- [ ] Unit tests cover all query types
- [ ] Integration test verifies reconstruction matches original state

---

## EP4: Feature Ranking & Selection

### Story 4.1: Implement Deterministic Scoring Algorithm

**Story ID:** 4.1  
**Epic:** EP4: Feature Ranking & Selection  
**Priority:** Critical  
**Complexity:** High  

**User Story:**

As a **NextLens system**, I want to **score candidate Features using nine fixed factors in deterministic order**, so that **ranking is reproducible and explainable**.

**Acceptance Criteria:**

1. **Outcome Alignment Score**
   - Given: candidate Feature with context
   - When: scoring runs
   - Then: score = (count of covered outcomes × weight by criticality) / total outcomes
   - Range: 0-100

2. **Journey Criticality Score**
   - Given: candidate Feature with context
   - When: scoring runs
   - Then: score = (count of journeys covered × weight by role dependency) / total journeys
   - Range: 0-100

3. **Role Value Score**
   - Given: candidate Feature with context
   - When: scoring runs
   - Then: score = (count of roles impacted × weight by stakeholder importance) / total roles
   - Range: 0-100

4. **Risk Reduction Score**
   - Given: candidate Feature with context
   - When: scoring runs
   - Then: if total risks = 0, score defaults to 100; otherwise score = (count of mitigated risks × weight by severity) / total risks
   - Range: 0-100

5. **Dependency Readiness Score**
   - Given: candidate Feature with dependencies
   - When: scoring runs
   - Then: score = (count of satisfied upstream dependencies) / total dependencies
   - Range: 0-100

6. **Implementation Boundedness Score**
   - Given: candidate Feature with scope
   - When: scoring runs
   - Then: score = clarity penalty (0-100), higher if scope is clear and bounded, lower if spillage detected

7. **BMAD Readiness Score**
   - Given: candidate Feature with BMAD inputs
   - When: scoring runs
   - Then: score = (count of available PRD, UX, architecture) / 3 × 100
   - Range: 0-100

8. **Evidence Clarity Score**
   - Given: candidate Feature with rationale
   - When: scoring runs
   - Then: score = (traceability completeness: system → role → outcome → journey → feature) × 100
   - Range: 0-100

9. **Open-Question Severity Score**
   - Given: candidate Feature with unresolved questions
   - When: scoring runs
   - Then: score = 100 - (count × severity weight) clamped to [0, 100]

10. **Composite Score**
    - Given: all nine factor scores are calculated
    - When: composite score is computed
    - Then: composite = average of all nine scores (equal weight)
    - Range: 0-100

11. **Scoring Determinism**
    - Given: identical inputs and state
    - When: scoring runs twice
    - Then: composite scores and ranking order are identical

**Dependencies:** Story 2.3 (context sufficiency provides validated context), Story 3.3 (state reconstruction provides entity context)

**Definition of Done:**
- [ ] All nine scoring factors are implemented
- [ ] Composite score calculation is deterministic
- [ ] Unit tests verify each factor calculation
- [ ] Integration test verifies reproducibility with identical inputs

---

### Story 4.2: Implement Tie-Break Sequence

**Story ID:** 4.2  
**Epic:** EP4: Feature Ranking & Selection  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **apply deterministic tie-break logic when candidates have equal composite scores**, so that **tie-break is always the same given identical inputs**.

**Acceptance Criteria:**

1. **Tier 1 Tie-Break: Outcome Alignment**
   - Given: two candidates with equal composite score
   - When: tie-break runs
   - Then: candidate with highest outcome alignment score ranks first

2. **Tier 2 Tie-Break: Journey Criticality**
   - Given: outcome alignment scores are also equal
   - When: tier 2 runs
   - Then: candidate with highest journey criticality score ranks first

3. **Tier 3 Tie-Break: Unresolved Blockers**
   - Given: tier 2 scores are equal
   - When: tier 3 runs
   - Then: candidate with fewest unresolved blockers ranks first

4. **Tier 4 Tie-Break: Evidence Clarity**
   - Given: tier 3 scores are equal
   - When: tier 4 runs
   - Then: candidate with highest evidence clarity score ranks first

5. **Tier 5 Tie-Break: Candidate Timestamp**
   - Given: tier 4 scores are equal
   - When: tier 5 runs
   - Then: candidate with earliest stable candidate timestamp (first discovered) ranks first

6. **Tier 6 Tie-Break: Lexical Order**
   - Given: tier 5 timestamps are equal
   - When: tier 6 runs
   - Then: lexical ordering of Feature ID is applied (A before Z)

7. **Full Tie-Break Determinism**
   - Given: identical inputs
   - When: tie-break sequence runs twice
   - Then: results are identical

**Dependencies:** Story 4.1 (scoring algorithm)

**Definition of Done:**
- [ ] All six tie-break tiers are implemented
- [ ] Tie-break sequence is deterministic
- [ ] Unit tests verify each tier
- [ ] Integration test verifies full tie-break sequence

---

### Story 4.3: Implement Candidate Presentation and Selection

**Story ID:** 4.3  
**Epic:** EP4: Feature Ranking & Selection  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens operator**, I want to **see the top-ranked candidate with up to two alternatives displayed**, so that **I understand why the system selected the top candidate and can override if needed**.

**Acceptance Criteria:**

1. **Display Format**
   - Given: candidates are ranked
   - When: presentation stage runs
   - Then: output format is:
     ```
     [stage:candidate-selection]
     
     Selected Candidate (Rank 1):
     [details...]
     
     Alternative (Rank 2):
     [details...]
     
     Alternative (Rank 3):
     [details...]
     ```

2. **Selected Candidate Details**
   - Given: top candidate is displayed
   - When: details are shown
   - Then: name, goal, score, and rationale (outcome alignment, journey criticality, role value, risk reduction, evidence) are displayed

3. **Alternative Candidate Details**
   - Given: alternative candidates are displayed
   - When: details are shown
   - Then: name, score, and reason why not selected (score delta, missing coverage) are displayed

4. **User Confirmation Prompt**
   - Given: candidates are displayed
   - When: display completes
   - Then: prompt is shown: `Confirm selection? [Y/n]`

5. **Confirm Action**
   - Given: operator confirms (Y)
   - When: confirmation is recorded
   - Then: selectedFeature is locked, proceed to confirmation gate

6. **Decline Action**
   - Given: operator declines (n)
   - When: operator is prompted
   - Then: offer options: (1) select different candidate, (2) cancel flow with no writes, (3) return to context with modifications

7. **Alternative Selection**
   - Given: operator chooses different candidate
   - When: selection is made
   - Then: new candidate is displayed with confirmation prompt

**Dependencies:** Story 4.1 (scoring algorithm), Story 4.2 (tie-break sequence), Story 1.3 (status output formatting)

**Definition of Done:**
- [ ] Candidate presentation format is correct
- [ ] Confirmation prompt works correctly
- [ ] Alternative selection flows work
- [ ] Integration test verifies full user flow

---

## EP5: Idempotent Writes & Replay Safety

### Story 5.1: Implement Idempotency Token Generation and Storage

**Story ID:** 5.1  
**Epic:** EP5: Idempotent Writes & Replay Safety  
**Priority:** Critical  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **generate and store idempotency tokens for all mutating requests**, so that **retry attempts are safe and deterministic**.

**Acceptance Criteria:**

1. **Token Generation**
   - Given: a mutating operation is about to start
   - When: token generation runs
   - Then: a unique idempotency token is created using the canonical string format of either UUID v4 or KSUID and stored without transformation

2. **Token Metadata**
   - Given: token is generated
   - When: metadata is recorded
   - Then: metadata includes:
     - token value
     - operation type (write-landscape, emit-packet, route-correction)
     - timestamp (request issued at, ISO 8601)
     - request digest (SHA256 hash of operation parameters)

3. **Token Storage**
   - Given: token is generated
   - When: token storage runs
   - Then: token record is persisted at {docs_path}/.idempotency/tokens/{token-value}.yaml

4. **Token Record Format**
   - Given: token record is created
   - When: format is determined
   - Then: record includes:
     - status: pending | completed | failed
     - createdAt: timestamp
     - completedAt: timestamp (if applicable)
     - result: response envelope (if completed)

5. **Token TTL**
   - Given: token is persisted
   - When: token lifecycle runs
   - Then: TTL is set to 24 hours; tokens older than TTL can be archived

6. **Token Uniqueness**
   - Given: multiple operations are executed
   - When: tokens are generated
   - Then: each token is unique (no collisions)

**Dependencies:** None

**Definition of Done:**
- [ ] Token generation is implemented
- [ ] Token storage is atomic and reliable
- [ ] Token metadata is complete
- [ ] TTL mechanism is implemented
- [ ] Unit tests verify token uniqueness and storage

---

### Story 5.2: Implement Request Deduplication

**Story ID:** 5.2  
**Epic:** EP5: Idempotent Writes & Replay Safety  
**Priority:** Critical  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **detect and deduplicate duplicate requests using idempotency tokens**, so that **reruns with the same token never create side effects**.

**Acceptance Criteria:**

1. **Token Lookup**
   - Given: a request arrives with idempotency token
   - When: deduplication check runs
   - Then: system checks if token exists in store at {docs_path}/.idempotency/tokens/{token-value}.yaml

2. **New Request Path**
   - Given: token does not exist
   - When: check completes
   - Then: operation is marked as "new" and proceeds to execution

3. **Completed Request Path**
   - Given: token exists with status=completed
   - When: check completes
   - Then: operation is skipped; original result is returned (replay)

4. **Pending Request Path**
   - Given: token exists with status=pending
   - When: check completes
   - Then: system waits for completion (with timeout) or returns error with retry-after suggestion

5. **Failed Request Path**
   - Given: token exists with status=failed
   - When: check completes
   - Then: system returns error and suggests contact support or manual recovery

6. **Deduplication Performance**
   - Given: token lookup runs
   - When: performance is measured
   - Then: lookup completes in < 100ms (file I/O bound)

**Dependencies:** Story 5.1 (token generation and storage)

**Definition of Done:**
- [ ] Token lookup is implemented
- [ ] All four request paths are tested
- [ ] Deduplication prevents duplicate side effects
- [ ] Integration test verifies replay semantics

---

### Story 5.3: Implement Response Replay Semantics

**Story ID:** 5.3  
**Epic:** EP5: Idempotent Writes & Replay Safety  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **return original response envelopes for replayed requests**, so that **operators experience consistent results across retries**.

**Acceptance Criteria:**

1. **Response Envelope Storage**
   - Given: operation completes successfully
   - When: response envelope is recorded
   - Then: envelope is stored in token record with:
     - operation_result (success/warning/failure)
     - output_summary (key values returned)
     - packet_reference (if applicable)
     - evidence_bundle_path (if applicable)
     - doctor_status (if applicable)
     - timestamp

2. **Response Replay**
   - Given: idempotency token is marked completed with result stored
   - When: duplicate request with same token arrives
   - Then: original response envelope is returned without re-executing operation

3. **Replay Logging**
   - Given: response is replayed
   - When: response is returned
   - Then: evidence bundle records that response was replayed, not freshly generated

4. **Configuration Consistency**
   - Given: operator runs same command twice with different output configuration
   - When: idempotency token is identical
   - Then: the same result is returned (configuration override ignored for replay)

5. **Replay Idempotency**
   - Given: same request is replayed multiple times
   - When: each replay is executed
   - Then: identical response envelope is returned each time

**Dependencies:** Story 5.2 (request deduplication), Story 5.1 (token storage)

**Definition of Done:**
- [ ] Response envelope format is defined and stored
- [ ] Replay logic returns original envelope
- [ ] Replay logging is implemented
- [ ] Integration test verifies replay consistency

---

## EP6: Derived Graph Projection

### Story 6.1: Implement Derived Graph Schema

**Story ID:** 6.1  
**Epic:** EP6: Derived Graph Projection  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **define a canonical derived graph schema that captures all landscape entity relationships**, so that **graph queries are fast and consistent**.

**Acceptance Criteria:**

1. **Node Types**
   - Given: graph schema is defined
   - When: node types are specified
   - Then: schema includes nodes for:
     - system (root)
     - role
     - outcome
     - journey
     - operating_loop
     - capability
     - decision
     - risk

2. **Edge Types**
   - Given: schema is defined
   - When: edge types are specified
   - Then: edges capture:
     - system → role (system requires role participation)
     - role → outcome (role achieves outcome)
     - outcome → journey (outcome fulfilled by journey)
     - journey → operating_loop (journey contains operating loop)
     - operating_loop → capability (loop uses capability)
     - capability → decision (decision governs capability)
     - feature → journey/outcome (feature proves journey/outcome)

3. **Node Properties**
   - Given: node is defined
   - When: properties are specified
   - Then: node includes:
     - id (stable ID)
     - type (enum of node types)
     - label (human-readable name)
     - metadata (entity-specific fields)

4. **Edge Properties**
   - Given: edge is defined
   - When: properties are specified
   - Then: edge includes:
     - source (node ID)
     - target (node ID)
     - type (edge type)
     - label (relationship description)
     - metadata (edge-specific fields)

5. **Schema Versioning**
   - Given: schema is versioned
   - When: version is specified
   - Then: schema includes version identifier (e.g., "1.0")

**Dependencies:** Story 3.1 (stable IDs), Story 3.3 (state reconstruction and query)

**Definition of Done:**
- [ ] Node and edge types are defined
- [ ] Schema is documented
- [ ] Unit tests verify schema structure

---

### Story 6.2: Implement Eager Graph Rebuild

**Story ID:** 6.2  
**Epic:** EP6: Derived Graph Projection  
**Priority:** Critical  
**Complexity:** High  

**User Story:**

As a **NextLens system**, I want to **rebuild derived graph immediately after every successful authoritative write**, so that **graph is always consistent with latest state**.

**Acceptance Criteria:**

1. **Post-Write Trigger**
   - Given: landscape write completes successfully
   - When: post-write stage runs
   - Then: graph rebuild is triggered automatically (not optional)

2. **Edge Resolution**
   - Given: graph rebuild runs
   - When: rebuilding edges
   - Then: all entity references are resolved:
     - system → role edges created for all roles in landscape
     - role → outcome edges created for roles that achieve outcomes
     - (and so on for all edge types)

3. **Broken Reference Detection**
   - Given: graph rebuild runs
   - When: edge resolution occurs
   - Then: broken references are reported as warnings:
     - feature references non-existent journey
     - outcome references deleted role
     - (etc.)

4. **Orphaned Node Detection**
   - Given: graph rebuild runs
   - When: node analysis occurs
   - Then: orphaned nodes are flagged:
     - nodes with no incoming or outgoing edges
     - nodes not reachable from system root

5. **Graph Output**
   - Given: graph rebuild completes
   - When: output is written
   - Then: graph is persisted at {docs_path}/derived/graph.json with:
     - nodes array (id, type, label, metadata)
     - edges array (source, target, type, label, metadata)
     - metadata (generatedAt, sourceStateRef, consistency checksum)

6. **Consistency Checksum**
    - Given: consistency checksum is generated
    - When: graph validation runs
    - Then: checksum is deterministic (same state → same checksum)
    - Checksum algorithm: SHA256(sorted node IDs + sorted edge IDs)

**Dependencies:** Story 3.2 (landscape state persistence), Story 6.1 (graph schema), Story 5.1 (token generation for rebuild operation)

**Definition of Done:**
- [ ] Post-write rebuild is triggered automatically
- [ ] All edges are resolved correctly
- [ ] Broken references and orphaned nodes are detected
- [ ] Graph is written with correct structure
- [ ] Consistency checksum is deterministic
- [ ] Integration test verifies rebuild on landscape write

---

### Story 6.3: Implement Graph Consistency Validation

**Story ID:** 6.3  
**Epic:** EP6: Derived Graph Projection  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **validate that derived graph is consistent with authoritative state before packet emission**, so that **packet never references stale relationships**.

**Acceptance Criteria:**

1. **Entity Validation**
   - Given: packet emission is about to start
   - When: consistency check runs
   - Then: system verifies that all entities referenced in graph exist in landscape state

2. **Edge Validation**
   - Given: consistency check runs
   - When: edges are validated
   - Then: all edges reference valid nodes; no edges to non-existent nodes

3. **Checksum Validation**
   - Given: consistency check runs
   - When: consistency checksum is compared
   - Then: checksum matches current state (no stale data)

4. **Broken Reference Detection**
   - Given: consistency check runs
   - When: broken references are found
   - Then: check reports all broken references

5. **Orphaned Node Detection**
   - Given: consistency check runs
   - When: orphaned nodes are found
   - Then: check reports all orphaned nodes

6. **Check Result**
   - Given: consistency check completes
   - When: status is set
   - Then:
     - pass: all checks pass, emit packet
     - advisory: warnings exist but packet can emit with advisory
     - fail: critical issues, emit is blocked, landscape is checked for corruption

7. **Recovery Guidance**
   - Given: consistency check fails
   - When: failure is reported
   - Then: error message identifies inconsistency and suggests recovery action (rebuild graph, fix landscape state, etc.)

**Dependencies:** Story 6.2 (eager graph rebuild), Story 3.3 (state query)

**Definition of Done:**
- [ ] Entity validation is implemented
- [ ] Edge validation is implemented
- [ ] Checksum validation is implemented
- [ ] Recovery guidance is provided
- [ ] Integration test verifies consistency check before emission

---

## EP7: Doctor Checks & Validation

### Story 7.1: Implement Doctor Check Framework

**Story ID:** 7.1  
**Epic:** EP7: Doctor Checks & Validation  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **provide a pluggable framework for non-mutating doctor checks**, so that **I can add new checks without modifying core emission logic**.

**Acceptance Criteria:**

1. **Check Registration**
   - Given: doctor framework is initialized
   - When: new check is registered
   - Then: check provides:
     - check_id (stable, immutable identifier)
     - name (human-readable)
     - category (schema, scope, traceability, readiness)
     - severity (blocking, advisory, informational)
     - description
     - remediation hint
     - execute function (non-mutating)

2. **Check Execution**
   - Given: check is registered
   - When: framework starts execution
   - Then: framework calls check.execute() with inputs:
     - landscape state (read-only)
     - derived graph (read-only)
     - packet candidate (if applicable)
     - selected feature (if applicable)

3. **Check Result Format**
   - Given: check is executed
   - When: result is returned
   - Then: result includes:
     - status (pass, warning, fail)
     - severity (blocking, advisory, informational)
     - message (human-readable)
     - references (list of affected entity IDs or paths)
     - remediation (suggested fix)

4. **Result Collection**
   - Given: all checks are executed
   - When: results are collected
   - Then: framework aggregates results by severity:
     - blocking results
     - advisory results
     - informational results

5. **Framework Logging**
   - Given: checks are executed
   - When: execution completes
   - Then: framework logs check execution with timestamps

6. **No Side Effects**
   - Given: doctor checks are executed
   - When: checks complete
   - Then: no files are modified, no state changes occur (read-only)

**Dependencies:** None

**Definition of Done:**
- [ ] Check registration is implemented
- [ ] Framework executes registered checks
- [ ] Result format is standardized
- [ ] Framework is non-mutating
- [ ] Unit tests verify check registration and execution

---

### Story 7.2: Implement Core Doctor Checks

**Story ID:** 7.2  
**Epic:** EP7: Doctor Checks & Validation  
**Priority:** High  
**Complexity:** High  

**User Story:**

As a **NextLens system**, I want to **provide essential doctor checks for common failure modes**, so that **operators can catch issues before packet emission**.

**Acceptance Criteria:**

1. **Check: schema-validity**
   - Given: landscape state is available
   - When: check runs
   - Then: check verifies:
     - all entity files match schema
     - required fields are present in all entities
     - no orphaned files exist
   - Severity: blocking
   - Result: pass/fail

2. **Check: feature-scope**
   - Given: candidate Feature is selected
   - When: check runs
   - Then: check verifies:
     - selectedFeature.includedScope does not contain adjacent journeys
     - selectedFeature.includedScope does not contain future Features
     - selectedFeature.includedScope does not contain unrelated platform architecture
     - selectedFeature.explicitOutOfScope is populated
   - Severity: blocking (if spillage detected)
   - Result: pass/fail

3. **Check: traceability**
   - Given: Feature packet is prepared
   - When: check runs
   - Then: check verifies:
     - packet.trace.systemId is valid and resolvable
     - packet.trace.roleIds all resolve to landscape roles
     - packet.trace.outcomeIds all resolve to landscape outcomes
     - packet.trace.journeyIds all resolve to landscape journeys
     - packet.selectionRationale is non-empty
   - Severity: advisory (if references broken)
   - Result: pass/warning

4. **Check: context-readiness**
   - Given: context sufficiency passed
   - When: check runs
   - Then: check verifies:
     - BMAD consumer hints (prdInput, uxInput, architectureInput) are populated
     - top-down context includes system thesis
     - roles, outcomes, journeys are present
     - open questions and risks are captured
   - Severity: advisory
   - Result: pass/warning

5. **Check: write-boundary**
   - Given: all pre-conditions checked
   - When: check runs
   - Then: check verifies:
     - write targets are within {docs_path}
     - write targets are not in governance repo
     - write targets are not in release clone paths
   - Severity: blocking
   - Result: pass/fail

6. **Check: graph-consistency**
   - Given: derived graph is available
   - When: check runs
   - Then: check verifies:
     - derived graph is consistent with landscape state
     - consistency checksum matches current state
     - no broken references or orphaned nodes
   - Severity: advisory
   - Result: pass/warning

**Dependencies:** Story 7.1 (doctor check framework), Story 3.2 (landscape state), Story 6.3 (graph consistency check)

**Definition of Done:**
- [ ] All six checks are implemented
- [ ] Checks are non-mutating and read-only
- [ ] Results follow standard format
- [ ] Unit tests verify each check
- [ ] Integration test verifies all checks execute correctly

---

### Story 7.3: Implement JSONL Doctor Report Generation

**Story ID:** 7.3  
**Epic:** EP7: Doctor Checks & Validation  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **emit doctor report as JSONL with one check result per line**, so that **operators can parse results with shell tools and CI systems**.

**Acceptance Criteria:**

1. **Report File Creation**
   - Given: all doctor checks complete
   - When: report generation runs
   - Then: report is written to {docs_path}/.nextlens/doctor-{run_id}.jsonl

2. **Report Line Format**
   - Given: each check completes
   - When: result is written to report
   - Then: format is valid JSON on single line:
     ```json
     {"timestamp": "2026-05-14T12:34:56Z", "check_id": "schema-validity", "status": "pass", "severity": "blocking", "message": "All landscape files valid", "references": []}
     ```

3. **Result Fields**
   - Given: check result is written
   - When: fields are populated
   - Then: fields include:
     - timestamp (ISO 8601)
     - check_id (stable identifier)
     - status (pass/warning/fail)
     - severity (blocking/advisory/informational)
     - message (human-readable)
     - references (affected entity IDs or paths)
     - remediation (if applicable)

4. **Summary Line**
   - Given: all checks complete
   - When: summary line is written
   - Then: final line includes:
     - checks_run: count
     - passed: count
     - blocked: count
     - advisory: count
     - overall_status: pass/advisory/blocked

5. **JSONL Compliance**
   - Given: report is generated
   - When: format is validated
   - Then:
     - each line is valid JSON
     - newline-delimited (one result per line)
     - readable by standard JSONL parsers

6. **Report Readability**
   - Given: report is generated
   - When: it is parsed by shell tools
   - Then: it can be processed with `jq`, `grep`, `awk` without issues

**Dependencies:** Story 7.2 (core doctor checks)

**Definition of Done:**
- [ ] JSONL report generation is implemented
- [ ] Report format is correct and readable
- [ ] All required fields are present
- [ ] Integration test verifies report generation and parsing

---

### Story 7.4: Implement Pre-Flight Doctor Integration

**Story ID:** 7.4  
**Epic:** EP7: Doctor Checks & Validation  
**Priority:** Critical  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **run all doctor checks before packet emission and block emission if blocking findings exist**, so that **invalid packets never reach BMAD**.

**Acceptance Criteria:**

1. **Pre-Flight Stage Trigger**
   - Given: packet emission is about to start
   - When: pre-flight stage runs
   - Then: all doctor checks execute automatically (not optional)

2. **Blocking Findings Detection**
   - Given: doctor finds blocking-severity findings
   - When: pre-flight completes
   - Then:
     - packet emission is blocked
     - status is fail
     - findings are displayed with remediation hints
     - operator must fix issues or cancel

3. **Advisory Findings Display**
   - Given: doctor finds advisory-severity findings
   - When: pre-flight completes
   - Then:
     - packet emission can proceed
     - operator is prompted for explicit confirmation ("Proceed with advisory findings?" [Y/n])

4. **Operator Confirmation on Advisory**
   - Given: advisory findings exist
   - When: operator is prompted
   - Then:
     - If Y: proceed to emission
     - If n: stop execution, no writes, packet not emitted

5. **Operator Cancellation**
   - Given: operator declines to proceed with advisory findings
   - When: user selects no
   - Then: execution stops cleanly, no writes occur, packet is not emitted

6. **Informational Findings Logging**
   - Given: informational-severity findings exist
   - When: pre-flight completes
   - Then: findings are logged but do not block or prompt for confirmation

**Dependencies:** Story 7.3 (JSONL report generation), Story 1.3 (status output formatting)

**Definition of Done:**
- [ ] Pre-flight is triggered automatically before emission
- [ ] Blocking findings prevent emission
- [ ] Advisory findings prompt for confirmation
- [ ] Operator can confirm or decline
- [ ] Integration test verifies pre-flight flow

---

## EP8: Feature Packet Schema & Emission

### Story 8.1: Implement Feature Packet Schema Validation

**Story ID:** 8.1  
**Epic:** EP8: Feature Packet Schema & Emission  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **validate Feature packet against nextlens.feature-packet.v1 schema**, so that **all emitted packets have required fields and correct types**.

**Acceptance Criteria:**

1. **Required Fields**
   - Given: Feature packet is being composed
   - When: schema validation runs
   - Then: packet must include:
     - schemaVersion: "nextlens.feature-packet.v1" (string)
     - packetId: unique ID (UUID, string)
     - featureId: references exactly one selected candidate (string)
     - sourceMode: "top_down" (string)
     - selectedFeature: object with {id, name, goal, includedScope, explicitOutOfScope}
     - trace: object with {systemId, discoveryEpochId, roleIds, outcomeIds, journeyIds, operatingLoopIds, relationshipRefs}
     - selectionRationale: object with {score, tieBreakEvidence, whyThisFeature, whyNow, rejectedAlternates}
     - sourceContextRefs: array
     - authoritativeStateRef: string (path)
     - derivedGraphRef: string (path)
     - doctorSummary: object
     - salmonRoutingSummary: object
     - bmadConsumerHints: object
     - evidenceBundleRef: string (path)
     - createdAt: ISO 8601 timestamp

2. **Field Validation Errors**
   - Given: required field is missing or wrong type
   - When: validation runs
   - Then: validation fails with clear error identifying:
     - field name
     - expected type
     - actual value (if present)

3. **Type Checking**
   - Given: fields are validated
   - When: type checking runs
   - Then:
     - arrays are checked for array type
     - objects are checked for object type
     - strings are checked for string type
     - etc.

4. **Nested Field Validation**
   - Given: nested objects are validated
   - When: validation runs
   - Then: required nested fields are checked (e.g., selectedFeature.id, selectedFeature.name)

**Dependencies:** None (schema validation is prerequisite)

**Definition of Done:**
- [ ] Schema validation is implemented against nextlens.feature-packet.v1
- [ ] All required fields are checked
- [ ] Type validation is correct
- [ ] Error messages are helpful
- [ ] Unit tests cover all validation paths

---

### Story 8.2: Implement Packet Composition Logic

**Story ID:** 8.2  
**Epic:** EP8: Feature Packet Schema & Emission  
**Priority:** High  
**Complexity:** High  

**User Story:**

As a **NextLens system**, I want to **assemble Feature packet from ranking results, landscape state, and context**, so that **packet contains all required information for BMAD downstream**.

**Acceptance Criteria:**

1. **Packet Assembly**
   - Given: Feature is selected and confirmed
   - When: packet composition runs
   - Then: system assembles:
     - schemaVersion = "nextlens.feature-packet.v1"
     - packetId from unique generator (UUID v4)
     - featureId from selected candidate
     - sourceMode = "top_down"

2. **Selected Feature Population**
   - Given: Feature is selected
   - When: selectedFeature is populated
   - Then:
     - name from Feature definition
     - goal from Feature definition
     - includedScope from Feature definition
     - explicitOutOfScope populated with: adjacent journeys, future Features, platform architecture, unrelated outcomes

3. **Trace Population**
   - Given: context is available
   - When: trace is populated
   - Then:
     - systemId from discovered system
     - discoveryEpochId from discovery context
     - roleIds from all impacted roles
     - outcomeIds from all covered outcomes
     - journeyIds from all involved journeys
     - operatingLoopIds from affected operating loops
     - relationshipRefs to preserve lineage

4. **Selection Rationale Population**
   - Given: ranking results are available
   - When: selectionRationale is populated
   - Then:
     - score from composite score
     - tieBreakEvidence from tie-break sequence (if applied)
     - whyThisFeature textual explanation
     - whyNow timing rationale
     - rejectedAlternates list of rank 2, 3 candidates and scores

5. **Source Context References**
   - Given: input documents are available
   - When: sourceContextRefs is populated
   - Then: array includes paths: product-brief.md, prd.md, ux-design.md, architecture.md, research.md, brainstorm.md

6. **State References**
   - Given: authoritative state and derived graph are available
   - When: references are populated
   - Then:
     - authoritativeStateRef = path to landscape directory
     - derivedGraphRef = path to derived graph JSON

7. **Doctor Summary Population**
   - Given: doctor checks complete
   - When: doctorSummary is populated
   - Then:
     - status from doctor overall_status
     - blocking_count
     - advisory_count
     - informational_count

8. **BMAD Hints Population**
   - Given: context is available
   - When: bmadConsumerHints is populated
   - Then: hints include:
     - warning about scope containment
     - prdInput: PRD goal and key requirements
     - uxInput: UX patterns and key flows
     - architectureInput: architecture decisions affecting Feature
     - epicStoryInput: estimated epic/story breakdown
     - readinessInput: current readiness status

9. **Timestamps**
   - Given: packet is composed
   - When: createdAt is set
   - Then: timestamp is ISO 8601 format with UTC timezone

10. **Packet Validation**
    - Given: packet composition completes
    - When: schema validation runs
    - Then: all required fields are present and valid (Story 8.1)

**Dependencies:** Story 4.1 (scoring algorithm), Story 4.3 (candidate selection), Story 3.3 (landscape state query), Story 6.2 (derived graph rebuild), Story 7.2 (doctor checks)

**Definition of Done:**
- [ ] Packet assembly logic is implemented
- [ ] All required fields are populated
- [ ] References are accurate and resolvable
- [ ] Schema validation passes
- [ ] Integration test verifies full composition

---

### Story 8.3: Implement Confirmation Gate

**Story ID:** 8.3  
**Epic:** EP8: Feature Packet Schema & Emission  
**Priority:** High  
**Complexity:** Low  

**User Story:**

As a **NextLens operator**, I want to **review the selected Feature and explicitly confirm before packet emission**, so that **I have final control and can cancel if needed**.

**Acceptance Criteria:**

1. **Confirmation Display**
   - Given: Feature is ranked and confirmed, doctor validation passed, packet is composed
   - When: confirmation gate stage runs
   - Then: display format is:
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

2. **Confirm Action**
   - Given: operator confirms (Y)
   - When: confirmation is recorded
   - Then: proceed to emission stage, record confirmation in evidence bundle with timestamp

3. **Cancel Action**
   - Given: operator declines (n)
   - When: cancellation is recorded
   - Then:
     - stop execution
     - no writes occur
     - no packet emitted
     - preserve diagnostic context for resume

4. **Input Validation**
   - Given: user provides invalid input (not Y or n)
   - When: input is processed
   - Then: prompt is re-displayed: "Please enter Y or n:"

**Dependencies:** Story 8.2 (packet composition), Story 7.4 (pre-flight doctor), Story 1.3 (status output formatting)

**Definition of Done:**
- [ ] Confirmation prompt is displayed correctly
- [ ] Confirm action proceeds to emission
- [ ] Cancel action stops execution
- [ ] Input validation handles invalid entries

---

### Story 8.4: Implement Packet Emission and File Writing

**Story ID:** 8.4  
**Epic:** EP8: Feature Packet Schema & Emission  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **emit Feature packet to {docs_path}/.nextlens/ with deterministic naming**, so that **BMAD can locate and consume the packet**.

**Acceptance Criteria:**

1. **Directory Creation**
   - Given: packet emission is starting
   - When: directory setup runs
   - Then: directory {docs_path}/.nextlens/ is created if not present

2. **File Naming**
   - Given: packet is ready to write
   - When: filename is determined
   - Then: filename is `packet-{packetId}.json` where packetId is UUID

3. **File Writing**
   - Given: confirmation gate passes
   - When: emission stage runs
   - Then: packet is written to {docs_path}/.nextlens/packet-{packetId}.json with:
     - JSON formatting (indented, readable)
     - UTF-8 encoding
     - file permissions (readable by control repo members)

4. **Atomic Write**
   - Given: packet is being written
   - When: write is executed
   - Then: write is atomic (temp file → rename) to prevent partial writes on failure

5. **Write Success**
   - Given: packet is written
   - When: write completes
   - Then:
     - status is pass
     - packet path is printed: `Packet emitted to: {docs_path}/.nextlens/packet-{packetId}.json`
     - evidence bundle records write result with timestamp

6. **Write Failure**
   - Given: write fails
   - When: failure occurs
   - Then:
     - status is fail
     - error message identifies failure reason
     - rollback guidance is printed (partial write cleanup, etc.)
     - no evidence of partial write remains
     - packet is not emitted

7. **File Permissions**
   - Given: packet file is written
   - When: permissions are set
   - Then: permissions allow control repo members to read and write

**Dependencies:** Story 8.3 (confirmation gate), Story 5.1 (idempotency token for write operation)

**Definition of Done:**
- [ ] Directory creation is implemented
- [ ] File writing is atomic
- [ ] Write success is confirmed
- [ ] Write failure is handled gracefully
- [ ] Integration test verifies file creation and content

---

### Story 8.5: Implement BMAD Scope Containment Enforcement

**Story ID:** 8.5  
**Epic:** EP8: Feature Packet Schema & Emission  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **document and enforce constraints that BMAD must respect**, so that **BMAD does not expand scope beyond selected Feature**.

**Acceptance Criteria:**

1. **Scope Constraints Documentation**
   - Given: Feature packet is emitted
   - When: bmadConsumerHints are populated
   - Then: hints include warning:
     ```
     "This packet represents one selected Feature from top-down discovery. 
      Do not expand into adjacent journeys, future Features, platform 
      architecture, or unrelated outcomes unless Salmon or correct-course 
      signals scope change."
     ```

2. **Selected Feature Scope**
   - Given: packet is prepared
   - When: bmadConsumerHints are populated
   - Then: hints include:
     - selectedFeature: {goal, includedScope, explicitOutOfScope}

3. **Architecture Constraints**
   - Given: packet is prepared
   - When: bmadConsumerHints are populated
   - Then: hints include references to:
     - key architecture.md constraints affecting this Feature
     - feature packet schema version
     - packet composition timestamp

4. **Traceability Documentation**
   - Given: packet is prepared
   - When: bmadConsumerHints are populated
   - Then: hints include lineage:
     ```
     system → role → outcome → journey → Feature
     ```

5. **Hint Format**
   - Given: bmadConsumerHints are populated
   - When: format is determined
   - Then:
     - hints are human-readable text
     - references are resolvable (paths, IDs)
     - warnings are clear and actionable

**Dependencies:** Story 8.2 (packet composition)

**Definition of Done:**
- [ ] Scope constraints are documented in packet
- [ ] BMAD hints are clear and complete
- [ ] Integration test verifies hints are present and readable

---

## EP9: Salmon Correction Routing

### Story 9.1: Implement Salmon Event Model

**Story ID:** 9.1  
**Epic:** EP9: Salmon Correction Routing  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **define canonical Salmon event schema that captures correction signals from multiple sources**, so that **corrections are traceable and deduplicated**.

**Acceptance Criteria:**

1. **Event Schema Definition**
   - Given: Salmon event model is defined
   - When: schema is specified
   - Then: event includes:
     - id: unique event ID (UUID)
     - raisedFrom: source system (human, doctor check, reviewer, implementation)
     - source: {type: human|doctor|review|implementation, sourceId: check_id|reviewer_id|etc}
     - discovery: {issueDescription, impactedFeature, impactLevel}
     - impactedNodes: {features[], journeys[], outcomes[], roles[], operatingLoops[], capabilities[], bmadArtifacts[]}
     - severity: blocking|advisory|informational
     - recommendedAction: {type: local_note|landscape_update|block_packet|correct_course, details}
     - dedupFingerprint: deterministic hash
     - createdAt: ISO 8601 timestamp
     - routingResult: {status: created|merged|duplicate_ignored, targetRef: path or entity ID}

2. **Field Types**
   - Given: event schema is defined
   - When: field types are specified
   - Then:
     - string fields are UTF-8, max 1000 chars (except description)
     - array fields are JSON arrays
     - timestamp fields are ISO 8601
     - hash fields are SHA256 hex

3. **Schema Validation**
   - Given: event schema is defined
   - When: validation is implemented
   - Then: all required fields are checked before event creation

4. **Event Versioning**
   - Given: event schema is defined
   - When: versioning is specified
   - Then: schema includes version identifier (e.g., "1.0")

**Dependencies:** None

**Definition of Done:**
- [ ] Event schema is fully defined
- [ ] Schema validation is implemented
- [ ] Schema is documented
- [ ] Unit tests verify schema structure

---

### Story 9.2: Implement Fingerprint-Based Deduplication

**Story ID:** 9.2  
**Epic:** EP9: Salmon Correction Routing  
**Priority:** Critical  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **deduplicate Salmon events using deterministic fingerprints**, so that **duplicate reports do not create duplicate events**.

**Acceptance Criteria:**

1. **Fingerprint Generation**
   - Given: new correction signal arrives
   - When: fingerprint is generated
   - Then: fingerprint is calculated as:
     - normalized_issue_class (standardized category name, lowercase)
     - target_stable_id (feature ID, journey ID, outcome ID, etc.)
     - canonical_path (normalized reference to affected node)
     - message_hash (SHA256 of normalized issue description)
     - fingerprint = SHA256(class + "|" + id + "|" + path + "|" + hash)

2. **Fingerprint Storage Lookup**
   - Given: fingerprint is generated
   - When: deduplication check runs
   - Then: system checks if fingerprint exists in:
     - {docs_path}/.nextlens/salmon/fingerprints.json (index)

3. **New Event Path**
   - Given: fingerprint does not exist
   - When: check completes
   - Then: event is marked as "new", routed to target, and persisted

4. **Duplicate Event Path**
   - Given: fingerprint exists with same event recorded
   - When: check completes
   - Then: event is marked as "duplicate_ignored", evidence records provenance attachment

5. **Merged Event Path**
   - Given: fingerprint exists but new source provides additional evidence
   - When: check completes
   - Then:
     - event is marked as "merged"
     - new source is appended to event.sources array
     - merged event is persisted (updates existing record)

6. **Fingerprint Determinism**
   - Given: identical correction signals
   - When: fingerprint is generated twice
   - Then: fingerprints match exactly

**Dependencies:** Story 9.1 (event schema)

**Definition of Done:**
- [ ] Fingerprint generation is deterministic
- [ ] Lookup and deduplication logic works correctly
- [ ] All three paths (new, duplicate, merged) are tested
- [ ] Integration test verifies deduplication end-to-end

---

### Story 9.3: Implement Impact Classification and Routing

**Story ID:** 9.3  
**Epic:** EP9: Salmon Correction Routing  
**Priority:** High  
**Complexity:** High  

**User Story:**

As a **NextLens system**, I want to **classify corrections by impact level and route to appropriate targets**, so that **corrections affect only intended scope**.

**Acceptance Criteria:**

1. **Impact Classification**
   - Given: correction event is created
   - When: impact classification runs
   - Then: system classifies into one of eight levels:
     1. local_feature_note: affects only Feature notes, no state change
     2. feature_scope_change: changes Feature includedScope or explicitOutOfScope
     3. journey_assumption_change: affects Journey steps or preconditions
     4. outcome_reframe: redefines or reprioritizes Outcome
     5. role_or_stakeholder_change: adds/removes/redefines Role
     6. operating_loop_change: affects Operating Loop cycles
     7. capability_or_landscape_update: adds/updates system Capabilities
     8. bmad_correct_course_required: requires BMAD to rescope or replan

2. **Routing Logic: local_feature_note**
   - Given: impact level is local_feature_note
   - When: routing logic runs
   - Then: target is Feature notes file; event is appended as comment

3. **Routing Logic: feature_scope_change**
   - Given: impact level is feature_scope_change
   - When: routing logic runs
   - Then: target is Feature definition file; includedScope or explicitOutOfScope is updated; landscape update recorded

4. **Routing Logic: journey_assumption_change**
   - Given: impact level is journey_assumption_change
   - When: routing logic runs
   - Then: target is Journey file; journey definition updated; if BMAD is active, BMAD is notified

5. **Routing Logic: outcome_reframe**
   - Given: impact level is outcome_reframe
   - When: routing logic runs
   - Then: target is Outcome file; outcome updated; all impacted Features and Journeys are notified

6. **Routing Logic: role_or_stakeholder_change**
   - Given: impact level is role_or_stakeholder_change
   - When: routing logic runs
   - Then: target is Role file; role updated; all impacted Features, Journeys, Outcomes are notified

7. **Routing Logic: operating_loop_change**
   - Given: impact level is operating_loop_change
   - When: routing logic runs
   - Then: target is OperatingLoop file; loop updated; impacted Journeys are notified

8. **Routing Logic: capability_or_landscape_update**
   - Given: impact level is capability_or_landscape_update
   - When: routing logic runs
   - Then: target is Capability file and Landscape updates; new entity records created if needed

9. **Routing Logic: bmad_correct_course_required**
   - Given: impact level is bmad_correct_course_required
   - When: routing logic runs
   - Then: target is Feature packet and BMAD correct-course record; correction-tracking record created

10. **Routing Result**
    - Given: routing decision is made
    - When: event is persisted
    - Then: routingResult includes:
      - status: created|merged|duplicate_ignored
      - targetRef: path or entity ID where routed
      - timestamp: when routed

**Dependencies:** Story 9.1 (event schema), Story 9.2 (deduplication)

**Definition of Done:**
- [ ] All eight impact levels are classified correctly
- [ ] Routing logic is implemented for each level
- [ ] Routing results are recorded
- [ ] Integration test verifies routing end-to-end

---

### Story 9.4: Implement Correction Event Persistence and Summary

**Story ID:** 9.4  
**Epic:** EP9: Salmon Correction Routing  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **persist all Salmon events with audit trail**, so that **corrections are traceable and reproducible**.

**Acceptance Criteria:**

1. **Event Persistence**
   - Given: correction event is routed
   - When: persistence runs
   - Then: event YAML is written atomically to {docs_path}/.nextlens/salmon/{packetId}/{dedupFingerprint}.yaml

2. **Directory Structure**
   - Given: event persistence runs
   - When: directory setup occurs
   - Then: directories are created:
     ```
     {docs_path}/.nextlens/salmon/
       {packetId}/
         [fingerprint1].yaml
         [fingerprint2].yaml
         ...
     ```

3. **Event Record Format**
   - Given: event is persisted
   - When: format is determined
   - Then: YAML includes all event fields plus:
     - _metadata: {persistedAt, persistedBy, fileChecksum}

4. **Summary Generation**
   - Given: one or more events are created per run
   - When: run completes
   - Then: summary is written to {docs_path}/.nextlens/salmon-summary-{packetId}.yaml with:
     ```yaml
     run_id: [UUID]
     packet_id: [packetId]
     events_created: [count]
     events_merged: [count]
     duplicates_ignored: [count]
     impact_distribution:
       local_feature_note: [count]
       feature_scope_change: [count]
       journey_assumption_change: [count]
       outcome_reframe: [count]
       role_or_stakeholder_change: [count]
       operating_loop_change: [count]
       capability_or_landscape_update: [count]
       bmad_correct_course_required: [count]
     routing_results:
       created: [count]
       merged: [count]
       ignored: [count]
     ```

5. **Atomic Persistence**
   - Given: event is being persisted
   - When: persistence runs
   - Then: writes are atomic (temp file → rename) to prevent partial writes

6. **Persistence Logging**
   - Given: events are persisted
   - When: persistence completes
   - Then: evidence bundle records:
     - event count
     - routing decisions
     - any errors or warnings

**Dependencies:** Story 9.3 (impact classification and routing)

**Definition of Done:**
- [ ] Event persistence is atomic
- [ ] Summary is generated accurately
- [ ] Directory structure is created correctly
- [ ] Integration test verifies persistence and summary

---

## EP10: Evidence Bundle & Traceability

### Story 10.1: Implement Evidence Collection Framework

**Story ID:** 10.1  
**Epic:** EP10: Evidence Bundle & Traceability  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **collect evidence from all stages in a structured, traceable format**, so that **operators and reviewers can reconstruct the run**.

**Acceptance Criteria:**

1. **Evidence Collector Initialization**
   - Given: command starts
   - When: evidence collector is initialized
   - Then: collector creates run record with:
     - run_id: unique UUID
     - started_at: ISO 8601 timestamp
     - stage_records: [] (empty array for stage results)

2. **Per-Stage Evidence Capture**
   - Given: each stage completes
   - When: stage result is available
   - Then: evidence collector records:
     - stage_name (context-intake, context-sufficiency, feature-ranking, etc.)
     - timestamp (stage start and end)
     - status (pass/warning/fail)
     - input summary (count of items processed, key values)
     - output summary (decisions made, confirmations given, blocks encountered)
     - duration (elapsed time in seconds)
     - any warnings or diagnostic output

3. **Evidence Collection Points**
   - Given: all stages execute
   - When: evidence is collected
   - Then: collection points include:
     - command arguments and config
     - context intake and parsing
     - context sufficiency check result
     - landscape state reconstruction
     - feature ranking and tie-break application
     - operator confirmations
     - doctor validation results
     - packet emission result
     - salmon routing results
     - any errors or exceptions

4. **Evidence Aggregation**
   - Given: all stages complete
   - When: evidence collection finishes
   - Then: evidence manifest is available for bundle assembly

**Dependencies:** None

**Definition of Done:**
- [ ] Evidence collector is initialized at command start
- [ ] All stages record evidence
- [ ] Evidence format is structured and queryable
- [ ] Unit tests verify evidence collection

---

### Story 10.2: Implement Evidence Bundle Generation

**Story ID:** 10.2  
**Epic:** EP10: Evidence Bundle & Traceability  
**Priority:** High  
**Complexity:** High  

**User Story:**

As a **NextLens system**, I want to **emit evidence bundle as structured YAML with all stage records**, so that **bundle serves as complete run audit trail**.

**Acceptance Criteria:**

1. **Bundle File Creation**
   - Given: run completes (success or failure)
   - When: bundle generation runs
   - Then: bundle is written to {docs_path}/.nextlens/evidence-{packetId-or-runId}.yaml, using run_id as the filename fallback when packet_id is null

2. **Run Metadata**
   - Given: bundle is created
   - When: metadata is populated
   - Then: includes:
     - run_id: unique UUID
     - packet_id: packet ID if emitted, null if not
     - started_at: ISO 8601 timestamp
     - completed_at: ISO 8601 timestamp
     - duration_seconds: elapsed time

3. **Context Intake Record**
   - Given: context intake stage completes
   - When: record is added to bundle
   - Then: record includes:
     - status: pass|fail
     - context_loaded_from: file path
     - schema_version_detected: version string
     - context_entity_counts: {systems, roles, outcomes, journeys, ...}

4. **Context Sufficiency Record**
   - Given: sufficiency check completes
   - When: record is added to bundle
   - Then: record includes:
     - status: ready|ready_with_warnings|blocked
     - gate_results: [{gate_name, status, value_if_applicable}]
     - warnings: array of warning messages
     - recommendation: string

5. **Landscape State Record**
   - Given: landscape stage completes
   - When: record is added to bundle
   - Then: record includes:
     - status: pass|fail
     - entities_loaded: {system: count, role: count, ...}
     - write_attempted: yes|no
     - write_status: success|failed|skipped
     - write_path: path if written

6. **Feature Ranking Record**
   - Given: ranking stage completes
   - When: record is added to bundle
   - Then: record includes:
     - status: pass|fail
     - candidates_evaluated: count
     - top_candidate_selected: {id, name, score}
     - tie_break_applied: yes|no
     - tie_break_sequence: array of tiers applied
     - confirmation_given: yes|no

7. **Doctor Validation Record**
   - Given: doctor stage completes
   - When: record is added to bundle
   - Then: record includes:
     - status: pass|fail
     - checks_run: count
     - blocking_findings: count
     - advisory_findings: count
     - informational_findings: count
     - doctor_report_path: file path

8. **Graph Consistency Record**
   - Given: graph check completes
   - When: record is added to bundle
   - Then: record includes:
     - status: pass|fail
     - consistency_checksum: SHA256 hash
     - nodes_validated: count
     - edges_validated: count

9. **Packet Emission Record**
   - Given: emission stage completes
   - When: record is added to bundle
   - Then: record includes:
     - status: success|skipped|failed
     - packet_id: UUID if emitted
     - packet_path: file path if written
     - idempotency_token: token used
     - idempotency_decision: new|replayed

10. **Salmon Routing Record**
    - Given: salmon stage completes
    - When: record is added to bundle
    - Then: record includes:
      - events_created: count
      - events_merged: count
      - duplicates_ignored: count
      - event_summary_path: file path

11. **Artifacts Record**
    - Given: bundle is created
    - When: artifacts section is populated
    - Then: includes paths to:
      - packet JSON file
      - doctor report JSONL
      - evidence bundle YAML (itself)
      - salmon events directory
      - landscape state directory
      - derived graph JSON

12. **Operator Confirmations Record**
    - Given: operator makes confirmations
    - When: confirmations are recorded
    - Then: includes array of:
      - stage name
      - confirmation (yes|no|timeout)
      - timestamp

13. **Errors and Warnings Record**
    - Given: run encounters errors or warnings
    - When: records are added
    - Then: includes:
      - error logs (if any)
      - warning logs (if any)
      - exception traces (if applicable)

**Dependencies:** Story 10.1 (evidence collection framework), all prior epics (evidence collected from all)

**Definition of Done:**
- [ ] Bundle file is created at correct path
- [ ] All required sections are populated
- [ ] Bundle is valid YAML
- [ ] Integration test verifies bundle generation

---

### Story 10.3: Implement Run Traceability Artifacts

**Story ID:** 10.3  
**Epic:** EP10: Evidence Bundle & Traceability  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens system**, I want to **generate a traceability index that links packet to evidence and source context**, so that **reviewers can quickly navigate audit artifacts**.

**Acceptance Criteria:**

1. **Traceability Index File**
   - Given: packet is emitted and evidence is collected
   - When: traceability index is generated
   - Then: index is written to {docs_path}/.nextlens/traceability-index.md

2. **Recent Runs Table**
   - Given: traceability index is created
   - When: recent runs section is populated
   - Then: table includes columns:
     - Run ID (first 8 chars)
     - Timestamp
     - Feature Selected
     - Doctor Status
     - Result (success|advisory|blocked)
     - Links to evidence and packet

3. **Quick Links Section**
   - Given: index is created
   - When: quick links section is populated
   - Then: includes markdown links to:
     - Latest evidence bundle
     - Latest packet JSON
     - Latest doctor report
     - Latest salmon events summary

4. **Lineage Section**
   - Given: index is created
   - When: lineage section is populated
   - Then: displays tree structure:
     ```
     System → Roles → Outcomes → Journeys → Feature → Packet
     [system-id]
       → [role-1-id]: [role-name]
           → [outcome-id]: [outcome-name]
               → [journey-id]: [journey-name]
                   → [feature-id]: [feature-name]
                       → [packet-id]: [packet-path]
     ```

5. **Key Decisions Section**
   - Given: index is created
   - When: key decisions section is populated
   - Then: includes:
     - Context sufficiency decision and rationale
     - Ranking scores and tie-break applied (if applicable)
     - Operator confirmations given
     - Doctor findings summary

6. **Index Updateability**
   - Given: new runs are generated
   - When: traceability index is updated
   - Then:
     - new run entry is appended to recent runs table
     - latest links are updated
     - lineage is updated if different Feature selected

**Dependencies:** Story 10.2 (evidence bundle generation), Story 8.4 (packet emission)

**Definition of Done:**
- [ ] Traceability index file is created
- [ ] All required sections are present
- [ ] Links are accurate and resolvable
- [ ] Index is human-readable markdown

---

## EP11: BMAD Module Packaging & Registration

### Story 11.1: Implement Module Identity and Configuration Schema

**Story ID:** 11.1  
**Epic:** EP11: BMAD Module Packaging & Registration  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens module designer**, I want to **define module.yaml with NextLens identity and configuration variables**, so that **BMAD can discover and configure module capabilities**.

**Acceptance Criteria:**

1. **Module Identity**
   - Given: module.yaml is created
   - When: identity fields are populated
   - Then: includes:
     - module_id: "nextlens-src" (lowercase, hyphenated)
     - module_name: "NextLens Top-Down Bridge" (human-readable)
     - module_version: "1.0.0" (semantic versioning)
     - description: clear summary of module purpose
     - author: "NextLens Team"
     - license: "MIT"

2. **Capabilities Definition**
   - Given: module capabilities are defined
   - When: capabilities section is populated
   - Then: includes entries for:
     - command: nextlens-new
       - description: "Create one Feature packet from top-down discovery context"
       - entry_point: relative path to skill file
       - skill_type: "command"
     - command: nextlens-doctor
       - description: "Run non-mutating validation checks on packet or landscape"
       - entry_point: relative path to skill file
       - skill_type: "command"
     - command: nextlens-salmon
       - description: "Route correction signals through deduplication and impact classification"
       - entry_point: relative path to skill file
       - skill_type: "command"

3. **Configuration Variables**
   - Given: module configuration is defined
   - When: configuration section is populated
   - Then: includes entries for:
     - NEXTLENS_DOCS_PATH: string, required, resolved from feature.yaml
     - NEXTLENS_LANDSCAPE_STORE: string, default {docs_path}/landscape
     - NEXTLENS_IDEMPOTENCY_TTL_HOURS: number, default 24

4. **Dependencies**
   - Given: module dependencies are defined
   - When: dependencies section is populated
   - Then: includes:
     - feature-yaml-resolver (for docs path resolution)
     - bmad-constitution-resolver (for governance context)

5. **Module YAML Format**
   - Given: module.yaml is created
   - When: format is validated
   - Then: file is valid YAML with correct schema

**Dependencies:** None (module packaging)

**Definition of Done:**
- [ ] module.yaml is created with all required fields
- [ ] Schema is valid YAML
- [ ] All capabilities are listed
- [ ] Configuration variables are documented

---

### Story 11.2: Implement Module Help Registration

**Story ID:** 11.2  
**Epic:** EP11: BMAD Module Packaging & Registration  
**Priority:** High  
**Complexity:** Low  

**User Story:**

As a **NextLens module designer**, I want to **register capabilities in module-help.csv for BMAD discoverability**, so that **bmad-help can list and guide users**.

**Acceptance Criteria:**

1. **CSV File Creation**
   - Given: module-help.csv is created
   - When: format is determined
   - Then: file is CSV with columns:
     - command
     - category
     - description
     - entry_point
     - trigger_keywords

2. **Command Registration**
   - Given: nextlens capabilities are listed
   - When: CSV rows are populated
   - Then: includes:
     ```
     nextlens-new,command,"Create one Feature packet from top-down discovery context",commands/new.ts,"nextlens new,top-down bridge,feature packet,deterministic selection"
     nextlens-doctor,command,"Run non-mutating validation checks on packet or landscape",commands/doctor.ts,"nextlens doctor,validate packet,check landscape,doctor validation"
     nextlens-salmon,command,"Route correction signals through deduplication and impact classification",commands/salmon.ts,"nextlens salmon,route correction,deduplicate events,correction routing"
     ```

3. **Keyword Coverage**
   - Given: keywords are populated
   - When: bmad-help searches
   - Then: all relevant search terms are covered:
     - primary command name
     - feature name
     - key functionality
     - domain keywords

4. **CSV Format**
   - Given: module-help.csv is created
   - When: format is validated
   - Then: file is valid CSV (RFC 4180 compliant)

**Dependencies:** Story 11.1 (module identity)

**Definition of Done:**
- [ ] module-help.csv is created with correct format
- [ ] All commands are registered
- [ ] Keywords are comprehensive
- [ ] bmad-help can parse and display entries

---

### Story 11.3: Implement Marketplace Manifest

**Story ID:** 11.3  
**Epic:** EP11: BMAD Module Packaging & Registration  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens module designer**, I want to **create .claude-plugin/marketplace.json for installer and distribution compatibility**, so that **NextLens can be installed from package sources**.

**Acceptance Criteria:**

1. **Manifest Directory**
   - Given: marketplace manifest is created
   - When: directory structure is determined
   - Then: path is .claude-plugin/marketplace.json

2. **Manifest Content**
   - Given: manifest is created
   - When: content is populated
   - Then: JSON includes:
     - name: "NextLens Top-Down Bridge"
     - version: "1.0.0"
     - description: "Deterministic v1 top-down Feature packet bridge with validation and correction routing"
     - author: "NextLens Team"
     - repository: URL to GitHub repo
     - plugins: array of plugin definitions
     - keywords: array of searchable keywords
     - license: "MIT"

3. **Plugin Definitions**
   - Given: plugins are defined
   - When: entries are populated
   - Then: each includes:
     - id: "nextlens-new" (or other command)
     - name: "NextLens New Packet"
     - description: clear summary
     - skills: array of skill file paths

4. **Skills Path References**
   - Given: skill files are referenced
   - When: paths are verified
   - Then:
     - paths are relative to repository root
     - paths point to actual skill files
     - all referenced skills exist

5. **Manifest Format**
   - Given: marketplace.json is created
   - When: format is validated
   - Then: JSON is valid and parseable

**Dependencies:** Story 11.1 (module identity), Story 11.2 (command registration)

**Definition of Done:**
- [ ] .claude-plugin/marketplace.json is created
- [ ] All required fields are present
- [ ] Plugin definitions reference existing skills
- [ ] JSON is valid

---

### Story 11.4: Implement Create Module and Validate Module Gates

**Story ID:** 11.4  
**Epic:** EP11: BMAD Module Packaging & Registration  
**Priority:** High  
**Complexity:** Medium  

**User Story:**

As a **NextLens implementation team**, I want to **run Create Module (CM) and Validate Module (VM) gates before release**, so that **packaged output is verified correct**.

**Acceptance Criteria:**

1. **Create Module (CM) Execution**
   - Given: implementation is ready for packaging
   - When: CM command runs
   - Then: CM:
     - regenerates module.yaml from current capabilities
     - regenerates module-help.csv from current skills
     - regenerates .claude-plugin/marketplace.json from current skills
     - verifies all referenced skill files exist
     - reports results (success/failure)

2. **CM File Generation**
   - Given: CM runs
   - When: files are generated
   - Then: generated files include:
     - updated module.yaml
     - updated module-help.csv
     - updated marketplace.json
     - with checksums or timestamps for tracking

3. **Validate Module (VM) Execution**
   - Given: CM completes successfully
   - When: VM command runs
   - Then: VM:
     - validates module.yaml schema
     - validates module-help.csv format and consistency
     - validates marketplace.json completeness
     - checks that all skills in module-help.csv exist in manifest
     - checks that all manifest skills exist in repository
     - validates semantic versioning (major.minor.patch)
     - reports detailed findings or pass status

4. **VM Validation Results**
   - Given: VM validation completes
   - When: results are returned
   - Then:
     - Pass: all checks pass, module is approved for distribution
     - Fail with findings: issues listed with remediation guidance

5. **VM Issue Resolution**
   - Given: VM finds structural issues
   - When: issues are reported
   - Then:
     - release is blocked until resolved
     - remediation guidance is specific and actionable
     - user can re-run VM after fixes

6. **Integration with Release**
   - Given: module is ready to release
   - When: CM and VM gates pass
   - Then: module can proceed to distribution

**Dependencies:** Story 11.1 (module identity), Story 11.2 (module help registration), Story 11.3 (marketplace manifest)

**Definition of Done:**
- [ ] CM command is implemented and regenerates all files
- [ ] VM command is implemented and validates all checks
- [ ] Pass/fail results are reported clearly
- [ ] Integration with release process is established

---

## Summary

**Total Stories:** 41  
**Total Epics:** 11

**Story Distribution:**
- EP1: 3 stories
- EP2: 3 stories
- EP3: 3 stories
- EP4: 3 stories
- EP5: 3 stories
- EP6: 3 stories
- EP7: 4 stories
- EP8: 5 stories
- EP9: 4 stories
- EP10: 3 stories
- EP11: 4 stories

**Recommended Implementation Sequence:**
1. EP1, EP2, EP3, EP5 (foundational infrastructure)
2. EP4, EP6 (data processing and consistency)
3. EP7, EP8 (validation and emission)
4. EP9, EP10 (correction and audit)
5. EP11 (packaging)

**Estimated Effort:** 38 stories totaling **213 planned sprint points** across 13 sequenced sprints (~13-14 weeks at ~16 points per sprint).
