---
story_id: E3-S1
epic: E3
feature: lens-dev-new-codebase-expressplan
title: Merge Planning PR and Signal Handoff
priority: High
size: XS
status: not-started
updated_at: '2026-04-30T00:00:00Z'
---

# E3-S1 — Merge Planning PR and Signal Handoff

## Context

All planning work is complete. The planning PR #30 must be merged, `feature.yaml` advanced
to `finalizeplan-complete`, and the dev handoff signalled.

## Tasks

1. Confirm all epic exit criteria (E1, E2) are met.
2. Confirm no unresolved fail-level findings in any review artifact.
3. Merge planning PR #30 (`lens-dev-new-codebase-expressplan-plan` →
   `lens-dev-new-codebase-expressplan`) on GitHub.
4. From the control repo root, run:
   ```bash
   uv run lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/scripts/feature-yaml-ops.py \
     update \
     --governance-repo "$GOV_REPO" \
     --feature-id lens-dev-new-codebase-expressplan \
     --set phase=finalizeplan-complete \
     --username "$USERNAME"
   ```
5. Commit and push governance repo changes.
6. Open the final PR (`lens-dev-new-codebase-expressplan` → `main`) via:
   ```bash
   uv run lens.core/_bmad/lens-work/skills/bmad-lens-git-orchestration/scripts/git-orchestration-ops.py \
     create-pr \
     --governance-repo "TargetProjects/lens/lens-governance" \
     --feature-id lens-dev-new-codebase-expressplan \
     --repo "."
   ```
7. Signal `/dev` handoff with a summary.

## Acceptance Criteria

- [ ] Planning PR #30 is merged.
- [ ] `feature.yaml` phase is `finalizeplan-complete`.
- [ ] Final `lens-dev-new-codebase-expressplan` → `main` PR is open.
- [ ] Dev handoff signalled.
