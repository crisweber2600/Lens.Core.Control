---
feature: lens-dev-new-codebase-split-feature
epic: 4
story_id: E4-S2
title: Commit all artifacts, open final PR, update feature.yaml phase
type: new
points: 1
status: not-started
phase: dev
updated_at: '2026-05-01T00:00:00Z'
depends_on: [E3-S2, E4-S1]
blocks: []
target_repo: lens.core.src
target_branch: develop
---

# E4-S2 — Commit all artifacts, open final PR, update feature.yaml phase

## Context

All script fixes, SKILL.md rewrite, test additions, and verification are complete.
This story commits everything, opens the final feature PR, and updates the governance
phase to `finalizeplan-complete`.

## Implementation Steps

### 1. Confirm all modified files are committed

On branch `lens-dev-new-codebase-split-feature` in the control repo:
- `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/SKILL.md` (rewritten)
- `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/scripts/split-feature-ops.py` (fixed)
- `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/scripts/tests/test-split-feature-ops.py` (extended)
- Any prompt file corrections (if applied in E2-S2)

Run `git status` to confirm working tree is clean.

### 2. Push the feature branch

```bash
git push origin lens-dev-new-codebase-split-feature
```

### 3. Open the final PR

Open PR: `lens-dev-new-codebase-split-feature` → `main`

PR title: `[lens-dev/new-codebase] split-feature rewrite — thin conductor + script fixes`

PR description must include:
- Summary of changes (script fixes, SKILL.md rewrite, test additions)
- Test count and pass status
- Reference to feature: `lens-dev-new-codebase-split-feature`
- Phase: `finalizeplan-complete`

Via `bmad-lens-git-orchestration create-pr`.

### 4. Update feature.yaml phase to finalizeplan-complete

Via `bmad-lens-feature-yaml`, update:
```yaml
phase: finalizeplan-complete
```

Add phase_transition:
```yaml
phase_transitions:
  - phase: finalizeplan-complete
    timestamp: <current ISO timestamp>
    user: crisweber2600
```

Commit and push governance repo.

### 5. Report

Report:
- PR URL
- Committed SHA on feature branch
- feature.yaml updated to finalizeplan-complete
- Next action: `/dev`

## Acceptance Criteria Checklist

```
[ ] All modified source files committed on lens-dev-new-codebase-split-feature
[ ] Feature branch pushed to origin
[ ] Final PR opened against main
[ ] PR title and description match template
[ ] feature.yaml phase = finalizeplan-complete
[ ] feature.yaml phase_transition recorded
[ ] Governance repo commit pushed
[ ] PR URL reported
[ ] Next action /dev reported
```
