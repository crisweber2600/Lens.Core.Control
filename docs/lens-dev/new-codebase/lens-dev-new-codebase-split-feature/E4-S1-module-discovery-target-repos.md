---
feature: lens-dev-new-codebase-split-feature
epic: 4
story_id: E4-S1
title: Verify module discovery registration and update target_repos
type: confirm
points: 1
status: not-started
phase: dev
updated_at: '2026-05-01T00:00:00Z'
depends_on: [E2-S1, E2-S2]
blocks: [E4-S2]
target_repo: lens.core.src
target_branch: develop
---

# E4-S1 — Verify module discovery registration and update target_repos

## Context

Per tech-plan §4 and business plan §5, the split-feature command is part of the
17-command retained published surface. The module discovery registration and
feature.yaml target_repos must be up to date before the final PR is opened.

## Implementation Steps

### 1. Check module.yaml for split-feature registration

Read `lens.core/_bmad/lens-work/module.yaml`:
```
[ ] prompts: section lists lens-split-feature.prompt.md
[ ] No duplicates
```

If not listed, add:
```yaml
- lens-split-feature.prompt.md
```

### 2. Check module-help.csv for split-feature entry

Read `lens.core/_bmad/lens-work/module-help.csv`:
```
[ ] An entry for lens-split-feature exists
[ ] Entry points to the correct prompt file
```

### 3. Update feature.yaml target_repos

The feature.yaml for `lens-dev-new-codebase-split-feature` in the governance repo
currently shows `target_repos: []`. Update to include the target source repo:

```yaml
target_repos:
  - lens.core.src
```

Use `bmad-lens-feature-yaml` to read and update, then commit and push via
`bmad-lens-git-orchestration commit-artifacts --push`.

### 4. Document result

In completion notes, record:
- module.yaml: correct / updated
- module-help.csv: correct / updated
- feature.yaml target_repos: updated

## Acceptance Criteria Checklist

```
[ ] lens-split-feature.prompt.md listed in module.yaml prompts section
[ ] module-help.csv has split-feature entry
[ ] feature.yaml target_repos includes lens.core.src
[ ] Changes committed and pushed
[ ] Result documented in completion notes
```
