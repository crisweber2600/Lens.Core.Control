# Story E3-S1: Implement check-compliance gate behavior

**Feature:** lens-dev-new-codebase-constitution
**Epic:** Epic 3 - Compliance and Progressive Display Parity
**Status:** ready-for-dev

---

## Story

As a planning conductor,
I want `check-compliance` to distinguish hard-gate failures from informational failures
so that lifecycle callers can trust both its payload and its exit code.

## Acceptance Criteria

1. Track, artifact, review, and stories checks are evaluated against the resolved constitution
2. Exit code `2` is reserved for hard-gate failures
3. Informational-only failures return exit code `0` with failure detail in payload
4. Output includes `checks`, `hard_failures`, and `informational_failures`
5. Express-track compliance succeeds when the hierarchy permits it

## Tasks / Subtasks

- [ ] Implement or verify track-permitted validation
- [ ] Implement required-artifact, review, and stories checks against the merged contract
- [ ] Return structured check entries for every evaluated requirement
- [ ] Add tests for hard-gate and informational-only outcomes
- [ ] Add an express-permitted compliance case

## Dev Notes

### Implementation Notes

- `check-compliance` should reuse resolver output rather than duplicating merge logic.
- Exit-code behavior is part of the public contract and must be regression-covered.
- Artifact checks should rely on explicit file presence, not assumptions about planning phase.

### References

- [tech-plan.md](../tech-plan.md)
- [finalizeplan-review.md](../finalizeplan-review.md)
- [epics.md](../epics.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List