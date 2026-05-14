---
feature: nextlens-src-topdownlens
doc_type: sprint-plan
status: draft
track: express
phase: expressplan
inputDocuments:
  - docs/nextlens/src/rawNotes/Reimagine.md
  - docs/nextlens/src/rawNotes/TopDown.md
goal: "Sequence the first buildable TopDownLens module increment from express planning into FinalizePlan."
key_decisions: []
open_questions: []
depends_on: [business-plan, tech-plan]
blocks: []
updated_at: 2026-05-14T01:30:00Z
---

# Sprint Plan - TopDownLens Module

## Sprint Objective

Deliver a small, coherent implementation plan for the TopDownLens module. The sprint should establish the new Lens mental model, schemas, storage topology, BMAD bridge, and Salmon signal contract without attempting a full framework rewrite.

## Delivery Strategy

Build the module spine first:

1. Define stable data contracts.
2. Define the source-of-truth topology.
3. Support one top-down discovery walkthrough.
4. Preserve bottom-up feature-first behavior.
5. Generate a focused BMAD packet for one selected feature.
6. Rebuild a minimal derived graph.
7. Record Salmon signals for upstream correction.

## Work Packages

### TL-1 - Module Ontology And Storage Contract

**Goal:** Define the core objects and where they live.

**Scope:**
- `feature.yaml`
- relationship records
- landscape entity records
- derived graph files
- Salmon signal records
- BMAD packet schema
- hierarchy records from system to product area to outcome to journey to capability to feature

**Acceptance:**
- Stable IDs are required for all durable entities.
- Paths are treated as mutable addresses.
- Graph files are marked derived and rebuildable.
- Feature archive, living landscape, and derived graph are separate.
- The hierarchy explicitly names system, product area, outcome, journey, capability, feature, story, task, code, test, and evidence levels.

### TL-2 - Top-Down Discovery Walkthrough

**Goal:** Document and/or prototype the flow from large product idea to selected feature.

**Scope:**
- Raw context capture.
- System thesis extraction.
- Product area map.
- Role and stakeholder map.
- Outcome map.
- Journey map.
- Capability candidates.
- Selected vertical feature.
- Focused BMAD packet.

**Acceptance:**
- The walkthrough does not begin with domain/service/feature.
- At least one selected feature traces to system, product area, role, outcome, journey, capability candidate, and evidence.
- Candidate services are labeled implementation consequences, not planning roots.

### TL-3 - Bottom-Up Compatibility Rules

**Goal:** Ensure TopDownLens does not break the feature-first mental model.

**Scope:**
- Standalone feature creation rules.
- Adjacency detection concept.
- Repeated pressure categories.
- Promotion threshold guidance.
- Bottom-up promotion path from feature to capability to product area to domain/system when evidence exists.

**Acceptance:**
- A feature can remain independent forever.
- Adjacency is weak by default.
- Promotion is advisory and requires evidence.
- The rule "no growth without pressure" is encoded in the planning model.

### TL-4 - BMAD Bridge Packet

**Goal:** Define how LENS hands one selected feature to BMAD.

**Scope:**
- Packet schema.
- Source context list.
- Include/exclude guardrails.
- Required BMAD artifacts.
- Traceability fields.

**Acceptance:**
- Packet targets exactly one feature.
- Packet includes scope boundaries and acceptance evidence.
- Packet includes outcome/journey traceability when available.
- BMAD remains responsible for PRD, architecture, epics, stories, implementation, and review.

### TL-5 - Minimal Derived Graph Rebuild

**Goal:** Define or implement a rebuildable graph projection from source files.

**Scope:**
- Relationship index.
- Traceability index.
- Derived map metadata.
- Rebuild freshness marker.

**Acceptance:**
- Source files remain authoritative.
- Rebuild output is disposable.
- Broken references are reported, not silently ignored.

### TL-6 - Salmon Signal Contract

**Goal:** Define recursive upstream consistency validation.

**Scope:**
- Signal schema.
- Signal lifecycle.
- Severity levels.
- Upstream targets.
- Recommended actions.

**Acceptance:**
- Signals can target feature, journey, outcome, product area, landscape, or BMAD correct-course.
- Signals distinguish local note, landscape update, split feature, and blocking correction.
- Signals preserve evidence and provenance.

### TL-7 - Doctor Checks

**Goal:** Add deterministic health checks for the new topology.

**Scope:**
- Missing IDs.
- Broken references.
- Derived graph freshness.
- Missing feature scope.
- Missing BMAD packet traceability.
- Open blocking Salmon signals.

**Acceptance:**
- Checks return machine-readable results.
- Checks do not mutate files.
- Blocking and informational findings are separated.

## Suggested Sprint Order

1. TL-1 - Module Ontology And Storage Contract.
2. TL-4 - BMAD Bridge Packet.
3. TL-2 - Top-Down Discovery Walkthrough.
4. TL-3 - Bottom-Up Compatibility Rules.
5. TL-6 - Salmon Signal Contract.
6. TL-5 - Minimal Derived Graph Rebuild.
7. TL-7 - Doctor Checks.

## Dependencies

- Existing Lens feature context remains active for this express feature.
- BMAD remains installed and available as the downstream planning/execution method.
- The current domain/service/feature lifecycle remains available for backward compatibility.

## Risks To Carry Into FinalizePlan

- The first build may need to choose between `docs/` and `_bmad-output/lens/` for durable module outputs.
- If command names are decided too early, they may hard-code the wrong mental model.
- Automated extraction from raw notes should start as assisted synthesis, not unattended truth creation.
- Bottom-up promotion thresholds need examples and tests to avoid over-modeling.
- Salmon signal routing needs severity rules before implementation agents rely on it.

## Definition Of Ready For Dev

- FinalizePlan converts this sprint plan into epics, implementation readiness, and story files.
- Story files include concrete file paths and schema examples.
- Each story states whether it is documentation-only, schema implementation, CLI implementation, or test coverage.
- No dev story requires implementing the full future Lens system in one pass.
