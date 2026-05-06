---
feature: lens-dev-new-codebase-quickdev-expressplan
story_id: "QD-2.1"
doc_type: story
status: done
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
**Priority:** P1 | **Points:** 3 | **Status:** done

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

- [x] The wrapper delegates implementation through the sanctioned Lens quick-dev path.
- [x] If no script facade exists, the wrapper loads the registered `bmad-quick-dev` skill directly with equivalent Lens context.
- [x] No second implementation engine is introduced.
- [x] Delegate outputs are captured for evidence update and validation handling.

## Governance Coordination Note

Implementation delegation may write source code in the target repo only. Control-repo documentation updates stay limited to feature docs and quickdev evidence unless a scope override is recorded.

## Dev Agent Record

### Agent Model Used

GitHub Copilot

### Debug Log References

- `uv run --with pytest --with pyyaml python -m pytest _bmad/lens-work/scripts/tests/test-quickdev-conductor-contract.py` - 14 passed.
- `git diff --check` - passed with no blocking output.
- Target repo commit: `8621d0c6` on `feature/quickdev-expressplan`.

### Completion Notes

- Added the `Delegation Packet` contract for `lens-quickdev` with feature id, target repo path, docs paths, evidence artifact path, ask, validation plan, and branch context.
- Defined the sanctioned Lens BMAD wrapper route and direct registered-skill fallback for `bmad-quick-dev`.
- Explicitly forbade reimplementing quick-dev planning, editing, validation, or review inside the wrapper.
- Added delegate output capture fields needed by later evidence and branch-policy stories.

### File List

- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-quickdev/SKILL.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/scripts/tests/test-quickdev-conductor-contract.py`