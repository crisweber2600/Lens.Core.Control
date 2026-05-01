---
feature: lens-dev-new-codebase-split-feature
epic: 2
story_id: E2-S2
title: Verify prompt files
type: confirm
points: 1
status: ready
phase: dev
updated_at: '2026-05-01T00:00:00Z'
depends_on: []
blocks: [E4-S2]
target_repo: lens.core.src
target_branch: develop
---

# E2-S2 — Verify prompt files

## Context

Two prompt files govern the split-feature command entry point. Both are in the correct
location but need verification before the final PR:

1. `.github/prompts/lens-split-feature.prompt.md` — public stub with preflight
2. `lens.core/_bmad/lens-work/prompts/lens-split-feature.prompt.md` — release prompt (thin redirect)

Per tech-plan §4.1: "Verify stub chain integrity" for `.github/prompts/lens-split-feature.prompt.md`
and "Rewrite — release prompt (stub redirect)" for the release prompt.

## Implementation Steps

### 1. Verify .github/prompts/lens-split-feature.prompt.md

Read the file and check:
```
[ ] Contains YAML frontmatter with description
[ ] Runs uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py in step 1
[ ] Stops if that command exits non-zero (step 2)
[ ] Redirects to lens.core/_bmad/lens-work/prompts/lens-split-feature.prompt.md (step 3)
[ ] Uses "This is a stub" designation
```

Current state: file exists and appears correct from prior inspection. Confirm no
changes needed.

### 2. Verify lens.core/_bmad/lens-work/prompts/lens-split-feature.prompt.md

Read the file and check:
```
[ ] Contains YAML frontmatter with description
[ ] Uses "This is a stub" designation
[ ] Redirects to: lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/SKILL.md
```

Current state: file exists with correct content. Confirm no changes needed.

### 3. Apply corrections if needed

If either file has incorrect content, fix it and commit with message:
`[DEV] lens-dev-new-codebase-split-feature — fix prompt file E2-S2`

### 4. Document result

In completion notes, record:
- `.github/prompts/lens-split-feature.prompt.md`: correct / corrected
- `lens.core/_bmad/lens-work/prompts/lens-split-feature.prompt.md`: correct / corrected

## Acceptance Criteria Checklist

```
[ ] .github/prompts/lens-split-feature.prompt.md has light-preflight.py preflight step
[ ] .github/prompts/lens-split-feature.prompt.md redirects to release prompt in lens.core
[ ] lens.core/_bmad/lens-work/prompts/lens-split-feature.prompt.md redirects to SKILL.md
[ ] Both files verified correct (or corrections applied and committed)
[ ] Result documented in completion notes
```
