---
feature: lens-dev-new-codebase-split-feature
epic: 1
story_id: E1-S3
title: Add sprint-status.yaml list-format support to sprint plan parser
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

# E1-S3 — Add sprint-status.yaml list-format support to sprint plan parser

## Context

The `_extract_statuses_from_yaml_str` function in `split-feature-ops.py` handles two
`stories:` formats:
1. Dict format: `stories: {story-id: {status: …}}`
2. String-value dict: `stories: {story-id: "status-string"}`

However, sprint-status.yaml files in this repo use the list format:
```yaml
stories:
  - id: E1-S1
    status: in-progress
    title: ...
```

When validate-split is called with a sprint-status.yaml file, this format is not
recognized and the function returns empty, causing fallback to story-file frontmatter.
If the story file also doesn't have the status, the story is treated as eligible
(unknown status = eligible by default), potentially allowing in-progress stories to
pass the hard gate.

Per tech-plan §2.3: "Canonical YAML — a top-level `stories:` map where each key is
a story ID and the value contains a `status:` field."

Note: The tech-plan refers to a map, but the actual sprint-status.yaml files use a list.
The implementation should handle both to be robust.

**File:** `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/scripts/split-feature-ops.py`

## Implementation Steps

### 1. Extend _extract_statuses_from_yaml_str for list format

In `_extract_statuses_from_yaml_str`, add handling for the list-format `stories:` field
after the existing dict-format check:

```python
# Add after existing stories dict-format handler:

# Format: stories: [{id: story-id, status: ...}] (list format used in sprint-status.yaml)
if isinstance(stories, list):
    result = {}
    for item in stories:
        if isinstance(item, dict):
            story_id = item.get("id")
            status = item.get("status")
            if story_id and status:
                result[str(story_id)] = str(status)
    if result:
        return result
```

### 2. Verify the list-format handling integrates with parse_sprint_plan

The `parse_sprint_plan` function calls `_extract_statuses_from_yaml_str` for both
pure YAML and fenced YAML blocks. Since the fix is inside
`_extract_statuses_from_yaml_str`, it will automatically apply to both call paths.

No changes needed in `parse_sprint_plan` itself.

### 3. Test manually with a sprint-status.yaml fixture

Create a temporary sprint-status.yaml matching the repo format:
```yaml
sprint_number: 1
feature: test-feature
status: not-started
stories:
  - id: story-good
    status: pending
  - id: story-wip
    status: in-progress
```

Run validate-split with this file and confirm `story-wip` appears in `blocked`.

### 4. Verify existing tests still pass

```
uv run tests/test-split-feature-ops.py
```

## Acceptance Criteria Checklist

```
[ ] _extract_statuses_from_yaml_str handles stories: [{id: X, status: Y}] list format
[ ] parse_sprint_plan returns correct statuses when given a sprint-status.yaml list-format file
[ ] validate-split returns status: fail when a story is in-progress in list-format sprint-status.yaml
[ ] validate-split returns status: pass when no story is in-progress in list-format sprint-status.yaml
[ ] All existing 87 tests still pass
```
