---
feature: lens-dev-new-codebase-discover
doc_type: stories
status: approved
goal: "Eight stories delivering the discover command rewrite: SKILL.md spec, script patches, regression tests, prompt chain, isolation audit, and integration smoke test"
key_decisions:
  - Story 5.4.9 added as hard dev-complete gate per OQ-FP3 resolution (A)
  - No-remote edge case deferred per OQ-FP2 resolution (E)
  - Config key stability test deferred to upgrade-feature TechPlan review per OQ-FP1 resolution (E)
depends_on: [epics, tech-plan, sprint-plan]
blocks: []
updated_at: 2026-04-29T00:00:00Z
---

# Stories — Discover Command

**Feature:** `lens-dev-new-codebase-discover`
**Author:** FinalizePlan bundle (lens-finalizeplan)
**Date:** 2026-04-29
**Epic:** Epic 5 — Discover Command Rewrite

---

## Story List

### Story 5.4.1 — Finalize SKILL.md Behavioral Spec

**Goal:** The existing `bmad-lens-discover/SKILL.md` covers all behavioral paths without gaps.

**Story Points:** 3

**Acceptance Criteria:**
- [ ] Interactive mode flow is documented with sample output
- [ ] Headless mode (`--headless`/`-H`) skips all confirmation prompts
- [ ] Dry-run mode (`--dry-run`) makes no file mutations and no git commits
- [ ] No-op path reports `[discover] Nothing to do` without committing
- [ ] Auto-commit section is clearly labelled as `## Auto-Commit (Governance-Main Exception)`
- [ ] Config resolution priority is documented (`.lens/governance-setup.yaml` → `bmadconfig.yaml` fallback)
- [ ] Script subcommands used at each step are listed with their arguments

**Known deferred gap:** No-remote edge case (OQ-FP2 deferred). SKILL.md may note that repos with no remote are currently out of scope.

**Implementation:** Review existing SKILL.md against tech-plan spec; use EW/OW via BMB for any edits. BMB-first for all lens-work file changes.

**Depends on:** Baseline SKILL.md stub exists
**Blocks:** 5.4.2 (conditional commit needs spec first), 5.4.5 (T8 depends on SKILL.md auto-commit spec), 5.4.9 (integration test needs full spec)

---

### Story 5.4.2 — Patch SKILL.md: Conditional Auto-Commit Guard

**Goal:** The skill's auto-commit logic fires only when `repo-inventory.yaml` has actually changed.

**Story Points:** 2

**Acceptance Criteria:**
- [ ] Pre-mutation SHA-256 hash of `repo-inventory.yaml` is captured before any `add-entry` calls
- [ ] Post-mutation hash is captured after all `add-entry` calls complete
- [ ] `git commit` is called only when pre-hash ≠ post-hash
- [ ] No empty commit is produced on a no-op run
- [ ] SKILL.md documents the hash comparison approach in the `## Auto-Commit (Governance-Main Exception)` section

**Note:** Hash comparison is implemented inline in the skill's orchestration section (not in `discover-ops.py`).

**Implementation:** Edit SKILL.md auto-commit section to add the pre/post hash guard; use EW via BMB.

**Depends on:** 5.4.1 (spec complete)
**Blocks:** 5.4.5 (T8 validates no-commit on identical hash), 5.4.9 (integration test validates auto-commit gate)

---

### Story 5.4.3 — Patch discover-ops.py: Path Resolution via resolve()

**Goal:** All path comparisons in `discover-ops.py` use `Path.resolve()` to prevent Windows-specific mismatches.

**Story Points:** 2

**Acceptance Criteria:**
- [ ] The `_resolve_repo_path` function (or equivalent) calls `.resolve()` on both sides of any comparison
- [ ] Tests that use TargetProjects/-relative paths pass on both POSIX and Windows path formats
- [ ] No `str()` comparison of raw path strings for equality checks

**Implementation:** Review `discover-ops.py` path handling; patch via EW/OW if needed. BMB-first for lens-work file changes.

**Depends on:** Baseline `discover-ops.py` stub exists
**Blocks:** 5.4.4 (path tests depend on resolve fix)

---

### Story 5.4.4 — Extend Test Suite: Missing-from-Disk and Add-Entry Tests

**Goal:** T3, T4, T5 from the tech-plan test matrix are passing.

**Story Points:** 3

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

**Depends on:** 5.4.3 (path resolution fix), baseline test file
**Blocks:** —

---

### Story 5.4.5 — Extend Test Suite: Validate and No-Op Tests

**Goal:** T6, T7, T8 from the tech-plan test matrix are passing.

**Story Points:** 3

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

**Depends on:** 5.4.1 (SKILL.md spec), 5.4.2 (hash guard documented)
**Blocks:** —

---

### Story 5.4.6 — Verify Prompt Stub and Release Prompt Chain

**Goal:** The full `discover` command chain is wired: stub → release prompt → SKILL.md.

**Story Points:** 1

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

**Depends on:** —
**Blocks:** —

---

### Story 5.4.7 — Architecture Isolation Audit

**Goal:** Confirm that the governance-main direct-commit pattern exists only in `bmad-lens-discover/SKILL.md` and nowhere else.

**Story Points:** 2

**Method:**
- Search all SKILL.md files in `lens.core/_bmad/lens-work/skills/` for direct `git push` references outside of discover
- Search for any `git commit` references in SKILL files other than discover and `bmad-lens-git-orchestration`
- Document findings in `arch-review-note.md` in the staged docs path

**Acceptance Criteria:**
- [ ] No SKILL.md other than `bmad-lens-discover/SKILL.md` contains a direct `git push` to governance `main` outside of the `publish-to-governance` CLI
- [ ] Findings documented in `docs/lens-dev/new-codebase/lens-dev-new-codebase-discover/arch-review-note.md`
- [ ] Any incidental direct-commit references found in other skills are surfaced as findings (not fixed here — audit only)

**Depends on:** —
**Blocks:** —

---

### Story 5.4.9 — Integration Smoke Test: Full Chain End-to-End

**Goal:** The full SKILL.md → `discover-ops.py` → governance-main commit chain is validated end-to-end. This is a **hard dev-complete gate** per OQ-FP3 resolution.

**Story Points:** 3

**Context:** Per OQ-FP3 (A — accept and implement), the auto-commit exception is a governance-critical behavior. Script-level tests alone cannot validate the full orchestration chain. A skill-level smoke test must verify that the full agent-orchestrated flow produces the correct git state in governance.

**Test Scenario: `test_integration_auto_commit_fires_on_changed_inventory`**
- Setup: governance repo clone with `repo-inventory.yaml` containing one entry; local TargetProjects/ directory with one untracked repo (not in inventory)
- Action: invoke discover skill orchestration sequence (scan → add-entry → hash check → conditional commit)
- Assert:
  - `add-entry` was called for the untracked repo
  - Post-mutation hash ≠ pre-mutation hash
  - A new commit exists on governance `main` HEAD with `repo-inventory.yaml` in the diff
  - Commit message follows the `[discover] Sync repo-inventory.yaml` pattern

**Test Scenario: `test_integration_no_commit_on_noop`**
- Setup: governance repo with inventory already in sync (no missing-from-disk, no untracked)
- Action: invoke discover skill orchestration sequence
- Assert:
  - No `add-entry` calls
  - Pre-hash = post-hash
  - No new commit on governance `main`

**Acceptance Criteria:**
- [ ] Both integration scenarios pass using a local bare-git test repository (no network required)
- [ ] Tests live in `lens.core/_bmad/lens-work/skills/bmad-lens-discover/scripts/tests/test-discover-integration.py`
- [ ] Included in the sprint DoD full-suite run

**Depends on:** 5.4.1 (SKILL.md spec complete), 5.4.2 (hash guard in place)
**Blocks:** — (hard gate for dev-complete milestone)
