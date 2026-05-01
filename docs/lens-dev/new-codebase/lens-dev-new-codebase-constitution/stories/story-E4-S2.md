# Story E4-S2: Add negative safety tests for malformed and hostile inputs

**Feature:** lens-dev-new-codebase-constitution
**Epic:** Epic 4 - Regression and Release Hardening
**Status:** ready-for-dev

---

## Story

As a governance maintainer,
I want negative tests for malformed frontmatter, invalid slugs, traversal attempts, and
no-write behavior
so that the shared resolver proves its highest-risk guarantees before merge.

## Acceptance Criteria

1. Malformed-frontmatter test asserts parse errors are surfaced cleanly
2. Invalid-slug and traversal tests assert exit code `1`
3. No-write test confirms no governance or feature-state mutation occurs during reads
4. Negative tests run under the same temp-directory isolation model as parity fixtures
5. Safety coverage is referenced in release-readiness notes

## Tasks / Subtasks

- [ ] Add malformed-frontmatter regression cases
- [ ] Add invalid-slug and traversal regressions
- [ ] Add a no-write assertion path for read operations and failure cases
- [ ] Keep all safety tests isolated to temp directories
- [ ] Cross-link release-readiness notes to this coverage

## Dev Notes

### Implementation Notes

- The no-write guarantee applies on both success and failure paths.
- Traversal tests should prove the resolver rejects unsafe scopes before path escape is
  possible.
- Keep error payload expectations explicit so safety failures do not become ambiguous.

### References

- [finalizeplan-review.md](../finalizeplan-review.md)
- [architecture.md](../architecture.md)
- [implementation-readiness.md](../implementation-readiness.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List