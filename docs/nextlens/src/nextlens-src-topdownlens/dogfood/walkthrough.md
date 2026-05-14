# TopDownLens Dogfood Walkthrough

## System Thesis

- ID: `system.nextlens`
- Name: NextLens
- Thesis: NextLens should let a team start from a system-level intent, walk down through product area, outcome, journey, and capability, and hand exactly one selected feature to BMAD without losing stable identity or governance context.

## Product Area

- ID: `product_area.planning_intelligence`
- Name: Planning Intelligence
- Reason selected: TopDownLens is the planning intelligence layer that connects raw landscape understanding to feature-ready BMAD work.

## Outcome

- ID: `outcome.bmad_ready_feature`
- Name: BMAD Ready Feature
- Desired change: a selected top-down feature can be transferred to BMAD as one scoped packet with evidence and acceptance context.

## Journey

- ID: `journey.capture_to_packet`
- Name: Capture To Packet
- Path: system thesis -> product area -> outcome -> journey -> capability -> feature -> BMAD packet.

## Capability

- ID: `capability.topdown_discovery`
- Name: Top-Down Discovery
- Role: maintain stable IDs while paths and planning documents evolve.

## Selected Feature

- ID: `feature.topdownlens_contract`
- Name: TopDownLens Contract
- Current path: `docs/nextlens/src/nextlens-src-topdownlens/feature.yaml`
- Reason selected: it is the smallest vertical slice that proves ontology, storage, derived graph, BMAD handoff, Salmon signals, and self-hosting topology can work together.

## BMAD Packet

- Packet ID: `bmad_packet.feature.topdownlens_contract.dogfood`
- Packet path: `docs/nextlens/src/nextlens-src-topdownlens/dogfood/bmad-packet.json`
- Selected feature count: exactly one.
- Handoff mode: simulated BMAD handoff, because this story validates the packet contract rather than starting a new downstream implementation feature.

## Resulting Artifacts

- `schemas/ontology.schema.json`
- `schemas/relationship.schema.json`
- `schemas/landscape-entity.schema.json`
- `schemas/derived-graph.schema.json`
- `schemas/bmad-packet.schema.json`
- `schemas/salmon-signal.schema.json`
- `derived/graph.json`
- `derived/freshness.json`
- `guides/top-down-discovery.md`
- `guides/bmad-bridge.md`
- `guides/self-hosting-topology.md`
- `guides/constitution-layering.md`
- `guides/salmon-signals.md`
- `guides/doctor-checks.md`

## Verification Notes

- Control topology: verified on branch `nextlens-src-topdownlens-dev`; feature docs exist at `docs/nextlens/src/nextlens-src-topdownlens`.
- Governance topology: verified at `TargetProjects/lens/Lens.Core.governance/features/nextlens/src/nextlens-src-topdownlens/feature.yaml`.
- Release topology: skipped for first-run acceptance because `nextlens-release` does not exist yet.
- Doctor result: one blocking finding remains for the intentionally open release-repo Salmon signal; derived freshness is current.