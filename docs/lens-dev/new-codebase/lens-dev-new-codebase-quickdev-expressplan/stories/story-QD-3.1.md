---
feature: lens-dev-new-codebase-quickdev-expressplan
story_id: "QD-3.1"
doc_type: story
status: done
type: new
title: "Publish Versioned Quickdev Evidence to Governance"
priority: P2
story_points: 3
epic: "Epic 3 - Audit Trail, Publication, and Safe Surface Expansion"
depends_on: ["QD-2.3"]
blocks: ["QD-3.2"]
updated_at: 2026-05-06T21:20:00Z
---

# Story QD-3.1 - Publish Versioned Quickdev Evidence to Governance

**Feature:** `lens-dev-new-codebase-quickdev-expressplan`
**Epic:** `Epic 3 - Audit Trail, Publication, and Safe Surface Expansion`
**Priority:** P2 | **Points:** 3 | **Status:** done

## Goal

Publish the exact versioned quickdev evidence artifact to the feature governance docs mirror after a completed run.

## Context

User decisions made governance publication mandatory. The governance copy must preserve the exact versioned filename under a dedicated `quickdev/` folder, and publication should use sanctioned Lens helpers rather than direct ad hoc governance writes.

## Implementation Steps

1. Resolve `feature.yaml.docs.governance_docs_path` for the active feature.
2. Map local `quickdev/quickdev-[summary]-vNNN.md` to the matching governance `quickdev/` destination.
3. Publish through the sanctioned Lens publication path or helper.
4. Verify the published artifact matches the local versioned artifact.
5. Record publication path and status in the evidence artifact.

## Acceptance Criteria

- [x] The exact versioned artifact is published to `feature.yaml.docs.governance_docs_path/quickdev/`.
- [x] Publication uses the sanctioned Lens publication path rather than direct governance authoring.
- [x] Published reruns preserve their unique version suffixes.
- [x] Publication status is recorded in the quickdev artifact.

## Governance Coordination Note

Governance publication is in scope for this feature, but the implementation must go through Lens publication helpers where available.

## Dev Agent Record

### Agent Model Used

GitHub Copilot

### Debug Log References

- `uv run --with pytest --with pyyaml python -m pytest _bmad/lens-work/scripts/tests/test-quickdev-conductor-contract.py` - 23 passed.
- `git diff --check` - passed with no blocking output.
- Target repo commit: `bcb18d40` on `feature/quickdev-expressplan`.

### Completion Notes

- Added `Governance Publication` to `lens-quickdev`, resolving `feature.yaml.docs.governance_docs_path` and mapping the local versioned evidence artifact to the matching governance `quickdev/` destination.
- Required sanctioned Lens publication helper usage, exact content verification, preserved rerun version suffixes, and publication status/path recording in `Commit and Publication Record`.
- Added focused contract tests for governance publication path, helper usage, version preservation, and publication result recording.

### File List

- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-quickdev/SKILL.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/scripts/tests/test-quickdev-conductor-contract.py`