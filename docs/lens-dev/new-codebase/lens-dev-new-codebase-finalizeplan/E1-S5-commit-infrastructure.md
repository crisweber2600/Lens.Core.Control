---
feature: lens-dev-new-codebase-finalizeplan
epic: 1
story_id: E1-S5
title: Commit untracked infrastructure files to target source repo
type: confirm
points: 2
status: ready
phase: dev
updated_at: '2026-04-30T00:00:00Z'
depends_on: [E1-S1, E1-S4]
blocks: [E2-S1, E3-S1]
target_repo: lens.core.src
target_branch: develop
---

# E1-S5 — Commit untracked infrastructure files to target source repo

## Context

The conductor SKILL.md files, prompt stubs, thin redirects, module.yaml updates, and
test files from the prior session are in `lens.core.src` but may still be untracked
(not committed). This story commits all of them to the `develop` branch.

## Implementation Steps

### 1. Check git status

```bash
cd TargetProjects/lens-dev/new-codebase/lens.core.src
git status --short
```

Expected untracked files (from prior session):
- `_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md`
- `_bmad/lens-work/skills/bmad-lens-finalizeplan/tests/test_finalizeplan_conductor.py`
- `_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md`
- `_bmad/lens-work/skills/bmad-lens-expressplan/tests/test_expressplan_conductor.py`
- `_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md`
- `_bmad/lens-work/prompts/lens-finalizeplan.prompt.md`
- `_bmad/lens-work/prompts/lens-expressplan.prompt.md`
- `.github/prompts/lens-finalizeplan.prompt.md`
- `.github/prompts/lens-expressplan.prompt.md`

Modified files:
- `_bmad/lens-work/module.yaml`
- `_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md`

### 2. Stage and commit

```bash
git add _bmad/lens-work/skills/bmad-lens-finalizeplan/ \
        _bmad/lens-work/skills/bmad-lens-expressplan/ \
        _bmad/lens-work/skills/bmad-lens-quickplan/ \
        _bmad/lens-work/prompts/lens-finalizeplan.prompt.md \
        _bmad/lens-work/prompts/lens-expressplan.prompt.md \
        .github/prompts/lens-finalizeplan.prompt.md \
        .github/prompts/lens-expressplan.prompt.md \
        _bmad/lens-work/module.yaml \
        _bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md

git commit -m "[FEAT] lens-dev-new-codebase-finalizeplan — finalizeplan/expressplan/quickplan conductor skills and prompt stubs"
git push
```

### 3. Verify

```bash
git status --short
# Should show clean working tree
git log --oneline -3
# Should show the new commit
```

## Definition of Done

- `git status` clean for all conductor infrastructure files
- Commit pushed to `develop` branch
- Commit SHA recorded in story completion notes
