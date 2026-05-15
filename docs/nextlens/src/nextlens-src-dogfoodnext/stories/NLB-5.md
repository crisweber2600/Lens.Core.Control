---
feature: nextlens-src-dogfoodnext
story_id: NLB-5
doc_type: story
status: ready-for-dev
title: Fix Specification Generation
depends_on: [NLB-2, NLB-3, NLB-4]
implementation_kind: specification
epic: 3
updated_at: 2026-05-15T20:00:00Z
---

# NLB-5 - Fix Specification Generation

## Goal

Generate a deterministic implementation handoff from normalized intake, namespaced bug state, and NextLens design context.

## Scope

- Include feature ID, bug slug, namespaced bug artifact path, actual behavior, expected behavior, evidence summary, and design-context references.
- Include allowed write root `TargetProjects/nextlens/src/NextLens` and prohibited roots for governance, release clones, and unrelated control paths.
- Include suspected target surfaces when provided or inferable, validation expectations, and Salmon linkage when present.

## Acceptance Criteria

- Given normalized intake and context, when a fix specification is generated, then required fields are present in deterministic order.
- Given a Salmon signal is present, when the spec is generated, then signal ID, severity, recommended action, and closure expectations are preserved.
- Given no Salmon signal is present, when the spec is generated, then evidence fields still support later signal or review attachment.
- Given the allowed target root is missing or ambiguous, when spec generation runs, then delegation is blocked.

## Validation

- Add structured-output or snapshot tests for required fields, deterministic ordering, Salmon and no-Salmon cases, and missing target-root behavior.

## Dev Notes

- The fix specification may be durable or in-memory, but the choice must be explicit and reviewable.