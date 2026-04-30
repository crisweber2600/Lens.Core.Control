---
feature: lens-dev-new-codebase-finalizeplan
doc_type: epics
status: draft
goal: "Decompose FinalizePlan + ExpressPlan + QuickPlan conductor delivery into reviewable epics aligned with the three sprint slices."
key_decisions:
  - Organize work around Foundation Validation, Discovery and Regressions, and Handoff Readiness epics.
  - Keep regressions close to the highest-risk predecessor-gate and constitution-permission compatibility points.
open_questions: []
depends_on:
  - lens-dev-new-codebase-expressplan
blocks: []
updated_at: '2026-04-30T00:00:00Z'
---

# Epics — FinalizePlan, ExpressPlan, QuickPlan Conductors

## Epic 1 — Foundation Validation

**Goal:** Confirm the conductor infrastructure created in the previous session is complete,
correct, and committed to the target source repo. Resolve the H2 predecessor gate
inconsistency flagged in the adversarial review.

**Scope:**
- Read and validate all three conductor SKILL.md files:
  - `bmad-lens-finalizeplan/SKILL.md`: three-step contract ordering, predecessor gate
    accepts both `techplan` and `expressplan-complete`, governance-write boundary, bundle
    delegation ordering
  - `bmad-lens-expressplan/SKILL.md`: express-only eligibility gate, QuickPlan delegation
    via `bmad-lens-bmad-skill --skill bmad-lens-quickplan`, adversarial review invocation
    `--phase expressplan --source phase-complete`, party-mode enforcement
  - `bmad-lens-quickplan/SKILL.md`: business→tech→sprint pipeline, internal-only
    designation (no public stub)
- Validate `.github/prompts/lens-finalizeplan.prompt.md` and `.github/prompts/lens-expressplan.prompt.md`
  follow the shared prompt-start preflight pattern.
- Validate both thin redirects in `_bmad/lens-work/prompts/`.
- Confirm `_bmad/lens-work/module.yaml` lists both prompt entries with no duplicates.
- Confirm `_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md` registers QuickPlan as an internal wrapper target.
- Confirm express-track constitution permission is set for `lens-dev/new-codebase`.
- Confirm `publish-to-governance` CLI handles hyphenated artifact names for express-track.
- Run all 34+ tests — confirm no regressions.
- Commit all untracked infrastructure files to `lens.core.src` `develop` branch.

**Exit Criteria:**
- All validation checklist items pass.
- H2 (predecessor gate) confirmed or remediation sub-story resolved.
- Constitution permission confirmed.
- Tests passing (≥ 34).
- All infrastructure files committed to target source repo.

---

## Epic 2 — Discovery and Regressions

**Goal:** Register both commands in the new-codebase discovery surface and document test
coverage gaps.

**Scope:**
- Identify the discovery/help registration file used by retained commands in the new-codebase
  target project (e.g., `module-help.csv` or equivalent).
- Register `lens-finalizeplan` and `lens-expressplan` following the same pattern as other
  retained commands.
- Document known test coverage gaps:
  - FinalizePlan step-2 (PR creation path) — integration level, not unit
  - FinalizePlan step-3 (bundle + final PR path) — integration level, not unit
  - Constitution check in ExpressPlan — requires mock constitution test
  - End-to-end command activation (behavior-level) — out of scope, tracked item
- Confirm shared skill prerequisites (`bmad-lens-adversarial-review`, `bmad-lens-bmad-skill`,
  `bmad-lens-git-orchestration`, `validate-phase-artifacts.py`) exist in target project.
- Add notation in business plan out-of-scope section: shared utility extraction (G3 from
  baseline) is a separate feature.
- Update `feature.yaml.target_repos` to include `lens.core.src`.

**Exit Criteria:**
- Both commands registered in discovery surface.
- Test gap report documented.
- Prerequisite skills confirmed or gaps flagged.
- `feature.yaml.target_repos` updated.

---

## Epic 3 — Handoff Readiness

**Goal:** Confirm the feature is dev-ready and the final PR can be merged.

**Scope:**
- Confirm no open fail-level findings in `expressplan-adversarial-review.md` or
  `finalizeplan-review.md`.
- Confirm Epic 1 H2 remediation is resolved.
- Confirm all sprint plan exit criteria for Epics 1 and 2 are met.
- Open final PR: `lens-dev-new-codebase-finalizeplan` → `main`.
- Signal `/dev` handoff.

**Exit Criteria:**
- Final PR open.
- `feature.yaml` phase = `finalizeplan-complete`.
- `/dev` signalled.
