---
feature: lens-dev-new-codebase-switch
story_id: SW-8
epic: EP-2
sprint: 2
title: Report Branch Checkout Result
estimate: S
status: not-started
blocked_by: [SW-6]
assignee: crisweber2600
doc_type: story
updated_at: 2026-04-27T00:00:00Z
---

# SW-8 — Report Branch Checkout Result

## Context

Switch must attempt `git checkout {featureId}-plan` in the control repo and report the result accurately. A missing branch is expected and normal; it must produce a clear user message with the next action, not a crash or silent fallback.

**Requirement:** SW-B6 (bounded side effects), SW-B4 (explicit, no inference)

## Task

In `switch-ops.py` switch operation, after context write (SW-7):

1. Attempt `git checkout {featureId}-plan` in the resolved control repo.
2. On success: set `branch_switched: true`, `checked_out_branch: "{featureId}-plan"`.
3. On branch-not-found error: set `branch_switched: false`, `branch_error: "branch_not_found"`, include `message: "Run /new-feature to initialize branches."` (using the retained command alias verified in SW-4).
4. On any other git error: set `branch_switched: false`, `branch_error: "{git stderr}"`.
5. Do not attempt any fallback checkout to another branch.

## Files

- `lens.core/_bmad/lens-work/skills/bmad-lens-switch/scripts/switch-ops.py`

## Acceptance Criteria

- [ ] Branch exists → `branch_switched: true`, `checked_out_branch: "{featureId}-plan"`.
- [ ] Branch missing → `branch_switched: false`, `branch_error: "branch_not_found"`, message contains retained new-feature command alias.
- [ ] Other git error → `branch_switched: false`, `branch_error: "{stderr text}"`.
- [ ] No fallback checkout attempted.
- [ ] Test fixtures: branch exists, branch missing, dirty working tree (other git error).

## Dev Notes

- Parse git stderr for "pathspec ... did not match any file(s) known to git" to detect branch-not-found specifically.
- The user-facing message must use the retained command alias (confirmed in SW-4), not `init-feature`.
