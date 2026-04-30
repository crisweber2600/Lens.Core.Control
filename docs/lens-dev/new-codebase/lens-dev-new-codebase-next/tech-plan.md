---
feature: lens-dev-new-codebase-next
doc_type: tech-plan
status: draft
goal: "Define the clean-room implementation and validation plan for the next command rewrite, including express-track routing parity."
key_decisions:
  - Implement Next as a thin conductor over a script-backed routing engine; no downstream phase logic belongs inside the skill.
  - Keep lifecycle.yaml as the source of truth for phase commands, track start phases, and phase-complete auto-advance behavior.
  - Add express-track fixture coverage so `track: express` no longer depends on full-track defaults.
  - Preserve pre-confirmed delegation by passing handoff context into the loaded phase skill instead of asking a second launch question.
  - Treat paused-state routing and constitution express-track filtering as explicit parity risks that must be resolved before release readiness.
open_questions:
  - Should paused state map to an internal pause-resume skill, a retained recovery command, or a blocker message?
  - Should constitution resolver express-track support be fixed in this feature or in the constitution prerequisite feature?
  - Which target-project discovery surface should be updated first: module-help.csv, module.yaml, lens.agent.md, or installer manifest?
depends_on:
  - business-plan.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/2-6-rewrite-next.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/architecture.md
blocks:
  - sprint-plan.md
  - expressplan-adversarial-review.md
updated_at: 2026-04-30T20:47:21Z
---

# Tech Plan - Next Command

## Overview

The rewritten `next` command is a navigation conductor, not a planning phase. Its job is to read feature state, compute one next action, and either delegate or stop. The implementation should stay small: prompt stubs handle entry, the skill handles orchestration, and `next-ops.py` owns deterministic routing logic that can be tested without an interactive chat session.

The express planning route for this feature adds an important validation requirement. The routing engine must treat `express` as a first-class lifecycle track, not as an exception or a full-track alias. A feature in `expressplan` routes to `/expressplan`; a feature in `expressplan-complete` routes to `/finalizeplan`; a missing phase on an express feature uses the lifecycle track start phase.

## Target Implementation Surface

The target project work should cover these clean-room files:

| Surface | Target path | Purpose |
| --- | --- | --- |
| Public prompt stub | `TargetProjects/lens-dev/new-codebase/lens.core.src/.github/prompts/lens-next.prompt.md` | Run light preflight, stop on failure, load release prompt |
| Release prompt | `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-next.prompt.md` | Thin redirect to the owning skill |
| Owning skill | `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-next/SKILL.md` | Orchestrate routing and delegation |
| Routing script | `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-next/scripts/next-ops.py` | Compute deterministic recommendation from feature state |
| Regression fixtures | `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-next/scripts/tests/` | Validate routing parity and blockers |
| Discovery surfaces | module registry, help surface, and agent menu | Keep `next` visible as one of the retained commands |

## Runtime Contract

### 1. Prompt-Start Chain

The public stub must run the light preflight script from the workspace root. If preflight exits non-zero, the command stops and reports the failure. Only after success does it load the release prompt. This is the old public prompt contract and it applies to every retained command.

### 2. Skill Orchestration

`bmad-lens-next` loads config, resolves the governance repo and feature id, runs `next-ops.py suggest`, then applies the result:

1. `status=fail`: report the error and do not delegate.
2. Non-empty blockers: report blockers and do not delegate.
3. Unblocked recommendation: mention warnings briefly, load the owning skill for the recommended command, and pass current feature context as pre-confirmed handoff state.

### 3. Routing Engine

`next-ops.py` should remain deterministic and script-testable. It reads feature.yaml and lifecycle.yaml, then returns JSON with the current phase, track, recommendation, blockers, and warnings.

Required routing logic:

| Rule | Implementation requirement |
| --- | --- |
| Active lifecycle phase | Use `lifecycle.yaml.phases` to derive the command name |
| `*-complete` state | Use that phase's `auto_advance_to` command |
| Missing phase | Use `lifecycle.yaml.tracks.{track}.start_phase` |
| Express track | Honor `start_phase: expressplan` and phase list `[expressplan, finalizeplan]` |
| Dev and complete | Route to `/dev` or `/complete` respectively |
| Paused state | Do not delegate to a removed public surface unless the internal recovery path is retained and loaded intentionally |
| Blockers | Return blockers before any command is loaded |
| Warnings | Keep stale context and open-problem warnings secondary to blockers |

## Express Track Handling

The feature record has been switched from `track: full` to `track: express`. That state must be part of the regression matrix because it exercises the path the user requested.

Minimum express fixtures:

1. `track=express`, `phase=expressplan` -> `/expressplan`.
2. `track=express`, `phase=expressplan-complete` -> `/finalizeplan`.
3. `track=express`, missing phase -> lifecycle start phase `/expressplan`.
4. `track=express`, blockers present -> no delegation.
5. `track=express`, stale context only -> warning plus delegation.

## Paused-State Decision

The existing behavior contract mentions paused-state recovery, while the retained 17-command baseline does not keep `pause-resume` as a public command. The clean-room implementation must decide this explicitly instead of inheriting an unreachable route.

Acceptable implementation outcomes:

- Keep an internal pause-resume skill and load it through the skill route without exposing a public stub.
- Report paused state as a blocker with instructions for the retained recovery path.
- Add a retained public recovery command only if the retained command inventory is intentionally revised.

Unacceptable outcome: returning `/pause-resume` as a user-facing command when no installed public command exists.

## Constitution Resolver Compatibility

The raw org, domain, and service constitutions list `express`, and lifecycle.yaml plus feature-yaml support `express`. The current constitution resolver filters `express` and `expressplan` as unknown values. That gap can block literal expressplan validation even after the feature record is aligned.

Implementation should either:

1. land resolver allow-list support for `express` before automated expressplan validation, or
2. explicitly depend on the constitution work package that fixes track resolution.

This is not a reason to fork constitution logic inside `next`.

## Validation Plan

### Unit And Fixture Tests

- Full-track preplan routes to `/preplan`.
- Express-track expressplan routes to `/expressplan`.
- Expressplan-complete routes to `/finalizeplan` from lifecycle metadata.
- Missing phase uses track start phase.
- Blockers prevent delegation.
- Stale context warning does not block by itself.
- Paused state follows the chosen retained-surface decision.
- Unknown phase fails clearly.

### Prompt And Skill Tests

- Public stub runs light preflight before loading the release prompt.
- Release prompt is a thin redirect to `bmad-lens-next/SKILL.md`.
- Skill invokes `next-ops.py suggest` before loading any downstream phase skill.
- Skill does not present a command menu.
- Skill passes pre-confirmed handoff context to delegated phase skills.

### Governance And Write-Boundary Tests

- `next` does not write feature.yaml, feature-index.yaml, docs, or governance mirrors.
- `next` can read feature state from governance main.
- Any later phase writes remain owned by phase conductors or feature-yaml tooling.

## Definition Of Done

The feature is technically complete when the target project contains the prompt chain, owning skill, routing script, discovery wiring, and regression fixtures needed to prove the same routing outcomes described in this packet. Express-track cases are required for done; they are not optional coverage.