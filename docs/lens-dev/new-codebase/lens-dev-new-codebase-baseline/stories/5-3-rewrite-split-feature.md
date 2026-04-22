---
feature: lens-dev-new-codebase-baseline
story_id: "5.3"
story_key: "5-3-rewrite-split-feature"
epic: "5"
title: "Rewrite split-feature Command"
status: re-evaluate
priority: must
story_points: 8
depends_on: [4-4-rewrite-finalizeplan]
updated_at: 2026-04-22T00:00:00Z
---

# Story 5.3: Rewrite split-feature Command

Status: re-evaluate

## Story

As a Lens user,
I want split-feature to validate first, block in-progress stories, create the new feature before modifying the source, and move eligible stories safely,
so that split operations never leave governance in a broken partial state.

## Acceptance Criteria

1. validate-split runs before any governance mutation.
2. In-progress stories hard-stop split execution.
3. create-split-feature creates the child feature before source modification, and move-stories preserves file integrity.

## Tasks / Subtasks

- [ ] Preserve validate-first execution with dry-run capability.
  - [ ] Keep the validate-split subcommand surface intact.
- [ ] Preserve in-progress blocking and story-file checks.
  - [ ] Read sprint-status and story-file status consistently.
- [ ] Preserve create-then-move ordering.
  - [ ] Keep moved stories valid in the target feature stories/ directory.

## Dev Notes

- Story files are a compatibility boundary for this command.
- Preserve both markdown and YAML story-file handling.
- Use the existing split-feature tests and references as the primary contract.

### References

- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/epics.md
- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/sprint-status.yaml
- lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/references/split-stories.md

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
