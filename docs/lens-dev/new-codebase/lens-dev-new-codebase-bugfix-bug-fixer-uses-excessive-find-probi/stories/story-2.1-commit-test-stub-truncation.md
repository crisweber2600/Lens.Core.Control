---
feature: lens-dev-new-codebase-bugfix-bug-fixer-uses-excessive-find-probi
story_id: "2.1"
epic: 2
title: Commit and test derive-feature-id stub truncation fix
points: 1
status: Ready
assignee: crisweber2600
updated_at: '2026-05-03T19:15:00Z'
---

# Story 2.1 — Commit and Test derive-feature-id Stub Truncation Fix

## Context

`bug-fixer-ops.py derive-feature-id` previously truncated the slug at index 96 chars, which
produced a 125-char feature ID rejected by `init-feature-ops.py` (`SAFE_ID_PATTERN` max 64
chars). The fix — `max_stub_len = 64 - len(FEATURE_ID_PREFIX)` (= 35 chars for the
`lens-dev-new-codebase-bugfix-` prefix) — was applied to the working tree during this
bugbash run but was not committed to the `lens.core.src` feature branch.

## Files to Change

| File | Action |
|---|---|
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/scripts/bug-fixer-ops.py` | Commit the existing working-tree fix |
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/scripts/tests/test_bug_fixer_ops.py` | Create — regression test for stub truncation |

## Acceptance Criteria

- [ ] `bug-fixer-ops.py` committed on branch `lens-dev-new-codebase-bugfix-bug-fixer-uses-excessive-find-probi` in `lens.core.src`
- [ ] Committed code contains `max_stub_len = 64 - len(FEATURE_ID_PREFIX)` (not `stub[:96]`)
- [ ] Test file `test_bug_fixer_ops.py` exists in `_bmad/lens-work/scripts/tests/`
- [ ] Test calls `derive_feature_id_from_slugs` (or equivalent) with a slug longer than 35 chars
- [ ] Test asserts `len(feature_id) <= 64`
- [ ] Test passes with current code

## Implementation Steps

1. In `TargetProjects/lens-dev/new-codebase/lens.core.src/`:
   ```bash
   git add _bmad/lens-work/scripts/bug-fixer-ops.py
   git commit -m "fix: constrain bug-fixer-ops derive-feature-id stub to 64-char SAFE_ID limit"
   ```
2. Create `_bmad/lens-work/scripts/tests/test_bug_fixer_ops.py`:
   ```python
   from scripts.bug_fixer_ops import derive_feature_id_from_slugs  # adjust import
   
   def test_derive_feature_id_respects_max_length():
       long_slug = "a-very-long-bugfix-description-that-exceeds-thirty-five-characters-easily"
       feature_id = derive_feature_id_from_slugs("lens-dev", "new-codebase", long_slug)
       assert len(feature_id) <= 64, f"Feature ID too long: {len(feature_id)} chars"
   ```
3. Commit the test:
   ```bash
   git add _bmad/lens-work/scripts/tests/test_bug_fixer_ops.py
   git commit -m "test: add regression test for derive-feature-id 64-char length constraint"
   ```
4. Push to remote feature branch

## Verification

```bash
cd TargetProjects/lens-dev/new-codebase/lens.core.src
uv run pytest _bmad/lens-work/scripts/tests/test_bug_fixer_ops.py -v
```

Expected: all tests pass, `PASSED test_derive_feature_id_respects_max_length`
