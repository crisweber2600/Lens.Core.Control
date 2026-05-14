---
feature: nextlens-src-topdownlens
story_id: TL-1
doc_type: story
status: in-progress
title: Module Ontology And Storage Contract
depends_on: []
implementation_kind: schema
epic: 1
spine: true
updated_at: 2026-05-14T04:00:00Z
---

# TL-1 - Module Ontology And Storage Contract

## Goal

Define the core TopDownLens objects and where they live on disk.

## Scope

- `feature.yaml` shape extensions for TopDownLens.
- Relationship records (edges between hierarchy entities).
- Landscape entity records (system, product area, outcome, journey, capability, feature, story, task, code, test, evidence).
- Derived graph files (marked derived and rebuildable).
- Salmon signal records (file format only; lifecycle handled in TL-6).
- BMAD packet schema reference (full schema in TL-4).
- Hierarchy contract from system down to evidence.

## Acceptance

- Stable IDs are required for all durable entities.
- Paths are treated as mutable addresses; references resolve by ID.
- Graph files are marked `derived: true` and `rebuildable_from: <list>`.
- Feature archive, living landscape, and derived graph live in separate directories.
- Hierarchy levels explicitly named: system, product area, outcome, journey, capability, feature, story, task, code, test, evidence.
- At least one example artifact per entity type is checked into `docs/nextlens/src/nextlens-src-topdownlens/examples/`.

## Files To Produce

- `docs/nextlens/src/nextlens-src-topdownlens/schemas/ontology.schema.json` (or `.yaml`).
- `docs/nextlens/src/nextlens-src-topdownlens/schemas/relationship.schema.json`.
- `docs/nextlens/src/nextlens-src-topdownlens/schemas/landscape-entity.schema.json`.
- `docs/nextlens/src/nextlens-src-topdownlens/schemas/derived-graph.schema.json`.
- `docs/nextlens/src/nextlens-src-topdownlens/examples/` with one example per schema.

## Notes For Dev

- Source of truth: `business-plan.md` and `tech-plan.md` (TopDownLens Module Architecture section).
- All other stories depend on this contract. Do not start TL-2 / TL-3 / TL-4 / TL-6 until schemas are checked in.

## Dev Agent Record
