---
feature: lens-dev-new-codebase-trueup
story_id: "TU-1.2"
story_key: "TU-1-2-author-lens-new-feature-prompt"
epic: "EP-1"
title: "Author lens-new-feature.prompt.md Prompt Stub"
status: ready-for-dev
priority: must
story_points: 2
depends_on: []
updated_at: 2026-04-29T00:00:00Z
---

# Story TU-1.2: Author lens-new-feature.prompt.md Prompt Stub

Status: ready-for-dev

## Story

As a Lens Workbench operator,
I want `lens-new-feature.prompt.md` authored in `_bmad/lens-work/prompts/` in the new-codebase source,
so that users of new-codebase installations have a properly scaffolded `lens-new-feature` command entry point.

## Acceptance Criteria

1. `lens.core.src/_bmad/lens-work/prompts/lens-new-feature.prompt.md` created and committed.
2. Stub header identifies the command as `lens-new-feature`.
3. Stub invokes `light-preflight.py` before loading the full prompt.
4. Stub points to `lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md` as the full prompt source.
5. Content matches the authority and scope of the `bmad-lens-init-feature create` command.
6. `.github/prompts/lens-new-feature.prompt.md` mirroring is **NOT** an acceptance criterion — post-dev human action (CF-6).

## Tasks / Subtasks

- [ ] Review other existing prompt stubs in `lens.core.src/_bmad/lens-work/prompts/` for format reference.
- [ ] Author `lens-new-feature.prompt.md` following the stub format.
- [ ] Ensure stub content matches the `bmad-lens-init-feature create` command scope (does not invoke detect, merge-plan, or other subcommands).
- [ ] Commit with message: `[DEV] TU-1.2 — author lens-new-feature.prompt.md stub`.

## Dev Notes

- Authority domain rule (CF-6): `.github/prompts/` is out of scope for this story. The dev agent must not write to `.github/prompts/`. Publishing there is a post-dev human action.
- The prompt stub format (from other stubs in the same directory) consists of:
  1. A stub comment header identifying the command
  2. An instruction to run `light-preflight.py` (or reference to it)
  3. A redirect/pointer to the full prompt in `lens.core/_bmad/lens-work/prompts/`
- The full prompt (the non-stub version) is a separate artifact — this story only authors the stub.

### References

- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/architecture.md` — Section 3.1
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/finalizeplan-review.md` — H1, CF-6
- Existing stubs in `lens.core.src/_bmad/lens-work/prompts/` (format reference)

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
