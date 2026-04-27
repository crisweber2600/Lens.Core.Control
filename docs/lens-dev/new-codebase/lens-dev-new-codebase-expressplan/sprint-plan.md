---
feature: lens-dev-new-codebase-expressplan
doc_type: sprint-plan
status: draft
goal: "Sequence clean-room /expressplan parity work into testable slices that preserve express-only gating, QuickPlan retention, review hard-stop behavior, and FinalizePlan bundle reuse"
key_decisions:
  - Start with command-surface and gating checks before implementing the compressed workflow.
  - Keep QuickPlan and FinalizePlan integration work in separate sprints so regressions stay attributable.
  - End with focused regressions and help-surface verification rather than broad rewrite testing.
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T22:50:00Z
---

# Sprint Plan - ExpressPlan Command

## Sprint Goal

Restore `/expressplan` as a retained express-track planning conductor that stages the compressed planning docs, enforces a hard review gate, and hands off into FinalizePlan without inventing a second downstream bundle.

## Sprint 1 - Surface and Eligibility

| Story | Outcome | Estimate | Dependencies |
|---|---|---:|---|
| EX-1: Preserve prompt routing and preflight | `/expressplan` stays discoverable and stops cleanly on preflight failure | 2 | business-plan.md, tech-plan.md |
| EX-2: Enforce express-track gating | The skill rejects unsupported track/phase combinations with actionable errors | 2 | EX-1 |

## Sprint 2 - Compressed Planning Core

| Story | Outcome | Estimate | Dependencies |
|---|---|---:|---|
| EX-3: Delegate QuickPlan through the Lens wrapper | QuickPlan receives resolved feature context and writes only to the staged docs path | 3 | EX-2 |
| EX-4: Enforce adversarial-review hard stop | Review fail blocks progression and review pass writes the expected artifact | 3 | EX-3 |

## Sprint 3 - FinalizePlan Handoff

| Story | Outcome | Estimate | Dependencies |
|---|---|---:|---|
| EX-5: Reuse FinalizePlan bundle | ExpressPlan hands off to FinalizePlan instead of generating epics/readiness/story files itself | 3 | EX-4 |
| EX-6: Preserve phase and help-surface consistency | Phase completion, auto-advance messaging, and help/module surfaces stay aligned | 2 | EX-5 |

## Sprint 4 - Verification and Handoff

| Story | Outcome | Estimate | Dependencies |
|---|---|---:|---|
| EX-7: Add focused expressplan regressions | Narrow tests cover gating, file naming, hard-stop review behavior, and bundle reuse | 3 | EX-6 |
| EX-8: Prepare implementation handoff notes | Dev gets file targets, non-goals, and known compatibility risks | 1 | EX-7 |

## Sequencing Notes

- EX-1 and EX-2 lock the public contract before deeper orchestration work starts.
- EX-3 and EX-4 stabilize the compressed planning core.
- EX-5 is the most important parity seam because it prevents bundle duplication.
- EX-6 through EX-8 make the feature safe to hand off into implementation.

## Risks To Track During Execution

| Risk | Mitigation |
|---|---|
| QuickPlan wrapper writes outside the feature docs path | Keep wrapper tests focused on resolved write scope |
| Review filename drift breaks validators | Standardize on the lifecycle contract and add a focused regression |
| ExpressPlan starts mutating governance directly | Preserve script-based publish/update boundaries and keep direct governance writes out of the conductor |
| FinalizePlan reuse silently diverges from standalone flow | Cover the handoff with direct contract tests rather than prose only |

## Definition of Done

- Prompt/help routing is stable.
- Express-only gating is enforced.
- QuickPlan delegation is preserved.
- Review failure halts progression.
- FinalizePlan reuse is proven with focused regressions.
- Dev handoff notes identify remaining compatibility debt clearly.