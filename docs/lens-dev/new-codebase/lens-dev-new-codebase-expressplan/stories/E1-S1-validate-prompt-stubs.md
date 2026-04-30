---
story_id: E1-S1
epic: E1
feature: lens-dev-new-codebase-expressplan
title: Validate Prompt Stubs
priority: High
size: XS
status: not-started
updated_at: '2026-04-30T00:00:00Z'
---

# E1-S1 — Validate Prompt Stubs

## Context

The expressplan command requires two prompt stubs in the target source repo:
- `.github/prompts/lens-expressplan.prompt.md`
- `_bmad/lens-work/prompts/lens-expressplan.prompt.md`

These were created in a previous session but may not be committed or may drift from the
shared prompt-start preflight pattern used by other retained commands.

## Tasks

1. Open `.github/prompts/lens-expressplan.prompt.md` in the target source repo
   (`TargetProjects/lens-dev/new-codebase/lens.core.src/`).
2. Compare its structure against another retained command prompt, e.g.
   `.github/prompts/lens-techplan.prompt.md`.
3. Confirm the prompt-start preflight pattern is present and matches.
4. Open `_bmad/lens-work/prompts/lens-expressplan.prompt.md` and confirm it delegates
   to the SKILL.md.
5. Verify both files are tracked by git (`git status` / `git add` if untracked).
6. Commit any untracked or modified files with message
   `[dev] E1-S1 — commit prompt stubs for expressplan`.

## Acceptance Criteria

- [ ] `.github/prompts/lens-expressplan.prompt.md` exists and matches preflight pattern.
- [ ] `_bmad/lens-work/prompts/lens-expressplan.prompt.md` exists and delegates correctly.
- [ ] Both files are committed in the target source repo.
