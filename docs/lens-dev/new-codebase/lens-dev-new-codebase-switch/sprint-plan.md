---
feature: lens-dev-new-codebase-switch
doc_type: sprint-plan
status: approved
goal: "Organize the switch command parity work into focused implementation and validation slices"
key_decisions:
  - Deliver switch parity as a small command-surface feature with tests first around observable behavior.
  - Treat deprecated command-name cleanup as part of parity because the 17-command surface removes `init-feature` as a public command.
  - Keep code changes scoped to the switch prompt, skill references, script behavior, tests, and help surfaces directly advertising switch behavior.
open_questions: []
depends_on: [business-plan, tech-plan]
blocks: []
updated_at: 2026-04-27T00:00:00Z
---

# Sprint Plan - Switch Command

## Sprint 1 - Contract Lock And Prompt Parity

**Goal:** Preserve the user-visible switch entrypoint and selection model.

| Story | Description | Acceptance Criteria |
|---|---|---|
| SW-1 | Verify prompt-start gate parity. | Stub runs light preflight before release prompt; release prompt does not read skills or search alternate paths before sync. |
| SW-2 | Preserve config resolution. | Governance override in `.lens/governance-setup.yaml` wins over module config; defaults work when config is missing. |
| SW-3 | Lock numbered menu behavior. | No explicit feature id triggers list; `features` mode asks for a number; `domains` mode renders inventory and stops; invalid input rerenders and stops. |
| SW-4 | Remove deprecated public command references. | Switch user-facing messages and references point users to retained commands, not deprecated `init-feature` prompt names. |

**Validation:** prompt smoke test, switch list fixture, no-inference menu regression.

## Sprint 2 - Switch Operation Parity

**Goal:** Preserve deterministic feature switching behavior and bounded side effects.

| Story | Description | Acceptance Criteria |
|---|---|---|
| SW-5 | Validate target feature identity. | Unsafe ids fail before path construction; feature id must exist in `feature-index.yaml`. |
| SW-6 | Return complete feature context. | Switch response includes feature id, phase, track, priority, status, owner, stale flag, context path, target repo state, and context paths. |
| SW-7 | Persist local context only. | `.lens/personal/context.yaml` records domain/service and update source; governance files remain unchanged. |
| SW-8 | Report branch checkout result. | Successful checkout reports `branch_switched: true`; failed checkout reports `branch_switched: false` and `branch_error` without fallback guessing. |
| SW-9 | Normalize dependency context. | `related` maps to summaries; `depends_on` and `blocks` map to full tech-plan docs. |

**Validation:** `test-switch-ops.py` plus a governance no-write regression.

## Sprint 3 - Release Surface And Documentation Parity

**Goal:** Ensure switch remains coherent inside the 17-command release surface.

| Story | Description | Acceptance Criteria |
|---|---|---|
| SW-10 | Align command discovery surfaces. | Module help, prompt manifests, agent menu, and docs all advertise `switch` consistently. |
| SW-11 | Document JSON contracts. | Skill references describe `list`, `domains`, `switch`, stale warnings, target repo state, and context path loading. |
| SW-12 | Wire focused regression command. | A single documented command validates switch script behavior and ExpressPlan artifact readiness. |

**Validation:** switch tests, command-surface scan, quickplan frontmatter checks, expressplan readiness validation.

## Risks And Mitigations

| Risk | Mitigation |
|---|---|
| Branch checkout makes switch feel mutating despite governance read-only language. | Document the distinction: governance remains read-only; local context and control-repo checkout are expected local side effects. |
| Domain fallback references deprecated command names. | Treat retained command naming as a parity requirement and test user-facing messages. |
| Menu state becomes implicit across turns. | Store the rendered numbered list in conversation context only; require exact numeric reply. |
| Feature-index status and feature.yaml phase drift. | Switch displays both index status and feature phase; lifecycle commands must keep governance registry updates aligned. |
