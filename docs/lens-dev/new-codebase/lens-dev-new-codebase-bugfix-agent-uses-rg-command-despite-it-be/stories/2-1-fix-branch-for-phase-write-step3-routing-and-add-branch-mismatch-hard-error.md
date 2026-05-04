# Story 2.1: Fix branch_for_phase_write Step3 Routing and Add Branch-Mismatch Hard Error

Status: ready-for-dev

## Story

As a **Lens agent**,  
I want `commit-artifacts --phase finalizeplan --phase-step step3` to route to the `{featureId}` branch and emit a structured hard error on any branch mismatch,  
so that downstream bundles are committed to the correct branch and mismatches are immediately detectable.

## Acceptance Criteria

1. **Given** `branch_for_phase_write("feat-abc", "finalizeplan", "step3")` is called, **when** the function executes, **then** it returns `("feat-abc", "finalizeplan_step_3_to_feature")`.

2. **And** `branch_for_phase_write("feat-abc", "finalizeplan", "step1")` still returns `("feat-abc-plan", ...)` — no regression.

3. **And** `branch_for_phase_write("feat-abc", "finalizeplan", "step2")` returns the existing step2 value — no regression.

4. **Given** `commit-artifacts` is called and `expected_branch != current_branch`, **when** the mismatch is detected, **then** the command exits non-zero and emits:
```json
{
  "status": "error",
  "error": "branch_mismatch",
  "current_branch": "<current>",
  "expected_branch": "<expected>",
  "detail": "Phase '<phase>' step '<step>' requires branch '<expected>'. Currently on '<current>'. Checkout '<expected>' before committing."
}
```

5. **And** no `--allow-branch-mismatch` flag is added; the mismatch is always a hard error.

6. **And** unit tests cover: step3 routing, step1/step2 no-regression, and branch-mismatch hard error with correct JSON fields.

## Tasks / Subtasks

- [ ] Task 1: Fix `branch_for_phase_write()` step3 routing (AC: #1, #2, #3)
  - [ ] Locate the step3 routing block (currently: `return f"{feature_id}-dev", "finalizeplan_step_3_to_dev"`)
  - [ ] Change to: `return feature_id, "finalizeplan_step_3_to_feature"`
  - [ ] Verify step1 and step2 routing is unchanged
- [ ] Task 2: Update branch-mismatch error path (AC: #4, #5)
  - [ ] Locate the branch comparison block in `commit_artifacts`
  - [ ] Replace generic error with structured JSON (see Dev Notes)
  - [ ] Ensure exit code is non-zero; do NOT add `--allow-branch-mismatch` flag
- [ ] Task 3: Write unit tests (AC: #6)
  - [ ] Test: `branch_for_phase_write("feat-abc", "finalizeplan", "step3")` returns `("feat-abc", "finalizeplan_step_3_to_feature")`
  - [ ] Test: step1/step2 routing regression guard
  - [ ] Test: branch mismatch emits correct JSON with all required fields

## Dev Notes

**Target file:** `TargetProjects/lens-dev/new-codebase/lens.core.src/skills/lens-git-orchestration/scripts/git-orchestration-ops.py`

**Step3 routing fix (from tech-plan B3, line ~199):**
```python
# BEFORE:
if normalized_step in {"step3", "3", "finalizeplan-step3"}:
    return f"{feature_id}-dev", "finalizeplan_step_3_to_dev"

# AFTER:
if normalized_step in {"step3", "3", "finalizeplan-step3"}:
    return feature_id, "finalizeplan_step_3_to_feature"
```

**Branch-mismatch structured error (from tech-plan B6):**
```python
import json, sys

# In the branch comparison block:
print(json.dumps({
    "status": "error",
    "error": "branch_mismatch",
    "current_branch": current_branch,
    "expected_branch": expected_branch,
    "detail": f"Phase '{phase}' step '{step}' requires branch '{expected_branch}'. Currently on '{current_branch}'. Checkout '{expected_branch}' before committing."
}))
sys.exit(1)
```

**Critical constraint:** Do NOT add `--allow-branch-mismatch`. The adversarial review explicitly rejected this as a silent-bypass risk. This is always a hard error.

**Locate existing code:** Search for `branch_for_phase_write` and `branch_mismatch` or `current_branch` in the script to find the exact locations before editing.

### Project Structure Notes

- Source repo path: `TargetProjects/lens-dev/new-codebase/lens.core.src/`
- Script path: `skills/lens-git-orchestration/scripts/git-orchestration-ops.py`
- Two separate changes in the same file (routing table + error message)

### References

- [Source: docs/...tech-plan.md#B3] Step3 routing fix — exact before/after code
- [Source: docs/...tech-plan.md#B6] Branch-mismatch structured error shape and no-bypass rationale
- [Source: docs/...epics.md#Story 2.1] AC specifications

## Dev Agent Record

### Agent Model Used

_to be filled by dev agent_

### Debug Log References

### Completion Notes List

### File List

- `TargetProjects/lens-dev/new-codebase/lens.core.src/skills/lens-git-orchestration/scripts/git-orchestration-ops.py`
