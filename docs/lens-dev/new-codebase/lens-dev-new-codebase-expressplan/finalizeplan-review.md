---
feature: lens-dev-new-codebase-expressplan
doc_type: finalizeplan-review
phase: finalizeplan
source: manual-rerun
verdict: pass-with-warnings
status: responses-recorded
critical_count: 0
high_count: 1
medium_count: 2
low_count: 0
carry_forward_blockers: []
updated_at: 2026-04-27T22:50:00Z
review_format: abc-choice-v1
---

# FinalizePlan Review - lens-dev-new-codebase-expressplan

**Source:** manual-rerun  
**Artifacts reviewed:** business-plan.md, tech-plan.md, sprint-plan.md, expressplan-adversarial-review.md, epics.md, stories.md, implementation-readiness.md, sprint-status.yaml, story files  
**Goal under review:** Move the expressplan feature from compressed planning into an implementation-ready bundle without duplicating FinalizePlan behavior or breaking write boundaries.

---

## H1 - PR Orchestration Exists Only as a Planning Contract Here

**Location:** FinalizePlan step-2 and step-3 expectations  
**Problem:** This staged docs-only session reconstructs the bundle artifacts but does not execute live plan-PR or final-PR orchestration. If implementation handoff ignores that distinction, reviewers may think the Git topology has already been proven.

- **A.** Treat live PR orchestration as an implementation story with focused contract tests and keep the planning docs explicit that it was not executed here.
- **B.** Mark the feature blocked until branch topology is exercised manually from this planning session.
- **C.** Remove PR topology from the expressplan feature scope entirely.
- **D.** Provide a custom response.
- **E.** Accept the ambiguity.

**Selected:** A  
**Why:** The planning docs should stay honest. The retained Git topology is still part of the feature contract, but proving it belongs to implementation and focused tests.

## M1 - ExpressPlan-To-FinalizePlan Reuse Needs an Executable Proof

**Location:** handoff boundary between `bmad-lens-expressplan` and `bmad-lens-finalizeplan`  
**Problem:** The staged epics and stories assume reuse of the FinalizePlan bundle. Without a focused proof, future work could still drift toward a duplicated downstream implementation.

- **A.** Add a focused regression that exercises expressplan and asserts FinalizePlan-owned bundle outputs.
- **B.** Rely on SKILL.md prose only.
- **C.** Split expressplan and finalizeplan into separate independently generated bundles.
- **D.** Provide a custom proof strategy.
- **E.** Accept the drift risk.

**Selected:** A  
**Why:** The handoff is one of the highest-value parity seams and needs executable coverage.

## M2 - Governance Publish Boundary Must Stay Script-Backed

**Location:** publish-before-author and governance mirror expectations  
**Problem:** The current planning docs intentionally stop at staged control-repo artifacts. A later implementation that starts patching governance docs directly would violate the current Lens authority rules.

- **A.** Keep governance publication behind `bmad-lens-git-orchestration publish-to-governance` and feature-state tooling.
- **B.** Allow direct governance doc writes for expressplan only.
- **C.** Delay governance publication until `/dev`.
- **D.** Provide a custom publication rule.
- **E.** Leave the boundary ambiguous.

**Selected:** A  
**Why:** The current repository rules are explicit: staged feature docs live in the control repo, governance is the mirrored destination.

---

## Blind-Spot Challenge

**Winston (Architect):** If the handoff to FinalizePlan is only implied, what stops a future refactor from regenerating epics and stories twice?

**Murat (Test Architect):** Which single regression would most quickly reveal that review `fail` no longer blocks the express path?

**John (PM):** If the docs say the feature is dev-ready but the Git topology was never exercised, what exact wording keeps that from becoming a release surprise?

### Direct Questions

1. Which focused test should be treated as the release gate for FinalizePlan reuse?
2. Do we want a separate implementation note that distinguishes staged planning completion from live PR execution?
3. Is any compatibility handling for older expressplan review filenames still required after implementation lands?