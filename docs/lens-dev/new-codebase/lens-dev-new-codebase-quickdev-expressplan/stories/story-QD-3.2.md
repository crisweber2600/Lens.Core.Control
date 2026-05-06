---
feature: lens-dev-new-codebase-quickdev-expressplan
story_id: "QD-3.2"
doc_type: story
status: backlog
type: new
title: "Reconcile Target Repos and Dev-Handoff Metadata"
priority: P2
story_points: 2
epic: "Epic 3 - Audit Trail, Publication, and Safe Surface Expansion"
depends_on: ["QD-3.1"]
blocks: ["QD-3.3"]
updated_at: 2026-05-06T21:20:00Z
---

# Story QD-3.2 - Reconcile Target Repos and Dev-Handoff Metadata

**Feature:** `lens-dev-new-codebase-quickdev-expressplan`
**Epic:** `Epic 3 - Audit Trail, Publication, and Safe Surface Expansion`
**Priority:** P2 | **Points:** 2 | **Status:** backlog

## Goal

Keep the feature metadata and handoff docs aligned so `/dev` can resolve `lens.core.src` without reopening planning questions.

## Context

FinalizePlan registered `lens.core.src` in `feature.yaml.target_repos`. This story preserves that contract during implementation and keeps readiness docs aligned with the live schema and versioned evidence rule.

## Implementation Steps

1. Read `feature.yaml.target_repos` through the sanctioned `feature-yaml` helper.
2. Confirm `lens.core.src` is registered for the feature.
3. If metadata must be corrected, update it through the sanctioned helper instead of patching governance by hand.
4. Update implementation-readiness notes if the implementation changes the quickdev evidence or publication path contract.
5. Run strict handoff validation after any metadata or readiness changes.

## Acceptance Criteria

- [ ] `feature.yaml.target_repos` includes `lens.core.src`.
- [ ] Any update is made through the sanctioned `feature-yaml` helper.
- [ ] Implementation-readiness records the versioned quickdev rule and sanctioned publication path.
- [ ] Strict handoff validation confirms the metadata and docs are internally consistent.

## Governance Coordination Note

This story may update governance feature metadata, but only through sanctioned Lens helpers and with resulting changes committed through the normal lifecycle path.

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

TBD

### Completion Notes

TBD

### File List

TBD