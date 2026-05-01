---
feature: lens-dev-new-codebase-dogfood
epic: 2
story_id: E2-S5
sprint_story_id: S2.5
title: Add branch cleanup and branch-switch discipline
type: fix
points: M
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E2-S1]
blocks: [E3-S1]
target_repo: lens.core.src
target_branch: develop
---

# E2-S5 — Add branch cleanup and branch-switch discipline

## Context

After a PR merges, stale local and remote branches accumulate. Without branch cleanup and a disciplined switch-and-pull sequence, the workspace drifts and subsequent operations may start from an outdated or wrong base.

## Implementation Steps

1. After each PR merge in `bmad-lens-git-orchestration`, delete the local and remote branches for that feature step.
2. After cleanup, switch to the correct next branch and pull before continuing.
3. Enforce: branch-switch includes a pull; working directory must be clean before any write.
4. Make cleanup idempotent: running twice should not fail.
5. Write focused tests: cleanup, switch, pull, and idempotent re-run.

## Acceptance Criteria

- [ ] After PR merge, local and remote branches for that step are deleted.
- [ ] Workflow switches to correct next branch and pulls before continuing.
- [ ] Branch-switch includes pull; working directory is clean before any write.
- [ ] Cleanup is idempotent.
- [ ] Focused tests cover cleanup, switch, and idempotent re-run.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill).

## Dev Notes

- Reference: tech-plan Git Orchestration Contract section.
- "Correct next branch" must be determined by the phase contract, not guessed.

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
