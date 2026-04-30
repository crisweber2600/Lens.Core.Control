# Story E2-S3: Write express-track routing parity fixtures

**Feature:** lens-dev-new-codebase-next
**Epic:** Epic 2 — Routing Engine Parity
**Status:** ready-for-dev

---

## Story

As a Lens module maintainer,
I want parity fixtures for all express-track routing paths including
`expressplan-complete → /finalizeplan`
so that express routing is regression-covered against the actual lifecycle contract.

## Acceptance Criteria

1. Fixture file at `_bmad/lens-work/skills/bmad-lens-next/tests/fixtures/next-routing-express-track.yaml`
2. Covers: `expressplan` → `/expressplan`, `expressplan-complete` → `/finalizeplan`,
   missing express phase → track start phase, express track with blockers → `status=blocked`
3. Each fixture entry specifies `phase`, `track`, `expected_recommendation`, `expected_status`
4. Fixtures load `lifecycle.yaml` from the live installed path (no stubs)
5. All fixtures pass against `next-ops.py suggest`
6. The `expressplan-complete → /finalizeplan` fixture explicitly exercises the `auto_advance_to`
   field in lifecycle.yaml express track definition

## Tasks / Subtasks

- [ ] Confirm E2-S1 is complete
- [ ] Enumerate all express-track phases from live `lifecycle.yaml`
- [ ] Write fixture for `expressplan → /expressplan` (AC #2)
- [ ] Write fixture for `expressplan-complete → /finalizeplan` exercising `auto_advance_to` (AC #2, #6)
- [ ] Write fixture for missing express phase → track start (AC #2)
- [ ] Write fixture for express track with blockers (AC #2)
- [ ] Ensure fixture references live lifecycle.yaml path (AC #4)
- [ ] Run all fixtures against `next-ops.py suggest` and confirm pass (AC #5)
- [ ] Commit fixture file to `lens.core.src` develop branch

## Dev Notes

- **`auto_advance_to` field:** In `lifecycle.yaml`, `expressplan-complete` phase in the express
  track must have an `auto_advance_to: finalizeplan` (or equivalent). Verify this field exists
  and the fixture exercises it explicitly — do not just check the string literal `/finalizeplan`.
- **Express-track phases:** lifecycle.yaml `tracks.express.phases = [expressplan, finalizeplan]`
- **Blind Spot 2 from finalizeplan-review:** Do NOT stub lifecycle.yaml content in these
  fixtures. Always load the live file.

### References
- [tech-plan.md — §3 Express-track routing](../tech-plan.md)
- [finalizeplan-review.md — Blind Spot 2 (live lifecycle.yaml warning)](../finalizeplan-review.md)
- [epics.md — Epic 2 express-track parity note](../epics.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List
