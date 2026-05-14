---
feature: nextlens-src-topdownlens
doc_type: finalizeplan-review
phase: finalizeplan
source: phase-complete
verdict: pass-with-warnings
status: fixes-applied
critical_count: 0
high_count: 0
medium_count: 3
low_count: 1
carry_forward_blockers: []
updated_at: '2026-05-14T03:35:00Z'
review_format: abc-choice-v1
---

# Adversarial Review: nextlens-src-topdownlens / finalizeplan

**Reviewed:** 2026-05-14T03:25:00Z
**Source:** phase-complete
**Overall Rating:** pass-with-warnings

## Summary

The ExpressPlan packet (business-plan, tech-plan, sprint-plan) is internally consistent and the rerun expressplan-adversarial-review verdict is `pass-with-warnings` with 0 critical, 2 high, 5 medium, 1 low. The packet now covers the top-down hierarchy and a self-hosting / dogfooding layer (repo topology, additive constitution, lens-core-bugfix flow, three GH Actions pipelines, dogfooding acceptance run TL-12). FinalizePlan can proceed to downstream bundle generation, but a small set of metadata gaps must be reconciled in the Pre-Review Fixes Applied section before bundle delegation: `feature.yaml.target_repos` is still empty (required for dev-ready handoff), `key_decisions` frontmatter is empty in business-plan and sprint-plan even though the bodies state explicit decisions, and several ExpressPlan high/medium findings need an explicit accept-or-apply record. No critical blockers were introduced at the FinalizePlan boundary.

## Pre-Review Fixes Applied

Operator answers (recorded 2026-05-14T03:30:00Z): `target_repos = [Lens.Core.Control]`; populate `key_decisions` now; encode TL-12 dependencies and relax acceptance for the first run.

Applied:
- **F1 resolved.** `feature.yaml.target_repos` set to `[Lens.Core.Control]`. `nextlens-control` is explicitly deferred as a forward-looking concept and is not a target repo for the first dev increment.
- **F2 partially resolved.** `business-plan.md` frontmatter already lists `key_decisions`; no change needed. `sprint-plan.md` frontmatter `key_decisions` populated with the spine scoping, sprint order closure, TL-12 deferral, and target-repo decisions.
- **F5 directive recorded.** `bmad-create-story` must emit TL-12 with `depends_on: [TL-1, TL-4, TL-8, TL-9]` and relaxed acceptance for the first run (skip `nextlens-release` verification because the repo is not yet stood up).
- **F3 directive recorded.** `bmad-create-story` must emit `implementation_kind` per story (docs-only | schema | cli | test) and treat unbuilt-repo paths as design specifications, not file targets.
- **F4 directive recorded.** `bmad-sprint-planning` must seed `sprint-status.yaml` with all 12 TL-x story IDs in the suggested sprint order.

Deferred:
- ExpressPlan findings H2, M1, M2, M3, M4, M7, M8 remain deferred as story-level acceptance criteria or carry-forward constraints (see ExpressPlan adversarial review).
- F6 (metadata skew on `expressplan-adversarial-review.md`) is non-blocking and will refresh on next routine update.

## Findings

### Critical

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| - | - | No critical blockers found. | Proceed to plan-PR readiness and downstream bundle generation. |

### High

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| - | - | All previously high findings resolved by Pre-Review Fixes. | n/a |

### Medium / Low

| # | Severity | Dimension | Finding | Recommendation |
|---|----------|-----------|---------|----------------|
| F3 | Medium | Coverage Gaps | TopDownLens is forward-looking; many target file paths do not yet exist. `bmad-create-story` must distinguish documentation-only stories from implementation stories. | Require per-story `implementation_kind` field; treat forward-looking paths as design specifications. (Directive recorded in Pre-Review Fixes.) |
| F4 | Medium | Assumptions and Blind Spots | Constitution sets `enforce_stories: true` (informational); sprint-status must include every TL-x story or the post-bundle metadata gate will diverge. | `bmad-sprint-planning` must seed `sprint-status.yaml` with all 12 TL-x story IDs in the suggested sprint order. (Directive recorded.) |
| F5 | Medium | Complexity and Risk | If TL-12 is not explicitly the final story with declared dependencies, the dogfooding test could be reordered out of the first increment. | Encode `depends_on: [TL-1, TL-4, TL-8, TL-9]` on TL-12 and relax acceptance to skip `nextlens-release` verification for the first run. (Directive recorded.) |
| F6 | Low | Coverage Gaps | `expressplan-adversarial-review.md` has minor `updated_at` vs body timestamp skew. | Refresh on next routine doc update; non-blocking. |

## Accepted Risks

- ExpressPlan H2 (output root) and M4 (`fetch-context` runtime gap) are explicitly carried as deferred constraints into the first dev increment.
- ExpressPlan M1, M2, M3 are deferred to story-level acceptance criteria authored by `bmad-create-epics-and-stories`.

## Party-Mode Challenge

John (Product): The plan is now thick enough that "what visible thing ships first" can hide behind the spine. If the first dev demo is just schemas and a graph rebuild, nobody outside engineering will see TopDownLens working. The downstream bundle must protect a user-visible walkthrough story (TL-2) inside the first increment.

Winston (Architect): FinalizePlan is about to commit a story queue that calls into a future `nextlens-control` repo that does not exist yet. If the bundle generates story files whose `target_repos` resolve to that ghost, the first `/dev` invocation will fail at branch creation. Either record the control repo (Lens.Core.Control) as the only initial target and treat all nextlens-control work as documentation-only, or stand up the new repo before `/dev`.

Sally (Release Engineer): TL-11 (GH Actions Pipelines) is the most expensive story and the dogfooding gate (TL-12) depends on it. If the bundle does not split TL-11 into "implement workflows" and "wire negative tests" stories, the dogfooding acceptance criteria will be impossible to verify before the loop closes.

### Blind-Spot Questions

1. Will `target_repos` for the first dev increment be just `Lens.Core.Control`, or do we also need to stub `nextlens-control` at FinalizePlan time?
2. Which single TL-x story is the user-visible demo that proves the spine is more than schemas?
3. Are we comfortable letting `bmad-create-story` split TL-11 into multiple stories, or do we want one story per work package?
4. Does the constitution change required by TL-9 (additive fields `required_doctor_checks`, `promotion_evidence`, `salmon_routing`) need to land as a constitution edit before `/dev`, or is it acceptable to ship it inside the first increment?
5. Should TL-12 be allowed to mark the increment "done" without exercising `nextlens-release` (since the repo is not stood up yet), or should TL-12's acceptance criteria be relaxed for the first run?

## Open Questions Surfaced

- FinalizePlan must resolve `feature.yaml.target_repos` before opening the final dev-ready PR.
- FinalizePlan must populate `key_decisions` in `business-plan.md` and `sprint-plan.md` frontmatter or record explicit deferral here.
- FinalizePlan must instruct `bmad-create-story` to emit `implementation_kind` per story and to encode TL-12's dependencies.
- FinalizePlan must confirm whether `nextlens-control` is a recorded forward-looking target repo or strictly a future-feature concept.
