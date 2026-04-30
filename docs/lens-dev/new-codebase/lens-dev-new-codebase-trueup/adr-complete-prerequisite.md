---
feature: lens-dev-new-codebase-trueup
doc_type: adr
adr_id: ADR-1
status: Accepted
accepted_at: 2026-04-28T02:20:00Z
decision: Graceful degradation is canonical for optional completion prerequisites
depends_on:
  - architecture.md
  - TargetProjects/lens-dev/old-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-complete/scripts/complete-ops.py
blocks: []
updated_at: 2026-04-30T00:00:00Z
---

# ADR-1: Complete Prerequisite Handling

## Status

Accepted. This decision was accepted on the TechPlan adversarial review pass at 2026-04-28T02:20:00Z.

## Context

`bmad-lens-complete` is the lifecycle endpoint for a Lens feature. It closes a feature by checking completion readiness, updating governance state, and writing an archive summary. The open design question was how the command should behave when recommended closeout artifacts are missing, especially `retrospective.md` and project documentation captured through `bmad-lens-document-project`.

The decision matters because `finalize` is irreversible. A hard prerequisite model would block archival until every supporting artifact exists. A graceful-degradation model allows archival to proceed when the lifecycle state is valid, while recording missing optional artifacts as warnings.

## Decision

Graceful degradation is canonical.

Missing optional completion artifacts must produce warnings, not hard failures:

- If `retrospective.md` is absent, `check-preconditions` returns `status: warn` and `retrospective_skipped: true`.
- If project documentation is absent, `check-preconditions` returns `status: warn` and `document_project_skipped: true`.
- `finalize` may proceed from `pass` or `warn` after explicit user confirmation.

Lifecycle state guards remain hard failures:

- Feature not found.
- Malformed `feature.yaml` or `feature-index.yaml`.
- Current phase is not completable.
- Missing explicit confirmation for non-dry-run `finalize`.

## Evidence

The old-codebase `complete-ops.py` implements graceful behavior. Its `check-preconditions` path checks whether `retrospective.md` exists and returns a warning when it is missing rather than failing the operation. Its `finalize` path computes `retrospective_skipped` from file existence and includes that state in the archive summary behavior.

The old implementation does not subprocess into retrospective or document-project skills during completion. It treats those outputs as completion evidence, not as runtime dependencies that must be created by `complete` itself.

This matches the architecture decision in `architecture.md` Section 4, ADR-1: missing closeout artifacts are advisory; phase validity is the hard gate.

## Implications

The `bmad-lens-complete` command contract must expose missing optional artifacts explicitly. `check-preconditions` should return a structured payload with warning details and missing-artifact flags so callers can render a clear confirmation gate before archival.

The future `complete-ops.py` implementation must support dry-run output and explicit confirmation. It must never silently finalize after a warning, and it must not invent or patch retrospective/document-project artifacts to make the warning disappear.

The scaffolded tests for `bmad-lens-complete` must include a missing-prerequisite scenario. The expected behavior is warning plus continued eligibility, not a hard block.

## Companion Governance Action

TU-5.1 owns the governance-visible blocker annotation for `lens-dev-new-codebase-complete`. The annotation should state that the complete skill was absent at audit time and that this ADR defines the prerequisite strategy future implementers must preserve.

This ADR does not write governance files directly. The governance write is intentionally separated from the audit and ADR authoring work.
