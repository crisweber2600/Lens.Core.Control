---
feature: nextlens-src-topdownlens
story_id: TL-7
doc_type: story
status: not-started
deferrable: true
title: Doctor Checks
depends_on: [TL-1, TL-4, TL-6]
implementation_kind: cli
epic: 3
spine: false
updated_at: 2026-05-14T04:00:00Z
---

# TL-7 - Doctor Checks

## Goal

Provide deterministic, non-mutating health checks for TopDownLens topology and contracts.

## Scope

- Missing IDs.
- Broken references.
- Derived graph freshness (consumes TL-5 output if present).
- Missing feature scope.
- Missing BMAD packet traceability (validates TL-4 packets).
- Open blocking Salmon signals (consumes TL-6 schema).

## Acceptance

- Checks return machine-readable results (JSON list of findings with severity, target_id, message).
- Checks do not mutate files.
- Blocking and informational findings are separated in the output.
- A `--strict` mode exits non-zero on any blocking finding.
- Each check has at least one positive and one negative test fixture.

## Files To Produce

- CLI entry point (location TBD with dev).
- `docs/nextlens/src/nextlens-src-topdownlens/guides/doctor-checks.md`.
- Test fixtures under `docs/nextlens/src/nextlens-src-topdownlens/examples/doctor-fixtures/`.

## Notes For Dev

- **Deferrable** beyond first dev increment; TL-12 closure can substitute manual validation if TL-7 slips.
- Wire into TL-11 `regression-and-doctor` pipeline once shipped.
