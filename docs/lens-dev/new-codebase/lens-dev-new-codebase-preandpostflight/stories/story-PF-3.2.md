# Story PF-3.2: Align the docs contract and carry the failure taxonomy into readiness handoff

**Feature:** lens-dev-new-codebase-preandpostflight
**Epic:** Epic 3 - Validation Hardening and Readiness Handoff
**Status:** ready-for-dev
**Story Points:** 2

---

## Story

As a Lens implementer,
I want the live preflight docs contract and downstream readiness inputs aligned to the same
policy,
so that `implementation-readiness.md` and future story files inherit the finalized
taxonomy instead of rediscovering it.

## Acceptance Criteria

1. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/SKILL.md` and any related preflight references describe the live target repo paths and supported invocation contract accurately.
2. `implementation-readiness.md` copies the hard-stop versus warning taxonomy from PF-2.2 and PF-2.3 without reopening the policy question.
3. Story files generated after this bundle inherit the same failure taxonomy in their acceptance criteria.
4. Any remaining mismatch between predecessor plan wording and the live target repo layout is recorded explicitly as a handoff note.
5. FinalizePlan can proceed to readiness and story-file generation without revisiting the accepted express-plan decisions.

## Tasks / Subtasks

- [ ] Align preflight skill docs and related references to the live target repo layout.
- [ ] Keep `implementation-readiness.md` synchronized to the fixed warning-versus-hard-stop taxonomy.
- [ ] Verify story files inherit the same taxonomy and target-surface notes.
- [ ] Record any remaining predecessor wording drift as an explicit handoff note.
- [ ] Confirm no accepted express-plan decision is reopened during doc alignment.

## Dev Notes

### Implementation Notes

- This story is the documentation and handoff companion to PF-3.1.
- The fixed taxonomy is already captured in `implementation-readiness.md`; this story ensures that target-repo docs and future handoffs stay aligned.
- If any older planning prose still references `skills/lens-preflight`, record the drift explicitly rather than hiding it.

### References

- [epics.md](../epics.md)
- [stories.md](../stories.md)
- [implementation-readiness.md](../implementation-readiness.md)
- [finalizeplan-review.md](../finalizeplan-review.md)
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/SKILL.md`

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List