# Story E2-S2: Preserve merge rules, defaults, and express-track parity

**Feature:** lens-dev-new-codebase-constitution
**Epic:** Epic 2 - Resolver Core and Merge Contract
**Status:** ready-for-dev

---

## Story

As a Lens maintainer,
I want the rewritten resolver to preserve the merge contract while supporting `express`
and `sensing_gate_mode`
so that downstream callers keep receiving the shared governance output they expect.

## Acceptance Criteria

1. `permitted_tracks` uses intersection across loaded levels
2. `required_artifacts` unions by phase bucket without duplicates
3. `gate_mode` and `sensing_gate_mode` preserve strongest-wins behavior
4. `enforce_stories` and `enforce_review` preserve true-wins behavior
5. Defaults and known-track handling include `express`

## Tasks / Subtasks

- [ ] Implement or verify each merge rule in the rewritten resolver
- [ ] Update defaults to include `express` and `sensing_gate_mode`
- [ ] Add targeted tests for merge-rule behavior and express-track parity
- [ ] Confirm no caller-specific fallback is needed when `express` is permitted

## Dev Notes

### Implementation Notes

- Treat `express` support as part of the shared contract, not a feature-local exception.
- `sensing_gate_mode` must survive the merge path unchanged when present.
- Deduplication matters for artifact and reviewer lists; do not let unions create repeated
  entries.

### References

- [business-plan.md](../business-plan.md)
- [tech-plan.md](../tech-plan.md)
- [expressplan-adversarial-review.md](../expressplan-adversarial-review.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List