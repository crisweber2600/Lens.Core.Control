---
feature: nextlens-src-topdownlens
doc_type: business-plan
status: draft
track: express
phase: expressplan
inputDocuments:
  - docs/nextlens/src/rawNotes/Reimagine.md
  - docs/nextlens/src/rawNotes/TopDown.md
goal: "Outline the TopDownLens module: a BMAD-native LENS redesign that supports top-down system discovery, bottom-up slice growth, and Salmon upstream validation."
key_decisions:
  - LENS should shift from domain/service/feature as the planning root to outcome, journey, slice, impact, and evolving topology.
  - Slice is the operational unit; domains, services, capabilities, and systems are metadata or promoted structure.
  - The module must support both top-down decomposition from a known system and bottom-up growth from one useful slice.
  - Promotion is optional and evidence-driven; no growth happens without repeated pressure.
  - Salmon is recursive upstream consistency validation, not a notification system.
updated_at: 2026-05-14T01:30:00Z
---

# Business Plan - TopDownLens Module

## Executive Summary

TopDownLens is the first planning slice for a reimagined LENS module. The desired module turns LENS from a feature lifecycle manager into a BMAD-native system-discovery, slice-orchestration, and topology-evolution framework.

The current Lens model can create domains, services, and features, but those objects mostly answer where work is stored. The raw notes argue that a next-generation Lens module must answer a more useful question: what outcome is being pursued, who experiences it, what journey changes, what slice should BMAD build, and what downstream implementation discoveries should update upstream understanding.

BMAD remains the execution engine. LENS supplies context, coherence, traceability, relationship discovery, slice selection, impact analysis, and Salmon validation around BMAD.

## Problem

The existing domain/service/feature model is too implementation-shaped to guide large ambiguous product work or tiny exploratory utilities.

For top-down products, such as a NorthStar-style education operating system, the planning root is not a service or feature. The work begins with a system promise, roles, outcomes, operating loops, journeys, and vertical slices. A service map should emerge later from architecture and implementation impact.

For bottom-up work, such as downloading images from 3D model websites, the user may only know one useful local task. Lens must not turn that task into a fake platform. The slice should stand alone until repeated evidence justifies adjacency, composition, capability promotion, or domain formation.

## Business Goals

- Help users start from either a large ambiguous system idea or one small useful slice.
- Produce BMAD-ready planning context without forcing premature architecture.
- Preserve long-lived current truth separately from work history.
- Keep AI planning grounded in roles, outcomes, journeys, slices, evidence, and implementation impact.
- Detect when implementation invalidates upstream assumptions and route those discoveries through Salmon.
- Let structure emerge only when repeated pressure justifies it.

## Non-Goals

- Do not replace BMAD PRDs, architecture, epics, stories, implementation, review, or retrospective workflows.
- Do not force every slice to become a capability, domain, service, or system.
- Do not make the derived graph authoritative.
- Do not use folder layout as the system topology.
- Do not implement the entire future Lens module in this express planning slice.
- Do not remove domain/service/feature compatibility in the current Lens control model.

## Target Users

- Product leads and founders shaping large ambiguous systems.
- Analysts and UX/product planners converting raw intent into outcomes and journeys.
- Architects identifying capability and service implications after slices are understood.
- Scrum masters and story authors preparing BMAD-ready slice plans.
- Developers and AI coding agents that need high-context implementation guidance.
- Stakeholders who need read-only visibility into current truth, risk, progress, and stale assumptions.

## Core Product Thesis

TopDownLens should define a new Lens mental model:

```text
BMAD makes the work buildable.
LENS makes the work understandable, traceable, adaptable, and coherent.
Salmon checks whether the built slice still matches upstream reality.
```

The module must support two valid entry modes:

- Top-down: known or suspected system -> roles -> outcomes -> loops -> journeys -> slices -> BMAD execution.
- Bottom-up: small useful slice -> local artifact -> optional adjacency -> repeated pressure -> optional capability/domain/system promotion -> BMAD execution when needed.

## MVP Scope

This express feature should define the module to build, not implement the whole runtime. The MVP planning target is a thin but coherent module contract:

- Core ontology and source-of-truth model.
- Top-down workflow from raw product vision to one BMAD-ready slice.
- Bottom-up compatibility rules so slice-first work is not over-modeled.
- Storage topology for slice archive, living landscape, and derived graph.
- Salmon signal concept and lifecycle.
- Command surface and lifecycle map for a future BMAD add-on module.
- Sprint-ready implementation sequence for the first buildable module increment.

## Required Concepts

- Slice: the smallest operational unit of useful, testable work.
- Artifact: something produced or consumed by a slice.
- Adjacency: a weak relationship between slices caused by shared artifacts, users, workflows, risks, or dependencies.
- Relationship: a typed connection with provenance, confidence, and lifecycle state.
- Outcome: a desired change for a user, business, stakeholder, or system.
- Journey: an end-to-end path through which an outcome becomes real.
- Capability: a durable system ability that emerges from repeated slice pressure.
- Landscape: the current human-readable interpretation of the system.
- Derived graph: rebuildable machine projection for traversal, traceability, and impact analysis.
- Salmon signal: downstream evidence that may need to update upstream truth.

## Success Criteria

- The planned module clearly supports both top-down and bottom-up entry modes.
- The artifacts define slice as the operational unit without deleting current Lens domain/service compatibility.
- The storage model separates work history, current interpretation, and derived machine projection.
- The BMAD bridge is explicit: LENS prepares context, BMAD plans/builds, LENS validates and updates landscape.
- Salmon is specified as an upstream correction mechanism with evidence, severity, target, and resolution path.
- The sprint plan identifies a small first implementation spine instead of attempting a full framework rewrite.

## Risks

- The module may overreach and try to implement all future Lens concepts at once.
- The top-down flow may accidentally reintroduce premature system architecture.
- The bottom-up flow may be too weak if adjacency and promotion thresholds are left vague.
- Derived graph output could drift from authoritative files if rebuild rules are not strict.
- Salmon signals may become noisy unless severity and routing rules are explicit.
- BMAD planning may expand beyond the selected slice unless the bridge enforces scope boundaries.

## Open Questions

- Should the first implementation command be named `lens-new-system`, `lens-capture`, or `lens-discover`?
- Should bottom-up seed creation ship in the first module increment or only be represented in the schema?
- Where should the future module store outputs: `docs/`, `_bmad-output/lens/`, or both during migration?
- What minimum relationship lifecycle is needed for useful AI traversal without over-modeling?
- What Salmon severity threshold should trigger BMAD correct-course versus a landscape-only update?