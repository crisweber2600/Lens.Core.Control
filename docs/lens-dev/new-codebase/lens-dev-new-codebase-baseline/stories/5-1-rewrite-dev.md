---
feature: lens-dev-new-codebase-baseline
story_id: "5.1"
story_key: "5-1-rewrite-dev"
epic: "5"
title: "Rewrite dev Command"
status: ready-for-dev
priority: must
story_points: 8
depends_on: [4-4-rewrite-finalizeplan]
updated_at: 2026-04-22T00:00:00Z
---

# Story 5.1: Rewrite dev Command

Status: ready-for-dev

## Story

As a Lens user,
I want dev to keep target-repo-only writes, per-task commits, resumable checkpoints, and a final PR,
so that implementation work survives the rewrite without losing in-progress state.

## Acceptance Criteria

1. All code writes stay in the target repo, never the control or release repo.
2. Each completed task produces a per-task commit.
3. dev-session.yaml resumes interrupted work without schema changes and ends with a final PR.

## Tasks / Subtasks

- [ ] Preserve repo-scoped branch preparation and target-repo-only writes.
  - [ ] Keep control repo artifacts read-only during implementation.
- [ ] Preserve per-task commit and checkpoint semantics.
  - [ ] Keep dev-session.yaml backward-compatible.
- [ ] Preserve final PR behavior.
  - [ ] Resume interrupted runs without recreating work state.

## Dev Notes

- This story depends on the finalizeplan bundle being present and discoverable.
- Preserve the discovery patterns for story files in stories/ and legacy shapes.
- Do not repurpose control-repo docs for code changes.

### References

- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/epics.md
- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/sprint-status.yaml
- lens.core/_bmad/lens-work/skills/bmad-lens-dev/SKILL.md

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
