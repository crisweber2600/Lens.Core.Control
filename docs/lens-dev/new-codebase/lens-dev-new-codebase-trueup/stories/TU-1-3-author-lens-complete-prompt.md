---
feature: lens-dev-new-codebase-trueup
story_id: "TU-1.3"
story_key: "TU-1-3-author-lens-complete-prompt"
epic: "EP-1"
title: "Author lens-complete.prompt.md Prompt Stub"
status: ready-for-dev
priority: must
story_points: 2
depends_on: []
updated_at: 2026-04-29T00:00:00Z
---

# Story TU-1.3: Author lens-complete.prompt.md Prompt Stub

Status: ready-for-dev

## Story

As a Lens Workbench operator,
I want `lens-complete.prompt.md` authored in `_bmad/lens-work/prompts/` in the new-codebase source,
so that users of new-codebase installations have a properly scaffolded `lens-complete` command entry point that invokes the `bmad-lens-complete` skill.

## Acceptance Criteria

1. `lens.core.src/_bmad/lens-work/prompts/lens-complete.prompt.md` created and committed.
2. Stub header identifies the command as `lens-complete`.
3. Stub invokes `light-preflight.py` before loading the full prompt.
4. Stub points to `lens.core/_bmad/lens-work/prompts/lens-complete.prompt.md` as the full prompt source.
5. Content matches the authority and scope of the `bmad-lens-complete` skill (not `bmad-lens-finalizeplan`, not `bmad-lens-init-feature`).
6. `.github/prompts/lens-complete.prompt.md` mirroring is **NOT** an acceptance criterion — post-dev human action (CF-6).

## Tasks / Subtasks

- [ ] Review other existing prompt stubs in `lens.core.src/_bmad/lens-work/prompts/` for format reference.
- [ ] Author `lens-complete.prompt.md` following the stub format.
- [ ] Ensure stub content refers to `bmad-lens-complete` skill scope — the command that marks a feature complete/archived, not the planning skill.
- [ ] Commit with message: `[DEV] TU-1.3 — author lens-complete.prompt.md stub`.

## Dev Notes

- Authority domain rule (CF-6): `.github/prompts/` is out of scope. The dev agent must not write to `.github/prompts/`. Publishing there is a post-dev human action.
- Disambiguation: `lens-complete` refers to the feature-archival skill (`bmad-lens-complete`), not the `bmad-lens-finalizeplan` or `bmad-lens-techplan` planning phases. The stub must be scoped correctly.
- Reference TU-2.1 (SKILL.md for `bmad-lens-complete`) as the companion story — the stub and the skill should be consistent in command name and trigger.

### References

- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/architecture.md` — Section 3.1
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/finalizeplan-review.md` — H1, CF-6
- Existing stubs in `lens.core.src/_bmad/lens-work/prompts/` (format reference)
- TU-2.1 story (companion — `bmad-lens-complete` SKILL.md)

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
