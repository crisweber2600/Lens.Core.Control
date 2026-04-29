---
feature: lens-dev-new-codebase-techplan
story_id: "TK-2.5"
doc_type: story
status: not-started
title: "Test Harness Foundation for lens-techplan"
priority: P1
story_points: 2
epic: "Epic 2 — Target-Project Command Surface"
depends_on: ["TK-2.4"]
blocks: ["TK-3.2"]
updated_at: 2026-04-29T00:00:00Z
---

# Story TK-2.5 — Test Harness Foundation for `lens-techplan`

**Feature:** `lens-dev-new-codebase-techplan`  
**Epic:** 2 — Target-Project Command Surface  
**Priority:** P1 | **Points:** 2 | **Status:** not-started

---

## Goal

Create the focused test file for `lens-techplan` at the path confirmed in TK-2.1. Add the prompt-start and wrapper-equivalence regression tests and confirm they pass in green mode (stub and conductor present; shared utilities mocked or stubbed).

---

## Context

TK-3.2 adds the full parity regression suite. This story only creates the test harness with the minimum regressions needed to catch breakage of the stub→release→conductor chain. Shared utilities (publish hook, BMAD wrapper) will not be present yet — tests that depend on them must be marked `@pytest.mark.skip(reason="TK-3.1 pending")` or equivalent.

---

## Acceptance Criteria

- [ ] Focused test file exists at the path confirmed in TK-2.1.
- [ ] **Prompt-start regression:** test verifies that invoking `lens-techplan` (via the public stub) runs preflight and loads `bmad-lens-techplan/SKILL.md` without crashing.
- [ ] **Wrapper-equivalence regression:** test verifies that the release prompt routes to the conductor skill (no inline logic branches are present in the release prompt).
- [ ] Tests requiring TK-3.1 shared utilities are explicitly skipped with a `TK-3.1 pending` reason.
- [ ] `uv run --with pytest pytest <test-file-path> -q` (or equivalent confirmed in TK-2.1) exits green.

---

## Dev Notes

- Use the test pattern confirmed in TK-2.1 for how focused tests are run.
- The full parity regression set (6 items) is in TK-3.2; this story contains only prompt-start and wrapper-equivalence.
- When writing the TK-3.1 skip annotations, include the exact TK-3.1 story ID in the reason string so they are searchable.

---

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

- (Test file path from TK-2.1 assessment — to be confirmed)
