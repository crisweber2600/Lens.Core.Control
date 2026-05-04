---
feature: lens-dev-new-codebase-preandpostflight
doc_type: tech-plan
status: draft
goal: "Define the implementation and validation plan for branch-sensitive preflight cadence and explicit pre/post request repo sync."
key_decisions:
  - Keep `light-preflight.py` as the minimal prompt-start wrapper for root and Python gating.
  - Split the full preflight path into request-time validation, release-derived refresh, and periodic hygiene rather than relying on the current timestamp semantics.
  - Force `lens.core` refresh and release-derived asset sync on every request when the release checkout branch is `develop`.
  - Treat control and governance sync as explicit request-lifecycle stages with touched-repo and policy checks before any commit, pull, or push behavior.
  - Preserve staged docs and governance reads as first-class request dependencies without assuming every request needs mutable repo sync.
open_questions:
  - Should request classification be explicit (`read-only`, `control-write`, `governance-write`, `mixed`) or inferred from touched paths?
  - Should post-request sync push automatically when commits are created, or should publish remain phase-gated even after a successful write request?
  - Is the current release hard-reset fallback still acceptable for `lens.core` while on `develop`, or should the mirror sync path be made more explicit?
depends_on:
  - business-plan.md
  - lens.core/_bmad/lens-work/skills/lens-preflight/scripts/light-preflight.py
  - lens.core/_bmad/lens-work/skills/lens-preflight/scripts/preflight.py
  - lens.core/_bmad/lens-work/skills/lens-preflight/scripts/tests/test-preflight.py
blocks: []
updated_at: 2026-05-04T00:00:00Z
---

# Tech Plan — PreAndPostFlight

## Overview

The implementation target is the Lens preflight stack in the target project and release mirror workflow, centered on `light-preflight.py`, `preflight.py`, and their tests. The feature does not change the public express-route contract. It changes how preflight decides what work belongs to every request, what work belongs to periodic hygiene, and what work belongs to explicit mutable sync stages before and after a request.

The key design change is to stop treating preflight as a single monolithic pipeline. Instead, the feature should model preflight as a layered request lifecycle:

1. Cheap prompt-start gates.
2. Branch-sensitive release-derived refresh.
3. Periodic hygiene.
4. Explicit pre-request mutable sync.
5. Explicit post-request mutable sync.

## Current Behavior Summary

The current stack has a two-layer entry path:

- `light-preflight.py` resolves the workspace root, checks Python, and delegates to `preflight.py`.
- `preflight.py` performs release sync, control sync, version checks, governance sync, `.github` sync, prompt hygiene, timestamp management, and a final control sync.

That current behavior has three implementation consequences that this feature must address:

1. Timestamp freshness does not actually gate the heaviest sync work.
2. `lens.core` refresh and `.github` mirror refresh are coupled to mutable control and governance sync.
3. Request-time sync policy is implicit inside git helper behavior instead of being explicit in the request lifecycle.

## Desired Runtime Model

### Layer 1 — Every-Request Prompt Gate

Keep the current lightweight responsibilities:

- workspace root detection
- Python version validation
- `LENS_VERSION` compatibility check
- required repo and path existence checks

These checks remain the minimal contract for every prompt-start and should stay cheap and deterministic.

### Layer 2 — Branch-Sensitive Release Refresh

Add an explicit branch rule for `lens.core`:

- If `lens.core` is on `develop`, run release sync and release-derived asset refresh on every request.
- If `lens.core` is not on `develop`, allow cadence gating to move those heavier refresh steps to daily execution.

Release-derived refresh includes:

- syncing the `lens.core` mirror
- syncing `.github`
- syncing retained prompt assets
- syncing managed entrypoints like `CLAUDE.md`
- updating the hash manifest

This layer is conceptually separate from control and governance sync even if some shared helper code remains.

### Layer 3 — Daily And Weekly Hygiene

Move cleanup-oriented work out of the default request path when possible.

Daily hygiene should own:

- release-derived refresh when `lens.core` is not on `develop`
- timestamp updates for periodic cadence
- routine prompt filtering and hash refresh outside request-time urgency

Weekly hygiene should own:

- stale managed file pruning
- legacy state migration cleanup
- deeper workspace consistency checks

### Layer 4 — Pre-Request Mutable Sync

Introduce an explicit policy stage for control and governance before request execution begins.

This stage must decide:

- whether the request needs current mutable control state
- whether the request needs current mutable governance state
- whether local repo state is safe to sync
- whether the sync is a no-op, pull-only, or a blocking condition

The core design requirement is that mutable sync must be policy-driven rather than assumed.

### Layer 5 — Post-Request Mutable Sync

Add a symmetric post-request stage that runs only after request execution knows what changed.

This stage must decide:

- whether the request touched the control repo
- whether the request touched the governance repo
- whether post-request sync should stop at local commit, pull-and-reconcile, or publish
- how failures are reported after user-visible work is already complete

## Target Implementation Surface

The eventual implementation should create or update these surfaces in the target project:

1. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/scripts/light-preflight.py`
2. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/scripts/preflight.py`
3. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/scripts/tests/test-preflight.py`
4. Any supporting helpers needed to classify request intent, touched repos, or cadence state without duplicating git logic across scripts

Planning artifacts remain staged in `docs/lens-dev/new-codebase/lens-dev-new-codebase-preandpostflight`.

## Required Runtime Behavior

The rewritten preflight flow must preserve these technical constraints:

1. Prompt-start gating remains available even when heavier sync stages are skipped.
2. `lens.core` `develop` is treated as volatile and therefore forces request-time release refresh.
3. Control and governance sync are never treated as mere mirror refreshes.
4. Read-only requests can no-op on mutable sync when policy permits.
5. Interrupted git state, detached HEAD, auth failures, and branch-policy failures are surfaced explicitly.
6. Non-`lens.core` destructive reset behavior is forbidden by default.

## Implementation Sequence

### Workstream 1 — Cadence Split

1. Separate request-time gates from periodic hygiene inside the full preflight path.
2. Make timestamp behavior explicit instead of letting it act as a partial hint.
3. Preserve backward compatibility for prompt-start callers while shifting the internal execution model.

### Workstream 2 — Branch-Sensitive Release Refresh

1. Detect the active `lens.core` branch before cadence decisions are finalized.
2. Force release-derived refresh on every request when that branch is `develop`.
3. Keep non-`develop` branches on daily cadence unless the request explicitly forces a refresh.

### Workstream 3 — Explicit Request-Lifecycle Sync Policy

1. Add request classification or touched-repo detection.
2. Define pre-request sync policy for control and governance.
3. Define post-request sync policy for touched repos.
4. Preserve no-op behavior when a request does not justify mutable sync.

### Workstream 4 — Validation And Observability

1. Add focused tests for `lens.core` branch-sensitive refresh.
2. Add tests that prove fresh timestamps do not mask required request-time release refresh on `develop`.
3. Add tests for touched-versus-untouched control and governance repo behavior.
4. Add failure-path coverage for interrupted git state, auth failures, and policy-blocked sync.

## Validation Plan

### Contract Checks

- Every-request gates work without requiring full mutable repo sync.
- `lens.core` on `develop` forces release-derived refresh on every request.
- Non-`develop` release branches follow the slower cadence policy.
- Control and governance sync can distinguish no-op, pull-only, and publish paths.

### Focused Test Coverage

- request-start gate still fails cleanly when root or Python requirements are unmet
- `LENS_VERSION` mismatch still blocks execution
- fresh timestamps do not suppress the `develop` release refresh path
- untouched repos do not auto-commit or auto-push post-request
- touched repos follow explicit sync policy rather than implicit default behavior

### Rollout Checks

- Logging identifies which cadence layer ran on each request.
- Sync results are attributable to pre-request or post-request stages.
- Existing prompt stubs continue to invoke the shared prompt-start gate successfully.

## Risks

- Request classification may become too implicit and hide policy decisions.
- Every-request release refresh on `develop` may increase prompt-start latency if the mirror path is not kept narrow.
- Post-request mutable sync can create churn if touched-repo detection is inaccurate.
- Carrying forward the current release hard-reset policy without a clearer mirror boundary may surprise maintainers.

## Success Definition

The technical plan succeeds when the implementation can prove a layered request lifecycle: cheap request gates always run, release-derived assets refresh on every request for `lens.core` `develop`, periodic hygiene is separated from prompt-start obligations, and control or governance sync only mutates repos when explicit request-lifecycle policy allows it.