---
storyId: SB2
title: "Author aspire-plan.md (C2 Aspire Mandate hard gate)"
epic: E6
feature: coffee-store-service-mvp
type: spike
priority: BLOCKER
points: 5
sprint: 0
status: not-started
owner: TBD
blocked_reason: "None — this story IS the blocker. It must be DONE before dev-ready and before S5.2."
blocks:
  - "dev-ready phase promotion (milestone.yaml phase: dev-ready)"
  - "S5.2 — Register StoreOperationsService in Aspire AppHost"
---

# SB2 — Author `aspire-plan.md` (C2 Aspire Mandate Hard Gate)

## Context

The Aspire Mandate clause in the coffee domain constitution states:

> All coffee services MUST use the shared .NET Aspire AppHost.

No `aspire-plan.md` currently exists for store-service/mvp. Until this SPIKE is done:

1. **The milestone cannot advance to `dev-ready`** (hard gate)
2. **S5.2 (Aspire AppHost registration) cannot be implemented** — developers do not know which AppHost to register in, or what resource binding pattern to use

## User Story

**As a** coffee Aspire Mandate owner,  
**I want** an `aspire-plan.md` authored for the store-service/mvp milestone,  
**so that** the Aspire Mandate gate is satisfied and developers know exactly how to register StoreOperationsService in the shared AppHost.

## Key Questions to Resolve

This SPIKE must answer:

1. **Which AppHost?** — Which existing Aspire AppHost project does StoreOperationsService register in? (e.g., is there a shared `CoffeeAppHost` or does each service have its own?)
2. **Resource bindings** — What resources does StoreOperationsService need? (SQL Server / PostgreSQL, message broker, secrets?)
3. **Service naming convention** — What name is used in the AppHost `AddProject<>()` call?
4. **Health check endpoint** — What path does Aspire monitor for service readiness?
5. **Connection string strategy** — Aspire-managed connection strings vs. `appsettings.json`?
6. **Local dev smoke test** — What does `aspire start` need to show for service validation?

## Acceptance Criteria

- [ ] `docs/coffee/store-service/mvp/aspire-plan.md` created
- [ ] `doc_type: aspire-plan` in frontmatter
- [ ] `feature: coffee-store-service-mvp` in frontmatter
- [ ] Sections present:
  - `## AppHost Project` — which project, and reference to it in the solution
  - `## Service Registration` — exact `builder.AddProject<StoreOperationsService>()` pattern
  - `## Resource Bindings` — all required resources with names and Aspire resource type
  - `## Health Check` — endpoint path and Aspire health check configuration
  - `## Connection Strings` — strategy for each resource binding
  - `## Local Dev Validation` — step-by-step to verify `aspire start` shows StoreOperationsService healthy
- [ ] `aspire-plan.md` reviewed and approved by the coffee Aspire Mandate owner
- [ ] S5.2 acceptance criteria unblocked (developer can implement from the aspire-plan alone)

## Definition of Done

- `aspire-plan.md` committed and pushed to `mvp` branch
- Coffee constitution Aspire Mandate gate satisfied
- S5.2 unblocked
- This story closed in `sprint-status.yaml` with status `done`

## Notes

Check `TargetProjects/coffee/` for existing Aspire AppHost projects before authoring.  
The aspire-plan is a design document, not a code story. Implementing the actual AppHost changes is S5.2.
