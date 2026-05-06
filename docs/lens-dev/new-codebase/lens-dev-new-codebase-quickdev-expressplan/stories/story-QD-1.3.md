---
feature: lens-dev-new-codebase-quickdev-expressplan
story_id: "QD-1.3"
doc_type: story
status: done
type: new
title: "Implement Feature Resolution and Dev-Ready Gate"
priority: P0
story_points: 3
epic: "Epic 1 - Governed Quickdev Entry and Planning Gate"
depends_on: ["QD-1.1"]
blocks: ["QD-1.4", "QD-2.1"]
updated_at: 2026-05-06T21:20:00Z
---

# Story QD-1.3 - Implement Feature Resolution and Dev-Ready Gate

**Feature:** `lens-dev-new-codebase-quickdev-expressplan`
**Epic:** `Epic 1 - Governed Quickdev Entry and Planning Gate`
**Priority:** P0 | **Points:** 3 | **Status:** done

## Goal

Resolve the active Lens feature, enforce dev-ready eligibility, and block safely when target repo metadata is missing or ambiguous.

## Context

`lens-quickdev` must not behave like a planning shortcut. It is valid only after FinalizePlan handoff has made the feature dev-ready and registered the target repository metadata needed for implementation.

## Implementation Steps

1. Add feature resolution to the `lens-quickdev` conductor using explicit `--feature-id` when provided and active Lens context otherwise.
2. Read feature state through the sanctioned `lens-feature-yaml` helper or existing Lens feature metadata path.
3. Block when the phase is not dev-ready or equivalent to `finalizeplan-complete`.
4. Resolve docs path, governance docs path, and `target_repos` before any quickdev evidence or code assessment is created.
5. Emit a clear hard-stop when target repo metadata is absent or ambiguous.

## Acceptance Criteria

- [x] Active feature context resolves automatically.
- [x] Explicit `--feature-id` overrides active context.
- [x] Non-dev-ready features block before target-repo assessment.
- [x] Missing or unresolved `target_repos` blocks the run without guessing a write target.
- [x] Docs and governance docs paths resolve before quickdev evidence is created.

## Governance Coordination Note

This story consumes `feature.yaml` state but does not mutate governance. Target repo reconciliation is handled separately by QD-3.2 if metadata is incomplete.

## Dev Agent Record

### Agent Model Used

GitHub Copilot

### Debug Log References

- `uv run --with pytest --with pyyaml python -m pytest _bmad/lens-work/scripts/tests/test-quickdev-conductor-contract.py` - 9 passed.
- `git diff --check` - passed with no blocking output.
- Target repo commit: `b5616d57` on `feature/quickdev-expressplan`.

### Completion Notes

- Added the `Feature Resolution Gate` to the `lens-quickdev` skill contract.
- Defined explicit `--feature-id` precedence, active context fallback, sanctioned `feature-yaml-ops.py read` usage, and dev-ready phase allowlist.
- Required docs path, governance docs path, and first target repo resolution before evidence creation or target source assessment.
- Added hard-stop language for unresolved target repos without guessing from open files, terminal state, or prompt text.

### File List

- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-quickdev/SKILL.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/scripts/tests/test-quickdev-conductor-contract.py`