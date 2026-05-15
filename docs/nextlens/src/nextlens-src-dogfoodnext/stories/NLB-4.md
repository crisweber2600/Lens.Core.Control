---
feature: nextlens-src-dogfoodnext
story_id: NLB-4
doc_type: story
status: ready-for-dev
title: Design Context And Target Resolver
depends_on: [NLB-1]
implementation_kind: resolver
epic: 2
updated_at: 2026-05-15T20:00:00Z
---

# NLB-4 - Design Context And Target Resolver

## Goal

Resolve design context, skill source, and runtime target roots portably across invocation contexts.

## Scope

- Resolve the control repo root and `docs/nextlens/src` from Lens configuration or feature metadata.
- Resolve the skill source root as `lens.core.src` and the runtime target root as `TargetProjects/nextlens/src/NextLens`.
- Accept operator overrides only after normalization proves they stay inside approved roots.
- Report missing or conflicting docs context without guessing.

## Acceptance Criteria

- Given the command runs from the workspace root, when context is resolved, then `docs/nextlens/src` and `TargetProjects/nextlens/src/NextLens` are found.
- Given the command runs from a non-root current working directory, when context is resolved, then the same approved roots are found.
- Given an override escapes the approved root through relative segments, casing tricks, or symlink or junction resolution where supported, when validation runs, then the workflow stops before mutation.
- Given relevant docs exist, when context loads, then it reports source paths and extracted constraints including the TopDownLens bugfix guide and example.

## Validation

- Add fixture tests for root invocation, non-root invocation, escaped override rejection, path casing, relative segments, Windows and POSIX separators, and symlink or junction escapes where supported.

## Dev Notes

- Use path normalization APIs rather than string concatenation.
- Governance and release surfaces remain read-only unless an approved Lens operation handles publication.