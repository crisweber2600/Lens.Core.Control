---
feature: lens-dev-new-codebase-techplan
doc_type: sprint-plan
status: draft
goal: "Sequence the expressplan-scoped delivery work for the techplan rewrite into implementation-ready slices."
key_decisions:
  - Use three delivery slices: express path alignment, target-project command surface, and parity hardening.
  - Record the governance track switch through the sanctioned `feature-yaml` flow and treat it as completed alignment work, not a future assumption.
  - Expand the first code slice to include prompt stub, release prompt, owning skill scaffolding, discovery wiring, and focused test-harness acceptance items.
  - Absorb the missing shared utility surfaces into this feature rather than waiting on sibling work for end-to-end execution.
  - Defer unrelated retained-command work to their own features.
open_questions:
  - Which exact discovery file should be updated first when `lens-techplan` is exposed publicly?
  - Which focused test file should own prompt-start and wrapper-equivalence regressions?
  - In what order should the absorbed shared utility surfaces land: publish orchestration, BMAD wrapper routing, adversarial review, or constitution loading?
depends_on:
  - business-plan.md
  - tech-plan.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-3-rewrite-techplan.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-5-rewrite-expressplan.md
blocks:
  - End-to-end execution remains incomplete until this feature lands the absorbed shared utility surfaces in the target project.
updated_at: 2026-04-29T02:08:41Z
---

# Sprint Plan — Techplan Command

## Sprint Objective

Turn this feature folder into a complete expressplan packet, then hand implementation an expanded, feature-owned path for restoring `techplan` parity in the new codebase.

## Current Packet Status

- Review-complete after the adversarial responses in `expressplan-adversarial-review.md`.
- Governance track alignment is complete; the feature record now carries `track: express`.
- This does not automatically make the packet finalizeplan-ready. Finalizeplan readiness remains a separate handoff decision after the widened implementation slice is accepted.

## Delivery Slices

| Slice ID | Slice | Objective | Exit Criteria |
| --- | --- | --- | --- |
| TK-1.1-express-alignment | Slice 1 | Express path and governance alignment | Business, tech, sprint, and review artifacts are coherent; governance track alignment is applied through `feature-yaml` |
| TK-2.1-command-surface | Slice 2 | Target-project command surface | The missing `lens-techplan` stub, release prompt, owning skill, discovery wiring, and focused test-harness acceptance items are fully specified |
| TK-3.1-shared-utility-delivery | Slice 3 | Shared utility delivery, parity hardening, and handoff | Absorbed shared utility surfaces, focused regressions, and handoff conditions are defined |

## Slice 1 — Express Path Alignment

### Scope

- Rewrite `business-plan.md` so it no longer rejects the express path.
- Rewrite `tech-plan.md` so it distinguishes the planning route from the command being rebuilt.
- Add `sprint-plan.md`.
- Refresh `expressplan-adversarial-review.md` against the completed packet.

### Deliverables

- A contradiction-free expressplan artifact set in `docs/lens-dev/new-codebase/lens-dev-new-codebase-techplan`.
- Governance track alignment recorded through the sanctioned `feature-yaml` flow.

### Risks

- Reviewers may treat the express path as a change to `techplan` runtime behavior.
- The packet may still be blocked operationally until governance state is aligned.

## Slice 2 — Target-Project Command Surface

### Scope

- Create the public `lens-techplan` prompt stub in the target project.
- Create the release prompt redirect.
- Create `bmad-lens-techplan/SKILL.md` as a conductor-only skill.
- Add the retained command discovery acceptance item for `lens-techplan`.
- Add the focused test-harness acceptance item for prompt-start and wrapper-equivalence checks.

### Deliverables

- `TargetProjects/lens-dev/new-codebase/lens.core.src/.github/prompts/lens-techplan.prompt.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-techplan.prompt.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md`
- The selected discovery surface is identified as part of the first slice.
- The focused test-harness path is identified as part of the first slice.

### Dependencies

- Shared preflight behavior already present in the target project.
- The first slice can land before end-to-end execution is complete, but it must name how absorbed shared utility delivery will follow.

## Slice 3 — Shared Utility Delivery, Parity Hardening, And Handoff

### Scope

- Land the absorbed shared utility surfaces in the target project.
- Add focused regression expectations for prompt-start routing, publish ordering, PRD reference enforcement, wrapper equivalence, and no direct governance writes.
- Define parity as reproducing the same four staged artifacts with equivalent routing, gates, and delivery slices.
- Prepare the packet for finalizeplan handoff after implementation-slice acceptance rather than after governance track correction.

### Deliverables

- Shared publication, wrapper, review, and constitution surfaces are explicitly sequenced inside this feature.
- A named regression list with owners and expected command surfaces.
- A go or no-go checklist for moving from staged planning to code implementation.

### Exit Gate

- Shared utility delivery is explicitly sequenced inside this feature.
- Adversarial review carries no unresolved fail-level findings.
- Packet status is review-complete; finalizeplan readiness is assessed separately.

## Critical Path

1. Land the target-project `techplan` prompt chain, discovery wiring, and focused test-harness foundation.
2. Land the absorbed shared utility surfaces in the target project.
3. Add focused parity checks against the four staged artifacts.
4. Decide finalizeplan handoff after the widened implementation slice is accepted.

## Definition Of Ready For Implementation

The feature is ready for code work when:

1. The expressplan artifact set is complete.
2. Governance track alignment is already applied through the sanctioned `feature-yaml` flow.
3. The target-project file plan is accepted, including discovery wiring and first-slice test-harness acceptance items.
4. The absorbed shared utility surfaces are explicitly in scope.
5. The initial regression harness is identified.