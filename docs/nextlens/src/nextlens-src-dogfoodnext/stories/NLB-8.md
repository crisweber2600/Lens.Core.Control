---
feature: nextlens-src-dogfoodnext
story_id: NLB-8
doc_type: story
status: ready-for-dev
title: End-To-End Tests And Documentation
depends_on: [NLB-1, NLB-2, NLB-3, NLB-4, NLB-5, NLB-6, NLB-7]
implementation_kind: validation-docs
epic: 3
updated_at: 2026-05-15T20:00:00Z
---

# NLB-8 - End-To-End Tests And Documentation

## Goal

Prove the full NextLens bugfix flow and document concise operator usage.

## Scope

- Add an end-to-end fixture that starts with observed chat behavior and expected behavior, creates a namespaced bug artifact, derives a branch identity, generates a fix spec, records PR evidence, and closes the artifact after validation.
- Add regression coverage proving existing `/lens-core-bugfix` and `bugs/QuickDev` behavior are unchanged.
- Add usage documentation for the three required inputs, optional Salmon metadata, expected outputs, and closeout evidence.
- Add validation or doctor hooks for skill registration, prompt/help metadata, helper availability, docs context access, target repo resolution, namespace configuration, and boundary enforcement.

## Acceptance Criteria

- Given the end-to-end fixture runs, when all inputs are valid, then the workflow produces deterministic artifact paths, branch identity, fix-spec fields, validation evidence, PR URL, and closeout state.
- Given Lens core bugfix regression fixtures run, when namespace support is present, then existing Lens core behavior is unchanged.
- Given docs are reviewed, when an operator follows them, then required inputs and expected outputs are clear without bypassing lifecycle conductor responsibilities.
- Given validation hooks run, when any registration or boundary surface drifts, then the check fails with an actionable message.

## Validation

- Run focused unit and integration tests added by NLB-1 through NLB-7.
- Run the narrow registration/help validation and any NextLens target-repo validation identified in the fix specification contract.

## Dev Notes

- Keep documentation concise and operational. Do not turn this into a generic Lens-wide bugfix manual.