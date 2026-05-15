---
feature: nextlens-src-dogfoodnext
story_id: NLB-6
doc_type: story
status: ready-for-dev
title: Fresh Branch Delegation And Boundary Enforcement
depends_on: [NLB-3, NLB-4, NLB-5]
implementation_kind: conductor
epic: 3
updated_at: 2026-05-15T20:00:00Z
---

# NLB-6 - Fresh Branch Delegation And Boundary Enforcement

## Goal

Mirror `/lens-core-bugfix` delegation discipline for NextLens: fresh branch, bounded implementation handoff, real commit required, and no out-of-scope writes.

## Scope

- Derive a branch such as `feature/nextlens-bugfix-{bug_slug}` from the stable bug slug.
- Prepare the target branch through approved git orchestration.
- Delegate implementation only after the allowed target root resolves to `TargetProjects/nextlens/src/NextLens`.
- Block no-op completion, branch reuse for another bug, dirty target worktrees, missing implementation commits, and proposed edits outside the target root.

## Acceptance Criteria

- Given a valid bug slug, when branch preparation runs, then a fresh branch is created from the configured base branch.
- Given branch reuse would attach a different bug, when preparation runs, then the workflow stops with a branch-scope error.
- Given proposed edits resolve outside `TargetProjects/nextlens/src/NextLens`, when delegation is prepared, then mutation is blocked.
- Given the implementation delegate returns without a target commit, when the conductor resumes, then success is blocked.

## Validation

- Add branch identity tests, no-op completion tests, target boundary tests, and dirty-worktree or branch-reuse fixtures.

## Dev Notes

- The conductor owns branch preparation and completion gates. The implementation delegate does not own push, PR, bug closeout, or final success reporting.