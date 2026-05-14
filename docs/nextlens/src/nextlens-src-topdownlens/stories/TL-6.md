---
feature: nextlens-src-topdownlens
story_id: TL-6
doc_type: story
status: not-started
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

- Signal schema (id, severity, target_kind, target_id, evidence_refs, recommended_action, lifecycle_state).
- Signal lifecycle states (open, acknowledged, in-progress, resolved, rejected).
- Severity levels (info, low, medium, high, blocking).
- Upstream targets (feature, journey, outcome, product area, landscape, BMAD correct-course).
- Recommended action vocabulary (local note, landscape update, split feature, blocking correction).

## Acceptance

- Signals can target feature, journey, outcome, product area, landscape, or BMAD correct-course.
- Signals distinguish local note, landscape update, split feature, and blocking correction.
- Signals preserve evidence and provenance (source story ID, source file path with line range).
- At least one example signal per severity level is committed.
- TL-10 bugfix routing rules reference this schema for high/blocking severities.

## Files To Produce

- `docs/nextlens/src/nextlens-src-topdownlens/schemas/salmon-signal.schema.json`.
- `docs/nextlens/src/nextlens-src-topdownlens/examples/salmon-signals/` (one example per severity).
- `docs/nextlens/src/nextlens-src-topdownlens/guides/salmon-signals.md` (lifecycle + routing).

## Notes For Dev

- Depends on TL-1 (entity IDs).
- TL-10 and TL-12 both consume this contract.
