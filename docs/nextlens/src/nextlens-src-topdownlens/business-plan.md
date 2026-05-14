---
feature: nextlens-src-topdownlens
doc_type: business-plan
status: draft
track: express
phase: expressplan
inputDocuments:
  - docs/nextlens/src/rawNotes/Reimagine.md
  - docs/nextlens/src/rawNotes/TopDown.md
goal: "Outline the TopDownLens module: a BMAD-native LENS redesign that supports top-down system discovery, bottom-up feature growth, and Salmon upstream validation."
key_decisions:
  - LENS should shift from domain/service/feature as the planning root to system, product area, outcome, journey, capability, feature, impact, and evolving topology.
  - Feature is the operational unit; domains, services, capabilities, product areas, and systems are context or promoted structure.
  - The module must support both top-down decomposition from a known system and bottom-up growth from one useful feature.
  - Promotion is optional and evidence-driven; no growth happens without repeated pressure.
  - Salmon is recursive upstream consistency validation, not a notification system.
open_questions: []
depends_on: []
blocks: []
updated_at: 2026-05-14T01:30:00Z
---

# Business Plan - TopDownLens Module

## Executive Summary

TopDownLens is the first planning feature for a reimagined LENS module. The desired module turns LENS from a feature lifecycle manager into a BMAD-native system-discovery, feature-orchestration, and topology-evolution framework.

The current Lens model can create domains, services, and features, but those objects mostly answer where work is stored. The raw notes argue that a next-generation Lens module must answer a more useful question: what system or product area is changing, what outcome is being pursued, who experiences it, what journey changes, what feature should BMAD build, and what downstream implementation discoveries should update upstream understanding.

BMAD remains the execution engine. LENS supplies context, coherence, traceability, relationship discovery, feature selection, impact analysis, and Salmon validation around BMAD.

## Problem

The existing domain/service/feature model is too implementation-shaped to guide large ambiguous product work or tiny exploratory utilities.

For top-down products, such as a NorthStar-style education operating system, the planning root is not a service or repo. The work begins with a system promise, product areas, roles, outcomes, operating loops, journeys, capabilities, and vertical features. A service map should emerge later from architecture and implementation impact.

For bottom-up work, such as downloading images from 3D model websites, the user may only know one useful local task. Lens must not turn that task into a fake platform. The feature should stand alone until repeated evidence justifies adjacency, composition, capability promotion, product-area grouping, or domain formation.

## Business Goals

- Help users start from either a large ambiguous system idea or one small useful feature.
- Produce BMAD-ready planning context without forcing premature architecture.
- Preserve long-lived current truth separately from work history.
- Keep AI planning grounded in system context, product areas, roles, outcomes, journeys, capabilities, features, evidence, and implementation impact.
- Detect when implementation invalidates upstream assumptions and route those discoveries through Salmon.
- Let structure emerge only when repeated pressure justifies it.

## Non-Goals

- Do not replace BMAD PRDs, architecture, epics, stories, implementation, review, or retrospective workflows.
- Do not force every feature to become a capability, product area, domain, service, or system.
- Do not make the derived graph authoritative.
- Do not use folder layout as the system topology.
- Do not implement the entire future Lens module in this express planning feature.
- Do not remove domain/service/feature compatibility in the current Lens control model.

## Target Users

- Product leads and founders shaping large ambiguous systems.
- Analysts and UX/product planners converting raw intent into outcomes and journeys.
- Architects identifying capability and service implications after features are understood.
- Scrum masters and story authors preparing BMAD-ready feature plans.
- Developers and AI coding agents that need high-context implementation guidance.
- Stakeholders who need read-only visibility into current truth, risk, progress, and stale assumptions.

## Core Product Thesis

TopDownLens should define a new Lens mental model:

```text
BMAD makes the work buildable.
LENS makes the work understandable, traceable, adaptable, and coherent.
Salmon checks whether the built feature still matches upstream reality.
```

The module must support two valid entry modes:

- Top-down: known or suspected system -> product areas -> roles -> outcomes -> loops -> journeys -> capabilities -> features -> BMAD execution.
- Bottom-up: small useful feature -> local artifact -> optional adjacency -> repeated pressure -> optional capability/product-area/domain/system promotion -> BMAD execution when needed.

## Hierarchy Naming Structure

TopDownLens should use a vocabulary that shows how a large idea becomes buildable without making service topology the planning root.

```text
System
  Product Area
    Outcome
      Journey
        Capability
          Feature
            BMAD Story
              Task / Code / Test / Evidence
```

- System: the highest-level product, operating model, or coherent solution boundary.
- Product Area: a major area of responsibility inside the system, such as checkout, model intake, or learner evidence.
- Outcome: the measurable state change the system should create for a user, business, stakeholder, or downstream system.
- Journey: the end-to-end path through which an actor experiences the outcome.
- Capability: a durable ability needed by one or more journeys; it may remain a candidate until repeated pressure proves it.
- Feature: the smallest useful, testable vertical change selected for BMAD planning and implementation.
- BMAD Story: the implementation-ready breakdown of the feature into repo-specific work.

A feature must trace upward to at least one journey and outcome when created top-down. A bottom-up feature may start without a known system, but it must record its produced artifacts and evidence so Lens can later promote relationships upward when pressure accumulates.

## MVP Scope

This express feature should define the module to build, not implement the whole runtime. The MVP planning target is a thin but coherent module contract:

- Core ontology and source-of-truth model.
- Top-down workflow from raw product vision to one BMAD-ready feature.
- Bottom-up compatibility rules so feature-first work is not over-modeled.
- Storage topology for feature archive, living landscape, and derived graph.
- Salmon signal concept and lifecycle.
- Command surface and lifecycle map for a future BMAD add-on module.
- Sprint-ready implementation sequence for the first buildable module increment.

## Required Concepts

- System: the highest-level coherent product or operating model being understood.
- Product Area: a named area inside a system that groups related journeys, capabilities, and features.
- Outcome: a desired change for a user, business, stakeholder, or system.
- Journey: an end-to-end path through which an outcome becomes real.
- Capability: a durable system ability that emerges from repeated feature pressure.
- Feature: the smallest operational unit of useful, testable vertical work.
- Artifact: something produced or consumed by a feature.
- Adjacency: a weak relationship between features caused by shared artifacts, users, workflows, risks, or dependencies.
- Relationship: a typed connection with provenance, confidence, and lifecycle state.
- Landscape: the current human-readable interpretation of the system.
- Derived graph: rebuildable machine projection for traversal, traceability, and impact analysis.
- Salmon signal: downstream evidence that may need to update upstream truth.

## Success Criteria

- The planned module clearly supports both top-down and bottom-up entry modes.
- The artifacts define feature as the operational unit without deleting current Lens domain/service/feature compatibility.
- The hierarchy makes the top-down path explicit from system to product area to outcome to journey to capability to feature.
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
- BMAD planning may expand beyond the selected feature unless the bridge enforces scope boundaries.

## Open Questions

- Should the first implementation command be named `lens-new-system`, `lens-capture`, or `lens-discover`?
- Should bottom-up seed creation ship in the first module increment or only be represented in the schema?
- Where should the future module store outputs: `docs/`, `_bmad-output/lens/`, or both during migration?
- What minimum relationship lifecycle is needed for useful AI traversal without over-modeling?
- What Salmon severity threshold should trigger BMAD correct-course versus a landscape-only update?
