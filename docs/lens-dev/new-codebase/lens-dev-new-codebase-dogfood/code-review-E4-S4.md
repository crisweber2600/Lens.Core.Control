---
feature: lens-dev-new-codebase-dogfood
doc_type: code-review
story: E4-S4
commit: cf051c34
status: approved
updated_at: "2025-07-17"
---

# Code Review — E4-S4: Retrospective Gate in bmad-lens-complete

## Story

Escalate the missing-retrospective check in `bmad-lens-complete/SKILL.md` from advisory (`warn`) to hard blocker (`fail`). A missing or non-approved retrospective now aborts the finalize operation.

## Changes

- `_bmad/lens-work/skills/bmad-lens-complete/SKILL.md` — guard escalation + failure JSON shapes
- `_bmad/lens-work/scripts/tests/test-complete-retrospective-gate.py` — 8 tests

## Review

### Correctness

- Missing retrospective → `status: fail` + `blocker: retrospective_missing`. ✅
- Retrospective with non-`approved` status → `status: fail` + `blocker: retrospective_not_approved`. ✅
- Error message directs user to `bmad-lens-retrospective`. ✅
- Finalize operation documentation states: "A `fail` status aborts finalize immediately". ✅
- `document-project` missing remains advisory (`warn`) — not a blocker. ✅
- Retrospective check appears in `check-preconditions` return shape checks list. ✅

### Test Coverage

8 tests; all pass. Test fixed a regex false-positive on "not advisory" phrase. ✅

### Issues

None.

## Verdict: APPROVED
