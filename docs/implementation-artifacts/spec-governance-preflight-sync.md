---
title: 'governance preflight sync'
type: 'bugfix'
created: '2026-04-20'
status: 'done'
context: []
---

<frozen-after-approval reason="human-owned intent — do not modify unless human renegotiates">

## Intent

**Problem:** Governance repo sync is inconsistent because shared preflight only performs a timestamp-gated blind pull and never reconciles locally committed governance changes back to origin. With a fresh timestamp, governance can remain behind remote or ahead locally until a later manual sync, which breaks the module’s “automatic governance git” expectation.

**Approach:** Update preflight so governance sync is handled explicitly and deterministically on every run: switch to the governance authority branch, pull latest remote state with rebase semantics, and push already-committed local governance changes when the repo is clean. Keep the existing release-module pull window behavior intact.

## Boundaries & Constraints

**Always:** Treat governance `main` as the sync authority branch; preserve the existing preflight version checks and `.github/` sync; warn and skip automation when governance has uncommitted local changes instead of forcing recovery.

**Ask First:** Any change that would auto-commit new governance content, alter release-module branch policy, or reset/discard governance worktree state.

**Never:** Use destructive git recovery, silently drop user edits, or broaden preflight into a general-purpose target-repo sync routine.

## I/O & Edge-Case Matrix

| Scenario | Input / State | Expected Output / Behavior | Error Handling |
|----------|--------------|---------------------------|----------------|
| Remote ahead | Fresh preflight timestamp, governance remote `main` has newer commits, local governance clone is clean | Preflight still updates the local governance clone so governance reads reflect latest remote state | If pull fails, preflight warns and leaves timestamp unsafely unrefreshed so the next run retries |
| Local ahead | Fresh preflight timestamp, governance clone has committed local changes ahead of origin, worktree is clean | Preflight pushes the committed governance changes to origin automatically | If push fails, preflight warns and does not mark the sync as freshly completed |
| Dirty governance repo | Governance clone contains uncommitted changes | Preflight leaves files untouched and surfaces a clear warning that auto-sync was skipped | Continue the rest of preflight without destructive cleanup |

</frozen-after-approval>

## Code Map

- `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/scripts/preflight.py` -- shared workspace preflight entrypoint; currently uses timestamp-gated blind pulls and needs explicit governance reconciliation.
- `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/scripts/tests/test-preflight.py` -- subprocess-based regression suite for preflight behavior and the right place to lock in governance push/pull expectations.
- `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py` -- existing governance `main` sync precedent (`sync_governance_main`) worth mirroring for branch and pull semantics.

## Tasks & Acceptance

**Execution:**
- [x] `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/scripts/preflight.py` -- replace the blind governance pull with explicit clean-worktree governance branch reconciliation that can both pull and push -- fixes the inconsistent automation at the shared lifecycle gate.
- [x] `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/scripts/tests/test-preflight.py` -- add regression coverage for governance remote-ahead sync, governance local-ahead push, and dirty-worktree skip behavior -- proves preflight now handles the critical sync paths.

**Acceptance Criteria:**
- Given a clean governance clone with a fresh preflight timestamp and new commits on remote `main`, when preflight runs, then the local governance clone is updated anyway.
- Given a clean governance clone with committed local changes ahead of origin and a fresh preflight timestamp, when preflight runs, then those governance commits are pushed to origin automatically.
- Given a governance clone with uncommitted local changes, when preflight runs, then it warns and skips governance auto-sync instead of altering or discarding the worktree.

## Spec Change Log

## Design Notes

Governance sync should mirror the existing feature-init contract: governance artifacts live on `main`, and automation should reconcile that branch directly instead of depending on whatever branch or tracking state happens to be checked out locally. Preflight should not manufacture commits; it should only move already-committed governance state across the local/remote boundary.

## Verification

**Commands:**
- `cd TargetProjects/lens.core/src/Lens.Core.Src && uv run --with pytest pytest _bmad/lens-work/scripts/tests/test-preflight.py -q` -- expected: preflight regression suite passes, including new governance sync cases.
- `cd TargetProjects/lens.core/src/Lens.Core.Src && uv run --script _bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/test-init-feature-ops.py` -- expected: existing automatic-governance feature-init coverage still passes after aligning preflight semantics.