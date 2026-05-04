# Story PF-3.1: Add focused regression coverage for cadence and sync policy

**Feature:** lens-dev-new-codebase-preandpostflight
**Epic:** Epic 3 - Validation Hardening and Readiness Handoff
**Status:** done
**Story Points:** 5

---

## Story

As a Lens maintainer,
I want focused regression coverage for cadence, sync policy, and failure outcomes,
so that the new layered lifecycle is provable instead of being carried only by prose.

## Context

The target repo already has shared git-orchestration tests and a focused preflight test
surface. This story extends that coverage and asserts the
fixed cadence plus sync taxonomy directly, especially around `develop` refresh, request
classification, and touched-repo publication behavior.

## Acceptance Criteria

1. A focused preflight test module is created under `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/scripts/tests/`.
2. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-git-orchestration/scripts/tests/test-git-orchestration-ops.py` is extended where mutation-path assertions belong.
3. Tests cover: root or Python gate behavior, `lens.core` `develop` every-request refresh, non-`develop` cadence downgrade, fresh timestamps not suppressing required `develop` refresh, explicit classification precedence over touched fallback, read-only governance warning, untouched repo no-op, and touched repo default publish behavior.
4. Tests assert the hard-stop versus warning taxonomy from PF-2.2 and PF-2.3 rather than reopening the policy.
5. Windows-safe path handling and repo-root detection remain covered.
6. The existing preflight log stream remains the authoritative user-visible signal in tests.

## Implementation Steps

1. Create the focused preflight test module under
	`TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/scripts/tests/`.
2. Extend
	`TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-git-orchestration/scripts/tests/test-git-orchestration-ops.py`
	where shared write-path assertions belong.
3. Add coverage for gate behavior, `develop` refresh, cadence downgrade, classification
	precedence, governance warnings, untouched no-op, and touched publish defaults.
4. Assert the approved hard-stop versus warning taxonomy directly in tests.
5. Preserve Windows-safe path handling, repo-root detection, and log-surface expectations.

## Tasks / Subtasks

- [ ] Create the focused preflight test surface under `skills/lens-preflight/scripts/tests/`.
- [ ] Extend `test-git-orchestration-ops.py` where shared mutation-path assertions belong.
- [ ] Add coverage for cadence, `develop` refresh, classification precedence, governance warnings, untouched no-op, and touched publish defaults.
- [ ] Assert the approved warning-versus-hard-stop taxonomy directly in tests.
- [ ] Preserve Windows-safe path handling, repo-root detection, and log-surface expectations.

## Dev Notes

### Implementation Notes

- The live target repo already contains a focused preflight test module; this story extends that surface for the new cadence and sync policy assertions.
- Use the existing git-orchestration test file for shared write-path assertions instead of cloning logic into preflight-only tests.
- The test suite should prove the policy that PF-2.2 and PF-2.3 define rather than renegotiating it.

### References

- [epics.md](../epics.md)
- [stories.md](../stories.md)
- [implementation-readiness.md](../implementation-readiness.md)
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-git-orchestration/scripts/tests/test-git-orchestration-ops.py`

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List