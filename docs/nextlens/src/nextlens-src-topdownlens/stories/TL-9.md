---
feature: nextlens-src-topdownlens
story_id: TL-9
doc_type: story
status: in-progress
title: Constitution Layering For TopDownLens
depends_on: [TL-8]
implementation_kind: schema
epic: 2
spine: true
updated_at: 2026-05-14T04:00:00Z
---

# TL-9 - Constitution Layering For TopDownLens

## Goal

Apply the 4-level constitution hierarchy (org -> domain -> service -> repo) to TopDownLens itself, with informational gates only for the first dev increment.

## Scope

- Org-level constitution at `nextlens` (top-level).
- Domain constitution at `nextlens` (domain layer).
- Service constitution at `nextlens/src`.
- Feature-level constitution stub at `nextlens-src-topdownlens` (may be empty / inherit).
- Gate mode declaration (informational for first increment) and rationale.

## Acceptance

- Each level documents its scope and inheritance (additive).
- Hard gates are explicitly NOT enforced for the first increment; gate mode is recorded as informational with rationale.
- Constitutions are resolved via `lens-constitution resolve`; the resolved view is documented in the feature's docs.
- Per-story implementations can show which constitution articles apply to them.

## Files To Produce

- `TargetProjects/lens/Lens.Core.Governance/orgs/nextlens/constitution.md` (or update).
- `TargetProjects/lens/Lens.Core.Governance/domains/nextlens/constitution.md` (or update).
- `TargetProjects/lens/Lens.Core.Governance/domains/nextlens/services/src/constitution.md` (or update).
- Feature-level constitution placeholder under `features/nextlens/src/nextlens-src-topdownlens/`.
- `docs/nextlens/src/nextlens-src-topdownlens/guides/constitution-layering.md` (control-side doc mirror).

## Notes For Dev

- Spine story. Governance writes must go through the approved orchestration boundary (publish-to-governance or lens-git-orchestration governed operations). Direct manual commits to governance main are not an approved path.
- Per session contract: informational gates only; do not block downstream stories on constitution violations.

## Dev Agent Record
