---
feature: lens-dev-new-codebase-bugfix-commands-should-have-run-via-lens-4
epic: 1
story_id: E1-S1
title: Update Phase 4 and Error Recovery in bmad-lens-bug-fixer/SKILL.md
type: fix
points: 1
status: ready-for-dev
phase: dev
updated_at: '2026-05-03T00:00:00Z'
depends_on: []
blocks: []
target_repo: lens.core.src
target_branch: fix/preflight-old-patterns
---

# E1-S1 — Update Phase 4 and Error Recovery in bmad-lens-bug-fixer/SKILL.md

## Context

When `/lens-bug-fixer --fix-all-new` reaches Phase 4 (expressplan execution), the SKILL.md
instruction "Delegate to `bmad-lens-expressplan` skill" was interpreted by a conductor as
a shell command (`lens-expressplan plan ...`). No such shell command exists. Phase 4 failed
with `command not found`, leaving bugs stranded in Inprogress.

**The fix has already been implemented** in commit `56b1be33` on branch
`fix/preflight-old-patterns` in `lens.core.src`. This story is in **ready-for-verification**
state — the dev task is to verify the fix and open the source repo PR.

## Implementation Steps

### 1. Verify the fix in source repo

File: `_bmad/lens-work/skills/bmad-lens-bug-fixer/SKILL.md` (in `lens.core.src`)

Check Phase 4 steps 18–20:

```
[ ] Step 18: "Collect planning input: read each Inprogress bug file and concatenate title +
    description fields as a single planning context string."
[ ] Step 19: Contains "Load {project-root}/lens.core/_bmad/lens-work/skills/lens-expressplan/SKILL.md"
[ ] Step 19: Contains "Follow its On Activation section"
[ ] Step 19: Contains "Do NOT run lens-expressplan or any variant as a shell command"
[ ] Step 20: "If the expressplan skill activation fails or is blocked by a gate, bugs remain
    Inprogress; record the gate/failure message in the outcome report."
```

Check Error Recovery step 3:
```
[ ] "Load {project-root}/lens.core/_bmad/lens-work/skills/lens-expressplan/SKILL.md
    and follow its On Activation section"
[ ] No longer says "Delegate expressplan manually" with implicit shell interpretation
```

### 2. Open PR in source repo

Open PR: `fix/preflight-old-patterns` → `develop` in `lens.core.src`

URL will be provided by GitHub. Link PR in `feature.yaml.links.pull_request` via
`lens-feature-yaml update`.

### 3. Verification test

Run `/lens-bug-fixer --fix-all-new` with at least one bug in `bugs/New/` after the fix
merges and confirm:
- Phase 4 activates expressplan skill via SKILL.md load (no shell command)
- `expressplan-adversarial-review.md` is produced
- `feature.yaml.phase` advances to `expressplan-complete`

## Acceptance Criteria

- [x] Phase 4 steps 18–20 match the spec above (clean-room implementation, commit a47f6da0)
- [x] Error Recovery step 3 uses explicit SKILL.md load language (commit a47f6da0)
- [x] Source repo PR opened from `feature/lens-dev-new-codebase-bugfix-commands-should-have-run-via-lens-4` → `develop` (Lens.Core.Src #42)
- [x] PR URL recorded in `feature.yaml.links.pull_request`

## Dev Agent Record

- implementation_commit: a47f6da0
- branch: feature/lens-dev-new-codebase-bugfix-commands-should-have-run-via-lens-4
- base_branch: develop
- target_repo: lens.core.src
- completed_at: '2026-05-03T12:00:00Z'
- notes: Clean-room implementation — fix applied directly on new feature branch off develop (not cherry-picked from fix/preflight-old-patterns).
