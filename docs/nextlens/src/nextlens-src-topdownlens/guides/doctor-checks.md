# TopDownLens Doctor Checks

The TopDownLens doctor is a non-mutating health check for the feature docs contract. It emits a JSON list of findings and separates blocking findings from informational findings through each finding's `severity` field.

Run from the control workspace root:

```bash
uv run python TargetProjects/nextlens/src/NextLens/scripts/lens_topdown/doctor_checks.py --feature-docs docs/nextlens/src/nextlens-src-topdownlens
```

Use `--strict` when the caller wants a non-zero exit if any blocking finding is present:

```bash
uv run python TargetProjects/nextlens/src/NextLens/scripts/lens_topdown/doctor_checks.py --feature-docs docs/nextlens/src/nextlens-src-topdownlens --strict
```

Checks covered:

- `missing_id`: every TopDownLens source record must carry a stable ID.
- `broken_reference`: relationship endpoints, relationship evidence, BMAD traceability, BMAD acceptance evidence, and Salmon signal references must resolve by ID.
- `derived_freshness`: `derived/freshness.json` must match the source hash from the current source records when TL-5 output is present.
- `missing_feature_scope`: BMAD packets must name one selected feature and non-empty include, exclude, and guardrail boundaries.
- `bmad_traceability`: BMAD packets must include non-empty outcome, journey, capability, and evidence traceability.
- `open_blocking_salmon_signal`: open blocking Salmon signals are surfaced as promotion blockers.

The fixture definitions in `examples/doctor-fixtures/` are used by the target-repo tests and provide one positive and one negative source set across the checks.