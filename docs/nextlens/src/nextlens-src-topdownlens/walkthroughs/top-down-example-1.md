---
feature: nextlens-src-topdownlens
doc_type: walkthrough
story_id: TL-2
title: Top-Down Example 1
updated_at: 2026-05-14T04:50:00Z
---

# Top-Down Example 1

## Raw Context

The team wants NextLens to help an operator start from a broad product idea, understand the system and user outcomes, select one useful feature, hand it to BMAD, and preserve evidence when implementation changes upstream assumptions.

## System Thesis

Stable ID: `system.nextlens`

NextLens is a system for turning ambiguous product intent into traceable BMAD-ready feature work and then feeding downstream evidence back into the landscape.

## Product Area Map

Stable ID: `product_area.planning_intelligence`

Planning Intelligence owns the path from raw context to selected feature. It does not own implementation repos directly; implementation repos are consequences of the selected feature's impact analysis.

## Roles And Stakeholders

- Operator: asks Lens to decompose an idea and keep the work coherent.
- Product lead: needs the selected feature to tie back to outcomes and evidence.
- BMAD agent: needs a focused packet instead of a broad system essay.
- Developer: needs scope boundaries and validation evidence.

## Outcome Map

Stable ID: `outcome.bmad_ready_feature`

The operator has one selected vertical feature with enough context for BMAD to plan without accidentally pulling in adjacent scope.

## Journey Map

Stable ID: `journey.capture_to_packet`

1. Capture raw product context.
2. Extract system thesis and product areas.
3. Name roles and outcomes.
4. Map the journey that realizes the outcome.
5. Identify capability candidates.
6. Select one feature.
7. Prepare a BMAD packet.

## Capability Candidate

Stable ID: `capability.topdown_discovery`

Top-down discovery is a candidate capability because this feature creates one instance of the discovery path. It should not be promoted beyond candidate until repeated features create pressure around the same durable ability.

## Selected Feature

Stable ID: `feature.topdownlens_contract`

Selected feature: define the first TopDownLens contract for ontology, storage topology, graph projection, Salmon signals, BMAD bridge, and self-hosting boundaries.

## Evidence

Stable ID: `evidence.tl1_schema_validation`

Evidence source: TL-1 schema and example validation. This proves the first entity and graph contracts exist and can be referenced by ID.

## Candidate Services

Candidate services are implementation consequences, not planning roots:

- `nextlens-control` may become the planning workspace.
- `nextlens-governance` may become the authority for feature metadata and published docs.
- `nextlens-release` may become the read-only module payload surface.

These service candidates do not replace `system.nextlens`, `product_area.planning_intelligence`, `outcome.bmad_ready_feature`, `journey.capture_to_packet`, `capability.topdown_discovery`, or `feature.topdownlens_contract`.

## BMAD Packet Reference

The handoff packet for this selected feature is represented by `examples/bmad-packet-example.json` and validates against `schemas/bmad-packet.schema.json`.