# Story 1.1: Document Unavailable Commands and Unsafe Patterns in AGENTS.md + Add Merge-Base Check to create-pr

Status: ready-for-dev

## Story

As a **Lens agent**,  
I want AGENTS.md to clearly document `rg` unavailability, the PowerShell heredoc prohibition, and the `create-pr` base-branch requirement, and I want `git-orchestration-ops.py create-pr` to automatically select the correct base branch by comparing merge-base timestamps,  
so that I do not waste turns retrying unavailable commands or corrupt prompt files, and so that PRs never target a base branch that shares no history with the head.

## Acceptance Criteria

1. **Given** the AGENTS.md `Common Terminal Errors & Fixes` section exists, **when** three new formatted error blocks are added (rg, PowerShell, gh pr history), **then** each block uses `**Error** / **Cause** / **Fix**` format matching the existing documented example.

2. **And** the `rg` block states: Error = `rg: command not found` (or similar), Cause = `ripgrep (rg) is not installed in this environment`, Fix = Use `grep` instead of `rg` for all text searches.

3. **And** the PowerShell block states: Error = prompt files contain literal `\r\n` text after bulk replace, Cause = PowerShell heredoc replacement does not expand `\r\n` as newlines, Fix = use Python for multi-file text replacement — never PowerShell `-Command` with regex replacements on prompt files.

4. **And** the `gh pr create` block states: Error = `gh pr create` fails with no common ancestor / no shared history, Cause = branch was created from `develop` but `--base main` was passed, Fix = use the merge-base timestamp comparison in `create-pr` of `git-orchestration-ops.py`; do not call `gh pr create --base main` directly.

5. **Given** I call `create-pr --head <branch> --base <branch>` with an explicit `--base`, **when** the command runs, **then** it verifies the specified `--head` branch shares history with the specified `--base`.

6. **Given** I call `create-pr --head <branch>` without specifying `--base` (auto-detect mode), **when** the command runs, **then** it performs `git merge-base <head> main` and `git merge-base <head> develop`, compares commit timestamps of the resulting SHAs, and selects the candidate with the more recent merge-base.

7. **And** if no candidate branch resolves a merge-base SHA, the command exits non-zero with `{"status": "error", "error": "no_common_ancestor", "detail": "No shared history found with any candidate base branch."}`.

8. **And** when `--base` is omitted, auto-detect runs automatically — no additional flag is required; the existing `--base` argument continues to work when provided explicitly.

9. **And** a unit test mocks `subprocess.run` responses for the `git merge-base` and `git log` calls and verifies `pick_base_branch()` selects the correct candidate.

## Tasks / Subtasks

- [ ] Task 1: Reformat the existing `rg` entry in `AGENTS.md` to use the full error block format (AC: #1, #2)
  - [ ] Locate the existing bare `rg is not a command.` text in `Common Terminal Errors & Fixes`
  - [ ] Replace with `**Error** / **Cause** / **Fix**` block matching the documented example format
- [ ] Task 2: Add PowerShell heredoc prohibition block (AC: #1, #3)
  - [ ] Add block immediately after the `rg` block
  - [ ] Include Python replacement pattern as the recommended `Fix`
- [ ] Task 3: Add gh pr create no-history block (AC: #1, #4)
  - [ ] Add block after the PowerShell block
  - [ ] Reference `git-orchestration-ops.py create-pr` as the canonical fix path
- [ ] Task 4: Add `pick_base_branch()` helper to `git-orchestration-ops.py` (AC: #6, #7)
  - [ ] Implement the merge-base timestamp comparison algorithm using the `--head` argument (not `HEAD`) as the target branch
  - [ ] Return `best_branch` or exit non-zero with structured error JSON
- [ ] Task 5: Wire `pick_base_branch()` into `create-pr` subcommand (AC: #5, #6, #8)
  - [ ] If `--base` is not provided, call `pick_base_branch(head_branch)` to auto-detect
  - [ ] If `--base` IS provided, verify shared history (merge-base returns a valid SHA for the given `--head`)
- [ ] Task 6: Write unit tests (AC: #9)
  - [ ] Test: `pick_base_branch()` returns `develop` when develop merge-base is more recent than main
  - [ ] Test: `pick_base_branch()` returns `main` when main merge-base is more recent
  - [ ] Test: `pick_base_branch()` exits non-zero with `no_common_ancestor` when neither candidate resolves

## Dev Notes

**Target files (two sequential commits in two repos):**

1. **Control repo:** `AGENTS.md` (control repo root)
2. **Source repo:** `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-git-orchestration/scripts/git-orchestration-ops.py`

This story requires two separate commit operations: one in the control repo for AGENTS.md, and one in the source repo for the git-orchestration-ops.py B5 fix.

**Existing format reference (AGENTS.md):**
```markdown
**Error**: `<error message>`  
**Cause**: `<root cause>`  
**Fix**: `<resolution>`
```

This format is the only documented example in AGENTS.md `Common Terminal Errors & Fixes`. New blocks must match it exactly.

**B1 (rg) content:**
```markdown
**Error**: `rg: command not found` (or similar)
**Cause**: `ripgrep (rg) is not installed in this environment`
**Fix**: Use `grep` instead of `rg` for all text searches
```

**B2 (PowerShell) content:**
```markdown
**Error**: Prompt files contain literal `\r\n` text after bulk replace
**Cause**: PowerShell heredoc replacement does not expand `\r\n` as newlines
**Fix**: Use Python for multi-file text replacement — never PowerShell `-Command` with regex replacements on prompt files. Example:
```python
from pathlib import Path
for p in Path(".github/prompts").glob("*.prompt.md"):
    content = p.read_text(encoding="utf-8")
    content = content.replace("OLD", "NEW")
    p.write_text(content, encoding="utf-8")
```
```

**B5 (gh pr create) content:**
```markdown
**Error**: `gh pr create` fails with no common ancestor / no shared history
**Cause**: Branch was created from `develop` (or another non-main base) but `--base main` was passed
**Fix**: Use the merge-base timestamp comparison in `create-pr` of `git-orchestration-ops.py`; do not call `gh pr create --base main` directly
```

**B5 algorithm (from tech-plan B5):**
```python
import subprocess, sys, json

def pick_base_branch(head_branch, candidates=("main", "develop")):
    """Return the candidate branch whose merge-base with head_branch is most recent."""
    best_branch, best_ts = None, -1
    for branch in candidates:
        mb = subprocess.run(
            ["git", "merge-base", head_branch, branch],
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

**Design decision:** `--base` is optional — when omitted, auto-detect runs automatically using the `--head` branch value. This is backward-compatible since current usage always provides `--base` explicitly.

**Existing `create-pr` CLI context:** The subcommand is in `git-orchestration-ops.py`. Locate the `create-pr` subparser and the function body to understand the full existing signature before making changes.

### Project Structure Notes

- Control repo AGENTS.md: control repo root
- Source repo path: `TargetProjects/lens-dev/new-codebase/lens.core.src/`
- Script path: `_bmad/lens-work/skills/bmad-lens-git-orchestration/scripts/git-orchestration-ops.py`
- Changes are additive to existing CLI — no arguments removed

### References

- [Source: docs/...tech-plan.md#B1] B1 AGENTS.md reformat
- [Source: docs/...tech-plan.md#B2] B2 PowerShell prohibition + Python replacement pattern
- [Source: docs/...tech-plan.md#B5] B5 gh pr create failure + merge-base algorithm

## Dev Agent Record

### Agent Model Used

_to be filled by dev agent_

### Debug Log References

### Completion Notes List

### File List

- `AGENTS.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-git-orchestration/scripts/git-orchestration-ops.py`
