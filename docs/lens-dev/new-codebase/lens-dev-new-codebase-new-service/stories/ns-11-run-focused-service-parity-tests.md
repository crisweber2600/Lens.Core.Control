# Story NS-11: Run focused service parity tests

Status: ready-for-dev

## Story

As a QA engineer,
I want to run the focused service parity test suite in isolation,
so that I can confirm `create-service` behavior before running the full regression.

## Acceptance Criteria

1. `uv run --with pytest pytest -k create_service` passes from the new-codebase source root
2. All NS-1, NS-2, NS-3 contract and boundary tests are green
3. No test is skipped without a documented justification in NS-13 handoff notes
4. Test run output is captured and referenced in NS-13 handoff notes

## Tasks / Subtasks

- [ ] Task 1: Run focused suite (AC: 1–2)
  - [ ] From `TargetProjects/lens-dev/new-codebase/lens.core.src/`: run `uv run --with pytest pytest _bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/ -k create_service -q`
  - [ ] All tests must pass
- [ ] Task 2: Document any skipped tests (AC: 3)
  - [ ] If any test is marked `@pytest.mark.skip`, record reason in NS-13 handoff notes
- [ ] Task 3: Capture test output (AC: 4)
  - [ ] Save the test run summary for NS-13 handoff notes

## Dev Notes

- Run command: `uv run --with pytest pytest _bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/ -k create_service -q`
- Expected: all 8+ create_service tests green
- If any test fails at this point, debug and fix before proceeding to NS-12
- This story is pure verification — no new code changes, only running existing tests

### Project Structure Notes

- Working directory: `TargetProjects/lens-dev/new-codebase/lens.core.src/`
- The `-k create_service` filter selects tests with `create_service` in the name — ensure all NS-1/NS-2/NS-3 test function names include this string

### References

- [sprint-plan.md § NS-11](../sprint-plan.md)
- [stories.md § NS-11](../stories.md)

## Dev Agent Record

### Agent Model Used

_to be filled by dev agent_

### Debug Log References

### Completion Notes List

### File List
