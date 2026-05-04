# Story PF-2.2: Implement pre-request control and governance sync policy

**Feature:** lens-dev-new-codebase-preandpostflight
**Epic:** Epic 2 - Explicit Request Lifecycle Sync Policy
**Status:** done
**Story Points:** 5

---

## Story

As a Lens maintainer,
I want deterministic pre-request sync rules for the control and governance repos,
so that the system can distinguish safe no-op and warning paths from true blockers before
work begins.

## Context

The FinalizePlan bundle fixed the warning-versus-hard-stop policy. This story turns that
policy into a pre-request decision surface with explicit per-repo outcomes while routing
any real write work through the shared git-orchestration implementation instead of ad hoc
git commands.

## Acceptance Criteria

1. Pre-request policy returns explicit per-repo outcomes: `no-op`, `warn`, `pull-only`, or `block`.
2. Read-only requests treat governance freshness as warning-only unless the command explicitly requires mutable governance state before execution.
3. The hard-stop taxonomy is explicit and testable: interrupted git state, detached or wrong branch when mutation is required, auth or permission failure for required mutable sync, unresolved merge or rebase conflict, policy-blocked sync, and missing required repo context.
4. Warning-only taxonomy is explicit and testable: governance freshness on read-only requests, optional publish lag, and other non-required freshness gaps that do not block safe execution.
5. Any actual pull or reconciliation work reuses `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-git-orchestration/scripts/git-orchestration-ops.py` or a shared helper it owns; preflight does not embed ad hoc git write sequences.
6. The preflight log stream states separately whether control and governance pre-request sync were skipped, warned, pulled, or blocked.

## Implementation Steps

1. Define a pre-request outcome contract with explicit `no-op`, `warn`, `pull-only`, and
	`block` results per repo.
2. Encode the fixed hard-stop and warning-only taxonomy in the lifecycle implementation.
3. Treat governance freshness as warning-only for read-only requests unless mutable
	governance state is required.
4. Route any actual pull or reconciliation through
	`TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-git-orchestration/scripts/git-orchestration-ops.py`
	or helpers it owns.
5. Emit separate control-repo and governance-repo outcomes in the existing preflight log
	stream.

## Tasks / Subtasks

- [ ] Define the per-repo pre-request outcome contract (`no-op`, `warn`, `pull-only`, `block`).
- [ ] Encode the explicit hard-stop and warning-only taxonomies exactly as approved in FinalizePlan.
- [ ] Wire read-only governance freshness to warning-only unless mutable governance state is required.
- [ ] Route any real pull or reconciliation through shared git-orchestration surfaces, not ad hoc git write logic.
- [ ] Emit separate control-repo and governance-repo pre-request outcomes in the existing preflight log stream.

## Dev Notes

### Implementation Notes

- The failure taxonomy is fixed by the FinalizePlan bundle; do not reopen policy.
- Preserve the separation between `lens.core` mirror refresh and mutable control or governance sync.
- Shared write logic belongs under `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-git-orchestration/`.

### References

- [epics.md](../epics.md)
- [stories.md](../stories.md)
- [implementation-readiness.md](../implementation-readiness.md)
- [finalizeplan-review.md](../finalizeplan-review.md)
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-git-orchestration/SKILL.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-git-orchestration/scripts/git-orchestration-ops.py`

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List