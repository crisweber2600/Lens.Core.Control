---
feature: lens-dev-new-codebase-quickdev-expressplan
story_id: "QD-2.4"
doc_type: story
status: backlog
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
**Priority:** P1 | **Points:** 3 | **Status:** backlog

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

- [ ] Pre-commit validation failures create no commit and mark the artifact `blocked`.
- [ ] Local post-commit validation failures do not push or create PRs and record `validation-failed` guidance.
- [ ] Pushed or PR validation failures avoid history rewrite and record fix-forward or blocked PR recovery.
- [ ] `/lens-bug-quickdev` routing remains unchanged under regression coverage.

## Governance Coordination Note

Failure handling changes must preserve the bug quickdev conductor contract. Any intentional change to bug quickdev requires a separate explicit feature or bugfix.

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

TBD

### Completion Notes

TBD

### File List

TBD