---
feature: lens-dev-new-codebase-quickdev-expressplan
story_id: "QD-1.2"
doc_type: story
status: backlog
type: new
title: "Register Command Discovery and Operator Help"
priority: P0
story_points: 2
epic: "Epic 1 - Governed Quickdev Entry and Planning Gate"
depends_on: ["QD-1.1"]
blocks: []
updated_at: 2026-05-06T21:20:00Z
---

# Story QD-1.2 - Register Command Discovery and Operator Help

**Feature:** `lens-dev-new-codebase-quickdev-expressplan`
**Epic:** `Epic 1 - Governed Quickdev Entry and Planning Gate`
**Priority:** P0 | **Points:** 2 | **Status:** backlog

## Goal

Make `lens-quickdev` discoverable through the Lens module surfaces and document that it is only valid for dev-ready features.

## Context

QD-1.1 creates the public prompt and skill. This story registers those surfaces in the module discovery/help metadata so users can find the command without expanding into unrelated packaging or generated control-root edits.

## Implementation Steps

1. Register `lens-quickdev` in `_bmad/lens-work/module.yaml` with the prompt and skill entries expected by the module manifest.
2. Add one `lens-quickdev` row to `_bmad/lens-work/module-help.csv` with dev-ready-only guidance.
3. Verify command discovery lists `lens-quickdev` exactly once.
4. Record any requested broader non-source surface work as scope expansion before editing it.

## Acceptance Criteria

- [ ] `_bmad/lens-work/module.yaml` exposes `lens-quickdev` exactly once.
- [ ] `_bmad/lens-work/module-help.csv` exposes `lens-quickdev` exactly once.
- [ ] Operator-facing guidance states that `lens-quickdev` is a dev-ready-only governed wrapper.
- [ ] Discovery/help updates do not silently expand into broader non-source surfaces without an explicit override record.

## Governance Coordination Note

Command discovery is an approved non-source surface for this feature. If implementation discovers generated root prompt/help files that need updates, warn first and document any override in the quickdev evidence.

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

TBD

### Completion Notes

TBD

### File List

TBD