---
feature: lens-dev-new-codebase-discover
story_id: "5.4.9"
doc_type: story
status: not-started
title: "Integration smoke test: full chain end-to-end"
priority: P0
story_points: 3
epic: "Epic 5 — Discover Command Rewrite"
depends_on: ["5.4.1", "5.4.2"]
blocks: []
gate: dev-complete
updated_at: 2026-04-29T00:00:00Z
---

# Story 5.4.9 — Integration Smoke Test: Full Chain End-to-End

**Feature:** `lens-dev-new-codebase-discover`
**Epic:** 5 — Discover Command Rewrite
**Priority:** P0 | **Points:** 3 | **Status:** not-started

> **Hard dev-complete gate** — this story must pass before the feature can reach `dev-complete`. Added per OQ-FP3 resolution (A — accept and implement as recommended).

---

## Goal

The full SKILL.md → `discover-ops.py` → governance-main commit chain is validated end-to-end in a controlled local test environment.

---

## Context

Script-level tests (T1–T8) validate `discover-ops.py` in isolation. They cannot validate that the SKILL.md orchestration section correctly invokes `discover-ops.py` AND correctly applies the hash guard AND correctly executes or skips the `git commit/push` sequence.

The auto-commit exception is the governance-critical distinguishing behavior of this command. A chain-level smoke test is required to have confidence that the full flow works as intended before dev-complete.

**Test environment constraint:** All tests use local bare-git repositories (no network access required). The tests initialize a local git repo to stand in for the governance remote.

---

## Test Scenarios

### Scenario 1: `test_integration_auto_commit_fires_on_changed_inventory`

**Setup:**
1. Create a local bare git repo (`governance-bare.git`) as the "governance remote"
2. Clone it to a working directory as the governance repo working copy
3. Write an initial `repo-inventory.yaml` with one registered repo entry
4. Create a `TargetProjects/` directory with one subdirectory that is NOT in the inventory (untracked repo)
5. Commit and push the initial inventory to the bare repo

**Action:** Invoke the discover skill orchestration sequence:
1. `scan` — identifies untracked repo
2. `add-entry` — adds it to inventory
3. Hash check — pre-hash ≠ post-hash (file was modified)
4. `git commit` + `git push` to governance bare remote

**Assertions:**
- [ ] `add-entry` was called for the untracked repo
- [ ] Post-mutation hash ≠ pre-mutation hash (inventory was modified)
- [ ] A new commit exists on the governance bare repo's `main` branch HEAD after the push
- [ ] The commit diff includes `repo-inventory.yaml`
- [ ] Commit message matches the `[discover] Sync repo-inventory.yaml` pattern

---

### Scenario 2: `test_integration_no_commit_on_noop`

**Setup:**
1. Same local bare git repo pattern as Scenario 1
2. Inventory already contains all repos that exist in `TargetProjects/`
3. No untracked repos; no missing-from-disk repos

**Action:** Invoke the discover skill orchestration sequence (same steps as Scenario 1).

**Assertions:**
- [ ] `scan` returns `missing_from_disk = 0` and `untracked = 0`
- [ ] No `add-entry` calls
- [ ] Pre-hash = post-hash (inventory file not modified)
- [ ] No new commit on the governance bare repo (HEAD SHA is unchanged)

---

## Acceptance Criteria

- [ ] Scenario 1 (`test_integration_auto_commit_fires_on_changed_inventory`) passes
- [ ] Scenario 2 (`test_integration_no_commit_on_noop`) passes
- [ ] Both tests use local bare-git repos (no network required)
- [ ] Tests live in `lens.core/_bmad/lens-work/skills/bmad-lens-discover/scripts/tests/test-discover-integration.py`
- [ ] Tests are included in the sprint DoD full-suite run: `uv run --with pytest pytest lens.core/_bmad/lens-work/skills/bmad-lens-discover/scripts/tests/ -q`
- [ ] Scenario 1 verifies commit message format explicitly

---

## Implementation Notes

- Create new test file: `lens.core/_bmad/lens-work/skills/bmad-lens-discover/scripts/tests/test-discover-integration.py`
- Use `git init --bare` + `git clone` pattern for local remote setup in `tmp_path`
- The test should invoke the discover skill orchestration as close to real execution as possible (calling the script sequence, not mocking it)
- Use BMB for lens-work file creation

---

## Definition of Done

- [ ] Both scenarios implemented and passing
- [ ] Test file at correct path
- [ ] All 6 acceptance criteria checked off
- [ ] `uv run --with pytest pytest lens.core/_bmad/lens-work/skills/bmad-lens-discover/scripts/tests/ -q` exits 0 (full suite)
- [ ] Committed via BMB
