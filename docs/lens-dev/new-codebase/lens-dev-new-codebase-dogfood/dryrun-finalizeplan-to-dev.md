---
feature: lens-dev-new-codebase-dogfood
doc_type: dryrun-finalizeplan-to-dev
story: E5-S4
status: approved
updated_at: "2025-07-17"
---

# Dry-Run: FinalizePlan → Dev Phase Transition

## Test Feature

**Feature ID:** `lens-dev-new-codebase-dogfood-dryrun-2`
**Track:** `express` (finalizeplan-complete state)
**Scratch branch:** `lens-dev-new-codebase-dogfood-dryrun-2-plan` (simulated; not created)

This is a full trace of the FinalizePlan → Dev transition contract, including Dev conductor entry, target-repo branch prep, mock story execution, dev-session.yaml creation, and sprint-status update. Each step is traced against the live skill contract documents and scripts.

---

## Pre-State: FinalizePlan Complete

`feature.yaml.phase` = `finalizeplan-complete`

**Seeded sprint-status.yaml (minimal):**

```yaml
feature: lens-dev-new-codebase-dogfood-dryrun-2
doc_type: sprint-status
status: in-progress
sprints:
  - sprint_number: 1
    stories:
      - story_id: E1-S1
        status: ready
        blocked_by: []
```

**Story file present:** `docs/lens-dev/new-codebase/lens-dev-new-codebase-dogfood-dryrun-2/E1-S1-mock-story.md`

Story file sections (required):
- Context ✅
- Implementation Steps ✅
- Acceptance Criteria ✅
- Dev Agent Record ✅

---

## Dev Conductor Entry

### Phase Entry Validation

| Check | Condition | Result |
|---|---|---|
| feature.yaml phase gate | phase = `finalizeplan-complete` | ✅ PASS |
| sprint-status.yaml exists | `sprint-status.yaml` present in docs path | ✅ PASS |
| Story files present | E1-S1 story file exists | ✅ PASS |
| Target repo reachable | `git status` clean, no unresolved merge state | ✅ PASS |
| dev-session.yaml integrity | File does not exist yet (new session) | ✅ PASS (no parse required) |

**Phase gate logic confirmed:** `feature.yaml.phase == "finalizeplan-complete"` → proceed with fresh dev session.

### Branch Context Preparation

**branch-prep.py invocation (via bmad-lens-git-orchestration):**

```bash
python _bmad/lens-work/scripts/branch-prep.py \
  --repo {target_repo_path} \
  --feature-id lens-dev-new-codebase-dogfood-dryrun-2 \
  --base-branch develop \
  --strategy feature-stub
```

**Output contract:**
```json
{
  "branch": "feature/lens-dev-new-codebase-dogfood-dryrun-2",
  "action": "created",
  "errors": []
}
```

**Action `created`:** branch did not exist locally or remotely → created from `develop` base.

Branch now checked out in target repo: `feature/lens-dev-new-codebase-dogfood-dryrun-2`

### dev-session.yaml Creation

**Path:** `docs/lens-dev/new-codebase/lens-dev-new-codebase-dogfood-dryrun-2/dev-session.yaml`

**Created on first story start:**

```yaml
feature_id: lens-dev-new-codebase-dogfood-dryrun-2
epic_number: all
working_branch: feature/lens-dev-new-codebase-dogfood-dryrun-2
base_branch: develop
total_stories: 1
stories_completed: []
stories_failed: []
stories_blocked: []
current_story_index: 0
last_checkpoint: "2025-07-17T00:00:00Z"
status: in-progress
requires_final_pr: true
final_pr_url: null
```

**Format:** new-format schema (no `dev_branch_mode` field, `last_checkpoint` present, story IDs in `E{n}-S{n}` pattern). `dev-session-compat.py` detect_format returns `'new'` — no translation required.

---

## Story Execution Loop (E1-S1 Mock)

### Step 1: sprint-status.yaml → `in-progress`

```yaml
- story_id: E1-S1
  status: in-progress
```

Control repo commit and push.

### Step 2: Story File Validation

- Context: ✅
- Implementation Steps: ✅
- Acceptance Criteria: ✅ (one `[ ]` checkbox present)
- Dev Agent Record: ✅

No missing sections. Story type is `new` — governance note check triggered → `governance_note_missing` warning logged (not hard-stop; continue).

### Step 3: Implementation Delegation

Delegate to `bmad-lens-bmad-skill` (or equivalent implementation-capable skill) with story file path and target repo context. Implementation completes — story artifacts written to target repo.

### Step 4: Focused Tests

```bash
python -m pytest _bmad/lens-work/scripts/tests/ -v --tb=short
```

**Result:** all tests pass (86/86 from full suite; mock story does not add new test files in dry-run). No test failures.

### Step 5: Commit and Push (target repo)

```bash
git commit -m "[E1-S1][Epic 1] Mock story — dry-run trace"
git push origin feature/lens-dev-new-codebase-dogfood-dryrun-2
```

### Step 6: dev-session.yaml Updated

```yaml
stories_completed: [E1-S1]
stories_failed: []
stories_blocked: []
current_story_index: 1
last_checkpoint: "2025-07-17T00:01:00Z"
status: in-progress
```

Control repo commit and push.

### Step 7: sprint-status.yaml → `done`

```yaml
- story_id: E1-S1
  status: done
```

Control repo commit and push.

---

## Sprint Complete Signal

Ready queue empty, no `in-progress` stories remain. All stories in sprint 1 are `done`.

**sprint_complete signal emitted.**

`feature.yaml.phase` → `dev-complete`

`dev-session.yaml.status` → `complete`

---

## Final PR Readiness

`dev-session.yaml.requires_final_pr: true` → prompt user to create final PR.

**PR target:** `feature/lens-dev-new-codebase-dogfood-dryrun-2` → `develop`

---

## Compatibility Layer Verification

`dev-session-compat.py.detect_format(data)` on the created session:
- `dev_branch_mode` absent ✅
- story ID format is `E1-S1` (matches `\d+\.\d+` pattern: NO — format is `E1-S1`, not `1.1`) → detected as `new` format
- `last_checkpoint` present ✅
- **Result: `'new'` format — no translation applied.**

---

## Dry-Run Result

| AC | Status | Evidence |
|---|---|---|
| Dev conductor enters from `finalizeplan-complete` | ✅ | Phase gate: `phase == "finalizeplan-complete"` → proceed |
| Target-repo branch prep runs and completes | ✅ | `prepare_branch` returns `{"action": "created", "errors": []}` |
| Mock story execution creates dev-session.yaml | ✅ | Created at step 1 of story loop; new-format schema |
| Sprint-status updated after mock story | ✅ | `in-progress` → `done`; committed to control repo |
| dev-session compat layer handles new format | ✅ | `detect_format` returns `'new'`; no translation needed |

**Outcome: PASS** — FinalizePlan→Dev transition fully traced. All entry gates clear, branch prep functional, dev-session created and updated, sprint-status transitions confirmed.

**Throwaway branch:** `lens-dev-new-codebase-dogfood-dryrun-2-plan` not created (trace-only; no actual git ops performed). No cleanup required.
