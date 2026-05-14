---
feature: nextlens-src-topdownlens
story_id: TL-2
doc_type: story
status: done
title: Top-Down Discovery Walkthrough
depends_on: [TL-1, TL-4]
implementation_kind: docs-only
epic: 3
spine: false
updated_at: 2026-05-14T04:00:00Z
---

# TL-2 - Top-Down Discovery Walkthrough

## Goal

Document the flow from large product idea down to one selected feature, using TopDownLens entities defined in TL-1.

## Scope

- Raw context capture step.
- System thesis extraction.
- Product area map.
- Role and stakeholder map.
- Outcome map.
- Journey map.
- Capability candidates.
- Selected vertical feature.
- Focused BMAD packet hand-off (uses TL-4 schema).

## Acceptance

- The walkthrough does not begin with domain/service/feature; it begins with system thesis and product areas.
- At least one selected feature traces to system, product area, role, outcome, journey, capability candidate, and evidence.
- Candidate services are labeled as implementation consequences, not planning roots.
- Walkthrough is recorded in `docs/nextlens/src/nextlens-src-topdownlens/walkthroughs/top-down-example-1.md`.

## Files To Produce

- `docs/nextlens/src/nextlens-src-topdownlens/walkthroughs/top-down-example-1.md` (worked example using TopDownLens ontology).
- `docs/nextlens/src/nextlens-src-topdownlens/guides/top-down-discovery.md` (procedural guide).

## Notes For Dev

- Docs-only. No code or schemas authored here.
- Use TL-1 schemas to validate the example traceability chain.

## Dev Agent Record

- Status: done
- Files produced: `walkthroughs/top-down-example-1.md`, `guides/top-down-discovery.md`.
- Validation: local content check confirmed the walkthrough traces to system, product area, role, outcome, journey, capability, selected feature, evidence, and BMAD packet reference.
