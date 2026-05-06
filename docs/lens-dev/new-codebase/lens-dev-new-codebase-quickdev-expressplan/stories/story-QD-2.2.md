---
feature: lens-dev-new-codebase-quickdev-expressplan
story_id: "QD-2.2"
doc_type: story
status: done
type: new
title: "Apply Branch Policy and PR Orchestration"
priority: P1
story_points: 2
epic: "Epic 2 - Scoped Implementation Execution and Branch Control"
depends_on: ["QD-2.1"]
blocks: ["QD-2.3"]
updated_at: 2026-05-06T21:20:00Z
---

# Story QD-2.2 - Apply Branch Policy and PR Orchestration

**Feature:** `lens-dev-new-codebase-quickdev-expressplan`
**Epic:** `Epic 2 - Scoped Implementation Execution and Branch Control`
**Priority:** P1 | **Points:** 2 | **Status:** done

## Goal

Apply the agreed branch and PR policy automatically so quickdev changes land in the right place without ad hoc git decisions.

## Context

User decisions in planning established conditional behavior: commit directly to an active in-progress feature branch, otherwise use the standard Lens git flow to create a working branch and PR.

## Implementation Steps

1. Detect whether the target repo is already on an active in-progress feature branch for the current quickdev run.
2. If active, use that branch for implementation and direct commit behavior.
3. If not active, invoke Lens git orchestration to prepare a working branch from the configured base.
4. Create or reuse a PR through Lens git orchestration when the run uses a working branch.
5. Record dirty, detached, or ambiguous branch states as blockers before implementation.

## Acceptance Criteria

- [x] Active in-progress feature branches receive direct commits.
- [x] Runs without an active feature branch prepare a working branch and PR through Lens git orchestration.
- [x] Dirty or ambiguous branch states block before implementation.
- [x] Branch, base, and PR URL are recorded for later evidence updates.

## Governance Coordination Note

Branch and PR orchestration must use sanctioned Lens git helpers. Do not hand-roll PR creation when the helper supports the required operation.

## Dev Agent Record

### Agent Model Used

GitHub Copilot

### Debug Log References

- `uv run --with pytest --with pyyaml python -m pytest _bmad/lens-work/scripts/tests/test-quickdev-conductor-contract.py` - 17 passed.
- `git diff --check` - passed with no blocking output.
- Target repo commit: `c41b0acf` on `feature/quickdev-expressplan`.

### Completion Notes

- Added the `Branch and PR Policy` contract to `lens-quickdev`.
- Defined direct commit behavior for active in-progress feature branches and Lens git orchestration for prepared working branches and PR creation.
- Added branch-state blockers for dirty, detached, ambiguous, merge, rebase, and cherry-pick states before implementation.
- Required branch context fields for later evidence updates.

### File List

- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-quickdev/SKILL.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/scripts/tests/test-quickdev-conductor-contract.py`