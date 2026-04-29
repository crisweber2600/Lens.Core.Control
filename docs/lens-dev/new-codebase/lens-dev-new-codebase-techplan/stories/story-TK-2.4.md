---
feature: lens-dev-new-codebase-techplan
story_id: "TK-2.4"
doc_type: story
status: not-started
title: "Wire lens-techplan to Discovery and Preflight"
priority: P1
story_points: 1
epic: "Epic 2 — Target-Project Command Surface"
depends_on: ["TK-2.3"]
blocks: ["TK-2.5"]
updated_at: 2026-04-29T00:00:00Z
---

# Story TK-2.4 — Wire `lens-techplan` to Discovery and Preflight

**Feature:** `lens-dev-new-codebase-techplan`  
**Epic:** 2 — Target-Project Command Surface  
**Priority:** P1 | **Points:** 1 | **Status:** not-started

---

## Goal

Register `lens-techplan` in the discovery file (resolved in TK-2.1) so that `lens-discover` and preflight surface it as a retained command.

---

## Context

The discovery mechanism is confirmed in TK-2.1. This story applies the registration. Until this story is complete, `lens-discover` will not surface the command and preflight will not validate its presence.

---

## Acceptance Criteria

- [ ] `lens-techplan` is registered in the discovery file confirmed by TK-2.1 assessment (file path recorded in TK-2.1).
- [ ] After registration, running the discovery scan against the target project includes `lens-techplan` in its output.
- [ ] After registration, preflight passes in the target project without reporting `lens-techplan` as missing.
- [ ] No other discovery registrations are added or removed by this story.

---

## Dev Notes

- Use the exact registration format confirmed by TK-2.1 (file and mechanism).
- If the registration format has changed since the assessment, document the deviation and update the assessment note.

---

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

- (File from TK-2.1 assessment — to be confirmed)
