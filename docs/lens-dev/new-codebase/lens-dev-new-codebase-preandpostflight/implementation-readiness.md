---
feature: lens-dev-new-codebase-preandpostflight
doc_type: implementation-readiness
status: approved
goal: "Assess readiness to begin implementation of the express-track preflight lifecycle redesign in lens.core.src and bind the settled failure taxonomy into the FinalizePlan handoff."
key_decisions:
  - This readiness assessment is scoped to the express-track FinalizePlan bundle and keeps that express-path boundary explicit.
  - Implementation starts in TargetProjects/lens-dev/new-codebase/lens.core.src against the live bmad-lens-preflight and bmad-lens-git-orchestration surfaces named in the story set.
  - Hard-stop, warning, and post-request reconciliation behavior is inherited directly from PF-2.2 and PF-2.3 and is not reopened here.
  - The missing full request-lifecycle entrypoint and focused preflight test module remain execution gates inside the approved stories, not planning blockers.
  - Story-file generation must preserve the same failure taxonomy, live target-surface notes, and git-orchestration routing constraints.
open_questions: []
depends_on:
  - business-plan.md
  - tech-plan.md
  - sprint-plan.md
  - expressplan-adversarial-review.md
  - finalizeplan-review.md
  - epics.md
  - stories.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-preandpostflight/feature.yaml
blocks: []
updated_at: 2026-05-04T00:00:00Z
---

# Implementation Readiness - PreAndPostFlight

## Readiness Assessment

### Overall Verdict: READY WITH CONDITIONS

This is the express-track FinalizePlan readiness gate for
`lens-dev-new-codebase-preandpostflight`. The reviewed express packet is coherent, the
downstream epic and story set is present, `feature.yaml` points implementation at
`TargetProjects/lens-dev/new-codebase/lens.core.src`, and the live `bmad-lens-*`
target surfaces needed to begin dev already exist.

The remaining work is execution-scoped, not planning-scoped. The bundle still needs
`sprint-status.yaml` and story-file generation, and implementation still has to land the
full request-lifecycle entrypoint plus focused preflight tests. Those items are already
allocated to PF-1.2, PF-1.3, PF-3.1, and PF-3.2, so they do not require reopening the
express-path policy packet.

---

## Main Gate Structure

| Gate | Status | Evidence | Blocks implementation start? |
| --- | --- | --- | --- |
| Express-track packet coherence | READY | Business, tech, and sprint plans align with the recorded express review responses and the FinalizePlan carry-forward decisions. | No |
| Target repo anchor | READY | `feature.yaml.target_repos` points to `TargetProjects/lens-dev/new-codebase/lens.core.src` on `develop`. | No |
| Live target surfaces | READY WITH EXECUTION FOLLOW-THROUGH | `bmad-lens-preflight` exists with `SKILL.md` and `scripts/light-preflight.py`; `bmad-lens-git-orchestration` exists with `scripts/git-orchestration-ops.py` and tests. | No |
| Failure taxonomy lock | READY | PF-2.2 and PF-2.3 acceptance criteria make hard-stop, warning, and post-request reconciliation behavior explicit. | No |
| FinalizePlan bundle follow-through | PENDING | `sprint-status.yaml` and story files still need to be generated in the feature docs path. | No for code start; yes for `finalizeplan-complete` |

---

## Bound Failure Taxonomy

FinalizePlan review item F2 required this bundle to make warning versus hard-stop
behavior explicit without reopening policy. The authoritative taxonomy is the one already
settled in PF-2.2 and PF-2.3 and must be copied forward unchanged.

| Outcome class | Required behavior | Included cases |
| --- | --- | --- |
| Hard-stop | Stop before mutable request work begins. | Interrupted git state; detached or wrong branch when mutation is required; auth or permission failure for required mutable sync; unresolved merge or rebase conflict; policy-blocked sync; missing required repo context. |
| Warning / continue | Continue the request and record the warning in the existing preflight log stream. | Governance freshness on read-only requests; optional publish lag; other non-required freshness gaps that do not block safe execution. |
| Post-request reconciliation required | User-visible work may complete, but sync cannot be reported as a clean success. | Auth or push failure; branch-policy failure; reconcile conflict; publish CLI failure; other post-request publication failures after touched repo work completes. |

This readiness artifact closes the remaining planning ambiguity by treating the taxonomy
above as fixed. Story files and implementation tests must inherit it verbatim.

---

## Implementation Surface Readiness

Implementation can begin in `TargetProjects/lens-dev/new-codebase/lens.core.src`
against the live `bmad-lens-*` surfaces below.

| Surface | Current state | Readiness implication |
| --- | --- | --- |
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/SKILL.md` | Present | The prompt-start contract already has a live skill anchor in the target repo. |
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/scripts/light-preflight.py` | Present | The cheap gate can remain frozen while heavier request-lifecycle behavior is added behind a fuller entrypoint. |
| Full request-lifecycle entrypoint under `.../bmad-lens-preflight/scripts/` | Not yet present | PF-1.1 and PF-1.2 must reserve and then implement the file before cadence work broadens. |
| Focused preflight test module under `.../bmad-lens-preflight/scripts/tests/` | Not yet present | PF-3.1 must create this surface before merge; absence is a dev execution gate, not a planning blocker. |
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-git-orchestration/scripts/git-orchestration-ops.py` | Present | Control and governance writes can route through the shared orchestration surface instead of ad hoc git logic. |
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-git-orchestration/scripts/tests/` | Present | Mutation-path assertions already have a live shared test home. |

---

## Carry-Forward Gates

### Required before FinalizePlan is bundle-complete

- Generate `sprint-status.yaml` in this feature docs folder.
- Generate story files for PF-1.1 through PF-3.2.

### Required assumptions for story-file generation

- Story files must use the live target repo paths under
  `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/`
  and `.../bmad-lens-git-orchestration/`, not the older predecessor wording that referred
  to `skills/lens-preflight`.
- Story files must copy the hard-stop, warning, and post-request reconciliation taxonomy
  from this readiness artifact and `stories.md` without reopening the policy decision.
- PF-1.1 or the first generated story file that owns naming must record the chosen full
  request-lifecycle entrypoint filename under `bmad-lens-preflight/scripts/` before
  PF-1.2 implementation expands beyond the frozen prompt-start gate.
- Any control or governance repo mutation must route through
  `bmad-lens-git-orchestration/scripts/git-orchestration-ops.py` or the shared helpers it
  owns; story files must not authorize ad hoc git write sequences.

---

## Prerequisites Checklist

- [x] `business-plan.md` reviewed and aligned to the accepted express responses
- [x] `tech-plan.md` reviewed and aligned to the accepted express responses
- [x] `sprint-plan.md` sequences cadence split, mutable sync policy, and validation hardening
- [x] `expressplan-adversarial-review.md` records responses with `pass-with-warnings`
- [x] `finalizeplan-review.md` records Step 3 carry-forward guidance
- [x] `epics.md` generated for PF-1.1 through PF-3.1 scope
- [x] `stories.md` generated with explicit taxonomy and target-surface notes
- [x] `feature.yaml` registers `TargetProjects/lens-dev/new-codebase/lens.core.src` as the implementation target
- [x] Live `bmad-lens-preflight` and `bmad-lens-git-orchestration` surfaces are present in the target repo
- [ ] `sprint-status.yaml` generated
- [ ] Story files generated