# Story E2-S3: Harden constitution loading and scope validation

**Feature:** lens-dev-new-codebase-constitution
**Epic:** Epic 2 - Resolver Core and Merge Contract
**Status:** ready-for-dev

---

## Story

As a Lens maintainer,
I want constitution loading to surface malformed frontmatter, unknown keys, and invalid
scope inputs safely
so that the shared resolver remains predictable under broken or hostile input.

## Acceptance Criteria

1. Malformed frontmatter returns parse-error detail without unsafe fallback behavior
2. Unknown keys are surfaced in payload metadata or warnings
3. Invalid slugs and traversal attempts fail with exit code `1`
4. Path construction stays within the configured constitutions root
5. Error handling introduces no write path

## Tasks / Subtasks

- [ ] Harden `load_constitution` parsing behavior
- [ ] Add unknown-key reporting for frontmatter fields outside the supported set
- [ ] Validate scope inputs before path construction
- [ ] Add regression tests for invalid slugs and traversal attempts
- [ ] Confirm failures remain read-only

## Dev Notes

### Implementation Notes

- The parsing contract should distinguish missing file, malformed frontmatter, and unknown-key
  scenarios.
- Path validation belongs before any filesystem read outside the already-resolved root.
- Do not silently ignore hostile inputs; fail clearly and safely.

### References

- [tech-plan.md](../tech-plan.md)
- [architecture.md](../architecture.md)
- [implementation-readiness.md](../implementation-readiness.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List