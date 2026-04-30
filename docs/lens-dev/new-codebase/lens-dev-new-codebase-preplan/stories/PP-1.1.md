---
feature: lens-dev-new-codebase-preplan
story_id: PP-1.1
epic: PP-E1
title: Add command prompt surfaces
estimate: S
sprint: 1
status: not-started
depends_on: []
blocks: [PP-1.2]
updated_at: 2026-04-28T00:00:00Z
---

# PP-1.1 — Add command prompt surfaces

## Story

**As a** Lens user starting a new feature,  
**I want** the `preplan` command to be available and discoverable in the new codebase,  
**so that** I can invoke `/preplan` through the prompt surface without manual path configuration.

## Context

The 17-command surface requires `preplan` to be installed as a stub in `.github/prompts/` that delegates through light preflight to the release prompt. The release prompt then loads the SKILL.md conductor. Neither file contains any implementation logic — they are pure redirect chains.

The stub follows the same pattern as all other Lens command stubs:
1. Run `light-preflight.py` from workspace root.
2. If non-zero exit, stop.
3. Delegate to the release prompt.

The release prompt follows the same pattern:
1. Load and follow `_bmad/lens-work/skills/bmad-lens-preplan/SKILL.md`.

## Implementation Target

`TargetProjects/lens-dev/new-codebase/lens.core.src/`

## Files to Create

```
.github/prompts/lens-preplan.prompt.md          ← stub
_bmad/lens-work/prompts/lens-preplan.prompt.md  ← release redirect
```

## Reference

See the existing `lens-expressplan.prompt.md` stub and the installed prompt surface in `lens.core/` as behavioral reference. Do NOT copy files from `lens.core/` into the new codebase — author them independently.

## Acceptance Criteria

- [ ] `.github/prompts/lens-preplan.prompt.md` exists in `lens.core.src`; runs `uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py` and delegates to the release prompt on success.
- [ ] `_bmad/lens-work/prompts/lens-preplan.prompt.md` exists; loads `lens.core/_bmad/lens-work/skills/bmad-lens-preplan/SKILL.md` with no additional logic.
- [ ] Stub redirect path stays relative to `lens.core/` (no hardcoded absolute paths).
- [ ] No old-codebase file patterns appear in the diff.
- [ ] No file copying from `lens.core/` into `lens.core.src`.

## Definition of Done

- Story files created and committed on a feature branch in the target project.
- All acceptance criteria above checked off.
- PR opened targeting the `lens-dev-new-codebase-preplan` integration branch in the target project.
