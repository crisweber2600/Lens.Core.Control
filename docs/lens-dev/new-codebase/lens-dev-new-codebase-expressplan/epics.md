---
feature: lens-dev-new-codebase-expressplan
doc_type: epics
status: draft
goal: "Break expressplan parity work into implementation epics that preserve express-only gating, QuickPlan compression, review hard-stop behavior, and FinalizePlan bundle reuse"
key_decisions:
  - Organize work around the public command surface, the compressed planning core, and the FinalizePlan handoff seam.
  - Keep regressions close to the highest-risk compatibility points.
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T22:50:00Z
---

# Epics - ExpressPlan Command

## Epic 1 - Command Surface and Eligibility

**Goal:** Keep `/expressplan` discoverable, preflight-gated, and legally bounded to express-track usage.

| Story | Outcome | Estimate | Dependencies |
|---|---|---:|---|
| Story 1.1 - Prompt routing and preflight | Public stub prompt runs light preflight and delegates cleanly into the release prompt | 2 | None |
| Story 1.2 - Express-track gating and state checks | Skill blocks unsupported track or phase combinations with explicit guidance | 2 | 1.1 |

**Definition of Done**

- `/expressplan` appears in the expected prompt/help surfaces.
- Preflight failure stops before skill logic loads.
- Unsupported tracks do not progress into QuickPlan.

## Epic 2 - Compressed Planning Core

**Goal:** Preserve one-session planning compression without losing governance or review quality.

| Story | Outcome | Estimate | Dependencies |
|---|---|---:|---|
| Story 2.1 - QuickPlan wrapper delegation | ExpressPlan invokes QuickPlan through the Lens wrapper and writes the three staged planning docs to the feature docs path | 3 | 1.2 |
| Story 2.2 - Adversarial-review hard gate | Review writes the expressplan review artifact and blocks progression on fail | 3 | 2.1 |

**Definition of Done**

- `business-plan.md`, `tech-plan.md`, and `sprint-plan.md` are produced through the wrapper.
- `expressplan-adversarial-review.md` is written to the staged docs path.
- Fail verdicts prevent FinalizePlan handoff.

## Epic 3 - FinalizePlan Handoff and Regression Safety

**Goal:** Reuse FinalizePlan for downstream bundle generation and prove that reuse with narrow, durable tests.

| Story | Outcome | Estimate | Dependencies |
|---|---|---:|---|
| Story 3.1 - FinalizePlan handoff and phase advance | ExpressPlan signals FinalizePlan completion boundaries instead of duplicating bundle logic | 3 | 2.2 |
| Story 3.2 - Regression net and help-surface consistency | Focused regressions cover gating, review naming, bundle reuse, and surface discoverability | 3 | 3.1 |

**Definition of Done**

- ExpressPlan stops owning downstream bundle generation.
- Focused tests cover the retained high-risk seams.
- Help and lifecycle metadata remain aligned with the implemented behavior.

## Cross-Epic Risks

| Risk | Affected epics | Handling |
|---|---|---|
| Review filename drift | 2, 3 | Lock the chosen filename in tests and docs |
| QuickPlan deletion risk | 2 | Cover wrapper delegation explicitly |
| Silent full-track bypass | 1 | Fail fast with guidance |
| Bundle duplication | 3 | Treat FinalizePlan reuse as a test-backed contract |