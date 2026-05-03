---
feature: lens-dev-new-codebase-bugfix-bug-fixer-saves-planning-artifacts
doc_type: tech-plan
status: approved
goal: "Define technical enforcement for docs path correctness and quick-dev-first execution order in expressplan orchestration."
key_decisions:
  - Resolve and use `feature.yaml.docs.path` as the only write target for expressplan artifacts.
  - Keep feature track as `express` and phase as `expressplan` until review pass.
  - Implement deterministic command ordering: quick-dev-first, then bounded fallback.
open_questions:
  - Should fallback be disabled entirely when quick-dev succeeds?
  - Should command telemetry be persisted to simplify future bug triage?
depends_on:
  - business-plan.md
blocks: []
updated_at: '2026-05-03T19:30:00Z'
---

# Tech Plan - Expressplan Path And Ordering

## Architecture Notes

- Source of truth for docs output is `feature.yaml.docs.path` in governance.
- Expressplan writes:
  - `business-plan.md`
  - `tech-plan.md`
  - `sprint-plan.md`
  - `expressplan-adversarial-review.md`
- Writes must stay in control repo staged docs path, never in alternate root locations.

## Execution Contract

1. Validate track gate: `express|expressplan` only.
2. Validate docs path availability.
3. Execute quick-dev-first step.
4. If quick-dev fails, allow bounded fallback (with explicit reason/log).
5. Generate required plan artifacts in the resolved docs path.
6. Run expressplan adversarial review.

## Validation Strategy

- Pre-run: verify feature track and docs.path.
- Run-time: ensure first orchestration action is quick-dev attempt.
- Post-run: verify required files exist only in expected docs directory.

## Risks

- Incorrect path resolution could regress to legacy fallback paths.
- Missing quick-dev-first guard can reintroduce excessive probing behavior.

Mitigations:
- Add acceptance criteria for command ordering and artifact location.
- Add lightweight assertions in integration/regression tests for the bug-fixer flow.
