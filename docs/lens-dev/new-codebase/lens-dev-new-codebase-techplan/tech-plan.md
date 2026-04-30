---
feature: lens-dev-new-codebase-techplan
doc_type: tech-plan
status: draft
goal: "Define the implementation surface, required runtime behaviors, and validation plan for the techplan command rewrite, and align this feature folder to the expressplan artifact contract."
key_decisions:
  - Keep the implementation target focused on the `lens-techplan` conductor; expressplan is the planning packaging wrapper for this feature, not a change to the runtime command.
  - Complete the expressplan artifact set with sprint-plan.md so this folder becomes a concrete parity target for the future `bmad-lens-expressplan` skill rewrite.
  - Preserve the full target-project implementation surface: public stub, release prompt redirect, owning skill, discovery wiring, and focused parity/test harness.
  - Absorb the shared utility surfaces (publish orchestration, BMAD wrapper routing, adversarial review gate, constitution loading) as in-scope delivery rather than external prerequisites.
  - Define parity as reproducibility: the four expressplan-contract artifacts must be reproducible by a future automation path with equivalent routing, gates, and delivery slices.
open_questions:
  - Which target-project discovery file should register `lens-techplan` first in the retained command surface?
  - Which focused test file path should own the prompt-start preflight regression and wrapper-equivalence check?
  - What is the optimal delivery sequence for the shared publication, wrapper, review, and constitution surfaces so command-surface work is not obscured?
depends_on:
  - business-plan.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-3-rewrite-techplan.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-5-rewrite-expressplan.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/architecture.md
blocks:
  - End-to-end techplan execution remains blocked until this feature delivers the shared publication, wrapper, review, and constitution surfaces now included in scope.
updated_at: 2026-04-30T00:00:00Z
---

# Tech Plan — Techplan Command Rewrite

## Overview

The implementation target is the missing `techplan` execution surface in `TargetProjects/lens-dev/new-codebase/lens.core.src`. The planning packet around it follows the expressplan contract so this folder can act as a concrete parity target for the future `bmad-lens-expressplan` skill rewrite. The code slice rebuilds `lens-techplan`. The staged docs form a complete expressplan output set that a future automation path can reproduce.

## Planning Path Versus Implementation Target

| Concern | Required behavior |
| --- | --- |
| Planning path | Stage `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, and `expressplan-adversarial-review.md` as a complete expressplan packet |
| Runtime contract under rewrite | Restore governed `techplan` behavior: publish-before-author, PRD reference enforcement, conductor-only delegation |
| Governance state | Feature record carries `track: express`; governance updates flow through the sanctioned feature-yaml path only |
| Code delivery target | Create or update the `lens-techplan` prompt chain and owning skill in `TargetProjects/lens-dev/new-codebase/lens.core.src` |

This distinction is the core technical decision. The express path changes how the plan is packaged. It does not change what the rewritten command must do at runtime.

## Clean-Room Interpretation Rule

The old-codebase prompt inputs available in this workspace are stubs. The `lens-techplan` public stub provides one actionable fact: the public command chain shape — `lens-techplan` stub runs the shared preflight and then redirects to `lens.core/_bmad/lens-work/prompts/lens-techplan.prompt.md`, which redirects to the owning `bmad-lens-techplan` skill. The `lens-constitution` public stub confirms the same 3-hop chain shape for the constitution command dependency. Functional behavior for both commands derives from the baseline rewrite PRD, architecture, and stories 4.3 and 4.5. No old skill prose is reused.

## Target Implementation Surface

The implementation creates or updates these five files in the target project:

1. `TargetProjects/lens-dev/new-codebase/lens.core.src/.github/prompts/lens-techplan.prompt.md` — public stub with preflight gate and release module redirect
2. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-techplan.prompt.md` — thin release prompt redirect to `bmad-lens-techplan/SKILL.md`
3. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md` — conductor-only skill with full activation sequence
4. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-techplan/scripts/` — focused ops or validation helpers that are phase-specific to techplan only
5. Any target-project discovery surface required to expose `lens-techplan` as part of the retained 17-command set

Planning artifacts remain staged under `docs/lens-dev/new-codebase/lens-dev-new-codebase-techplan`. Code changes belong in the target project.

## Required Runtime Behavior

The rewritten `techplan` command must preserve all seven behavioral obligations from baseline story 4.3, regardless of the express planning route used for this feature:

1. **Preflight gate** — the public stub runs the shared `light-preflight.py` check and stops cleanly on non-zero exit before loading the release prompt.
2. **Thin redirect chain** — the release prompt stays a redirect to `bmad-lens-techplan/SKILL.md` with no inline instructions that duplicate skill content.
3. **Context resolution without direct governance writes** — the owning skill resolves feature context, staged docs path, and governance mirror path without creating or editing governance repo files directly; all governance writes go through the publish CLI.
4. **Publish-before-author entry hook** — reviewed `businessplan` artifacts are published to the governance docs mirror before architecture authoring begins; the publish CLI performs the copy, not the skill directly.
5. **PRD reference enforcement** — architecture generation fails if the authoritative PRD artifact cannot be located or referenced; this constraint is load-bearing and must not become optional.
6. **Conductor-only delegation** — architecture authoring is delegated through the Lens BMAD wrapper (`bmad-lens-bmad-skill`) rather than implemented inline by the techplan conductor.
7. **Adversarial review gate** — the `bmad-lens-adversarial-review` gate must pass (or pass-with-warnings) before the feature can advance toward finalizeplan; a fail verdict stops execution and blocks `feature.yaml` update.

The express planning route adds packaging and gating from story 4.5 (express eligibility check, QuickPlan delegation, hard-stop review before finalize bundling). It does not modify or relax any of the seven runtime obligations above.

## Expressplan Alignment Tasks

### 1. Artifact Completeness

This folder must contain the full expressplan artifact set. The missing `sprint-plan.md` is an in-scope deliverable for this feature. Without it, the folder cannot serve as a parity target and the expressplan contract is not satisfied.

### 2. Governance Alignment

The feature record in the governance repo carries `track: express`. The governance state for the express-track switch must be set through the sanctioned `bmad-lens-feature-yaml` flow before the expressplan packet is treated as complete.

### 3. Review Gate

The `expressplan-adversarial-review.md` artifact is produced by the `bmad-lens-adversarial-review` skill at the expressplan review gate. A failed review blocks advancement. A pass or pass-with-warnings verdict allows the feature to proceed to finalizeplan bundling.

## Shared Utility Surfaces

These shared utilities are in-scope for delivery within this feature. They must exist and be wired correctly before the techplan command can execute end-to-end:

| Utility | Role in techplan execution |
| --- | --- |
| `bmad-lens-git-orchestration` publish-to-governance | Copies reviewed businessplan artifacts to the governance docs mirror before architecture begins |
| `bmad-lens-bmad-skill` | Routes architecture creation through the Lens-aware BMAD wrapper |
| `bmad-lens-adversarial-review` | Runs the phase completion review gate; blocks `feature.yaml` update on fail |
| `scripts/validate-phase-artifacts.py` | Provides the review-ready fast path check (status=pass skips native architecture handoff) |
| `bmad-lens-constitution` | Loads the domain constitution for architectural constraints during the activation sequence |

## Implementation Sequence

### Workstream 1 — Planning Packet Alignment

Complete the expressplan artifact set: ensure business-plan.md, tech-plan.md, sprint-plan.md, and expressplan-adversarial-review.md are all present and internally consistent. Remove any contradictions between staged docs and the declared `track: express` governance state.

### Workstream 2 — Target-Project Command Surface

1. Create the public `lens-techplan` prompt stub with preflight gate and release module redirect.
2. Create the release prompt redirect pointing to `bmad-lens-techplan/SKILL.md`.
3. Build `bmad-lens-techplan` as a conductor-only skill with all seven required runtime behaviors.
4. Wire the retained command discovery surface for `lens-techplan`.
5. Establish the focused test-harness foundation for prompt-start and wrapper-equivalence regressions.

### Workstream 3 — Shared Utility Delivery

Deliver or confirm the shared utility surfaces listed above. These are prerequisite to end-to-end techplan execution and are treated as in-scope for this feature rather than as external dependencies.

### Workstream 4 — Validation and Handoff

1. Run `validate-phase-artifacts.py` against the staged docs path to verify all required artifacts pass the review-ready contract.
2. Run `bmad-lens-adversarial-review` at phase completion.
3. Verify architecture artifact references the authoritative PRD.
4. Confirm wrapper-equivalence and architecture-reference regression checks pass.
5. Update `feature.yaml` to `techplan-complete` via `bmad-lens-feature-yaml` after the review gate passes.

## Validation Plan

### Contract Checks

- `validate-phase-artifacts.py --phase techplan --contract review-ready` returns `status=pass` against the staged docs path.
- All four expressplan artifact files are present and have required YAML frontmatter fields.

### Behavior Checks

- Preflight gate exits non-zero on an unhealthy workspace and stops the chain cleanly.
- Publish-before-author: reviewed businessplan artifacts appear in the governance docs mirror before any architecture artifact is created.
- PRD reference: architecture document contains a reference to the PRD artifact; `validate-phase-artifacts.py` with the `artifact_validation.prd_reference` check returns pass.
- Conductor-only: no architecture prose is authored directly by the techplan skill; all architecture content originates from `bmad-lens-bmad-skill`.

### Regression Checks

- **Wrapper-equivalence**: the `bmad-lens-techplan` skill invocation of `bmad-lens-bmad-skill` produces equivalent results to the old-codebase delegation pattern.
- **Architecture-reference**: the generated architecture artifact consistently references the authoritative PRD regardless of feature context.
- **No direct governance writes**: no tool calls or patches create or edit files in the governance repo directly during the techplan phase.
