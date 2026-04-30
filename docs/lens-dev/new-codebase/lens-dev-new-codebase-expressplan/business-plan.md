---
feature: lens-dev-new-codebase-expressplan
doc_type: business-plan
status: draft
goal: "Define the business outcomes and delivery guardrails for rewriting the expressplan command through an expressplan-compatible planning set."
key_decisions:
  - Use the express track to plan this feature, consistent with the same pattern used for lens-dev-new-codebase-techplan.
  - Treat the old-codebase lens-expressplan prompt as a chain-shape reference only; baseline story 4.5 defines behavior.
  - Stage business-plan.md, tech-plan.md, sprint-plan.md, and expressplan-review.md as the authoritative planning set.
  - Record governance state changes only through the sanctioned feature-yaml flow; the express track switch has already been applied.
  - The previous session landed the skill infrastructure; this feature's implementation scope is the remaining completion, wiring, and validation.
open_questions:
  - Does expressplan need a dedicated bmad-lens-expressplan/scripts/expressplan-ops.py, or is a conductor-only skill sufficient without a supporting script?
  - Which discovery surface in the target project should register lens-expressplan first?
  - Which focused test path should own prompt-start regression coverage beyond the current test-expressplan-ops.py?
depends_on:
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-5-rewrite-expressplan.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-techplan/docs/business-plan.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/prd.md
blocks:
  - End-to-end expressplan execution in the new codebase is blocked until this feature lands discovery wiring and targeted regression coverage.
updated_at: '2026-04-30T00:00:00Z'
---

# Business Plan — Expressplan Command

## Executive Summary

The `lens-dev-new-codebase-expressplan` feature restores the missing expressplan command
surface in the new codebase. The previous session created the skill infrastructure — prompt
stubs, the conductor skill, tests, and module registration — but the command is not yet
discoverable end-to-end nor regression-covered at the integration level. This planning pass
describes the remaining delivery, validates the already-landed foundation, and records the
expressplan artifact set using the same express-planning shape that was established as the
reference for this initiative in `lens-dev-new-codebase-techplan`.

The user-facing outcome is continuity: a Lens user running `/expressplan` on an express-track
feature must get the governed expressplan conductor, which enforces track eligibility, delegates
to QuickPlan via the Lens wrapper, runs adversarial review with party mode, and advances to
FinalizePlan on a passing verdict. The express path is the planning route for this feature; it
is not a change to the expressplan command contract.

## Problem Statement

Before this feature, the new codebase had no expressplan command surface at all. The old
codebase prompt was a public stub only; no substantive conductor skill or internal routing
existed in the target project. The new codebase source tree (`TargetProjects/lens-dev/new-codebase/lens.core.src`)
was therefore incomplete: users who reached `/expressplan` had no skill to execute against.

The previous session created the skill infrastructure using a clean-room approach derived from
baseline story 4.5 and the techplan express packet. What remains is:

1. Confirming the landed implementation is complete and correct.
2. Wiring the retained command discovery surface for `lens-expressplan`.
3. Establishing targeted regression coverage at the integration level.
4. Completing the expressplan planning packet so finalizeplan can proceed.

## Users and Stakeholders

- Lens feature owners on express track who need `/expressplan` to produce governed planning artifacts.
- Lens maintainers proving the new-codebase rewrite can enforce track eligibility and delegate
  to QuickPlan without bypassing the review gate.
- Governance reviewers who need a complete expressplan planning packet as the parity target for
  verifying the expressplan command surface.
- Future implementation agents who can use this packet to recreate the expected behavior without
  reopening the clean-room question.

## Goals

1. Confirm and complete the expressplan command surface in the new codebase:
   `lens-expressplan.prompt.md` (public), `lens-expressplan.prompt.md` (release), and
   `bmad-lens-expressplan/SKILL.md` (conductor).
2. Preserve the baseline 4.5 contract: express-only eligibility gate, QuickPlan delegation via
   the Lens wrapper, hard-stop adversarial review, and auto-advance to FinalizePlan.
3. Keep the clean-room boundary explicit: baseline 4.5 and the techplan packet define behavior;
   the old prompt stub only confirms the public chain shape.
4. Define output parity explicitly: the new skill must reproduce the same four staged artifacts —
   `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, `expressplan-review.md` —
   with equivalent routing, gates, and delivery slices.

## Non-Goals

- Changing the expressplan command runtime behavior beyond what baseline 4.5 defines.
- Directly editing governance repo metadata from staged docs.
- Copying implementation prose from old-codebase prompt or skill files.
- Solving unrelated retained-command gaps outside the expressplan surface.
- Adding inline planning logic to the conductor skill (conductor-only is the constraint).

## Required Outcomes

### User Outcomes

- A Lens user on the express track can invoke `/expressplan` and receive a governed conductor
  that enforces all baseline 4.5 obligations.
- The command surface is registered in the new-codebase discovery surface so it appears in help.

### Governance Outcomes

- The feature folder contains a coherent expressplan-compatible planning artifact set.
- Governance track alignment is confirmed: `track: express`, `phase: expressplan`.
- No governance files are written directly; all state changes go through sanctioned flows.

### Delivery Outcomes

- The landed skill infrastructure is verified as complete: prompt stubs, conductor skill, tests,
  and module registration.
- Discovery wiring is added so `lens-expressplan` is discoverable.
- Focused regression expectations are defined and passed.

## Scope

### In Scope

- Validating and completing the already-landed expressplan skill surface.
- Adding the retained command discovery acceptance item for `lens-expressplan`.
- Adding focused regression coverage beyond the existing test file.
- Producing the full expressplan planning packet for this feature.

### Out of Scope

- Reimplementing any retained Lens commands other than expressplan.
- Changing the governance model or lifecycle contract.
- Implementing the QuickPlan or adversarial-review skills (those are independent features).

## Clean-Room Source Packet

This plan is derived from the following approved inputs:

- `TargetProjects/lens-dev/old-codebase/lens.core.src/.github/prompts/lens-expressplan.prompt.md` (chain-shape reference only)
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/prd.md`
- `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-5-rewrite-expressplan.md`
- `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-techplan/docs/business-plan.md`
- `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-techplan/docs/tech-plan.md`

## Success Criteria

This feature is successful when:

1. The feature folder contains a coherent expressplan-compatible artifact set with no
   contradictory track statements.
2. The conductor skill in the target project enforces all baseline 4.5 obligations.
3. The `lens-expressplan` command is registered in the new-codebase discovery surface.
4. The parity target is reproduced: four staged artifacts with equivalent routing, gates, and
   delivery slices.
5. The adversarial review carries no unresolved fail-level findings against this packet.
