---
story_id: E2-S1
epic: E2
feature: lens-dev-new-codebase-expressplan
title: Register Command in Discovery Surface
priority: High
size: S
status: not-started
updated_at: '2026-04-30T00:00:00Z'
---

# E2-S1 — Register Command in Discovery Surface

## Context

The new-codebase target project has a retained command discovery surface (the same file
used by `lens-techplan` and other commands). `lens-expressplan` must be registered there
so that lifecycle tooling can locate it.

## Tasks

1. Identify the discovery file — check
   `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/` for any file
   that lists retained commands (e.g. `agent-manifest.csv`, `skill-manifest.csv`, or a
   YAML/JSON discovery registry).
2. Confirm the pattern used by `lens-techplan` (or `lens-baselineplan`) to register.
3. Add `lens-expressplan` following the identical pattern.
4. Commit the change to the target source repo.

## Acceptance Criteria

- [ ] Discovery file identified.
- [ ] `lens-expressplan` registered following the same pattern as other retained commands.
- [ ] Change committed.
