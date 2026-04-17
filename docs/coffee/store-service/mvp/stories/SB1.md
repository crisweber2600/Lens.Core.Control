---
storyId: SB1
title: "Author business-plan.md (C1 hard gate)"
epic: E6
feature: coffee-store-service-mvp
type: spike
priority: BLOCKER
points: 5
sprint: 0
status: not-started
owner: TBD
blocked_reason: "None — this story IS the blocker. It must be DONE before dev-ready."
blocks:
  - "dev-ready phase promotion (milestone.yaml phase: dev-ready)"
---

# SB1 — Author `business-plan.md` (C1 Hard Gate)

## Context

The coffee domain constitution (`TargetProjects/lens/lens-governance/constitutions/coffee/constitution.md`) lists `business-plan` as a required planning artifact with `gate_mode: hard`. No `business-plan.md` currently exists for the store-service/mvp milestone.

Until this SPIKE is done, the milestone **cannot advance to `dev-ready`** regardless of code completion.

## User Story

**As a** coffee domain stakeholder,  
**I want** a `business-plan.md` authored for the store-service/mvp milestone,  
**so that** the coffee constitution hard gate is satisfied and the milestone can advance to `dev-ready`.

## Scope

- Document the business rationale for StoreOperationsService
- Identify target personas (Barista, Shift Lead, Expediter, Corporate Analyst)
- Quantify the operational problem being solved (queue visibility, rush handling, real-time throughput)
- Identify the cost of the build vs. the value delivered
- Provide a risk register for the business risks (not technical — see architecture.md for those)
- Document go/no-go acceptance criteria from a business perspective

## Acceptance Criteria

- [ ] `docs/coffee/store-service/mvp/business-plan.md` created
- [ ] `doc_type: business-plan` in frontmatter
- [ ] `feature: coffee-store-service-mvp` in frontmatter
- [ ] Sections present:
  - `## Problem Statement` — what store operations problem does this solve?
  - `## Value Proposition` — measurable operational improvements expected
  - `## Target Personas` — aligns with `product-brief.md` persona list
  - `## Revenue and Cost Assumptions` — build cost estimate + expected ROI or operational metric
  - `## Business Risk Register` — top 3–5 business risks with likelihood and mitigation
  - `## Go/No-Go Criteria` — conditions under which the MVP should be shipped vs. stopped
- [ ] Document reviewed by at least one named coffee domain stakeholder
- [ ] `milestone.yaml` `required_planning_artifacts.business_plan` updated to reflect presence

## Definition of Done

- `business-plan.md` committed and pushed to `mvp` branch
- Coffee constitution `required_planning_artifacts.business-plan` gate satisfied
- This story closed in `sprint-status.yaml` with status `done`
- No outstanding review comments on the document

## Notes

This is a discovery + authoring spike, not a code story. Output is a markdown document.  
Use `docs/coffee/store-service/mvp/product-brief.md` as input — it already contains persona analysis, operational workflows, and pain points that map to business-plan sections.
