# Story E3-S1: Implement pre-confirmed handoff in SKILL.md

**Feature:** lens-dev-new-codebase-next
**Epic:** Epic 3 — Delegation and Release Hardening
**Status:** ready-for-dev

---

## Story

As a Lens user,
I want the `next` command to immediately load the recommended phase skill when the
recommendation is unblocked, without surfacing a second run-confirmation prompt,
so that `/next` is a seamless handoff conductor and not a two-step confirmation loop.

## Acceptance Criteria

1. `bmad-lens-next/SKILL.md` invokes the target skill via
   `bmad-lens-bmad-skill --skill {recommended_skill}` when `status=unblocked`
2. No second "Proceed?" or "Run [phase]?" confirmation prompt appears
3. Handoff passes the resolved `feature_id` and any relevant context state as arguments
4. Blocked recommendations do not invoke any downstream skill
5. Behavior is verified against E1-S3 acceptance criteria (no inline routing, no writes)

## Tasks / Subtasks

- [ ] Confirm E1-S3 (SKILL.md shell) is complete
- [ ] Confirm E2-S1 (`next-ops.py`) is complete and deterministic
- [ ] Implement the `status=unblocked` → `bmad-lens-bmad-skill` delegation path (AC #1)
  - [ ] Strip the leading `/` from `recommendation` to get skill name, e.g. `/finalizeplan` → `bmad-lens-finalizeplan`
  - [ ] Pass `feature_id` in the delegation call (AC #3)
- [ ] Audit SKILL.md: confirm no confirmation prompt in the `status=unblocked` path (AC #2)
- [ ] Confirm `status=blocked` path stops without delegating (AC #4)
- [ ] Re-audit no-write and no-inline-routing properties (AC #5)
- [ ] Commit updated SKILL.md to `lens.core.src` develop branch

## Dev Notes

- **Delegation call pattern:** `bmad-lens-bmad-skill --skill bmad-lens-{phase}` where
  `{phase}` is derived from `recommendation` field (drop the `/` prefix)
- **No confirmation prompt:** The `next` command is a conductor — it does not own the phase
  execution. The target skill's own activation is the point where user intent is confirmed.
  Adding a second confirmation here is the anti-pattern to avoid.
- **Context handoff:** Pass at minimum `--feature-id {feature_id}` so the target skill
  does not need to re-resolve feature context.

### References
- [story-E1-S3.md — SKILL.md shell (prerequisite)](./story-E1-S3.md)
- [story-E2-S1.md — next-ops.py (prerequisite)](./story-E2-S1.md)
- [tech-plan.md — §3 Delegation contract, pre-confirmed handoff](../tech-plan.md)
- [business-plan.md — prompt-start chain and seamless handoff requirement](../business-plan.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List
