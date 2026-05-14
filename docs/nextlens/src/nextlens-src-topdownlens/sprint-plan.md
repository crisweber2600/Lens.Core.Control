---
feature: nextlens-src-topdownlens
doc_type: sprint-plan
status: approved
track: express
phase: expressplan
inputDocuments:
  - docs/nextlens/src/rawNotes/Reimagine.md
  - docs/nextlens/src/rawNotes/TopDown.md
goal: "Sequence the first buildable TopDownLens module increment from express planning into FinalizePlan, including the self-hosting and dogfooding spine."
key_decisions:
  - First dev increment is scoped to the spine: TL-1, TL-4, TL-8, TL-9, TL-12 plus enabling stories from TL-2, TL-3, TL-6, TL-10, TL-11.
  - Suggested sprint order ends with TL-12 (Dogfooding Acceptance) as the explicit closure gate.
  - For the first run, TL-12 acceptance is relaxed to skip `nextlens-release` verification because that repo does not yet exist.
  - TL-12 depends_on at minimum: TL-1, TL-4, TL-8, TL-9.
  - Target repo for the first dev increment is Lens.Core.Control only; nextlens-control remains a forward-looking concept.
open_questions: []
depends_on: [business-plan, tech-plan]
blocks: []
updated_at: 2026-05-14T03:30:00Z
---

# Sprint Plan - TopDownLens Module

## Sprint Objective

Deliver a small, coherent implementation plan for the TopDownLens module. The sprint should establish the new Lens mental model, schemas, storage topology, BMAD bridge, and Salmon signal contract without attempting a full framework rewrite.

## Delivery Strategy

Build the module spine first:

1. Define stable data contracts.
2. Define the source-of-truth topology.
3. Support one top-down discovery walkthrough.
4. Preserve bottom-up feature-first behavior.
5. Generate a focused BMAD packet for one selected feature.
6. Rebuild a minimal derived graph.
7. Record Salmon signals for upstream correction.

## Work Packages

### TL-1 - Module Ontology And Storage Contract

**Goal:** Define the core objects and where they live.

**Scope:**
- `feature.yaml`
- relationship records
- landscape entity records
- derived graph files
- Salmon signal records
- BMAD packet schema
- hierarchy records from system to product area to outcome to journey to capability to feature

**Acceptance:**
- Stable IDs are required for all durable entities.
- Paths are treated as mutable addresses.
- Graph files are marked derived and rebuildable.
- Feature archive, living landscape, and derived graph are separate.
- The hierarchy explicitly names system, product area, outcome, journey, capability, feature, story, task, code, test, and evidence levels.

### TL-2 - Top-Down Discovery Walkthrough

**Goal:** Document and/or prototype the flow from large product idea to selected feature.

**Scope:**
- Raw context capture.
- System thesis extraction.
- Product area map.
- Role and stakeholder map.
- Outcome map.
- Journey map.
- Capability candidates.
- Selected vertical feature.
- Focused BMAD packet.

**Acceptance:**
- The walkthrough does not begin with domain/service/feature.
- At least one selected feature traces to system, product area, role, outcome, journey, capability candidate, and evidence.
- Candidate services are labeled implementation consequences, not planning roots.

### TL-3 - Bottom-Up Compatibility Rules

**Goal:** Ensure TopDownLens does not break the feature-first mental model.

**Scope:**
- Standalone feature creation rules.
- Adjacency detection concept.
- Repeated pressure categories.
- Promotion threshold guidance.
- Bottom-up promotion path from feature to capability to product area to domain/system when evidence exists.

**Acceptance:**
- A feature can remain independent forever.
- Adjacency is weak by default.
- Promotion is advisory and requires evidence.
- The rule "no growth without pressure" is encoded in the planning model.

### TL-4 - BMAD Bridge Packet

**Goal:** Define how LENS hands one selected feature to BMAD.

**Scope:**
- Packet schema.
- Source context list.
- Include/exclude guardrails.
- Required BMAD artifacts.
- Traceability fields.

**Acceptance:**
- Packet targets exactly one feature.
- Packet includes scope boundaries and acceptance evidence.
- Packet includes outcome/journey traceability when available.
- BMAD remains responsible for PRD, architecture, epics, stories, implementation, and review.

### TL-5 - Minimal Derived Graph Rebuild

**Goal:** Define or implement a rebuildable graph projection from source files.

**Scope:**
- Relationship index.
- Traceability index.
- Derived map metadata.
- Rebuild freshness marker.

**Acceptance:**
- Source files remain authoritative.
- Rebuild output is disposable.
- Broken references are reported, not silently ignored.

### TL-6 - Salmon Signal Contract

**Goal:** Define recursive upstream consistency validation.

**Scope:**
- Signal schema.
- Signal lifecycle.
- Severity levels.
- Upstream targets.
- Recommended actions.

**Acceptance:**
- Signals can target feature, journey, outcome, product area, landscape, or BMAD correct-course.
- Signals distinguish local note, landscape update, split feature, and blocking correction.
- Signals preserve evidence and provenance.

### TL-7 - Doctor Checks

**Goal:** Add deterministic health checks for the new topology.

**Scope:**
- Missing IDs.
- Broken references.
- Derived graph freshness.
- Missing feature scope.
- Missing BMAD packet traceability.
- Open blocking Salmon signals.

**Acceptance:**
- Checks return machine-readable results.
- Checks do not mutate files.
- Blocking and informational findings are separated.

### TL-8 - Self-Hosting Repo Topology Contract

**Goal:** Define the `nextlens-control` / `nextlens-governance` / `nextlens-release` split so TopDownLens can dogfood itself after the first dev increment.

**Scope:**
- Write-scope boundaries per repo.
- Publication boundary rules (no direct governance or release patches).
- Migration window between incubation in `Lens.Core.Governance` and a live `nextlens-governance`.
- Mapping between control-repo `featureId` and TopDownLens `feature.<slug>`.

**Acceptance:**
- A clear contract states which repo accepts which writes.
- The migration boundary is documented as a one-time copy plus reconcile.
- The feature identity mapping is recorded in `feature.yaml`.

### TL-9 - Constitution Layering For TopDownLens

**Goal:** Apply the 4-level additive constitution model to TopDownLens with module-specific extensions.

**Scope:**
- Org / domain / service / repo levels for TopDownLens.
- Additive fields: `required_doctor_checks`, `promotion_evidence`, `salmon_routing`.
- Resolution rules: additive only, resolution must succeed before authoring.
- Constitution prose is passed to every authoring delegate.

**Acceptance:**
- Constitution scaffolds exist (or are explicitly deferred) for `nextlens` and `nextlens/src`.
- Additive extension fields are validated.
- A resolution failure blocks TopDownLens authoring.

### TL-10 - Bugfix Flow (Lens-Core-Bugfix Pattern)

**Goal:** Wire a governed correction loop for TopDownLens defects without allowing direct edits in governance or release repos.

**Scope:**
- Tracked bugfix feature creation in `nextlens-control`.
- Standard lifecycle gates or hotfix-express track.
- Salmon-to-bugfix routing rules for `high` and `blocking` severities.
- Publication via `publish-to-governance` and `promote-to-release` only.

**Acceptance:**
- Defects always produce a tracked feature, evidence, and reviewed publication.
- Direct edits in release or governance are rejected by pipeline guardrails.
- Salmon-triggered bugfixes carry their originating signal ID.

### TL-11 - GitHub Actions Pipelines

**Goal:** Stand up the three pipelines required for self-hosting.

**Scope:**
- `promote-to-release` workflow mirroring the existing Lens release pipeline.
- `publish-to-governance` workflow restricted to lifecycle metadata changes on protected branches.
- `regression-and-doctor` pipeline for schema validation, doctor checks, derived graph round-trip, and Salmon lint on PRs.

**Acceptance:**
- Workflows live in `nextlens-control` and are mirrored to `nextlens-release` only via `promote-to-release`.
- All pipelines fail closed on missing validators.
- No pipeline can mutate `nextlens-release` or `nextlens-governance` from a feature branch.

### TL-12 - Dogfooding Acceptance Run

**Goal:** Prove TopDownLens can be used to plan TopDownLens itself.

**Scope:**
- After the first dev increment, create the next TopDownLens feature using TopDownLens commands.
- Run doctor checks, BMAD packet generation, and one Salmon signal end-to-end.
- Capture friction notes as input for the next sprint.

**Acceptance:**
- Next feature's planning artifacts are produced by TopDownLens commands, not by hand.
- Doctor checks pass before FinalizePlan.
- A deliberately broken assumption produces a routable Salmon signal observable in `nextlens-governance`.
- `nextlens-release` is updated only by GitHub Actions during the run.

## Suggested Sprint Order

1. TL-1 - Module Ontology And Storage Contract.
2. TL-4 - BMAD Bridge Packet.
3. TL-8 - Self-Hosting Repo Topology Contract.
4. TL-9 - Constitution Layering For TopDownLens.
5. TL-2 - Top-Down Discovery Walkthrough.
6. TL-3 - Bottom-Up Compatibility Rules.
7. TL-6 - Salmon Signal Contract.
8. TL-10 - Bugfix Flow (Lens-Core-Bugfix Pattern).
9. TL-11 - GitHub Actions Pipelines.
10. TL-5 - Minimal Derived Graph Rebuild.
11. TL-7 - Doctor Checks.
12. TL-12 - Dogfooding Acceptance Run.

## Dependencies

- Existing Lens feature context remains active for this express feature.
- BMAD remains installed and available as the downstream planning/execution method.
- The current domain/service/feature lifecycle remains available for backward compatibility.

## Risks To Carry Into FinalizePlan

- The first build may need to choose between `docs/` and `_bmad-output/lens/` for durable module outputs.
- If command names are decided too early, they may hard-code the wrong mental model.
- Automated extraction from raw notes should start as assisted synthesis, not unattended truth creation.
- Bottom-up promotion thresholds need examples and tests to avoid over-modeling.
- Salmon signal routing needs severity rules before implementation agents rely on it.

## Definition Of Ready For Dev

- FinalizePlan converts this sprint plan into epics, implementation readiness, and story files.
- Story files include concrete file paths and schema examples.
- Each story states whether it is documentation-only, schema implementation, CLI implementation, or test coverage.
- No dev story requires implementing the full future Lens system in one pass.
