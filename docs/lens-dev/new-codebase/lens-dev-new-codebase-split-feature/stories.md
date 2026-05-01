---
feature: lens-dev-new-codebase-split-feature
doc_type: stories
status: approved
goal: "Story list with acceptance criteria for the split-feature command rewrite: script gap fixes, conductor rewrite, test completion, and handoff."
key_decisions:
  - Stories align to four epics: Script Gap Resolution, Conductor Rewrite, Test Suite Completion, Handoff
  - Script stories (E1) are independent and can run in parallel; SKILL.md story (E2-S1) may follow after E1 is complete
  - Story files use direct implementation channel (not BMB-gated) for script and test changes; SKILL.md uses BMB-first
open_questions: []
depends_on:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-split-feature/tech-plan.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-split-feature/business-plan.md
blocks: []
updated_at: '2026-05-01T00:00:00Z'
---

# Stories — Rewrite split-feature Command

## Epic 1 — Script Gap Resolution

---

### E1-S1 — Add status normalization to validate-split and move-stories

**Type:** [fix]  
**Points:** 2

**Story:** As a Lens user running validate-split, I want in-progress stories to be
blocked regardless of whether the status uses hyphen (`in-progress`), underscore
(`in_progress`), uppercase (`IN_PROGRESS`), or space (`in progress`) so that old-codebase
sprint files do not silently pass the hard gate.

**Context:**
- File: `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/scripts/split-feature-ops.py`
- Current: `IN_PROGRESS_STATUS = "in-progress"` compared directly without normalization
- Spec reference: tech-plan §2.3 (BS-1), `IN_PROGRESS_STATUS` normalization requirement

**Acceptance Criteria:**
- [ ] Add a `normalize_status(s: str) -> str` helper that lower-cases and replaces
  underscores and spaces with hyphens before comparison
- [ ] Apply normalization in `cmd_validate_split` before the `if status == IN_PROGRESS_STATUS` check
- [ ] Apply normalization in `cmd_move_stories` before the in-progress check
- [ ] `validate-split` returns `"status": "fail"` when a story has `in_progress` (underscore) status
- [ ] `validate-split` returns `"status": "fail"` when a story has `IN_PROGRESS` (uppercase) status
- [ ] `validate-split` returns `"status": "fail"` when a story has `in progress` (with space) status
- [ ] All existing 87 tests still pass

---

### E1-S2 — Fix duplicate detection to check feature-index.yaml

**Type:** [fix]  
**Points:** 2

**Story:** As a Lens operator running create-split-feature, I want the duplicate-feature
check to read feature-index.yaml rather than checking for a feature.yaml file on disk, so
that a feature ID that was previously registered but whose directory was cleaned up still
triggers the duplicate guard.

**Context:**
- File: `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/scripts/split-feature-ops.py`
- Current: `if new_feature_yaml_path.exists(): return {status: fail, ...}` (checks file on disk)
- Spec reference: tech-plan §2.4 (BS-3): "Before writing any artifact, check that
  `new-feature-id` does not already exist in `feature-index.yaml`"

**Acceptance Criteria:**
- [ ] In `cmd_create_split_feature`, before creating any directory or writing any file:
  - Read `feature-index.yaml` (if it exists)
  - If an entry whose `featureId` matches `args.new_feature_id` is present, exit 1
    with error message including "duplicate feature" and the feature ID
- [ ] The check fires before `mkdir` is called (i.e., no side-effects before the guard)
- [ ] When feature-index.yaml does not exist yet, no error is raised (new repo case)
- [ ] The existing `test_create_split_feature_duplicate_fails` test continues to pass
  (behavior is preserved; only detection mechanism changes)
- [ ] A second `create-split-feature` call with the same new-feature-id returns
  `status: fail` with an error message referencing feature-index.yaml or "duplicate"

---

### E1-S3 — Add sprint-status.yaml list-format support to sprint plan parser

**Type:** [fix]  
**Points:** 2

**Story:** As a Lens developer running validate-split against a sprint-status.yaml
that uses the list format (`stories: - id: … status: …`), I want the parser to
correctly identify in-progress stories so the hard gate fires reliably.

**Context:**
- File: `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/scripts/split-feature-ops.py`
- Function: `_extract_statuses_from_yaml_str`
- Current: handles `stories: {dict}` format only, not `stories: [{id: …, status: …}]` list format
- Sprint-status.yaml files in this repo (e.g., `lens-dev-new-codebase-finalizeplan/sprint-status.yaml`)
  use the list format with `- id:` entries

**Acceptance Criteria:**
- [ ] `_extract_statuses_from_yaml_str` handles the list-format `stories:` field:
  `stories:\n  - id: E1-S1\n    status: in-progress` → `{"E1-S1": "in-progress"}`
- [ ] List-format handling is tried alongside existing dict-format logic (not replacing it)
- [ ] `parse_sprint_plan` correctly propagates list-format statuses to the caller
- [ ] `validate-split` returns `status: fail` when the sprint plan uses list format
  and a story has `in-progress` status
- [ ] `validate-split` returns `status: pass` when the sprint plan uses list format
  and no story has in-progress status
- [ ] All existing 87 tests still pass

---

## Epic 2 — Conductor Rewrite

---

### E2-S1 — Rewrite SKILL.md as thin conductor (BMB-first)

**Type:** [rewrite]  
**Points:** 3

**Story:** As a Lens maintainer, I want the split-feature SKILL.md to follow the thin-conductor
pattern from the baseline architecture contract so that the skill has no inline governance
writes and delegates all mutations to the script.

**Context:**
- File: `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/SKILL.md`
- Spec reference: tech-plan §2.1–§2.2 (thin conductor), §5 (L3 post-move scan, BS-2 behavioral reference)
- Implementation channel: BMB-first (use `bmad-module-builder` skill or equivalent)

**Acceptance Criteria:**
- [ ] SKILL.md contains the three-section execution flow from tech-plan §2.1:
  `load config → validate → confirm split plan → dry-run → execute → report`
- [ ] No inline file writes or direct governance mutations anywhere in SKILL.md
- [ ] All governance mutations explicitly delegated to `split-feature-ops.py` via the
  subcommand surface (validate-split, create-split-feature, move-stories)
- [ ] On Activation loads config from `bmadconfig.yaml` + `config.user.yaml`
- [ ] Post-move scan note present (L3): after `move-stories` completes, the skill
  directs the agent to scan moved story files and report any with `feature:` frontmatter
  still pointing to the source feature ID; no automatic rewrite
- [ ] Behavioral reference path present (BS-2): old-codebase reference is documented as
  `TargetProjects/lens-dev/old-codebase/lens.core.src/` and discovery feature as
  `lens-dev-old-codebase-discovery`
- [ ] Script Reference section retained with all three subcommand examples
- [ ] Capabilities table retained (Validate Split, Split Scope, Split Stories → references)
- [ ] Integration Points table retained

---

### E2-S2 — Verify prompt files

**Type:** [confirm]  
**Points:** 1

**Story:** As a Lens module maintainer, I want to confirm that both prompt files for
split-feature use the correct pattern before the final PR is opened.

**Acceptance Criteria:**
- [ ] `.github/prompts/lens-split-feature.prompt.md` contains `light-preflight.py` preflight step
- [ ] `.github/prompts/lens-split-feature.prompt.md` redirects to
  `lens.core/_bmad/lens-work/prompts/lens-split-feature.prompt.md`
- [ ] `lens.core/_bmad/lens-work/prompts/lens-split-feature.prompt.md` redirects to
  `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/SKILL.md`
- [ ] If either file is incorrect, apply the fix and commit
- [ ] Document result (correct / corrected) in completion notes

---

## Epic 3 — Test Suite Completion and Verification

---

### E3-S1 — Add missing test cases for normalization, list format, and duplicate detection

**Type:** [new]  
**Points:** 3

**Story:** As a Lens maintainer, I want the test suite to cover all 10 test class
categories from tech-plan §6 so that the implementation is verifiably complete against
the spec.

**Context:**
- File: `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/scripts/tests/test-split-feature-ops.py`
- Missing coverage: delimiter normalization (BS-1), list-format sprint plan (E1-S3), and feature-index
  duplicate detection (BS-3)

**Acceptance Criteria:**
- [ ] Test: `in_progress` (underscore) status triggers in-progress hard-stop in validate-split
- [ ] Test: `IN_PROGRESS` (uppercase) status triggers in-progress hard-stop in validate-split
- [ ] Test: `in progress` (space) status triggers in-progress hard-stop in validate-split
- [ ] Test: `in_progress` (underscore) status blocks move-stories
- [ ] Test: sprint-status.yaml list-format (` stories: - id: X  status: in-progress`)
  causes validate-split to return `status: fail`
- [ ] Test: sprint-status.yaml list-format with eligible statuses returns `status: pass`
- [ ] Test: `create-split-feature` fails when new-feature-id already exists in
  feature-index.yaml (not just feature.yaml); confirm error references "duplicate" or
  feature-index.yaml concept
- [ ] Test: sprint plan format fallback — when sprint plan has unrecognized content,
  story-file frontmatter is used as fallback (no crash, no unexpected blocks)
- [ ] All existing 87 tests still pass after additions

---

### E3-S2 — Run full test suite and confirm all test classes pass

**Type:** [confirm]  
**Points:** 2

**Story:** As a Lens maintainer, I want to confirm the full test suite passes with zero
failures after all script fixes and test additions so the feature is ready for the
final PR.

**Acceptance Criteria:**
- [ ] Run `uv run tests/test-split-feature-ops.py` from the scripts directory
- [ ] All tests pass; zero failures
- [ ] Test count is ≥ 95 (87 existing + at least 8 new normalization/format/duplicate tests)
- [ ] Document final test count in completion notes

---

## Epic 4 — Handoff and Final PR

---

### E4-S1 — Verify module discovery registration and update target_repos

**Type:** [confirm]  
**Points:** 1

**Story:** As a Lens module maintainer, I want to confirm the split-feature command is
registered in the module discovery surface and that the feature.yaml target_repos field
is updated to include the target source repo.

**Acceptance Criteria:**
- [ ] Locate `lens.core/_bmad/lens-work/module.yaml` or `module-help.csv`
- [ ] Confirm `lens-split-feature` prompt is listed; add if missing
- [ ] Update `feature.yaml.target_repos` to include `lens.core.src` (target source repo
  for the rewrite)
- [ ] Document result (correct / corrected / updated) in completion notes

---

### E4-S2 — Commit all artifacts, open final PR, update feature.yaml phase

**Type:** [new]  
**Points:** 1

**Story:** As a Lens module maintainer, I want to open the final
`lens-dev-new-codebase-split-feature` → `main` PR and mark the feature
`finalizeplan-complete` so the dev phase can begin.

**Acceptance Criteria:**
- [ ] All modified files committed to `lens-dev-new-codebase-split-feature` branch
- [ ] Final PR opened against `main` in the control repo
- [ ] `feature.yaml` phase updated to `finalizeplan-complete`
- [ ] PR link documented in completion notes
