---
feature: lens-dev-new-codebase-expressplan
doc_type: story-file
story_id: 1.2
status: ready-for-dev
goal: "Block unsupported feature state before the express path begins"
depends_on:
  - 1.1
updated_at: 2026-04-27T22:50:00Z
---

# Story 1.2 - Express-Track Gating and State Checks

## User Story

As a governance owner, I want expressplan to reject unsupported track and phase combinations so the compressed path never becomes a hidden lifecycle bypass.

## Acceptance Criteria

- The skill resolves feature context before any QuickPlan delegation occurs.
- Unsupported track or phase combinations return a blocking message with the correct next step.
- The command does not silently convert a feature from full track to express track.
- The message distinguishes between user error and command failure.

## Implementation Notes

- Prefer feature-state reads over inferred branch guesses.
- Keep any future track conversion outside expressplan itself.
- Align failure messaging with the retained lifecycle model.

## Validation Targets

- Negative-path tests for unsupported tracks
- No implicit phase or track mutation
- Clear operator guidance on failure