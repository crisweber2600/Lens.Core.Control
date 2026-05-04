---
feature: lens-dev-new-codebase-bugfix-commit-artifacts-rejects-message-fl
story_id: "1.1"
epic: 1
title: Feature YAML command aliases (set-phase, --field phase --value)
points: 3
status: Ready
assignee: crisweber2600
updated_at: '2026-05-04T02:20:00Z'
---

# Story 1.1 — Feature YAML Command Aliases

## Context

Recent finalize-flow runs used `feature-yaml-ops.py set-phase <phase>` and
`feature-yaml-ops.py update --field phase --value <phase>`. Neither command shape is
currently supported, causing conductor retries and ad hoc workarounds.

## Files to Change

| File | Action |
|---|---|
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-feature-yaml/scripts/feature-yaml-ops.py` | Edit — add `set-phase` subcommand and `--field`/`--value` argument handling |

> ⚠️ Do NOT edit `lens.core/` — that is the read-only release clone.

## Acceptance Criteria

- [ ] `feature-yaml-ops.py set-phase <phase>` calls the same transition validator as `update --phase <phase>`
- [ ] `feature-yaml-ops.py update --field phase --value <phase>` routes to the same transition validator
- [ ] Unsupported `--field` names produce a structured error message listing supported fields
- [ ] Tests cover: valid `set-phase`, valid `--field phase --value`, invalid field rejection, invalid phase rejection
- [ ] No duplicate validation logic — aliases delegate to existing internal functions

## Implementation Steps

1. Add a `set-phase` subparser that accepts a positional `<phase>` argument and delegates to the existing `--phase` transition logic.
2. In the `update` subcommand, accept `--field` and `--value` arguments. If `--field phase` is provided, route to the phase transition validator. Reject all other `--field` values with a structured error.
3. Add/extend tests in the script's test suite.
4. Commit to the `lens.core.src` feature branch.
