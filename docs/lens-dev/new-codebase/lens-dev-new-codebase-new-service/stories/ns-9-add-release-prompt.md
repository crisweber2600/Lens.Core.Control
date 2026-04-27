# Story NS-9: Add release prompt

Status: ready-for-dev

## Story

As a user invoking `/new-service`,
I want a release prompt that delegates to `bmad-lens-init-feature` with `create-service` intent,
so that the command works from any Lens-equipped IDE.

## Acceptance Criteria

1. `lens-new-service.prompt.md` exists at `_bmad/lens-work/prompts/lens-new-service.prompt.md` in the new-codebase source
2. Prompt loads `bmad-lens-init-feature/SKILL.md` and specifies `create-service` intent
3. Prompt requires config resolution for: `governance_repo`, optional `target_projects_path`, optional `output_folder`, required `personal_output_folder`
4. Prompt instructs callers to pass `--execute-governance-git` and report `governance_commit_sha` on success
5. **Implementation channel:** release prompt authored through `.github/skills/bmad-workflow-builder`
6. NS-3 prompt discovery check turns green
7. Breaking change: false

## Tasks / Subtasks

- [ ] Task 1: Author release prompt through `bmad-workflow-builder` channel (AC: 1–5)
  - [ ] Open `.github/skills/bmad-workflow-builder/SKILL.md` and follow its workflow for adding a release prompt
  - [ ] Create `_bmad/lens-work/prompts/lens-new-service.prompt.md` in the new-codebase source
  - [ ] Prompt header: loads SKILL.md, specifies `create-service` intent
  - [ ] Prompt body: config resolution block (governance_repo, target_projects_path, output_folder, personal_output_folder)
  - [ ] Prompt body: instruction to pass `--execute-governance-git` and return SHA
- [ ] Task 2: Verify NS-3 prompt check turns green (AC: 6)

## Dev Notes

- **Implementation channel rule (H2):** release prompt and workflow artifacts route through `.github/skills/bmad-workflow-builder`. Open that skill and follow its authoring pattern.
- Model after the existing `lens-new-domain.prompt.md` (look in `_bmad/lens-work/prompts/` for the domain prompt)
- Target path: `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-new-service.prompt.md`
- The prompt is a stub that delegates; it should be short (< 30 lines) — no implementation logic in the prompt itself
- Breaking change is false: this is an additive change

### Project Structure Notes

- Published prompt stub (control repo): `.github/prompts/lens-new-service.prompt.md`
- Release prompt (new-codebase source): `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-new-service.prompt.md`
- Both files must exist for the full discovery chain to work

### References

- [tech-plan.md § Release Prompt Contract](../tech-plan.md)
- [finalizeplan-review.md § H2 response — bmad-workflow-builder channel](../finalizeplan-review.md)
- [stories.md § NS-9](../stories.md)

## Dev Agent Record

### Agent Model Used

_to be filled by dev agent_

### Debug Log References

### Completion Notes List

### File List
