---
feature: nextlens-src-dogfoodnext
story_id: NLB-3
doc_type: story
status: ready-for-dev
title: Namespaced Bug Artifact Operations
depends_on: [NLB-2]
implementation_kind: bug-state
epic: 2
updated_at: 2026-05-15T20:00:00Z
---

# NLB-3 - Namespaced Bug Artifact Operations

## Goal

Create, find, update, and close NextLens bug artifacts under `bugs/nextlens/...` while preserving existing Lens core bug behavior.

## Scope

- Extend `bug-reporter-ops.py` or add an approved wrapper so NextLens defects use `bugs/nextlens/{New|QuickDev|Inprogress|Fixed}/{slug}.md`.
- Preserve the content-hash slug pattern and duplicate detection across all NextLens status folders.
- Preserve frontmatter fields used by existing bugbash tooling, plus namespace metadata when needed.
- Ensure `record-quickdev-pr` and `close-quickdev-bug` resolve namespaced artifacts without mutating `bugs/QuickDev` Lens core artifacts.

## Acceptance Criteria

- Given a new NextLens bug intake, when create-bug runs with namespace `nextlens`, then it writes `bugs/nextlens/QuickDev/{slug}.md`.
- Given the same title and description are submitted twice, when create-bug runs, then it returns the existing NextLens artifact instead of creating a duplicate.
- Given a PR URL is recorded, when the artifact is namespaced, then only the matching `bugs/nextlens/...` artifact is updated.
- Given closeout succeeds, when validation and PR evidence exist, then the artifact moves to `bugs/nextlens/Fixed/{slug}.md`.
- Given existing Lens core bug fixtures are exercised, when namespace support is enabled, then `bugs/QuickDev` behavior remains unchanged.

## Validation

- Add tests for create, duplicate lookup, invalid namespace or path scope, PR recording, closeout, and regression coverage for existing Lens core bug artifacts.

## Dev Notes

- This story is a state-machine change. Start with failing tests around lookup and closeout paths.