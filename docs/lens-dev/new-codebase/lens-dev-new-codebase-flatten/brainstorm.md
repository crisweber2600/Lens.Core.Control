---
feature: lens-dev-new-codebase-flatten
doc_type: brainstorm
status: draft
goal: "Explore a governance-level workflow mode that allows either the current control-repo branch/PR model or a flat control-repo mode for teams not ready for full PR discipline"
brainstorm_mode: bmad-cis-design-thinking
key_decisions:
  - Frame the request as a user-onboarding and workflow-fit problem, not as a rejection of governance itself.
  - Treat the requested behavior as a governance-repo setting that applies universally within a governance installation.
  - Scope the requested flat mode to the control repo; governance remains flat and target-project branch strategy stays a separate concern unless explicitly expanded later.
  - Preserve the current branch-and-PR workflow as the advanced/default mode and introduce flat mode as an explicit alternative.
open_questions:
  - What should the governance setting be called, where should it live, and what inheritance rules should apply?
  - Which commands must change behavior in flat mode beyond `new-feature`, `switch`, and git orchestration?
  - Does flat mode remove only branch and PR enforcement, or does it also soften phase-review requirements?
  - How should existing in-flight features behave if a governance repo changes modes after work has started?
depends_on: []
blocks: []
updated_at: 2026-05-07T00:00:00Z
---

# Brainstorm — Governance-Level Flat Mode For Control Repo Workflow

## Session Context

**Feature idea:** Lens currently bakes in a branch-and-PR workflow in multiple places. Some teams, especially new Lens users and smaller projects with interconnected services, are not ready for that level of process overhead. The request is to add a governance-level switch that makes Lens operate in one of two modes:

1. **Current mode:** existing multi-branch / PR-driven control-repo workflow.
2. **Flat mode:** no control-repo branching, no PR enforcement, and direct context persistence of `domain`, `service`, and `feature` in `.lens/personal/context.yaml`.

**Observed tension in the current system:** recent planning artifacts explicitly describe flat control topology as invalid for the control repo, while separately allowing flat strategies for target project repos. This feature would deliberately reverse that assumption, but only when configured through governance.

## Design Challenge

How might Lens support teams that need a lighter-weight control-repo workflow without fragmenting the core lifecycle model or creating ambiguity about which commands still expect branches and PRs?

## Challenge Statement

Design a governance-controlled workflow mode that lets new or low-process teams use Lens without control-repo PR and branch overhead, while preserving the current branch-based mode for teams that want full governance rigor.

## Empathize

### Primary Users

- New Lens users who are still learning the lifecycle model.
- Small teams that move quickly and do not want planning PR ceremony in the control repo.
- Teams working across several tightly-coupled services that need one coherent feature context more than repo-level workflow complexity.

### User Insights

- Users are not objecting to feature context, planning phases, or governance visibility; they are objecting to workflow friction.
- The current branch and PR model is valuable for disciplined teams but feels like forced ceremony for early adoption.
- A universal governance-level setting is preferable to per-user overrides because teams want consistent behavior across participants.
- Interconnected-service work makes branch choreography feel artificial when the real need is coherent state tracking.

### Key Observations

- The pain point is concentrated in the control repo workflow, not necessarily in target project code workflows.
- Existing Lens behavior already writes session context and tracks feature lifecycle state; the request is mainly about reducing git/process coupling.
- Commands such as `switch` and the git orchestration layer currently assume a plan branch or other derived control-repo branch exists.
- A mode switch will only be credible if it is enforced centrally and reflected consistently across every command that touches branch state.

### Empathy Map

**Says:** “I want the Lens structure, but not the PR ceremony.”

**Thinks:** “I should be able to keep domain, service, and feature context without juggling multiple control branches.”

**Does:** Works across several connected repos or services, often with small teams and fast iteration.

**Feels:** Lens is promising, but current workflow discipline may be too heavy for the team’s maturity level.

## Define

### Point of View Statement

New Lens teams need a lighter-weight control-repo operating mode because the current branch/PR workflow adds coordination overhead before they have enough process maturity to benefit from it.

### How Might We Questions

- How might we let governance choose a flat control-repo mode without forking Lens behavior into two unrelated products?
- How might we disable control-repo branch and PR requirements while preserving feature identity and lifecycle state?
- How might we make flat mode explicit enough that commands do not silently half-assume branches still exist?
- How might we keep the advanced workflow intact so established teams lose nothing?
- How might we prevent confusion between control-repo flat mode and already-separate target-project branch strategy settings?

### Problem Insights

- This is a workflow-fit problem, not a missing-feature problem.
- The real design boundary is command behavior: one governance toggle must flow into `new-feature`, `switch`, lifecycle conductors, and git orchestration consistently.
- The biggest risk is hybrid ambiguity, where some commands honor flat mode and others still require plan/dev branches or PR state.
- The second biggest risk is scope creep into target-project branching, which should stay separate unless explicitly designed together.

## Ideate

### Ideation Methods

- Design Thinking “How Might We” framing for user-centered solution space.
- Assumption busting around the current belief that flat control topology is always invalid.
- Constraint-first ideation to distinguish what must remain governed from what can become optional.

### Generated Ideas

1. Add a governance-level `control_repo_workflow_mode` setting with values like `structured` and `flat`.
2. Keep current workflow as default and make flat mode opt-in.
3. Teach `switch` to write context without attempting branch checkout in flat mode.
4. Teach `new-feature` to skip plan/dev branch creation in flat mode.
5. Mark PR-related states as `not-required` in flat mode instead of leaving them null/ambiguous.
6. Add one shared resolver so every command reads the same workflow mode rather than reimplementing logic.
7. Keep lifecycle phases unchanged even when git topology is flattened.
8. Add explicit warnings when a user tries to run a branch-dependent operation while governance is in flat mode.
9. Introduce a compatibility matrix documenting command behavior per mode.
10. Allow flat mode only for control repo operations, not governance repo publishing semantics.
11. Preserve review artifacts but remove PR creation as the enforcement mechanism in flat mode.
12. Add a migration check that blocks mode changes while a feature is mid-finalizeplan or mid-dev handoff.
13. Store effective workflow mode in command outputs for transparency and debugging.
14. Add tests that prove current behavior remains unchanged in structured mode.
15. Add tests that prove branch checkout and PR calls are suppressed in flat mode.
16. Keep target-project branch strategy independent, even when control repo is flat.
17. Expose the resolved mode in `context.yaml` or command output for easier support diagnostics.
18. Provide onboarding copy that recommends flat mode for early teams and structured mode for disciplined review-centric teams.

### Top Concepts

#### Concept A — Governance-Resolved Workflow Mode

One governance setting determines whether the control repo uses the existing branch/PR topology or a flat direct-workflow topology. Every command reads the same resolved mode through a shared helper.

**Why it stands out:** central, testable, and consistent with the user request for universal configuration.

#### Concept B — Command Behavior Matrix

Document and implement exact per-command behavior in each mode, especially for `new-feature`, `switch`, `preplan`, `finalizeplan`, `dev`, and git orchestration.

**Why it stands out:** reduces hidden assumptions and makes the mode switch operational instead of rhetorical.

#### Concept C — Review Without PR Dependence

In flat mode, preserve lifecycle state and review artifacts but decouple them from branch/PR enforcement.

**Why it stands out:** keeps Lens recognizable while removing the specific pain point users reported.

## Prototype Approach

Use a document-first prototype before code changes:

- Define the governance setting and where it is resolved.
- Map command behavior in both modes.
- Walk a representative flow: `new-feature` → `switch` → `preplan` → `dev` under flat mode.
- Identify every branch/PR assumption that breaks under the new mode.

## Prototype Description

The first prototype should not be “just add a flag.” It should be a small behavior contract showing:

- where the mode lives in governance,
- how commands resolve it,
- what changes in control-repo git behavior,
- what stays the same in lifecycle semantics,
- and what is explicitly out of scope.

## Features To Test In The Prototype

- Governance-level mode resolution is universal and deterministic.
- `switch` writes `domain`, `service`, and `feature_id` context without branch checkout in flat mode.
- `new-feature` does not create control-repo feature branches in flat mode.
- PR-creation paths are skipped or marked not required in flat mode.
- Structured mode remains behaviorally unchanged.
- Target-project branch strategy continues to behave independently.

## Testing Plan

Test the concept with representative user journeys rather than isolated flags:

1. New user creates a feature in flat mode and moves through early planning without branch prompts.
2. Existing disciplined team uses structured mode and sees no regression.
3. Multi-service team switches features and retains reliable context without control-repo checkout behavior.
4. A command that currently assumes plan/dev branch existence surfaces a clear flat-mode-safe behavior instead of failing ambiguously.

## Key Learnings To Carry Into Research

- The request is valid only if mode resolution is centralized.
- The existing doctrine that flat control topology is invalid is now the primary design constraint to revisit.
- The design must explicitly separate control-repo workflow mode from target-project branching strategy.
- The most important follow-up is technical research: identify every branch/PR enforcement point across the current new codebase and classify which ones must be bypassed, rewritten, or preserved.

## Recommended Next Step

Proceed with **technical research** focused on the current enforcement points for control-repo branching, branch checkout, PR creation, and mode-dependent context persistence.