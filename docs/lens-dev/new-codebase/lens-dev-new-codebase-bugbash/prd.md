---
feature: lens-dev-new-codebase-bugbash
doc_type: prd
status: draft
goal: "Establish a closed-loop bug lifecycle for new-codebase: structured governance intake, automated batch fix pipeline, and deterministic status progression from New to Fixed."
key_decisions: []
open_questions: []
depends_on: []
blocks: []
updated_at: "2026-05-03T00:00:00Z"
stepsCompleted: [step-01-init, step-02-discovery, step-02b-vision, step-02c-executive-summary, step-03-success, step-04-journeys, step-05-domain, step-06-innovation, step-07-project-type, step-08-scoping, step-09-functional, step-10-nonfunctional, step-11-polish, step-12-complete]
inputDocuments: []
workflowType: prd
classification:
  projectType: bmad_workflow
  domain: general
  complexity: medium
  projectContext: greenfield
---

# Product Requirements Document - Bugbash

**Author:** crisweber2600
**Date:** 2026-05-03

## Executive Summary

Bugbash establishes a cleanroom bug lifecycle for new-codebase by turning Lens failures into structured governance artifacts and routing them into an automated express planning fix pipeline. The system introduces two dedicated prompts: an intake prompt that captures one bug per run as a canonical markdown artifact, and a fix prompt that processes all New bugs in batch to launch coordinated remediation work. The target users are Lens developers operating governance-first workflows who need reproducible bug capture, traceability from incident to fix feature, and deterministic status progression without manual bookkeeping overhead.

### What Makes This Special

The differentiator is the closed-loop workflow between operational failure signals and planning execution. Instead of treating chat logs as informal notes, Bugbash formalizes bug reports as first-class governance records containing structured frontmatter and full incident context (description plus pasted chat log). The fix flow then consumes those records as executable planning inputs, creates express-track fix features, and mutates bug metadata as work advances. On kickoff, bug artifacts transition from New to Inprogress and receive a featureId binding to the created fix feature. On completion runs for fixbugs, frontmatter is updated again to reflect completion state, ensuring governance remains the source of truth across intake, execution, and closure.

## Project Classification

- Project Type: bmad_workflow
- Domain: general
- Complexity: medium
- Project Context: greenfield

## Success Criteria

### User Success

- Lens developers can capture a bug in one run using intake prompt input: title, description, and pasted chat log.
- Each intake run creates exactly one governance bug artifact under a bugs folder with valid frontmatter.
- Developers can run fixbugs once to process all New bugs without manual per-bug routing.
- Developers can clearly trace each bug to its created fix feature through featureId linkage in bug frontmatter.

### Business Success

- Bug intake-to-work-start lead time is reduced to same-session turnaround for all New bugs.
- Governance bug queue becomes the canonical source for bug status and remediation ownership.
- Two-developer operation can sustain backlog triage without ad hoc spreadsheets or side tracking.
- New-codebase cleanroom policy is preserved across intake, planning, and fix execution.

### Technical Success

- Intake prompt enforces required frontmatter fields: title, description, status, featureId.
- Allowed status values are constrained to New, Inprogress, Fixed.
- Fixbugs flow creates express-track features for all New bug files in a single run.
- On fix start, bug frontmatter is atomically updated to Inprogress and featureId set.
- On fix completion run, corresponding bug frontmatter is updated to Fixed.
- Workflow only considers artifacts under new-codebase scope.

### Measurable Outcomes

- 100% of new bug entries created as markdown artifacts with complete required frontmatter.
- 100% of New bugs processed by fixbugs in batch mode per run.
- 100% of bugs moved New -> Inprogress during kickoff with non-empty featureId.
- 100% of completed fixbugs runs update associated bug status to Fixed.
- 0 cross-scope updates outside new-codebase artifacts.

## Product Scope

### MVP - Minimum Viable Product

- Intake prompt creates one bug file per run in governance bugs folder.
- Frontmatter schema enforced: title, description, status, featureId.
- Fixbugs prompt scans New bug files and creates express-track features in batch.
- Status transition logic implemented: New -> Inprogress at kickoff, then Fixed on completion run.
- featureId synchronization into each bug artifact implemented.

### Growth Features (Post-MVP)

- Duplicate bug detection and merge suggestions.
- Auto-generated bug title normalization and taxonomy tags.
- Priority/severity fields and triage queue ordering.
- Partial-failure recovery and retry semantics for batch fix execution.

### Vision (Future)

- Fully closed-loop bug lifecycle with automatic verification signals before marking Fixed.
- Cross-run analytics for bug throughput, cycle time, and recurring failure patterns.
- Integrated governance dashboards sourced directly from bug frontmatter state.

## User Journeys

### Journey 1: Primary User Success Path (Lens Developer, Intake)

Cris is in a live Lens session when a prompt flow fails in a way that blocks forward progress. Instead of opening side notes or losing context, Cris runs the bug intake prompt immediately.
In the opening scene, frustration is high because the issue is reproducible but fragile in memory. Cris pastes a concise description of what went wrong and the relevant chat log transcript.
During rising action, the intake prompt validates required frontmatter fields (title, description, status, featureId) and creates a single new bug artifact under the governance bugs folder scoped to new-codebase.
The climax is the moment the bug is stored as canonical governance state with status New and an empty or placeholder featureId awaiting fix assignment.
The resolution is confidence: the bug is no longer in chat history, it is now trackable, auditable, and ready for automated remediation flow.

### Journey 2: Primary User Edge Case (Lens Developer, Ambiguous/Noisy Incident)

Todd captures a bug where the chat log includes multiple failed attempts and partial fixes, making root cause unclear.
In the opening scene, Todd fears creating low-quality reports that pollute the queue.
Rising action: Todd still submits one bug artifact per run, but includes a focused summary and the complete transcript. The prompt preserves raw context while enforcing consistent schema.
At climax, the record is accepted despite noisy input because schema validity is separate from diagnosis quality.
Resolution: bug moves into structured triage/fix flow; developers can improve understanding later without losing original evidence.
Failure scenario handled: if required fields are missing, prompt blocks creation and asks for correction before write.

### Journey 3: Admin/Operations User (Fix Orchestrator, Batch Start)

A Lens developer acting as fix orchestrator starts a fixbugs run at the beginning of a remediation cycle.
Opening scene: multiple bug files are in New state; manual per-bug feature creation would be slow and inconsistent.
Rising action: fixbugs scans only new-codebase bug artifacts with status New, creates express-track features in batch, and binds each bug to its created feature by updating featureId.
Climax: each processed bug transitions atomically New -> Inprogress with traceable feature linkage.
Resolution: the bug queue now reflects active work-in-progress with deterministic ownership and execution context.
Failure scenario handled: partial batch failures should not silently skip updates; failed items remain New with explicit error reporting.

### Journey 4: Support/Troubleshooting User (Investigation and Traceability)

A second Lens developer is asked, "What is being fixed right now and why?"
Opening scene: without structured linkage, support would have to search chat transcripts and guess mapping to features.
Rising action: support reads governance bug artifacts, filters by Inprogress, and follows featureId to active fix features and expressplan outputs.
Climax: support can answer status and provenance quickly from governance truth, not tribal memory.
Resolution: escalation and communication become fast and accurate because bug state, source context, and fix feature are connected.

### Journey 5: API/Integration-Style Technical Consumer (Workflow Automation Consumer)

A workflow maintainer wants to build downstream automation that reacts to bug lifecycle state.
Opening scene: they need stable metadata transitions to trigger notifications and reporting.
Rising action: automation watches frontmatter status and featureId changes only in new-codebase scope.
Climax: completion run for fixbugs updates bug frontmatter to Fixed, enabling reliable done-state triggers.
Resolution: external automation remains simple because lifecycle transitions are explicit and standardized in markdown frontmatter.

### Journey Requirements Summary

- Intake must create exactly one bug artifact per run.
- Required frontmatter must include: title, description, status, featureId.
- Status enum must be constrained to New, Inprogress, Fixed.
- Fixbugs must batch-process all New bugs in one run.
- Fixbugs kickoff must update both status (to Inprogress) and featureId.
- Fix completion run must update status to Fixed for linked bugs.
- All reads/writes must be scoped to new-codebase artifacts only.
- Workflow must preserve raw chat log evidence while enabling structured lifecycle automation.
- Failure handling must keep unprocessed bugs in New with explicit error visibility.

## Domain-Specific Requirements

### Compliance and Regulatory

- No external regulated-industry compliance baseline is required for MVP.
- Governance integrity is mandatory: bug artifacts must remain canonical records and preserve change history through git.
- Workflow policy compliance is required: operations must remain scoped to new-codebase artifacts only.

### Technical Constraints

- Frontmatter schema enforcement is required on bug artifacts:
  - title
  - description
  - status (New, Inprogress, Fixed)
  - featureId
- Status transitions must be deterministic and valid:
  - intake sets New
  - fix kickoff sets Inprogress and assigns featureId
  - fix completion sets Fixed
- Batch safety requirement: fixbugs processes all New items, but failed items must remain New with explicit failure reporting.
- Idempotency requirement: reruns must not duplicate bug files or create conflicting feature mappings.

### Integration Requirements

- Governance repo bug storage path under new-codebase scope with a dedicated bugs folder.
- Feature initialization integration:
  - fixbugs must create express-track features via Lens feature init flow
  - created feature IDs must be written back to bug frontmatter
- Express planning integration:
  - fixbugs must run expressplan using each New bug artifact content (description plus chat log) as planning input.
- Completion integration:
  - completion run must resolve corresponding bug artifacts and update status to Fixed.

### Risk Mitigations

- Risk: cross-scope writes outside new-codebase.
  - Mitigation: hard path guard and explicit scope validation before any write.
- Risk: partial batch updates leave ambiguous state.
  - Mitigation: transactional per-item update logic and per-item outcome log.
- Risk: stale or missing featureId linkage.
  - Mitigation: require featureId write confirmation after feature creation; block status promotion when linkage fails.
- Risk: low-quality incident input reduces fix quality.
  - Mitigation: required description plus full chat log capture in intake prompt.

## Innovation & Novel Patterns

### Detected Innovation Areas

- Conversational bug intake as a first-class governance write path, where a chat transcript is converted into structured lifecycle data rather than freeform notes.
- Batch remediation orchestration that turns all New bug records into express-track feature work in one run.
- Bidirectional lifecycle sync: bug records drive feature creation, and feature completion updates bug state back to Fixed.
- Cleanroom enforcement at workflow level: processing is intentionally constrained to new-codebase artifacts only.

### Market Context & Competitive Landscape

- Existing issue trackers typically separate conversational debugging from planning orchestration; this workflow unifies capture, planning kickoff, and state transitions in one command surface.
- Many systems support status transitions, but fewer enforce deterministic frontmatter schema and feature linkage inside governance-native markdown artifacts.
- Differentiation is strongest in governance-as-source-of-truth plus workflow-native automation rather than external ticketing integration.

### Validation Approach

- Run controlled pilot with two Lens developers over a defined bug sample set.
- Measure end-to-end cycle metrics:
  - Intake completeness rate
  - Time from intake to Inprogress
  - Percentage of New bugs batch-processed per fix run
  - Completion accuracy (Fixed only when linked fix run completes)
- Validate idempotency through rerun tests and partial-failure scenarios.
- Validate scope guard by attempting mixed-scope inputs and confirming only new-codebase artifacts are mutated.

### Risk Mitigation

- Risk: automation over-updates incorrect bug records.
  - Mitigation: strict matching rules and per-item confirmation logs.
- Risk: batch run fails mid-stream and creates inconsistent states.
  - Mitigation: per-item transactional updates with explicit failure retention at New.
- Risk: false confidence from low-quality pasted chat logs.
  - Mitigation: required structured description plus full transcript retention for auditability.
- Risk: lifecycle drift between bug and feature entities.
  - Mitigation: mandatory featureId linkage checks before status promotion.

## BMAD Workflow Specific Requirements

### Project-Type Overview

Bugbash is a BMAD workflow that orchestrates lifecycle actions across governance artifacts and feature routing. It is command-invoked, but the primary value is workflow control, state progression, and deterministic multi-step execution.

### Workflow Architecture Considerations

- Intake workflow:
  - capture one bug per run
  - persist bug artifact under new-codebase bugs folder
  - enforce required frontmatter: title, description, status, featureId
- Fix workflow:
  - discover all bugs with status New in new-codebase scope
  - create one express-track feature for the batch of New bugs (N bugs → 1 feature)
  - run expressplan using each bug artifact body (description + chat log)
  - update bug frontmatter to Inprogress and assign featureId
- Completion workflow:
  - on fix completion, resolve linked bug artifacts
  - update status to Fixed

### State and Transition Model

- Allowed status values: New, Inprogress, Fixed
- Valid transitions:
  - intake: unset/new artifact -> New
  - fix kickoff: New -> Inprogress (+ featureId assignment)
  - completion: Inprogress -> Fixed
- Transition safety:
  - per-item validation and failure isolation
  - failed records remain in prior valid state with explicit errors

### Scope and Policy Controls

- Hard scope restriction: only new-codebase artifacts are eligible
- Path guard required before every read/write operation
- No cross-domain or cross-service mutation during bug intake/fix workflows

### Implementation Considerations

- Idempotent workflow reruns
- Batch result reporting per bug record
- Stable frontmatter read/write handling
- Git-traceable lifecycle mutations in governance repo

## Project Scoping and Phased Development

### MVP Strategy & Philosophy

- MVP Approach: workflow-validating MVP focused on fastest validated learning.
- Core proof target: prove deterministic bug lifecycle automation from intake to completion state update.
- Resource Requirements: 2 Lens developers.
- Delivery style: lean vertical slices across intake, fix orchestration, and completion update path.

### MVP Feature Set (Phase 1)

Core User Journeys Supported:

- Single-run bug intake from failure description + pasted chat log.
- Batch fix kickoff processing all New bug artifacts at once.
- Completion run updating linked bug artifacts to Fixed.
- Operational traceability journey via status + featureId linkage.

Must-Have Capabilities:

- New-codebase-only scope guard on all reads and writes.
- Governance bugs folder storage model.
- Required frontmatter schema:
  - title
  - description
  - status
  - featureId
- Valid status enum and transition logic:
  - New
  - Inprogress
  - Fixed
- Batch run behavior:
  - discover all New bugs
  - create express-track feature per bug
  - run expressplan with bug artifact content
  - update status to Inprogress and set featureId
- Completion behavior:
  - resolve linked bugs
  - set status to Fixed
- Failure handling:
  - per-item result reporting
  - failed items remain in prior valid state

### Post-MVP Features

Phase 2 (Post-MVP):

- Priority/severity frontmatter fields and triage ordering.
- Duplicate bug detection and merge recommendations.
- Partial rerun controls and retry flags for failed items.
- Structured machine-readable run summaries for automation hooks.

Phase 3 (Expansion):

- Bug lifecycle analytics dashboard (cycle time, reopen rate, recurring failure patterns).
- Policy-as-code validation for frontmatter and scope rules.
- Intelligent root-cause clustering from chat transcripts.
- Auto-verification gates before status promotion to Fixed.

### Risk Mitigation Strategy

Technical Risks:

- Risk: partial batch mutation inconsistency.
- Mitigation: per-item transactional updates and strict idempotency checks.

Market/Adoption Risks:

- Risk: developers bypass intake and keep ad hoc bug notes.
- Mitigation: make intake frictionless and tie fixbugs processing strictly to governance artifacts.

Resource Risks:

- Risk: 2-developer bandwidth bottlenecks.
- Mitigation: keep MVP narrow, enforce phase boundaries, defer non-core enhancements to Phase 2+.

## Functional Requirements

### Bug Intake Management

- FR1: Lens developers can create a single bug record per intake run.
- FR2: Lens developers can provide a bug title during intake.
- FR3: Lens developers can provide a bug description during intake.
- FR4: Lens developers can provide a pasted chat log during intake.
- FR5: The workflow can persist each accepted bug record as a markdown artifact in the governance bugs folder scoped to new-codebase.
- FR6: The workflow can reject intake when required frontmatter fields are missing.

### Bug Lifecycle State Management

- FR7: The workflow can assign status New to newly created bug artifacts.
- FR8: The workflow can enforce allowed bug statuses as New, Inprogress, and Fixed.
- FR9: The workflow can update status from New to Inprogress when fix work is initiated.
- FR10: The workflow can update status from Inprogress to Fixed when fix completion is confirmed.
- FR11: The workflow can prevent invalid status transitions that violate lifecycle rules.
- FR12: The workflow can preserve prior valid status when a transition operation fails.

### Feature Linkage and Planning Orchestration

- FR13: The fix workflow can discover all bug artifacts with status New within new-codebase scope.
- FR14: The fix workflow can create an express-track feature for each discovered New bug.
- FR15: The fix workflow can execute expressplan using each bug artifact's description and chat log content.
- FR16: The fix workflow can write featureId linkage into bug frontmatter after feature creation.
- FR17: The workflow can prevent promotion to Inprogress when featureId linkage is not established.
- FR18: The completion workflow can resolve linked bug records by featureId before setting Fixed.

### Batch Execution and Operational Control

- FR19: Lens developers can run fixbugs as a single batch operation across all eligible New bugs.
- FR20: The workflow can process bugs independently within a batch run.
- FR21: The workflow can report per-bug success or failure outcomes for each batch run.
- FR22: The workflow can continue processing remaining bugs when one bug fails in batch.
- FR23: The workflow can support idempotent reruns without duplicating bug artifacts.
- FR24: The workflow can leave failed or unprocessed bugs in a recoverable state for subsequent reruns.

### Scope and Governance Safety

- FR25: The workflow can restrict all reads and writes to new-codebase artifacts only.
- FR26: The workflow can block cross-domain or cross-service mutation attempts.
- FR27: The workflow can preserve governance artifact history through normal git-traceable file updates.
- FR28: The workflow can keep bug records as canonical lifecycle state for intake, active fix work, and completion.

### Team and Workflow Collaboration

- FR29: Two Lens developers can operate the workflow concurrently without changing the lifecycle model.
- FR30: Developers can inspect bug records to determine current status and linked feature work.
- FR31: Developers can identify in-progress remediation by reading status plus featureId from bug artifacts.
- FR32: Developers can distinguish completed fixes from active fixes using frontmatter state only.

## Non-Functional Requirements

### Performance

- Intake run can validate and persist a single bug artifact within 5 seconds under normal repository conditions.
- Fixbugs batch run can produce a per-item outcome summary for all discovered New bugs in a single execution.
- Frontmatter update operations can complete without blocking unrelated bug records in the same batch.

### Security

- Workflow can prevent writes outside the new-codebase scope through mandatory path validation before mutation.
- Bug artifacts can preserve chat-log content without exposing data outside governance-authorized repositories.
- Only authorized Lens developer workflows can mutate bug status or featureId fields.
- Feature linkage mutations can be auditable via git history for each changed bug artifact.

### Reliability

- Batch processing can isolate per-item failures so one failed bug does not abort successful updates for other eligible bugs.
- Failed items can remain in their prior valid lifecycle state (typically New) with explicit error reporting.
- Reruns can be idempotent for already-processed items unless state has changed intentionally.
- Completion updates can require successful bug-to-feature resolution before status promotion to Fixed.

### Integration

- Fix workflow can integrate with feature creation using express track and consume returned featureId values.
- Fix workflow can integrate with expressplan execution using each bug artifact's description and chat log body.
- Completion workflow can integrate with fix completion signals to update linked bug records to Fixed.

### Scalability

- MVP can support concurrent operation by two Lens developers without lifecycle corruption.
- Workflow design can scale to larger bug sets through repeatable batch execution and deterministic filtering by status New.
