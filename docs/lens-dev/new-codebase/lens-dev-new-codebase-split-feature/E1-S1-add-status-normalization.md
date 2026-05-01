---
feature: lens-dev-new-codebase-split-feature
epic: 1
story_id: E1-S1
title: Add status normalization to validate-split and move-stories
type: fix
points: 2
status: ready
phase: dev
updated_at: '2026-05-01T00:00:00Z'
depends_on: []
blocks: [E3-S1]
target_repo: lens.core.src
target_branch: develop
---

# E1-S1 — Add status normalization to validate-split and move-stories

## Context

The current `split-feature-ops.py` uses `IN_PROGRESS_STATUS = "in-progress"` and compares
story statuses directly without normalization. Old-codebase sprint files may use
`in_progress` (underscore), `IN_PROGRESS` (uppercase), or `in progress` (space) — all
should trigger the same hard gate.

Per tech-plan §2.3 (BS-1): "All status values are normalized before the `in-progress`
check: `in_progress`, `in-progress`, `IN_PROGRESS`, and `in progress` are treated as
equivalent."

**File:** `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/scripts/split-feature-ops.py`

**Old-codebase reference:** `TargetProjects/lens-dev/old-codebase/lens.core.src/` (discovery
feature: `lens-dev-old-codebase-discovery`)

## Implementation Steps

### 1. Add normalize_status helper

Add a `normalize_status` helper function near the top of the file (after the constant
definitions):

```python
def normalize_status(s: str) -> str:
    """Normalize status string: lowercase, replace underscores and spaces with hyphens."""
    return s.lower().replace("_", "-").replace(" ", "-")
```

### 2. Apply normalization in cmd_validate_split

In `cmd_validate_split`, where story statuses are checked:

```python
# Before (current):
if status == IN_PROGRESS_STATUS:
    blocked.append(...)

# After:
if status is not None and normalize_status(status) == IN_PROGRESS_STATUS:
    blocked.append(...)
```

Also apply normalization when reading the status from `sprint_statuses` and
`get_story_status_from_file`.

### 3. Apply normalization in cmd_move_stories

In `cmd_move_stories`, apply the same normalization to the status comparison:

```python
# Before (current):
if status == IN_PROGRESS_STATUS:
    blocked.append(...)

# After:
if status is not None and normalize_status(status) == IN_PROGRESS_STATUS:
    blocked.append(...)
```

### 4. Verify existing tests still pass

```
uv run tests/test-split-feature-ops.py
```
All 87 existing tests must still pass.

## Acceptance Criteria Checklist

```
[ ] normalize_status() helper added and used in both cmd_validate_split and cmd_move_stories
[ ] validate-split returns status: fail when story has in_progress (underscore) status
[ ] validate-split returns status: fail when story has IN_PROGRESS (uppercase) status
[ ] validate-split returns status: fail when story has in progress (with space) status
[ ] All existing 87 tests still pass
```

## Carry-Forward Notes

- This story enables E3-S1 normalization test additions
- The normalize_status helper should also be applied in get_story_status_from_file
  if that function returns raw status values from frontmatter without normalization
