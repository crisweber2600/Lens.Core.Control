---
feature: lens-dev-new-codebase-new-service
doc_type: implementation-readiness
status: approved
goal: "Assess readiness for /dev handoff on the new-service retained command"
key_decisions:
  - Feature is dev-ready after NS-13 handoff notes are verified by NS-12
  - Constitution gate mode is informational for the express track — no hard block on direct lens.core.src edits
open_questions: []
depends_on:
  - epics.md
  - stories.md
  - finalizeplan-review.md
blocks: []
updated_at: 2026-04-27T16:00:00Z
---

# Implementation Readiness — New Service Command

## Readiness Assessment

**Overall verdict: READY FOR DEV**

The `lens-dev-new-codebase-new-service` feature has completed the express track planning phases (PrePlan→ExpressPlan→FinalizePlan) and all FinalizePlan review findings have been addressed. The planning set is coherent, internally consistent, and scoped to a well-bounded problem.

| Readiness Area | Status | Notes |
|----------------|--------|-------|
| Business case | ✅ Ready | `business-plan.md` approved — retained command parity, clean-room scope |
| Technical design | ✅ Ready | `tech-plan.md` approved — 5 ADRs, explicit API contracts, no schema changes |
| Planning review | ✅ Ready | `finalizeplan-review.md` verdict: pass-with-warnings; all H1–GI3 findings addressed |
| Epics and stories | ✅ Ready | 4 epics, 13 stories, 29 points — sequenced with explicit gates |
| Sprint organisation | ✅ Ready | `sprint-status.yaml` generated with per-story estimates |
| Story files | ✅ Ready | All 13 story files created in `stories/` subfolder |
| Dependency tracking | ✅ Ready | `new-feature` dependency recorded in both feature.yaml files |
| Constitution gate | ✅ Informational | Express track — `gate_mode: informational`; direct `lens.core.src` edits accepted for this cycle |
| Target repo setup | 🔲 Pre-dev required | `/discover` run with `domain: lens-dev, service: new-codebase` before Sprint 1; target repos cloned |

**Pre-dev setup required** (as documented in `sprint-plan.md` Definition of Done):
- Associated target repos (`lens.core.src`, `lens-governance`) cloned into `TargetProjects/lens-dev/new-codebase/` and `TargetProjects/lens/` respectively
- `/discover` run with `domain: lens-dev, service: new-codebase` before Sprint 1 begins

---

## Risk Assessment

| ID | Risk | Severity | Likelihood | Mitigation | Owner |
|----|------|----------|------------|------------|-------|
| R1 | `create-service` accidentally writes feature lifecycle state | High | Low | NS-2 boundary tests are a hard regression gate; inspect governance tree after each run | Dev |
| R2 | Auto-git path commits scaffold files unintentionally | Medium | Low | Keep governance git and workspace scaffold commands separated in output; NS-7 idempotency test required | Dev |
| R3 | Context writer change regresses `new-domain` context tests | Medium | Medium | NS-6 explicit regression check; full init-feature suite in NS-12 | Dev |
| R4 | NS-10 discovery metadata ships without script support | Medium | Medium | NS-10 formally depends on NS-9; enforce sequencing | Dev |
| R5 | ADR-3 delegation boundary violated (parallel domain mutation path) | High | Low | ADR-3 boundary documented in NS-4 before implementation; NS-13 handoff must name the exact helper functions called | Dev |
| R6 | NS-13 handoff notes incomplete → `/dev` agent re-reads full planning set | Low | Low | NS-12 validates NS-13 existence before marking itself complete | Dev |
| R7 | `lens-dev-new-codebase-new-feature` begins before `new-service` reaches `dev-ready` | Medium | Low | Dependency recorded in both feature.yaml files (`depended_by`/`depends_on`); governance will surface sequencing risk | Planning |
| R8 | Sprint 3 runs long; NS-10 (discovery metadata) dropped | Low | Medium | NS-10 has explicit acceptance gate per John PM party-mode concern in FinalizePlan review | PM |

---

## Sequencing Constraints

```
NS-1 → NS-2, NS-3            (test lock before any implementation)
NS-4 → NS-5                  (builders before parser route)
NS-5 → NS-6, NS-7            (parser before context writer and governance git)
NS-8 → NS-9 → NS-10          (skill doc before prompt before discovery metadata)
NS-7, NS-10 → NS-11          (all impl done before focused regression)
NS-11 → NS-12 → NS-13        (focused pass → full regression → handoff)
```

---

## Planning Artifact Inventory

| Artifact | File | Status |
|----------|------|--------|
| Business plan | `business-plan.md` | Approved |
| Tech plan | `tech-plan.md` | Approved |
| Sprint plan | `sprint-plan.md` | Approved |
| ExpressPlan review | `expressplan-review.md` | Approved (pass-with-warnings) |
| FinalizePlan review | `finalizeplan-review.md` | Approved (pass-with-warnings) |
| Epics | `epics.md` | Created |
| Stories | `stories.md` | Created |
| Sprint status | `sprint-status.yaml` | Created |
| Story files | `stories/ns-*.md` (13 files) | Created |

---

## Constitution Gate Result

**Track:** express  
**Constitution gate mode:** informational  
**Verdict:** No hard block. Direct edits to `lens.core.src` for NS-4–NS-7 are accepted per `gate_mode: informational`. The NS-13 handoff notes must record this deviation explicitly for the `/dev` agent.

**Permitted tracks verified:** `express` added to `new-codebase` service constitution (GI-1 fix applied during FinalizePlan review).

---

## Next Action

`/dev` — implementation may begin after:
1. Target repos cloned into `TargetProjects/`
2. `/discover` run for `domain: lens-dev, service: new-codebase`
3. NS-1 story file read by the dev agent before writing any code
