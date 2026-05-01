---
feature: lens-dev-new-codebase-dogfood
epic: 4
story_id: E4-S4
sprint_story_id: S4.4
title: Fix Complete closeout — retrospective-first discipline
type: fix
points: L
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E1-S4, E4-S1]
blocks: [E5-S5]
target_repo: lens.core.src
target_branch: develop
---

# E4-S4 — Fix Complete closeout: retrospective-first discipline

## Context

The `bmad-lens-complete` closeout conductor allows the feature to be marked complete without a retrospective artifact existing. The correct discipline requires that a retrospective be created and reviewed before `feature.yaml` phase is updated to `complete`. This is BF-7 (complete closeout ordering).

## Implementation Steps

1. Add pre-condition gate to `bmad-lens-complete`: verify that `retrospective.md` exists in the feature docs folder.
2. If retrospective is absent: surface a structured error directing the user to `bmad-lens-retrospective`.
3. Verify retrospective `status` field is `approved` (or equivalent) before proceeding.
4. After gate passes, proceed with closeout: update feature.yaml, sync feature-index, clean branches.
5. Write focused tests: missing retrospective → error, present but not approved → error, approved → closeout proceeds.

## Acceptance Criteria

- [ ] `retrospective.md` must exist before `bmad-lens-complete` proceeds.
- [ ] Absent retrospective raises structured error pointing to `bmad-lens-retrospective`.
- [ ] Retrospective `status` must be `approved` or closeout is blocked.
- [ ] After gates pass: feature.yaml updated, feature-index synced, branches cleaned.
- [ ] Focused tests: all three gate states.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill).

Load the BMB documentation index at `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md` before authoring.

## Governance Coordination Note

This story reproduces reference behavior for clean-room parity testing. First-class Dev command authoring belongs to `lens-dev-new-codebase-dev`; any overlap with that feature's eventual PRD must be surfaced as explicit acceptance criteria alignment before implementation proceeds.

## Dev Notes

- Reference: tech-plan, BF-7, lifecycle.yaml v4 Complete phase (E1-S2).

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
