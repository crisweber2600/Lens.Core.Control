---
feature: lens-dev-new-codebase-finalizeplan
doc_type: business-plan
status: draft
goal: "Deliver the FinalizePlan conductor command for the lens-work new codebase — the post-TechPlan planning consolidation phase that gates governance bundle generation and opens the final planning PR."
key_decisions:
  - FinalizePlan is a three-step conductor: review-and-push → plan-PR-readiness → downstream-bundle + final PR
  - All epics/stories/readiness/sprint work routes through bmad-lens-bmad-skill (Lens wrapper), never directly
  - Governance writes are publish-CLI-only; no direct file creation inside FinalizePlan
  - The FinalizePlan conductor also includes the ExpressPlan and internal QuickPlan conductors because the express track is the primary delivery path
  - The skills were created clean-room in the new codebase under _bmad/lens-work/skills/
open_questions: []
depends_on: [product-brief, research, brainstorm, prd]
blocks: []
updated_at: '2026-04-30T00:00:00Z'
---

# Business Plan — FinalizePlan Command (+ ExpressPlan, QuickPlan)

## 1. Problem Statement

The new-codebase lens-work rewrite targets 17 published commands. FinalizePlan
(`/lens-finalizeplan`) is the planning consolidation gate that closes the TechPlan
milestone, runs the final adversarial review across all staged planning artifacts,
creates the planning PR from `{featureId}-plan` to `{featureId}`, generates the
downstream planning bundle (epics, stories, implementation readiness, sprint plan,
story files), and opens the final `{featureId}` → `main` PR that signals `/dev`
readiness.

Without this command the new codebase cannot complete any feature's planning lifecycle.
The express track, which skips the three-phase PrePlan→BusinessPlan→TechPlan sequence
and condenses all planning into a single QuickPlan session, also requires its own
conductor (`/lens-expressplan`) and the internal QuickPlan delegate
(`bmad-lens-quickplan`). Neither of these was present in the new codebase.

## 2. Target Users

| User | Pain Without FinalizePlan | Benefit |
|------|--------------------------|---------|
| **Feature leads on full track** | Cannot close TechPlan milestone; cannot get planning PR; cannot start dev | Unblocked end-to-end planning cycle with governance cross-check |
| **Feature leads on express track** | Cannot run accelerated single-session planning; cannot advance to finalizeplan | Full express path: one QuickPlan session → adversarial review → governance bundle |
| **Lens module maintainers** | No conductor skill to test or extend; express track is a dead path in new codebase | Clean-room implementation with test coverage; express track fully functional |

## 3. Business Goals

### BG1 — Complete the Express Track
The express track (`phases: [expressplan, finalizeplan]`) is the fastest supported
path from feature registration to dev-ready. Delivering ExpressPlan and FinalizePlan
together completes the entire express track in the new codebase.

### BG2 — Unblock New-Codebase Feature Lifecycle
All features tracked in governance that use the full or express track need FinalizePlan
to reach `finalizeplan-complete`. Without it the new codebase is not lifecycle-complete
and cannot replace the old codebase.

### BG3 — Enforce Governance Boundaries
FinalizePlan enforces the publish-CLI-only governance write boundary. The conductor
pattern documented in this feature is the reference implementation for how all
post-TechPlan planning work interacts with the governance repo.

## 4. Success Criteria

| Criterion | Measurable Signal |
|-----------|------------------|
| FinalizePlan command activates | `/lens-finalizeplan` prompt stub exists and redirects correctly to SKILL.md |
| ExpressPlan command activates | `/lens-expressplan` prompt stub exists and redirects correctly to SKILL.md |
| Express track blocks non-express features | Validation gate in ExpressPlan SKILL.md confirmed by tests |
| QuickPlan delegation chain works | bmad-lens-bmad-skill → bmad-lens-quickplan route confirmed |
| Adversarial review gate is hard-stop | `fail` verdict blocks expressplan and finalizeplan phase advance |
| Governance writes go through publish CLI only | No direct file creation in conductor skills |
| module.yaml registrations correct | Both prompt stubs listed in module.yaml prompts section |
| Test coverage ≥ 34 passing | All existing tests pass; no regressions introduced |

## 5. Scope

**In scope:**
- `bmad-lens-finalizeplan/SKILL.md` — FinalizePlan conductor
- `bmad-lens-expressplan/SKILL.md` — ExpressPlan conductor (express track only)
- `bmad-lens-quickplan/SKILL.md` — internal QuickPlan delegate (not a published command)
- `.github/prompts/lens-finalizeplan.prompt.md` — public prompt stub
- `.github/prompts/lens-expressplan.prompt.md` — public prompt stub
- `_bmad/lens-work/prompts/lens-finalizeplan.prompt.md` — thin redirect
- `_bmad/lens-work/prompts/lens-expressplan.prompt.md` — thin redirect
- `_bmad/lens-work/module.yaml` — registration of both prompt stubs
- `bmad-lens-bmad-skill/SKILL.md` — registration of QuickPlan as internal wrapper target
- Test files for both conductor skills

**Out of scope:**
- Implementing the adversarial review skill (delegates to existing `bmad-lens-adversarial-review`)
- Implementing the epics/stories/sprint/readiness skills (delegates via `bmad-lens-bmad-skill`)
- Governance publish CLI changes
- Any behavior changes to the old codebase

## 6. Dependencies

- `lens-dev-new-codebase-baseline` (split source — product-brief, research, brainstorm, PRD)
- `bmad-lens-adversarial-review` must be present in the target source repo
- `bmad-lens-bmad-skill` must be present in the target source repo
- `bmad-lens-git-orchestration` must be present in the target source repo
- `validate-phase-artifacts.py` must be present in the target source repo scripts

## 7. Risks

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|-----------|
| express track constitution permission not set | Medium | FinalizePlan blocks | Document in feature.yaml; add constitution check in tests |
| publish-before-author ordering violated | Low | Governance corruption | Conductor delegates publish via CLI; no direct writes |
| Review-ready fast path skips required review | Low | Quality gate bypass | Tests validate fast path only skips QuickPlan re-entry, not review |
