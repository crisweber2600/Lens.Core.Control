---
feature: nextlens-src-topdownlens
story_id: TL-6
doc_type: story
status: in-progress
title: Salmon Signal Contract
depends_on: [TL-1]
implementation_kind: schema
epic: 1
spine: false
updated_at: 2026-05-14T04:00:00Z
---

# TL-6 - Salmon Signal Contract

## Goal

Define a recursive upstream consistency validation signal that flows from low-level evidence back up the hierarchy.

## Scope

- Signal schema aligned with `salmon-signal.yaml` module contract:
  - `id` (salmon.<timestamp>.<slug>)
  - `source` (type: story | feature | implementation | review | validation; id)
  - `signal_type` (assumption_invalidated | missing_context | scope_drift | impact_discovered | boundary_wrong | evidence_changed)
  - `severity` (low | medium | high | blocking)
  - `upstream_targets` (feature | journey | outcome | product_area | landscape)
  - `finding` (description of what was discovered)
  - `recommended_action` (local_note | landscape_update | bmad_correct_course | split_feature | block_promotion)
  - `status` (open | accepted | rejected | resolved | superseded)
- Severity routing rules: which severities trigger which recommended_action defaults.
- Upstream target scope: feature, journey, outcome, product area, landscape.
- Recommended action vocabulary: local_note, landscape_update, split_feature, bmad_correct_course, block_promotion.

## Acceptance

- Signals can target feature, journey, outcome, product area, or landscape.
- Signal schema matches the `salmon-signal.yaml` contract defined in the tech plan.
- Signals distinguish local_note, landscape_update, split_feature, bmad_correct_course, and block_promotion actions.
- Signals preserve evidence and provenance (source.type, source.id, upstream_targets, finding).
- Signal status lifecycle is: open → accepted | rejected → resolved | superseded.
- At least one example signal per severity level is committed.
- TL-10 bugfix routing rules reference this schema for high/blocking severities.

## Files To Produce

- `docs/nextlens/src/nextlens-src-topdownlens/schemas/salmon-signal.schema.json`.
- `docs/nextlens/src/nextlens-src-topdownlens/examples/salmon-signals/` (one example per severity).
- `docs/nextlens/src/nextlens-src-topdownlens/guides/salmon-signals.md` (lifecycle + routing).

## Notes For Dev

- Depends on TL-1 (entity IDs).
- TL-10 and TL-12 both consume this contract.

## Dev Agent Record
