---
feature: lens-dev-new-codebase-trueup
doc_type: adr
adr_id: ADR-2
status: Accepted
decision: New-codebase constitution template is canonical for permitted tracks
depends_on:
  - architecture.md
  - lens.core/_bmad/lens-work/lifecycle.yaml
  - TargetProjects/lens/lens-governance/constitutions/lens-dev/constitution.md
  - TargetProjects/lens/lens-governance/constitutions/lens-dev/new-codebase/constitution.md
blocks: []
updated_at: 2026-04-30T00:00:00Z
---

# ADR-2: Constitution Track Template

## Status

Accepted.

## Context

The old-codebase and new-codebase constitution templates diverged in their permitted track lists. The new-codebase domain and service constitutions include the express planning tracks, while older templates omitted them or lagged behind the lifecycle contract.

This matters for `create-domain` and `create-service`. These commands generate future governance containers, so their default constitution template determines which planning tracks new Lens work can choose without a manual governance correction.

## Decision

The new-codebase constitution template is canonical.

Generated Lens Dev constitutions must permit the current standard track set:

```yaml
permitted_tracks: [quickplan, full, hotfix, tech-change, express, expressplan]
```

`express` and `expressplan` are intentional governance capabilities, not parity gaps.

## Evidence

`lens.core/_bmad/lens-work/lifecycle.yaml` uses schema version 4 and defines `expressplan` as a standalone supported phase. The `express` track starts at `expressplan` and then moves through `finalizeplan` before dev-ready.

The Lens Dev domain constitution in `TargetProjects/lens/lens-governance/constitutions/lens-dev/constitution.md` permits `quickplan`, `full`, `hotfix`, `tech-change`, `express`, and `expressplan`.

The new-codebase service constitution in `TargetProjects/lens/lens-governance/constitutions/lens-dev/new-codebase/constitution.md` inherits the standard tracks and explicitly calls out the express planning tracks for the service.

Active governance records also demonstrate express usage: `lens-dev-new-codebase-new-service`, `lens-dev-new-codebase-switch`, and `lens-dev-new-codebase-complete` use the `express` track.

## Implications

`create-domain` and `create-service` must continue generating constitutions with the canonical track list. Removing `express` or `expressplan` would make new governance containers inconsistent with lifecycle v4 and with active feature records.

Parity audits must classify this divergence as a reviewed decision. It is not a schema defect and should not trigger a migration blocker.

Future changes to the track list should be driven by the lifecycle contract first, then reflected in constitution templates and documented with a new ADR if they change compatibility expectations.

## Parity Audit Classification

Classification: `reviewed-decision`.

The permitted-track divergence is resolved by this ADR. New-codebase behavior is authoritative for future Lens Dev containers.
