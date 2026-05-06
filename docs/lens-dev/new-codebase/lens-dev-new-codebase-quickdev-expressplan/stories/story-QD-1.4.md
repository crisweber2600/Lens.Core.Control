---
feature: lens-dev-new-codebase-quickdev-expressplan
story_id: "QD-1.4"
doc_type: story
status: done
type: new
title: "Create Versioned Quickdev Evidence Scaffold"
priority: P0
story_points: 3
epic: "Epic 1 - Governed Quickdev Entry and Planning Gate"
depends_on: ["QD-1.3"]
blocks: []
updated_at: 2026-05-06T21:20:00Z
---

# Story QD-1.4 - Create Versioned Quickdev Evidence Scaffold

**Feature:** `lens-dev-new-codebase-quickdev-expressplan`
**Epic:** `Epic 1 - Governed Quickdev Entry and Planning Gate`
**Priority:** P0 | **Points:** 3 | **Status:** done

## Goal

Create a durable quickdev evidence artifact before implementation starts so each run captures request, assessment, assumptions, plan, and validation path.

## Context

The approved planning packet removed a separate `commit.md` and made `quickdev/quickdev-[summaryofrequeststub]-vNNN.md` the single versioned evidence artifact. Reruns must preserve prior evidence instead of overwriting it.

## Implementation Steps

1. Add quickdev evidence path resolution under the feature docs `quickdev/` folder.
2. Generate a filesystem-safe request summary stub from the user request.
3. Select the next `vNNN` suffix for the request stub without overwriting existing artifacts.
4. Write the initial artifact before delegation with request, assessment, assumptions, validation plan, and implementation plan sections.
5. Keep the artifact local until the run reaches the governance publication story.

## Acceptance Criteria

- [x] The wrapper creates `quickdev/quickdev-[summaryofrequeststub]-vNNN.md` for each run.
- [x] Reruns create the next available version instead of overwriting prior evidence.
- [x] The artifact records request, assessment, assumptions, validation plan, and implementation plan before delegation.
- [x] The artifact is the only quickdev evidence file; no separate `commit.md` is created.

## Governance Coordination Note

This story creates local feature docs evidence only. Governance publication is handled by QD-3.1 through the sanctioned publication path.

## Dev Agent Record

### Agent Model Used

GitHub Copilot

### Debug Log References

- `uv run --with pytest --with pyyaml python -m pytest _bmad/lens-work/scripts/tests/test-quickdev-conductor-contract.py` - 11 passed.
- `git diff --check` - passed with no blocking output.
- Target repo commit: `751e9607` on `feature/quickdev-expressplan`.

### Completion Notes

- Added a `Versioned Evidence Scaffold` section to the `lens-quickdev` skill contract.
- Defined evidence directory resolution, request stub creation, `vNNN` version selection, required artifact sections, and no-overwrite behavior.
- Explicitly forbade separate `commit.md`, `quickdev-commit.md`, or sidecar commit evidence files.
- Added focused tests for version selection, pre-delegation evidence sections, overwrite prevention, and sidecar prevention.

### File List

- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-quickdev/SKILL.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/scripts/tests/test-quickdev-conductor-contract.py`