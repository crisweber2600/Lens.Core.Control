---
feature: lens-dev-new-codebase-next
doc_type: business-plan
status: approved
goal: "Define the business outcomes and parity guardrails for rewriting the next command through an express planning path."
key_decisions:
  - Use the express track for this feature so the Next command rewrite can move through one compact planning packet.
  - Treat the old-codebase public prompt as an entry-contract reference: prompt-start preflight must run before the release prompt is loaded.
  - Treat the baseline rewrite corpus as the behavioral authority for Next: single recommendation, blocker-first output, and pre-confirmed delegation.
  - Define output parity as equivalent routing decisions, blocker behavior, warnings, and delegated command handoff semantics, not prose-level copying.
  - Record the full-to-express lifecycle switch through the sanctioned feature-yaml operation before relying on expressplan automation.
open_questions:
  - Should paused historical feature state route through an internal recovery skill or stop with an explicit blocker now that pause-resume is outside the retained public surface?
  - Which exact regression fixture file should own express-track routing cases for next-ops.py?
  - Should the constitution resolver's express-track allow-list update land inside this feature or remain a prerequisite from the constitution work package?
depends_on:
  - TargetProjects/lens-dev/old-codebase/lens.core.src/.github/prompts/lens-next.prompt.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/2-6-rewrite-next.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/architecture.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/stories.md
blocks:
  - The target-project implementation must include focused routing fixtures before output parity can be claimed.
updated_at: 2026-04-30T20:47:21Z
---

# Business Plan - Next Command

## Executive Summary

The `lens-dev-new-codebase-next` feature restores the user-facing behavior of the Lens `next` command in the clean-room new codebase. The business value is workflow confidence: a user should not need to inspect lifecycle internals or choose among command menus when the feature state already determines the next action. `next` must identify one unblocked action, surface hard blockers before any handoff, and delegate immediately when the path is deterministic.

This feature is now planned through the express track. That changes the planning route, not the product promise. The rewritten `next` command still belongs to the retained 17-command surface and still has to preserve the old entry contract and the baseline routing behavior. The express path gives the feature a compact packet: this business plan, a technical plan, a sprint plan, and an expressplan review.

## Problem Statement

Without a working `next` command, Lens users are forced back into command selection and lifecycle interpretation. That defeats one of the core stability goals of the rewrite: make the retained surface feel intentional and safe after prompt reduction. If `next` becomes a menu, ignores blockers, or asks users to reconfirm a handoff they already requested, the workflow becomes slower and less trustworthy.

There is also a track-alignment issue. The feature was initialized on the full track even though the requested planning route is express. The lifecycle record has now been switched to `track: express` and `phase: expressplan` through the sanctioned feature-yaml tooling so this packet can be used by the expressplan path.

## Users And Stakeholders

- Lens users who want the system to route them to the next valid lifecycle action.
- Maintainers of the 17-command rewrite who need the navigation surface to stay predictable.
- Governance reviewers who need blockers and lifecycle gates to remain visible instead of bypassed.
- Implementation agents who need a precise parity target for the new `bmad-lens-next` skill and `next-ops.py` behavior.

## Goals

1. Preserve the prompt-start chain: public stub runs light preflight, stops on non-zero, then loads the release prompt.
2. Preserve the single-choice behavior: choose exactly one recommendation from lifecycle state.
3. Preserve blocker-first behavior: blockers prevent delegation and must be reported clearly.
4. Preserve pre-confirmed handoff: an unblocked `/next` invocation counts as consent to enter the delegated phase.
5. Add explicit express-track coverage: express features route to `/expressplan` and `expressplan-complete` routes to `/finalizeplan`.
6. Avoid direct governance mutation from the command. `next` reads state and delegates; phase conductors own writes.

## Non-Goals

- Rebuilding downstream phase conductors inside `next`.
- Turning `next` into a menu or multi-command advisor.
- Copying old-codebase skill prose into the new implementation.
- Changing lifecycle schema, feature identity, branch topology, or governance docs path rules.
- Solving all historical paused-state recovery semantics without an explicit retained-surface decision.

## Clean-Room Source Packet

This plan uses the old-codebase public prompt only for the entry contract: run light preflight first, stop on failure, then load the release prompt. The retained behavior comes from the baseline Next story and rewrite architecture: lifecycle authority, blocker-first routing, single recommendation, and pre-confirmed delegation.

No old-codebase files are copied into this feature. The implementation should be authored from the behavior contract and verified by routing fixtures.

## Output Parity Definition

The new skill achieves output parity when it produces equivalent observable results for the same feature states:

| Input state | Required observable output |
| --- | --- |
| `phase: preplan`, `track: full` | Recommend and delegate `/preplan` if unblocked |
| `phase: expressplan`, `track: express` | Recommend and delegate `/expressplan` if unblocked |
| `phase: expressplan-complete`, `track: express` | Recommend and delegate `/finalizeplan` from lifecycle `auto_advance_to` |
| Missing phase with `track: express` | Use lifecycle track `start_phase: expressplan` |
| Hard blockers present | Report blockers and do not delegate |
| Stale context or many open problems | Warn briefly without hiding blockers |
| Unknown or unsupported state | Fail clearly rather than guessing |

Parity is not word-for-word content reuse. It is matching route selection, blocker handling, warnings, JSON/script result shape where applicable, and the delegated skill handoff contract.

## Success Criteria

1. `lens-next` is part of the clean-room public prompt surface and resolves through the standard stub -> release prompt -> skill chain.
2. The owning skill runs `next-ops.py suggest` before any handoff decision.
3. The routing engine reads lifecycle metadata for phases, tracks, and `auto_advance_to` behavior.
4. Exactly one unblocked action is selected.
5. Blockers stop delegation.
6. Express-track routing cases are covered by regression fixtures.
7. Delegated skills are entered without a redundant launch confirmation.

## Risks And Mitigations

| Risk | Impact | Mitigation |
| --- | --- | --- |
| `next` delegates to a command removed from the retained public surface | Users hit a dead command path | Add paused-state handling as an explicit implementation decision and regression case |
| Express track is present in lifecycle and feature-yaml but filtered by constitution tooling | Expressplan automation may fail despite governance prose allowing it | Treat resolver allow-list parity as a prerequisite or scoped fix before automated express validation |
| Output parity is checked only by prose review | Regressions can slip through route-specific cases | Add fixture-based tests for full, express, complete, paused, stale, blocker, and unknown states |
| Delegated phase skills re-ask for launch confirmation | `/next` feels inefficient and violates the handoff contract | Add a handoff contract test and carry a `preconfirmed` context flag or equivalent convention |

## Exit Decision

Proceed on the express planning path. The business target is a stable, predictable `next` command that keeps Lens moving by choosing one valid next action and delegating only when the path is unblocked.