---
feature: lens-dev-new-codebase-techplan
doc_type: epics
status: approved
goal: "Break the techplan rewrite into three sequenced epics matching the three delivery slices from the expressplan sprint plan."
key_decisions:
  - Slice 1 (TK-1.1) is already complete as planning artifacts; Epic 1 is closed.
  - Slice 2 (TK-2.1) is the primary code slice: command surface in the target project.
  - Slice 3 (TK-3.1) owns absorbed shared utility delivery and parity hardening.
  - techplan-owned shared utilities are authoritative; lens-dev-new-codebase-expressplan and lens-dev-new-codebase-finalizeplan are expected to consume, not re-implement, the shared surfaces delivered by this feature.
  - Clean-room rule: no old-codebase skill prose reproduced; implementation derived from baseline PRD and architecture only.
open_questions:
  - Which exact discovery file registers `lens-techplan` in the target project? (Must resolve before TK-2.2 begins.)
  - Which focused test file owns prompt-start and wrapper-equivalence regressions? (Must resolve before TK-2.2 begins.)
depends_on:
  - business-plan.md
  - tech-plan.md
  - sprint-plan.md
  - finalizeplan-review.md
blocks:
  - lens-dev-new-codebase-expressplan (shared utility surfaces)
  - lens-dev-new-codebase-finalizeplan (shared utility surfaces)
updated_at: 2026-04-29T00:00:00Z
---

# Epic List — Techplan Command

## Epic Overview

| Epic | Story ID | Title | Status | Blocks |
| --- | --- | --- | --- | --- |
| Epic 1 | TK-1.1 | Express Path and Governance Alignment | ✅ Complete | — |
| Epic 2 | TK-2.1 | Target-Project Command Surface | 🔵 Ready for Dev | Epic 3 can start in parallel after Slice 2 is accepted |
| Epic 3 | TK-3.1 | Shared Utility Delivery, Parity Hardening, and Handoff | 🟡 Blocked until TK-2.1 is in-progress | Unblocks end-to-end execution for expressplan and finalizeplan command features |

---

## Epic 1 — Express Path and Governance Alignment (TK-1.1)

**Status:** ✅ Complete — all planning artifacts are staged and reviewed.

**Scope:**
- Rewrote `business-plan.md` to present the expressplan route without contradiction.
- Rewrote `tech-plan.md` to separate planning path from runtime command contract.
- Authored `sprint-plan.md` to complete the expressplan artifact set.
- Refreshed `expressplan-adversarial-review.md` against the completed packet (verdict: pass-with-warnings).
- Applied governance track alignment via the sanctioned `feature-yaml` flow (`track: express`, `phase: expressplan-complete`).
- Completed `finalizeplan-review.md` (verdict: pass-with-warnings) with all six findings responded to.

**Definition of Done (already met):**
- [ ] ✅ Expressplan artifact set staged and review-complete.
- [ ] ✅ Governance track set to `express` and phase to `expressplan-complete`.
- [ ] ✅ All F1–F6 findings from `finalizeplan-review.md` responded to.

**Dependencies:** None.
**Sequencing note:** Epic 1 is the planning gate. Epics 2 and 3 are implementation work.

---

## Epic 2 — Target-Project Command Surface (TK-2.1)

**Status:** 🔵 Ready for Dev.

**Scope:**
Deliver the missing `lens-techplan` command surface in `TargetProjects/lens-dev/new-codebase/lens.core.src`:

1. **Public stub** — `.github/prompts/lens-techplan.prompt.md` — runs shared prompt-start preflight before delegating to the release prompt; fails fast if preflight exits non-zero.
2. **Release prompt** — `_bmad/lens-work/prompts/lens-techplan.prompt.md` — thin redirect to `bmad-lens-techplan/SKILL.md`; no inline logic.
3. **Conductor skill** — `_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md` — conductor-only; resolves feature context, delegates architecture authoring through the Lens BMAD wrapper, enforces PRD reference rule; no direct governance writes.
4. **Discovery wiring** — Register `lens-techplan` on the chosen retained-command discovery surface. Specific file to be confirmed at story start (open question OQ-1).
5. **Focused test-harness foundation** — Identify and document the test file path for prompt-start and wrapper-equivalence regressions. Specific path to be confirmed at story start (open question OQ-2).

**Definition of Done:**
- [ ] `lens-techplan.prompt.md` stub exists in `.github/prompts/` and runs prompt-start preflight.
- [ ] `lens-techplan.prompt.md` release prompt exists in `_bmad/lens-work/prompts/` and redirects only.
- [ ] `bmad-lens-techplan/SKILL.md` exists, is conductor-only, enforces publish-before-author and PRD reference rule.
- [ ] `lens-techplan` is discoverable through the chosen retained-command surface.
- [ ] Focused regression test file path is identified and a placeholder or initial test exists.
- [ ] No old-codebase skill prose reproduced; implementation derived from baseline PRD and architecture only.

**Dependencies:**
- Shared preflight behavior already present in target project (confirmed in tech-plan current state).
- Absorbed shared utility surfaces (Epic 3) are not required for Slice 2 acceptance, but Slice 2 must name how utility delivery will follow.

**Sequencing note:** Slice 2 can land and be merged independently of Slice 3. Epic 3 starts after Epic 2 is in-progress; both can overlap.

**F3 sequencing note:** The shared utility surfaces absorbed into Slice 3 (publish hook, BMAD wrapper, adversarial review gate, constitution loading) are authoritative for this domain. `lens-dev-new-codebase-expressplan` and `lens-dev-new-codebase-finalizeplan` features must consume these surfaces when they land — not re-implement them.

---

## Epic 3 — Shared Utility Delivery, Parity Hardening, and Handoff (TK-3.1)

**Status:** 🟡 Starts after TK-2.1 is in-progress.

**Scope:**
Land the four absorbed shared utility surfaces in the target project and add focused parity regression coverage:

1. **Publish-before-author entry hook** — `bmad-lens-git-orchestration` publish hook for reviewed `businessplan` artifacts; runs before architecture authoring begins.
2. **BMAD wrapper routing** — `bmad-lens-bmad-skill` wrapper for delegated architecture authoring; enables conductor-only delegation from `bmad-lens-techplan`.
3. **Adversarial review gate** — `bmad-lens-adversarial-review` gate integration; enables lifecycle review before progression.
4. **Constitution loading** — Constitution resolution path for gating and context loading.
5. **Parity regression checks** — Named regression list:
   - Prompt-start regression: stub runs preflight, stops on failure.
   - Publish-before-author regression: reviewed predecessor artifacts publish before authoring.
   - PRD reference regression: architecture output explicitly references authoritative PRD.
   - Wrapper-equivalence regression: delegation stays routed through Lens BMAD wrapper.
   - Discovery regression: command is exposed through chosen retained discovery surface.
   - Governance-write audit: `techplan` never writes governance files directly.

**Abandon condition:** If all four shared utility surfaces are delivered by another feature before Slice 3 completes, this feature's implementation scope reduces to command surface only (Epic 2). Governance gates remain unaffected.

**Definition of Done:**
- [ ] Publish-before-author hook is present and callable from `bmad-lens-techplan`.
- [ ] BMAD wrapper routing enables delegation without inline implementation.
- [ ] Adversarial review gate is reachable from the `techplan` phase transition.
- [ ] Constitution loading resolves correctly for this domain/service.
- [ ] All six named parity regressions have a test case or explicit coverage acknowledgment.
- [ ] Clean-room validation confirmed: "No old-codebase skill prose reproduced; implementation derived from baseline PRD and architecture only."
- [ ] `dev-complete` milestone entry criteria satisfied per constitution gate (`gate_mode: informational`).

**Dependencies:** Epic 2 (TK-2.1) must be in-progress before Epic 3 begins.
**Blocks:** End-to-end `techplan` execution is incomplete until Epic 3 lands. `lens-dev-new-codebase-expressplan` and `lens-dev-new-codebase-finalizeplan` are blocked from consuming these shared surfaces until they are landed.
