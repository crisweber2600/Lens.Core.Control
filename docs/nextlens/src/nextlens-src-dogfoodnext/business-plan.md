---
feature: nextlens-src-dogfoodnext
doc_type: business-plan
track: express
updated_at: 2026-05-15
inputDocuments:
  - docs/nextlens/src/nextlens-src-topdownlens/guides/bugfix-flow.md
  - docs/nextlens/src/nextlens-src-topdownlens/examples/bugfix-example.md
---

# Business Plan

## Problem

NextLens development needs a purpose-built bugfix skill that behaves like the Lens-core-bugfix flow but is scoped to the NextLens project. Today, defects discovered through chat history must be translated manually into a bounded correction workflow, which increases the risk of missed evidence, ambiguous expectations, and writes outside the approved target surface.

The product intent is a NextLens-specific bugfix skill that accepts three operator inputs: what happened, what should have happened, and the supporting chat history. It should turn those inputs into a governed, evidence-backed correction path for NextLens.

## Operator And User

- Primary operator: a Lens or NextLens conductor initiating a targeted fix from observed chat behavior.
- Primary user: the implementation agent that needs a precise, bounded fix specification.
- Secondary users: reviewers validating evidence, write boundaries, and closure against the originating Salmon signal when one exists.

## Value

- Reduces rediscovery by converting chat history into a compact fix specification.
- Reuses the proven Lens-core-bugfix pattern for branch discipline, evidence, verification, and closure.
- Keeps NextLens corrections inside `TargetProjects/nextlens/src/NextLens` during implementation.
- Improves review quality by preserving the gap between actual behavior and expected behavior as explicit evidence.

## Scope

- Add a NextLens bugfix skill capability, exposed as `bmad-nextlens-bugfix` and/or `nextlens-bugfix` according to the existing module conventions.
- Intake the three required inputs: what happened, what should have happened, and chat history evidence.
- Read design context from `docs/nextlens/src` so fixes can be framed against current NextLens guidance.
- Generate a bounded fix specification that identifies affected surfaces, expected correction, evidence references, validation requirements, and write boundaries.
- Integrate with existing Salmon routing where a signal ID, severity, or recommended action is present.
- Prepare implementation handoff for changes only under `TargetProjects/nextlens/src/NextLens`.

## Non-Goals

- Do not implement the skill or target repo code in this ExpressPlan package.
- Do not create a generic Lens-wide bugfix skill.
- Do not write directly to governance feature folders, governance docs mirrors, release clones, or unrelated control-root files.
- Do not bypass FinalizePlan story generation, dev story gates, peer review, or target repo verification.
- Do not automatically close Salmon signals without evidence and the approved closure path.

## Dependencies

- Existing NextLens module capability registration in `TargetProjects/nextlens/src/NextLens/skills/module.yaml`.
- Existing NextLens skill surface under `TargetProjects/nextlens/src/NextLens/.agents/skills/`.
- Shared NextLens skill scripts under `.agents/skills/bmad-nextlens/scripts/`.
- Current design context under `docs/nextlens/src`.
- TopDownLens bugfix guidance for evidence capture, branch discipline, target-only implementation, verification, PR recording, and Salmon closure.
- ExpressPlan constitution result: planning requires business and technical plans; dev requires stories; review is enforced.

## Risks

- Chat history may be noisy, incomplete, or contain sensitive material that should be summarized rather than copied into durable artifacts.
- Design context under `docs/nextlens/src` may be broad; the loader must select relevant context without overfitting to stale guidance.
- A bugfix skill can accidentally become a general-purpose implementation shortcut unless write boundaries and validation gates are explicit.
- Salmon integration could duplicate or conflict with existing signal handling if the contract is not narrow.
- Module registration, help text, and doctor validation can drift unless they are tested together.

## Success Criteria

- FinalizePlan can generate implementation stories without rediscovering the problem or expected capability boundaries.
- The future skill has a clear intake contract for actual behavior, expected behavior, and chat history evidence.
- The future workflow reads NextLens design context from `docs/nextlens/src` and writes implementation changes only to `TargetProjects/nextlens/src/NextLens`.
- The future workflow can generate a fix specification suitable for implementation delegation and review.
- Registration, validation, evidence recording, and Salmon linkage are covered by implementation slices and acceptance criteria.