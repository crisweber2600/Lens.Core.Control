# Story E2-S2: Write full-track routing parity fixtures

**Feature:** lens-dev-new-codebase-next
**Epic:** Epic 2 — Routing Engine Parity
**Status:** done

---

## Story

As a Lens module maintainer,
I want a parity test fixture covering all full-track routing paths
so that `next-ops.py` routing correctness is verifiable against the lifecycle contract
without manual inspection.

## Acceptance Criteria

1. Fixture file at `_bmad/lens-work/skills/bmad-lens-next/scripts/tests/next-routing-full-track.yaml`
   (canonical regression-fixture path — consistent with tech-plan target surface table)
2. Covers: `preplan` → `/preplan`, `preplan-complete` → next phase, all full-track phase
   transitions, final phase → complete message or blocked
3. Each fixture entry specifies: `phase`, `track`, `expected_recommendation`,
   `expected_status`
4. Fixtures load `lifecycle.yaml` from the live installed path; no hard-coded lifecycle
   content inside the fixture
5. All fixtures pass against `next-ops.py suggest`

## Tasks / Subtasks

- [ ] Confirm E2-S1 is complete and `next-ops.py suggest` is callable
- [ ] Enumerate all full-track phases from live `lifecycle.yaml`
- [ ] Write fixture entries for each phase transition (AC #2, #3)
- [ ] Ensure fixture references live lifecycle.yaml path (AC #4)
- [ ] Run fixtures against `next-ops.py suggest` and confirm all pass (AC #5)
- [ ] Commit fixture file to `lens.core.src` develop branch

## Dev Notes

- **Fixture format:** Follow the established fixture pattern in the test suite (check
  `lens.core/_bmad/lens-work/tests/` for format reference)
- **Live lifecycle.yaml:** Do not copy lifecycle phase lists into the fixture. The fixture
  test runner must resolve phases from the live file at test time.
- **Full track phases:** Inspect `lifecycle.yaml` tracks.full.phases for the complete list

### References
- [tech-plan.md — §3 Routing engine, fixture strategy](../tech-plan.md)
- [stories.md — E2-S2 acceptance criteria](../stories.md)
- [epics.md — Epic 2 scope](../epics.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List
