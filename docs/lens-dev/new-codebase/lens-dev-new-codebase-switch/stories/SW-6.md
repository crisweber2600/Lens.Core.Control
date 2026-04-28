---
feature: lens-dev-new-codebase-switch
story_id: SW-6
epic: EP-2
sprint: 2
title: Return Complete Feature Context
estimate: M
status: review
blocked_by: [SW-5]
assignee: crisweber2600
doc_type: story
updated_at: 2026-04-27T18:00:00Z
---

# SW-6 — Return Complete Feature Context

## Context

A successful switch response must include all fields required by the tech-plan JSON contract. Partial responses break downstream callers (release prompt, dev agent, next-action logic). Stale and target repo state must be computed and returned correctly.

**Requirements:** SW-B6, SW-B8, SW-B9

## Task

In `switch-ops.py` switch operation, after identity validation (SW-5):

1. Load `feature.yaml` from the resolved governance path.
2. Compute `stale: true` when `feature.yaml.updated` is more than 30 days before today; `false` otherwise.
3. If `feature.yaml.target_repos` is empty, set `target_repo_state: null`.
4. If `target_repos` has entries, summarize the first (or all) with: `repo`, `working_branch`, `dev_branch_mode`, `pr_state` (null if no PR).
5. Return the full response shape per tech-plan section 4 JSON contract.

## Expected Response Shape

```json
{
  "status": "pass",
  "feature_id": "lens-dev-new-codebase-switch",
  "domain": "lens-dev",
  "service": "new-codebase",
  "phase": "finalizeplan",
  "track": "express",
  "priority": "medium",
  "owner": "crisweber2600",
  "stale": false,
  "context_path": "TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-switch",
  "target_repo_state": {
    "repo": "lens.core.src",
    "working_branch": "feature/switch-dev",
    "dev_branch_mode": "feature-id",
    "pr_state": null
  },
  "context_paths": { ... },
  "branch_switched": false,
  "branch_error": null
}
```

## Files

- `lens.core/_bmad/lens-work/skills/bmad-lens-switch/scripts/switch-ops.py`
- `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-switch/feature.yaml` (read)

## Acceptance Criteria

- [ ] Response contains all fields from the tech-plan 4.3 contract.
- [ ] `stale: true` when `feature.yaml.updated` is >30 days before today.
- [ ] `stale: false` when within 30 days.
- [ ] `target_repo_state: null` when `target_repos` is empty.
- [ ] `target_repo_state` populated with `repo`, `working_branch`, `dev_branch_mode`, `pr_state` when `target_repos` is not empty.
- [ ] Test fixtures: stale feature (old timestamp), fresh feature, feature with target repo, feature without target repo.

## Status

review

## Dev Agent Record

### Debug Log

- 2026-04-27: Implemented `SW-6` under target repo `TargetProjects/lens-dev/new-codebase/lens.core.src` on branch `feature/switch-dev`.
- 2026-04-27: Verified with `uv run --with pytest _bmad/lens-work/skills/bmad-lens-switch/scripts/tests/test-switch-ops.py -q`.
- 2026-04-27: Verified with `uv run --with pytest --with pyyaml python -m pytest _bmad/lens-work -q`.

### Completion Notes

- Returned full top-level switch JSON contract including stale, target_repo_state, context paths, and branch result.
- Story status moved to `review`; implementation is ready for code review.

### File List

- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/agents/lens.agent.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-work-setup/assets/module-help.csv`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/module-help.csv`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-switch.prompt.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-switch/SKILL.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-switch/references/list-features.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-switch/references/switch-feature.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-switch/scripts/switch-ops.py`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-switch/scripts/tests/test-switch-ops.py`
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-switch/stories/SW-6.md`
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-switch/sprint-status.yaml`

### Change Log

- 2026-04-27: Implemented, tested, and moved to review.

