---
feature: lens-dev-new-codebase-expressplan
doc_type: business-plan
status: draft
goal: "Restore /expressplan as the retained express-track planning accelerator that compresses business, technical, and sprint planning into one guarded session"
key_decisions:
  - Keep expressplan in the retained 17-command surface rather than folding it into businessplan or techplan.
  - Preserve express-track-only gating instead of silently allowing full-track features to bypass normal planning order.
  - Keep QuickPlan as an internal wrapper-driven capability rather than a separate published command.
  - Reuse FinalizePlan for downstream bundle generation instead of duplicating epics, readiness, and story-file logic.
open_questions:
  - Decide whether compatibility shims are needed for older references to expressplan-review.md.
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T22:50:00Z
---

# Business Plan - ExpressPlan Command

## Executive Summary

`/expressplan` is the retained command that lets an already-qualified express-track feature move from feature context to implementation-ready planning in one guided session. It is not a shortcut around governance. Its value comes from compressing the already-approved planning sequence into a smaller operator experience while preserving the same hard boundaries: controlled write scope, adversarial review, and explicit handoff into FinalizePlan for the downstream bundle.

The rewrite needs this command because the retained command surface promises a fast planning path for features that do not need the full preplan -> businessplan -> techplan chain. Without `expressplan`, the new codebase can claim parity on paper but still force users back onto the longer full-track route, weakening the published lifecycle model and breaking user expectations carried forward from the old command surface.

## Business Context

The baseline rewrite research identifies `expressplan` as a first-class retained command with medium-to-high compatibility risk. The command exists to compress planning, not to remove it. The retained behavior is:

- QuickPlan produces `business-plan.md`, `tech-plan.md`, and `sprint-plan.md` in one session.
- Adversarial review is a hard gate, not an informational warning.
- FinalizePlan bundle generation is reused rather than reimplemented.
- The command remains express-track only.

This feature therefore protects two product promises at the same time:

1. Experienced operators can move faster when the feature truly qualifies for the express path.
2. Governance quality does not collapse just because the operator wants speed.

## Stakeholders

| Stakeholder | Interest | Sign-off concern |
|---|---|---|
| Feature authors | Need a faster path from feature selection to dev-ready planning | The shortcut still catches gaps before `/dev` |
| Governance owners | Need express-track usage to stay explicit and bounded | Full-track work must not slip through the express path silently |
| Lens maintainers | Need clean-room parity to the retained command surface | The rewrite must preserve the observable command contract without copying old implementation |
| FinalizePlan and Dev owners | Need the downstream bundle and `/dev` handoff to remain coherent | ExpressPlan must reuse FinalizePlan rather than inventing a second downstream planning system |
| Test and release owners | Need focused regressions for the most failure-prone seams | QuickPlan retention, review file naming, and handoff sequencing must be covered directly |

## Success Criteria

1. `/expressplan` remains a published prompt/help surface that routes to `bmad-lens-expressplan`.
2. The command runs light preflight before loading the release prompt and skill.
3. The skill enforces express-track eligibility and blocks unsupported features with a clear explanation.
4. QuickPlan runs through `bmad-lens-bmad-skill` and writes only to the control-repo planning docs path.
5. QuickPlan produces `business-plan.md`, `tech-plan.md`, and `sprint-plan.md` in one session.
6. Adversarial review writes `expressplan-adversarial-review.md` and a fail verdict halts progression.
7. On pass or pass-with-warnings, the skill hands off into the FinalizePlan bundle rather than duplicating epics, stories, readiness, and story-file generation.
8. No direct governance-doc file writes are required from the expressplan conductor.
9. Focused tests prove express-only gating, QuickPlan retention, review hard-stop behavior, and FinalizePlan bundle reuse.
10. The implementation remains clean-room and traceable to retained behavior rather than copied source.

## Scope

### In Scope

- Retain the `/expressplan` prompt stub and release-prompt routing.
- Keep `bmad-lens-expressplan` as the workflow conductor for express-track planning.
- Delegate QuickPlan through `bmad-lens-bmad-skill`.
- Generate the three staged planning docs used by the express path.
- Run adversarial review with party-mode challenge as a hard gate.
- Hand off into FinalizePlan bundle generation and `/dev` readiness.
- Add focused regression coverage around gating, review behavior, and downstream bundle reuse.

### Out of Scope

- Replacing FinalizePlan with new express-only bundle logic.
- Allowing full-track features to silently convert to express in the middle of the workflow.
- Writing governance mirror artifacts directly from prompt or wrapper logic.
- Redefining lifecycle phases or adding a second express-only phase tree.
- Copying old-codebase prompt or skill prose directly into the rewrite.

## Risks and Mitigations

| Risk | Probability | Impact | Mitigation |
|---|---:|---:|---|
| Review artifact naming drifts between `expressplan-review.md` and `expressplan-adversarial-review.md` | Medium | High | Standardize the implementation and tests on the current lifecycle contract, while documenting the older reference as compatibility debt. |
| QuickPlan is treated as deprecated and removed entirely | Medium | High | Keep QuickPlan internal and cover its presence through expressplan-focused regression tests. |
| Express-track gating gets weakened to make demos easier | Medium | High | Treat unsupported track usage as a blocker and require an explicit conversion path outside this command. |
| ExpressPlan duplicates FinalizePlan downstream logic | Medium | Medium | Make the handoff explicit in the skill contract and cover it with narrow integration tests. |
| Operators assume expressplan also performs live PR orchestration | Low | Medium | Keep the docs and tests clear that expressplan stops at reviewed planning outputs and the FinalizePlan bundle handoff. |

## Dependencies

- Baseline retained-command research for `lens-dev-new-codebase-baseline`, especially the expressplan and finalizeplan contract summaries.
- `bmad-lens-bmad-skill`, which provides the Lens-aware write boundary and feature-context wrapper.
- `bmad-lens-adversarial-review`, which supplies the hard gate after QuickPlan.
- `bmad-lens-finalizeplan`, which owns downstream bundle generation and the final PR path.
- Light preflight and module-help surfaces that preserve the published command entrypoint.

## Timing and Delivery Note

This feature should be implementation-ready before the rewrite claims command-surface parity. A new codebase that can do full-track planning but not the retained express path is still missing a published lifecycle promise.

These artifacts are staged in the control-repo docs path only. Governance publication, phase mutation, and PR orchestration remain separate operational steps owned by Lens scripts and skills.