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
  - Treat Feature Archive, Living Landscape, and Derived Graph as separate topology layers.
  - Make promotion advisory and evidence-driven.
  - Keep BMAD as the artifact and implementation engine; LENS supplies context and validation boundaries.
updated_at: 2026-05-14T01:30:00Z
---

# Tech Plan - TopDownLens Module

## Technical Summary

TopDownLens should be implemented as a BMAD-native Lens module layer that defines and later automates system discovery, journey-to-feature breakdown, BMAD packet generation, derived graph rebuilds, and Salmon upstream validation. The express planning scope is to define the architecture and first implementation spine, not to build every future command.

The design centers on three truths:

- Operational truth: features and their work archive.
- Human current truth: living landscape ledgers.
- Machine traversal truth: derived graph projection.

The graph is disposable. The landscape is curated. The feature archive records what happened.

## Controlling Architecture Principles

### 1. Feature First

The operational root is `feature`, not domain, service, initiative, or system. A feature is the smallest useful, testable vertical change selected for BMAD planning and implementation. A feature can remain independent forever. Higher-order structure is derived or promoted only when evidence exists.

### 2. Dual Entry Modes

The module must support both:

- Top-down: raw system vision -> product areas -> roles/outcomes/loops/journeys -> candidate capabilities -> selected feature -> BMAD packet.
- Bottom-up: one useful feature -> produced artifacts -> optional adjacency -> repeated pressure -> optional promotion.

### 3. No Growth Without Pressure

Capabilities, product areas, domains, services, and systems should not be created from anticipation alone. Promotion requires repeated pressure such as artifact reuse, repeated workflow, repeated dependency, repeated risk, ownership pressure, or cross-feature coordination.

### 4. IDs Are Identity

Every durable entity has a stable ID. Paths are storage addresses and may change.

```yaml
feature:
  id: feature.download_model_images
  path: docs/features/download-model-images/feature.yaml
```

References use IDs, not paths.

### 5. BMAD Owns Execution

LENS prepares context and enforces coherence. BMAD produces PRDs, architecture, epics, stories, implementation, review, and retrospective artifacts.

### 6. Salmon Owns Upstream Correction

Salmon signals capture downstream discoveries that may invalidate upstream assumptions. Signals may update local feature notes, the living landscape, or trigger BMAD correct-course.

## Hierarchy Model

TopDownLens should make decomposition explicit without treating folders, services, or repos as the product model.

```text
system.<slug>
  product_area.<slug>
    outcome.<slug>
      journey.<slug>
        capability.<slug>
          feature.<slug>
            story.<slug>
              task.<slug>
```

The hierarchy is directional but not always fully known. Top-down work starts near `system` and narrows toward a BMAD-ready `feature`. Bottom-up work starts at `feature` and may promote upward only when repeated evidence proves a stable capability, product area, domain, or system boundary.

Minimum traceability rules:

- A top-down feature must reference at least one `journey`, `outcome`, and source document.
- A bottom-up feature must record its `origin_need`, produced artifacts, explicit exclusions, and evidence.
- A capability may be candidate until two or more features create repeated pressure around the same durable ability.
- Product areas, domains, services, and systems remain promoted context, not required parents for every feature.
- Existing Lens control-repo feature IDs remain lifecycle containers; TopDownLens feature IDs use the `feature.<slug>` namespace.

## Proposed Storage Topology

The future module should separate storage concerns explicitly.

```text
_bmad-output/lens/
  archive/
    features/
      <feature-id>/
        feature.yaml
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

For this existing Lens control repo, the current feature docs path remains `docs/nextlens/src/nextlens-src-topdownlens`; future implementation can bridge from Lens control-repo feature docs to `_bmad-output/lens` once the module is formalized.

## Core Data Contracts

### feature.yaml

```yaml
id: feature.<slug>
kind: feature
status: draft | active | implemented | validated | superseded
origin_mode: top_down | bottom_up
hierarchy:
  system: null
  product_area: null
  outcome: null
  journey: null
  capabilities: []
goal: ""
origin_need: ""
starts_with: []
ends_with: []
includes: []
excludes: []
produces: []
consumes: []
traceability:
  system: null
  product_area: null
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
from: feature.download_model_images
to: feature.generate_model_description
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
  type: story | feature | implementation | review | validation
  id: ""
signal_type: assumption_invalidated | missing_context | scope_drift | impact_discovered | boundary_wrong | evidence_changed
severity: low | medium | high | blocking
upstream_targets:
  - feature
  - journey
  - outcome
  - product_area
  - landscape
finding: ""
recommended_action: local_note | landscape_update | bmad_correct_course | split_feature | block_promotion
status: open | accepted | rejected | resolved | superseded
```

## Top-Down Workflow Design

The top-down path should support large ambiguous systems without jumping directly into PRD or service architecture.

1. Capture raw product context.
2. Extract candidate system thesis, product areas, roles, stakeholders, outcomes, operating loops, and language.
3. Challenge assumptions and unknowns.
4. Map journeys from outcomes.
5. Break the journey into vertical feature candidates.
6. Select one feature and map initial capabilities and implementation impact only as candidates.
7. Generate a focused BMAD packet for the selected feature.
8. Sync BMAD outputs back into the feature archive and landscape.
9. Run Salmon validation after implementation/review.

Command candidates:

```text
lens capture
lens synthesize system
lens map roles
lens map outcomes
lens map journeys
lens break journey
lens analyze impact
lens prepare bmad
lens sync bmad
lens validate outcome
```

## Bottom-Up Workflow Design

The bottom-up path begins with a small useful task. It intentionally avoids invented architecture.

1. Create a feature from one local need.
2. Scope it tightly and record explicit out-of-scope items.
3. Prepare BMAD only for that feature.
4. Record produced artifacts after implementation.
5. Detect adjacent features when artifacts, workflows, or dependencies overlap.
6. Detect repeated pressure over time.
7. Suggest capability, product-area, domain, service, or system promotion only after evidence accumulates.

Command candidates:

```text
lens feature new
lens detect adjacency
lens detect repetition
lens suggest promotion
lens promote --candidate <id>
```

Promotion should be manual and reviewable.

## BMAD Bridge

The BMAD bridge creates a packet that constrains BMAD to the selected feature.

```yaml
bmad_packet:
  feature_id: feature.evidence_visible_to_teacher
  planning_scope: selected_feature_only
  source_context:
    - system_thesis
    - product_area
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

BMAD artifacts should link back to feature ID, outcome ID, journey ID, and source documents.

## Impact Analysis

Impact analysis should be a first-class artifact for both top-down and bottom-up modes.

```yaml
impact_map:
  feature_id: feature.<slug>
  product_area_id: product_area.<slug>
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
- Every feature has includes/excludes and acceptance evidence.
- Every BMAD packet references exactly one active feature.
- Relationship statuses use the allowed lifecycle.
- Salmon signals have source, target, severity, and status.

## Testing Strategy

- Schema validation tests for feature, relationship, landscape entity, BMAD packet, and Salmon signal.
- Golden-output tests for top-down product context -> system map -> feature packet.
- Golden-output tests for bottom-up feature -> adjacency detection -> promotion candidate.
- Doctor checks for broken references and stale derived graph output.
- Round-trip rebuild test: source files -> derived graph -> source files remain unchanged.

## Technical Risks

- Schema sprawl could make the module too heavy for small features.
- Automated extraction could hallucinate stable relationships from weak evidence.
- Promotion thresholds may be subjective without explicit evidence categories.
- BMAD packet generation may leak future-system scope into a tiny feature.
- Salmon could become noisy unless signals are routed by severity and action type.

## Implementation Boundary For This Feature

This feature should hand FinalizePlan a coherent module blueprint and implementation spine. It should not build a complete CLI or UI. The first buildable implementation after FinalizePlan should focus on schemas, storage layout, one top-down walkthrough, one bottom-up compatibility example, BMAD packet generation, derived graph rebuild, and Salmon signal recording.