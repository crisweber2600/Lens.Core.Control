# Story 1.2: Add Merge-Base History Check to git-orchestration-ops.py create-pr

Status: ready-for-dev

## Story

As a **Lens agent**,  
I want `git-orchestration-ops.py create-pr` to automatically select the correct base branch by comparing merge-base timestamps,  
so that PRs never target a base branch that shares no history with the head.

## Acceptance Criteria

1. **Given** I call `create-pr --head <branch> --base <branch>` with an explicit `--base`, **when** the command runs, **then** it verifies the head branch shares history with the specified base.

2. **Given** I call `create-pr` without specifying `--base` (auto-detect mode), **when** the command runs, **then** it performs `git merge-base HEAD main` and `git merge-base HEAD develop`, compares commit timestamps of the resulting SHAs, and selects the candidate with the more recent merge-base.

3. **And** if no candidate branch resolves a merge-base SHA, the command exits non-zero with `{"status": "error", "error": "no_common_ancestor", "detail": "No shared history found with any candidate base branch."}`.

4. **And** the existing `--base` argument continues to work; auto-detect only applies when `--base` is omitted or a new `--auto-detect-base` flag is passed.

5. **And** a unit test mocks `subprocess.run` responses for the `git merge-base` and `git log` calls and verifies `pick_base_branch()` selects the correct candidate.

## Tasks / Subtasks

- [ ] Task 1: Add `pick_base_branch()` helper to `git-orchestration-ops.py` (AC: #2, #3)
  - [ ] Implement the merge-base timestamp comparison algorithm (see Dev Notes)
  - [ ] Return `best_branch` or exit non-zero with structured error JSON
- [ ] Task 2: Wire `pick_base_branch()` into `create-pr` subcommand (AC: #1, #2, #4)
  - [ ] If `--base` is not provided, call `pick_base_branch()` to auto-detect
  - [ ] If `--base` IS provided, verify shared history (merge-base returns a valid SHA)
- [ ] Task 3: Write unit tests (AC: #5)
  - [ ] Test: `pick_base_branch()` returns `develop` when develop merge-base is more recent than main
  - [ ] Test: `pick_base_branch()` returns `main` when main merge-base is more recent
  - [ ] Test: `pick_base_branch()` exits non-zero with `no_common_ancestor` when neither candidate resolves

## Dev Notes

**Target file:** `TargetProjects/lens-dev/new-codebase/lens.core.src/skills/lens-git-orchestration/scripts/git-orchestration-ops.py`

**Algorithm (from tech-plan B5):**
```python
import subprocess, sys, json

def pick_base_branch(candidates=("main", "develop")):
    """Return the candidate branch whose merge-base with HEAD is most recent."""
    best_branch, best_ts = None, -1
    for branch in candidates:
        mb = subprocess.run(
            ["git", "merge-base", "HEAD", branch],
            capture_output=True, text=True
        )
        if mb.returncode != 0 or not mb.stdout.strip():
            continue
        sha = mb.stdout.strip()
        ts_out = subprocess.run(
            ["git", "log", "-1", "--format=%ct", sha],
            capture_output=True, text=True
        )
        ts = int(ts_out.stdout.strip()) if ts_out.stdout.strip() else -1
        if ts > best_ts:
            best_ts, best_branch = ts, branch
    if best_branch is None:
        print(json.dumps({"status": "error", "error": "no_common_ancestor",
                          "detail": "No shared history found with any candidate base branch."}))
        sys.exit(1)
    return best_branch
```

**Design decision (from implementation-readiness minor note):** Developer needs to decide whether to make `--base` optional (auto-detect when omitted) or add a new `--auto-detect-base` flag. Recommendation: make `--base` optional — default to auto-detect. This is backward-compatible since current usage always provides `--base` explicitly.

**Existing `create-pr` CLI context:** The subcommand is in `git-orchestration-ops.py`. Locate the `create-pr` subparser and the function body to understand the full existing signature before making changes.

**Testing approach:** Use `unittest.mock.patch("subprocess.run", ...)` to return controlled responses for each `git merge-base` and `git log` call sequence.

### Project Structure Notes

- Source repo path: `TargetProjects/lens-dev/new-codebase/lens.core.src/`
- Script path: `skills/lens-git-orchestration/scripts/git-orchestration-ops.py`
- Changes are additive to existing CLI — no arguments removed

### References

- [Source: docs/...tech-plan.md#B5] B5 — full algorithm and output shape specification
- [Source: docs/...epics.md#Story 1.2] AC specifications

## Dev Agent Record

### Agent Model Used

_to be filled by dev agent_

### Debug Log References

### Completion Notes List

### File List

- `TargetProjects/lens-dev/new-codebase/lens.core.src/skills/lens-git-orchestration/scripts/git-orchestration-ops.py`
