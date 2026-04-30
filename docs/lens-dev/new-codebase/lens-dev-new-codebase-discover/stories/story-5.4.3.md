---
feature: lens-dev-new-codebase-discover
story_id: "5.4.3"
doc_type: story
status: not-started
title: "Patch discover-ops.py: path resolution via resolve()"
priority: P1
story_points: 2
epic: "Epic 5 — Discover Command Rewrite"
depends_on: []
blocks: ["5.4.4"]
updated_at: 2026-04-29T00:00:00Z
---

# Story 5.4.3 — Patch discover-ops.py: Path Resolution via resolve()

**Feature:** `lens-dev-new-codebase-discover`
**Epic:** 5 — Discover Command Rewrite
**Priority:** P1 | **Points:** 2 | **Status:** not-started

---

## Goal

All path comparisons in `discover-ops.py` use `Path.resolve()` to prevent Windows-specific path mismatches.

---

## Context

Python `Path` objects on Windows can produce different string representations for the same filesystem path depending on whether the path is relative or absolute, whether it contains drive letters, and whether it uses forward or backward slashes. Comparisons on raw path strings break silently. Using `Path.resolve()` normalises both sides before comparison, ensuring correctness on Windows and POSIX alike.

This story can proceed in parallel with Story 5.4.2.

---

## Acceptance Criteria

- [ ] The `_resolve_repo_path` function (or equivalent) calls `.resolve()` on both sides of any path comparison
- [ ] All path equality comparisons use `Path.resolve()` output, not raw `str()` comparisons
- [ ] Tests that use TargetProjects/-relative paths pass on both POSIX and Windows path formats
- [ ] No `str()` comparison of raw path strings for path equality checks anywhere in the scan/add-entry/validate flow

---

## Implementation Notes

- File to edit: `lens.core/_bmad/lens-work/skills/bmad-lens-discover/scripts/discover-ops.py`
- Use EW/OW via BMB for lens-work file changes
- Pattern to apply:
  ```python
  # Before (fragile):
  if str(local_path) == str(inventory_path):
  
  # After (safe):
  if Path(local_path).resolve() == Path(inventory_path).resolve():
  ```
- Scan the full file for any `str(path)` equality comparisons and convert them

---

## Definition of Done

- [ ] `discover-ops.py` patched with `Path.resolve()` on all comparison sites
- [ ] All 4 acceptance criteria checked off
- [ ] Committed via BMB
