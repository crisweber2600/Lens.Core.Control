# Story NS-3: Add prompt and help discovery expectations

Status: ready-for-dev

## Story

As a Lens user,
I want static checks or tests that confirm `/new-service` is discoverable from both the prompt stub and module help surfaces,
so that the command does not ship silently or become unreachable.

## Acceptance Criteria

1. Test or static check confirms `lens-new-service.prompt.md` exists at the expected release path
2. Test or static check confirms the module help entry for `new-service` exists and names the retained command
3. Checks are in failing/absent state until NS-9 (release prompt) and NS-10 (module help) are delivered

## Tasks / Subtasks

- [ ] Task 1: Add discovery checks to test suite (AC: 1–3)
  - [ ] `test_new_service_prompt_exists` — assert `_bmad/lens-work/prompts/lens-new-service.prompt.md` exists in new-codebase source root
  - [ ] `test_new_service_module_help_entry` — assert `module-help.csv` contains a row for `new-service`; assert the row names the prompt path

## Dev Notes

- These tests should be in `tests/test_create_service.py` or a dedicated `tests/test_discovery.py`
- Tests are expected to **fail** at the end of NS-3 (file doesn't exist yet) — they are green checkpoints that NS-9 and NS-10 must satisfy
- Use `pathlib.Path` relative to the new-codebase source root for path assertions
- Module help CSV path: `_bmad/lens-work/module-help.csv` (verify against existing `new-domain` tests for exact path)
- The prompt path to check: `_bmad/lens-work/prompts/lens-new-service.prompt.md`

### Project Structure Notes

- module-help.csv is in `lens.core/_bmad/lens-work/module-help.csv` (resolve from script working directory)
- NS-3 tests should be parameterizable so they can be run independently as a smoke test during NS-10 verification

### References

- [tech-plan.md § Release Prompt Contract](../tech-plan.md)
- [sprint-plan.md § NS-3](../sprint-plan.md)
- [stories.md § NS-3](../stories.md)
- [finalizeplan-review.md § Party-Mode Challenge — John PM](../finalizeplan-review.md)

## Dev Agent Record

### Agent Model Used

_to be filled by dev agent_

### Debug Log References

### Completion Notes List

### File List
