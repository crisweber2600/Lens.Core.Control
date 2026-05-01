---
feature: lens-dev-new-codebase-dogfood
epic: 4
story_id: E4-S1
sprint_story_id: S4.1
title: Implement Dev phase conductor
type: new
points: XL
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E3-S3, E3-S4]
blocks: [E4-S2, E4-S3, E5-S4]
target_repo: lens.core.src
target_branch: develop
---

# E4-S1 — Implement Dev phase conductor

## Context

The Dev phase conductor (`bmad-lens-dev`) is the primary orchestration point for story execution: it loads story files, manages target-repo branch prep, records agent output to dev-session.yaml, and drives story-by-story iteration. The `bmad-lens-dev` SKILL.md stub was created in E3-S3; this story provides the full implementation.

## Implementation Steps

1. Implement `bmad-lens-dev` conductor full lifecycle:
   a. Phase entry: validate feature.yaml phase == `finalizeplan-complete`.
   b. Load sprint-status.yaml, identify next ready story.
   c. Load story file; validate required sections (Context, Implementation Steps, Acceptance Criteria, Dev Agent Record).
   d. Prepare target-repo branch (delegate to E4-S3 logic).
   e. Present story to dev agent, record output to dev-session.yaml (story-scoped).
   f. Mark story complete in sprint-status.yaml; commit and push control repo updates.
   g. Iterate to next ready story or pause at sprint boundary.
2. Conductor must validate that all E4 story files include the Governance Coordination Note before executing them.
3. Write integration test: mock story, mock dev agent, confirm dev-session.yaml populated and sprint-status updated.

## Acceptance Criteria

- [ ] Conductor validates feature.yaml phase == `finalizeplan-complete` at entry.
- [ ] Loads sprint-status.yaml and identifies next ready story.
- [ ] Validates story file sections before execution.
- [ ] Target-repo branch prep delegated correctly.
- [ ] Dev agent output recorded in story-scoped dev-session.yaml.
- [ ] Sprint-status updated and pushed after each story.
- [ ] Sprint boundary pause works.
- [ ] Integration test with mock story and dev agent passes.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill) — full implementation of `bmad-lens-dev` SKILL.md.

Load the BMB documentation index at `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md` before authoring.

## Governance Coordination Note

This story reproduces reference behavior for clean-room parity testing. First-class Dev command authoring belongs to `lens-dev-new-codebase-dev`; any overlap with that feature's eventual PRD must be surfaced as explicit acceptance criteria alignment before implementation proceeds.

## Dev Notes

- Reference: tech-plan Dev Phase Contract section, lifecycle.yaml v4 (E1-S2), bmad-lens-dev SKILL.md stub (E3-S3).
- dev-session.yaml is story-scoped: one file per story execution, named `{storyId}-dev-session.yaml`.

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
