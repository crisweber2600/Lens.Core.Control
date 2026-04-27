---
feature: lens-dev-new-codebase-expressplan
doc_type: implementation-readiness
status: draft
goal: "Confirm the expressplan feature has enough planning detail to move into implementation while keeping known compatibility risks explicit"
key_decisions:
  - Treat this as implementation-ready for code work, while keeping live PR orchestration and governance publication as script-backed implementation concerns.
  - Carry review filename drift and FinalizePlan reuse proof as explicit warnings rather than hidden assumptions.
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T22:50:00Z
---

# Implementation Readiness - ExpressPlan Command

## Verdict

**Pass with warnings**

The feature has enough planning depth to move into implementation. The public surface, technical layering, story decomposition, and regression targets are all explicit. The remaining concerns are implementation-shaping risks, not planning gaps.

## Evidence Matrix

| Area | Status | Evidence |
|---|---|---|
| Business intent | Pass | business-plan.md defines retained value, stakeholders, scope, and success criteria |
| Technical design | Pass | tech-plan.md defines orchestration layers, ADRs, contracts, and test strategy |
| Execution sequencing | Pass | sprint-plan.md and epics.md break the work into bounded slices |
| Story readiness | Pass | stories.md and `stories/*.md` provide actionable implementation slices |
| Review gate | Pass with warnings | expressplan-adversarial-review.md documents risks and chosen responses |
| FinalizePlan bundle readiness | Pass with warnings | finalizeplan-review.md, epics.md, stories.md, and sprint-status.yaml define the downstream bundle, but live PR orchestration remains unexecuted in this docs-only session |

## What Is Ready

- The retained command contract is explicit.
- The prompt, skill, wrapper, review, and handoff boundaries are documented.
- The highest-risk compatibility seams are named and assigned to stories.
- The implementation sequence is small enough to validate incrementally.

## Warnings To Carry Forward

1. **Review filename compatibility**
   Some retained notes still reference `expressplan-review.md`, while the current lifecycle contract expects `expressplan-adversarial-review.md`.

2. **FinalizePlan reuse needs executable proof**
   The planning set is aligned on reuse, but implementation must prove that expressplan does not duplicate downstream bundle logic.

3. **Operational PR steps were not executed here**
   This planning session staged artifacts only. Branch validation, PR creation, and governance publication remain implementation-time script behaviors.

## Ready-For-Implementation Checklist

- [x] Business goals documented
- [x] Technical architecture documented
- [x] Stories broken into implementation slices
- [x] Regression targets named
- [x] FinalizePlan handoff boundary documented
- [ ] Focused command-level tests implemented
- [ ] Live PR topology exercised by code and tests

## Recommendation

Proceed to implementation against the six staged stories. Start with the prompt/help surface and track gating, then add the compressed planning core, and finish with FinalizePlan reuse plus focused regressions. Do not treat governance publication or phase mutation as ad hoc file edits.