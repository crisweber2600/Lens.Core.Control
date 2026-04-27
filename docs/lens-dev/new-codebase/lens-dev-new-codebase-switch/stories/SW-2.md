---
feature: lens-dev-new-codebase-switch
story_id: SW-2
epic: EP-1
sprint: 1
title: Preserve Config Resolution
estimate: S
status: not-started
blocked_by: []
assignee: crisweber2600
doc_type: story
updated_at: 2026-04-27T00:00:00Z
---

# SW-2 — Preserve Config Resolution

## Context

Switch config resolution must prefer `.lens/governance-setup.yaml` when present, then fall back to `lens.core/_bmad/lens-work/bmadconfig.yaml`. This two-tier resolution allows users to override the governance repo path for local workspaces without modifying the module config. Missing files must not crash the command.

## Task

Verify or implement in `switch-ops.py` (and the release prompt if applicable):

1. When `.lens/governance-setup.yaml` exists at the workspace root, its `governance_repo_path` wins.
2. When it is absent, the module config `governance_repo_path` is used.
3. When both are missing, the command returns a config error with actionable guidance ("Run `/lens-onboard` to set up governance config").
4. Config resolution runs before any governance file reads.

## Files

- `lens.core/_bmad/lens-work/skills/bmad-lens-switch/scripts/switch-ops.py` (primary implementation)
- `lens.core/_bmad/lens-work/bmadconfig.yaml` (fallback config — read-only)
- `.lens/governance-setup.yaml` (optional override — read)

## Acceptance Criteria

- [ ] Override file present → its `governance_repo_path` is used.
- [ ] Override file absent → module config `governance_repo_path` is used.
- [ ] Both absent → command returns `status: fail`, `error: config_missing` with actionable message.
- [ ] Config resolution precedes any `feature-index.yaml` or `feature.yaml` file read.

## Dev Notes

- The `.lens/governance-setup.yaml` check must be path-safe; use `os.path.join` with the workspace root, not string concatenation.
- Config error message must name the setup command (`/lens-onboard`) so users know what to run.
