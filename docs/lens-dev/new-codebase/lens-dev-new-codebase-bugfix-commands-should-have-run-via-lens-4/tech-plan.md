---
feature: lens-dev-new-codebase-bugfix-commands-should-have-run-via-lens-4
title: "Bugbash: Commands Should Have Run Via Lens 4F788235"
doc_type: tech-plan
status: draft
track: express
phase: expressplan
created_at: 2026-05-03
---

# Tech Plan — Commands Should Have Run Via Lens

## Architecture Assessment

This is a single-artifact text fix. No new scripts, no new skills, no new test infra.

**File to change:**
`TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-bug-fixer/SKILL.md`

## Root Cause Detail

Phase 4, step 19 currently reads:
```
19. Delegate to `bmad-lens-expressplan` skill with the combined bug content as planning context.
```

A conductor reading this interprets "delegate to skill" as running a shell command. There
is no executable named `lens-expressplan`. The correct interpretation is:

> Load `{project-root}/lens.core/_bmad/lens-work/skills/lens-expressplan/SKILL.md` and
> execute its `On Activation` section, passing `featureId` and bug descriptions as planning context.

## Implementation Boundaries

- **Change scope:** `bmad-lens-bug-fixer/SKILL.md` — Phase 4 steps 18–20 only.
- **No changes to:** `bug-fixer-ops.py`, `init-feature-ops.py`, `light-preflight.py`,
  `bmad-lens-expressplan/SKILL.md`, or any governance file.
- **Write path:** source repo only — `TargetProjects/lens-dev/new-codebase/lens.core.src/`.

## Proposed Change

Replace Phase 4 steps 18–20 with unambiguous conductor instructions:

```markdown
### Phase 4 — Expressplan Execution

18. Collect planning input: read each Inprogress bug file and concatenate title +
    description + chat_log fields as a single planning context string.
19. Activate the `lens-expressplan` skill:
    - Load `{project-root}/lens.core/_bmad/lens-work/skills/lens-expressplan/SKILL.md`.
    - Follow its `On Activation` steps, passing `featureId` as the target feature.
    - Pass the concatenated bug descriptions as additional planning context for QuickPlan.
    - Do NOT run `lens-expressplan` or any variant as a shell command. It is not a
      registered shell command; it is a skill delegation.
20. If the expressplan skill activation fails or is blocked by a gate, bugs remain
    Inprogress; record the gate/failure message in the outcome report.
```

Also update the Error Recovery section to clarify the same delegation pattern.

## Artifact Contracts

| Artifact | Change |
|----------|--------|
| `bmad-lens-bug-fixer/SKILL.md` | Update Phase 4 steps 18–20 + Error Recovery |

## Testing Strategy

- Manually run `/lens-bug-fixer --fix-all-new` after the fix and confirm Phase 4 executes
  expressplan via skill delegation.
- Verify `expressplan-adversarial-review.md` is produced.
- Verify `feature.yaml.phase` advances to `expressplan-complete`.

## Rollout

Deploy via source-repo commit on current branch → promote-to-release CI/CD.
No migration needed — bugs currently Inprogress from failed prior runs require manual
recovery per the Error Recovery section.
