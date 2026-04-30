# Story E3-S3: Document and resolve constitution resolver dependency

**Feature:** lens-dev-new-codebase-next
**Epic:** Epic 3 — Delegation and Release Hardening
**Status:** ready-for-dev

---

## Story

As a Lens module maintainer,
I want the constitution resolver dependency on `lens-dev-new-codebase-constitution` formally
confirmed or scoped before Slice 4 closes
so that the express-track allow-list gap does not silently break constitution permission
checks for future express-track features.

## Acceptance Criteria (H1 gate)

1. Check governance state of `lens-dev-new-codebase-constitution`: has it reached at least
   `expressplan-complete`?
2. If yes: document the confirmed state in Dev Notes (Gate Outcome) and mark gate closed
3. If no: confirm whether the express-track allow-list fix should be scoped into this
   feature's Slice 4 or remain as a dependency on the constitution feature; document the
   owner decision
4. Document the owner decision in Dev Notes
5. If scoped in: write and test the allow-list fix in `next-ops.py` or the relevant
   resolver component
6. `feature.yaml` `depends_on` is updated to reflect the outcome (add or remove
   `lens-dev-new-codebase-constitution` as appropriate)

## Tasks / Subtasks

- [ ] Run: `cat TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-constitution/feature.yaml | grep phase`
- [ ] Fill in Gate Outcome in Dev Notes below
- [ ] If constitution is at `expressplan-complete` or beyond: mark gate closed (AC #2)
- [ ] If constitution is NOT ready: determine scope (this feature vs dependency); document (AC #3, #4)
  - [ ] If scoped in: implement allow-list fix (AC #5); write test
  - [ ] If kept as dependency: document expected delivery timeline and risk
- [ ] Update `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-next/feature.yaml`
  `depends_on` field accordingly (AC #6)
- [ ] Commit governance `feature.yaml` change (git -C TargetProjects/lens/lens-governance ...)
- [ ] Push governance repo

## Dev Notes

### Gate Outcome (fill in before marking done)

> **Constitution state check:**
> `cat TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-constitution/feature.yaml`
>
> Phase found: _(fill in)_
> Gate status: OPEN / CLOSED _(fill in)_
> Owner decision: _(this feature / constitution feature / N/A)_
> Rationale: _(fill in)_

### feature.yaml update instructions

If gate is closed (constitution reached target phase):
- Remove `lens-dev-new-codebase-constitution` from `depends_on` in this feature's `feature.yaml`
- Or if it was never added: no change needed

If gate remains open AND fix scoped here:
- Ensure `lens-dev-new-codebase-constitution` is in `depends_on` until the fix ships
- Fix implementation lives in `_bmad/lens-work/skills/bmad-lens-next/scripts/next-ops.py`
  or the relevant resolver component

### References
- [finalizeplan-review.md — H1 response (A): formal dependency recorded](../finalizeplan-review.md)
- [expressplan-adversarial-review.md — H1 original finding: constitution resolver gap](../expressplan-adversarial-review.md)
- [epics.md — Epic 3 constitution resolver scope](../epics.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List
