---
feature: lens-dev-new-codebase-bugfix-bug-fixer-uses-excessive-find-probi
doc_type: sprint-plan
status: approved
goal: "Sequence the two-story bugfix delivery for the bug-fixer excessive find-probe issue into implementation-ready slices."
key_decisions:
  - S1 (SKILL.md path tightening) must complete before S2 (stub truncation commit) to preserve explicit sequencing.
  - feature-yaml-ops.py import bug is filed as a follow-on artifact, not a delivery slice here.
  - lens-bug-reporter/SKILL.md path-probing is deferred; no slice allocated.
open_questions:
  - Is there a conductor trace or test harness for SKILL.md changes, or is verification manual?
  - Should MAX_STUB_LEN reference SAFE_ID_PATTERN cross-file comment be added in the same commit as the truncation fix?
depends_on:
  - business-plan.md
  - tech-plan.md
blocks: []
updated_at: '2026-05-03T19:10:00Z'
---

# Sprint Plan — Bug Fixer Excessive Find Probe Fix

## Sprint Objective

Land the two-story bugfix for `bmad-lens-bug-fixer` excessive find-probe behaviour: tighten Story S1 AC to name the exact SKILL.md writable path, and commit the `bug-fixer-ops.py` stub truncation fix to the feature branch.

## Delivery Slices

| Slice ID | Slice | Objective | Exit Criteria |
| --- | --- | --- | --- |
| BF-1.1-skill-path-tightening | Slice 1 — Story S1 | Tighten SKILL.md path in AC | Story S1 AC names exact writable path; committed to feature branch |
| BF-2.1-stub-truncation-commit | Slice 2 — Story S2 | Commit stub truncation fix | `bug-fixer-ops.py` with `MAX_STUB_LEN = 35` committed to `lens.core.src` feature branch |

## Slice 1 — Story S1: SKILL.md Path Tightening

### Scope

- Update Story S1 acceptance criteria to assert the exact writable file path:
  `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-bug-fixer/SKILL.md`
- Make clear the `lens.core/` release payload clone is read-only and must not be modified.

### Exit Criteria

- Story S1 AC text reviewed and committed.
- Path reference unambiguous to any implementer reading the AC.

### Definition of Done

- AC updated in the story file.
- No references to `lens.core/` as a valid edit target.
- Committed and pushed to the `lens.core.src` feature branch.

---

## Slice 2 — Story S2: Stub Truncation Fix

### Scope

- Commit the `bug-fixer-ops.py` stub truncation fix that was applied during expressplan but not yet committed.
- Add `MAX_STUB_LEN = 35` as a named constant.
- Add a comment referencing `SAFE_ID_PATTERN` in `init-feature-ops.py`.

### Depends On

- Slice 1 (BF-1.1) must be complete before BF-2.1 starts.

### Exit Criteria

- `bug-fixer-ops.py` committed to the feature branch with the truncation fix.
- `MAX_STUB_LEN = 35` is a named constant with a cross-reference comment.
- Verified via `git log` on the `lens.core.src` feature branch.

### Definition of Done

- Fix committed, not just present in local working tree.
- Constant naming and comment reviewed.

---

## Follow-On Work (Not In This Sprint)

| Item | Owner | Notes |
| --- | --- | --- |
| `feature-yaml-ops.py` import bug (`ModuleNotFoundError: No module named 'lens_config'`) | Follow-on bug report | File before FinalizePlan closes |
| `lens-bug-reporter/SKILL.md` path-probing ambiguity | Deferred | Explicitly out of scope for this batch |

## Sequencing Summary

```
BF-1.1-skill-path-tightening  →  BF-2.1-stub-truncation-commit
```

Both slices must complete for the feature to close. S2 must not be merged without S1.
