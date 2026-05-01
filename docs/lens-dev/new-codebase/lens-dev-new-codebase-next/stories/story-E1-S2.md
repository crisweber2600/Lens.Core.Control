# Story E1-S2: Create release prompt redirect

**Feature:** lens-dev-new-codebase-next
**Epic:** Epic 1 — Prompt Chain and Discovery
**Status:** done

---

## Story

As a Lens module maintainer,
I want a thin release-prompt redirect at `_bmad/lens-work/prompts/lens-next.prompt.md`
that points to the owning SKILL.md
so that future edits to routing logic live in exactly one place.

## Acceptance Criteria

1. `_bmad/lens-work/prompts/lens-next.prompt.md` exists in the target repo
2. The file is a thin redirect: it loads `bmad-lens-next/SKILL.md` and passes through control
3. No routing logic, no inline heuristics
4. Follows the same redirect pattern as `lens-finalizeplan.prompt.md` → `bmad-lens-finalizeplan/SKILL.md`

## Tasks / Subtasks

- [ ] Locate reference redirect (`_bmad/lens-work/prompts/lens-finalizeplan.prompt.md`)
- [ ] Author `_bmad/lens-work/prompts/lens-next.prompt.md` as thin redirect to `bmad-lens-next/SKILL.md`
  - [ ] No routing logic (AC #3)
  - [ ] Follows reference redirect pattern (AC #4)
- [ ] Commit to `lens.core.src` develop branch

## Dev Notes

- **Target file:** `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-next.prompt.md`
- **Reference pattern:** `_bmad/lens-work/prompts/lens-finalizeplan.prompt.md`

### References
- [tech-plan.md — thin conductor pattern](../tech-plan.md)
- [epics.md — Epic 1 scope](../epics.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List
