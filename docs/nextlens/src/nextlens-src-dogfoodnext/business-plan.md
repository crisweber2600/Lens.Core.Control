---
feature: nextlens-src-dogfoodnext
doc_type: business-plan
track: express
updated_at: 2026-05-15
inputDocuments:
  - docs/nextlens/src/nextlens-src-topdownlens/guides/bugfix-flow.md
  - docs/nextlens/src/nextlens-src-topdownlens/examples/bugfix-example.md
  - lens.core/_bmad/lens-work/skills/bmad-lens-core-bugfix/SKILL.md
  - lens.core/_bmad/lens-work/scripts/bug-reporter-ops.py
---

# Business Plan

## Problem

NextLens development needs a purpose-built bugfix skill that behaves like the Lens-core-bugfix flow but is scoped to the NextLens project. Today, defects discovered through chat history must be translated manually into a bounded correction workflow, which increases the risk of missed evidence, ambiguous expectations, and writes outside the approved target surface.

The product intent is a NextLens-specific bugfix skill that accepts three operator inputs: what happened, what should have happened, and the supporting chat history. It should turn those inputs into a governed, evidence-backed correction path for NextLens.

Important boundary clarification: the new skill is authored in `lens.core.src` as a Lens-owned orchestration skill. When that skill runs, its implementation fixes are constrained to `TargetProjects/nextlens/src/NextLens`.

## Operator And User

- Primary operator: a Lens or NextLens conductor initiating a targeted fix from observed chat behavior.
- Primary user: the implementation agent that needs a precise, bounded fix specification.
- Secondary users: reviewers validating evidence, write boundaries, and closure against the originating Salmon signal when one exists.

## Value

- Reduces rediscovery by converting chat history into a compact fix specification.
- Reuses the proven Lens-core-bugfix pattern for branch discipline, evidence, verification, and closure.
- Keeps the bugfix skill source in `lens.core.src` while keeping runtime NextLens corrections inside `TargetProjects/nextlens/src/NextLens`.
- Improves review quality by preserving the gap between actual behavior and expected behavior as explicit evidence.

## Scope

- Add a Lens-owned NextLens bugfix skill in `lens.core.src`, exposed through the Lens skill and prompt conventions.
- Intake the three required inputs: what happened, what should have happened, and chat history evidence.
- Create the bug report under a NextLens namespace, e.g. `bugs/nextlens/QuickDev/{slug}.md`, rather than mixing NextLens defects into the generic Lens core bug queue.
- Read design context from `docs/nextlens/src` so fixes can be framed against current NextLens guidance.
- Generate a bounded fix specification that identifies affected surfaces, expected correction, evidence references, validation requirements, and write boundaries.
- Integrate with existing Salmon routing where a signal ID, severity, or recommended action is present.
- Prepare implementation handoff for changes only under `TargetProjects/nextlens/src/NextLens`.

## Non-Goals

- Do not implement the skill or target repo code in this ExpressPlan package.
- Do not create a generic Lens-wide bugfix skill.
- Do not author the new skill inside `TargetProjects/nextlens/src/NextLens`; that repository is the fix target, not the skill source.
- Do not write directly to governance feature folders, governance docs mirrors, release clones, or unrelated control-root files.
- Do not bypass FinalizePlan story generation, dev story gates, peer review, or target repo verification.
- Do not automatically close Salmon signals without evidence and the approved closure path.

## Dependencies

- Lens source skill, prompt, registry, and release-sync surfaces in `lens.core.src`.
- Existing `/lens-core-bugfix` conductor behavior, especially its bug artifact creation, fresh branch rule, PR recording, and closeout gate.
- Existing bug reporter operations for slug generation, duplicate detection, frontmatter schema, QuickDev provenance, PR recording, and closeout.
- Target repo clone at `TargetProjects/nextlens/src/NextLens`, including the NextLens module assets and tests that future bugfixes may modify.
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
- Bug report artifacts are written through an approved operation into a `nextlens` folder under the bug store, with status transitions and closeout evidence kept separate from Lens core bug reports.
- The future skill is implemented in `lens.core.src`, reads NextLens design context from `docs/nextlens/src`, and writes runtime implementation fixes only to `TargetProjects/nextlens/src/NextLens`.
- The future workflow can generate a fix specification suitable for implementation delegation and review.
- Registration, validation, branch preparation, PR recording, bug closeout, evidence recording, and Salmon linkage are covered by implementation slices and acceptance criteria.