# Story PF-1.1: Reconcile live preflight surface and preserve prompt-start contract

**Feature:** lens-dev-new-codebase-preandpostflight
**Epic:** Epic 1 - Cadence Split and Branch-Sensitive Refresh
**Status:** done
**Story Points:** 2

---

## Story

As a Lens maintainer,
I want the live preflight implementation surface in `lens.core.src` reconciled with the
planning packet while keeping `light-preflight.py` as the frozen prompt-start gate,
so that the cadence redesign starts from the real target repo and does not break published
commands.

## Context

The live target repo exposes the frozen prompt-start gate at
`TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/scripts/light-preflight.py`
plus its owning `SKILL.md` and full lifecycle entrypoint `scripts/preflight.py`. Earlier
planning prose referred to `bmad-lens-*`, so the first implementation slice had to
reconcile that path drift while keeping the prompt-start gate frozen.

## Acceptance Criteria

1. The implementation owner is confirmed as `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/`, and story notes explicitly call out the path drift from the older `bmad-lens-*` wording in predecessor docs.
2. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/scripts/light-preflight.py` continues to return `0` to proceed and non-zero to halt, and remains limited to cheap root, Python, and required-path validation.
3. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/SKILL.md` documents the same invocation path and supported arguments that the script actually accepts.
4. The full request-lifecycle entrypoint file under `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/scripts/` remains `preflight.py`; prompt stubs continue to call only `light-preflight.py`.
5. No control or governance repo mutation is added directly to the prompt-start gate.

## Implementation Steps

1. Confirm the live preflight source of truth under
	`TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/`
	and record any path drift from earlier planning references.
2. Verify `scripts/light-preflight.py` still implements only the cheap halt-or-proceed
	contract and keep its behavior frozen.
3. Align `skills/lens-preflight/SKILL.md` so its documented invocation path and arguments match
	the live script behavior.
4. Reserve the filename for the heavier request-lifecycle entrypoint under
	`skills/lens-preflight/scripts/` without routing prompt stubs to it yet.

## Tasks / Subtasks

- [x] Confirm the live target-repo preflight surface and record any path drift from the older predecessor wording.
- [x] Verify `light-preflight.py` still enforces the cheap halt-or-proceed contract only.
- [x] Align `skills/lens-preflight/SKILL.md` to the real script arguments and invocation path.
- [x] Reserve the full request-lifecycle entrypoint filename under `skills/lens-preflight/scripts/` without routing prompt stubs to it yet.
- [x] Record any drift or naming decision in Completion Notes for PF-1.2 to consume.

## Dev Notes

### Implementation Notes

- The live target surface is `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/`, not the older `bmad-lens-*` wording used in the first planning packet.
- `light-preflight.py` is the frozen public gate and must stay cheap: root discovery, Python gating, and required-path validation only.
- Do not hide any mutable control-repo or governance-repo behavior behind the prompt-start gate.
- PF-1.2 depends on the reserved heavier lifecycle entrypoint remaining `preflight.py`.

### References

- [epics.md](../epics.md)
- [stories.md](../stories.md)
- [implementation-readiness.md](../implementation-readiness.md)
- [tech-plan.md](../tech-plan.md)
- [finalizeplan-review.md](../finalizeplan-review.md)
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/SKILL.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/scripts/light-preflight.py`

## Dev Agent Record

### Agent Model Used

GPT-5.4

### Debug Log References

### Completion Notes List

- Confirmed the live target surface is `skills/lens-preflight`, not the older `bmad-lens-*` path family described in the original packet.
- Preserved `light-preflight.py` as the cheap prompt-start gate while documenting and retaining `preflight.py` as the full request-lifecycle entrypoint.

### File List

- TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/SKILL.md
- TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/scripts/light-preflight.py
- TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/scripts/preflight.py