# Story PF-1.2: Add layered request-lifecycle entrypoint and cadence state

**Feature:** lens-dev-new-codebase-preandpostflight
**Epic:** Epic 1 - Cadence Split and Branch-Sensitive Refresh
**Status:** done
**Story Points:** 5

---

## Story

As a Lens maintainer,
I want a full request-lifecycle entrypoint under `skills/lens-preflight/scripts/` that
separates every-request gates from cadence-owned work,
so that prompt-start behavior stays cheap and the heavier lifecycle becomes explicit.

## Context

PF-1.1 establishes the live target surface and reserves the heavier entrypoint filename.
This story introduces that entrypoint in the target repo and splits request-time work into
explicit layers while keeping the published prompt-start gate unchanged.

## Acceptance Criteria

1. A full lifecycle script is added under `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/scripts/` plus supporting helper modules for cadence state as needed.
2. The implementation separates four internal layers: every-request gates, branch-sensitive release refresh, daily hygiene, and weekly hygiene.
3. Timestamp freshness can suppress only cadence-owned layers; it never suppresses root, Python, `LENS_VERSION`, or required repo and path checks.
4. The lifecycle entrypoint records which layers ran and why in the existing preflight log stream.
5. When the lifecycle entrypoint is unavailable or fails before mutable work begins, the prompt-start gate still halts cleanly with a diagnosable error.
6. No new request-status UX surface is introduced.

## Implementation Steps

1. Add the reserved full request-lifecycle script under
	`TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/scripts/`.
2. Add any helper modules needed for cadence-state resolution and layer execution.
3. Separate the lifecycle flow into every-request gates, branch-sensitive refresh, daily
	hygiene, and weekly hygiene.
4. Ensure timestamp freshness gates only cadence-owned work and never suppresses the frozen
	prompt-start validations.
5. Emit layer decisions through the existing preflight log stream and preserve clean failure
	behavior when the full lifecycle path fails early.

## Tasks / Subtasks

- [ ] Implement the reserved full lifecycle entrypoint under `skills/lens-preflight/scripts/`.
- [ ] Add any helper modules needed for cadence-state calculation without widening the prompt-start gate.
- [ ] Split execution into every-request, branch-sensitive refresh, daily hygiene, and weekly hygiene layers.
- [ ] Ensure timestamp freshness only gates cadence-owned layers.
- [ ] Preserve the existing preflight log stream and emit enough detail for later PF-3.1 assertions.
- [ ] Prove the prompt-start gate still fails cleanly if the full lifecycle path is missing or errors early.

## Dev Notes

### Implementation Notes

- Keep prompt stubs and the light gate thin; all heavier orchestration belongs under `skills/lens-preflight/scripts/`.
- This story establishes the runtime layering that PF-1.3, PF-2.1, and PF-2.2 build on.
- The lifecycle entrypoint must remain compatible with Windows-safe path handling and repo-root detection.
- Logging stays on the existing preflight stream; do not invent a second status channel.

### References

- [epics.md](../epics.md)
- [stories.md](../stories.md)
- [implementation-readiness.md](../implementation-readiness.md)
- [tech-plan.md](../tech-plan.md)
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/SKILL.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/scripts/light-preflight.py`

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List