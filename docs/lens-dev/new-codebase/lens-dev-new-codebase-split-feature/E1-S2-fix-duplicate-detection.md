---
feature: lens-dev-new-codebase-split-feature
epic: 1
story_id: E1-S2
title: Fix duplicate detection to check feature-index.yaml
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

# E1-S2 — Fix duplicate detection to check feature-index.yaml

## Context

The current `cmd_create_split_feature` checks for duplicates by testing whether
`new_feature_yaml_path.exists()` (the feature.yaml file on disk). Per tech-plan §2.4
(BS-3): "Before writing any artifact, check that `new-feature-id` does not already
exist in `feature-index.yaml`. If it does, exit 1 with a clear duplicate-feature error."

The feature-index.yaml is the publication point — a feature is considered discoverable
only when its entry exists in the index. The current filesystem check would miss a case
where the feature directory was cleaned up but the feature-index.yaml entry was not
(or vice versa).

**File:** `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/scripts/split-feature-ops.py`

**Old-codebase reference:** `TargetProjects/lens-dev/old-codebase/lens.core.src/`

## Implementation Steps

### 1. Add a helper to check feature-index.yaml for an existing entry

Add or update a helper function to read the feature-index.yaml and check if a
feature ID already exists:

```python
def feature_id_in_index(governance_repo: str, feature_id: str) -> bool:
    """Return True if feature_id is already registered in feature-index.yaml."""
    index_path = get_feature_index_path(governance_repo)
    if not index_path.exists():
        return False
    try:
        content = index_path.read_text(encoding="utf-8")
        data = yaml.safe_load(content)
        if not isinstance(data, dict):
            return False
        features = data.get("features", [])
        if isinstance(features, list):
            return any(
                (isinstance(f, dict) and f.get("featureId") == feature_id)
                for f in features
            )
    except (yaml.YAMLError, OSError):
        pass
    return False
```

### 2. Replace the file-existence check with the index check

In `cmd_create_split_feature`, replace:

```python
# Before (current):
if new_feature_yaml_path.exists():
    return {"status": "fail", "error": f"Feature already exists: {new_feature_yaml_path}"}
```

With:

```python
# After:
if feature_id_in_index(governance_repo, args.new_feature_id):
    return {
        "status": "fail",
        "error": (
            f"Duplicate feature: '{args.new_feature_id}' already exists in feature-index.yaml. "
            "Remove the existing entry or use a different feature ID."
        ),
    }
```

### 3. Verify the check fires before any directory or file creation

Ensure the index check is the very first validation after identifier validation
and before any `mkdir` or file write calls.

### 4. Verify existing tests still pass

```
uv run tests/test-split-feature-ops.py
```
All 87 existing tests must still pass. The existing `test_create_split_feature_duplicate_fails`
creates a feature (which writes a feature-index.yaml entry) and then tries to create
it again — this should continue to fail.

## Acceptance Criteria Checklist

```
[ ] In cmd_create_split_feature, duplicate check reads feature-index.yaml before mkdir
[ ] Duplicate check fires before any directory or file is created
[ ] Error message references "duplicate feature" or "feature-index.yaml"
[ ] When feature-index.yaml does not exist, no error is raised (new repo case)
[ ] All existing 87 tests still pass
[ ] Second create-split-feature call with same new-feature-id returns status: fail
```
