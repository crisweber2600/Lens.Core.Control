# Story 1.1: Document Unavailable Commands and Unsafe Patterns in AGENTS.md

Status: ready-for-dev

## Story

As a **Lens agent**,  
I want AGENTS.md to clearly document `rg` unavailability, the PowerShell heredoc prohibition, and the `create-pr` base-branch requirement,  
so that I do not waste turns retrying unavailable commands or corrupt prompt files with literal escape sequences.

## Acceptance Criteria

1. **Given** the AGENTS.md `Common Terminal Errors & Fixes` section exists, **when** three new formatted error blocks are added (rg, PowerShell, gh pr history), **then** each block uses `**Error** / **Cause** / **Fix**` format matching the existing documented example.

2. **And** the `rg` block states: Error = `rg: command not found` (or similar), Cause = `ripgrep (rg) is not installed in this environment`, Fix = Use `grep` instead of `rg` for all text searches.

3. **And** the PowerShell block states: Error = prompt files contain literal `\r\n` text after bulk replace, Cause = PowerShell heredoc replacement does not expand `\r\n` as newlines, Fix = use Python for multi-file text replacement — never PowerShell `-Command` with regex replacements on prompt files.

4. **And** the `gh pr create` block states: Error = `gh pr create` fails with no common ancestor / no shared history, Cause = branch was created from `develop` but `--base main` was passed, Fix = use the merge-base timestamp comparison in `create-pr` of `git-orchestration-ops.py`; do not call `gh pr create --base main` directly.

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

## Dev Notes

**Target file:** `D:/Lens.Core.Control - Copy/AGENTS.md` (control repo root — NOT the source repo)

This story applies exclusively to the control repo's `AGENTS.md`. The source repo has no `AGENTS.md`.

**Existing format reference:**
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

### Project Structure Notes

- AGENTS.md is in the control repo root: `D:/Lens.Core.Control - Copy/AGENTS.md`
- No changes to source repo files in this story

### References

- [Source: docs/...tech-plan.md#B1] B1 AGENTS.md reformat
- [Source: docs/...tech-plan.md#B2] B2 PowerShell prohibition + Python replacement pattern
- [Source: docs/...tech-plan.md#B5] B5 gh pr create failure + merge-base reference

## Dev Agent Record

### Agent Model Used

_to be filled by dev agent_

### Debug Log References

### Completion Notes List

### File List

- `D:/Lens.Core.Control - Copy/AGENTS.md`
