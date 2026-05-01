# Story E1-S2: Verify the 3-hop prompt chain stays thin

**Feature:** lens-dev-new-codebase-constitution
**Epic:** Epic 1 - Express Alignment and Command Surface
**Status:** ready-for-dev

---

## Story

As a Lens maintainer,
I want the public prompt, release prompt, and SKILL.md surfaces to stay thin
so that resolver behavior remains centralized in `constitution-ops.py` and future drift is
easy to detect.

## Acceptance Criteria

1. `.github/prompts/lens-constitution.prompt.md` preserves the preflight-first entry pattern
2. `lens.core/_bmad/lens-work/prompts/lens-constitution.prompt.md` remains a redirect to the
   skill surface
3. `bmad-lens-constitution/SKILL.md` delegates logic to `constitution-ops.py` rather than
   embedding merge behavior inline
4. Any prompt-chain drift is documented before implementation starts

## Tasks / Subtasks

- [ ] Inspect the public stub for the required preflight sequence
- [ ] Inspect the release prompt for a pure redirect contract
- [ ] Inspect the skill for inline logic drift versus script delegation
- [ ] Record any required cleanup in Completion Notes before marking done

## Dev Notes

### Implementation Notes

- The chain to verify is: public stub -> release prompt -> SKILL.md -> `constitution-ops.py`.
- This story does not change the resolver behavior. It prevents future behavior from being
  duplicated across prompt layers.
- If prompt or skill surfaces drift, fix the thin-surface issue before any new resolver logic
  is added.

### References

- [tech-plan.md](../tech-plan.md)
- [architecture.md](../architecture.md)
- [epics.md](../epics.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List