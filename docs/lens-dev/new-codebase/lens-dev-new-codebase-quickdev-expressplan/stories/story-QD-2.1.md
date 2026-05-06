---
feature: lens-dev-new-codebase-quickdev-expressplan
story_id: "QD-2.1"
doc_type: story
status: backlog
type: new
title: "Delegate Implementation Through bmad-quick-dev"
priority: P1
story_points: 3
epic: "Epic 2 - Scoped Implementation Execution and Branch Control"
depends_on: ["QD-1.3"]
blocks: ["QD-2.2"]
updated_at: 2026-05-06T21:20:00Z
---

# Story QD-2.1 - Delegate Implementation Through bmad-quick-dev

**Feature:** `lens-dev-new-codebase-quickdev-expressplan`
**Epic:** `Epic 2 - Scoped Implementation Execution and Branch Control`
**Priority:** P1 | **Points:** 3 | **Status:** backlog

## Goal

Route actual implementation through the existing `bmad-quick-dev` capability while keeping `lens-quickdev` a conductor.

## Context

The wrapper exists to add Lens governance, lifecycle gates, evidence, and publication around quick development. It must not fork or duplicate the quick-dev implementation engine.

## Implementation Steps

1. Load the registered `bmad-quick-dev` skill from the Lens/BMAD skill registry or installed `.github/skills` surface.
2. Build a delegation packet containing feature id, target repo, docs path, evidence artifact path, and user request.
3. Invoke the existing quick-dev workflow with Lens context rather than reimplementing its implementation logic.
4. Capture the delegate result fields needed by later evidence and branch-policy stories.

## Acceptance Criteria

- [ ] The wrapper delegates implementation through the sanctioned Lens quick-dev path.
- [ ] If no script facade exists, the wrapper loads the registered `bmad-quick-dev` skill directly with equivalent Lens context.
- [ ] No second implementation engine is introduced.
- [ ] Delegate outputs are captured for evidence update and validation handling.

## Governance Coordination Note

Implementation delegation may write source code in the target repo only. Control-repo documentation updates stay limited to feature docs and quickdev evidence unless a scope override is recorded.

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

TBD

### Completion Notes

TBD

### File List

TBD