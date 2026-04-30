---
feature: lens-dev-new-codebase-finalizeplan
epic: 1
story_id: E1-S1
title: Validate all conductor infrastructure
type: confirm
points: 5
status: ready
phase: dev
updated_at: '2026-04-30T00:00:00Z'
depends_on: []
blocks: [E1-S2, E1-S4, E1-S5]
target_repo: lens.core.src
target_branch: develop
---

# E1-S1 — Validate all conductor infrastructure

## Context

In the prior session, three conductor SKILL.md files were created in the new-codebase
target repo (`lens.core.src`), along with prompt stubs, thin redirects, module.yaml
updates, and test files. This story reads and validates each file to confirm structural
correctness before Sprint 2 proceeds.

The adversarial review (H2) flagged a potential issue: the FinalizePlan SKILL.md
On Activation step 5 may only check for `techplan` as the predecessor phase, not
`expressplan-complete`. For the express track this must accept both.

## Implementation Steps

### 1. Read `bmad-lens-finalizeplan/SKILL.md`

Location: `_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md`

Check each item:
```
[ ] Three-step execution contract present: review-and-push, plan-pr-readiness, downstream-bundle-and-final-pr
[ ] On Activation step 5: validates BOTH techplan AND expressplan-complete as predecessors
     → If only techplan: update to accept expressplan-complete (2-point sub-story)
[ ] No direct governance file creation in the conductor
[ ] Step 3 bundle ordering: epics-and-stories → readiness → sprint-planning → story creation
[ ] feature.yaml update to finalizeplan-complete is in Step 3
```

### 2. Read `bmad-lens-expressplan/SKILL.md`

Location: `_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md`

```
[ ] Express-only eligibility gate blocks non-express features before delegation
[ ] Step 1: delegates via bmad-lens-bmad-skill --skill bmad-lens-quickplan
[ ] Step 2: invokes bmad-lens-adversarial-review --phase expressplan --source phase-complete
[ ] Party-mode is required, not optional
[ ] Step 3: sets expressplan-complete and signals /finalizeplan
```

### 3. Read `bmad-lens-quickplan/SKILL.md`

Location: `_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md`

```
[ ] Three-phase pipeline: business plan (John/PM) → tech plan (Winston/Architect) → sprint plan (Bob/SM)
[ ] No public prompt stub — internal-only
[ ] Intended invocation: via bmad-lens-bmad-skill only
```

### 4. Validate prompt stubs and thin redirects

```
[ ] .github/prompts/lens-finalizeplan.prompt.md — prompt-start preflight pattern
[ ] .github/prompts/lens-expressplan.prompt.md — prompt-start preflight pattern
[ ] _bmad/lens-work/prompts/lens-finalizeplan.prompt.md — thin redirect to SKILL.md
[ ] _bmad/lens-work/prompts/lens-expressplan.prompt.md — thin redirect to SKILL.md
```

### 5. Validate module.yaml and bmad-lens-bmad-skill

```
[ ] _bmad/lens-work/module.yaml lists lens-finalizeplan.prompt.md in prompts:
[ ] _bmad/lens-work/module.yaml lists lens-expressplan.prompt.md in prompts:
[ ] No duplicate entries (compare with expressplan feature's registrations)
[ ] bmad-lens-bmad-skill/SKILL.md integration table includes bmad-lens-quickplan
```

## Definition of Done

- All checklist items checked (pass or documented gap)
- If H2 predecessor gate gap found: remediation sub-story completed before Sprint 1 done
- Story completion notes document any gaps found
