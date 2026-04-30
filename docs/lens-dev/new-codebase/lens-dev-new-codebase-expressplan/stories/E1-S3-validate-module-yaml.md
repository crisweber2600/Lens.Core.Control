---
story_id: E1-S3
epic: E1
feature: lens-dev-new-codebase-expressplan
title: Validate module.yaml Registration
priority: High
size: XS
status: not-started
updated_at: '2026-04-30T00:00:00Z'
---

# E1-S3 — Validate module.yaml Registration

## Context

`lens-expressplan.prompt.md` must be registered in `_bmad/lens-work/module.yaml` so that
lifecycle tooling and module manifests include the retained command.

## Tasks

1. Open `_bmad/lens-work/module.yaml` in the target source repo.
2. Find the `prompts:` section.
3. Confirm `lens-expressplan.prompt.md` is listed there.
4. Compare its entry shape against another retained-command entry (e.g. `lens-techplan`).
5. If missing or malformed, add/fix the entry and commit.

## Acceptance Criteria

- [ ] `lens-expressplan.prompt.md` present in `prompts:` section of `module.yaml`.
- [ ] Entry shape matches the pattern used by other retained commands.
- [ ] File is committed (no uncommitted change).
