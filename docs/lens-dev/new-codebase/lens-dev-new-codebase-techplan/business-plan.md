---
feature: lens-dev-new-codebase-techplan
doc_type: business-plan
status: draft
goal: "Define the business outcomes and delivery guardrails for rewriting the techplan command through an expressplan-compatible planning set."
key_decisions:
  - Use expressplan as the planning path for this feature while preserving the rewritten `techplan` command as a governed conductor.
  - Treat the old-codebase `lens-techplan` prompt as an entry-contract reference only; baseline rewrite docs define behavior.
  - Stage `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, and `expressplan-adversarial-review.md` in this feature folder as the authoritative planning set.
  - Record governance state changes only through the sanctioned `feature-yaml` flow; the express track switch has already been applied there.
  - Treat discovery wiring, focused test-harness ownership, and parity definition as first-slice acceptance criteria rather than deferred follow-up work.
open_questions:
  - Which exact discovery file should register `lens-techplan` first in the target project: installer wiring, module help, or another retained manifest surface?
  - Which exact focused test path should own prompt-start and wrapper-equivalence coverage when the first implementation slice lands?
depends_on:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/prd.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-3-rewrite-techplan.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-5-rewrite-expressplan.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/architecture.md
blocks:
  - End-to-end execution remains blocked until this feature lands the shared governance publication, wrapper, review, and constitution surfaces it now owns.
updated_at: 2026-04-29T02:08:41Z
---

# Business Plan — Techplan Command

## Executive Summary

The `lens-dev-new-codebase-techplan` feature still exists to restore the missing `techplan` command in the new codebase. What changes in this planning pass is the route, not the delivery target. Instead of treating the feature as a long full-track planning exercise, this folder must act as a clean, expressplan-compatible planning set: one business plan, one tech plan, one sprint plan, and one adversarial review. That gives the rewrite a compressed planning path while keeping the actual `techplan` command contract intact.

The user-facing outcome remains continuity. A Lens user must still get a governed `techplan` conductor that publishes reviewed predecessor artifacts, enforces PRD traceability, and delegates architecture authoring through the Lens wrapper. The express path is only the planning mechanism for this feature. It is not a license to turn the rewritten `techplan` command into an express-only shortcut.

## Problem Statement

This feature folder was previously written as if the feature had to stay on the full planning path. That created three concrete problems:

1. The staged artifacts contradicted the requested `expressplan` route.
2. `sprint-plan.md` was missing, so the expressplan output set was incomplete.
3. The business narrative treated express planning as a violation instead of a valid way to compress planning for this feature.

The result was an unusable middle state: the folder looked partially like an expressplan output set, but its own prose argued against using expressplan at all. That makes it a poor clean-room input for future implementation and a poor parity target for the eventual `bmad-lens-expressplan` rewrite.

## Users And Stakeholders

- Lens feature owners who need a coherent planning packet for the techplan rewrite.
- Lens maintainers proving the rewrite can preserve retained-command behavior under an express planning path.
- Governance reviewers who need staged artifacts that align with the requested lifecycle route.
- Future implementation agents who need a clean-room, contradiction-free plan for the target project work.

## Goals

1. Produce a complete expressplan-compatible planning set for the `techplan` rewrite: `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, and `expressplan-adversarial-review.md`.
2. Preserve the baseline `techplan` contract: publish-before-author ordering, PRD reference enforcement, conductor-only delegation, and no direct governance writes.
3. Keep the clean-room boundary explicit: baseline rewrite docs define behavior; the old prompt stub only confirms the public chain shape.
4. Define output parity explicitly: the future new-codebase skill must reproduce the same four staged artifacts with equivalent routing, gates, and delivery slices.

## Non-Goals

- Changing the rewritten `techplan` command into an express-only runtime.
- Directly editing governance repo metadata from these staged docs.
- Copying implementation prose from old-codebase prompt or skill files.
- Solving unrelated retained-command gaps outside the `techplan` rewrite.

## Required Outcomes

### User Outcomes

- The planning packet clearly separates the feature’s implementation target from the planning path used to author it.
- Future implementation work can recreate the expected `techplan` behavior without reopening the clean-room question.
- The future expressplan rewrite has a concrete parity target for this feature’s staged outputs.

### Governance Outcomes

- The staged docs no longer contradict the requested express path.
- Governance track alignment is completed through the sanctioned `feature-yaml` flow rather than left as a hypothetical follow-up.
- The review packet can be evaluated as a complete expressplan artifact set.

### Delivery Outcomes

- The new-codebase target project gets a sequenced plan for the missing `techplan` command surface.
- The first implementation slice explicitly owns command discovery wiring and focused test-harness setup.
- The work is decomposed into implementation slices, widened shared-utility delivery, and validation gates.

## Scope

### In Scope

- Reframing this feature’s planning packet to use the expressplan artifact shape.
- Defining the target-project implementation slice for the missing `techplan` prompt chain and owning skill.
- Folding the missing shared utility surfaces needed for end-to-end execution into this feature’s implementation scope.
- Preserving the retained-command behavior documented in the baseline rewrite corpus.
- Naming the governance and dependency prerequisites needed before automation can rerun on the express path.

### Out Of Scope

- Reclassifying the runtime semantics of `techplan` itself.
- Direct governance writes outside the sanctioned `feature-yaml` and git orchestration flows.
- Implementing the target-project code during this planning pass.
- Reworking unrelated lifecycle contracts or feature identity rules.

## Clean-Room Source Packet

This plan is derived from the following approved inputs:

- `TargetProjects/lens-dev/old-codebase/lens.core.src/.github/prompts/lens-techplan.prompt.md`
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/prd.md`
- `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-3-rewrite-techplan.md`
- `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-5-rewrite-expressplan.md`
- `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/architecture.md`
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/research.md`
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/brainstorm.md`

## Success Criteria

This feature is successful when:

1. The feature folder contains a coherent expressplan-compatible artifact set with no contradictory track statements.
2. The technical plan names the target-project files, absorbed shared utility surfaces, sequence, and validations required to restore `techplan` parity.
3. The sprint plan decomposes the work into reviewable implementation slices and assigns discovery and test-harness acceptance items to the first code slice.
4. Output parity is defined as reproducing the same four staged artifacts with equivalent routing, gates, and delivery slices.

## Risks And Mitigations

| Risk | Impact | Mitigation |
| --- | --- | --- |
| Folding missing shared utilities into this feature widens the delivery slice | Scope expansion can slow implementation or blur ownership | Keep the absorbed utility work grouped into explicit implementation slices and retain no direct governance writes from `techplan` |
| The express planning route is mistaken for a change to `techplan` runtime behavior | Implementation scope drifts | Re-state throughout the packet that express changes the planning path, not the command contract |
| Discovery or test-harness work is still treated as optional | The command lands but stays undiscoverable or weakly validated | Put discovery wiring and focused test-harness setup in the first implementation slice acceptance criteria |
| Clean-room discipline erodes into copy/paste | Rewrite loses defensibility | Use baseline rewrite docs as the normative source and old prompt material only as an entry-contract reference |

## Exit Decision

Proceed on the express planning path for this feature. The staged docs are now the source of truth for planning, and the governance feature record has been aligned to `track: express` through the sanctioned `feature-yaml` flow. The remaining work is implementation scope, not governance track correction.