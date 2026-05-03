---
feature: lens-dev-new-codebase-bugfix-bug-fixer-uses-excessive-find-probi
doc_type: implementation-readiness
status: approved
track: express
domain: lens-dev
service: new-codebase
depends_on: []
blocks: []
key_decisions:
  - Both stories are independent and can be implemented in either order
  - The bug-fixer-ops.py fix exists in the working tree; Story 2.1 only needs commit + test
open_questions:
  - Should feature-yaml-ops.py import path bug be filed as a separate bug before dev closes?
updated_at: '2026-05-03T19:15:00Z'
---

# Implementation Readiness — Bug Fixer Excessive Find Probing

## Readiness Summary

| Check | Status | Notes |
|---|---|---|
| Business plan approved | ✅ | `business-plan.md` status: approved |
| Tech plan approved | ✅ | `tech-plan.md` status: approved |
| Sprint plan approved | ✅ | `sprint-plan.md` status: approved |
| Epics and stories defined | ✅ | 2 epics, 2 stories |
| Acceptance criteria complete | ✅ | Both stories have full AC |
| Dependencies clear | ✅ | No cross-feature dependencies |
| Write boundary confirmed | ✅ | All changes in `TargetProjects/lens-dev/new-codebase/lens.core.src/` |
| Read-only clone risk addressed | ✅ | Story 1.1 AC explicitly targets writable path |
| Existing fix committed | ⚠️ | `bug-fixer-ops.py` fix in working tree; Story 2.1 commits it |
| Regression test coverage | ⚠️ | Test does not yet exist; Story 2.1 creates it |

**Overall Readiness: READY FOR DEV** — warnings are handled within the dev stories.

## Story Readiness Detail

### Story 1.1 — Fix SKILL.md preflight path and working directory
- **Ready:** ✅
- **Scope clear:** SKILL.md path explicitly identified (writable source, not read-only clone)
- **AC testable:** Manual verification via conductor trace on next `/lens-bug-fixer` run
- **Risk:** Low — narrowing change only

### Story 2.1 — Commit and test derive-feature-id stub truncation fix
- **Ready:** ✅
- **Scope clear:** Commit existing fix + add regression test
- **AC testable:** `uv run pytest` or equivalent on the test file
- **Risk:** Low — fix already verified working in this run

## Open Questions for Dev

1. Should `feature-yaml-ops.py` import path bug be filed as a follow-on bug artifact before
   this feature closes? (Observed: `ModuleNotFoundError: No module named 'lens_config'` when
   invoked via `uv run --script` due to temp-copy path resolution failure)

## Constraints

- All code changes MUST target `TargetProjects/lens-dev/new-codebase/lens.core.src/`
- `lens.core/` is READ-ONLY — never edit it directly
- SKILL.md changes affect AI conductor behaviour only; no runtime interface changes
