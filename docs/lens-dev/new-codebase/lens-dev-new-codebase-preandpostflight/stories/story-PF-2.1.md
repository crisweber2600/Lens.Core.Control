# Story PF-2.1: Add explicit request classification and sync intent resolution

**Feature:** lens-dev-new-codebase-preandpostflight
**Epic:** Epic 2 - Explicit Request Lifecycle Sync Policy
**Status:** ready-for-dev
**Story Points:** 3

---

## Story

As a Lens maintainer,
I want request classification to be explicit and available throughout the lifecycle,
so that mutable sync policy is auditable instead of inferred from side effects.

## Acceptance Criteria

1. The implementation exposes explicit request classes: `read-only`, `control-write`, `governance-write`, and `mixed`.
2. Classification is resolved before pre-request sync begins and remains available to post-request handling and tests.
3. Touched-repo detection is used only as the fallback execution check when explicit classification is absent or insufficient.
4. Requests that only read staged docs, governance data, or release-derived assets do not trigger mutable sync by default.
5. Classification decisions are emitted in logs or structured results in a way tests can assert.

## Tasks / Subtasks

- [ ] Add an explicit request-classification model under `bmad-lens-preflight/scripts/`.
- [ ] Resolve classification before pre-request sync and persist it through post-request handling.
- [ ] Keep touched-repo detection as a fallback-only execution check.
- [ ] Ensure read-only access to staged docs, governance data, and release-derived assets stays non-mutating by default.
- [ ] Emit classification decisions in log or structured outputs so PF-3.1 can assert them.

## Dev Notes

### Implementation Notes

- Classification decisions are authoritative for policy and must not be recreated ad hoc in later sync stages.
- This story is the dependency anchor for both PF-2.2 and PF-2.3.
- Keep the logic in the target repo under `bmad-lens-preflight/scripts/`; do not spread it into prompt files or governance docs.

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