---
feature: lens-dev-new-codebase-dogfood
epic: 2
story_id: E2-S6
sprint_story_id: S2.6
title: Add express publish artifact mapping
type: fix
points: M
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E1-S3, E2-S1]
blocks: [E5-S3]
target_repo: lens.core.src
target_branch: develop
---

# E2-S6 — Add express publish artifact mapping

## Context

Express publish currently copies only the review artifact by default (Defect 4). The full QuickPlan artifact set (business-plan, tech-plan, sprint-plan, review) must be published without requiring an explicit operator override. Additionally, both the current filename (`expressplan-adversarial-review.md`) and legacy filename (`expressplan-review.md`) must be recognized. Windows path normalization must also be applied (Defect 3).

**Additional AC from M3 / ADR-5:** Output must report which express review filename was matched.

## Implementation Steps

1. Update `publish-to-governance --phase expressplan` to copy: `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, and the express review artifact.
2. Add filename resolution: try `expressplan-adversarial-review.md` first, then fall back to `expressplan-review.md`; report the matched name in output.
3. Apply Windows-safe path normalization before all publish file operations (Defect 3).
4. Write focused tests: full artifact set, legacy filename fallback, and Windows path normalization.

## Acceptance Criteria

- [ ] `publish-to-governance --phase expressplan` copies all four artifact types.
- [ ] Both `expressplan-adversarial-review.md` (current) and `expressplan-review.md` (legacy) are recognized; matched filename is reported in output.
- [ ] Express publish does not require explicit operator override for full artifact set.
- [ ] Windows path normalization applied before all publish operations.
- [ ] Focused tests cover full artifact set, legacy fallback, and Windows normalization.
- [ ] **M3/ADR-5 AC:** Output includes which review filename was matched.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill) — publish-to-governance is a `lens-work` operation.

Load the BMB documentation index at `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md` before authoring.

## Dev Notes

- Reference: tech-plan Defects 3 and 4, ADR-5, sprint-plan S2.6 Additional AC.
- The legacy fallback must be explicit and tested — do not silently use whichever file exists.
- ADR-5 accepted tech debt: both filenames are recognized; the current canonical is `expressplan-adversarial-review.md`.

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
