# TopDownLens Dogfood Report

## Verdict

pass-with-warnings

## Summary

TopDownLens was run against its own feature artifacts from system thesis to one selected feature and one BMAD packet. The run validates the stable-ID hierarchy, source-vs-derived storage boundary, BMAD handoff packet, governance topology, constitution layering evidence, workflow-template packaging, and Salmon signal loop.

## Acceptance Results

| Criterion | Result | Evidence |
| --- | --- | --- |
| Complete top-down walkthrough recorded under `dogfood/` | Pass | `dogfood/walkthrough.md` |
| Exactly one selected feature and one BMAD packet produced | Pass | `feature.topdownlens_contract`; `dogfood/bmad-packet.json` |
| All entities in the chain have stable IDs and are referenced by ID | Pass | `system.nextlens` -> `product_area.planning_intelligence` -> `outcome.bmad_ready_feature` -> `journey.capture_to_packet` -> `capability.topdown_discovery` -> `feature.topdownlens_contract` |
| Topology verified in nextlens-control and nextlens-governance | Pass | control branch `nextlens-src-topdownlens-dev`; governance `feature.yaml` present |
| First-run nextlens-release verification skipped and recorded | Pass with warning | `dogfood/salmon-signals/release-verification-deferred.json` |
| Doctor checks run after TL-7 | Pass with warning | one open blocking Salmon signal remains for missing release verification |
| TL-11 workflow artifacts stored with feature documents | Pass | `workflows/contract-validation.yml`; `workflows/governance-publish-gate.yml`; `workflows/regression-and-doctor.yml` |

## Topology Verification

- Control: branch `nextlens-src-topdownlens-dev`; docs root `docs/nextlens/src/nextlens-src-topdownlens` exists.
- Governance: `TargetProjects/lens/Lens.Core.governance/features/nextlens/src/nextlens-src-topdownlens/feature.yaml` exists and records `phase: finalizeplan-complete`.
- Release: `nextlens-release` verification skipped because the release repo does not exist yet.

## Warnings

- TL-11 workflow YAML is stored as docs-owned templates, not installed into root `.github/workflows/`; installation still requires an approved setup or publish path.
- The doctor intentionally reports `open_blocking_salmon_signal` for `salmon.20260514T045800Z.blocking_release_missing` until release-repo verification is available.
- `dogfood/salmon-signals/release-verification-deferred.json` records the relaxed first-run skip so the missing release topology is visible on the next pass.

## BMAD Handoff Result

The dogfood BMAD handoff was simulated. It produced one packet at `dogfood/bmad-packet.json` and listed the already implemented artifacts that BMAD would receive as context for downstream implementation and review.

## Closure Decision

The TopDownLens feature is acceptable for first-run completion with warnings. All stories now have docs or implementation artifacts recorded; workflow installation and release-repo verification remain deferred topology operations.