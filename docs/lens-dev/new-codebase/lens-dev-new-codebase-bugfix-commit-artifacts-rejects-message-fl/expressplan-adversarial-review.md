---
feature: lens-dev-new-codebase-bugfix-commit-artifacts-rejects-message-fl
doc_type: expressplan-adversarial-review
status: responses-recorded
review_format: abc-choice-v1
track: express
updated_at: '2026-05-04T02:00:00Z'
---

# ExpressPlan Adversarial Review

## Scope

Reviewed artifacts:

- `business-plan.md`
- `tech-plan.md`
- `sprint-plan.md`

Feature bundles 11 script-error and on-the-fly-script bugs into one express-track fix plan.

## Findings

### F1 — Generic `--field --value` support could be too broad

Severity: Medium

The plan proposes allowlisted generic updates. If implemented without a tight allowlist, it could bypass field-specific lifecycle validation.

Response: Accepted. S1 must either support only `--field phase --value <phase>` through the same transition validator as `--phase`, or reject all generic fields with structured guidance. No broad arbitrary YAML mutation is allowed.

### F2 — Preflight full-sync restoration could increase prompt-start latency

Severity: Medium

Restoring the old full preflight behavior may reintroduce slower prompt startup or remote dependency sensitivity.

Response: Accepted. S3 must preserve the timestamp/freshness behavior from the full helper and treat offline authority repo sync the same way the old-codebase helper did. If delegation is not restored, docs must explicitly describe the lighter gate.

### F3 — On-the-fly script prevention needs enforcement beyond prose

Severity: Low

AGENTS.md guidance alone may not stop future conductors from using ad hoc Python/PowerShell snippets.

Response: Accepted. S4 should prefer a repo-owned helper for recurring lifecycle state inspection and prompt repair. AGENTS.md updates are necessary but not sufficient for recurring operations.

## Party-Mode Blind-Spot Challenge

**Mary (Analyst):** The business value is clear, but several bugs overlap around the same symptom: conductor command drift. The dev stories should avoid fixing each symptom independently if one command-compatibility layer can address multiple failures.

**Winston (Architect):** The risky part is not aliasing; it is accidental bypass of validators. All aliases must call existing internal functions instead of duplicating validation logic.

**Quinn (QA):** Tests need to prove both success and rejection cases. For `--field --value`, test unsupported fields. For preflight, test documented arguments. For prompt repair, test literal newline-token fixtures.

**Bob (SM):** Four stories are reasonable for express scope, but S4 can sprawl. Keep S4 limited to helpers/guidance directly tied to the observed chat-log failures.

## Verdict

`pass-with-warnings`

The plan is implementation-ready for FinalizePlan. Warnings are captured in story acceptance criteria and do not block advancing to `expressplan-complete`.
