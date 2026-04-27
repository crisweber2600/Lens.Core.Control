---
feature: lens-dev-new-codebase-switch
story_id: SW-3
epic: EP-1
sprint: 1
title: Lock Numbered Menu Behavior
estimate: M
status: review
blocked_by: [SW-2]
assignee: crisweber2600
doc_type: story
updated_at: 2026-04-27T18:00:00Z
---

# SW-3 — Lock Numbered Menu Behavior

## Context

When switch is invoked without an explicit feature id, it must call `list` and render a numbered menu. Selection must be explicit — no inference from git state, editor state, or conversation history. The `domains` fallback mode must render inventory and stop without asking for a number. Invalid input rerenders and stops. `q` cancels cleanly.

**Requirement:** SW-B3, SW-B4

## Task

In the switch release prompt and/or `switch-ops.py`:

1. Auto-invoke `list` when no feature id is supplied.
2. On `mode: domains` response: render domain/service inventory table, then stop.
3. On `mode: features` response: render numbered list (num, id, domain/service, phase, owner), then ask for a number or `q`.
4. Numeric input in range → map to `features[n-1].id` → invoke switch.
5. `q` → cancel, report "Switch cancelled. No changes made."
6. Out-of-range, non-numeric, or blank → rerender the same menu and stop (do not guess).
7. At no point use branch name, open file, recent path, or previous answer as a feature substitute.

## Files

- `lens.core/_bmad/lens-work/prompts/lens-switch.prompt.md` (release prompt — menu interaction)
- `lens.core/_bmad/lens-work/skills/bmad-lens-switch/scripts/switch-ops.py` (`list` operation)

## Acceptance Criteria

- [ ] No feature id supplied → `list` is called automatically.
- [ ] `mode: domains` → renders inventory table, stops without asking for a number.
- [ ] `mode: features` → renders numbered list and prompts for number or `q`.
- [ ] Valid number → correct feature id selected and switch proceeds.
- [ ] `q` → clean cancellation, no side effects.
- [ ] Invalid input → menu rerenders, stops, no feature selected, no side effects.
- [ ] Regression fixture: domains mode produces no feature selection.
- [ ] Regression fixture: invalid input (text, out-of-range) produces no feature selection.

## Dev Notes

- The no-inference constraint is absolute. Do not add any "most-recently-used" or "current branch" shortcuts.
- If `vscode_askQuestions` is available in the agent environment, the prompt may use it to collect the numeric selection; otherwise render the menu and stop.

## Status

review

## Dev Agent Record

### Debug Log

- 2026-04-27: Implemented `SW-3` under target repo `TargetProjects/lens-dev/new-codebase/lens.core.src` on branch `feature/switch-dev`.
- 2026-04-27: Verified with `uv run --with pytest _bmad/lens-work/skills/bmad-lens-switch/scripts/tests/test-switch-ops.py -q`.
- 2026-04-27: Verified with `uv run --with pytest --with pyyaml python -m pytest _bmad/lens-work -q`.

### Completion Notes

- Locked list output numbering and documented no-inference menu behavior; regression covers domains/features modes.
- Story status moved to `review`; implementation is ready for code review.

### File List

- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/agents/lens.agent.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-work-setup/assets/module-help.csv`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/module-help.csv`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-switch.prompt.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-switch/SKILL.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-switch/references/list-features.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-switch/references/switch-feature.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-switch/scripts/switch-ops.py`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-switch/scripts/tests/test-switch-ops.py`
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-switch/stories/SW-3.md`
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-switch/sprint-status.yaml`

### Change Log

- 2026-04-27: Implemented, tested, and moved to review.

