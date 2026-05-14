---
feature: nextlens-src-topdownlens
story_id: TL-8
doc_type: story
status: done
title: Self-Hosting Repo Topology Contract
depends_on: [TL-1]
implementation_kind: docs-only
epic: 2
spine: true
updated_at: 2026-05-14T04:00:00Z
---

# TL-8 - Self-Hosting Repo Topology Contract

## Goal

Document the three-repo plus per-domain repo topology that LENS will host itself with: `nextlens-control`, `nextlens-governance`, `nextlens-release`, and `nextlens-<domain>` target repos. Codify ownership, write boundaries, and branch model.

## Scope

- Repo roles: control, governance, release, target.
- Branch model per repo.
- Ownership and write boundaries per repo.
- Promotion path from control plan branches into governance (publish-to-governance) and from dev branches into release (promote-to-release).
- Bootstrap notes for `nextlens-control` self-hosting LENS itself.

## Acceptance

- Control = planning workspace and branch topology owner.
- Governance = metadata + published-artifact authority on main only.
- Release = published, read-only consumer surface (for current feature: not yet created).
- Target repos = implementation surfaces only.
- The contract explicitly forbids direct writes to governance feature folders, governance docs mirror, and release clone paths from non-orchestration code paths.
- The doc names the approved orchestration boundaries (publish-to-governance, promote-to-release, lens-git-orchestration governed ops).

## Files To Produce

- `docs/nextlens/src/nextlens-src-topdownlens/guides/self-hosting-topology.md`.
- `docs/nextlens/src/nextlens-src-topdownlens/diagrams/topology.md` (mermaid diagram acceptable).

## Notes For Dev

- Spine story. Docs-only. No file writes outside `docs/`.
- TL-9 (constitution layering) and TL-10 (bugfix flow) both reference this topology.

## Dev Agent Record

- Status: done
- Files produced: `guides/self-hosting-topology.md`, `diagrams/topology.md`.
- Validation: local content check confirmed required repo roles, approved orchestration boundaries, and mermaid topology are present.
