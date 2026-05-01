# Story E3-S2: Implement progressive-display filters and warning propagation

**Feature:** lens-dev-new-codebase-constitution
**Epic:** Epic 3 - Compliance and Progressive Display Parity
**Status:** ready-for-dev

---

## Story

As a Lens caller,
I want `progressive-display` to return context-filtered governance rules with accurate
warning propagation
so that users can understand the effective constitution without reading every hierarchy
level manually.

## Acceptance Criteria

1. Phase-filtered output exposes `required_artifacts_for_phase`
2. Track-filtered output exposes `track_permitted` and `permitted_tracks`
3. Missing-level warnings from `resolve` are preserved in display output
4. `full_constitution_available` is false whenever org is absent
5. Express-track filtering reports the correct permission state

## Tasks / Subtasks

- [ ] Implement phase-filtered display behavior
- [ ] Implement track-filtered display behavior
- [ ] Propagate warnings from resolver output without mutation
- [ ] Preserve `full_constitution_available` semantics
- [ ] Add tests for express-track filtering and org-missing scenarios

## Dev Notes

### Implementation Notes

- `progressive-display` is a presentation layer over resolver output, not a second resolver.
- Warning propagation must be lossless so users see the same sparse-hierarchy context callers
  use programmatically.
- The org-missing case must still be useful to users even while reporting
  `full_constitution_available=false`.

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