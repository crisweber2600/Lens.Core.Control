# Story E2-S1: Rewrite partial-hierarchy resolution flow

**Feature:** lens-dev-new-codebase-constitution
**Epic:** Epic 2 - Resolver Core and Merge Contract
**Status:** ready-for-dev

---

## Story

As a Lens maintainer,
I want `resolve` to skip missing constitution levels with warnings instead of hard-failing
so that partial governance hierarchies remain valid for callers across planning and dev.

## Acceptance Criteria

1. Missing org, domain, service, or repo levels append structured warnings instead of errors
2. Valid partial hierarchies return exit code `0`
3. Empty hierarchies return defaults plus warnings rather than a hard failure
4. Resolve output includes `levels_loaded`, merged constitution data, and warning details
5. No caller-specific workaround logic is added outside the script

## Tasks / Subtasks

- [ ] Replace the org-level hard-fail path in `constitution-ops.py`
- [ ] Add `level_absent` warnings for each skipped hierarchy level
- [ ] Implement the empty-hierarchy defaults path
- [ ] Keep the result payload stable for existing callers
- [ ] Add regression coverage for org-missing and sparse-hierarchy scenarios

## Dev Notes

### Implementation Notes

- The fix belongs in `constitution-ops.py`, not in prompts or SKILL.md.
- Missing levels are valid governance states. Missing-level warnings must be explicit and
  structured.
- Reserve exit code `1` for bad arguments or unreadable paths, not valid sparse hierarchies.

### References

- [tech-plan.md](../tech-plan.md)
- [architecture.md](../architecture.md)
- [finalizeplan-review.md](../finalizeplan-review.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List