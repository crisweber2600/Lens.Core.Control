---
feature: lens-dev-new-codebase-switch
story_id: SW-7
epic: EP-2
sprint: 2
title: Persist Local Context Only
estimate: S
status: review
blocked_by: []
assignee: crisweber2600
doc_type: story
updated_at: 2026-04-27T18:00:00Z
---

# SW-7 — Persist Local Context Only

## Context

Switch must write `.lens/personal/context.yaml` with the active domain, service, timestamp, and update source. This is the only write side effect on switch success. Governance files must remain unchanged.

**Requirement:** SW-B6

## Task

In `switch-ops.py`, after a successful switch:

1. Resolve personal folder as `{control_repo}/.lens/personal` when a control repo is supplied; fallback to `{workspace_root}/.lens/personal`.
2. Create the directory if it does not exist.
3. Write `.lens/personal/context.yaml` with:
   - `domain: {domain}`
   - `service: {service}`
   - `updated_at: {ISO timestamp}`
   - `updated_by: lens-switch`
4. The write is idempotent; re-running switch for the same feature overwrites without error.
5. Do not modify any file under `TargetProjects/lens/lens-governance/`, `lens.core/_bmad/`, or the target repo.

## Files

- `lens.core/_bmad/lens-work/skills/bmad-lens-switch/scripts/switch-ops.py`
- `.lens/personal/context.yaml` (written by switch)

## Acceptance Criteria

- [ ] After switch, `.lens/personal/context.yaml` contains `domain`, `service`, `updated_at`, `updated_by: lens-switch`.
- [ ] No file in the governance repo is modified.
- [ ] No file in `lens.core/_bmad/` is modified.
- [ ] Re-running switch overwrites context file without error.
- [ ] Governance no-write regression test passes.

## Dev Notes

- Use `yaml.safe_dump` or equivalent; do not write raw string concatenation.
- The `updated_at` field must be a valid ISO 8601 timestamp.
- The personal folder path should be documented in the switch script docstring so it is discoverable.

## Status

review

## Dev Agent Record

### Debug Log

- 2026-04-27: Implemented `SW-7` under target repo `TargetProjects/lens-dev/new-codebase/lens.core.src` on branch `feature/switch-dev`.
- 2026-04-27: Verified with `uv run --with pytest _bmad/lens-work/skills/bmad-lens-switch/scripts/tests/test-switch-ops.py -q`.
- 2026-04-27: Verified with `uv run --with pytest --with pyyaml python -m pytest _bmad/lens-work -q`.

### Completion Notes

- Persisted local-only .lens/personal/context.yaml and added governance no-write regression.
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
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-switch/stories/SW-7.md`
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-switch/sprint-status.yaml`

### Change Log

- 2026-04-27: Implemented, tested, and moved to review.

