---
feature: lens-dev-new-codebase-split-feature
doc_type: epics
status: approved
goal: "Deliver a clean-room rewrite of the split-feature command as a thin conductor with validate-first semantics, status normalization, feature-index duplicate detection, and dual story-file format support."
key_decisions:
  - Script fixes target three spec gaps: status normalization (BS-1), feature-index duplicate detection (BS-3), and stories list-format parsing
  - SKILL.md rewrite uses thin-conductor pattern — all governance mutations delegated to script; SKILL.md has no direct file writes
  - Test suite additions close all 10 test classes from tech-plan §6
  - Sprint 1 covers all script + conductor implementation; Sprint 2 covers test suite + verification + handoff
open_questions: []
depends_on:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-split-feature/tech-plan.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-split-feature/business-plan.md
blocks: []
updated_at: '2026-05-01T00:00:00Z'
---

# Epics — Rewrite split-feature Command

## Epic 1 — Script Gap Resolution

**Goal:** Close the three implementation gaps in `split-feature-ops.py` that prevent full
compliance with the tech-plan spec. The current old-codebase script lacks status
normalization, checks feature.yaml existence for duplicate detection instead of
feature-index.yaml, and does not handle the list-format `stories:` field used in
this repo's sprint-status.yaml files.

**Scope:**
- **E1-S1 — Status normalization:** Add normalization to `cmd_validate_split` and
  `cmd_move_stories` so that `in_progress`, `IN_PROGRESS`, and `in progress` all trigger
  the in-progress hard-stop identically to `in-progress`. The normalization must fire
  before any status comparison, not only at the presentation layer.
- **E1-S2 — Feature-index duplicate detection:** In `cmd_create_split_feature`,
  replace the current `new_feature_yaml_path.exists()` check with an explicit read of
  `feature-index.yaml`. If the `new-feature-id` already appears as an entry, exit 1
  with a clear duplicate-feature error before creating any directory or file. This is
  the BS-3 pre-condition check from tech-plan §2.4.
- **E1-S3 — Sprint plan list-format parsing:** Extend `_extract_statuses_from_yaml_str`
  to handle the list-format `stories:` field (e.g., `- id: E1-S1\n  status: not-started`)
  that sprint-status.yaml files in this repo use. The canonical YAML list format should
  be tried alongside the existing dict-format handler; if the list format matches, return
  a dict mapping story IDs to their status values.

**Exit Criteria:**
- `cmd_validate_split` blocks `in_progress`, `IN_PROGRESS`, and `in progress` stories
- `cmd_create_split_feature` exits 1 when new-feature-id exists in feature-index.yaml
  before any directory or file is created
- `parse_sprint_plan` / `_extract_statuses_from_yaml_str` handles list-format stories
  from sprint-status.yaml files
- All existing 87 tests continue to pass

---

## Epic 2 — Conductor Rewrite

**Goal:** Rewrite `SKILL.md` as a true thin-conductor following the baseline architecture
contract, and verify that the prompt files are correct.

**Scope:**
- **E2-S1 — SKILL.md rewrite (BMB-first):** Rewrite
  `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/SKILL.md` to follow the
  thin-conductor pattern defined in tech-plan §2.1–§2.2:
  - All governance mutations delegated to `split-feature-ops.py`; no inline file writes
  - On Activation reads config from `bmadconfig.yaml` + `config.user.yaml`
  - Execution flow follows §2.1: load → validate → confirm → dry-run → execute → report
  - Post-move scan note (L3): after `move-stories` completes, direct the Lens agent to
    scan moved story files and report any whose `feature:` frontmatter still references
    the source feature ID
  - Behavioral reference path (BS-2): note that old-codebase reference is at
    `TargetProjects/lens-dev/old-codebase/lens.core.src/` and discovery feature is
    `lens-dev-old-codebase-discovery`
  - Keep Script Reference, Capabilities, Integration Points, and reference doc links
- **E2-S2 — Prompt file verification:** Confirm the following are correct:
  - `.github/prompts/lens-split-feature.prompt.md` — uses shared stub pattern with
    `light-preflight.py` preflight
  - `lens.core/_bmad/lens-work/prompts/lens-split-feature.prompt.md` — thin redirect
    to SKILL.md

**Exit Criteria:**
- SKILL.md follows thin-conductor pattern with no inline governance writes
- Post-move scan note present in SKILL.md
- Behavioral reference path note present in SKILL.md
- Both prompt files verified correct; no changes needed or corrections applied

---

## Epic 3 — Test Suite Completion and Verification

**Goal:** Add the missing test cases to close all 10 test class categories from
tech-plan §6, then run the full suite to confirm no regressions.

**Scope:**
- **E3-S1 — Add missing test cases:**
  - Status delimiter normalization: `in_progress` (underscore), `IN_PROGRESS`
    (uppercase), `in progress` (space with space) all trigger in-progress hard-stop
  - Sprint plan list-format: sprint-status.yaml with `stories: - id: …` list format
    correctly identifies in-progress stories
  - Feature-index duplicate detection: second `create-split-feature` run fails
    because the feature-index.yaml entry already exists (not because feature.yaml exists)
  - Sprint plan format fallback: unrecognized format falls back to story-file
    frontmatter without error
- **E3-S2 — Run full test suite:** Run `uv run tests/test-split-feature-ops.py` and
  confirm all test cases pass with zero failures. Document test count in completion notes.

**Exit Criteria:**
- All 10 tech-plan §6 test classes have at least one covering test case
- Full test suite passes with zero failures
- Test count documented in story completion notes

---

## Epic 4 — Handoff and Final PR

**Goal:** Verify module discovery registration and open the final feature PR.

**Scope:**
- **E4-S1 — Module discovery and target_repos:** Confirm `split-feature` is registered
  in `lens.core/_bmad/lens-work/module.yaml` or `module-help.csv` with the correct
  prompt entry. Update `feature.yaml.target_repos` to include `lens.core.src` (the
  target source repo for this rewrite).
- **E4-S2 — Final PR and phase update:** Commit all completed artifacts, open the
  `lens-dev-new-codebase-split-feature` → `main` PR, and update `feature.yaml` phase
  to `finalizeplan-complete`.

**Exit Criteria:**
- Module discovery registration confirmed or corrected
- `feature.yaml.target_repos` includes `lens.core.src`
- Final PR open against `main`
- `feature.yaml` phase = `finalizeplan-complete`
