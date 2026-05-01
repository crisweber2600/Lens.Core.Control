---
feature: lens-dev-new-codebase-split-feature
epic: 3
story_id: E3-S1
title: Add missing test cases for normalization, list format, and duplicate detection
type: new
points: 3
status: not-started
phase: dev
updated_at: '2026-05-01T00:00:00Z'
depends_on: [E1-S1, E1-S2, E1-S3]
blocks: [E3-S2]
target_repo: lens.core.src
target_branch: develop
---

# E3-S1 — Add missing test cases for normalization, list format, and duplicate detection

## Context

The existing test suite (87 tests) covers the core behavior but is missing test cases for
three new-spec requirements from the adversarial review. These tests cannot be written
until E1-S1, E1-S2, and E1-S3 are implemented (the behavior they test does not exist yet).

**File:** `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/scripts/tests/test-split-feature-ops.py`

Missing test classes (per tech-plan §6):
1. Status delimiter normalization — `in_progress`, `IN_PROGRESS`, `in space`
2. Sprint plan list-format — sprint-status.yaml `stories: [{id, status}]` format
3. Feature-index duplicate detection — second run fails via index check, not file check
4. Sprint plan format fallback — unrecognized format falls back to story-file frontmatter

## Implementation Steps

### 1. Add test_validate_split_in_progress_underscore

```python
def test_validate_split_in_progress_underscore():
    """validate-split blocks stories with in_progress (underscore) status."""
    with tempfile.NamedTemporaryFile(mode="w", suffix=".yaml", delete=False) as f:
        f.write("stories:\n  story-wip: in_progress\n  story-ok: pending\n")
        sprint_plan_path = f.name
    try:
        result, code = run([
            "validate-split",
            "--sprint-plan-file", sprint_plan_path,
            "--story-ids", "story-wip,story-ok",
        ])
        assert_eq("underscore status blocked", result["status"], "fail")
        assert_true("story-wip in blockers", "story-wip" in result["blockers"])
        assert_eq("story-ok eligible", result["eligible"], ["story-ok"])
    finally:
        os.unlink(sprint_plan_path)
```

### 2. Add test_validate_split_in_progress_uppercase

```python
def test_validate_split_in_progress_uppercase():
    """validate-split blocks stories with IN_PROGRESS (uppercase) status."""
    # Same structure as above with status: IN_PROGRESS
    ...
```

### 3. Add test_validate_split_in_progress_with_space

```python
def test_validate_split_in_progress_with_space():
    """validate-split blocks stories with 'in progress' (space) status."""
    ...
```

### 4. Add test_validate_split_in_progress_underscore_in_move_stories

```python
def test_move_stories_blocks_in_progress_underscore():
    """move-stories blocks stories with in_progress (underscore) status."""
    # Create source feature with story file that has status: in_progress
    ...
```

### 5. Add test_validate_split_list_format_sprint_plan

```python
def test_validate_split_list_format_sprint_plan():
    """validate-split handles sprint-status.yaml list format."""
    with tempfile.NamedTemporaryFile(mode="w", suffix=".yaml", delete=False) as f:
        f.write(
            "sprint_number: 1\n"
            "feature: test-feature\n"
            "status: not-started\n"
            "stories:\n"
            "  - id: story-wip\n"
            "    status: in-progress\n"
            "    title: Work In Progress\n"
            "  - id: story-ok\n"
            "    status: pending\n"
            "    title: Pending\n"
        )
        sprint_plan_path = f.name
    try:
        result, code = run([
            "validate-split",
            "--sprint-plan-file", sprint_plan_path,
            "--story-ids", "story-wip,story-ok",
        ])
        assert_eq("list format blocks wip", result["status"], "fail")
        assert_true("story-wip blocked", "story-wip" in result["blockers"])
        assert_eq("story-ok eligible", result["eligible"], ["story-ok"])
    finally:
        os.unlink(sprint_plan_path)
```

### 6. Add test_validate_split_list_format_all_eligible

```python
def test_validate_split_list_format_all_eligible():
    """validate-split list format with all eligible stories returns pass."""
    ...
```

### 7. Add test_create_split_feature_duplicate_in_index

```python
def test_create_split_feature_duplicate_in_index():
    """create-split-feature fails when new feature-id exists in feature-index.yaml."""
    with tempfile.TemporaryDirectory() as tmp:
        args = [...]  # same as before
        run(args)  # first create — writes feature-index.yaml entry
        # Now remove the feature directory but keep feature-index.yaml
        import shutil
        shutil.rmtree(Path(tmp) / "features" / "platform" / "identity" / "auth-dup")
        result, code = run(args)  # second create — should fail from index check
        assert_eq("index duplicate fail", result["status"], "fail")
        assert_true("error mentions duplicate", "duplicate" in result.get("error", "").lower()
                    or "feature-index" in result.get("error", "").lower())
        assert_eq("exit code 1", code, 1)
```

### 8. Add test_validate_split_format_fallback

```python
def test_validate_split_format_fallback():
    """Unrecognized sprint plan format falls back to story-file frontmatter."""
    # Create sprint plan with unrecognized content
    # Create story file with status in frontmatter
    # Run validate-split and confirm story frontmatter status is used
    ...
```

### 9. Run full suite after additions

```
uv run tests/test-split-feature-ops.py
```
All tests (original 87 + new additions) must pass.

## Acceptance Criteria Checklist

```
[ ] test_validate_split_in_progress_underscore added and passes
[ ] test_validate_split_in_progress_uppercase added and passes
[ ] test_validate_split_in_progress_with_space added and passes
[ ] test_move_stories_blocks_in_progress_underscore added and passes
[ ] test_validate_split_list_format_sprint_plan added and passes
[ ] test_validate_split_list_format_all_eligible added and passes
[ ] test_create_split_feature_duplicate_in_index added and passes
[ ] test_validate_split_format_fallback added and passes
[ ] All existing 87 tests still pass
[ ] Total test count documented in completion notes (target ≥ 95)
```
