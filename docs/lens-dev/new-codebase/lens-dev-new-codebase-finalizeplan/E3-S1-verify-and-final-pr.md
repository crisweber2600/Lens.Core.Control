---
feature: lens-dev-new-codebase-finalizeplan
epic: 3
story_id: E3-S1
title: Verify review findings resolved, open final PR
type: new
points: 3
status: blocked-on-e1-e2
phase: dev
updated_at: '2026-04-30T00:00:00Z'
depends_on: [E1-S1, E1-S2, E1-S3, E1-S4, E1-S5, E2-S1, E2-S2, E2-S3, E2-S4]
blocks: []
target_repo: lens.core.src
target_branch: develop
---

# E3-S1 — Verify review findings resolved, open final PR

## Context

This is the final story in the planning cycle. It confirms all open findings are resolved,
opens the final PR from the feature base branch to `main` in the control repo, updates
`feature.yaml` to `finalizeplan-complete`, and signals `/dev`.

## Implementation Steps

### 1. Verify no open fail-level findings

```
[ ] expressplan-adversarial-review.md — no carry-forward blockers
[ ] finalizeplan-review.md — no carry-forward blockers
[ ] E1-S1 H2 check: predecessor gate accepts expressplan-complete (confirmed or remediated)
```

### 2. Verify Epic 1 and Epic 2 exit criteria

```
[ ] E1: All 5 stories done, tests ≥ 34 passing, files committed
[ ] E2: Commands in discovery surface, gaps documented, prerequisites confirmed, target_repos updated
```

### 3. Open final PR

```bash
cd "<control-repo-root>"
gh pr create \
  --base main \
  --head lens-dev-new-codebase-finalizeplan \
  --title "[FEATURE] lens-dev-new-codebase-finalizeplan — FinalizePlan, ExpressPlan, QuickPlan conductors" \
  --body "Delivers FinalizePlan, ExpressPlan, and QuickPlan conductor skills for the lens-work new-codebase rewrite. Express track fully functional. All 34+ tests passing. Planning artifacts, epics, stories, and readiness assessment complete."
```

### 4. Update feature.yaml and feature-index.yaml

```bash
cd "<governance-repo-root>"
# Update feature.yaml: phase → finalizeplan-complete, milestones.finalizeplan → timestamp
# Update feature-index.yaml summary for lens-dev-new-codebase-finalizeplan
git add features/lens-dev/new-codebase/lens-dev-new-codebase-finalizeplan/feature.yaml
git add feature-index.yaml
git commit -m "[FINALIZEPLAN] lens-dev-new-codebase-finalizeplan — finalizeplan-complete"
git push
```

### 5. Signal /dev

Report to the feature lead:
- Planning cycle complete
- Final PR opened
- `/dev` is ready to begin

## Definition of Done

- Final PR open (URL recorded)
- `feature.yaml` phase = `finalizeplan-complete`
- `feature-index.yaml` updated
- `/dev` signalled
