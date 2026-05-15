---
feature: nextlens-src-dogfoodnext
doc_type: sprint-plan
track: express
updated_at: 2026-05-15
inputDocuments:
  - docs/nextlens/src/nextlens-src-topdownlens/guides/bugfix-flow.md
  - docs/nextlens/src/nextlens-src-topdownlens/examples/bugfix-example.md
  - lens.core/_bmad/lens-work/skills/bmad-lens-core-bugfix/SKILL.md
  - lens.core/_bmad/lens-work/scripts/bug-reporter-ops.py
---

# Sprint Plan

## Sprint Goal

Prepare implementation-ready stories for a Lens-owned NextLens bugfix skill in `lens.core.src` that converts observed chat failures into namespaced bug reports, fresh bugfix branches, and bounded corrections under `TargetProjects/nextlens/src/NextLens`.

## Slice 1 - Skill Registration And Entrypoint

Create the Lens-owned `nextlens` bugfix skill surface in `lens.core.src` and expose the operator command or prompt according to Lens module conventions.

Acceptance criteria:

- The skill is discoverable from Lens skill and prompt metadata.
- Module help describes required inputs: what happened, what should have happened, and chat history.
- The entrypoint states both boundaries: skill source in `lens.core.src`, runtime fixes in `TargetProjects/nextlens/src/NextLens`.

Validation notes: run the Lens module discovery/help checks and confirm the new capability appears once with consistent naming.

## Slice 2 - Intake Schema And Parser

Define and implement the structured intake schema for chat-history-driven bugfix requests.

Acceptance criteria:

- Required fields are `what_happened`, `what_should_have_happened`, and `chat_history`.
- Optional fields support severity, Salmon signal ID, evidence references, suspected surface, validation request, and operator notes.
- The normalized intake maps to title, description, repro or observed evidence summary, expected behavior, actual behavior, and chat evidence reference.
- Missing required fields produce actionable errors.
- Noisy chat history is accepted and summarized or referenced without losing the behavior gap.

Validation notes: add parser tests for complete input, missing required fields, large transcript input, optional Salmon metadata, and expected/actual mapping.

## Slice 3 - Namespaced Bug Report Creation

Create the operational bug report under a NextLens-specific bug folder while preserving the Lens core bugfix state machine.

Acceptance criteria:

- New bug reports are written under `bugs/nextlens/QuickDev/{slug}.md` or the configured status folder below `bugs/nextlens/`.
- Status folders mirror the existing model: `New`, `QuickDev`, `Inprogress`, and `Fixed`.
- Slugs use the existing content-hash pattern so duplicate reports are idempotent.
- Frontmatter preserves title, description, status, featureId, slug, timestamps, source, and any NextLens namespace metadata.
- PR recording and closeout locate the namespaced artifact and do not mutate Lens core bug artifacts.

Validation notes: add tests for create, duplicate, record PR, closeout, and invalid namespace/path scope.

## Slice 4 - NextLens Design Context Loader

Load relevant read-only planning and design context from `docs/nextlens/src` for use in bugfix specification.

Acceptance criteria:

- The loader can include the TopDownLens bugfix flow and related examples as pattern references.
- Returned context includes source paths and extracted constraints.
- Missing or conflicting context is reported without guessing.
- Governance and release surfaces are not treated as implementation write locations.

Validation notes: add fixture tests that load a small docs set and produce stable context references.

## Slice 5 - Bugfix Specification Generation

Generate a compact fix specification from intake plus design context.

Acceptance criteria:

- The spec preserves actual behavior, expected behavior, and evidence summary.
- The spec includes the namespaced bug artifact path and derived bug slug.
- The spec lists allowed and prohibited write roots.
- The spec identifies suspected target surfaces when provided or inferable from context.
- The spec includes validation commands or validation expectations.
- Salmon linkage is included when signal metadata exists.

Validation notes: snapshot or structured-output tests should verify required fields and deterministic ordering.

## Slice 6 - Implementation Delegation And Boundary Enforcement

Prepare the handoff from bugfix specification to the implementation agent while enforcing the NextLens target boundary.

Acceptance criteria:

- A fresh target branch is derived from the namespaced bug slug, for example `feature/nextlens-bugfix-{bug_slug}`.
- Delegation is blocked unless the allowed target root resolves to `TargetProjects/nextlens/src/NextLens`.
- Proposed edits outside the allowed target root are rejected before mutation.
- The handoff includes validation and evidence requirements.
- Lifecycle requirements for story-backed dev work are surfaced before implementation begins.
- Tests distinguish the `lens.core.src` skill source root from the runtime target repo root.
- No-op completion, branch reuse for a different bug, or missing implementation commit blocks success.

Validation notes: add Windows and POSIX path-normalization tests for allowed and prohibited write roots.

## Slice 7 - Validation, Evidence, PR Recording, And Salmon Routing

Add validation and evidence handling for completed bugfix work.

Acceptance criteria:

- The workflow records validation output references expected by reviewers.
- The conductor pushes the target branch and creates or reuses a PR before reporting success.
- The PR URL is recorded on the namespaced bug artifact before closeout.
- The bug artifact moves to `bugs/nextlens/Fixed/` only after validation and PR evidence exist.
- If a Salmon signal exists, the workflow preserves signal ID, severity, recommended action, and closure expectations.
- High and blocking signals follow the immediate bugfix pattern from the TopDownLens guidance.
- The workflow does not directly mutate governance or release artifacts for closure.

Validation notes: add tests or fixtures for no-signal, high-signal, and blocking-signal cases.

## Slice 8 - Tests, Validation Hooks, And Documentation

Round out operational checks and concise usage guidance.

Acceptance criteria:

- Lens validation checks verify skill registration, prompt/help metadata, helper availability, schema validity, docs path access, and boundary configuration.
- Tests cover parser, namespaced bug artifact operations, context loader, fix-spec generation, branch identity, boundary enforcement, PR recording, closeout, and Salmon metadata preservation.
- Usage docs show the three required inputs and the expected bounded output without duplicating lifecycle conductor responsibilities.

Validation notes: run targeted Lens skill tests plus any NextLens target-repo validation required by the generated fix specification.

## FinalizePlan Notes

FinalizePlan should convert these slices into story files before dev work begins. The first implementation story should establish registration, intake, and namespaced bug artifact contracts before any delegation behavior is added, because later slices depend on stable capability discovery, slug identity, and schema validation.