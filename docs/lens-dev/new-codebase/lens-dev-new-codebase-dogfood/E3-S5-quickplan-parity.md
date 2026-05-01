---
feature: lens-dev-new-codebase-dogfood
epic: 3
story_id: E3-S5
sprint_story_id: S3.5
title: Resolve QuickPlan public vs internal surface decision
type: new
points: M
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E1-S3, E3-S2]
blocks: [E5-S2]
target_repo: lens.core.src
target_branch: develop
---

# E3-S5 — Resolve QuickPlan public vs internal surface decision

## Context

QuickPlan is registered in `module.yaml` but its surface classification — public command vs. internal orchestration step — is ambiguous. The classification affects whether it appears in the user-facing command list, what input/output contract it exposes, and how it is tested in E5-S2 command-trace validation.

## Implementation Steps

1. Review the business plan, tech-plan, and parity map to determine the intended classification for QuickPlan.
2. Make the classification decision explicit in `module.yaml` (public or internal).
3. Update the SKILL.md stub created in E3-S3 to match the decision.
4. If public: add to the public help surface and expose standard input/output contract.
5. If internal: mark as internal in `module.yaml` and exclude from public help output.
6. Write a test confirming `module.yaml` classification is consistent with SKILL.md declaration.

## Acceptance Criteria

- [ ] QuickPlan classification (public or internal) is explicitly recorded in `module.yaml`.
- [ ] SKILL.md matches the classification decision.
- [ ] Public help output includes QuickPlan if and only if classified as public.
- [ ] Test: `module.yaml` classification consistent with SKILL.md → passes.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill).

## Dev Notes

- Reference: tech-plan parity map command surface section, business-plan quickplan section.
- If ambiguity persists after reviewing the business and tech plans, default to "internal" and document the assumption.

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
