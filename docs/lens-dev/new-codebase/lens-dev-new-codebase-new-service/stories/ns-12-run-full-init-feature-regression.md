# Story NS-12: Run full init-feature regression

Status: ready-for-dev

## Story

As a release engineer,
I want the complete init-feature test suite to pass after `create-service` is added,
so that I can be confident the `new-domain` path is not regressed.

## Acceptance Criteria

1. Full init-feature test suite passes: `uv run --with pytest pytest _bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/ -q`
2. No `new-domain` tests regress
3. **NS-13 gate check:** before marking NS-12 complete, verify that NS-13 handoff notes exist in `stories/ns-13-prepare-implementation-handoff-notes.md` AND contain all 7 required items listed in NS-13 acceptance criteria
4. NS-12 is the explicit gating check for NS-13 completeness — both must be satisfied simultaneously

## Tasks / Subtasks

- [ ] Task 1: Run full suite (AC: 1–2)
  - [ ] From `TargetProjects/lens-dev/new-codebase/lens.core.src/`: run `uv run --with pytest pytest _bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/ -q`
  - [ ] All tests must pass, including `new-domain` tests
- [ ] Task 2: NS-13 gate check (AC: 3)
  - [ ] Open `stories/ns-13-prepare-implementation-handoff-notes.md`
  - [ ] Verify NS-13 Completion Notes List contains all 7 items from NS-13 acceptance criteria
  - [ ] Only mark NS-12 complete after NS-13 gate check passes

## Dev Notes

- Run command: `uv run --with pytest pytest _bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/ -q`
- If any `create-domain` or `new-domain` test fails after `create-service` changes, debug and fix before proceeding
- The NS-13 gate check is mandatory — do not skip it even if the test run passes

### Project Structure Notes

- Working directory: `TargetProjects/lens-dev/new-codebase/lens.core.src/`
- NS-13 handoff notes location: `docs/lens-dev/new-codebase/lens-dev-new-codebase-new-service/stories/ns-13-prepare-implementation-handoff-notes.md` in the control repo

### References

- [sprint-plan.md § NS-12](../sprint-plan.md)
- [stories.md § NS-12](../stories.md)

## Dev Agent Record

### Agent Model Used

_to be filled by dev agent_

### Debug Log References

### Completion Notes List

### File List
