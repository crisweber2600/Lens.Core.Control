---
feature: nextlens-src-topdownlens
doc_type: guide
story_id: TL-4
title: BMAD Bridge Packet Guide
updated_at: 2026-05-14T04:35:00Z
---

# BMAD Bridge Packet Guide

TopDownLens hands BMAD one selected vertical feature at a time. The bridge packet is a context envelope, not a replacement for BMAD planning artifacts.

## Boundary

LENS owns:

- Selecting exactly one feature for BMAD execution.
- Providing source context, scope boundaries, traceability, and acceptance evidence.
- Identifying known outcomes, journeys, capabilities, and evidence by stable ID.
- Stating what is excluded so BMAD does not expand into adjacent product work.

BMAD owns:

- PRD creation.
- Architecture creation.
- Epic and story generation.
- Implementation.
- Review and retrospective artifacts.

The packet may name the required BMAD artifact types, but it must not pre-author those artifacts. That keeps LENS focused on coherence and keeps BMAD responsible for buildable execution detail.

## Packet Rules

- `selected_feature.id` is the only feature BMAD should plan from the packet.
- `scope_boundaries.include` names what is in scope.
- `scope_boundaries.exclude` names adjacent work BMAD must not pull into the plan.
- `traceability` links BMAD work back to outcomes, journeys, capabilities, and evidence when those IDs are known.
- `acceptance_evidence` maps criteria to evidence IDs so downstream implementation and Salmon validation can point to durable records.
- Paths are mutable context. Stable IDs are the durable references.

## Minimal Handoff Flow

1. TopDownLens identifies the selected feature and records its stable ID.
2. TopDownLens gathers source context and relevant landscape entities.
3. TopDownLens writes a packet that validates against `schemas/bmad-packet.schema.json`.
4. BMAD receives the packet as input and produces its normal planning and implementation artifacts.
5. LENS records produced artifacts and downstream evidence back into the feature archive and landscape.