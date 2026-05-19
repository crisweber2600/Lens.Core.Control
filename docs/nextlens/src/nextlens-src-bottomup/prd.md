---
feature: nextlens-src-bottomup
doc_type: prd
status: in-review
goal: "Define the Bottom-Up LENS feature packet creator MVP."
key_decisions: []
open_questions: []
depends_on:
  - docs/nextlens/src/nextlens-src-bottomup/product-brief.md
  - docs/nextlens/src/nextlens-src-bottomup/research.md
  - docs/nextlens/src/nextlens-src-bottomup/brainstorm.md
inputDocuments:
  - docs/nextlens/src/nextlens-src-bottomup/product-brief.md
  - docs/nextlens/src/nextlens-src-bottomup/research.md
  - docs/nextlens/src/nextlens-src-bottomup/brainstorm.md
documentCounts:
  briefCount: 1
  researchCount: 1
  brainstormingCount: 1
  projectDocsCount: 0
stepsCompleted:
  - step-01-init
  - step-02-discovery
  - step-02b-vision
  - step-02c-executive-summary
  - step-03-success
  - step-04-journeys
  - step-05-domain
  - step-06-innovation
  - step-07-project-type
  - step-08-scoping
  - step-09-functional
  - step-10-nonfunctional
  - step-11-polish
  - step-12-complete
classification:
  projectType: developer_tool
  domain: general / product-development tooling
  complexity: medium
  projectContext: greenfield feature within NextLens
workflowType: prd
updated_at: 2026-05-19T00:00:00Z
---

# Product Requirements Document - Bottom-Up LENS Feature Packet Creator

**Author:** BMad
**Date:** 2026-05-19

## Executive Summary

Bottom-Up LENS Feature Packet Creator is a NextLens capability that lets an operator capture one locally valuable feature before a system, domain, capability, roadmap, or architecture is known. The feature solves the trust problem in early product discovery: users often know one useful thing worth building, but current planning flows pressure them to invent surrounding structure before evidence exists. This reduces the cost of starting governed work by allowing one honest feature to enter the Lens/BMAD path without staging a premature planning hierarchy.

The MVP creates a governed bottom-up feature packet for exactly one selected feature candidate. It validates local sufficiency, records included and explicitly out-of-scope work, preserves assumptions as unpromoted, requires human preview confirmation, and writes a machine-readable receipt proving no adjacency, pressure, promotion, Salmon, Living Landscape, or Derived Graph side effects occurred. If the creator cannot prove the one-candidate constraint or its non-effects, it fails closed and writes nothing.

Target users are Lens/BMAD operators, individual builders, and early product teams who need governed feature capture without false topology. Success means a user can start honestly from one feature, preserve provenance, and prepare future BMAD handoff without turning uncertainty into authoritative structure.

### What Makes This Special

The differentiator is trust-preserving restraint. Most planning tools reward early categorization; this feature rewards evidence discipline. It treats the first useful feature as archived work evidence, not as proof that a larger system exists.

The core insight is that bottom-up planning starts with non-inference. Packet creation must prove what it did not create as clearly as what it captured. Relationships, repeated pressure, promotion candidates, Salmon routing, Landscape updates, and Graph projections emerge only from later evidence-bearing phases and human gates.

The user delight moment is relief: “I only know this one feature” is accepted as valid input, and NextLens responds by capturing it safely without pretending the user knows more.

## Project Classification

- **Project Type:** Developer tool
- **Domain:** General / product-development tooling
- **Complexity:** Medium
- **Project Context:** Greenfield feature within NextLens
- **Primary Artifact:** Schema-validatable bottom-up feature packet with stable identity, provenance, and non-effects receipt
- **Primary Risk:** Premature topology creation or downstream inference from insufficient evidence

## Success Criteria

### User Success

- A user can start from raw context containing one useful feature without first defining a system, domain, capability, roadmap, or architecture.
- The user sees candidate choices when raw context contains multiple possible features and can select exactly one.
- The user can complete packet creation only after local value, problem, acceptance criteria, known constraints, assumptions, included scope, and explicit out-of-scope are captured.
- The user sees a preview before write that clearly indicates packet validity status, the intended write target, and the downstream effects that will not occur.
- The user receives a clear machine-readable receipt showing the packet was written and downstream side effects were not emitted.

### Business Success

- Bottom-up entry reduces the friction of starting governed Lens/BMAD work for early feature ideas.
- Operators can bring feature-first work into NextLens without polluting Living Landscape or Derived Graph truth.
- The feature establishes a reusable trust pattern for later bottom-up BMAD handoff, adjacency detection, pressure detection, and promotion workflows.
- Within the first release cycle, at least one internal dogfood run creates a valid packet without manual topology cleanup.

### Technical Success

- Packet output is schema-validatable and includes stable identity, source mode, selected feature, scope, constraints, unpromoted assumptions, provenance, non-effects receipt, and null/unpromoted topology fields.
- Creation fails closed when more than one candidate is selected, required local context is missing, explicit out-of-scope is empty, non-inference rules are absent, preview confirmation is missing, or non-effects cannot be proven.
- Packet validity and BMAD readiness are separate validator states.
- Packet creation writes only the packet and receipt in the approved archive/staging path.
- The non-effects receipt is machine-readable and independently testable against the filesystem changes from the run.
- No adjacency records, pressure signals, promotion candidates, Salmon signals, Landscape updates, or Graph updates are written during initial packet creation.

### Measurable Outcomes

- 100% of packets include exactly one selected feature.
- 100% of packets include at least one included-scope item and one explicit out-of-scope item.
- 100% of previews show valid/invalid status and non-effects before write.
- 100% of packet creation runs emit a non-effects receipt.
- 0 packet creation runs write adjacency, pressure, promotion, Salmon, Landscape, or Graph artifacts.
- Validator test coverage includes one happy path and fail-closed cases for multi-candidate selection, missing local value, missing explicit out-of-scope, missing confirmation, attempted side-effect writes, denied side-effect paths, and false receipt claims.
- Internal dogfood confirms the user can create a valid packet from raw feature context without inventing topology.

## Product Scope

### MVP - Minimum Viable Product

- Bottom-up packet creation entry surface.
- Raw-context intake and candidate-selection gate.
- Exactly-one selected feature enforcement.
- Sufficiency prompts for local value, problem, actor/user, acceptance criteria, constraints, and assumptions.
- Scope safety gate for included scope and explicit out-of-scope.
- Non-inference rules preventing system/domain/capability/roadmap/architecture invention.
- Human preview confirmation before writing.
- Schema-valid feature packet output.
- Machine-readable non-effects receipt and fail-closed validation.
- Separate BMAD readiness status field or validator result.

### Growth Features (Post-MVP)

- BMAD handoff from a validated packet.
- Archive capture conventions for executed bottom-up work.
- Read-only packet status reporting for Auspex-style consumers.
- Adjacency detection based on later artifact evidence.
- Repeated pressure detection and promotion suggestion workflows.
- Salmon routing when implementation reveals assumption changes.

### Vision (Future)

- A complete bottom-up lane where feature-first archive evidence can accumulate, relationships can be detected before structure, and human-gated promotion can gradually update the Living Landscape and Derived Graph without compromising source-of-truth boundaries.

## User Journeys

### Journey 1: Primary User Success Path — Ari Captures One Honest Feature

Ari is an individual builder with a concrete feature idea but no reliable system shape yet. They know the feature should solve a real problem, but they do not know what domain, capability, roadmap, or architecture it belongs to.

Ari opens the bottom-up packet creator and pastes raw context. NextLens identifies candidate feature slices and shows Ari the choices without ranking them as a roadmap. Ari selects one candidate. NextLens asks only for local sufficiency: the actor, problem, useful outcome, acceptance criteria, constraints, assumptions, included scope, and explicit out-of-scope.

The turning point comes at preview. Ari sees that the packet is valid, that it contains one feature, and that no system/domain/capability/roadmap/architecture will be inferred. Ari confirms. NextLens writes the feature packet and a machine-readable non-effects receipt.

Ari’s new reality: the feature is governed and ready for later validation without pretending the surrounding product structure exists.

### Journey 2: Primary User Edge Case — Morgan Tries To Capture A System

Morgan describes a broad product concept with multiple possible features. NextLens detects more than one candidate and refuses to write a packet until Morgan selects exactly one. Morgan tries to leave out-of-scope empty because “we will figure that out later.” NextLens blocks creation with friendly, corrective copy: the problem is not that Morgan is wrong; the product is saving Morgan from turning early uncertainty into false structure.

Morgan narrows the selected feature, records deferred candidates as unranked notes, and adds explicit out-of-scope boundaries. The preview now shows a valid packet and confirms no downstream topology will be created.

Morgan’s new reality: NextLens preserved the useful feature while preventing a speculative system from becoming false truth.

### Journey 3: Lens/BMAD Operator — Riley Audits Packet Safety

Riley is a Lens/BMAD operator reviewing a bottom-up packet before future BMAD handoff. Riley needs to know whether the packet is valid archive evidence and whether it is BMAD-ready.

Riley opens the packet and receipt. The packet exposes stable identity, source mode, selected feature, scope, assumptions, provenance, and topology fields that remain null or unpromoted. The receipt shows no adjacency, pressure, promotion, Salmon, Landscape, or Graph outputs occurred. Riley runs or inspects validation results and sees packet validity separate from BMAD readiness. A packet may be `packet_valid=true` while `bmad_ready=false` because capture safety and downstream execution sufficiency are separate gates.

Riley’s new reality: they can trust the packet as historical evidence and decide separately whether more context is needed before BMAD execution.

### Journey 4: Support/Troubleshooting — Quinn Investigates A Side-Effect Claim

Quinn receives a report that packet creation may have updated a graph artifact. Quinn checks the machine-readable receipt and verifies it against reproducible run metadata and filesystem changes from the run. The receipt claims no graph updates, but the verification path detects a changed graph file.

NextLens marks the run invalid because a false non-effects claim is a hard failure. The packet creation result is not accepted, and the issue is routed as a defect rather than silently treating the packet as valid.

Quinn’s new reality: non-effects are testable claims, not trust-me prose.

### Journey 5: Read-Only Reporting Consumer — Auspex Shows Packet Status

A stakeholder views bottom-up work through Auspex-style reporting. The report can show packet status, provenance, validity, BMAD readiness, and whether promotion has been suggested later. It cannot mutate the packet, promote assumptions, or create Landscape/Graph truth.

The stakeholder sees that the feature is captured as archive evidence only. They understand that absence of promotion is intentional, not missing work.

Auspex’s new reality: reporting increases visibility without gaining mutation authority.

### Journey Requirements Summary

These journeys reveal requirements for:

- Raw-context intake and candidate extraction.
- Exactly-one candidate selection.
- Local sufficiency prompts.
- Required included-scope and explicit out-of-scope capture.
- Human preview confirmation with validity and non-effects.
- Schema-valid packet output.
- Machine-readable non-effects receipt.
- Fail-closed validation for multi-candidate input, missing scope boundaries, and false non-effects claims.
- Separate packet validity and BMAD readiness states.
- Reproducible receipt verification from run metadata.
- Read-only reporting fields that cannot mutate topology.

## Domain-Specific Requirements

### Compliance & Governance

- The packet creator must follow Lens write-scope rules: planning artifacts are written only under the resolved control-repo docs/archive path, not directly into governance mirrors or release clone surfaces.
- Governance updates, if any later become necessary, must occur only through approved Lens orchestration boundaries.
- Packet creation must preserve separation between Feature Archive evidence, Living Landscape truth, and Derived Graph projections. Archive is historical evidence, Landscape is promoted interpretation, and Graph is a derived projection/cache.
- Human confirmation is required before packet write; human acceptance is required for any future promotion.

### Technical Constraints

- The packet must be schema-validatable and deterministic enough for repeatable, idempotent validation.
- The non-effects receipt must be machine-readable and independently verifiable.
- The creator must fail closed when it cannot prove exactly-one candidate selection, explicit scope boundaries, confirmation, or absence of side effects.
- Fail-closed user-facing copy must explain why the block exists and how it protects the user from false topology, not merely state that validation failed.
- Packet validity and BMAD readiness must be represented as separate states.
- Topology fields must remain null/unpromoted during MVP packet creation unless a later human-gated promotion flow changes them.

### Integration Requirements

- The packet should preserve provenance links to source context and planning artifacts.
- Future BMAD handoff must consume the packet without requiring invented system/domain/capability/roadmap/architecture context.
- Future reporting consumers may read packet status/provenance/readiness but must not mutate packet state or topology.
- Future Salmon, adjacency, pressure, promotion, Landscape, and Graph integrations are explicitly post-MVP.

### Risk Mitigations

- **Premature topology risk:** enforce non-inference rules and null/unpromoted topology fields.
- **Scope creep risk:** require included scope, explicit out-of-scope, and deferred candidates as unranked notes.
- **False receipt risk:** verify non-effects claims against reproducible run metadata and changed files.
- **BMAD overreach risk:** keep BMAD readiness separate from packet validity and block handoff when downstream context is insufficient.
- **Reporting mutation risk:** expose read-only status fields only.

## Innovation & Novel Patterns

### Detected Innovation Areas

- **Feature-first governance:** The product allows a governed feature artifact to exist before system/domain/capability/roadmap/architecture context is known.
- **Non-effects as a product artifact:** The receipt proves absence of downstream topology mutation, not only successful packet creation. The receipt must be independently verifiable against run metadata and changed files.
- **Archive-before-landscape model:** The packet is historical evidence first; promoted interpretation and derived graph projection are separate later concerns. The Derived Graph is a cache/projection, not source truth.
- **Separated readiness gates:** Packet validity and BMAD readiness are intentionally independent states.
- **Human-gated emergence:** Structure emerges from repeated evidence and explicit acceptance, not from initial capture.

### Market Context & Competitive Landscape

This is not competing with generic project intake or issue creation. The differentiator is governed restraint: normal tools capture work items; this feature captures one feature while preventing unearned topology. The closest alternatives are manual ADRs, issue templates, product briefs, or schema-based intake forms, but those do not normally include machine-verifiable non-effects, unpromoted assumptions, and explicit topology separation.

If users do not understand “bottom-up” terminology, the UX should frame the entry point as “Start from one feature” and introduce Bottom-Up LENS language only after the concept is clear.

### Validation Approach

- Dogfood with raw context that contains one clean feature.
- Dogfood with raw context that contains multiple candidate features.
- Verify packet validity and BMAD readiness can diverge.
- Verify non-effects receipt against run metadata and changed files.
- Verify downstream consumers cannot treat packet existence as promotion.

### Risk Mitigation

- If feature-first capture is too permissive, strengthen sufficiency and scope gates.
- If users are confused by blocking behavior, improve friendly explanation copy.
- If BMAD expects hierarchy too early, make BMAD readiness fail separately without invalidating packet capture.
- If reporting implies promotion, restrict report language and expose only read-only packet states.

## Developer Tool Specific Requirements

### Project-Type Overview

The Bottom-Up LENS Feature Packet Creator is a developer/product-operator workflow tool inside NextLens. Its primary job is not code generation; it is governed artifact creation with strict validation, provenance, and non-effects guarantees.

### Technical Architecture Considerations

- The feature should expose a clear operator entry point such as “Start from one feature” / Bottom-Up LENS packet creation.
- The creator should operate deterministically from raw context, user answers, and resolved Lens context.
- Feature identity and scope must resolve from explicit input, session feature context, or `feature.yaml`; the creator must not infer feature identity from branch name, open files, or current working directory.
- The packet schema should be versioned and suitable for automated validation.
- The output path should be explicit, governed, and constrained to the approved archive/staging location.
- The command should produce reproducible run metadata for receipt verification.
- The creator should be scriptable enough for tests and future automation, while still supporting interactive operator confirmation.

### Language Matrix

- MVP implementation language should follow existing NextLens/Lens runtime conventions.
- Packet schema should use a portable data format such as JSON or YAML, with explicit schema/version fields.
- Generated packets and receipts should be readable by humans and machines.

### Installation Methods

- MVP should integrate into the existing Lens command/prompt surface rather than requiring a separate installation channel.
- No new package distribution mechanism is required for the MVP unless implementation architecture later demands it.

### API Surface

- `preview`: derive candidate packet content and show validity/non-effects before write.
- `validate`: validate a packet or preview without mutating state.
- `write`: write the packet only after confirmation and all hard constraints pass.
- `verify-receipt`: verify the non-effects receipt against reproducible run metadata and changed files.
- Duplicate packet attempts for the same candidate should be detected and require an explicit operator decision before any write.
- Report separate packet validity and BMAD readiness states.

### Code Examples

- Documentation must include an example valid packet with one selected feature, explicit scope, unpromoted assumptions, null topology, and non-effects receipt.
- Documentation must include an example invalid multi-candidate input.
- Documentation must include an example invalid missing out-of-scope.
- Documentation must include an example `packet_valid=true` / `bmad_ready=false` result.

### Migration Guide

- No existing bottom-up packets require migration in the MVP.
- Existing top-down planning artifacts remain unchanged.
- Future migration work, if needed, belongs to a separate archive-conversion feature.

### Implementation Considerations

- Tests should verify both artifact content and absence of forbidden writes.
- Error messages should be actionable and user-protective.
- Reporting integrations must consume read-only fields only.
- Future BMAD handoff should be implemented as a separate capability after packet validity is proven.

## Project Scoping & Phased Development

### MVP Strategy & Philosophy

**MVP Approach:** Trust-preserving artifact MVP. The smallest useful product is a packet creator that can safely capture one feature, validate it, and prove non-effects.

**Resource Requirements:** One Lens/NextLens implementer plus reviewer coverage for schema validation, filesystem/write-scope behavior, and UX copy. QA/test review is needed because absence-of-side-effects is a core product claim.

### MVP Feature Set (Phase 1)

**Core User Journeys Supported:**

- Ari captures one honest feature.
- Morgan is blocked from capturing a system and guided to one feature.
- Riley audits packet validity and BMAD readiness.
- Quinn verifies receipt claims against run metadata.

**Must-Have Capabilities:**

- Entry point framed as “Start from one feature” / Bottom-Up LENS packet creation.
- Raw-context intake.
- Candidate identification from raw context with user confirmation.
- Exactly-one selection enforcement.
- Local sufficiency capture: actor/user, problem, useful outcome, acceptance criteria, constraints, assumptions.
- Scope safety capture: included scope, explicit out-of-scope, deferred candidates as unranked notes.
- Non-inference rules for system/domain/capability/roadmap/architecture.
- Preview with validity, write target, and non-effects.
- Human confirmation before write.
- Versioned feature packet schema.
- Packet validation without mutation.
- Packet write only to approved archive/staging path.
- Machine-readable non-effects receipt and reproducible run metadata.
- Receipt verification.
- Separate `packet_valid` and minimal `bmad_ready` status with reasons.
- Packet metadata states such as `draft`, `confirmed`, `valid`, `bmad-ready`, `executed`, and `archived`, represented as metadata rather than a full MVP workflow.
- Tests for happy path, fail-closed validation, denied side-effect paths, false receipt claims, and golden fixtures for valid and invalid packet schema/validator behavior.
- Required valid/invalid packet examples in docs.

### Post-MVP Features

**Phase 2 (Post-MVP):**

- BMAD handoff from valid packets.
- Read-only packet status reporting.
- Archive capture conventions for executed bottom-up work.
- Additional UX affordances for candidate comparison and deferred candidates.

**Phase 3 (Expansion):**

- Evidence-based adjacency detection.
- Repeated pressure detection.
- Promotion candidate workflows.
- Human-gated Living Landscape updates.
- Derived Graph rebuild/projection support.
- Salmon routing for implementation-truth corrections.
- Broader Auspex reporting integration.

### Risk Mitigation Strategy

**Technical Risks:** The hardest claim is proving absence of forbidden side effects. Mitigate with constrained write paths, changed-file tracking, machine-readable receipts, and receipt verification tests.

**Market/Product Risks:** Users may not understand bottom-up terminology or may try to capture systems. Mitigate with “Start from one feature” language and friendly fail-closed copy.

**Resource Risks:** If scope must shrink further, keep only raw-context intake, exactly-one feature selection, explicit scope/out-of-scope, preview confirmation, packet schema, write, non-effects receipt, and minimal BMAD readiness status. Defer reporting fields if necessary, but do not defer non-effects receipt.

## Functional Requirements

### Bottom-Up Entry and Context Intake

- FR1: Operators can start a bottom-up packet creation flow from a clear “Start from one feature” entry point.
- FR2: Operators can provide raw feature context for packet creation.
- FR3: The system can resolve required Lens feature context from explicit input, session context, or `feature.yaml`.
- FR4: The system can reject context resolution attempts that rely on branch name, open file, or current working directory inference.
- FR5: Operators can see the resolved feature context, output path, and write scope before packet write.

### Candidate Identification and Selection

- FR6: The system can identify possible feature candidates from raw context for operator confirmation.
- FR7: Operators can select exactly one candidate for packet creation.
- FR8: The system can block packet creation when no candidate is selected.
- FR9: The system can block packet creation when more than one selected candidate remains.
- FR10: Operators can record deferred candidates as unranked notes that do not become roadmap or topology.

### Local Sufficiency and Scope Safety

- FR11: Operators can capture the selected feature’s actor or user.
- FR12: Operators can capture the selected feature’s problem and locally useful outcome.
- FR13: Operators can capture acceptance criteria for the selected feature.
- FR14: Operators can capture known constraints.
- FR15: Operators can capture assumptions as unpromoted.
- FR16: Operators can capture included scope.
- FR17: Operators can capture explicit out-of-scope.
- FR18: The system can block packet creation when included scope is missing.
- FR19: The system can block packet creation when explicit out-of-scope is missing.
- FR20: The system can explain fail-closed blocks in user-protective language.

### Packet Preview, Confirmation, and Write

- FR21: Operators can preview the packet before it is written.
- FR22: The preview can show packet validity status.
- FR23: The preview can show the intended write target.
- FR24: The preview can show the non-effects that packet creation will preserve.
- FR25: Operators can revise packet inputs after preview without writing a packet.
- FR26: Operators can provide an explicit confirmation action or token before packet creation.
- FR27: The system can write a packet only after confirmation and passing validation.
- FR28: The system can write the packet only to the approved archive/staging path.
- FR29: The system can prevent packet creation from writing to governance mirror, Landscape, Graph, or other forbidden paths in the MVP.
- FR30: The system can fail closed and write nothing when hard validation constraints fail.
- FR54: Operators can run preview or dry-run validation without writing a packet.
- FR55: The system can detect duplicate packet attempts for the same candidate and require explicit operator resolution.

### Packet Schema and Validation

- FR31: The system can validate a packet without mutating state.
- FR32: The system can enforce packet schema version and source mode.
- FR33: The system can enforce required packet fields for identity, selected feature, scope, constraints, assumptions, provenance, receipt, and topology.
- FR34: The system can keep topology fields null or unpromoted during MVP packet creation.
- FR35: The system can report `packet_valid` status with validation reasons.
- FR36: The system can report minimal `bmad_ready` status with reasons independently from packet validity.
- FR37: Operators can see when a packet is valid but not BMAD-ready.
- FR56: The system can preserve raw user wording alongside normalized packet fields for auditability.
- FR57: The system can distinguish intentionally unknown values from missing or empty required values during validation.
- FR58: The system can expose packet lifecycle metadata states without treating them as Living Landscape truth.

### Non-Effects Receipt and Verification

- FR38: The system can emit a machine-readable non-effects receipt for each packet creation run.
- FR39: The receipt can state whether adjacency records, pressure detection, promotion candidates, Salmon signals, Landscape updates, and Graph updates were emitted.
- FR40: The system can record reproducible run metadata for receipt verification.
- FR41: Operators or support users can verify a receipt against run metadata and changed files.
- FR42: The system can mark a run invalid when receipt claims conflict with observed changes.
- FR43: The system can block or fail runs that attempt forbidden side-effect writes.

### Documentation and Examples

- FR44: Operators can access documentation that explains Bottom-Up LENS as “Start from one feature.”
- FR45: Operators can access a valid packet example.
- FR46: Operators can access invalid packet examples for multi-candidate input and missing explicit out-of-scope.
- FR47: Operators can access an example showing `packet_valid=true` and `bmad_ready=false`.
- FR48: Downstream planning users can trace packet requirements to source inputs, provenance, selected-candidate rationale, and decision rationale.
- FR49: Maintainers can preserve golden valid and invalid packet fixtures for schema and validator behavior.

### Read-Only Reporting and Future Handoff Support

- FR50: Reporting consumers can read packet status, provenance, validity, and BMAD readiness.
- FR51: Reporting consumers cannot mutate packet state or promote topology.
- FR52: Future BMAD handoff can consume packet validity and BMAD readiness states.
- FR53: Future promotion, adjacency, pressure, Salmon, Landscape, and Graph workflows can distinguish archive evidence from promoted truth.

## Non-Functional Requirements

### Reliability and Integrity

- Packet validation must be idempotent: repeated validation of unchanged input returns the same result and does not mutate state.
- Packet creation must be atomic from the operator’s perspective: failed validation or failed write leaves no accepted packet result.
- The system must fail closed when any hard safety condition cannot be proven.
- Unsupported packet schema versions must fail with actionable guidance rather than being accepted or silently upgraded.
- Validation must distinguish intentionally unknown values from missing or empty required values.
- Non-effects receipt verification must detect mismatches between receipt claims and observed changed files.
- Packet validity and BMAD readiness status must be reproducible from saved packet/run metadata.

### Security and Write-Scope Safety

- Packet creation must not write to governance mirror paths, release clone paths, Living Landscape paths, Derived Graph paths, or Salmon signal paths during MVP creation.
- Packet creation must constrain writes to the approved archive/staging path resolved from governed context.
- The system must avoid inferring feature identity from branch name, open editor file, or current working directory.
- Run metadata and receipts must not expose secrets, credentials, tokens, or sensitive local environment values.
- Future integrations must preserve read-only access for reporting consumers.

### Performance

- Preview and validation should complete quickly enough for interactive use on typical packet input sizes.
- Receipt verification should scale with the changed-file set for a run, not require scanning unrelated repository history.
- Validation failures should return actionable messages in the same interaction rather than requiring log inspection.

### Usability and Accessibility

- The entry point should use “Start from one feature” language where “Bottom-Up LENS” is not yet meaningful to users.
- Fail-closed messages must explain the protective reason for the block and the next corrective action.
- Documentation examples for fail-closed cases must include both “why blocked” and “how to fix.”
- Preview content must be understandable without requiring the user to know Lens topology internals.
- Documentation examples must include valid and invalid packets.
- UX copy should distinguish “valid packet” from “BMAD-ready packet” in plain language.

### Integration Boundaries

- MVP packet creation must not trigger BMAD execution, adjacency detection, pressure detection, promotion, Salmon routing, Landscape updates, or Graph updates.
- Future consumers must treat packet artifacts as Feature Archive evidence until a separate human-gated promotion occurs.
- Reporting consumers must not mutate packet state or derived topology.
- BMAD readiness output must be consumable by future handoff workflows without requiring them to reinterpret packet validity.
- Resume behavior for interrupted packet intake is post-MVP unless later implementation planning identifies it as necessary for safe packet creation.

### Testability

- The implementation must include golden fixtures for valid and invalid packets.
- Tests must cover happy path, multi-candidate failure, missing local value failure, missing explicit out-of-scope failure, missing confirmation failure, forbidden write attempt, denied side-effect path, false receipt claim, and `packet_valid=true` / `bmad_ready=false`.
- Tests must verify both written artifacts and absence of forbidden writes.
- Receipt verification must be automatable in CI or an equivalent repeatable validation command.