---
feature: nextlens-src-dogfoodnext
doc_type: tech-plan
track: express
updated_at: 2026-05-15
inputDocuments:
  - docs/nextlens/src/nextlens-src-topdownlens/guides/bugfix-flow.md
  - docs/nextlens/src/nextlens-src-topdownlens/examples/bugfix-example.md
---

# Tech Plan

## Planning Boundary

This document defines the implementation contract for a future Lens-owned NextLens bugfix skill. It does not implement code, modify registration, or write to the target repo.

There are two distinct boundaries:

- Skill source boundary: the new orchestration skill is authored in `lens.core.src`.
- Runtime fix boundary: when the skill performs or delegates a NextLens fix, implementation edits are allowed only under `TargetProjects/nextlens/src/NextLens`.

Governance repositories, governance docs mirrors, release clone surfaces, and unrelated control-root files remain out of scope for runtime implementation writes.

## Expected Capability Additions

- Add a dedicated Lens skill such as `lens-nextlens-bugfix` or equivalent under the `lens.core.src` skill surface.
- Expose an operator-facing prompt or command alias according to Lens skill and prompt conventions.
- Update Lens skill registry, help, and release-sync metadata so the capability can be discovered from the Lens module.
- Add source-owned helpers in `lens.core.src` for chat-history intake, design-context loading, fix-spec generation, and runtime boundary validation.
- Extend Lens validation checks so missing registration, missing helpers, invalid intake, inaccessible docs context, or target-boundary misconfiguration are reported clearly.

## Intake Contract

The bugfix skill should require these inputs:

- `what_happened`: concise description of the observed behavior.
- `what_should_have_happened`: concise description of the expected behavior.
- `chat_history`: evidence text, transcript excerpt, or path to an approved evidence artifact.

Optional fields may include severity, originating Salmon signal ID, evidence references, suspected surface, requested validation, and operator notes. The parser should normalize these inputs into a structured intake object before any implementation handoff.

## Design Context Loader

The context loader should read from `docs/nextlens/src` and select relevant NextLens design guidance, with the TopDownLens bugfix flow as the initial pattern reference. It should treat governance and release surfaces as read-only unless an approved Lens operation handles publication or promotion outside this skill.

The loader should return a compact context bundle with source paths, extracted constraints, known workflow patterns, and any conflicts or missing context that require operator resolution.

## Fix Specification Contract

The fix-spec generator should produce an implementation-ready artifact or in-memory handoff containing:

- Feature ID and bugfix title.
- Actual behavior, expected behavior, and summarized evidence.
- Relevant design-context references.
- Suspected target files or capability surfaces when known.
- Explicit skill source root: `lens.core.src`.
- Explicit runtime allowed write root: `TargetProjects/nextlens/src/NextLens`.
- Explicit prohibited write roots: governance repos, governance docs mirrors, release clones, and unrelated control repo paths.
- Implementation approach, validation commands, and evidence expectations.
- Salmon linkage and closure notes when an originating signal exists.

## Salmon Integration

If a Salmon signal is present, the skill should preserve signal ID, severity, recommended action, and evidence references in the fix specification. High or blocking signals should follow the bugfix pattern: capture evidence, prepare a dedicated correction path, verify the target repo change, record PR evidence, and close or supersede the originating signal only through the approved route.

If no Salmon signal is present, the skill should still produce evidence fields that can later be attached to a signal or review artifact.

## Delegation And Write Boundary

The bugfix skill should prepare implementation delegation, not perform broad discovery or unrestricted edits. Before delegation it must confirm:

- Target repo path resolves to `TargetProjects/nextlens/src/NextLens`.
- The proposed files are inside the allowed target root.
- The fix spec includes validation and evidence requirements.
- Dev work has an implementation story when lifecycle gates require one.
- The Lens skill source root is not treated as the NextLens fix target except while implementing this feature's own skill code.

Any attempt to write outside the target root should stop the workflow with a boundary violation.

## Validation Strategy

- Unit tests for the intake parser with complete, missing, and noisy chat-history inputs.
- Unit tests for fix-spec generation and required field enforcement.
- Boundary tests proving prohibited paths are rejected.
- Integration-style fixture covering chat history plus docs context producing a deterministic fix spec.
- Lens validation that checks skill registration, prompt/help metadata, helper availability, docs context access, target repo resolution, and schema health.

## Technical Risks

- Input transcripts can be large; scripts should summarize or reference durable evidence without storing excessive raw chat.
- Context loading can become too broad; selection should prefer explicit feature docs and known NextLens guidance.
- Boundary enforcement must be tested at path-normalization edges, especially on Windows paths.
- Lens prompt, skill, and help metadata must stay synchronized with the skill entrypoint.