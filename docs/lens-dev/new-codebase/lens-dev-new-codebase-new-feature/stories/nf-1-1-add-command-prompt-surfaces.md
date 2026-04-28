# Story NF-1.1: Add Command Prompt Surfaces

**Feature:** lens-dev-new-codebase-new-feature  
**Epic:** 1 — Command Surface and Parity Foundation  
**Estimate:** M  
**Sprint:** 1  
**Status:** backlog  
**Depends on:** —  
**Blocks:** NF-1.2, NF-2.1  
**Updated:** 2026-04-27

---

## Goal

Create the installed stub prompt and release prompt for the `new-feature` command in the new codebase. The command must be discoverable via the same two prompt surfaces as every other Lens command: an installed `.github/prompts/` stub and a release prompt under `_bmad/lens-work/prompts/`.

---

## Acceptance Criteria

- [ ] `lens.core/.github/prompts/lens-new-feature.prompt.md` exists in the new codebase
  - Runs `uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py` on activation
  - Gracefully surfaces preflight failure without crashing the shell menu
- [ ] `lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md` exists in the new codebase
  - Loads `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md`
- [ ] Both prompt paths resolve correctly when `lens.core/` is the installed module root (paths are relative, not absolute)
- [ ] No old-codebase prompt file is copied verbatim into the new codebase

---

## Technical Context

### Architecture Path

```
.github/prompts/lens-new-feature.prompt.md        ← installed stub (runs preflight)
  └── triggers →
      lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md  ← release prompt
          └── loads →
              lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md
                  └── invokes →
                      init-feature-ops.py create | fetch-context
```

### Naming Change

The old codebase uses `lens-init-feature` for this command; the new codebase renames it to `lens-new-feature`. This rename was resolved in `lens-dev-new-codebase-baseline` — do not use the old name.

### Preflight Integration

Light preflight (`light-preflight.py`) does NOT run the full Lens preflight. It only verifies:
1. `uv` is available
2. Required config files exist
3. Returns a non-blocking result if checks fail (logs warning, does not halt)

The preflight invocation in the stub must use a **workspace-relative** path (`./lens.core/...`), not an absolute path or a path relative to the script location.

### Reference Files in Old Codebase

Both files below exist in the old codebase as structural reference. Read the format/invocation pattern; do not copy verbatim.

- `TargetProjects/lens-dev/old-codebase/lens.core.src/.github/prompts/lens-init-feature.prompt.md`
- `TargetProjects/lens-dev/old-codebase/lens.core.src/_bmad/lens-work/prompts/lens-init-feature.prompt.md`

### Target File Paths in New Codebase

```
TargetProjects/lens-dev/new-codebase/lens.core.src/.github/prompts/lens-new-feature.prompt.md
TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-new-feature.prompt.md
```

---

## Test Requirements

This story does not have automated tests. Manual validation:
1. Open VS Code with `lens.core/` installed — confirm `lens-new-feature` appears in the prompt menu
2. Trigger the stub — confirm preflight runs without error
3. Confirm SKILL.md loads from the release prompt

Automated coverage starts in NF-1.3.

---

## Definition of Done

- Both prompt files created in the correct paths in `TargetProjects/lens-dev/new-codebase/lens.core.src/`
- Preflight invocation is workspace-relative
- Manual verification passes
- No verbatim old-codebase copies
