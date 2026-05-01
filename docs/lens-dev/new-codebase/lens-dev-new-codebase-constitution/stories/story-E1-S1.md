# Story E1-S1: Verify and lock express-track feature state alignment

**Feature:** lens-dev-new-codebase-constitution
**Epic:** Epic 1 - Express Alignment and Command Surface
**Status:** ready-for-dev

---

## Story

As a Lens maintainer,
I want the sanctioned `bmad-lens-feature-yaml` transition to remain the authoritative record
of this feature's express state
so that the planning packet and runtime automation do not disagree about which lifecycle
path this feature is using.

## Acceptance Criteria

1. Governance `feature.yaml` confirms `track: express` and `phase: expressplan-complete`
2. FinalizePlan bundle documents the express packet as the predecessor set instead of the
   superseded full-path assumption
3. Any remaining lifecycle assumptions are captured in notes rather than hidden in code or
   docs drift
4. No manual governance mutation path is introduced; sanctioned feature-yaml operations
   remain the only state-changing route

## Tasks / Subtasks

- [ ] Read the current governance `feature.yaml` and confirm the stored track/phase values
- [ ] Cross-check `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, and
      `finalizeplan-review.md` for explicit express-path language
- [ ] Record any mismatch between planning docs and governance state in Completion Notes
- [ ] Confirm no story or note instructs manual governance file edits outside the sanctioned
      feature-yaml script

## Dev Notes

### Implementation Notes

- This is a governance-alignment story, not a feature behavior rewrite.
- The expected current state is `track: express` and `phase: expressplan-complete` in
  `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-constitution/feature.yaml`.
- If a drift is found, document it and route through the sanctioned feature-yaml tool; do not
  patch governance state directly from this story.
- The main purpose of this story is to make later implementation stories safe: resolver,
  compliance, and display work should all assume the express path is intentional.

### References

- [finalizeplan-review.md](../finalizeplan-review.md)
- [business-plan.md](../business-plan.md)
- [tech-plan.md](../tech-plan.md)
- [sprint-plan.md](../sprint-plan.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List