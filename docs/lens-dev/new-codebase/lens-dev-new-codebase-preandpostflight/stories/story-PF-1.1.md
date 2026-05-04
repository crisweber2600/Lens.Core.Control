# Story PF-1.1: Reconcile live preflight surface and preserve prompt-start contract

**Feature:** lens-dev-new-codebase-preandpostflight
**Epic:** Epic 1 - Cadence Split and Branch-Sensitive Refresh
**Status:** ready-for-dev
**Story Points:** 2

---

## Story

As a Lens maintainer,
I want the live preflight implementation surface in `lens.core.src` reconciled with the
planning packet while keeping `light-preflight.py` as the frozen prompt-start gate,
so that the cadence redesign starts from the real target repo and does not break published
commands.

## Acceptance Criteria

1. The implementation owner is confirmed as `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/`, and story notes explicitly call out the path drift from the older `skills/lens-preflight` wording in predecessor docs.
2. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/scripts/light-preflight.py` continues to return `0` to proceed and non-zero to halt, and remains limited to cheap root, Python, and required-path validation.
3. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/SKILL.md` documents the same invocation path and supported arguments that the script actually accepts.
4. The full request-lifecycle entrypoint file under `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/scripts/` is named and reserved before cadence work begins; prompt stubs continue to call only `light-preflight.py`.
5. No control or governance repo mutation is added directly to the prompt-start gate.

## Tasks / Subtasks

- [ ] Confirm the live target-repo preflight surface and record any path drift from the older predecessor wording.
- [ ] Verify `light-preflight.py` still enforces the cheap halt-or-proceed contract only.
- [ ] Align `bmad-lens-preflight/SKILL.md` to the real script arguments and invocation path.
- [ ] Reserve the full request-lifecycle entrypoint filename under `bmad-lens-preflight/scripts/` without routing prompt stubs to it yet.
- [ ] Record any drift or naming decision in Completion Notes for PF-1.2 to consume.

## Dev Notes

### Implementation Notes

- The live target surface is `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/`, not the older `skills/lens-preflight` wording used in the first planning packet.
- `light-preflight.py` is the frozen public gate and must stay cheap: root discovery, Python gating, and required-path validation only.
- Do not hide any mutable control-repo or governance-repo behavior behind the prompt-start gate.
- PF-1.2 depends on a concrete reserved filename for the heavier lifecycle entrypoint.

### References

- [epics.md](../epics.md)
- [stories.md](../stories.md)
- [implementation-readiness.md](../implementation-readiness.md)
- [tech-plan.md](../tech-plan.md)
- [finalizeplan-review.md](../finalizeplan-review.md)
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/SKILL.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/scripts/light-preflight.py`

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List