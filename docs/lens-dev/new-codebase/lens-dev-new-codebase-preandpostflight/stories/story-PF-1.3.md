# Story PF-1.3: Implement develop-sensitive release refresh and mirror boundary

**Feature:** lens-dev-new-codebase-preandpostflight
**Epic:** Epic 1 - Cadence Split and Branch-Sensitive Refresh
**Status:** ready-for-dev
**Story Points:** 3

---

## Story

As a Lens maintainer,
I want release-derived refresh to be branch-sensitive to `lens.core`,
so that `develop` stays fresh on every request while stable branches avoid unnecessary
refresh churn.

## Acceptance Criteria

1. The current `lens.core` branch is resolved before cadence decisions are finalized.
2. If the branch is `develop`, release-derived assets refresh on every request even when timestamps are otherwise fresh.
3. If the branch is not `develop`, the refresh automatically downgrades to cadence-managed execution with no extra user setting.
4. Any retained hard-reset behavior is explicitly documented and confined to the `lens.core` mirror path only; it is never reused for control or governance repos.
5. The preflight log stream distinguishes no-op, refresh-only, and mutable-sync paths without changing the user-visible surface.

## Tasks / Subtasks

- [ ] Resolve the active `lens.core` branch before cadence decisions run.
- [ ] Implement the forced every-request release refresh path for `develop`.
- [ ] Implement the automatic downgrade path for non-`develop` branches.
- [ ] Confine any hard-reset behavior to the release mirror path and document that boundary.
- [ ] Emit log output that distinguishes no-op, refresh-only, and mutable-sync outcomes.

## Dev Notes

### Implementation Notes

- Release-derived assets include prompts and managed mirror outputs already sourced from `lens.core`; do not mix them with control or governance mutation rules.
- The mirror boundary must be explicit so PF-2.2 and PF-2.3 can treat mutable repo sync as a separate policy surface.
- This story must preserve the current user-visible log surface even if the internal cadence logic changes significantly.

### References

- [epics.md](../epics.md)
- [stories.md](../stories.md)
- [implementation-readiness.md](../implementation-readiness.md)
- [tech-plan.md](../tech-plan.md)
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/scripts/light-preflight.py`

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List