---
feature: lens-dev-new-codebase-quickdev-expressplan
story_id: "QD-2.4"
doc_type: story
status: done
type: new
title: "Handle Validation Failure Branches and Preserve Bug Quickdev"
priority: P1
story_points: 3
epic: "Epic 2 - Scoped Implementation Execution and Branch Control"
depends_on: ["QD-2.3"]
blocks: []
updated_at: 2026-05-06T21:20:00Z
---

# Story QD-2.4 - Handle Validation Failure Branches and Preserve Bug Quickdev

**Feature:** `lens-dev-new-codebase-quickdev-expressplan`
**Epic:** `Epic 2 - Scoped Implementation Execution and Branch Control`
**Priority:** P1 | **Points:** 3 | **Status:** done

## Goal

Implement the approved validation-failure behavior and ensure the existing `/lens-bug-quickdev` flow remains unchanged.

## Context

Planning decisions define three recovery paths: pre-commit failures create no commit, local post-commit failures do not push, and pushed or PR failures avoid shared-history rewrites. The bug-specific quickdev route is intentionally separate and must keep its commit/push/PR behavior.

## Implementation Steps

1. Add pre-commit validation failure handling that marks the evidence artifact blocked and creates no commit.
2. Add local post-commit validation failure handling that does not push or create a PR and records recovery guidance.
3. Add pushed or PR validation failure handling that avoids history rewrite and records fix-forward or blocked PR guidance.
4. Add regression coverage or contract checks showing `/lens-bug-quickdev` routing and mandatory commit behavior remain unchanged.

## Acceptance Criteria

- [x] Pre-commit validation failures create no commit and mark the artifact `blocked`.
- [x] Local post-commit validation failures do not push or create PRs and record `validation-failed` guidance.
- [x] Pushed or PR validation failures avoid history rewrite and record fix-forward or blocked PR recovery.
- [x] `/lens-bug-quickdev` routing remains unchanged under regression coverage.

## Governance Coordination Note

Failure handling changes must preserve the bug quickdev conductor contract. Any intentional change to bug quickdev requires a separate explicit feature or bugfix.

## Dev Agent Record

### Agent Model Used

GitHub Copilot

### Debug Log References

- `uv run --with pytest --with pyyaml python -m pytest _bmad/lens-work/scripts/tests/test-quickdev-conductor-contract.py` - 21 passed.
- `git diff --check` - passed with no blocking output.
- Target repo commit: `49b36359` on `feature/quickdev-expressplan`.

### Completion Notes

- Added `Validation Failure Handling` to `lens-quickdev` for pre-commit, local post-commit, and pushed/PR validation failures.
- Required blocked evidence with no commit for pre-commit failures, `validation-failed` guidance without push/PR for local post-commit failures, and fix-forward or blocked PR guidance without history rewrite after push/PR.
- Added regression checks proving `/lens-bug-quickdev` still routes to `bmad-lens-bug-quickdev` and keeps mandatory commit, push, PR, bug-artifact recording, and closeout behavior.

### File List

- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-quickdev/SKILL.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/scripts/tests/test-quickdev-conductor-contract.py`