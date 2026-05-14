# Brainstorm - nextlens-src-implement

## Purpose

Define the smallest useful implementation path for nextlens-src-implement using source material under docs/nextlens/src.

## Scope and Constraints

- Stable IDs are authoritative; paths are mutable.
- Feature or slice is the operational root.
- Derived graph is disposable and rebuilt from authoritative files.
- BMAD receives one selected vertical feature packet.
- Doctor checks are non-mutating and machine-readable.
- Salmon is an upstream correction mechanism.
- No direct governance or release writes.
- First demo target: ambiguous idea -> one selected feature -> validated BMAD packet -> doctor output.

## Key Decision Record

1. Shell entry: interactive wizard.
2. Product context ingestion: inline with new-system (selected B).
3. ID strategy: semantic ID for human-visible surfaces; opaque ID for machine-only surfaces.
4. Landscape storage: docs/{system}/landscape in control repo.
5. Graph rebuild: eager on every landscape write.
6. Feature selection: evidence-ranked proposal with human confirmation gate.
7. BMAD packet boundary: full system context.
8. Output root: feature.yaml docs.path.
9. Doctor mode: pre-flight hook before every write, still non-mutating.
10. Doctor output: structured JSON lines.
11. Salmon trigger: multi-source (human, doctor, review).
12. Salmon routing: route by signal class to landscape, feature notes, or correct-course.
13. Constitution at init: bootstrap constitution embedded in command.
14. Idempotency: merge behavior on rerun.
15. Implementation location: TargetProjects/nextlens/src/NextLens.

## Idea Clusters

### Command Spine and Runtime Shape

- Single command spine with deterministic outputs.
- One-fixture ingestion contract for v1.
- Output root always derived from feature metadata.
- First-demo evidence bundle as one audit object per run.

### Identity and Data Durability

- Human-visible ID rule gate to decide semantic vs opaque IDs.
- Merge-safe idempotency for reruns.
- Rename shock testing for semantic ID durability.

### Graph and Packet Discipline

- Eager graph rebuild on successful landscape writes.
- One-packet vertical slice generation with human confirmation.
- Packet checksum and metadata capture for reproducibility.

### Validation and Correction Loops

- One JSONL doctor report schema with stable check IDs.
- Multi-source Salmon trigger with de-duplication by fingerprint.
- Vocabulary normalization guard for feature/slice terminology drift.

### Lifecycle and Boundary Safety

- Zero governance and release mutation guard via write allowlist.
- Status drift triangulation across story, sprint-status, dev-session, retrospective.
- Workflow installability detection separates template docs from installed automation.

## Resource-Constrained v1 Blueprint

Build exactly one end-to-end path with these hard limits:

- one command
- one input fixture shape
- one selected packet
- one graph rebuild rule
- one doctor report schema
- one Salmon event per failure domain
- zero direct governance or release writes

## Failure Modes to Test First

1. Vocabulary drift between feature and slice.
2. Status drift across artifacts.
3. Missing release surface.
4. Output root ambiguity.
5. Workflow docs not installed as executable workflows.
6. Semantic ID rename breakage.
7. Fetch-context runtime dependency gaps.
8. Duplicate Salmon event noise.

## Recommended Build Sequence

1. Implement single-command wizard path with inline ingestion (Decision 2B).
2. Write landscape files to docs path with stable ID gate logic.
3. Rebuild derived graph eagerly on each write.
4. Generate one ranked candidate packet and require human confirm.
5. Emit doctor JSONL report and enforce pre-flight checks before writes.
6. Route one Salmon event for detected failures with de-duplication.
7. Produce first-demo evidence bundle and run drift checks.

## Acceptance Signals for Smallest Useful Demo

- One command run from ambiguous context to one confirmed feature packet.
- No writes outside allowed control-repo docs and target implementation paths.
- Doctor output is machine-readable and non-mutating.
- At least one routed Salmon correction event can be generated and traced.
- Evidence bundle is sufficient for preplan review without manual reconstruction.
