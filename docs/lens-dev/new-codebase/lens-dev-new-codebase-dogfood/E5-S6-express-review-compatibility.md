---
feature: lens-dev-new-codebase-dogfood
epic: 5
story_id: E5-S6
sprint_story_id: S5.6
title: Confirm express review filename compatibility mapping
type: confirm
points: S
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E2-S6, E5-S5]
blocks: []
target_repo: lens.core.src
target_branch: develop
---

# E5-S6 — Confirm express review filename compatibility mapping

## Context

ADR-5 (M3 accepted tech debt) records that both `expressplan-adversarial-review.md` (current canonical) and `expressplan-review.md` (legacy) are recognized by the express publish operation. This confirmation story verifies that the ADR-5 mapping is implemented, tested, and that the compatibility behavior is correctly documented.

## Implementation Steps

1. Confirm the E2-S6 implementation: both filenames are recognized; matched filename is reported.
2. Run the focused tests from E2-S6 to confirm they still pass.
3. Update `docs/lens-dev/new-codebase/lens-dev-new-codebase-dogfood/parity-report.md` with a section confirming ADR-5 status.
4. Confirm ADR-5 entry in tech-plan (or a companion ADR doc) accurately describes the mapping and scope.

## Acceptance Criteria

- [ ] Both `expressplan-adversarial-review.md` and `expressplan-review.md` are recognized by publish.
- [ ] Matched filename reported in publish output.
- [ ] E2-S6 focused tests still pass.
- [ ] Parity report updated with ADR-5 confirmation section.
- [ ] ADR-5 documentation accurate and complete.

## Dev Notes

- Reference: tech-plan ADR-5, E2-S6 implementation, M3 finalizeplan-review response.
- This story is a confirmation and documentation close-out, not new implementation.
- If E2-S6 tests fail: escalate as a blocking defect before closing this story.

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
