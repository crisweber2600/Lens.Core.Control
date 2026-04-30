---
feature: lens-dev-new-codebase-next
doc_type: sprint-plan
status: approved
goal: "Sequence the Next command rewrite into implementation-ready slices with express routing and parity validation."
key_decisions:
  - Use four delivery slices: express alignment, prompt chain, routing engine parity, and handoff/regression hardening.
  - Include express-track fixtures in the routing-engine slice, not as a late regression add-on.
  - Keep paused-state handling as a named decision gate before release readiness.
  - Keep constitution resolver express support as either a direct dependency or a scoped fix, but do not duplicate constitution logic inside Next.
  - Treat output parity as fixture-backed observable behavior.
open_questions:
  - Which retained recovery behavior should handle paused features?
  - Which test file should become the canonical next-ops parity suite?
  - Should module discovery wiring land in the first code slice or after the routing engine is complete?
depends_on:
  - business-plan.md
  - tech-plan.md
blocks:
  - expressplan-adversarial-review.md
updated_at: 2026-04-30T20:47:21Z
---

# Sprint Plan - Next Command

## Sprint Objective

Deliver a clean-room `next` command that preserves the retained behavior contract, supports express-track routing, and proves output parity through focused fixtures rather than prose review alone.

## Current Packet Status

- Feature state has been switched to `track: express` and `phase: expressplan` through feature-yaml tooling.
- The staged docs now define the requested expressplan path for the Next feature.
- The implementation remains future target-project work; this packet is the planning and review handoff.

## Delivery Slices

| Slice | Objective | Exit Criteria |
| --- | --- | --- |
| Slice 1 | Express planning alignment | Feature track is express; business, tech, sprint, and review artifacts exist |
| Slice 2 | Prompt chain and discovery | Public stub, release prompt, owning skill shell, and discovery surface are present |
| Slice 3 | Routing engine parity | next-ops fixtures cover full, express, complete, missing phase, blocker, warning, paused, and unknown states |
| Slice 4 | Delegation and release hardening | Pre-confirmed handoff and no-write boundaries are tested; paused and constitution decisions are closed |

## Slice 1 - Express Planning Alignment

### Scope

- Switch the feature from full track to express track through the sanctioned feature-yaml operation.
- Author the expressplan packet from clean-room inputs.
- Run the expressplan adversarial review and record response options.

### Deliverables

- `business-plan.md`
- `tech-plan.md`
- `sprint-plan.md`
- `expressplan-adversarial-review.md`
- Governance feature state aligned to `track: express`

### Exit Criteria

- The packet no longer routes the feature through the full planning path.
- The review has no unresolved fail-level finding.

## Slice 2 - Prompt Chain And Discovery

### Scope

- Create the public `lens-next` prompt stub in the target project.
- Create the release prompt redirect.
- Scaffold `bmad-lens-next/SKILL.md` as a thin conductor.
- Register or verify `next` in the retained discovery surfaces.

### Acceptance Criteria

- The stub runs light preflight before the release prompt.
- The release prompt contains no inline routing logic.
- The skill contract says `next-ops.py suggest` runs before delegation.
- `next` remains discoverable as one of the retained commands.

## Slice 3 - Routing Engine Parity

### Scope

- Implement or port `next-ops.py` from the clean-room behavior contract.
- Add fixture-backed parity cases.
- Use lifecycle.yaml for phases, tracks, and auto-advance commands.

### Acceptance Criteria

- Full-track `preplan` routes to `/preplan`.
- Express-track `expressplan` routes to `/expressplan`.
- `expressplan-complete` routes to `/finalizeplan`.
- Missing phase uses lifecycle track start phase.
- Blockers stop delegation.
- Warnings do not hide blockers.
- Unknown states fail clearly.
- Paused-state behavior follows the selected retained-surface decision.

## Slice 4 - Delegation And Release Hardening

### Scope

- Add the pre-confirmed handoff mechanism or equivalent context propagation.
- Verify delegated phase skills are not asked for redundant launch confirmation.
- Confirm `next` remains read-only.
- Resolve or depend on constitution resolver express-track support before automated express validation is declared usable.

### Acceptance Criteria

- Unblocked recommendations load the target skill immediately.
- Blocked recommendations do not load downstream skills.
- `next` produces no governance or control-doc writes.
- The express-track allow-list issue has an owner and regression expectation.

## Critical Path

1. Close the paused-state route decision.
2. Land prompt chain and discovery wiring.
3. Land deterministic routing fixtures, including express cases.
4. Land pre-confirmed handoff behavior.
5. Verify no-write behavior and release discovery consistency.

## Definition Of Ready For Implementation

The feature is ready for code work when this packet is accepted, the paused-state decision is assigned, and the target test path for `next-ops.py` fixtures is selected.

## Definition Of Done

The feature is done when `lens-next` is installed, discoverable, route-correct for full and express tracks, blocker-first, pre-confirmed for delegation, and covered by focused regression fixtures.