# Story E3-S3: Keep prompt, skill, and references aligned to the script contract

**Feature:** lens-dev-new-codebase-constitution
**Epic:** Epic 3 - Compliance and Progressive Display Parity
**Status:** ready-for-dev

---

## Story

As a Lens maintainer,
I want the prompt, skill, and references to describe the same script-backed contract
so that future changes do not reintroduce inline logic or stale documentation.

## Acceptance Criteria

1. SKILL.md capability descriptions match implemented script behavior
2. Release prompt remains a thin redirect to the skill
3. Reference docs cover partial hierarchies, express parity, and safety behavior
4. No document claims a write-capable path for constitution operations

## Tasks / Subtasks

- [ ] Audit SKILL.md against the implemented script interface
- [ ] Audit the release prompt for drift or inlined behavior
- [ ] Update references to reflect partial-hierarchy, express, and safety contracts
- [ ] Remove or flag any stale write-capable wording

## Dev Notes

### Implementation Notes

- This story closes documentation drift after behavior work lands.
- Keep the prompt and skill focused on orchestration. All substantive behavior stays in the
  script and its tests.
- Use the references to make future reviews cheaper: drift should be obvious from docs.

### References

- [tech-plan.md](../tech-plan.md)
- [epics.md](../epics.md)
- [implementation-readiness.md](../implementation-readiness.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List