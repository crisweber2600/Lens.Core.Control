---
feature: lens-dev-new-codebase-bugfix-commit-artifacts-rejects-message-fl
story_id: "4.1"
epic: 4
title: Durable lifecycle helpers and AGENTS.md terminal guidance
points: 3
status: Ready
assignee: crisweber2600
updated_at: '2026-05-04T02:20:00Z'
---

# Story 4.1 — Durable Lifecycle Helpers and AGENTS.md Terminal Guidance

## Context

Conductors have used inline Python one-liners for lifecycle state inspection and PowerShell
heredocs for prompt file repair. These patterns are not reproducible, hard to review, and
have caused follow-on bugs (e.g., literal `\r\n` tokens in prompt files). This story
replaces them with durable repo-owned helpers and adds explicit prohibitions to `AGENTS.md`.

This story spans **two repositories**:

1. **Source repo** (`lens.core.src`): script helpers for lifecycle state inspection and prompt repair.
2. **Control repo** (this repository): `AGENTS.md` terminal-error guidance update.

> ⚠️ The `AGENTS.md` change MUST be committed and PR'd against the control repo separately
> from the `lens.core.src` feature branch. Do NOT attempt to land `AGENTS.md` via the
> source-repo PR.

## Files to Change

| File | Repo | Action |
|---|---|---|
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/scripts/` | `lens.core.src` | Add lifecycle state inspection command and/or prompt repair helper |
| `AGENTS.md` | Control repo | Add prohibitions for ad hoc snippets; direct conductors to repo-owned scripts |

## Acceptance Criteria (source-repo — `lens.core.src`)

- [ ] A Lens script command (new or extended existing) prints feature phase, track, target repos, docs path, and PR links without requiring inline Python
- [ ] A repo-owned prompt normalization helper exists for newline/path repair, OR the repair pattern is explicitly prohibited in favour of an existing tool
- [ ] If a prompt repair helper is added: fixture-based tests exist using files with literal `\r\n` tokens

## Acceptance Criteria (control-repo — `AGENTS.md`)

- [ ] `AGENTS.md` "Common Terminal Errors & Fixes" section includes an entry prohibiting PowerShell bulk prompt replacements and directing conductors to use Python for multi-file text replacement
- [ ] `AGENTS.md` includes guidance directing conductors to use repo-owned Lens scripts for recurring lifecycle state checks instead of ad hoc Python one-liners
- [ ] The `AGENTS.md` change is landed via a separate control-repo PR targeting the appropriate base branch

## Implementation Steps (source-repo)

1. Add or extend a script in `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/scripts/` for lifecycle state inspection.
2. Add a prompt repair helper or document the prohibition in the script's help text.
3. Add tests for prompt repair if a helper is created.
4. Commit to the `lens.core.src` feature branch.

## Implementation Steps (control-repo)

1. Edit `AGENTS.md` in the control repo root.
2. Add an entry under "Common Terminal Errors & Fixes" for PowerShell prompt replacement prohibition.
3. Add guidance for lifecycle state checks.
4. Commit the `AGENTS.md` change and open a PR targeting the appropriate base branch in the control repo.
