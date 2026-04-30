# Story E2-S4: Write paused-state and edge-case fixtures

**Feature:** lens-dev-new-codebase-next
**Epic:** Epic 2 — Routing Engine Parity
**Status:** ready-for-dev

---

## Story

As a Lens module maintainer,
I want parity fixtures for paused-state and edge cases (warnings-only, unknown phase,
missing `feature.yaml`)
so that `next-ops.py` handles every possible routing input gracefully.

## Acceptance Criteria

1. Paused-state fixture exists; behavior matches the decision recorded in E2-S1 Dev Notes
2. Warnings-only fixture: feature has warnings but no blockers; `status=unblocked` with
   `warnings` populated
3. Unknown-phase fixture: `status=fail`, descriptive message
4. Missing-`feature.yaml` fixture: `status=fail`, descriptive message
5. All fixtures pass against `next-ops.py suggest`

## Precondition (M1)

**Do not start this story** until E2-S1 Dev Notes has the paused-state behavior decision
filled in and committed. The paused-state fixture must match the recorded decision exactly.

## Tasks / Subtasks

- [ ] **GATE:** Confirm E2-S1 Dev Notes paused-state decision is recorded (read the file)
- [ ] Write paused-state fixture matching the E2-S1 decision (AC #1)
- [ ] Write warnings-only fixture (AC #2)
- [ ] Write unknown-phase fixture (AC #3)
- [ ] Write missing-feature.yaml fixture (AC #4)
- [ ] Run all fixtures against `next-ops.py suggest` and confirm pass (AC #5)
- [ ] Commit fixture file to `lens.core.src` develop branch

## Dev Notes

- **Gate check:** Before writing paused-state fixture, run:
  `grep -A5 "Paused-State Decision" story-E2-S1.md`
  If the decision is not filled in, stop and alert.
- **Fixture path:** `_bmad/lens-work/skills/bmad-lens-next/tests/fixtures/next-routing-edge-cases.yaml`

### References
- [story-E2-S1.md — Paused-State Decision (required fill-in)](./story-E2-S1.md)
- [finalizeplan-review.md — M1 paused-state decision gate](../finalizeplan-review.md)
- [epics.md — Epic 2 paused-state gate](../epics.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List
