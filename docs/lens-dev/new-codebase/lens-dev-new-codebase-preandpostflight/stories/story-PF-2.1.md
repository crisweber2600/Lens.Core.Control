# Story PF-2.1: Add explicit request classification and sync intent resolution

**Feature:** lens-dev-new-codebase-preandpostflight
**Epic:** Epic 2 - Explicit Request Lifecycle Sync Policy
**Status:** done
**Story Points:** 3

---

## Story

As a Lens maintainer,
I want request classification to be explicit and available throughout the lifecycle,
so that mutable sync policy is auditable instead of inferred from side effects.

## Context

Once release refresh is separated from cadence behavior, mutable control and governance
policy needs an explicit request-intent model. This story makes request classification the
authoritative policy input so later sync stages do not infer behavior from incidental file
touches alone.

## Acceptance Criteria

1. The implementation exposes explicit request classes: `read-only`, `control-write`, `governance-write`, and `mixed`.
2. Classification is resolved before pre-request sync begins and remains available to post-request handling and tests.
3. Touched-repo detection is used only as the fallback execution check when explicit classification is absent or insufficient.
4. Requests that only read staged docs, governance data, or release-derived assets do not trigger mutable sync by default.
5. Classification decisions are emitted in logs or structured results in a way tests can assert.

## Implementation Steps

1. Add an explicit request-classification model under
	`TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/scripts/`.
2. Resolve classification before pre-request sync begins and keep it available to
	post-request handling.
3. Keep touched-repo detection as a fallback-only execution check.
4. Ensure read-only requests that only consume staged docs, governance reads, or
	release-derived assets remain non-mutating by default.
5. Emit classification decisions in a form later tests can assert.

## Tasks / Subtasks

- [ ] Add an explicit request-classification model under `skills/lens-preflight/scripts/`.
- [ ] Resolve classification before pre-request sync and persist it through post-request handling.
- [ ] Keep touched-repo detection as a fallback-only execution check.
- [ ] Ensure read-only access to staged docs, governance data, and release-derived assets stays non-mutating by default.
- [ ] Emit classification decisions in log or structured outputs so PF-3.1 can assert them.

## Dev Notes

### Implementation Notes

- Classification decisions are authoritative for policy and must not be recreated ad hoc in later sync stages.
- This story is the dependency anchor for both PF-2.2 and PF-2.3.
- Keep the logic in the target repo under `skills/lens-preflight/scripts/`; do not spread it into prompt files or governance docs.

### References

- [epics.md](../epics.md)
- [stories.md](../stories.md)
- [implementation-readiness.md](../implementation-readiness.md)
- [tech-plan.md](../tech-plan.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List