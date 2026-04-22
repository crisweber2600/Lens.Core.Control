---
feature: lens-dev-new-codebase-baseline
story_id: "1.1"
story_key: "1-1-scaffold-published-surface"
epic: "1"
title: "New Codebase Scaffold, Install Surfaces, and Module Surface Reduction"
status: re-evaluate
priority: must
story_points: 8
depends_on: []
updated_at: 2026-04-22T00:00:00Z
---

# Story 1.1: New Codebase Scaffold, Install Surfaces, and Module Surface Reduction

Status: re-evaluate

## Story

As a lens-work maintainer,
I want the source tree scaffolded with exactly 17 published prompt stubs, aligned help and shell surfaces, and updated installer and adapter metadata,
so that every rewrite starts from a consistent, auditable foundation.

## Acceptance Criteria

1. The published prompt surface is reduced to exactly 17 retained commands.
2. setup.py, module-help.csv, lens.agent.md, and supported adapters agree on the same command inventory.
3. The retained internal skill inventory matches the approved keep/remove matrix.

## Tasks / Subtasks

- [ ] Reduce the published prompt surface to the retained 17-command set.
  - [ ] Remove deprecated public stubs without deleting retained internal dependencies.
- [ ] Align installer, module help, shell menu, and adapter inventories.
  - [ ] Compare setup.py, prompt manifests, module-help.csv, lens.agent.md, and adapter surfaces together.
- [ ] Audit the retained internal skill inventory against the architecture matrix.
  - [ ] Record any remaining drift before merge.

## Dev Notes

- Use the old-codebase discovery docs as the parity reference for published surface counts and retained dependency coverage.
- Treat the source-authoritative lens-work tree as the implementation surface; do not patch only the copied prompt stubs.
- Preserve the 17-command contract exactly, including split-feature.

### References

- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/epics.md
- docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/architecture.md
- TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/dependency-mapping.md

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
