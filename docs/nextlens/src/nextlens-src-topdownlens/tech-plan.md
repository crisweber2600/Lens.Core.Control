---
feature: nextlens-src-topdownlens
doc_type: tech-plan
status: draft
track: express
phase: expressplan
inputDocuments:
  - docs/nextlens/src/rawNotes/Reimagine.md
  - docs/nextlens/src/rawNotes/TopDown.md
goal: "Define the TopDownLens technical module design: ontology, storage topology, workflows, BMAD bridge, Salmon validation, and first implementation boundaries."
key_decisions:
  - Use stable IDs as identity and paths as mutable addresses.
  - Keep authoritative truth in versioned control-repo files; rebuild graph projections from those files.
  - Treat Slice Archive, Living Landscape, and Derived Graph as separate topology layers.
  - Make promotion advisory and evidence-driven.
  - Keep BMAD as the artifact and implementation engine; LENS supplies context and validation boundaries.
updated_at: 2026-05-14T01:30:00Z
---

# Tech Plan - TopDownLens Module

## Technical Summary

TopDownLens should be implemented as a BMAD-native Lens module layer that defines and later automates system discovery, journey slicing, BMAD packet generation, derived graph rebuilds, and Salmon upstream validation. The express planning scope is to define the architecture and first implementation spine, not to build every future command.

The design centers on three truths:

- Operational truth: slices and their work archive.
- Human current truth: living landscape ledgers.
- Machine traversal truth: derived graph projection.

The graph is disposable. The landscape is curated. The slice archive records what happened.

## Controlling Architecture Principles

### 1. Slice First

The operational root is `slice`, not domain, service, feature, initiative, or system. A slice can remain independent forever. Higher-order structure is derived or promoted only when evidence exists.

### 2. Dual Entry Modes

The module must support both:

- Top-down: raw system vision -> roles/outcomes/loops/journeys -> selected slice -> BMAD packet.
- Bottom-up: one useful slice -> produced artifacts -> optional adjacency -> repeated pressure -> optional promotion.

### 3. No Growth Without Pressure

Capabilities, domains, services, and systems should not be created from anticipation alone. Promotion requires repeated pressure such as artifact reuse, repeated workflow, repeated dependency, repeated risk, ownership pressure, or cross-slice coordination.

### 4. IDs Are Identity

Every durable entity has a stable ID. Paths are storage addresses and may change.

```yaml
slice:
  id: slice.download_model_images
  path: docs/slices/download-model-images/slice.yaml
```

References use IDs, not paths.

### 5. BMAD Owns Execution

LENS prepares context and enforces coherence. BMAD produces PRDs, architecture, epics, stories, implementation, review, and retrospective artifacts.

### 6. Salmon Owns Upstream Correction

Salmon signals capture downstream discoveries that may invalidate upstream assumptions. Signals may update local slice notes, the living landscape, or trigger BMAD correct-course.

## Proposed Storage Topology

The future module should separate storage concerns explicitly.

```text
_bmad-output/lens/
  archive/
    slices/
      <slice-id>/
        slice.yaml
        bmad-packet.yaml
        notes.md
        validation.md
        salmon-signals.yaml
  landscape/
    systems/
    outcomes/
    journeys/
    capabilities/
    domains/
    services/
    ledgers/
  graph/
    derived-map.yaml
    relationship-index.yaml
    traceability-index.yaml
  bridge/
    bmad-packets/
  checks/
    doctor-results.yaml
```

For this existing Lens control repo, the current feature docs path remains `docs/nextlens/src/nextlens-src-topdownlens`; future implementation can bridge from Lens feature docs to `_bmad-output/lens` once the module is formalized.

## Core Data Contracts

### slice.yaml

```yaml
id: slice.<slug>
kind: slice
status: draft | active | implemented | validated | superseded
origin_mode: top_down | bottom_up
goal: ""
starts_with: []
ends_with: []
includes: []
excludes: []
produces: []
consumes: []
traceability:
  system: null
  outcome: null
  journey: null
  loop_steps: []
  capabilities: []
evidence: []
relationships: []
provenance:
  source_documents: []
  confidence: low | medium | high
```

### relationship.yaml / relationship-index.yaml

```yaml
id: rel.<from>.<to>.<type>
from: slice.download_model_images
to: slice.generate_model_description
type: adjacency | depends_on | consumes | produces | invalidates | supports
status: extracted | hypothesized | challenged | reviewed | promoted | implemented | validated | superseded
confidence: low | medium | high
evidence: []
```

### landscape entity

```yaml
id: capability.model_image_processing
kind: capability
status: candidate | active | superseded
promoted_from: []
promotion_evidence: []
owned_relationships: []
current_interpretation: ""
```

### salmon-signal.yaml

```yaml
id: salmon.<timestamp>.<slug>
source:
  type: story | slice | implementation | review | validation
  id: ""
signal_type: assumption_invalidated | missing_context | scope_drift | impact_discovered | boundary_wrong | evidence_changed
severity: low | medium | high | blocking
upstream_targets:
  - slice
  - journey
  - outcome
  - landscape
finding: ""
recommended_action: local_note | landscape_update | bmad_correct_course | split_slice | block_promotion
status: open | accepted | rejected | resolved | superseded
```

## Top-Down Workflow Design

The top-down path should support large ambiguous systems without jumping directly into PRD or service architecture.

1. Capture raw product context.
2. Extract candidate system thesis, roles, stakeholders, outcomes, operating loops, and language.
3. Challenge assumptions and unknowns.
4. Map journeys from outcomes.
5. Select one vertical slice.
6. Map initial capabilities and implementation impact only as candidates.
7. Generate a focused BMAD packet for the selected slice.
8. Sync BMAD outputs back into the slice archive and landscape.
9. Run Salmon validation after implementation/review.

Command candidates:

```text
lens capture
lens synthesize system
lens map roles
lens map outcomes
lens map journeys
lens slice journey
lens analyze impact
lens prepare bmad
lens sync bmad
lens validate outcome
```

## Bottom-Up Workflow Design

The bottom-up path begins with a small useful task. It intentionally avoids invented architecture.

1. Create a slice from one local need.
2. Scope it tightly and record explicit out-of-scope items.
3. Prepare BMAD only for that slice.
4. Record produced artifacts after implementation.
5. Detect adjacent slices when artifacts, workflows, or dependencies overlap.
6. Detect repeated pressure over time.
7. Suggest promotion only after evidence accumulates.

Command candidates:

```text
lens slice new
lens detect adjacency
lens detect repetition
lens suggest promotion
lens promote --candidate <id>
```

Promotion should be manual and reviewable.

## BMAD Bridge

The BMAD bridge creates a packet that constrains BMAD to the selected slice.

```yaml
bmad_packet:
  slice_id: slice.evidence_visible_to_teacher
  planning_scope: selected_slice_only
  source_context:
    - system_thesis
    - outcome
    - journey
    - impact_map
  required_outputs:
    - product-brief.md
    - prd.md
    - architecture.md
    - epics.md
    - stories.md
  scope_guardrails:
    include: []
    exclude: []
```

BMAD artifacts should link back to slice ID, outcome ID, journey ID, and source documents.

## Impact Analysis

Impact analysis should be a first-class artifact for both top-down and bottom-up modes.

```yaml
impact_map:
  slice_id: slice.<slug>
  likely_domains: []
  likely_services: []
  likely_repos: []
  likely_contracts: []
  shared_dependencies: []
  related_workstreams: []
  tests_required: []
  observability_required: []
  decisions_needed: []
```

For early TopDownLens, static/manual impact analysis is acceptable. Automated repo/file impact can be a later increment.

## Doctor Checks

The first Doctor checks should be simple and deterministic:

- All referenced IDs exist.
- No authoritative file references derived graph as source truth.
- Every slice has includes/excludes and acceptance evidence.
- Every BMAD packet references exactly one active slice.
- Relationship statuses use the allowed lifecycle.
- Salmon signals have source, target, severity, and status.

## Testing Strategy

- Schema validation tests for slice, relationship, landscape entity, BMAD packet, and Salmon signal.
- Golden-output tests for top-down product context -> system map -> slice packet.
- Golden-output tests for bottom-up slice -> adjacency detection -> promotion candidate.
- Doctor checks for broken references and stale derived graph output.
- Round-trip rebuild test: source files -> derived graph -> source files remain unchanged.

## Technical Risks

- Schema sprawl could make the module too heavy for small slices.
- Automated extraction could hallucinate stable relationships from weak evidence.
- Promotion thresholds may be subjective without explicit evidence categories.
- BMAD packet generation may leak future-system scope into a tiny slice.
- Salmon could become noisy unless signals are routed by severity and action type.

## Implementation Boundary For This Feature

This feature should hand FinalizePlan a coherent module blueprint and implementation spine. It should not build a complete CLI or UI. The first buildable implementation after FinalizePlan should focus on schemas, storage layout, one top-down walkthrough, one bottom-up compatibility example, BMAD packet generation, derived graph rebuild, and Salmon signal recording.