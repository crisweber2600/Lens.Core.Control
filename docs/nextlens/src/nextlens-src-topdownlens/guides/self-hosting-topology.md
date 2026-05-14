---
feature: nextlens-src-topdownlens
doc_type: guide
story_id: TL-8
title: Self-Hosting Repo Topology
updated_at: 2026-05-14T04:40:00Z
---

# Self-Hosting Repo Topology

TopDownLens self-hosting uses the same role split as Lens: the control repo owns planning and branch topology, the governance repo owns authoritative metadata and published artifacts, release is a read-only consumer surface, and target repos contain implementation work.

For the first dev increment, the active target is `Lens.Core.Control` and durable outputs remain under `docs/nextlens/src/nextlens-src-topdownlens/`. The future self-hosting topology keeps the same boundaries when the dedicated `nextlens-*` repos exist.

## Repo Roles

| Repo | Role | Default Branch | Writes Allowed By |
| --- | --- | --- | --- |
| `nextlens-control` | Planning workspace, feature branch topology owner, dev-cycle docs staging. | `main` | Lens planning/dev conductors on feature branches. |
| `nextlens-governance` | Metadata authority, feature index, lifecycle state, constitution hierarchy, published docs mirror. | `main` | Approved governance orchestration only. |
| `nextlens-release` | Published module payload and read-only consumer surface. | `Stable` or protected release branch | `promote-to-release` only. |
| `nextlens-<domain>` target repos | Implementation surfaces for code, tests, fixtures, and runtime docs owned by a domain. | repo-specific | Dev implementation flow on governed working branches. |

## Branch Model

- Control repo planning branches follow the Lens feature lifecycle: `{feature_id}-plan`, `{feature_id}`, and `{feature_id}-dev`.
- Governance remains on `main`; feature-branch governance topology is not allowed.
- Release remains protected; local release clones are read-only.
- Target repos use repo-scoped working branches prepared through `lens-git-orchestration`.
- A target branch may be `feature/{feature_id}`, `feature/{feature_id}-{user}`, or direct-default only when governance inventory explicitly allows it.

## Write Boundaries

Control repo writers may update only the resolved feature docs path for planning/dev artifacts unless a dedicated setup operation says otherwise. Governance writers must not patch feature folders or docs mirror paths by hand. Release writers must never mutate local release clone files directly. Target repo writers must stay inside the resolved target repo path.

Direct writes are forbidden to:

- Governance feature folders.
- Governance docs mirror paths.
- Release clone paths.
- Generated setup mirrors under control-root `.github` unless the setup/sync command owns the change.

## Approved Orchestration Boundaries

The approved mutation boundaries are:

- `publish-to-governance` for copying reviewed control docs into the governance mirror.
- `promote-to-release` for publishing reviewed module payloads into release.
- `lens-git-orchestration` governed operations for branch preparation, push, PR, and lifecycle-safe git writes.
- `lens-feature-yaml` lifecycle metadata operations for authoritative feature state changes.

No hand-copy fallback is approved for governance or release publication.

## Bootstrap Notes

During incubation, TopDownLens features live in `Lens.Core.Control` and use `Lens.Core.Governance` constitutions. The first increment records contracts and evidence under `docs/nextlens/src/nextlens-src-topdownlens/`. After the dedicated repos exist, new TopDownLens features move through the same boundaries with `nextlens-control`, `nextlens-governance`, and `nextlens-release` replacing the incubation surfaces.