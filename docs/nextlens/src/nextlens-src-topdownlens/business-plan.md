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
  - "Feature is the operational unit; domains, services, capabilities, product areas, and systems are context or promoted structure."
  - The module must support both top-down decomposition from a known system and bottom-up growth from one useful feature.
  - Promotion is optional and evidence-driven; no growth happens without repeated pressure.
  - Salmon is recursive upstream consistency validation, not a notification system.
  - "TopDownLens is self-hosting: its own design and evolution must be planned by TopDownLens itself, using a dedicated governance repo, a dedicated release/publish repo, constitution layering, and a Lens-style bugfix flow wired through GitHub Actions."
open_questions:
  - "What minimum relationship lifecycle states are needed for useful AI traversal without over-modeling? (you decide)"
  - "What Salmon severity threshold should trigger BMAD correct-course versus a landscape-only update? (you decide)"
depends_on: []
blocks: []
updated_at: 2026-05-14T03:10:00Z
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

- Should the first implementation command be named `lens-new-system`, `lens-capture`, or `lens-discover`? Provisional: `lens capture` (from tech plan command candidates). Command names are not fixed and will be confirmed in TL-12 dogfooding.
- Should bottom-up seed creation ship in the first module increment or only be represented in the schema? yes as it will help with evolution as needed. 
- Where should the future module store outputs: `docs/`, `_bmad-output/lens/`, or both during migration? docs
- What minimum relationship lifecycle is needed for useful AI traversal without over-modeling? you decide 
- What Salmon severity threshold should trigger BMAD correct-course versus a landscape-only update? you decide 

## Dogfooding And Self-Hosting Strategy

The primary acceptance test for TopDownLens is that TopDownLens can be used to plan, evolve, and govern itself. The module is not successful if the team designing it cannot run its top-down workflow on its own backlog.

### Goals

- Use TopDownLens to discover, sequence, and govern TopDownLens features after the first dev increment lands.
- Mirror the existing Lens control / governance / release split so the module reaches dogfooding with the same operational guarantees as Lens.
- Make publication, bugfix routing, and constitution enforcement reproducible through GitHub Actions instead of manual hand-copy.

### Repo Topology

- `nextlens-control`: planning workspace and branch topology owner for TopDownLens features (this repo, or a clean clone once the module exits incubation).
- `nextlens-governance`: authoritative metadata, feature index, constitutions, and published artifacts for TopDownLens.
- `nextlens-release`: read-only publish destination for the TopDownLens module payload, mirroring the role `Lens.Core.Release` plays for `lens-work`.
- TargetProjects clones for `nextlens-governance` and `nextlens-release` remain local read-only surfaces under the control workspace.

### Constitution Layering For TopDownLens

TopDownLens reuses the 4-level additive constitution hierarchy: org -> domain -> service -> repo.

- Org level: shared safety, review, and publication rules for the next-generation Lens program.
- Domain level (`nextlens`): planning, evidence, and Salmon defaults that apply to every TopDownLens feature.
- Service level (`nextlens/src`): module-implementation rules such as schema validation, doctor-check enforcement, and packet traceability requirements.
- Repo level: per-repository overrides only when a target repo has stricter constraints than the service default.

During incubation, TopDownLens features remain governed by the existing `nextlens` and `nextlens/src` constitutions in `Lens.Core.Governance`. After the first dev increment, the constitutions migrate to `nextlens-governance` without losing their additive resolution order.

### Bugfix Flow

TopDownLens reuses the `lens-core-bugfix` pattern as its standard correction loop:

- Defects surfaced in published TopDownLens artifacts route through a governed bugfix conductor, not direct edits in `nextlens-release`.
- Each bugfix produces a feature.yaml-style record, evidence, and a governed publish step, so the release repo only ever receives reviewed changes.
- Salmon signals with severity `high` or `blocking` can open a bugfix automatically once the module reaches dogfooding.

### GitHub Actions Surface

TopDownLens needs three pipelines from day one of self-hosting:

- A `promote-to-release` workflow that publishes the module payload from `nextlens-control` to `nextlens-release` on protected branches, mirroring the existing `promote-to-release.yml`.
- A `publish-to-governance` workflow that updates feature metadata, feature-index, and published artifacts in `nextlens-governance` only through approved orchestration boundaries.
- A regression / doctor pipeline that runs schema validation, derived-graph rebuild round-trip, and doctor checks on every PR.

### Dogfooding Acceptance Criteria

- The next TopDownLens feature after the first dev increment is created using TopDownLens commands, not by hand.
- That feature's planning artifacts pass TopDownLens doctor checks before FinalizePlan.
- A deliberately broken assumption triggers a Salmon signal that is verifiable in control-repo docs and routable through the bugfix flow.
- **Post-migration acceptance (not required for first dev increment):** `nextlens-release` is updated by GitHub Actions only (no manual file pushes); Salmon signals are observable in `nextlens-governance`. These criteria apply once `nextlens-governance` and `nextlens-release` repos are stood up.

### Non-Goals For This Express Feature

- Do not stand up `nextlens-governance` or `nextlens-release` as live repos inside this ExpressPlan. The express scope is the contract and migration plan; the repos are created during the first dev increment.
- Do not migrate existing Lens governance content; TopDownLens governance starts empty and grows as features land.
- Do not duplicate `lens-core-bugfix` logic; reuse the pattern via configuration rather than forking the skill.
