---
feature: lens-dev-new-codebase-complete
doc_type: implementation-readiness
status: draft
goal: "Confirm the complete feature has enough planning depth to move into implementation while keeping known archive-risk seams explicit"
key_decisions:
  - Treat this as implementation-ready for code work; CP-7 summary naming audit is a gate on CP-5 acceptance, not a follow-up item.
  - Branch cleanup is out of scope for the archive script — explicitly documented so implementation scope stays bounded.
  - Carry confirmation-gate and archive-boundary concerns as explicit warnings rather than hidden assumptions.
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T22:55:00Z
---

# Implementation Readiness - Complete Command

## Verdict

**Pass with warnings**

The feature has enough planning depth to move into implementation. The public command surface, technical layering, story decomposition, regression targets, and governance sensing findings are all explicit. The remaining concerns are implementation-shaping risks, not planning gaps.

## Evidence Matrix

| Area | Status | Evidence |
|---|---|---|
| Business intent | Pass | business-plan.md defines retained value, stakeholders, scope, and success criteria with 10 measurable outcomes |
| Technical design | Pass | tech-plan.md defines orchestration layers, 5 ADRs, script subcommand contract, and test strategy referencing `test-complete-ops.py` |
| Execution sequencing | Pass | sprint-plan.md breaks work into 4 sprints; epics.md organizes into 4 bounded epics |
| Story readiness | Pass | stories.md defines 13 stories with observable acceptance criteria and explicit done conditions |
| Express review gate | Pass with warnings | expressplan-adversarial-review.md: overall pass-with-warnings, 0 critical / 2 high / 3 medium-low findings, all responded to |
| FinalizePlan review gate | Pass with warnings | finalizeplan-review.md: overall pass-with-warnings, 0 critical / 1 high / 2 medium / 1 low findings, all responded to with selected responses |
| Governance sensing | Pass | Sensing identified 2 archived express features (`new-service`, `switch`) as behavioral reference; no cross-feature conflicts found |

## What Is Ready

- The retained command contract is explicit: prompt stub → release prompt → `bmad-lens-complete` → `complete-ops.py`.
- The archive mutation boundary is named and bounded: `feature.yaml`, `feature-index.yaml`, `summary.md` only.
- The split responsibility model (skill orchestrates, script mutates) is documented and encoded in ADR 2.
- The three script subcommands (`check-preconditions`, `finalize`, `archive-status`) are defined with clear semantics.
- The implementation sequence is small enough to validate incrementally (test-first, contract-first).
- Two prior archived express features (`new-service`, `switch`) provide behavioral reference for archive output shape.

## Warnings To Carry Forward

1. **Summary naming audit (CP-7 gate)**
   Discovery prose still contains `final-summary.md` references. CP-7 must audit and remove these before CP-5 (atomic archive writes) can be signed off. This is an acceptance gate, not cleanup.

2. **Confirmation gate needs explicit test artifact**
   CP-10's done condition is named (`test-complete-ops.py::test_finalize_requires_confirmation`), but the test does not yet exist. This is the single highest-trust boundary in the entire closure workflow.

3. **Branch cleanup is explicitly out of scope**
   The archive script contract covers only governance file writes. Branch cleanup is post-archive operational follow-up. Any story that interprets branch cleanup as in-scope is out of bounds.

4. **Retrospective and document-project delegation**
   The skill delegates to `bmad-lens-retrospective` and `bmad-lens-document-project`. These are assumed available and behaviorally stable. CP-12's adjacent command checks should include a smoke check of these delegation surfaces.

5. **Operational PR steps were not executed in this planning session**
   Staged docs only. Branch validation, PR creation, and governance publication remain script-backed implementation behaviors.

## Ready-For-Implementation Checklist

- [x] Business goals documented
- [x] Technical architecture documented (5 ADRs)
- [x] Stories broken into 13 implementation slices with explicit acceptance criteria
- [x] Regression targets named (`test-complete-ops.py`, `test_finalize_requires_confirmation`)
- [x] Archive mutation boundary documented and bounded
- [x] Branch cleanup explicitly excluded from script contract
- [x] CP-7 summary naming audit designated as acceptance gate
- [ ] Focused command-level tests implemented (CP-1 through CP-3)
- [ ] `test-complete-ops.py::test_finalize_requires_confirmation` implemented (CP-10)
