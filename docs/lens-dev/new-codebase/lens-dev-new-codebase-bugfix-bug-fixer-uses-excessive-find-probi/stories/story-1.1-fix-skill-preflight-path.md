---
feature: lens-dev-new-codebase-bugfix-bug-fixer-uses-excessive-find-probi
story_id: "1.1"
epic: 1
title: Fix SKILL.md preflight path and working directory
points: 2
status: Ready
assignee: crisweber2600
updated_at: '2026-05-03T19:15:00Z'
---

# Story 1.1 — Fix SKILL.md Preflight Path and Working Directory

## Context

The `/lens-bug-fixer` conductor (`bmad-lens-bug-fixer/SKILL.md`) invokes the light preflight
check but does not specify an exact working directory or script path. This forces the AI
conductor to probe for the script using `find` and `cat` commands, causing double-invocation
and extraneous file access.

The Lens workbench has one canonical preflight location:

```
TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/lens-preflight/scripts/light-preflight.py
```

And one canonical CWD for script invocation:

```
{control_repo}/TargetProjects/lens-dev/new-codebase/lens.core.src
```

## Files to Change

| File | Action |
|---|---|
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-bug-fixer/SKILL.md` | Edit — replace vague preflight step with explicit CWD + path block |

> ⚠️ Do NOT edit `lens.core/_bmad/lens-work/bmad-lens-bug-fixer/SKILL.md` — that is the
> read-only release clone.

## Acceptance Criteria

- [ ] "On Activation" step 1 (or equivalent preflight step) in SKILL.md specifies exact CWD
- [ ] The exact relative script path `_bmad/lens-work/lens-preflight/scripts/light-preflight.py` is present
- [ ] No `find` or `cat` probing commands appear before the preflight invocation in a fresh run
- [ ] Preflight is invoked exactly once per `--fix-all-new` run (verified via execution trace)
- [ ] The invocation block reads:
  ```
  cd "{control_repo}/TargetProjects/lens-dev/new-codebase/lens.core.src"
  uv run --script _bmad/lens-work/lens-preflight/scripts/light-preflight.py
  ```

## Implementation Steps

1. Open `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-bug-fixer/SKILL.md`
2. Locate the "On Activation" preflight step (usually the first step in the activation sequence)
3. Replace any vague description like "run light-preflight" with the explicit block above
4. Save and verify the file is in the `TargetProjects` path (not `lens.core/`)
5. Commit to feature branch `lens-dev-new-codebase-bugfix-bug-fixer-uses-excessive-find-probi` in `lens.core.src`

## Verification

Run `/lens-bug-fixer --fix-all-new` on a fresh session; inspect the tool call log:
- Exactly one `run_in_terminal` call to `light-preflight.py`
- No preceding `find` or `cat` for `SKILL.md` or `light-preflight.py`
