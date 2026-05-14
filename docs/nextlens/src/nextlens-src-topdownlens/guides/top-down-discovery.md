---
feature: nextlens-src-topdownlens
doc_type: guide
story_id: TL-2
title: Top-Down Discovery Guide
updated_at: 2026-05-14T04:50:00Z
---

# Top-Down Discovery Guide

Top-down discovery starts from a system thesis and narrows toward one selected feature. It does not begin with domain, service, repo, or implementation topology. Those may appear later as implementation consequences.

## Procedure

1. Capture raw context in the user's language.
2. Extract a system thesis: the coherent product, operating model, or solution boundary being changed.
3. Map product areas inside the system.
4. Identify roles and stakeholders affected by the system.
5. Name measurable outcomes those actors need.
6. Map journeys that make the outcomes real.
7. Identify capability candidates required by the journeys.
8. Select one vertical feature that can be handed to BMAD.
9. Build a BMAD packet with source context, include/exclude boundaries, traceability IDs, and acceptance evidence.
10. Label candidate services or repos only as implementation consequences, not as planning roots.

## Traceability Minimum

A selected top-down feature should reference these IDs when known:

- `system.<slug>`
- `product_area.<slug>`
- at least one actor or role in the walkthrough prose
- `outcome.<slug>`
- `journey.<slug>`
- `capability.<slug>`
- `feature.<slug>`
- at least one `evidence.<slug>`

Paths can appear as context, but durable references use IDs.

## Service Boundary Rule

Candidate services are implementation consequences. They are allowed to appear after feature selection when implementation impact is analyzed. They must not replace product-area, outcome, journey, capability, or feature discovery.

## BMAD Handoff

Use `schemas/bmad-packet.schema.json` for the final handoff. The packet should target exactly one feature and must not pre-author BMAD PRD, architecture, epics, stories, implementation, or review artifacts.