---
feature: lens-dev-new-codebase-discover
doc_type: sprint-plan
status: approved
goal: "Single-sprint delivery of the discover command rewrite with full regression coverage and architecture isolation audit"
key_decisions:
  - One sprint; Story 5.4 scope from baseline is fully captured here (8 stories: 5.4.1–5.4.7 + 5.4.9)
  - BMB-first authoring applies to all lens-work file changes
  - Regression test pass is a hard gate before dev-complete milestone
  - Pre-assessment gate (Story 5.4.1 first AC) is required before coding begins on Story 5.4.2
  - No-remote edge case deferred to follow-on feature (OQ-FP2 — E)
  - Integration test (Story 5.4.9) is a hard dev-complete gate (OQ-FP3 — A)
depends_on: [business-plan, tech-plan]
blocks: []
updated_at: 2026-04-28T00:00:00Z
---

# Sprint Plan — Discover Command

**Feature:** `lens-dev-new-codebase-discover`
**Author:** CrisWeber
**Date:** 2026-04-28
**Sprint:** 1 of 1

---

## Sprint Goal

Deliver the `discover` command rewrite to `dev-complete` status within a single focused sprint. The sprint is complete when all regression tests pass, the SKILL.md behavioral spec covers all paths (interactive, headless, dry-run, no-op, conditional auto-commit), and an architecture review confirms the auto-commit exception is isolated to this command only.

---

## Epic and Story Map

### Epic 5 — Discover Command (split from baseline Epic 5.4)

| Story | Title | Status | Priority | Notes |
|---|---|---|---|---|
| 5.4.1 | Finalize SKILL.md behavioral spec | not-started | P0 | Review and complete the existing SKILL.md: auto-commit section, headless, dry-run, no-op, config resolution |
| 5.4.2 | Patch SKILL.md: conditional auto-commit guard | not-started | P0 | Add pre/post hash comparison guard in SKILL.md to ensure no empty commits |
| 5.4.3 | Patch discover-ops.py: path resolution via resolve() | not-started | P1 | Verify all path comparisons use Path.resolve() for Windows safety |
| 5.4.4 | Extend test suite: missing-from-disk and add-entry tests | not-started | P0 | T3–T5 from tech-plan test matrix |
| 5.4.5 | Extend test suite: validate and no-op tests | not-started | P0 | T6–T8 from tech-plan test matrix |
| 5.4.6 | Verify prompt stub and release prompt chain | not-started | P1 | Ensure lens-discover.prompt.md stub exists and chains to release prompt |
| 5.4.7 | Architecture isolation audit | not-started | P0 | Confirm no other SKILL.md references governance-main direct commit; document in arch review note |
| 5.4.9 | Integration smoke test: full chain end-to-end | not-started | P0 | Hard dev-complete gate; added per OQ-FP3 resolution |

---

## Story Detail

### Story 5.4.1 — Finalize SKILL.md Behavioral Spec

**Goal:** The existing `bmad-lens-discover/SKILL.md` covers all behavioral paths without gaps.

**Acceptance Criteria:**
- [ ] Interactive mode flow is documented with sample output
- [ ] Headless mode (`--headless`/`-H`) skips all confirmation prompts
- [ ] Dry-run mode (`--dry-run`) makes no file mutations and no git commits
- [ ] No-op path reports `[discover] Nothing to do` without committing
- [ ] Auto-commit section is clearly labelled as `## Auto-Commit (Governance-Main Exception)`
- [ ] Config resolution priority is documented (`.lens/governance-setup.yaml` → `bmadconfig.yaml` fallback)
- [ ] Script subcommands used at each step are listed with their arguments

**Implementation:** Review existing SKILL.md against tech-plan spec; use EW/OW via BMB for any edits.

---

### Story 5.4.2 — Patch SKILL.md: Conditional Auto-Commit Guard

**Goal:** The skill's auto-commit logic fires only when `repo-inventory.yaml` has actually changed.

**Context:** The conditional auto-commit is implemented inline in the skill's orchestration section (not in `discover-ops.py`). The skill captures a SHA-256 hash of the inventory file before and after `add-entry` calls, and only executes `git add / commit / push` when the hashes differ. The guard must be in place before regression tests cover it.

**Acceptance Criteria:**
- [ ] Pre-mutation SHA-256 hash of `repo-inventory.yaml` is captured before any `add-entry` calls
- [ ] Post-mutation hash is captured after all `add-entry` calls complete
- [ ] `git commit` is called only when pre-hash ≠ post-hash
- [ ] No empty commit is produced on a no-op run
- [ ] SKILL.md documents the hash comparison approach

**Implementation:** Edit SKILL.md auto-commit section to add the pre/post hash guard; use EW via BMB.

---

### Story 5.4.3 — Patch discover-ops.py: Path Resolution via resolve()

**Goal:** All path comparisons in `discover-ops.py` use `Path.resolve()` to prevent Windows-specific mismatches.

**Acceptance Criteria:**
- [ ] The `_resolve_repo_path` function (or equivalent) calls `.resolve()` on both sides of any comparison
- [ ] Tests that use TargetProjects/-relative paths pass on both POSIX and Windows path formats
- [ ] No `str()` comparison of raw path strings for equality checks

**Implementation:** Review `discover-ops.py` path handling; patch via EW/OW if needed.

---

### Story 5.4.4 — Extend Test Suite: Missing-from-Disk and Add-Entry Tests

**Goal:** T3, T4, T5 from the tech-plan test matrix are passing.

**Test Cases:**

**T3 — `test_scan_reports_missing_from_disk`**
- Setup: inventory has one entry; no corresponding local repo exists
- Assert: `missing_from_disk` count = 1, `already_cloned` = 0, `untracked` = 0

**T4 — `test_add_entry_creates_new_entry`**
- Setup: empty inventory file
- Action: call `add-entry` with `--name`, `--remote-url`, `--local-path`
- Assert: `added: true`, inventory YAML contains one entry with correct fields

**T5 — `test_add_entry_is_idempotent_by_remote_url`**
- Setup: inventory with one entry
- Action: call `add-entry` with the same `remote_url`
- Assert: `added: false`, inventory file content unchanged (byte-for-byte)

**Acceptance Criteria:**
- [ ] All three tests pass with `uv run --with pytest`
- [ ] Tests use `tempfile.TemporaryDirectory` for isolation
- [ ] No tests require network access or external git remotes

---

### Story 5.4.5 — Extend Test Suite: Validate and No-Op Tests

**Goal:** T6, T7, T8 from the tech-plan test matrix are passing.

**Test Cases:**

**T6 — `test_validate_passes_well_formed_inventory`**
- Setup: inventory with two entries, each having `name` and `remote_url`
- Assert: `valid: true`, `errors` is empty list

**T7 — `test_validate_fails_missing_name`**
- Setup: inventory with one entry missing the `name` field
- Assert: `valid: false`, `errors` has one entry with correct `index` and `issue`

**T8 — `test_noop_run_produces_unchanged_hash`**
- Setup: disk repo and inventory entry are in sync (repo cloned, entry registered)
- Action: call `scan`; check that `missing_from_disk` and `untracked` are both empty
- Assert: No add-entry calls are triggered; a SHA-256 hash of inventory taken before and after confirms the file is unchanged

**Acceptance Criteria:**
- [ ] All three tests pass with `uv run --with pytest`
- [ ] T8 explicitly verifies the no-commit path by checking file hash equality

---

### Story 5.4.6 — Verify Prompt Stub and Release Prompt Chain

**Goal:** The full `discover` command chain is wired: stub → release prompt → SKILL.md.

**Chain:**
```
.github/prompts/lens-discover.prompt.md
  → lens.core/_bmad/lens-work/prompts/lens-discover.prompt.md
    → bmad-lens-discover/SKILL.md
```

**Acceptance Criteria:**
- [ ] `.github/prompts/lens-discover.prompt.md` exists as a standard Lens stub
- [ ] Stub runs `light-preflight.py` and redirects to the release prompt path
- [ ] Release prompt resolves config and delegates to SKILL.md
- [ ] A manual smoke test confirms the chain executes without errors

---

### Story 5.4.7 — Architecture Isolation Audit

**Goal:** Confirm that the governance-main direct-commit pattern exists only in `bmad-lens-discover/SKILL.md` and nowhere else.

**Method:**
- Search all SKILL.md files in `lens.core/_bmad/lens-work/skills/` for direct `git push` references outside of discover
- Search for any `git commit` references in SKILL files other than discover and `bmad-lens-git-orchestration` (which orchestrates its own commit flows per its domain)
- Document findings in an `arch-review-note.md` in the staged docs path

**Acceptance Criteria:**
- [ ] No SKILL.md other than `bmad-lens-discover/SKILL.md` contains a direct `git push` to governance `main` outside of the `publish-to-governance` CLI
- [ ] Findings documented in `docs/lens-dev/new-codebase/lens-dev-new-codebase-discover/arch-review-note.md`
- [ ] Any incidental direct-commit references found in other skills are surfaced as findings (not fixed here — this is audit, not remediation)

---

### Story 5.4.9 — Integration Smoke Test: Full Chain End-to-End

**Goal:** The full SKILL.md → `discover-ops.py` → governance-main commit chain is validated end-to-end.

> **Hard dev-complete gate** — this story must pass before the feature can reach `dev-complete`. Added per OQ-FP3 resolution.

**Acceptance Criteria:**
- [ ] Scenario 1 (`test_integration_auto_commit_fires_on_changed_inventory`) passes: untracked repo detected, `add-entry` called, commit+push to local bare remote, commit message is `[discover] Sync repo-inventory.yaml`
- [ ] Scenario 2 (`test_integration_no_commit_on_noop`) passes: disk and inventory in sync, no `add-entry` calls, no commit, HEAD SHA unchanged
- [ ] Both tests use local bare-git repos only (no network required)
- [ ] Tests live in `lens.core/_bmad/lens-work/skills/bmad-lens-discover/scripts/tests/test-discover-integration.py`

**Implementation:** Create `test-discover-integration.py`; untracked repo directories must be initialised with `git init` and a local `origin` remote so scan and `add-entry` work correctly.

- [ ] All 8 stories are `dev-complete`
- [ ] Full test suite passes: `uv run --with pytest pytest lens.core/_bmad/lens-work/skills/bmad-lens-discover/scripts/tests/ -q`
- [ ] SKILL.md covers all modes with no gaps
- [ ] Architecture isolation audit is documented
- [ ] All BMB-first authoring rules followed for lens-work file changes
- [ ] Control repo plan branch (`lens-dev-new-codebase-discover-plan`) is up to date

---

## Story Sequencing

```
5.4.1 (SKILL.md finalize) ──┐
5.4.2 (conditional commit) ──┤──→ 5.4.5 (no-op test depends on T8 guard)
5.4.3 (path resolve) ────────┤──→ 5.4.4 (path tests depend on resolve fix)
                              ├──→ 5.4.9 (integration smoke — depends on 5.4.1 + 5.4.2, hard dev-complete gate)
                              └──→ 5.4.6 (prompt chain can be verified any time)
                              └──→ 5.4.7 (audit is independent, run last)
```

Stories 5.4.1–5.4.3 can be worked in parallel. Tests (5.4.4, 5.4.5) depend on script patches completing. Prompt chain (5.4.6) and architecture audit (5.4.7) are independent and can run after 5.4.1 is settled.

---

## Velocity Notes

This is a small, tightly scoped feature split from a larger baseline. All core implementation (SKILL.md and discover-ops.py) already exists — the sprint focuses on completeness review, targeted patches, and regression test authoring. Estimated effort is 2–3 focused sessions.
