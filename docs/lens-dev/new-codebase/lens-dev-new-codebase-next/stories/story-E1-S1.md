# Story E1-S1: Create public prompt stub

**Feature:** lens-dev-new-codebase-next
**Epic:** Epic 1 — Prompt Chain and Discovery
**Status:** done

---

## Story

As a Lens user,
I want a `lens-next.prompt.md` stub in `.github/prompts/` that runs light preflight and
then loads the release prompt
so that the `next` command is invokable from any Copilot session and fails gracefully on
bad state.

## Acceptance Criteria

1. `.github/prompts/lens-next.prompt.md` exists in the target repo
2. Opening the prompt triggers `light-preflight.py`; a non-zero exit stops execution with
   the preflight error message
3. On success the stub loads `_bmad/lens-work/prompts/lens-next.prompt.md`
4. The stub contains no inline routing logic
5. The stub follows the identical structure as other retained-command stubs (cross-check
   against `lens-finalizeplan.prompt.md` or `lens-expressplan.prompt.md`)

## Tasks / Subtasks

- [ ] Locate reference stubs (`.github/prompts/lens-finalizeplan.prompt.md` or
  `lens-expressplan.prompt.md`) for structural comparison
- [ ] Author `.github/prompts/lens-next.prompt.md` following the identical pattern
  - [ ] Preflight block calls `light-preflight.py` (AC #2)
  - [ ] On success, loads release prompt (AC #3)
  - [ ] No inline routing logic (AC #4)
- [ ] Verify structure matches reference stubs character-for-character in the shared blocks (AC #5)
- [ ] Commit to `lens.core.src` develop branch (feature branch as appropriate)

## Dev Notes

- **Target file:** `TargetProjects/lens-dev/new-codebase/lens.core.src/.github/prompts/lens-next.prompt.md`
- **Reference pattern:** `lens-finalizeplan.prompt.md` — use as structural template for preflight + load
- **Clean-room constraint:** Do not copy inline logic from old-codebase next prompt; follow the shared preflight pattern only
- **Architecture contract:** [tech-plan.md](../tech-plan.md) §2 (prompt-start chain) and §3 (thin conductor pattern)

### References
- [tech-plan.md — §2 Prompt-start chain preserved](../tech-plan.md)
- [business-plan.md — prompt-start chain preservation requirement](../business-plan.md)
- [epics.md — Epic 1 scope](../epics.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List
