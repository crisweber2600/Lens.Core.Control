---
feature: lens-dev-new-codebase-expressplan
doc_type: sprint-plan
status: draft
goal: "Sequence the expressplan-scoped delivery work for the expressplan command rewrite into implementation-ready slices."
key_decisions:
  - Use three delivery slices: foundation validation, discovery and regressions, and packet completion with FinalizePlan handoff.
  - Treat the previous-session infrastructure (prompt stubs, SKILL.md, tests, module.yaml) as Slice 1 already-complete work to be validated rather than re-done.
  - Expand Slice 2 to include discovery wiring, regression hardening, and any missing integration tests.
  - Slice 3 is the planning packet completion and FinalizePlan handoff — distinct from code implementation.
  - Defer unrelated retained-command work to their own features.
open_questions:
  - Which discovery file in the target project should register lens-expressplan?
  - Are there focused test-harness items from baseline 4.5 not yet covered by test-expressplan-ops.py?
depends_on:
  - business-plan.md
  - tech-plan.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-5-rewrite-expressplan.md
blocks:
  - FinalizePlan handoff is blocked until this sprint plan and the adversarial review are accepted.
updated_at: '2026-04-30T00:00:00Z'
---

# Sprint Plan — Expressplan Command

## Sprint Objective

Validate the already-landed expressplan infrastructure, complete discovery wiring and regression
coverage, then advance the expressplan planning packet through the adversarial review gate and
into FinalizePlan.

## Current Packet Status

The previous session created:
- `.github/prompts/lens-expressplan.prompt.md` (public stub)
- `_bmad/lens-work/prompts/lens-expressplan.prompt.md` (release prompt)
- `_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md` (conductor)
- `_bmad/lens-work/skills/bmad-lens-expressplan/scripts/tests/test-expressplan-ops.py` (tests)
- `_bmad/lens-work/module.yaml` updated with `lens-expressplan.prompt.md`

These files are untracked in the target source repo and need validation and commit.

## Delivery Slices

| Slice | Objective | Exit Criteria |
| --- | --- | --- |
| Slice 1 | Foundation validation | Prompt stubs, conductor skill, tests, and module.yaml confirmed correct; artifacts committed |
| Slice 2 | Discovery and regressions | lens-expressplan registered in discovery; regression expectations defined and passing |
| Slice 3 | Packet completion and FinalizePlan handoff | Expressplan planning packet complete; review passed; phase advanced to expressplan-complete |

## Slice 1 — Foundation Validation

### Scope

- Validate `lens-expressplan.prompt.md` (public) follows shared prompt-start preflight pattern.
- Validate release prompt is a thin redirect to SKILL.md.
- Validate `bmad-lens-expressplan/SKILL.md` implements the three-step conductor contract with
  express-only eligibility gate.
- Confirm `module.yaml` registration shape matches existing patterns.
- Commit the untracked files to the target source repo.

### Deliverables

- All four infrastructure files confirmed valid.
- `module.yaml` registration confirmed.
- Files committed under the target project.

### Risks

- The previous-session infrastructure may have gaps relative to baseline 4.5 obligations
  (e.g., missing eligibility gate or incomplete party-mode enforcement).

## Slice 2 — Discovery and Regressions

### Scope

- Identify the retained command discovery file in the new-codebase target project.
- Register `lens-expressplan` in that discovery surface.
- Define regression expectations for:
  - Express-eligibility gate (non-express features blocked before delegation)
  - QuickPlan delegation route (`bmad-lens-bmad-skill --skill bmad-lens-quickplan`)
  - Adversarial review invocation (`--phase expressplan --source phase-complete`)
  - Phase-advance success on `pass`/`pass-with-warnings`
  - Phase-advance blocked on `fail`
- Verify all regression expectations pass against the landed implementation.

### Deliverables

- `lens-expressplan` registered in the discovery surface.
- Regression expectations documented or covered in test files.
- All defined regressions passing.

### Dependencies

- Slice 1 complete and infrastructure confirmed.
- The existing `test-expressplan-ops.py` covers prompt-start and wrapper-equivalence.

## Slice 3 — Packet Completion and FinalizePlan Handoff

### Scope

- Complete the expressplan planning packet (`business-plan.md`, `tech-plan.md`, `sprint-plan.md`).
- Run expressplan adversarial review gate.
- On pass: write `expressplan-review.md`.
- Advance `feature.yaml` phase to `expressplan-complete`.
- Commit planning artifacts to the `-plan` branch and push.
- Signal FinalizePlan.

### Deliverables

- Complete expressplan planning packet in `docs/lens-dev/new-codebase/lens-dev-new-codebase-expressplan/`.
- `expressplan-review.md` with a pass or pass-with-warnings verdict.
- `feature.yaml` phase updated to `expressplan-complete`.
- Artifacts committed and pushed on `lens-dev-new-codebase-expressplan-plan`.

### Exit Gate

- Adversarial review carries no unresolved fail-level findings.
- All four planned artifacts are staged and committed.
- Phase is advanced to `expressplan-complete`.
- FinalizePlan is signalled as the next step.

## Critical Path

1. Validate the previous-session infrastructure and commit it (Slice 1).
2. Wire discovery and harden regressions (Slice 2).
3. Complete the planning packet, pass the review gate, and hand off to FinalizePlan (Slice 3).

## Definition of Ready for Implementation

The feature is ready for code work when:

1. The expressplan artifact set is complete and the adversarial review has no fail findings.
2. Discovery wiring is confirmed in the target project.
3. Regression expectations are defined and passing.
4. Phase is `expressplan-complete` and FinalizePlan is signalled.
