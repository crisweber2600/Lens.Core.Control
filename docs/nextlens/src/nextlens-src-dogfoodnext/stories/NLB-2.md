---
feature: nextlens-src-dogfoodnext
story_id: NLB-2
doc_type: story
status: ready-for-dev
title: Intake Schema And Transcript Minimization
depends_on: [NLB-1]
implementation_kind: parser
epic: 1
updated_at: 2026-05-15T20:00:00Z
---

# NLB-2 - Intake Schema And Transcript Minimization

## Goal

Normalize operator-provided chat failure evidence into the structured fields required by the bug reporter without persisting raw transcript material by default.

## Scope

- Require `what_happened`, `what_should_have_happened`, and `chat_history`.
- Support optional severity, Salmon signal ID, evidence references, suspected target surface, validation request, and operator notes.
- Map normalized intake to title, description, repro or observed evidence summary, expected behavior, actual behavior, chat evidence reference, source `nextlens-bugfix`, queue `QuickDev`, and namespace `nextlens`.

## Acceptance Criteria

- Given all required inputs are present, when intake is parsed, then normalized output contains actual behavior, expected behavior, evidence summary, and bug reporter fields.
- Given required input is missing, when intake is parsed, then the skill stops before artifact creation with an actionable error.
- Given noisy chat history is supplied, when durable output is created, then the artifact stores a concise summary and evidence reference instead of the full transcript by default.
- Given obvious secret-like material appears in chat input, when feasible, then the parser rejects or redacts it before durable artifact writing.

## Validation

- Add parser tests for complete input, missing required fields, large transcript input, optional Salmon metadata, expected/actual mapping, transcript minimization, and secret-like input handling.

## Dev Notes

- Prefer structured parsing and explicit field validation over ad hoc prompt text scanning.
- Raw transcript persistence requires an approved evidence artifact reference and should be exceptional.