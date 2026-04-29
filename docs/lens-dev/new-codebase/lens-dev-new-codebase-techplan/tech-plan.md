---
feature: lens-dev-new-codebase-techplan
doc_type: tech-plan
status: draft
goal: "Define the implementation and validation plan for the techplan rewrite while aligning this feature folder to the expressplan artifact contract."
key_decisions:
  - Keep the implementation target focused on `lens-techplan`; expressplan is the planning wrapper for this feature, not the runtime behavior being rebuilt.
  - Complete the artifact set with `sprint-plan.md` and a refreshed review rather than leaving the folder in a partial express state.
  - Preserve the target-project implementation slice: public stub, release prompt, owning skill, discovery wiring, and a focused parity/test harness foundation.
  - Absorb the missing shared utility surfaces into this feature instead of leaving end-to-end execution behind external prerequisites.
  - Define parity as reproducing the same four staged artifacts with equivalent routing, gates, and delivery slices.
open_questions:
  - Which exact target-project discovery file should register `lens-techplan` first?
  - Which exact focused test file should own prompt-start and wrapper-equivalence regressions?
  - What sequence best lands publish orchestration, BMAD wrapper routing, adversarial review, and constitution loading inside this feature without obscuring the command-surface work?
depends_on:
  - business-plan.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-3-rewrite-techplan.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-5-rewrite-expressplan.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/architecture.md
blocks:
  - End-to-end execution remains incomplete until this feature lands the shared publication, wrapper, review, and constitution surfaces now included in scope.
updated_at: 2026-04-29T02:08:41Z
---

# Tech Plan — Techplan Command

## Overview

The implementation target remains the missing `techplan` execution surface in `TargetProjects/lens-dev/new-codebase/lens.core.src`. The planning packet around it, however, now follows the expressplan contract so the feature folder can act as a parity target for the future `bmad-lens-expressplan` rewrite. The practical consequence is simple: the code slice still rebuilds `lens-techplan`, while the staged docs must look like a complete expressplan output set that a future automation path can reproduce.

## Planning Path Versus Implementation Target

| Concern | Required behavior |
| --- | --- |
| Planning path | Stage `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, and `expressplan-adversarial-review.md` as a complete expressplan packet |
| Runtime contract under rewrite | Restore governed `techplan` behavior: publish-before-author, PRD reference enforcement, conductor-only delegation |
| Governance state gap | Resolved via the sanctioned `feature-yaml` flow; the feature record now carries `track: express` |
| Code delivery target | Add the missing `lens-techplan` prompt chain and owning skill in `TargetProjects/lens-dev/new-codebase/lens.core.src` |

This split is the core technical decision for the feature. The express path changes how the plan is packaged. It does not change what the rewritten command must do.

## Clean-Room Interpretation Rule

The old-codebase prompt input available in this workspace is a stub. Its only actionable information is the public chain shape: `lens-techplan` resolves to an owning `bmad-lens-techplan` skill. Functional behavior therefore comes from the baseline rewrite PRD, architecture, research, and stories 4.3 and 4.5. No old skill prose is reused.

## Target Implementation Surface

The eventual implementation should create or update these files in the target project:

1. `TargetProjects/lens-dev/new-codebase/lens.core.src/.github/prompts/lens-techplan.prompt.md`
2. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-techplan.prompt.md`
3. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md`
4. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-techplan/scripts/` for any focused ops or validation helpers that are truly phase-specific
5. Any discovery surface needed to expose `techplan` as part of the retained command set

The planning artifacts remain staged under `docs/lens-dev/new-codebase/lens-dev-new-codebase-techplan`. The code changes themselves belong in the target project.

## Required Runtime Behavior

The rewritten `techplan` command must preserve the baseline 4.3 contract even though this feature is planned through expressplan:

1. The public stub runs the shared prompt-start preflight before loading the release prompt.
2. The release prompt stays a thin redirect to `bmad-lens-techplan/SKILL.md`.
3. The owning skill resolves feature context and staged docs without writing governance files directly.
4. Reviewed `businessplan` artifacts are published before architecture authoring begins.
5. Architecture generation fails if the authoritative PRD cannot be located or referenced.
6. Architecture authoring is delegated through the Lens BMAD wrapper rather than implemented inline.
7. Adversarial review remains the gate before the feature can move toward finalizeplan.

The express planning route adds packaging and gating from story 4.5. It does not loosen any of the `techplan` runtime obligations above.

## Expressplan Alignment Tasks

### 1. Artifact Completeness

This folder must contain the full expressplan artifact set. Before this patch, the sprint plan was missing and the business and technical plans were internally contradictory about track usage. That gap is now part of the technical scope because parity cannot be measured against a partial packet.

### 2. Governance Alignment

The feature record has been aligned to `track: express` through the sanctioned `feature-yaml` flow. That closes the specific governance-track mismatch that originally prevented this packet from being replayed as a literal expressplan path.

### 3. Review Gate

The adversarial review remains part of the expressplan packet. A future automation path should be able to recreate the staged review findings from these artifacts and stop on a fail verdict.

## Shared Utility Surfaces Now Owned By This Feature

The review response for H1 moves the missing shared surfaces into this feature’s implementation scope. That does not mean creating `techplan`-local forks of shared behavior. It means this feature now owns landing the canonical shared surfaces in the target project so `techplan` can execute end to end.

The absorbed utility work includes:

- `bmad-lens-git-orchestration` for publish-before-author governance publication
- `bmad-lens-bmad-skill` for architecture-authoring delegation
- `bmad-lens-adversarial-review` for the lifecycle review gate
- `scripts/validate-phase-artifacts.py` for the shared review-ready fast path
- constitution resolution needed for gating and context loading

The implementation goal remains the same: land these as shared surfaces in the target project, not as phase-local clones embedded inside `bmad-lens-techplan`.

## Implementation Sequence

### Workstream 1 — Planning Packet Alignment

1. Normalize the business and technical plans for the express path.
2. Add `sprint-plan.md` so the artifact set is complete.
3. Refresh the adversarial review against the completed packet.

### Workstream 2 — Target-Project Command Surface

1. Add the public `lens-techplan` prompt stub.
2. Add the release prompt redirect.
3. Scaffold `bmad-lens-techplan` as a conductor-only skill.
4. Wire the retained command discovery surface for `lens-techplan`.
5. Create the focused test-harness foundation for prompt-start and wrapper-equivalence checks.

### Workstream 3 — Shared Utility Delivery Owned By This Feature

1. Land the publish-before-author entry hook for reviewed `businessplan` artifacts.
2. Land BMAD wrapper routing needed for delegated architecture authoring.
3. Land the adversarial review gate in the target project.
4. Land the constitution-loading path needed for gated execution.
5. Enforce PRD presence and explicit reference in architecture output once those shared surfaces are present.

### Workstream 4 — Validation And Handoff

1. Treat parity as reproducing `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, and `expressplan-adversarial-review.md` with equivalent routing, gates, and delivery slices.
2. Add focused regressions for prompt-start routing, publish ordering, PRD reference enforcement, and wrapper equivalence.
3. Verify the retained command remains discoverable from the chosen installer/help surface.
2. Audit for direct governance writes from `techplan`.
4. Treat the packet as review-complete now that governance track alignment is closed; finalizeplan readiness is a separate handoff decision after this widened implementation slice is accepted.

## Validation Plan

### Contract Checks

- The folder now contains the full expressplan artifact set.
- The future prompt chain still follows stub -> prompt -> skill separation.
- The future owning skill remains conductor-only.
- Parity means reproducing the same four staged artifacts with equivalent routing, gates, and delivery slices.

### Behavior Checks

- `publish-to-governance --phase businessplan` runs before architecture authoring.
- Architecture generation fails when the PRD reference is missing.
- Review-ready behavior stays delegated to the shared validator.
- The express planning packet remains reproducible without direct governance writes.

### Regression Checks

- Prompt-start regression: the stub runs preflight and stops on failure.
- Publish-before-author regression: reviewed predecessor artifacts publish before authoring.
- PRD reference regression: architecture output must reference the authoritative PRD.
- Wrapper-equivalence regression: delegation stays routed through the Lens BMAD wrapper.
- Discovery regression: the command is exposed through the chosen retained discovery surface.
- Governance-write audit: `techplan` never writes governance files directly.

## Current State

### Present In The Target Project

- `_bmad/lens-work/skills/bmad-lens-preflight/`
- `_bmad/lens-work/skills/bmad-lens-init-feature/`
- `_bmad/lens-work/skills/bmad-lens-complete/`
- `.github/prompts/lens-new-domain.prompt.md`

### Missing For This Feature

- `.github/prompts/lens-techplan.prompt.md`
- `_bmad/lens-work/prompts/lens-techplan.prompt.md`
- `_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md`
- `techplan`-specific focused regression coverage in the target project

That still makes the code slice a missing retained-command conductor. The difference now is that the planning packet no longer contradicts the requested express route.

## Definition Of Done

This feature is technically ready for implementation when:

1. The feature folder contains a clean expressplan-compatible artifact set.
2. The target-project implementation slice is fully enumerated, including discovery wiring and the focused test-harness foundation.
3. The governance track switch is applied through the sanctioned `feature-yaml` flow.
4. The widened shared-utility delivery work is named explicitly as part of this feature.
5. Focused regressions are named before code work starts.

The feature is technically complete only after the target project contains a working `techplan` prompt chain, the absorbed shared utility surfaces are landed, and the parity checks above pass.