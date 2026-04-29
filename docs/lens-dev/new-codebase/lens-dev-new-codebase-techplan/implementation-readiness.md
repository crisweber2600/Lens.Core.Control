---
feature: lens-dev-new-codebase-techplan
doc_type: implementation-readiness
status: approved
goal: "Assess the readiness of lens-dev-new-codebase-techplan for implementation and identify risks before dev begins."
key_decisions:
  - Feature is ready for Epic 2 (command surface) immediately.
  - Epic 3 (shared utilities) starts after TK-2.1 is in-progress.
  - Two open questions must be resolved at the start of TK-2.1 before any code is written.
depends_on:
  - epics.md
  - stories.md
  - finalizeplan-review.md
blocks: []
updated_at: 2026-04-29T00:00:00Z
---

# Implementation Readiness Assessment — Techplan Command

## Readiness Verdict

**Overall:** ✅ Ready for Dev — Epic 2 may begin. Epic 3 starts after TK-2.1 pre-assessment is complete.

---

## Readiness Assessment

### Planning Artifacts

| Artifact | Present | Quality | Notes |
| --- | --- | --- | --- |
| `business-plan.md` | ✅ | Coherent | Expressplan path clearly separated from runtime contract |
| `tech-plan.md` | ✅ | Coherent | Planning route vs. implementation target well-defined |
| `sprint-plan.md` | ✅ | Coherent | Three slices, story IDs pre-assigned |
| `expressplan-adversarial-review.md` | ✅ | Pass-with-warnings | All four findings responded to |
| `finalizeplan-review.md` | ✅ | Pass-with-warnings | All six findings (F1–F6) responded to |
| `epics.md` | ✅ | Approved | Three epics, sequencing and abandon conditions defined |
| `stories.md` | ✅ | Approved | Eight stories (TK-1.1 complete; TK-2.1–TK-2.5, TK-3.1–TK-3.2 ready/blocked) |

### Infrastructure

| Check | Status | Notes |
| --- | --- | --- |
| `lens-dev-new-codebase-techplan` base branch | ✅ Exists on remote | Created via F1 resolution; PR #25 merged |
| `feature.yaml` phase | ✅ `expressplan-complete` | Aligned via F2 resolution |
| Governance `feature-index.yaml` | ✅ Updated | `track: express`, `status: expressplan-complete` |
| Shared preflight present in target project | ✅ Confirmed | Per tech-plan current state |
| Target project structure understood | ✅ Partially | Discovery file and test path are open questions (OQ-1, OQ-2) |

### Open Questions (Must Resolve Before TK-2.2 Begins)

| OQ | Question | Resolution Target | Risk If Unresolved |
| --- | --- | --- | --- |
| OQ-1 | Which exact discovery file registers `lens-techplan`? | TK-2.1 assessment | TK-2.4 cannot proceed; discovery wiring undefined |
| OQ-2 | Which focused test file owns prompt-start and wrapper-equivalence regressions? | TK-2.1 assessment | TK-2.5 cannot proceed; test harness path undefined |

Both open questions are addressed in Story TK-2.1 as a one-story pre-assessment before any code is written. This is the sequencing gate that bounds TK-2.2 through TK-2.5.

---

## Risk Register

| ID | Severity | Risk | Mitigation |
| --- | --- | --- | --- |
| R1 | High | Shared utility surfaces may not arrive in time for end-to-end execution | Epic 2 (command surface) can land independently; Epic 3 delivers the shared surfaces. TK-2.3 ACs explicitly document the dependency so it's visible, not hidden. |
| R2 | Medium | Sibling features (`expressplan`, `finalizeplan`) may race to implement the same shared surfaces | F3 mitigation: techplan-owned surfaces are authoritative. Sibling features are expected to consume, not re-implement. Sequencing note is embedded in `epics.md`. |
| R3 | Medium | `bmad-lens-techplan` conductor skill may inadvertently couple to the publish hook before it exists (TK-3.1) | TK-2.3 AC explicitly allows documenting the dependency rather than creating a workaround. The hook is referenced, not duplicated. |
| R4 | Low | Clean-room boundary may be violated if an implementer references old-codebase skill prose | TK-3.2 AC includes explicit clean-room validation as a code-review checklist item. All story DoDs carry the clean-room reminder. |
| R5 | Low | Abandon condition (all four shared surfaces delivered by another feature) | Defined in Epic 3 and TK-3.1: if another feature delivers all four surfaces first, TK-3.1 scope reduces to documenting the consumption pattern. Governance gates unchanged. |

---

## Prerequisites Checklist

Before TK-2.1 begins:
- [x] Expressplan artifact set is complete and review-complete.
- [x] `feature.yaml` phase is `expressplan-complete`.
- [x] `lens-dev-new-codebase-techplan` base branch exists on remote.
- [x] Planning PR #25 merged.
- [x] `epics.md`, `stories.md`, `implementation-readiness.md`, `sprint-status.yaml`, and story files committed to base branch.

Before TK-2.2 begins:
- [ ] OQ-1 resolved (discovery file identified).
- [ ] OQ-2 resolved (test file path identified).
- [ ] TK-2.1 assessment note committed.

Before TK-3.1 begins:
- [ ] TK-2.1 is in-progress.
- [ ] No other feature has already landed all four shared utility surfaces (check `lens-dev/new-codebase` governance docs).

---

## Constitution Compliance Summary

| Rule | Status |
| --- | --- |
| Express track permitted (domain + service constitution) | ✅ |
| Adversarial review gate completed | ✅ (`finalizeplan-review.md` pass-with-warnings) |
| Stories and epics present for dev phase | ✅ |
| Governance gate mode | `informational` — governance conditions are advisory, not hard blockers |
| Clean-room rule | Embedded in all code story DoDs |
| Given/When/Then format for behavior-critical ACs | Applied where implementation behavior is specified |
