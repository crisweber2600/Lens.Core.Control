---
feature: nextlens-src-topdownlens
story_id: TL-5
doc_type: story
status: not-started
deferrable: true
title: Minimal Derived Graph Rebuild
depends_on: [TL-1]
implementation_kind: cli
epic: 3
spine: false
updated_at: 2026-05-14T04:00:00Z
---

# TL-5 - Minimal Derived Graph Rebuild

## Goal

Provide a rebuildable graph projection over source files (entities and relationships defined in TL-1).

## Scope

- Relationship index.
- Traceability index.
- Derived map metadata (timestamp, source files, schema version).
- Rebuild freshness marker.

## Acceptance

- Source files remain authoritative; the derived graph never overrides them.
- Rebuild output is disposable (re-creatable from source files alone).
- Broken references are reported (machine-readable), not silently ignored.
- Rebuild command produces zero new file edits on a clean tree (idempotent).

## Files To Produce

- CLI entry point (location TBD with dev; default: `TargetProjects/lens/Lens.Core.Control/scripts/lens-topdown/rebuild-derived-graph.py` or equivalent).
- Output: `docs/nextlens/src/nextlens-src-topdownlens/derived/graph.json` plus `derived/freshness.json`.
- Tests covering: rebuild from clean tree, rebuild with broken references, rebuild idempotency.

## Notes For Dev

- **Deferrable** beyond first dev increment if time is tight; TL-12 closure does not require this.
- If shipped, TL-7 doctor checks should consume the freshness marker.
