---
feature: lens-dev-new-codebase-discover
story_id: "5.4.4"
doc_type: story
status: not-started
title: "Extend test suite: missing-from-disk and add-entry tests"
priority: P0
story_points: 3
epic: "Epic 5 — Discover Command Rewrite"
depends_on: ["5.4.3"]
blocks: []
updated_at: 2026-04-29T00:00:00Z
---

# Story 5.4.4 — Extend Test Suite: Missing-from-Disk and Add-Entry Tests

**Feature:** `lens-dev-new-codebase-discover`
**Epic:** 5 — Discover Command Rewrite
**Priority:** P0 | **Points:** 3 | **Status:** not-started

---

## Goal

T3, T4, T5 from the tech-plan test matrix are implemented and passing.

---

## Context

This story depends on Story 5.4.3 (path resolution fix) because the `scan` subcommand's path comparisons must be correct before tests that rely on them can pass reliably.

**Pre-work item (M4 — from adversarial review):** Consider adding a T-dry-run test case if feasible within the story points. If scoping out, note it explicitly.

---

## Test Cases

### T3 — `test_scan_reports_missing_from_disk`

```python
def test_scan_reports_missing_from_disk(tmp_path):
    """Inventory has one entry; no corresponding local repo exists on disk."""
    inventory = tmp_path / "repo-inventory.yaml"
    inventory.write_text("repos:\n  - name: my-repo\n    remote_url: https://github.com/org/my-repo\n    local_path: ./TargetProjects/my-repo\n")
    target_dir = tmp_path / "TargetProjects"
    target_dir.mkdir()
    # my-repo NOT cloned — intentionally absent
    result = run_discover_scan(inventory_path=inventory, target_path=target_dir)
    assert result["missing_from_disk"] == 1
    assert result["already_cloned"] == 0
    assert result["untracked"] == 0
```

### T4 — `test_add_entry_creates_new_entry`

```python
def test_add_entry_creates_new_entry(tmp_path):
    """add-entry with no existing inventory creates a new entry."""
    inventory = tmp_path / "repo-inventory.yaml"
    inventory.write_text("repos: []\n")
    result = run_add_entry(
        inventory_path=inventory,
        name="new-repo",
        remote_url="https://github.com/org/new-repo",
        local_path=str(tmp_path / "TargetProjects" / "new-repo"),
    )
    assert result["added"] is True
    content = yaml.safe_load(inventory.read_text())
    assert len(content["repos"]) == 1
    assert content["repos"][0]["name"] == "new-repo"
```

### T5 — `test_add_entry_is_idempotent_by_remote_url`

```python
def test_add_entry_is_idempotent_by_remote_url(tmp_path):
    """add-entry with same remote_url does not modify the inventory."""
    inventory = tmp_path / "repo-inventory.yaml"
    original = "repos:\n  - name: existing-repo\n    remote_url: https://github.com/org/existing-repo\n    local_path: ./TargetProjects/existing-repo\n"
    inventory.write_text(original)
    result = run_add_entry(
        inventory_path=inventory,
        name="existing-repo",
        remote_url="https://github.com/org/existing-repo",
        local_path="./TargetProjects/existing-repo",
    )
    assert result["added"] is False
    assert inventory.read_text() == original  # byte-for-byte unchanged
```

---

## Acceptance Criteria

- [ ] T3 `test_scan_reports_missing_from_disk` passes
- [ ] T4 `test_add_entry_creates_new_entry` passes
- [ ] T5 `test_add_entry_is_idempotent_by_remote_url` passes
- [ ] All tests use `tempfile.TemporaryDirectory` or `tmp_path` fixture for isolation
- [ ] No tests require network access or external git remotes
- [ ] Tests run with `uv run --with pytest`

---

## Implementation Notes

- File to edit/extend: `lens.core/_bmad/lens-work/skills/bmad-lens-discover/scripts/tests/test-discover-ops.py`
- Use BMB for lens-work file changes
- Helper functions `run_discover_scan` and `run_add_entry` should call the `discover-ops.py` script via subprocess or import the module functions directly

---

## Definition of Done

- [ ] T3, T4, T5 implemented and passing
- [ ] All 6 acceptance criteria checked off
- [ ] `uv run --with pytest pytest lens.core/_bmad/lens-work/skills/bmad-lens-discover/scripts/tests/ -q` exits 0
- [ ] Committed via BMB
