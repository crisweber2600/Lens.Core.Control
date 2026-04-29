---
feature: lens-dev-new-codebase-trueup
story_id: "TU-2.1"
story_key: "TU-2-1-author-bmad-lens-complete-skill"
epic: "EP-2"
title: "Author bmad-lens-complete SKILL.md via BMB Channel"
status: ready-for-dev
priority: must
story_points: 5
depends_on: []
updated_at: 2026-04-29T00:00:00Z
---

# Story TU-2.1: Author bmad-lens-complete SKILL.md via BMB Channel

Status: ready-for-dev

## Story

As a Lens Workbench maintainer,
I want a complete `SKILL.md` for the `bmad-lens-complete` skill authored via the BMad Module Builder (BMB) channel,
so that the `lens-complete` command has a fully documented command contract in the new-codebase source.

## Acceptance Criteria

1. **Pre-checks (CF-1 — must verify before authoring begins):**
   - Load `lens.core/_bmad/bmb/bmadconfig.yaml` and verify the BMB module is accessible.
   - Load the BMad Builder reference index at `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md`.
   - Invoke the `bmad-module-builder` skill before authoring the SKILL.md content.

2. `lens.core.src/_bmad/lens-work/skills/bmad-lens-complete/SKILL.md` created and committed.

3. `check-preconditions` command contract documented:
   - Inputs: feature identifier (or auto-detection)
   - Guards: phase check (must be at a terminal phase), retrospective check (presence of retrospective artifact)
   - Return shape: `{status: pass|warn|fail, messages: [...], missing_artifacts: [...]}`
   - Graceful-degradation behavior: if retrospective is absent, warn (not fail) and document the gap (ADR-1)

4. `finalize` command contract documented:
   - Inputs: feature identifier, `--dry-run` flag
   - Irreversible confirmation gate: user must confirm before phase is set to `complete`
   - Dry-run mode: shows what would change without writing
   - Operations list: phase transition, governance archive write, control repo branch cleanup (if applicable)

5. `archive-status` command contract documented:
   - Read-only command
   - Return shape: `{archived: bool, archived_at: ISO8601|null, reason: string|null}`

6. SKILL.md follows the established module SKILL.md format for the BMB module system (frontmatter, command table, per-command sections).

## Tasks / Subtasks

- [ ] Load `lens.core/_bmad/bmb/bmadconfig.yaml`.
- [ ] Load BMad Builder reference index at `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md`.
- [ ] Invoke `bmad-module-builder` skill.
- [ ] Review existing SKILL.md files in `lens.core.src/_bmad/lens-work/skills/` for format reference.
- [ ] Review ADR-1 in `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/adr-complete-prerequisite.md` for the prerequisite handling strategy.
- [ ] Author `SKILL.md` with three command contracts: `check-preconditions`, `finalize`, `archive-status`.
- [ ] Commit with message: `[DEV] TU-2.1 — author bmad-lens-complete SKILL.md via BMB channel`.

## Dev Notes

- The `check-preconditions` graceful-degradation behavior is the core ADR-1 decision: retrospective absent → warn, not fail. This must be accurately reflected in the SKILL.md.
- The `finalize` command is irreversible (phase set to `complete`). The confirmation gate is required to prevent accidental invocation.
- Do not conflate `bmad-lens-complete` with `bmad-lens-finalizeplan`. This skill archives an already-finalized feature; `finalizeplan` is the planning phase that produces epics/stories.
- The companion test stubs are in TU-2.2 — they must be consistent with the command contracts authored here.

### References

- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/architecture.md` — Section 3.2, ADR-1 (Section 4)
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/adr-complete-prerequisite.md` — binding decision (once authored in TU-3.1)
- `lens.core/_bmad/bmb/bmadconfig.yaml` — BMB module configuration
- `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md` — BMad Builder reference

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
