---
feature: lens-dev-new-codebase-preandpostflight
doc_type: business-plan
status: draft
goal: "Define the business outcomes and decision boundaries for turning Lens preflight into a cadence-aware request lifecycle."
key_decisions:
  - Treat the lightweight root, Python, and LENS_VERSION checks as every-request gates.
  - Refresh release-derived assets from lens.core on every request when the lens.core checkout is on develop.
  - When lens.core leaves develop, release-derived refresh returns to periodic cadence automatically.
  - Separate daily and weekly hygiene from request-time work instead of letting timestamp freshness imply a full skip.
  - Treat control repo sync and governance repo sync as mutable request-lifecycle operations with explicit pre-request and post-request policy, not as generic mirror refreshes.
  - Define read-only and write requests differently so request-time sync can no-op when no mutable repo work is needed.
  - Read-only requests warn on governance freshness instead of hard-blocking.
  - Post-request push is the default outcome for qualifying touched control and governance repos.
  - Existing preflight log output remains the user-visible signal; this feature does not introduce a separate request-status UX surface.
open_questions:
  - Which failures should degrade to warnings versus hard-stop the request lifecycle?
depends_on:
  - lens.core/_bmad/lens-work/skills/lens-preflight/scripts/light-preflight.py
  - lens.core/_bmad/lens-work/skills/lens-preflight/scripts/preflight.py
  - lens.core/_bmad/lens-work/scripts/validate-phase-artifacts.py
blocks: []
updated_at: 2026-05-04T00:00:00Z
---

# Business Plan — PreAndPostFlight

## Executive Summary

The `lens-dev-new-codebase-preandpostflight` feature exists to make Lens preflight predictable, cadence-aware, and safe for request-time use. The current preflight path mixes cheap prompt-start validation with heavyweight repo synchronization, mutable git operations, and hygiene work that runs on the same request path regardless of whether the request is read-only, write-heavy, or already within a fresh sync window.

This feature defines a simpler contract. Every request should pay only for the work that is justified by prompt freshness and request correctness. Release-derived assets from `lens.core` should refresh on every request while the workspace is following `develop`, because that branch is treated as volatile. Daily and weekly hygiene should remain available, but they should no longer be conflated with request-start gating. Control and governance repo sync must be treated as deliberate mutable operations with explicit pre-request and post-request policy.

## Problem Statement

The current preflight flow creates three practical problems:

1. It over-couples request-start gating with heavyweight sync behavior.
2. It treats timestamp freshness as an informational signal instead of a reliable cadence boundary.
3. It uses the same default execution path for read-only checks, release-mirror refresh, and mutable control or governance repo synchronization.

That coupling makes prompt-start behavior harder to reason about. A user cannot tell from the high-level command contract which operations are cheap validation, which are mirror refreshes, and which are stateful git mutations that may commit, rebase, or push repository state.

## Users And Stakeholders

- Lens users who expect fast, predictable prompt-start behavior on every request.
- Lens maintainers who need release-derived prompts and assets to stay current when `lens.core` tracks `develop`.
- Governance maintainers who need a clear policy for when request-time operations may touch the governance repo.
- Future implementation agents who need a clean separation between every-request checks, periodic hygiene, and mutable repo sync.

## Goals

1. Define an every-request preflight path that is cheap, deterministic, and appropriate for prompt-start use.
2. Refresh release-derived assets from `lens.core` on every request when the release checkout is on `develop`.
3. Separate request-time behavior from daily and weekly hygiene so cadence decisions are explicit.
4. Specify a safe request-lifecycle policy for control repo and governance repo sync before and after requests.
5. Preserve enough observability that sync decisions and failures are diagnosable rather than implicit.

## Applied Predecessor Review Decisions

The expressplan review responses that materially affect this packet are now treated as binding planning decisions:

- Read-only requests do not hard-block on governance freshness; they warn unless the request explicitly depends on mutable governance state.
- Request classification remains explicit at the planning level, with touched-repo detection allowed only as the operational fallback.
- Post-request push behavior is acceptable by default for qualifying touched control and governance repos.
- The current preflight log stream remains the user-visible signal for no-op, refresh-only, and mutable-sync request paths.
- When `lens.core` is no longer on `develop`, request-time release refresh automatically downgrades to the slower cadence path.

## Non-Goals

- Rewriting the full Lens lifecycle outside the preflight and request-lifecycle surface.
- Converting governance publication into an always-on background process.
- Allowing destructive reset behavior outside the `lens.core` mirror policy.
- Solving unrelated prompt or command-surface gaps not affected by preflight cadence and request-time sync.

## Required Outcomes

### User Outcomes

- Request-start behavior is explainable in terms of every-request, daily, weekly, and explicit request-lifecycle responsibilities.
- Users on `lens.core` `develop` always see current release-derived prompts and assets without waiting for daily sync.
- Read-only requests do not create unnecessary control or governance repo churn.

### Governance Outcomes

- Mutable repo sync is governed by explicit policy instead of being treated like a generic freshness step.
- Control and governance operations surface whether they no-op, pull, commit, or push.
- Failure categories are actionable and distinguish branch-state, auth, conflict, and policy violations.

### Delivery Outcomes

- The implementation path can split prompt-start validation from heavier repo sync orchestration.
- The feature creates a clear policy handoff for future request-lifecycle automation.
- Automated tests can verify branch-sensitive release refresh and touched-versus-untouched repo handling.

## Scope

### In Scope

- Defining every-request preflight responsibilities.
- Defining the `lens.core` `develop` branch rule for request-time release refresh.
- Defining daily and weekly hygiene categories for preflight-owned work.
- Defining pre-request and post-request control and governance repo sync policy.
- Defining success, failure, and no-op behavior for request-time sync decisions.

### Out Of Scope

- Implementing unrelated lifecycle phase changes.
- Changing the ownership model for governance publication artifacts.
- Moving planning or implementation work out of their existing repos.
- Expanding this feature into a full workflow redesign for every Lens command.

## Clean-Room Source Packet

This plan is derived from the current `light-preflight.py` and `preflight.py` behavior, the express-route discussion in this chat, and existing planning packet conventions in the control repo. The planning posture is to preserve the current repo topology while redefining cadence, sync boundaries, and safety policy.

## Success Criteria

This feature is successful when:

1. The planning packet clearly separates every-request checks, daily hygiene, weekly hygiene, and explicit pre/post request sync behavior.
2. The technical plan defines branch-sensitive `lens.core` refresh behavior and a deterministic request-lifecycle policy for control and governance sync.
3. The sprint plan sequences the work into implementation-ready slices without collapsing mirror refresh and mutable repo sync into the same default path.
4. The eventual implementation can prove that request-time behavior no-ops when appropriate, refreshes release-derived assets when `lens.core` is on `develop`, and surfaces sync failures with actionable categories.