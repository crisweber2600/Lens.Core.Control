---
feature: lens-dev-new-codebase-quickdev-expressplan
story_id: "QD-3.1"
doc_type: story
status: backlog
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
**Priority:** P2 | **Points:** 3 | **Status:** backlog

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

- [ ] The exact versioned artifact is published to `feature.yaml.docs.governance_docs_path/quickdev/`.
- [ ] Publication uses the sanctioned Lens publication path rather than direct governance authoring.
- [ ] Published reruns preserve their unique version suffixes.
- [ ] Publication status is recorded in the quickdev artifact.

## Governance Coordination Note

Governance publication is in scope for this feature, but the implementation must go through Lens publication helpers where available.

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

TBD

### Completion Notes

TBD

### File List

TBD