---
feature: lens-dev-new-codebase-bugfix-commit-artifacts-rejects-message-fl
story_id: "2.1"
epic: 2
title: Git orchestration command compatibility and PR body handling
points: 3
status: Ready
assignee: crisweber2600
updated_at: '2026-05-04T02:20:00Z'
---

# Story 2.1 — Git Orchestration Command Compatibility and PR Body Handling

## Context

Conductors have observed three failure patterns with `git-orchestration-ops.py`:

1. `commit-artifacts --message <msg>` rejected — only `--description` is accepted.
2. `create-pr --source-branch/--target-branch` not supported — only `--head/--base` accepted.
3. PR body generation required a temporary shell heredoc because no direct `--body` string was supported.

## Files to Change

| File | Action |
|---|---|
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-git-orchestration/scripts/git-orchestration-ops.py` | Edit — add flag aliases and `--body` string support |

> ⚠️ Do NOT edit `lens.core/` — that is the read-only release clone.

## Acceptance Criteria

- [ ] `commit-artifacts --message <msg>` accepted as a compatibility alias for `--description`
- [ ] `create-pr --source-branch <branch>` maps unambiguously to `--head <branch>`
- [ ] `create-pr --target-branch <branch>` maps unambiguously to `--base <branch>`
- [ ] `create-pr --body <string>` generates the PR body directly without requiring a temporary file
- [ ] Tests cover all aliases and `--body` handling (success and rejection cases)
- [ ] Aliases delegate to the existing command implementation — no duplicate logic

## Implementation Steps

1. In `commit-artifacts` argument parsing, add `--message` as an alias for `--description`.
2. In `create-pr` argument parsing, add `--source-branch` as an alias for `--head` and `--target-branch` as an alias for `--base`.
3. Add `--body` as an optional string argument for `create-pr`.
4. Extend tests to cover new aliases and the `--body` path.
5. Commit to the `lens.core.src` feature branch.
