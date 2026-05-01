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
> Phase found: `preplan`
> Track: `full`
> Gate status: **OPEN**
> Owner decision: **constitution feature** — keep dependency, do not scope the allow-list fix into this feature
> Rationale: The constitution feature is at `preplan` (early analysis phase). Implementing the
> express-track allow-list fix here would be out of scope and premature. The correct resolution
> is for `lens-dev-new-codebase-constitution` to advance through its lifecycle. The existing
> `depends_on: [lens-dev-new-codebase-constitution]` in this feature's `feature.yaml` correctly
> blocks progression to `/dev` until the constitution feature is complete. No `next-ops.py`
> code changes are required for this gate.
>
> **Risk:** Until `lens-dev-new-codebase-constitution` reaches `dev-complete` or later, `/next`
> on this feature will return `status=blocked` with the constitution dependency as a blocker.
> This is correct behavior — the blocker will clear when the constitution feature ships.
>
> **feature.yaml `depends_on` status:** `lens-dev-new-codebase-constitution` is already in
> `depends_on`. No change needed.

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

Claude Sonnet 4.6

### Debug Log References

None — gate check performed via direct file read.

### Completion Notes List

- Checked `lens-dev-new-codebase-constitution/feature.yaml` → phase: preplan, track: full
- Gate is OPEN; constitution is far from `expressplan-complete`
- Decision: keep dependency on constitution feature; no code changes required
- `depends_on` in this feature's `feature.yaml` already includes `lens-dev-new-codebase-constitution`
- No governance file changes required

### File List

- `docs/lens-dev/new-codebase/lens-dev-new-codebase-next/stories/story-E3-S3.md` (updated Dev Notes)
