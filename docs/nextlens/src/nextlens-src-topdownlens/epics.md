---
feature: nextlens-src-topdownlens
doc_type: epics
status: draft
track: express
phase: finalizeplan
depends_on: [business-plan, tech-plan, sprint-plan]
blocks: []
updated_at: 2026-05-14T04:00:00Z
---

# Epics - TopDownLens Module

Three epics organize the 12 work packages from `sprint-plan.md` into coherent delivery slices. The first dev increment is constrained to the spine (TL-1, TL-4, TL-8, TL-9, TL-12) plus supporting stories (TL-2, TL-3, TL-6, TL-10, TL-11). TL-5 and TL-7 are deferrable beyond the first increment.

## Epic 1 - Core Contracts And Schemas

**Goal:** Establish the durable data contracts that the rest of TopDownLens builds on.

**Stories:**
- TL-1 - Module Ontology And Storage Contract (spine, schema)
- TL-4 - BMAD Bridge Packet (spine, schema)
- TL-6 - Salmon Signal Contract (supporting, schema)

**Why first:** No other work can be built without stable IDs, packet shape, and signal shape. These three define the file formats every other story reads or writes.

**Done when:**
- Schemas exist for ontology entities, BMAD packet, and Salmon signals.
- Stable-ID rule and source-of-truth invariant are enforced by validators.
- At least one example artifact per schema is checked into `docs/nextlens/src/nextlens-src-topdownlens/`.

## Epic 2 - Self-Hosting And Governance

**Goal:** Encode the repo topology, constitution layering, bugfix loop, and pipelines that let TopDownLens publish itself without bypassing governance.

**Stories:**
- TL-8 - Self-Hosting Repo Topology Contract (spine, docs-only)
- TL-9 - Constitution Layering For TopDownLens (spine, docs-only)
- TL-10 - Bugfix Flow (supporting, docs-only)
- TL-11 - GitHub Actions Pipelines (supporting, cli)

**Why second:** Once contracts exist they need a place to live, a constitution to govern them, and pipelines that publish only through the approved boundary. This epic produces the rules and the automation that prevent direct governance or release patches.

**Done when:**
- Write-scope boundaries for `nextlens-control` / `nextlens-governance` / `nextlens-release` are documented.
- Constitution scaffolds for `nextlens` and `nextlens/src` resolve cleanly (or are explicitly deferred with rationale).
- Bugfix lifecycle and Salmon-to-bugfix routing rules are documented.
- `promote-to-release`, `publish-to-governance`, and `regression-and-doctor` workflows exist and fail closed.

## Epic 3 - Top-Down Flow And Validation

**Goal:** Deliver the top-down discovery walkthrough, bottom-up compatibility rules, optional derived graph and doctor tooling, and the dogfooding acceptance run that closes the increment.

**Stories:**
- TL-2 - Top-Down Discovery Walkthrough (supporting, docs-only)
- TL-3 - Bottom-Up Compatibility Rules (supporting, docs-only)
- TL-5 - Minimal Derived Graph Rebuild (deferrable, cli)
- TL-7 - Doctor Checks (deferrable, cli)
- TL-12 - Dogfooding Acceptance Run (spine, test)

**Why last:** This epic exercises the contracts and governance plumbing end-to-end. TL-12 is the explicit closure gate for the first increment. For the first run, TL-12 acceptance is relaxed to skip `nextlens-release` verification because that repo does not yet exist.

**Done when:**
- The top-down walkthrough is documented with at least one worked example.
- Bottom-up rules are encoded in planning guidance.
- TL-12 produces planning artifacts for the next TopDownLens feature using TopDownLens commands and surfaces at least one routable Salmon signal.

## Out Of Scope For First Increment

- A full live `nextlens-governance` or `nextlens-release` repo (incubation remains in `Lens.Core.Governance`).
- Implementation of the full future Lens system in one pass (TopDownLens module only).
- Unattended extraction from raw notes (assisted synthesis only).

## Traceability

Each story file under `stories/TL-*.md` carries `depends_on`, `implementation_kind`, and acceptance criteria traceable back to the work-package definitions in `sprint-plan.md` and the architecture in `tech-plan.md`.
