---
feature: lens-dev-new-codebase-discover
story_id: "5.4.5"
doc_type: story
status: not-started
title: "Extend test suite: validate and no-op tests"
priority: P0
story_points: 3
epic: "Epic 5 — Discover Command Rewrite"
depends_on: ["5.4.1", "5.4.2"]
blocks: []
updated_at: 2026-04-29T00:00:00Z
---

# Story 5.4.5 — Extend Test Suite: Validate and No-Op Tests

**Feature:** `lens-dev-new-codebase-discover`
**Epic:** 5 — Discover Command Rewrite
**Priority:** P0 | **Points:** 3 | **Status:** not-started

---

## Goal

T6, T7, T8 from the tech-plan test matrix are implemented and passing.

---

## Context

This story depends on:
- Story 5.4.1 (SKILL.md spec) — T8 requires the no-op path to be correctly specified before testing it
- Story 5.4.2 (hash guard) — T8 verifies the no-commit behavior by checking file hash equality, which depends on the hash guard being in place

**Pre-work item (M5 — from adversarial review):** T8 must explicitly verify the no-commit path by checking file hash equality, not just checking that `scan` returned empty lists. The test must confirm the file was not written to disk.

---

## Test Cases

### T6 — `test_validate_passes_well_formed_inventory`

```python
def test_validate_passes_well_formed_inventory(tmp_path):
    """Inventory with two well-formed entries validates successfully."""
    inventory = tmp_path / "repo-inventory.yaml"
    inventory.write_text(
        "repos:\n"
        "  - name: repo-a\n    remote_url: https://github.com/org/repo-a\n"
        "  - name: repo-b\n    remote_url: https://github.com/org/repo-b\n"
    )
    result = run_validate(inventory_path=inventory)
    assert result["valid"] is True
    assert result["errors"] == []
```

### T7 — `test_validate_fails_missing_name`

```python
def test_validate_fails_missing_name(tmp_path):
    """Inventory entry missing 'name' field fails validation with correct error."""
    inventory = tmp_path / "repo-inventory.yaml"
    inventory.write_text(
        "repos:\n"
        "  - remote_url: https://github.com/org/repo-missing-name\n"
    )
    result = run_validate(inventory_path=inventory)
    assert result["valid"] is False
    assert len(result["errors"]) == 1
    assert result["errors"][0]["index"] == 0
    assert "name" in result["errors"][0]["issue"].lower()
```

### T8 — `test_noop_run_produces_unchanged_hash`

```python
def test_noop_run_produces_unchanged_hash(tmp_path):
    """When disk and inventory are in sync, scan returns empty lists and inventory is unchanged."""
    target_dir = tmp_path / "TargetProjects"
    target_dir.mkdir()
    repo_dir = target_dir / "synced-repo"
    repo_dir.mkdir()  # repo exists on disk
    inventory = tmp_path / "repo-inventory.yaml"
    inventory.write_text(
        "repos:\n"
        "  - name: synced-repo\n"
        "    remote_url: https://github.com/org/synced-repo\n"
        f"    local_path: {str(repo_dir)}\n"
    )
    hash_before = hashlib.sha256(inventory.read_bytes()).hexdigest()
    result = run_discover_scan(inventory_path=inventory, target_path=target_dir)
    hash_after = hashlib.sha256(inventory.read_bytes()).hexdigest()
    assert result["missing_from_disk"] == 0
    assert result["untracked"] == 0
    assert hash_before == hash_after, "No-op run must not modify the inventory file"
```

---

## Acceptance Criteria

- [ ] T6 `test_validate_passes_well_formed_inventory` passes
- [ ] T7 `test_validate_fails_missing_name` passes
- [ ] T8 `test_noop_run_produces_unchanged_hash` passes
- [ ] T8 explicitly verifies file hash equality (not just empty result lists)
- [ ] All tests use `tmp_path` fixture for isolation; no network access required
- [ ] Tests run with `uv run --with pytest`

---

## Implementation Notes

- File to edit/extend: `lens.core/_bmad/lens-work/skills/bmad-lens-discover/scripts/tests/test-discover-ops.py`
- Import `hashlib` in the test file for T8 hash comparison
- Use BMB for lens-work file changes

---

## Definition of Done

- [ ] T6, T7, T8 implemented and passing
- [ ] T8 uses hash equality check (not just result assertion)
- [ ] All 6 acceptance criteria checked off
- [ ] `uv run --with pytest pytest lens.core/_bmad/lens-work/skills/bmad-lens-discover/scripts/tests/ -q` exits 0
- [ ] Committed via BMB
