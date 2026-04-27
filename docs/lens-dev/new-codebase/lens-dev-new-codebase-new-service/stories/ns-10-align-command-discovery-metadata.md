# Story NS-10: Align command discovery metadata

Status: ready-for-dev

## Story

As a Lens user browsing module help,
I want `new-service` to appear in the retained command index,
so that I can find the command without reading the full planning set.

## Acceptance Criteria

1. Module help CSV (`module-help.csv`) includes a row for `new-service` consistent with retained command policy
2. `new-service` row lists the prompt path and command description
3. No other retained commands are inadvertently removed or renamed
4. **Acceptance gate (John PM party-mode finding):** `new-service` must be verifiably discoverable from all listed entry points: prompt stub, release prompt, and module help. All three must exist before NS-10 is marked complete.
5. NS-3 module-help discovery check turns green

## Tasks / Subtasks

- [ ] Task 1: Add `new-service` entry to `module-help.csv` (AC: 1–3)
  - [ ] Add a row: command=`new-service`, prompt=`lens-new-service.prompt.md`, description="Initialize a service container in the governance repository"
  - [ ] Verify no existing row for `new-domain` or other retained commands is affected
- [ ] Task 2: Acceptance gate verification (AC: 4)
  - [ ] Confirm `.github/prompts/lens-new-service.prompt.md` exists (prompt stub)
  - [ ] Confirm `_bmad/lens-work/prompts/lens-new-service.prompt.md` exists (release prompt from NS-9)
  - [ ] Confirm `module-help.csv` row exists (this task)
  - [ ] All three verified — only then mark NS-10 complete
- [ ] Task 3: Run NS-3 module-help discovery check (AC: 5)

## Dev Notes

- Module help CSV path: `lens.core/_bmad/lens-work/module-help.csv` — look at the `new-domain` row for the column format
- **Discovery surface completeness (FinalizePlan review gap #2):** The baseline notes that command discovery spans three surfaces in sync. All three must be verified as part of NS-10 completion:
  1. Published prompt stub: `.github/prompts/lens-new-service.prompt.md`
  2. Release prompt: `_bmad/lens-work/prompts/lens-new-service.prompt.md`
  3. Module help entry: `module-help.csv`
- Also check `agents/lens.agent.md` for any agent command menu that should list `new-service` (gap #2 from FinalizePlan review — "agents/lens.agent.md shell menu, help-topics.yaml")
- If `agents/lens.agent.md` or `help-topics.yaml` exist and list `new-domain`, add a corresponding `new-service` entry

### Project Structure Notes

- Module help is in the lens.core source; target `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/module-help.csv`
- CSV column order: check `new-domain` entry for exact column names

### References

- [sprint-plan.md § NS-10 note](../sprint-plan.md)
- [finalizeplan-review.md § Party-Mode Challenge — John PM NS-10 acceptance gate](../finalizeplan-review.md)
- [finalizeplan-review.md § Gaps — gap #2 discovery surface completeness](../finalizeplan-review.md)
- [stories.md § NS-10](../stories.md)

## Dev Agent Record

### Agent Model Used

_to be filled by dev agent_

### Debug Log References

### Completion Notes List

### File List
