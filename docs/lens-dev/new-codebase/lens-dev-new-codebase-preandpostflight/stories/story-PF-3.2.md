# Story PF-3.2: Align the docs contract and carry the failure taxonomy into readiness handoff

**Feature:** lens-dev-new-codebase-preandpostflight
**Epic:** Epic 3 - Validation Hardening and Readiness Handoff
**Status:** done
**Story Points:** 2

---

## Story

As a Lens implementer,
I want the live preflight docs contract and downstream readiness inputs aligned to the same
policy,
so that `implementation-readiness.md` and future story files inherit the finalized
taxonomy instead of rediscovering it.

## Context

By the time PF-3.2 starts, the lifecycle code and focused tests should already express the
fixed failure taxonomy. This story keeps the target-repo preflight docs, readiness
handoff, and remaining implementation notes aligned to that final behavior so later dev or
review work does not reopen settled policy.

## Acceptance Criteria

1. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/SKILL.md` and any related preflight references describe the live target repo paths and supported invocation contract accurately.
2. `implementation-readiness.md` copies the hard-stop versus warning taxonomy from PF-2.2 and PF-2.3 without reopening the policy question.
3. Story files generated after this bundle inherit the same failure taxonomy in their acceptance criteria.
4. Any remaining mismatch between predecessor plan wording and the live target repo layout is recorded explicitly as a handoff note.
5. FinalizePlan can proceed to readiness and story-file generation without revisiting the accepted express-plan decisions.

## Implementation Steps

1. Align the target-repo preflight docs and references to the live `skills/lens-*` paths and
	supported invocation contract.
2. Keep `implementation-readiness.md` synchronized to the fixed hard-stop, warning, and
	post-request reconciliation taxonomy.
3. Verify downstream story and handoff materials inherit the same taxonomy and target
	surface notes.
4. Record any remaining predecessor wording drift explicitly as a handoff note.
5. Confirm no accepted express-plan decision is reopened during documentation alignment.

## Tasks / Subtasks

- [x] Align preflight skill docs and related references to the live target repo layout.
- [x] Keep `implementation-readiness.md` synchronized to the fixed warning-versus-hard-stop taxonomy.
- [x] Verify story files inherit the same taxonomy and target-surface notes.
- [x] Record any remaining predecessor wording drift as an explicit handoff note.
- [x] Confirm no accepted express-plan decision is reopened during doc alignment.

## Dev Notes

### Implementation Notes

- This story is the documentation and handoff companion to PF-3.1.
- The fixed taxonomy is already captured in `implementation-readiness.md`; this story ensures that target-repo docs and future handoffs stay aligned.
- If any older planning prose still references `bmad-lens-*`, record the drift explicitly rather than hiding it.

### References

- [epics.md](../epics.md)
- [stories.md](../stories.md)
- [implementation-readiness.md](../implementation-readiness.md)
- [finalizeplan-review.md](../finalizeplan-review.md)
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/SKILL.md`

## Dev Agent Record

### Agent Model Used

GPT-5.4

### Debug Log References

### Completion Notes List

- Updated the planning packet to point at the implemented `skills/lens-preflight` and `skills/lens-git-orchestration` surfaces.
- Carried the fixed hard-stop, warning, and post-request reconciliation taxonomy forward without reopening policy.

### File List

- docs/lens-dev/new-codebase/lens-dev-new-codebase-preandpostflight/epics.md
- docs/lens-dev/new-codebase/lens-dev-new-codebase-preandpostflight/stories.md
- docs/lens-dev/new-codebase/lens-dev-new-codebase-preandpostflight/implementation-readiness.md
- TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/SKILL.md