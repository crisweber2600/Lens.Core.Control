---
feature: lens-dev-new-codebase-bugfix-commit-artifacts-rejects-message-fl
story_id: "3.1"
epic: 3
title: Preflight contract alignment
points: 2
status: Ready
assignee: crisweber2600
updated_at: '2026-05-04T02:20:00Z'
---

# Story 3.1 — Preflight Contract Alignment

## Context

`lens-preflight/SKILL.md` documents `--caller` and `--governance-path` arguments, but
`light-preflight.py` does not accept them. Additionally, the old-codebase pattern used
`light-preflight.py` as a prompt-start sync that delegated to the full `preflight.py` helper.
That delegation may have been lost, leaving conductors with a lighter gate that does not
sync as documented.

Two options exist:

- **Option A:** Treat `light-preflight.py` as a lightweight gate permanently and correct
  `SKILL.md` to remove the unsupported argument references.
- **Option B:** Restore delegation to the full `preflight.py` helper and add argument
  parsing for `--caller` and `--governance-path`.

The tech plan prefers Option B unless tests reveal unacceptable startup cost.

## Files to Change

| File | Action |
|---|---|
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/scripts/light-preflight.py` | Edit — add argument parsing (Option B) or no change (Option A) |
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/SKILL.md` | Edit — align documented arguments with actual script behavior |

> ⚠️ Do NOT edit `lens.core/` — that is the read-only release clone.

## Acceptance Criteria

- [ ] Implementation decision documented in story notes: Option A or Option B chosen
- [ ] If Option B: `light-preflight.py` accepts `--caller` and `--governance-path` and delegates to `preflight.py`; `SKILL.md` documents these arguments
- [ ] If Option A: `SKILL.md` documents only the arguments `light-preflight.py` actually accepts; no unsupported argument references remain
- [ ] Script invocation examples in `SKILL.md` use `lens.core/...` for workspace-root and `_bmad/...` for source-root contexts
- [ ] Tests cover: root detection from workspace root, root detection from source-repo root, documented argument acceptance/rejection
- [ ] `light-preflight.py` does not silently accept and ignore undocumented arguments

## Implementation Steps

1. Read the current `light-preflight.py` source and `SKILL.md` to confirm the exact delta.
2. Decide Option A or Option B. Record the decision in a code comment.
3. If Option B: implement argument parsing and delegation. If Option A: update `SKILL.md` only.
4. Normalize invocation examples for workspace-root and source-root paths.
5. Add root-detection and argument handling tests.
6. Commit to the `lens.core.src` feature branch.

## Dev Agent Record

- Status: Done
- Implementation decision: Option B — `light-preflight.py` now accepts documented arguments and delegates to `preflight.py`; root detection prefers the containing control workspace before falling back to standalone source-repo layout.
- Source commit: `b20484cb`
- Validation: `uv run --with pytest --with pyyaml python -m pytest _bmad/lens-work/skills/lens-feature-yaml/scripts/tests/test-feature-yaml-ops.py _bmad/lens-work/skills/lens-git-orchestration/scripts/tests/test-git-orchestration-ops.py _bmad/lens-work/skills/lens-preflight/scripts/tests/test-light-preflight.py _bmad/lens-work/scripts/tests/test-lifecycle-state.py _bmad/lens-work/scripts/tests/test-prompt-normalize.py` — 113 passed
