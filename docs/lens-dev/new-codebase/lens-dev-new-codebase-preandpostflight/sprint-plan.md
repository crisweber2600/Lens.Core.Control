---
feature: lens-dev-new-codebase-preandpostflight
doc_type: sprint-plan
status: draft
goal: "Sequence the preflight cadence redesign into implementation-ready slices for request-time behavior, mutable sync policy, and validation hardening."
key_decisions:
  - Use three delivery slices: cadence split, request-lifecycle sync policy, and validation hardening.
  - Treat `lens.core` `develop` refresh as part of the first implementation slice because it changes default request behavior.
  - When `lens.core` leaves `develop`, release-derived refresh cadence downgrades automatically without an extra user setting.
  - Treat control and governance sync policy as a dedicated slice so mutable repo behavior is specified before implementation broadens.
  - Resolve request classification explicitly and allow touched-repo inference only as the execution fallback.
  - Treat read-only governance freshness as a warning path, not a hard blocker.
  - Treat post-request publish or push as the default for qualifying touched repos.
  - Keep daily and weekly hygiene as explicit outcomes rather than hidden timestamp side effects.
  - Defer any broader lifecycle or unrelated workflow redesign to follow-on features.
open_questions:
  - Which failure categories should hard-stop the request versus record warnings for later reconciliation?
depends_on:
  - business-plan.md
  - tech-plan.md
  - lens.core/_bmad/lens-work/skills/lens-preflight/scripts/preflight.py
blocks: []
updated_at: 2026-05-04T00:00:00Z
---

# Sprint Plan — PreAndPostFlight

## Sprint Objective

Turn preflight into a layered request lifecycle that always performs the right amount of work for the current request: minimal gates on every request, release-derived refresh on every request when `lens.core` tracks `develop`, and explicit mutable sync policy for control and governance before and after requests.

## Current Packet Status

- Feature state is `track: express`, `phase: expressplan-complete`.
- This packet is intended to supply the three required QuickPlan outputs before the express adversarial review gate.
- FinalizePlan handoff remains separate and should only occur after the express review records a non-fail verdict.

## Applied Predecessor Review Decisions

- Read-only requests warn on governance freshness instead of hard-blocking.
- Explicit request classification is part of PF-2.1; touched-repo inference is fallback behavior only.
- Post-request publish or push is the default for qualifying touched repos.
- The current preflight log stream remains the user-visible signal.
- The `lens.core` branch rule downgrades automatically when the release checkout leaves `develop`.

## Delivery Slices

| Slice ID | Slice | Objective | Exit Criteria |
| --- | --- | --- | --- |
| PF-1.1-cadence-split | Slice 1 | Separate every-request gates from periodic hygiene and implement the `lens.core` `develop` rule | Request-time gates, release-derived refresh policy, and daily or weekly hygiene responsibilities are clearly defined and implementation-ready |
| PF-2.1-sync-policy | Slice 2 | Define explicit pre-request and post-request control or governance sync behavior | Mutable repo sync has deterministic no-op, pull, commit, and publish rules with failure handling |
| PF-3.1-validation-hardening | Slice 3 | Add tests, observability, and rollout safeguards | Branch-sensitive refresh, touched-repo handling, and failure categories are validated and diagnosable |

## Slice 1 — Cadence Split

### Scope

- Preserve the lightweight prompt-start gate.
- Separate every-request validation from daily and weekly hygiene.
- Define the `lens.core` `develop` branch rule for release-derived refresh.
- Make timestamp behavior an explicit cadence mechanism rather than an implicit partial skip.

### Deliverables

- A defined every-request gate contract.
- A defined daily and weekly hygiene contract.
- A branch-sensitive release refresh rule that forces request-time refresh when `lens.core` is on `develop`.

### Risks

- The first slice may appear to solve preflight fully while mutable repo policy is still unresolved.
- Release refresh latency may become visible if the release mirror path is not kept narrow.

## Slice 2 — Explicit Request-Lifecycle Sync Policy

### Scope

- Define how requests are classified for mutable sync purposes.
- Define pre-request control repo and governance repo sync behavior.
- Define post-request touched-repo detection and sync behavior.
- Define which failures block request start, which failures block request completion, and which failures are warnings.

### Deliverables

- Deterministic policy for control repo sync.
- Deterministic policy for governance repo sync.
- No-op behavior for untouched repos.
- Clear boundaries for auto-commit, pull, rebase, and push behavior, including warning-only handling for read-only governance freshness.

### Dependencies

- Slice 1 must establish the layered runtime model first.
- The sync policy should consume, not redefine, the release-derived refresh behavior.

## Slice 3 — Validation Hardening And Rollout

### Scope

- Add focused regression coverage for branch-sensitive release refresh.
- Add regression coverage for fresh-timestamp behavior versus required request-time work.
- Add regression coverage for touched-versus-untouched repo behavior.
- Add logging and outcome reporting for pre-request and post-request sync stages.

### Deliverables

- Focused automated tests for cadence and sync policy.
- Outcome categories that distinguish no-op, refresh, pull-only, commit-and-sync, and failure.
- Rollout notes for adopting the new layered preflight behavior.

### Exit Gate

- The request lifecycle is understandable from logs and tests.
- The release refresh rule is enforced for `lens.core` `develop`.
- Mutable sync behavior is deterministic and no-ops when a request does not justify repo mutation.

## Critical Path

1. Define the layered preflight cadence and `lens.core` branch-sensitive refresh rule.
2. Define explicit control and governance sync policy before and after requests.
3. Add focused validation for branch-sensitive behavior, touched-repo handling, and failure outcomes.
4. Carry the reviewed packet into FinalizePlan only after the express adversarial review records a non-fail verdict.

## Definition Of Ready For Implementation

The feature is ready for code work when:

1. The business and technical plans agree on the layered request lifecycle.
2. The sprint slices distinguish release-derived refresh from mutable repo sync.
3. Control and governance sync policy has explicit success, no-op, and failure behavior.
4. Validation expectations are concrete enough to convert into focused tests without reopening scope.