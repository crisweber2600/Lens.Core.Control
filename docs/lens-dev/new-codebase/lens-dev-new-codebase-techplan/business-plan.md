---
feature: lens-dev-new-codebase-techplan
doc_type: business-plan
status: draft
goal: "Define the business outcome and delivery guardrails for the clean-room techplan rewrite."
key_decisions:
  - Treat the old-codebase techplan prompt as an entry-contract reference only; do not copy implementation text.
  - Preserve publish-before-author ordering and PRD reference enforcement as the load-bearing user outcomes.
  - Stage planning artifacts under docs/lens-dev/new-codebase/lens-dev-new-codebase-techplan while targeting implementation changes in TargetProjects/lens-dev/new-codebase/lens.core.src.
  - Keep techplan as a conductor that delegates architecture authoring through the Lens BMAD wrapper.
open_questions: []
depends_on:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/prd.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-2-rewrite-businessplan.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-3-rewrite-techplan.md
blocks:
  - Shared publish-before-author and constitution-fix prerequisites must land in the target project before implementation can complete.
updated_at: 2026-04-28T00:00:00Z
---

# Business Plan - Techplan Command

## Executive Summary

The `lens-dev-new-codebase-techplan` feature exists to restore the missing `techplan` step in the new-codebase rewrite without violating the clean-room boundary. The new codebase currently lacks the `businessplan`, `techplan`, and `expressplan` conductor surfaces in `TargetProjects/lens-dev/new-codebase/lens.core.src`, which leaves the full-track planning chain incomplete. This feature closes the most technical part of that gap by defining the user-facing and governance-facing outcomes the rewritten `techplan` command must preserve.

The business outcome is continuity, not reinvention. Lens users must be able to move from reviewed business-planning artifacts into architecture authoring with the same safety properties documented in the baseline rewrite corpus: publish reviewed predecessor artifacts first, stage current outputs locally, enforce the PRD reference rule, and keep governance writes confined to `publish-to-governance`. The feature succeeds when the new-codebase `techplan` surface behaves like a governed conductor again, rather than a one-off authoring shortcut.

## Problem Statement

The baseline rewrite documents establish `techplan` as a retained published command in the 17-command stable surface. They also make three constraints explicit:

1. `techplan` remains a conductor, not a direct architecture-writing implementation.
2. The command must publish reviewed `businessplan` artifacts before architecture generation begins.
3. The architecture artifact must reference the authoritative PRD, and governance writes must remain inside the shared publish hook.

In the target project, the command surface needed to satisfy those constraints does not yet exist. The repo currently exposes only a small subset of Lens skills, so a user cannot complete the full-track path from `businessplan` into `techplan` there. Without this feature, the rewrite cannot claim parity for the planning chain, and downstream `finalizeplan` work remains blocked on an incomplete dependency graph.

## Users And Stakeholders

- Lens feature authors who depend on the full-track planning path.
- Lens maintainers responsible for parity with the old command surface.
- Governance reviewers who need traceable, staged planning artifacts.
- Future implementation agents who need a clean-room description of what to build in the target project.

## Goals

1. Re-establish the `techplan` command as a first-class step in the new-codebase planning lifecycle.
2. Preserve the baseline rewrite contract for publish-before-author ordering and PRD-referenced architecture generation.
3. Keep authoring authority explicit: stage current artifacts locally, publish only through the shared governance hook.
4. Produce a delivery plan that can be executed in the target project without copying old-codebase implementation files.

## Non-Goals

- Changing the feature to the `express` track.
- Replacing `techplan` with `expressplan` behavior for non-express features.
- Altering lifecycle schema, branch topology, or feature identity formulas.
- Designing a new architecture authoring workflow outside the established Lens wrapper contracts.

## Required Outcomes

### User-Facing Outcomes

- A Lens user can invoke `techplan` in the new codebase through the standard prompt chain.
- The command behaves as a governed transition from reviewed business-planning artifacts into architecture authoring.
- The user is not asked to accept unsafe shortcuts such as direct governance writes or bypassing the PRD dependency.

### Governance Outcomes

- Reviewed `businessplan` artifacts are published before architecture authoring begins.
- Current-phase outputs remain staged under the feature docs path until the next handoff.
- Architecture generation fails fast when the authoritative PRD is missing or cannot be referenced.

### Delivery Outcomes

- The target project gets a clearly-scoped implementation slice for the missing `techplan` surface.
- Regression expectations are explicit before code work starts.
- The planning set stays clean-room: derived from baseline rewrite requirements and public entry contracts, not copied implementation prose.

## Scope

### In Scope

- The `techplan` command's public entry contract and owning-skill behavior.
- Publish-before-author ordering on entry to `techplan`.
- Delegation from `bmad-lens-techplan` through `bmad-lens-bmad-skill` to architecture authoring.
- PRD reference enforcement and local staging rules.
- Focused regression expectations for wrapper equivalence, governance write boundaries, and architecture reference enforcement.

### Out Of Scope

- Rebuilding unrelated public commands.
- Implementing `expressplan` for this feature; the current feature is `track: full` and remains on the full planning path.
- Solving every missing target-project dependency inside this single feature if those dependencies belong to prerequisite features.

## Clean-Room Source Packet

This plan is derived from the following approved inputs:

- The public old-codebase `lens-techplan` prompt stub, which confirms the prompt-chain entry contract but does not provide reusable implementation text.
- The baseline rewrite PRD, which fixes `techplan` as a retained command in the stable surface.
- Baseline rewrite story `4-2`, which defines the predecessor `businessplan` publish-before-author contract.
- Baseline rewrite story `4-3`, which defines the `techplan` acceptance criteria.
- Baseline rewrite architecture and research documents, which define the retained command topology, shared utilities, and dependency ordering.

## Success Criteria

This feature is successful when:

1. The new-codebase target project has an implementable plan for the missing `techplan` command.
2. The plan preserves the baseline user contract instead of creating a simplified but incompatible substitute.
3. The technical plan names the exact target-project files and tests needed to deliver parity.
4. The feature can advance into implementation without reopening the clean-room question.

## Risks And Mitigations

| Risk | Impact | Mitigation |
| --- | --- | --- |
| Upstream shared utilities are not yet present in the target project | Implementation stalls or duplicates shared logic | Treat shared publish hook, constitution fix, and wrapper plumbing as hard prerequisites |
| `techplan` is implemented as a direct authoring flow | Governance parity is broken | Keep conductor-only scope explicit in the technical plan |
| PRD reference enforcement becomes optional | Architecture artifacts drift from approved business intent | Make PRD presence and reference validation part of command entry and regression coverage |
| The feature is forced down the `expressplan` path | Full-track lifecycle semantics are violated | Keep this feature on the full path and treat expressplan gating as a separate contract |

## Exit Decision

Proceed to implementation only if the target project is ready to add the missing prompt, prompt redirect, owning skill, and focused regressions under `TargetProjects/lens-dev/new-codebase/lens.core.src`. If those prerequisite surfaces are still absent, implementation should begin by scaffolding them there rather than redirecting work into governance or the release payload.---
feature: lens-dev-new-codebase-techplan
doc_type: business-plan
status: draft
goal: "Define the business outcomes and delivery boundaries for the techplan command rewrite in the new codebase."
key_decisions:
  - Preserve techplan as a Lens conductor rather than moving architecture authoring into the command itself.
  - Keep publish-before-author ordering as a user-visible governance guarantee.
  - Treat the old-codebase prompt as a verification reference only and derive the implementation contract from approved baseline docs.
  - Stage planning artifacts in the feature docs path while targeting implementation changes in TargetProjects/lens-dev/new-codebase/lens.core.src.
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
  - rewrite-businessplan
  - publish-before-author-hook
blocks: []
updated_at: 2026-04-28T00:00:00Z
---

# Business Plan — Techplan Command

## Executive Summary

This feature delivers the missing `techplan` planning conductor for the new codebase rewrite of `lens-work`. The command must restore a governed path from completed business planning into architecture authoring without reintroducing the instability that the rewrite is intended to remove. The user-facing value is not a new planning concept. It is dependable continuity: `techplan` must preserve the existing Lens contract that reviewed predecessor artifacts publish first, architecture generation remains delegated, and the authoritative PRD stays explicitly referenced throughout technical planning.

This planning set is intentionally clean-room. The old-codebase prompt supplied for input is only a stub, so it cannot act as the implementation source. The functional contract instead comes from the approved baseline rewrite artifacts for stories 4.2 through 4.5, the baseline PRD, the baseline architecture, and the rewrite research record.

## Problem Statement

The target project at `TargetProjects/lens-dev/new-codebase/lens.core.src` does not yet contain a `techplan` prompt chain or owning skill. Without that surface, the new codebase cannot advance a full-track feature from business planning into governed technical design. That creates three business problems:

1. Full-track planning cannot be completed in the new codebase.
2. Publish-before-author governance cannot be exercised for the `techplan` handoff.
3. The rewrite cannot claim parity for a core retained command in the 17-command stable surface.

## Users and Stakeholders

- Primary user: Lens feature owners progressing a full-track feature from `businessplan` to `techplan`.
- Secondary user: Lens maintainers validating that the rewrite preserves retained-command behavior.
- Governance stakeholder: reviewers who rely on published predecessor artifacts and explicit PRD traceability.
- Delivery stakeholder: developers implementing the new codebase command surface in `TargetProjects/lens-dev/new-codebase/lens.core.src`.

## Desired Outcomes

### User Outcomes

- A Lens user can invoke `techplan` in the new codebase and get the same governed phase behavior expected from the retained command surface.
- The user does not need to manually publish predecessor artifacts or manually explain which PRD the architecture is answering.
- The command remains predictable: it behaves as a conductor, not as an ad hoc architecture generator.

### Product Outcomes

- The new codebase regains parity for one of the required planning commands in the retained 17-command surface.
- The rewrite keeps governance trust intact by preserving publish-before-author ordering and explicit PRD traceability.
- The delivery team can prove parity through focused tests instead of relying on prompt prose or memory.

## Scope

### In Scope

- Define the business intent and success criteria for the `techplan` command rewrite.
- Specify the required outcome parity with the baseline rewrite artifacts.
- Plan the missing prompt chain and owning skill in `TargetProjects/lens-dev/new-codebase/lens.core.src`.
- Preserve the relationship between `businessplan`, `techplan`, adversarial review, and finalizeplan.

### Out of Scope

- Replacing `bmad-create-architecture` with a new authoring engine.
- Changing feature identity, branch topology, governance paths, or lifecycle schema.
- Reclassifying this feature from `track: full` to `track: express`.
- Bypassing governance publication with direct writes to the governance repo.

## Functional Requirements

1. `techplan` must remain a full-track planning conductor, not an express-only shortcut.
2. Entering `techplan` must publish reviewed `businessplan` artifacts before architecture authoring begins.
3. The architecture output must explicitly reference the authoritative PRD.
4. The command must keep staged artifacts local under the feature docs path until later lifecycle handoff.
5. Regression coverage must protect wrapper equivalence, publish-before-author ordering, and PRD reference enforcement.

## Success Measures

- The new codebase exposes a working `techplan` prompt chain in the target project.
- Focused regressions covering publish-before-author behavior and PRD reference enforcement pass.
- No direct governance writes occur from `techplan` outside the shared publish hook.
- The resulting implementation aligns with the baseline story 4.3 acceptance criteria.

## Risks and Mitigations

| Risk | Impact | Mitigation |
| --- | --- | --- |
| `techplan` is implemented as a direct authoring surface | Governance and wrapper parity drift | Keep the conductor/delegation model explicit in the technical plan and tests |
| The PRD reference rule becomes optional | Architecture loses traceability to business intent | Add a dedicated validation and regression check for required PRD reference content |
| Publish-before-author behavior is skipped or reordered | Reviewed predecessor artifacts no longer form the governing input | Route phase entry through the shared publish hook and test order explicitly |
| Clean-room discipline erodes into file copying | Rewrite loses defensibility | Treat baseline docs as normative inputs and use old-codebase prompt material only as a reference anchor |

## Source Packet Used For This Plan

- `TargetProjects/lens-dev/old-codebase/lens.core.src/.github/prompts/lens-techplan.prompt.md`
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/prd.md`
- `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-2-rewrite-businessplan.md`
- `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-3-rewrite-techplan.md`
- `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-4-rewrite-finalizeplan.md`
- `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-5-rewrite-expressplan.md`
- `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/architecture.md`
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/research.md`

## Delivery Decision

This feature should proceed on the full planning path. The user requested `lens-expressplan`, but the active feature metadata declares `track: full`, and the retained-command contract explicitly blocks non-express features from entering expressplan. The actionable outcome for this session is therefore to produce the business and technical planning artifacts that unblock the full-track rewrite work without fabricating an express-track transition.