---
feature: lens-dev-new-codebase-trueup
story_id: "TU-1.1"
story_key: "TU-1-1-verify-lens-switch-prompt"
epic: "EP-1"
title: "Verify lens-switch.prompt.md Prompt Stub"
status: ready-for-dev
priority: must
story_points: 1
depends_on: []
updated_at: 2026-04-29T00:00:00Z
---

# Story TU-1.1: Verify lens-switch.prompt.md Prompt Stub

Status: ready-for-dev

## Story

As a Lens Workbench operator,
I want `lens-switch.prompt.md` verified as present and correctly formatted in `_bmad/lens-work/prompts/` in the new-codebase source,
so that the `lens-switch` command has a properly scaffolded entry point for all new projects that install from the new codebase.

## Acceptance Criteria

1. `lens.core.src/_bmad/lens-work/prompts/lens-switch.prompt.md` exists in the target repo on the plan branch.
2. The file follows the established stub format: stub header identifying the command, `light-preflight.py` invocation, and a pointer to the full prompt source.
3. The file is committed to the target repo.
4. `.github/prompts/lens-switch.prompt.md` mirroring is **NOT** an acceptance criterion for this story — that is a post-dev human action (CF-6).

## Tasks / Subtasks

- [ ] Navigate to `lens.core.src/_bmad/lens-work/prompts/` in the target repo source tree.
- [ ] Verify `lens-switch.prompt.md` exists.
  - [ ] If it exists: confirm it follows the stub format (stub header + light-preflight invocation + full-prompt pointer).
  - [ ] If it does not exist: author the stub following the same pattern as other prompt stubs in the directory.
- [ ] Commit verification result with message: `[DEV] TU-1.1 — verify lens-switch.prompt.md stub present and formatted`.

## Dev Notes

- The authority domain rule (architecture Section 5 + finalizeplan-review H1) prohibits the dev agent from writing to `.github/prompts/`. Stay within `_bmad/lens-work/prompts/` only.
- If the file already exists and is correct, this story closes as a trivial pass — document it in the commit message.
- Reference: `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/stories.md` TU-1.1.

### References

- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/architecture.md` — Section 3.1 (prompt stub layer)
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/finalizeplan-review.md` — H1 finding (authority domain)
- Other existing prompt stubs in `lens.core.src/_bmad/lens-work/prompts/` (use as format reference)

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
