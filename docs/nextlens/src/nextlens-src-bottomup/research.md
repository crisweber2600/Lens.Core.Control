---
feature: nextlens-src-bottomup
doc_type: research
status: complete
workflowType: research
research_type: technical
research_topic: "Bottom-Up LENS feature packet architecture and evidence topology"
research_goals: "Ground the future bottom-up lane design in technical practices for schema validation, provenance, source-of-truth projections, decision records, and safe deferred topology promotion."
inputDocuments:
  - docs/nextlens/src/nextlens-src-bottomup/brainstorm.md
web_research_enabled: true
source_verification: true
updated_at: 2026-05-19T00:00:00Z
---

# Technical Research — Bottom-Up LENS Feature Packet Architecture

**Date:** 2026-05-19  
**Author:** BMad  
**Research Type:** Technical

---

## Research Overview

This research grounds the Bottom-Up LENS PrePlan in technical patterns that support one constrained feature packet without prematurely creating a system, domain, capability, roadmap, architecture, graph, or landscape entry.

The research uses two evidence classes:

1. **Internal Lens/BMAD context:** the completed `brainstorm.md`, including the Bottom-Up LENS brief, the Two-Tree Model with Derived Map, Salmon deferral, and Auspex reporting implications.
2. **External technical references:** JSON Schema for explicit packet validation, W3C PROV for provenance concepts, ADR practice for decision rationale capture, and OpenGitOps principles for declarative, versioned, immutable state and reconciliation.

The goal is not to design the full bottom-up lane. The goal is to identify the safest technical shape for the first support slice: a bottom-up feature packet creator.

---

## Research Questions

1. What should a bottom-up feature packet contain at minimum?
2. Which validations should block packet creation?
3. Which validations should defer BMAD execution without blocking packet capture?
4. How should packet provenance be represented so later evidence can be trusted?
5. How should the packet fit the Two-Tree Model: Feature Archive, Living Landscape, and Derived Map?
6. What should the command explicitly not emit during packet creation?
7. How should future reporting and Salmon workflows consume packet metadata without turning packet creation into promotion?

---

## External Technical Grounding

### JSON Schema: Explicit Packet Validation

JSON Schema describes and validates JSON document structure through a schema document. The JSON Schema guide identifies `$schema`, `$id`, `title`, `description`, `type`, `properties`, and `required` as core pieces for defining a valid data shape, and distinguishes annotations from validation keywords.

**Relevance to Bottom-Up LENS:**

A bottom-up packet should be schema-validated because its primary risk is implicit invention. The packet creator should not rely on prose alone to enforce safety. JSON Schema-style validation can enforce:

- required packet identity fields
- required selected-feature fields
- required included-scope and explicit-out-of-scope arrays
- required non-inference flags
- required confirmation receipt
- invalid or missing source mode
- invalid downstream side-effect claims

**Design implication:** Define a `feature-packet.schema.json` with a stable `$id`, required packet fields, and nested object validation for `selectedFeature`, `scope`, `assumptions`, `provenance`, and `nonEffectsReceipt`.

Source: JSON Schema, “Creating your first schema,” https://json-schema.org/learn/getting-started-step-by-step

### W3C PROV: Provenance For Trustworthiness

The W3C PROV overview defines provenance as information about entities, activities, and people involved in producing data or a thing, useful for assessing quality, reliability, and trustworthiness. PROV also emphasizes identifiers, attribution, processing steps, reproducibility, versioning, derivation, and validation constraints.

**Relevance to Bottom-Up LENS:**

A bottom-up packet is not only content; it is evidence of how a feature candidate was selected. The packet must preserve provenance so future BMAD, adjacency, Salmon, or promotion decisions can tell:

- who or what produced the packet
- what raw context it came from
- which candidate was selected
- which candidates were deferred
- which assumptions were recorded but not promoted
- what confirmation occurred before writing
- what command activity produced the file

**Design implication:** The packet should include a compact provenance block, not just feature fields. The block should capture activity, actor, source inputs, timestamp, decision rationale, and derivation links to brainstorm/research/product-brief artifacts.

Source: W3C PROV Overview, https://www.w3.org/TR/prov-overview/

### ADRs: Decision Rationale And Consequences

The ADR community describes an Architectural Decision Record as a record of a single justified design choice, including rationale, trade-offs, and consequences. ADR collections form a project decision log and can support iterative/incremental engineering.

**Relevance to Bottom-Up LENS:**

The packet creator makes decisions that should be auditable but should not become architecture. Those decisions include candidate selection, scope boundaries, non-effects, and BMAD readiness state. ADR-style rationale is useful, but the packet should not imply architectural commitment.

**Design implication:** Use a lightweight decision-rationale section inside the packet or adjacent `packet-decision.md` for:

- why this candidate was selected
- why other candidates were deferred
- why the scope is bounded as written
- what trade-offs were accepted
- what consequences are explicitly deferred

Source: ADR GitHub Organization, https://adr.github.io/

### OpenGitOps: Declarative, Versioned, Immutable State

OpenGitOps principles describe systems managed by declarative desired state that is versioned, immutable, pulled automatically, and continuously reconciled.

**Relevance to Bottom-Up LENS:**

Lens planning artifacts are not infrastructure, but similar state principles apply. The packet should be declarative, versioned in source control, immutable as historical evidence once confirmed, and later reconciled into derived projections rather than manually edited into a governance map.

**Design implication:** Treat packet files as source artifacts and derived maps/graphs as rebuildable projections. Do not hand-author graph edges during packet creation. Future validators or rebuild commands can reconcile packet metadata into projections after evidence exists.

Source: OpenGitOps Principles v1.0.0, https://opengitops.dev/

---

## Internal Lens/BMAD Findings

### Finding 1: Packet Creation Is A Restraint Mechanism

The brainstorm’s strongest conclusion is that bottom-up’s first mechanism is not growth. It is restraint. The first slice should create one safe packet and prove what it did not do.

**Implication:** The command must be fail-closed. If it cannot prove a hard constraint, it should write nothing.

### Finding 2: Packet Validity And BMAD Readiness Are Separate

A packet can be valid archive evidence before it is ready for BMAD execution. Packet validity proves the feature candidate is captured safely. BMAD readiness proves downstream generation has enough context to avoid inventing hierarchy.

**Implication:** Implement two gates:

1. `packet_validity`: can this packet be saved?
2. `bmad_readiness`: can BMAD execute from this packet?

### Finding 3: Feature Archive Is The Correct Future Home

The integrated topology context says features are immutable facts, the landscape is interpretation, and the map is a cache. A bottom-up packet is a feature-first archived fact, not living landscape truth.

**Implication:** Future bottom-up packet output belongs in a stable Feature Archive path such as `docs/features/<feature-id>/feature-packet.json`, while this top-down planning feature continues to write artifacts under `docs/nextlens/src/nextlens-src-bottomup`.

### Finding 4: Derived Map Compatibility Matters But Must Not Create Truth

Packet metadata should be reconstructible by a future derived-map rebuild. But packet creation should not write graph edges, landscape entries, or promotion candidates.

**Implication:** Include stable IDs and relationship hints as null/unpromoted fields, but mark all promoted structure as absent.

### Finding 5: Auspex Reporting Needs Stable Status Fields

Auspex reinforces that stakeholder-facing reporting should be read-only and sourced from artifacts. Bottom-up packets should expose reportable state without allowing report views to mutate governance.

**Implication:** Use explicit packet statuses such as `draft`, `confirmed`, `valid`, `bmad-ready`, `executed`, `evidence-recorded`, and `promotion-suggested` only when those states are actually reached.

---

## Recommended Packet Model

### Minimum Packet Fields

```yaml
featureId: <stable-feature-id>
kind: feature_packet
sourceMode: bottom_up
status: confirmed
created_at: <timestamp>
updated_at: <timestamp>
selectedFeature:
  goal: <one independently useful outcome>
  problem: <real problem solved now>
  user_or_actor: <known actor or unknown-with-reason>
  acceptanceCriteria: []
scope:
  includedScope: []
  explicitOutOfScope: []
  deferredCandidates: []
constraints:
  knownConstraints: []
  nonInferenceRules:
    - do_not_infer_system
    - do_not_infer_domain
    - do_not_infer_capability
    - do_not_infer_roadmap
    - do_not_infer_architecture
assumptions:
  unpromoted: []
provenance:
  sourceInputs: []
  selectedCandidateRationale: <text>
  createdBy: <actor/tool>
  activity: bottom_up_packet_creation
  confirmation: <receipt id or timestamp>
nonEffectsReceipt:
  adjacencyRecordsEmitted: false
  pressureDetectionEmitted: false
  promotionCandidatesEmitted: false
  salmonSignalsEmitted: false
  landscapeUpdatesEmitted: false
  graphUpdatesEmitted: false
topology:
  docs_path: <stable archive path>
  belongs_to:
    service: null
    domain: null
    program: null
  assumptions_promoted: false
  landscape_promoted: false
  graph_edges_emitted: false
```

### Packet Validity Gate

The packet validity gate should fail if:

- `sourceMode` is not `bottom_up`
- more than one selected candidate exists
- selected feature goal/problem is missing
- included scope is empty
- explicit out-of-scope is empty or generic
- non-inference rules are missing
- assumptions are promoted at packet creation
- confirmation receipt is absent
- any non-effects receipt field is true

### BMAD Readiness Gate

The BMAD readiness gate should fail or warn if:

- acceptance criteria are too weak for PRD/story generation
- known constraints are empty and no reason is supplied
- user/actor is unknown without reason
- BMAD handoff instructions do not prohibit inferred system/domain/capability/roadmap/architecture
- packet provenance is insufficient to identify source context

This gate should not necessarily invalidate packet capture. It should block execution through BMAD until corrected.

---

## Non-Effects Contract

Packet creation must not emit:

- adjacency records
- pressure detection records
- promotion candidates
- Salmon signals
- Living Landscape updates
- Derived Graph updates
- roadmap entries
- service/domain/program ownership claims

Recommended validator behavior:

1. Run packet schema validation.
2. Run non-inference validation.
3. Run non-effects validation.
4. Write packet only after preview confirmation.
5. Return a machine-readable receipt showing exactly one packet file was written.

---

## Storage And Topology Recommendation

### For This Top-Down Feature

Continue writing planning artifacts to:

```text
docs/nextlens/src/nextlens-src-bottomup/
```

This follows current Lens lifecycle rules and respects the active `feature.yaml.docs.path`.

### For Future Bottom-Up Lane

Prefer a Feature Archive location:

```text
docs/features/<feature-id>/feature-packet.json
```

Optional supporting files after later phases:

```text
docs/features/<feature-id>/
  feature-packet.json
  prd.md
  architecture.md
  stories/
  implementation-notes.md
  evidence.md
  salmon-signals.md
```

Do not write to the Living Landscape until a promotion is accepted.

---

## Risks And Mitigations

| Risk | Impact | Mitigation |
|---|---|---|
| Packet schema grows into top-down planning | Bottom-up loses its value | Keep schema minimal and validate non-inference rules |
| BMAD invents missing hierarchy | Downstream artifacts become misleading | Separate BMAD readiness gate with explicit non-inference instructions |
| Deferred candidates become roadmap | Premature prioritization | Store deferred candidates as unranked notes only |
| Graph or landscape is updated too early | Evidence becomes false truth | Non-effects receipt and fail-closed validation |
| Provenance is too thin | Later promotion decisions are untrustworthy | Include source inputs, actor/activity, rationale, and confirmation receipt |
| Reporting implies promotion | Stakeholders misread packet state | Use explicit statuses and Auspex read-only reporting language |

---

## Research Conclusion

The technical research supports the brainstorm direction: the first Bottom-Up LENS support slice should be a dedicated packet creator with explicit schema validation, provenance, non-effects validation, and two separate readiness gates.

The safest architecture is:

1. create one stable archived feature packet;
2. validate local value, scope, explicit out-of-scope, provenance, and non-effects;
3. keep assumptions unpromoted;
4. defer BMAD execution readiness until a separate handoff validator passes;
5. defer adjacency, pressure, Salmon, promotion, Landscape, and Graph behavior until implementation evidence exists.

This makes packet creation a restrained archival act, not an act of topology creation.

---

## Sources

1. JSON Schema, “Creating your first schema,” https://json-schema.org/learn/getting-started-step-by-step
2. W3C, “PROV-Overview,” https://www.w3.org/TR/prov-overview/
3. ADR GitHub Organization, “Architectural Decision Records,” https://adr.github.io/
4. OpenGitOps, “GitOps Principles v1.0.0,” https://opengitops.dev/
5. Internal Lens artifact: `docs/nextlens/src/nextlens-src-bottomup/brainstorm.md`
