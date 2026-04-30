---
feature: lens-dev-new-codebase-finalizeplan
doc_type: sprint-plan
status: draft
goal: "Organise FinalizePlan, ExpressPlan, and QuickPlan delivery into reviewable sprints."
key_decisions:
  - Three sprints: Foundation Validation → Discovery and Regressions → Handoff Readiness
  - Sprint 1 focuses on confirming implementation correctness (already done) and committing
  - Sprint 2 hardens test coverage and discovery surface registration
  - Sprint 3 closes the planning PR and signals dev handoff
open_questions: []
depends_on: [business-plan, tech-plan]
blocks: []
updated_at: '2026-04-30T00:00:00Z'
---

# Sprint Plan — FinalizePlan, ExpressPlan, QuickPlan Conductors

## Sprint 1 — Foundation Validation

**Goal:** Confirm the implementation created in the prior session is complete, correct,
and committed to the target source repo.

**Stories:**

| ID | Story | Points |
|----|-------|--------|
| S1.1 | Read and validate `bmad-lens-finalizeplan/SKILL.md` — confirm three-step contract, review-first ordering, bundle delegation, governance-write boundary | 2 |
| S1.2 | Read and validate `bmad-lens-expressplan/SKILL.md` — confirm express-only gate, QuickPlan delegation route, adversarial review invocation, party-mode enforcement | 2 |
| S1.3 | Read and validate `bmad-lens-quickplan/SKILL.md` — confirm business→tech→sprint pipeline, internal-only designation | 1 |
| S1.4 | Validate prompt stubs and thin redirects for both commands | 1 |
| S1.5 | Validate `module.yaml` registers both prompt entries correctly | 1 |
| S1.6 | Confirm `bmad-lens-bmad-skill` registers QuickPlan as internal wrapper target | 1 |
| S1.7 | Run all 34 tests — confirm no regressions | 2 |
| S1.8 | Commit all untracked infrastructure files to target source repo `develop` branch | 2 |

**Sprint Exit Criteria:**
- All 8 stories done
- Tests passing (≥ 34)
- All new files committed to target source repo

---

## Sprint 2 — Planning Artifacts and Regressions

**Goal:** Produce the formal planning documents for this feature and harden regression
expectations for both conductor skills.

**Stories:**

| ID | Story | Points |
|----|-------|--------|
| S2.1 | Create `business-plan.md` for `lens-dev-new-codebase-finalizeplan` | 2 |
| S2.2 | Create `tech-plan.md` for `lens-dev-new-codebase-finalizeplan` | 3 |
| S2.3 | Create `sprint-plan.md` for `lens-dev-new-codebase-finalizeplan` | 2 |
| S2.4 | Run ExpressPlan adversarial review across business-plan + tech-plan | 3 |
| S2.5 | Identify test coverage gaps for FinalizePlan step-2 (PR creation path) | 2 |
| S2.6 | Identify test coverage gaps for FinalizePlan step-3 (bundle + final PR path) | 2 |
| S2.7 | Document registration of `lens-finalizeplan` and `lens-expressplan` in the new-codebase discovery surface | 1 |

**Sprint Exit Criteria:**
- Planning docs created
- Adversarial review completed with pass or pass-with-warnings
- Test gap report documented

---

## Sprint 3 — FinalizePlan and Dev Handoff

**Goal:** Run FinalizePlan to produce epics, stories, implementation readiness, sprint
plan, and story files, then open the planning PR.

**Stories:**

| ID | Story | Points |
|----|-------|--------|
| S3.1 | Run FinalizePlan review across all staged planning artifacts | 2 |
| S3.2 | Generate epics for `lens-dev-new-codebase-finalizeplan` | 3 |
| S3.3 | Generate stories with acceptance criteria | 3 |
| S3.4 | Run implementation readiness check | 2 |
| S3.5 | Generate sprint-status.yaml | 2 |
| S3.6 | Create story files for dev-ready stories | 3 |
| S3.7 | Create planning PR: `lens-dev-new-codebase-finalizeplan-plan` → `lens-dev-new-codebase-finalizeplan` | 1 |
| S3.8 | Open final PR: `lens-dev-new-codebase-finalizeplan` → `main` | 1 |

**Sprint Exit Criteria:**
- Planning PR open
- Final PR open
- `feature.yaml` phase = `finalizeplan-complete`
- `/dev` signalled
