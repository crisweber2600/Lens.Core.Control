---
feature: lens-dev-new-codebase-expressplan
doc_type: stories
status: draft
goal: "Define implementation-ready stories for restoring expressplan parity in the new codebase"
key_decisions:
  - Stories stay aligned to the three retained expressplan seams: entry, compressed planning, and FinalizePlan reuse.
  - Acceptance criteria stay observable and testable at the command boundary.
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T22:50:00Z
---

# Stories - ExpressPlan Command

## Story 1.1 - Prompt Routing and Light Preflight

**User story:** As a Lens operator, I want `/expressplan` to fail fast on bootstrap issues so I never begin a compressed planning session from stale or invalid state.

**Acceptance criteria**

- Given `/expressplan` is invoked, when light preflight fails, then the command stops and surfaces the failure clearly.
- Given light preflight succeeds, when the release prompt loads, then the prompt does not embed workflow logic itself and instead delegates to `bmad-lens-expressplan`.
- The prompt/help surface remains discoverable after the rewrite.

## Story 1.2 - Express-Track Gating and State Checks

**User story:** As a governance owner, I want expressplan to reject unsupported feature state so the express path never becomes a hidden full-track bypass.

**Acceptance criteria**

- Given a feature is not eligible for the express track, when `/expressplan` runs, then the skill stops before QuickPlan.
- The failure message tells the operator what to do next instead of implying the command is broken.
- No feature track conversion happens implicitly inside the command.

## Story 2.1 - QuickPlan Wrapper Delegation

**User story:** As a feature author, I want expressplan to generate the three compressed planning docs in one session so I can move quickly without losing structured planning output.

**Acceptance criteria**

- The skill delegates to `bmad-lens-bmad-skill --skill bmad-lens-quickplan`.
- The wrapper resolves feature context and writes only to the staged docs path for the active feature.
- `business-plan.md`, `tech-plan.md`, and `sprint-plan.md` are produced in the staged docs path.

## Story 2.2 - Adversarial-Review Hard Gate

**User story:** As a release owner, I want a failed expressplan review to halt progression so the compressed path cannot bypass planning quality.

**Acceptance criteria**

- ExpressPlan invokes `bmad-lens-adversarial-review --phase expressplan --source phase-complete` after QuickPlan artifacts exist.
- A `fail` verdict blocks FinalizePlan handoff.
- A pass or pass-with-warnings verdict writes the chosen review artifact filename consistently.

## Story 3.1 - FinalizePlan Handoff and Phase Advance

**User story:** As a maintainer, I want expressplan to reuse FinalizePlan rather than clone it so downstream planning behavior stays in one place.

**Acceptance criteria**

- ExpressPlan signals completion and hands off into FinalizePlan-owned bundle generation.
- ExpressPlan does not generate epics, stories, readiness, sprint status, or story files itself.
- Auto-advance messaging remains aligned with lifecycle metadata.

## Story 3.2 - Regression Net and Help-Surface Consistency

**User story:** As a maintainer, I want focused regressions around the highest-risk seams so future refactors cannot quietly break the express path.

**Acceptance criteria**

- Tests cover express-only gating, QuickPlan retention, review hard-stop behavior, and FinalizePlan bundle reuse.
- Prompt/help/module surfaces still list expressplan after implementation.
- Review filename expectations are checked directly to prevent drift.

## Dependency Summary

- Stories 1.1 and 1.2 must land before compressed planning logic is trustworthy.
- Story 2.1 must precede Story 2.2 because review depends on staged docs.
- Story 3.1 depends on the review hard gate being stable.
- Story 3.2 closes the loop and makes the feature safe to hand off into implementation.