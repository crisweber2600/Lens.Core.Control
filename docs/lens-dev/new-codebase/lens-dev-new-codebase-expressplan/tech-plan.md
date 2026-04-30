---
feature: lens-dev-new-codebase-expressplan
doc_type: tech-plan
status: draft
goal: "Define the implementation and validation plan for the expressplan rewrite while aligning this feature folder to the expressplan artifact contract."
key_decisions:
  - Keep the implementation target focused on lens-expressplan; expressplan is both the planning wrapper for this feature and the runtime behavior being rebuilt.
  - The conductor-only constraint applies: the rewritten skill delegates all planning and review work; it does not implement planning logic inline.
  - Treat the previous-session infrastructure (prompt stubs, SKILL.md, test file, module.yaml) as a complete foundation layer that must be validated, not reworked.
  - Absorb discovery wiring into this feature's scope rather than treating it as a future follow-up.
  - Define parity as reproducing the same four staged artifacts with equivalent routing, gates, and delivery slices as a future automation pass would expect.
open_questions:
  - Which exact target-project discovery file should register lens-expressplan first?
  - Does the bmad-lens-expressplan/SKILL.md need any script-level ops beyond validate-phase-artifacts.py, or is the conductor fully self-contained?
  - What sequence best lands express-eligibility gating, QuickPlan delegation, adversarial review, and phase-advance without obscuring the command-surface work?
depends_on:
  - business-plan.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-5-rewrite-expressplan.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-techplan/docs/tech-plan.md
blocks:
  - End-to-end expressplan execution remains incomplete until discovery wiring and integration-level regressions are in place.
updated_at: '2026-04-30T00:00:00Z'
---

# Tech Plan — Expressplan Command

## Overview

The implementation target is the expressplan command surface in
`TargetProjects/lens-dev/new-codebase/lens.core.src`. The previous session landed the skill
infrastructure. This tech plan validates that foundation and defines the remaining delivery:
discovery wiring, focused regression expectations, and the expressplan packet needed for
FinalizePlan handoff.

The expressplan command is both the feature planning route and the runtime behavior being
rebuilt. That dual role is this plan's core distinction from `lens-dev-new-codebase-techplan`.
There, express planning was used to plan a different command. Here, the feature is literally
building the expressplan command using expressplan as its own planning path.

## Implementation Surface

The following files in the target project constitute the expressplan command surface:

| File | Role | Status |
| --- | --- | --- |
| `TargetProjects/lens-dev/new-codebase/lens.core.src/.github/prompts/lens-expressplan.prompt.md` | Public VS Code stub | Created in previous session |
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-expressplan.prompt.md` | Release prompt redirect | Created in previous session |
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md` | Conductor skill | Created in previous session |
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-expressplan/scripts/tests/test-expressplan-ops.py` | Focused contract tests | Created in previous session |
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/module.yaml` | Module registration | Updated in previous session |
| Discovery surface registration | Retained command visibility | Not yet wired |

## Planning Path vs. Implementation Target

| Concern | Required behavior |
| --- | --- |
| Planning path | Stage `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, and `expressplan-review.md` as a complete expressplan packet |
| Runtime contract under rewrite | Enforce express-only eligibility, delegate to QuickPlan via Lens wrapper, enforce hard-stop adversarial review, auto-advance to FinalizePlan on pass |
| Governance state | Already aligned: `track: express`, `phase: expressplan` via sanctioned `feature-yaml` flow |
| Code delivery target | Complete the expressplan command surface, wire discovery, and define regression coverage |

## Clean-Room Interpretation Rule

The old-codebase prompt stub's only actionable information is the public chain shape:
`lens-expressplan` resolves through a release prompt to `bmad-lens-expressplan/SKILL.md`.
Functional behavior is defined by baseline story 4.5. No old prompt prose is reused in the
conductor skill.

## Required Runtime Behavior

The rewritten expressplan command must preserve the baseline 4.5 contract:

1. Non-express features are blocked before QuickPlan delegation begins.
2. Express-eligible features delegate planning to `bmad-lens-quickplan` via `bmad-lens-bmad-skill`.
3. The adversarial review with party-mode blind-spot challenge is a hard gate; a fail verdict
   blocks auto-advance to FinalizePlan.
4. On a pass or pass-with-warnings verdict, the phase is updated to `expressplan-complete` and
   FinalizePlan is signalled.
5. The public stub runs the shared prompt-start preflight before loading the release prompt.
6. The release prompt stays a thin redirect to `bmad-lens-expressplan/SKILL.md`.

## Foundation Layer Validation

The previous session landed the skill infrastructure. Validation confirms:

- Public stub follows the shared prompt-start preflight pattern (analogous to `lens-techplan`).
- Release prompt redirects to `bmad-lens-expressplan/SKILL.md` without inline logic.
- SKILL.md implements the three-step conductor contract: QuickPlan delegation → adversarial
  review gate → phase advance.
- SKILL.md enforces the express-only eligibility gate before delegation.
- `module.yaml` registers `lens-expressplan.prompt.md` in the prompts list.
- `test-expressplan-ops.py` covers prompt-start routing and wrapper equivalence.

## Remaining Implementation Scope

### 1. Discovery Wiring

The `lens-expressplan` command must be registered in the new-codebase discovery surface.
The discovery surface is the same retained command manifest that other express-track commands
use. The exact file (skill manifest, module help CSV, or agent manifest) is to be confirmed
as part of the first implementation slice.

### 2. Focused Regression Expectations

Beyond the existing `test-expressplan-ops.py`, the following regression checks must be defined:

- Express-eligibility gate blocks non-express features before QuickPlan delegation.
- QuickPlan delegation route resolves to `bmad-lens-bmad-skill --skill bmad-lens-quickplan`.
- Adversarial review invocation passes `--phase expressplan --source phase-complete`.
- Phase-advance invocation updates `feature.yaml` phase to `expressplan-complete`.
- Public stub chain: prompt-start → release prompt → SKILL.md (no inline logic at stub level).

### 3. Module Registration Confirmation

Confirm `module.yaml` now lists `lens-expressplan.prompt.md` and the entry follows the same
shape as other retained-command registrations in the file.

## Implementation Sequence

### Workstream 1 — Foundation Validation

1. Read and confirm the created public stub follows prompt-start preflight pattern.
2. Read and confirm the release prompt is a thin redirect.
3. Read and confirm the SKILL.md conductor implements the three-step contract and eligibility gate.
4. Confirm `module.yaml` registration shape is correct.

### Workstream 2 — Discovery Wiring

1. Identify the retained command discovery file in the target project.
2. Register `lens-expressplan` in that file following the existing pattern.
3. Verify the command appears in the expected help or manifest output.

### Workstream 3 — Regression and Validation

1. Confirm `test-expressplan-ops.py` covers prompt-start and wrapper-equivalence.
2. Add or document regression expectations for eligibility gate, delegation route, and phase advance.
3. Verify all regression expectations pass.

### Workstream 4 — Packet Completion and Handoff

1. Produce the full expressplan planning packet (this document set).
2. Run the expressplan adversarial review gate.
3. On pass: advance phase to `expressplan-complete`, commit artifacts, signal FinalizePlan.

## Validation Plan

### Contract Checks

- The public stub, release prompt, and conductor skill each remain in their respective
  separated files (chain separation is preserved).
- The conductor is gate-only: no inline planning logic.
- Express-only eligibility is checked before QuickPlan delegation.

### Behavior Checks

- `bmad-lens-bmad-skill --skill bmad-lens-quickplan` is the QuickPlan delegation route.
- `bmad-lens-adversarial-review --phase expressplan --source phase-complete` is the review invocation.
- Phase-advance succeeds when the review verdict is `pass` or `pass-with-warnings`.
- Phase-advance does not run when the verdict is `fail`.

### Regression Checks

- Eligibility gate behavior (non-express features are blocked).
- Delegation route integrity (no inline QuickPlan bypass).
- Review-gate enforcement (no advance on fail verdict).
- Discovery surface registration (command appears in the retained manifest).
