# Story E4-S1: Build fixture-backed parity coverage

**Feature:** lens-dev-new-codebase-constitution
**Epic:** Epic 4 - Regression and Release Hardening
**Status:** ready-for-dev

---

## Story

As a maintainer,
I want temp-directory governance fixtures for full and partial hierarchies
so that the rewritten resolver has observable parity coverage instead of prose-only claims.

## Acceptance Criteria

1. Fixtures cover full hierarchy and representative sparse hierarchies
2. Resolve, compliance, and progressive-display are exercised against the same fixture model
3. Merge-rule scenarios cover intersection, union, and strongest-wins behavior
4. Express-track parity cases are present in the fixture suite
5. Fixture helpers keep tests isolated to temp directories

## Tasks / Subtasks

- [ ] Create or extend temp-directory fixture helpers for governance trees
- [ ] Add representative full and sparse hierarchy fixtures
- [ ] Use the same fixture model across resolve, compliance, and display tests
- [ ] Add express-track parity fixture cases
- [ ] Record any uncovered repo-level fixture need in Completion Notes

## Dev Notes

### Implementation Notes

- Temp-directory fixtures are required. Do not point tests at a real governance checkout.
- The fixture model should make sparse hierarchies easy to express and easy to reason about.
- Keep parity fixtures focused on contract behavior, not incidental formatting.

### References

- [sprint-plan.md](../sprint-plan.md)
- [tech-plan.md](../tech-plan.md)
- [implementation-readiness.md](../implementation-readiness.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List