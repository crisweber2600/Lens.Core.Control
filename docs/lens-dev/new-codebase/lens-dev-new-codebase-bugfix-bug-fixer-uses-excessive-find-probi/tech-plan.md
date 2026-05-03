---
feature: lens-dev-new-codebase-bugfix-bug-fixer-uses-excessive-find-probi
doc_type: tech-plan
status: approved
goal: "Define the implementation plan for tightening the bug-fixer SKILL.md target path and landing the stub truncation fix."
key_decisions:
  - Story S1 AC must assert the exact writable path: TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-bug-fixer/SKILL.md
  - bug-fixer-ops.py stub truncation fix must be committed before dev begins (MAX_STUB_LEN = 35 constant with comment pointing to SAFE_ID_PATTERN).
  - S1 and S2 must be explicitly sequenced; S2 depends on S1.
  - feature-yaml-ops.py ModuleNotFoundError is a follow-on bug, not a blocker for this feature.
open_questions:
  - Should MAX_STUB_LEN = 35 be a named constant referencing SAFE_ID_PATTERN in init-feature-ops.py for cross-file traceability?
  - Is there a conductor trace or test harness to verify SKILL.md changes, or is verification purely manual?
depends_on:
  - business-plan.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/prd.md
blocks: []
updated_at: '2026-05-03T19:10:00Z'
---

# Tech Plan — Bug Fixer Excessive Find Probe Fix

## Overview

This feature targets two concrete defects in the `bmad-lens-bug-fixer` skill. The implementation is narrow: one story to tighten the SKILL.md path in acceptance criteria, and one story to commit the stub truncation fix. Neither story requires runtime behaviour changes to the bug-fixer skill beyond the already-validated truncation constant.

## Target Files

| File | Change | Notes |
| --- | --- | --- |
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-bug-fixer/SKILL.md` | Add path-pinning note to acceptance criteria | Exact writable path; read-only `lens.core/` clone must NOT be edited |
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-bug-fixer/bug-fixer-ops.py` | Commit stub truncation fix (`MAX_STUB_LEN = 35`) | Fix was applied during expressplan run but not committed to feature branch |

## Implementation Slices

### Slice 1 — Story S1: SKILL.md Path Tightening

**Objective:** Update Story S1 AC to name the exact writable SKILL.md path and prevent editing the read-only `lens.core/` clone.

**Acceptance Criteria:**
- Story S1 AC contains: "Edit only `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-bug-fixer/SKILL.md`; the `lens.core/` release payload clone is read-only and must not be modified."
- A reviewer can confirm the writable target without ambiguity.

**Exit Criteria:** Story S1 AC updated and committed; path reference is unambiguous.

---

### Slice 2 — Story S2: Stub Truncation Fix Committed

**Objective:** Commit the `bug-fixer-ops.py` stub truncation fix to the feature branch.

**Depends on:** Slice 1 (S1 must be committed first to preserve sequencing).

**Acceptance Criteria:**
- `bug-fixer-ops.py` contains `MAX_STUB_LEN = 35` as a named constant.
- A comment references `SAFE_ID_PATTERN` in `init-feature-ops.py` for cross-file traceability.
- The fix is committed to the `lens.core.src` feature branch (not just the local working tree).

**Exit Criteria:** `bug-fixer-ops.py` committed with the truncation fix on the feature branch.

---

## Follow-On: feature-yaml-ops.py Import Bug

`feature-yaml-ops.py` fails with `ModuleNotFoundError: No module named 'lens_config'` when invoked via `uv run --script` (temp-copy execution breaks relative parent path computation). This is a follow-on bug and is out of scope for this feature. It must be filed as a separate bug artifact before FinalizePlan closes.

## Out-Of-Scope Deferral

`lens-bug-reporter/SKILL.md` path-probing ambiguity is explicitly out of scope for this batch. No action required.

## Validation

| Check | Method | Owner |
| --- | --- | --- |
| Story S1 AC pins exact writable path | Manual review of AC text | Planning reviewer |
| bug-fixer-ops.py fix committed | `git log` on feature branch | Dev |
| feature-yaml-ops.py bug filed | Bug artifact present in follow-on | Dev |
| No edits to lens.core/ clone | Diff review at PR time | Reviewer |
