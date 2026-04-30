---
feature: lens-dev-new-codebase-techplan
doc_type: business-plan
status: draft
goal: "Define the business outcomes and delivery guardrails for rewriting the techplan command in the new codebase through a clean expressplan-compatible planning set."
key_decisions:
  - Use expressplan as the planning path for this feature; the express route is a packaging decision, not a change to the techplan runtime contract.
  - Treat the old-codebase `lens-techplan` public stub as a chain-shape reference only; the baseline rewrite corpus defines all functional requirements.
  - Stage four expressplan-contract artifacts in this folder — business-plan.md, tech-plan.md, sprint-plan.md, and expressplan-adversarial-review.md — as the canonical planning set.
  - Record governance state changes only through the sanctioned feature-yaml flow; no direct governance writes from staged planning docs.
  - Discovery wiring, focused test-harness ownership, and output parity definition are first-slice acceptance criteria, not deferred follow-up items.
open_questions:
  - Which target-project discovery surface should register `lens-techplan` first: the installer wiring file, the module-help CSV, or another retained manifest surface?
  - Which focused test path should own prompt-start and wrapper-equivalence coverage once the first implementation slice lands?
depends_on:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/prd.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-3-rewrite-techplan.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-5-rewrite-expressplan.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/architecture.md
blocks:
  - End-to-end techplan execution is blocked until this feature delivers the publish orchestration, BMAD wrapper routing, adversarial review gate, and constitution loading surfaces it now owns.
updated_at: 2026-04-30T00:00:00Z
---

# Business Plan — Techplan Command Rewrite

## Executive Summary

The `lens-dev-new-codebase-techplan` feature exists to restore the `techplan` conductor in the new codebase. The rewritten command must preserve the full baseline contract: publish reviewed businessplan artifacts before architecture authoring begins, enforce a traceable PRD reference in the generated architecture, delegate architecture authoring through the Lens BMAD wrapper rather than writing it inline, and write no governance files directly.

This planning folder uses the expressplan packaging route. That decision affects how the plan is organized — four expressplan-contract artifacts, a compressed review gate, and a single adversarial review before advancing — but it does not relax any of the runtime obligations the rewritten `techplan` command must satisfy. The express path is the planning mechanism for this feature. It is not a change to the rewritten command's behavior.

## Problem Statement

The new-codebase rewrite requires a complete, internally consistent planning set for the `techplan` command. Without it:

1. The feature folder lacks a sprint-plan.md, so the expressplan artifact contract is incomplete and cannot serve as a parity target for the eventual `bmad-lens-expressplan` rewrite.
2. Planning artifacts that argue against the express route while the governance record already carries `track: express` create a contradiction that makes implementation guidance unreliable.
3. Missing or contradictory baseline planning artifacts leave future implementation agents without a clear derivation path — they cannot distinguish behavioral requirements from incidental planning choices.

This folder resolves all three gaps: it delivers a complete expressplan packet, removes internal contradictions, and provides an unambiguous clean-room source for the implementation agent.

## Users and Stakeholders

- **Lens feature owners** who depend on a coherent planning packet so the techplan rewrite can begin without ambiguity.
- **Lens maintainers** proving the 17-command stable surface is fully restored in the new codebase.
- **Governance reviewers** who validate that staged artifacts align with the feature's declared lifecycle route.
- **Future implementation agents** who need a contradiction-free source to derive implementation behavior.

## Goals

1. Produce a complete expressplan artifact set for this feature: `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, and `expressplan-adversarial-review.md`.
2. Preserve the full baseline `techplan` runtime contract: publish-before-author ordering, PRD reference enforcement, conductor-only delegation, and no direct governance writes.
3. Maintain an explicit clean-room boundary: functional behavior comes from the baseline rewrite corpus; the old-codebase public stub confirms the command chain shape only.
4. Define output parity explicitly so the eventual `bmad-lens-expressplan` skill rewrite has a concrete, reproducible target.

## Non-Goals

- Changing the rewritten `techplan` command into an express-only runtime workflow.
- Writing governance metadata directly from these staged planning docs.
- Re-scoping the feature to absorb the `expressplan` command rewrite itself.
- Replacing the BMAD architecture authoring workflow with a conductor-authored inline document.

## Required Outcomes

| Outcome | Acceptance signal |
| --- | --- |
| Complete expressplan artifact set | All four required artifacts present, internally consistent |
| Preserved techplan runtime contract | Tech plan names each behavioral constraint with a verifiable condition |
| Clean-room boundary enforced | No old-codebase prose re-used; derivation traced to baseline corpus |
| Output parity defined | Tech plan specifies the same four artifacts and equivalent routing/gates |

## Clean-Room Source Packet

The following sources are the authoritative inputs for this planning set. All behavioral claims must trace to one of them. The old-codebase `lens-techplan` public stub contributes one fact: the command chain shape (public stub → preflight → release prompt → owning skill). It provides no behavioral content.

| Source | Contribution |
| --- | --- |
| `lens-dev-new-codebase-baseline/prd.md` | FR11 (techplan artifacts), FR29 (approved publication path), FR30 (no direct governance writes) |
| `lens-dev-new-codebase-baseline/architecture.md` | 17-command surface, 3-hop command chain, shared utility contracts |
| `lens-dev-new-codebase-baseline/docs/4-3-rewrite-techplan.md` | Three AC items: publish entry hook, PRD reference, regression coverage |
| `lens-dev-new-codebase-baseline/docs/4-5-rewrite-expressplan.md` | Express gating, QuickPlan delegation, adversarial review hard-stop |
| Old-codebase `lens-techplan` public stub | Public chain shape only: stub → preflight → `lens.core/_bmad/lens-work/prompts/lens-techplan.prompt.md` |
| Old-codebase `lens-constitution` public stub | Chain shape for constitution dependency: stub → preflight → constitution release prompt → constitution skill |

## Success Criteria

- This feature folder contains all four expressplan-contract artifacts with no internal contradictions.
- The tech plan names the five target-project implementation files and the seven runtime behavioral obligations.
- The sprint plan decomposes delivery into reviewable slices that an implementation agent can execute without additional clarification.
- Output parity is defined as: the future `bmad-lens-expressplan` skill can reproduce these four artifacts with equivalent routing decisions, adversarial review gating, and delivery slices.
