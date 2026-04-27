---
feature: lens-dev-new-codebase-switch
story_id: SW-1
epic: EP-1
sprint: 1
title: Verify Prompt-Start Gate Parity
estimate: S
status: review
blocked_by: []
assignee: crisweber2600
doc_type: story
updated_at: 2026-04-27T18:00:00Z
---

# SW-1 — Verify Prompt-Start Gate Parity

## Context

The published stub for the switch command (`lens.core/_bmad/lens-work/prompts/lens-switch.prompt.md`) must enforce the mandatory preflight gate before any other logic runs. This parity requirement comes from the baseline 17-command surface: every retained command's stub runs `light-preflight.py` first, stops on non-zero exit, and only then loads the release-module prompt.

**Requirement:** SW-B1

## Task

Inspect `lens-switch.prompt.md` in the target repo (`TargetProjects/lens-dev/new-codebase/lens.core.src`). Verify or update it so that:

1. `uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py` is the first executable step.
2. Non-zero exit stops the command and surfaces the failure message.
3. Only after successful preflight does the stub load the release-module prompt at `lens.core/_bmad/lens-work/prompts/lens-switch.prompt.md`.
4. The release prompt does not search alternate paths or load skills before receiving control from the stub.

## Files

- `lens.core/_bmad/lens-work/prompts/lens-switch.prompt.md` (stub — inspect and update if needed)
- `lens.core/_bmad/lens-work/scripts/light-preflight.py` (preflight script — read-only reference)

## Acceptance Criteria

- [ ] Stub file contains `uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py` as the first executable step.
- [ ] Stub stops and surfaces failure when preflight exits non-zero.
- [ ] Stub loads release prompt only after successful preflight.
- [ ] Release prompt does not read skills or search alternate paths before receiving control.
- [ ] Smoke test confirms preflight → release prompt handoff flow.

## Dev Notes

- Compare against `lens-init.prompt.md` or another retained command stub as a reference pattern.
- Do not modify `light-preflight.py`.
- If the stub is already correct, document that as the acceptance outcome and close the story.

## Status

review

## Dev Agent Record

### Debug Log

- 2026-04-27: Implemented `SW-1` under target repo `TargetProjects/lens-dev/new-codebase/lens.core.src` on branch `feature/switch-dev`.
- 2026-04-27: Verified with `uv run --with pytest _bmad/lens-work/skills/bmad-lens-switch/scripts/tests/test-switch-ops.py -q`.
- 2026-04-27: Verified with `uv run --with pytest --with pyyaml python -m pytest _bmad/lens-work -q`.

### Completion Notes

- Verified prompt-start preflight handoff via focused regression; no light-preflight.py change required.
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
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-switch/stories/SW-1.md`
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-switch/sprint-status.yaml`

### Change Log

- 2026-04-27: Implemented, tested, and moved to review.

