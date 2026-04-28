---
feature: lens-dev-new-codebase-techplan
doc_type: tech-plan
status: draft
goal: "Describe the clean-room implementation plan for the techplan command in the new codebase target project."
key_decisions:
  - Implement the public chain as stub to release prompt to bmad-lens-techplan under TargetProjects/lens-dev/new-codebase/lens.core.src.
  - Reuse shared publish-before-author, batch, and adversarial-review contracts instead of embedding new copies.
  - Treat reviewed businessplan artifacts and the authoritative PRD as hard prerequisites for architecture authoring.
  - Keep governance publication inside bmad-lens-git-orchestration only.
open_questions: []
depends_on:
  - business-plan
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/architecture.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-3-rewrite-techplan.md
blocks:
  - Shared target-project dependencies such as bmad-lens-git-orchestration, bmad-lens-bmad-skill, and bmad-lens-adversarial-review must exist before end-to-end execution can pass.
updated_at: 2026-04-28T00:00:00Z
---

# Tech Plan - Techplan Command

## Overview

This feature plans the missing `techplan` command for `TargetProjects/lens-dev/new-codebase/lens.core.src`. The implementation target is not the workspace-root release payload. It is the target-project source tree that will eventually carry the rewritten command surface. The command must behave as the full-track `techplan` conductor described in the baseline rewrite corpus, not as an express shortcut or a standalone architecture generator.

The current target-project surface confirms the gap: it contains `_bmad/lens-work/skills/bmad-lens-preflight`, `_bmad/lens-work/skills/bmad-lens-init-feature`, and `_bmad/lens-work/skills/bmad-lens-complete`, but not `bmad-lens-businessplan`, `bmad-lens-techplan`, or `bmad-lens-expressplan`. This feature therefore plans both the missing public entry chain and the minimal orchestration behavior required for parity.

## Clean-Room Interpretation Rule

The old-codebase prompt input available in this workspace is a stub. Its only actionable information is that `lens-techplan` resolves to an owning `bmad-lens-techplan` skill. Functional behavior is therefore derived from the baseline rewrite PRD, architecture, research, and rewrite stories, not from copied old skill text. Any implementer should treat those baseline docs as the normative clean-room contract and use the old prompt only to verify the public chain shape.

## Target Implementation Surface

The implementation should create or update these files in the target project:

1. `TargetProjects/lens-dev/new-codebase/lens.core.src/.github/prompts/lens-techplan.prompt.md`
2. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-techplan.prompt.md`
3. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md`
4. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-techplan/scripts/` for any phase-specific ops or focused tests
5. Any registry, installer, or discovery surfaces required to expose `techplan` as part of the retained public command set

The command should live beside the existing target-project skill directories under `_bmad/lens-work/skills/` and follow the same prompt-chain topology used by the retained Lens commands.

## Required Runtime Flow

### 1. Entry Stub

The public stub must run the shared prompt-start preflight before loading the release prompt. This keeps prompt-start behavior aligned with the stable Lens command surface.

### 2. Release Prompt Redirect

The target-project release prompt should be a thin redirect that loads `bmad-lens-techplan/SKILL.md`. It should not embed orchestration logic directly.

### 3. Owning Skill Activation

`bmad-lens-techplan` should:

1. Resolve feature context and the staged docs root for `lens-dev-new-codebase-techplan`.
2. Validate that the feature is on the full planning path.
3. Confirm or inherit mode according to the shared batch and next-handoff contracts.
4. Run the shared review-ready fast path check before authoring.
5. Publish reviewed `businessplan` artifacts through `bmad-lens-git-orchestration publish-to-governance --phase businessplan`.
6. Validate that the authoritative PRD is present and must be referenced by architecture output.
7. Delegate architecture authoring through `bmad-lens-bmad-skill` to the architecture generator.
8. Run adversarial review and write the staged review artifact.
9. Return control without performing direct governance writes outside the publish hook.

### 4. Output Rules

The command's staged outputs remain local to the feature docs path until the next lifecycle handoff. For parity with the baseline rewrite contract, the technical authoring surface must preserve:

- local staging as the default write mode
- governance mirror publication only through `publish-to-governance`
- PRD dependency enforcement before architecture generation
- review-gated completion rather than silent promotion

## Data Contracts And Preconditions

### Feature State

- Feature id remains `lens-dev-new-codebase-techplan`.
- Track remains `full`; this feature does not use the express path.
- The feature docs root is the authoritative staging location for these planning artifacts.

### Shared Dependencies

The target-project implementation should rely on, not re-clone, these shared contracts:

- `bmad-lens-git-orchestration` for publish-before-author governance publication
- `bmad-lens-bmad-skill` for architecture-authoring delegation
- `bmad-lens-adversarial-review` for the lifecycle review gate
- the shared review-ready validation path rather than phase-local copies
- the constitution resolver for gating and context loading

If those dependencies are not yet present in the target project, the implementation should scaffold compatibility surfaces or sequence this feature behind the prerequisite work rather than duplicating their behavior inside `bmad-lens-techplan`.

## Implementation Sequence

1. Add the prompt stub and release prompt redirect in the target project.
2. Scaffold `bmad-lens-techplan` as a conductor-only skill.
3. Wire feature-context loading and staged-docs resolution.
4. Add the publish-before-author entry hook for `businessplan` artifacts.
5. Add PRD presence and reference enforcement for architecture generation.
6. Delegate to the architecture BMAD skill with Lens context passed through.
7. Add adversarial-review handoff and result handling.
8. Add focused tests and parity checks.

This sequence keeps the command surface installable early while leaving risky shared behavior in the shared utilities where it belongs.

## Validation Plan

### Contract Checks

- Stub invokes the shared prompt-start preflight before redirecting.
- Release prompt loads the owning skill and nothing else.
- Owning skill stays conductor-only and does not author governance files directly.

### Behavior Checks

- `publish-to-governance --phase businessplan` runs before architecture authoring starts.
- Architecture generation fails when the authoritative PRD is absent.
- Generated architecture content is required to reference the PRD.
- Review-ready fast path behaves consistently with the shared validator contract.

### Regression Checks

- Wrapper-equivalence checks stay green for architecture delegation.
- Governance-write audit shows no direct governance writes from `techplan`.
- Architecture-reference regression remains green.

## Delivery Boundaries

This feature should not attempt to solve FinalizePlan, ExpressPlan, or unrelated prompt-surface gaps as part of the same implementation slice. Its job is to restore the full-track `techplan` conductor with the right contracts so that later planning and delivery stages can depend on it safely.

## Definition Of Done

The feature is technically complete when the target project contains a working `techplan` prompt chain, the command honors publish-before-author and PRD reference rules, and the focused regressions named above pass without requiring direct governance writes or clean-room exceptions.---
feature: lens-dev-new-codebase-techplan
doc_type: tech-plan
status: draft
goal: "Define the implementation plan for adding the governed techplan command surface to the new codebase target project."
key_decisions:
  - Implement the full prompt chain in the target project: public stub, release prompt, and owning skill.
  - Reuse the shared publish-before-author, adversarial-review, and BMAD delegation contracts instead of inventing techplan-specific mutations.
  - Preserve the PRD reference rule as a hard validation requirement for architecture output.
  - Treat missing upstream shared utilities as prerequisites rather than re-solving them inside techplan.
open_questions: []
depends_on:
  - business-plan
  - rewrite-businessplan
  - constitution-partial-hierarchy-fix
blocks: []
updated_at: 2026-04-28T00:00:00Z
---

# Tech Plan — Techplan Command

## Overview

The new codebase target project currently lacks the entire `techplan` execution surface. There is no public stub for `techplan`, no release prompt, and no owning `bmad-lens-techplan` skill under `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/`. This plan adds that missing vertical slice while preserving the retained-command behavior defined by the rewrite baseline.

The implementation target is the target project source tree:

- `TargetProjects/lens-dev/new-codebase/lens.core.src/.github/prompts/`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/`

The docs in this feature folder remain the staging authority for planning. The code changes themselves belong in the target project.

## Current State

### Present in the Target Project

- `_bmad/lens-work/skills/bmad-lens-preflight/`
- `_bmad/lens-work/skills/bmad-lens-init-feature/`
- `_bmad/lens-work/skills/bmad-lens-complete/`
- `.github/prompts/lens-new-domain.prompt.md`

### Missing for This Feature

- `.github/prompts/lens-techplan.prompt.md`
- `_bmad/lens-work/prompts/lens-techplan.prompt.md`
- `_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md`
- Any `techplan`-specific focused regression coverage in the target project

This confirms that the feature is not an incremental tweak. It is the introduction of a missing retained-command conductor that must integrate with shared Lens lifecycle contracts.

## Architecture Constraints

1. `techplan` remains a conductor. It does not author architecture directly.
2. Phase entry must publish reviewed `businessplan` artifacts before authoring starts.
3. Architecture generation must remain delegated through the Lens BMAD wrapper to `bmad-create-architecture`.
4. The architecture artifact must reference the authoritative PRD.
5. Governance writes are prohibited except through the shared `publish-to-governance` path.
6. The feature docs path remains the authoritative staging root for locally staged planning artifacts.

## Proposed Implementation

### 1. Public Prompt Stub

Create `TargetProjects/lens-dev/new-codebase/lens.core.src/.github/prompts/lens-techplan.prompt.md`.

Responsibilities:

- Run `light-preflight.py` from the target project root before loading the release prompt.
- Stop immediately on a non-zero preflight result.
- Redirect to the release prompt only after prompt-start sync succeeds.

This keeps the public command surface aligned with the retained-command prompt contract.

### 2. Release Prompt

Create `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-techplan.prompt.md`.

Responsibilities:

- Act as a thin redirect to `bmad-lens-techplan/SKILL.md`.
- Avoid embedding orchestration logic directly in the prompt file.
- Preserve the same separation used throughout the retained command surface: stub -> prompt -> skill.

### 3. Owning Skill

Create `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md`.

Responsibilities:

1. Resolve the active feature and governance context.
2. Confirm the feature is on the full planning path, not the express path.
3. Resolve the staged docs path from feature context and treat it as authoritative.
4. Run the publish-before-author entry hook for `businessplan` artifacts.
5. Delegate architecture authoring through the Lens BMAD wrapper rather than inlining authoring logic.
6. Enforce the architecture reference contract so the output explicitly names the PRD it is implementing.
7. Invoke the adversarial review gate before allowing phase completion.

This skill should stay thin. The point of the rewrite is to centralize shared contracts, not to recreate them phase by phase.

### 4. Shared Dependencies Assumed, Not Reimplemented Here

The feature should treat the following as prerequisites owned by other rewrite work:

- shared publish hook from git orchestration
- shared constitution resolution that tolerates partial hierarchy
- shared adversarial-review gate
- shared BMAD wrapper routing
- businessplan command rewrite that produces the reviewed predecessor set

If any prerequisite is absent in the target project at implementation time, the correct response is to wire to the shared dependency or block on it, not to build a local duplicate inside `bmad-lens-techplan`.

## Execution Flow

1. User invokes `techplan` from the published prompt surface.
2. Public stub runs `light-preflight.py`.
3. Release prompt loads `bmad-lens-techplan/SKILL.md`.
4. Skill resolves feature context and validates that full-track techplan is appropriate.
5. Skill publishes reviewed `businessplan` artifacts through the shared publish hook.
6. Skill delegates architecture generation through `bmad-lens-bmad-skill` to `bmad-create-architecture`.
7. Generated architecture is checked for explicit PRD reference.
8. Skill runs the adversarial review gate for `techplan`.
9. Only after a passing or warning-level verdict can the lifecycle continue toward finalizeplan.

## Validation Strategy

### Required Focused Checks

- Prompt-start regression: stub runs preflight and stops on failure.
- Publish-before-author regression: `businessplan` artifacts publish before architecture generation begins.
- PRD reference regression: architecture output fails validation when the PRD reference is missing.
- Governance write audit: direct governance writes from `techplan` are rejected.
- Wrapper-equivalence regression: delegation still routes through the Lens BMAD wrapper rather than bypassing it.

### Review Expectations

- `techplan` output remains reviewable through the shared adversarial-review contract.
- Any new tests should stay narrow and behavior-scoped, matching the retained-command parity model.

## Implementation File Plan

| Path | Purpose |
| --- | --- |
| `TargetProjects/lens-dev/new-codebase/lens.core.src/.github/prompts/lens-techplan.prompt.md` | public entry stub |
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-techplan.prompt.md` | release prompt redirect |
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md` | owning conductor contract |
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-techplan/scripts/` | optional focused ops or validation helpers only if the skill truly needs them |
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-techplan/scripts/tests/` | focused regressions for parity obligations |

## Risks and Controls

| Risk | Control |
| --- | --- |
| Upstream shared utilities are missing in the target project | Keep them as explicit prerequisites in the implementation plan instead of duplicating behavior in `techplan` |
| The target project adds `techplan` but skips PRD traceability | Make PRD reference validation a mandatory acceptance check |
| The command surface is added without the release prompt indirection | Require the full three-hop prompt chain in implementation review |
| Businessplan publication is treated as optional | Wire the shared publish hook at skill entry and cover ordering with regression tests |

## Clean-Room Boundary

The old-codebase prompt supplied to this task is a stub and provides only the existence of the command surface. The normative behavior for the rewrite comes from the approved baseline artifacts, especially story 4.3, the rewrite architecture, the PRD retained-command matrix, and the research record. Implementation work should use those documents as the source of truth and use old-codebase material only to verify externally observable parity.