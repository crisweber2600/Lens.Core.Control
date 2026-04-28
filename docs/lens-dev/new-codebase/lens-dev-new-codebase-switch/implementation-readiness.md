---
feature: lens-dev-new-codebase-switch
doc_type: implementation-readiness
status: approved
goal: "Validate that switch command planning artifacts are complete and implementation can begin"
key_decisions:
  - All planning artifacts reviewed and approved
  - Carry-forward items from finalizeplan-review are scoped to dev story acceptance criteria, not blockers
  - Dev branch feature/switch-dev is provisioned and ready
open_questions: []
depends_on: [epics.md, stories.md, tech-plan.md, business-plan.md, sprint-plan.md, finalizeplan-review.md]
blocks: []
updated_at: 2026-04-27T00:00:00Z
---

# Implementation Readiness — Switch Command

## Verdict: READY TO IMPLEMENT ✅

All pre-implementation gates pass. Development can begin immediately on `feature/switch-dev` in `crisweber2600/lens.core.src`.

---

## Readiness Checklist

### Planning Completeness

| Artifact | Present | Status | Notes |
|---|---|---|---|
| `business-plan.md` | ✅ | Approved | 10 requirements SW-B1–SW-B10, all traced |
| `tech-plan.md` | ✅ | Approved | 3-layer architecture, full JSON contracts |
| `sprint-plan.md` | ✅ | Approved | 3 sprints, 12 stories |
| `epics.md` | ✅ | Approved | EP-1, EP-2, EP-3 scoped and sequenced |
| `stories.md` | ✅ | Approved | All 12 stories with acceptance criteria |
| `expressplan-adversarial-review.md` | ✅ | Reviewed | Carry-forwards addressed in stories |
| `finalizeplan-review.md` | ✅ | pass-with-warnings | 0 critical, 2 high, 4 medium, 2 low |

### Branch & Environment

| Check | Status | Detail |
|---|---|---|
| Control repo base branch | ✅ | `lens-dev-new-codebase-switch` exists, planning PR merged |
| Control repo plan branch | ✅ | `lens-dev-new-codebase-switch-plan` exists, bundle committed |
| Target repo | ✅ | `crisweber2600/lens.core.src` cloned at `TargetProjects/lens-dev/new-codebase/lens.core.src` |
| Dev working branch | ✅ | `feature/switch-dev` created and pushed |
| Governance feature.yaml | ✅ | Phase: `finalizeplan`, target_repos registered |

### Requirement Traceability

| Requirement | Story | Epic | Traced |
|---|---|---|---|
| SW-B1 (prompt-start gate) | SW-1 | EP-1 | ✅ |
| SW-B2 (index-driven listing) | SW-3 | EP-1 | ✅ |
| SW-B3 (empty-index fallback) | SW-3 | EP-1 | ✅ |
| SW-B4 (explicit selection) | SW-3 | EP-1 | ✅ |
| SW-B5 (validation before action) | SW-5 | EP-2 | ✅ |
| SW-B6 (bounded side effects) | SW-7, SW-8 | EP-2 | ✅ |
| SW-B7 (proportional context) | SW-9 | EP-2 | ✅ |
| SW-B8 (stale warning) | SW-6 | EP-2 | ✅ |
| SW-B9 (target repo state) | SW-6 | EP-2 | ✅ |
| SW-B10 (output parity testable) | SW-12 | EP-3 | ✅ |

---

## Risk Register

| ID | Severity | Risk | Mitigation | Owner Story |
|---|---|---|---|---|
| R1 | High | SW-4 deprecated command name references may extend beyond switch output text | SW-4 requires string-scan regression covering all user-facing surfaces; coordinate with new-feature team on final retained alias | SW-4 |
| R2 | High | `express` track not in service constitution `permitted_tracks` | Governance housekeeping — add `express` to constitution permitted tracks in parallel with dev; does not block implementation | H2 (separate task) |
| R3 | Medium | `feature-index.yaml` status and `feature.yaml` phase can diverge | Switch reports both; lifecycle tooling gap accepted as M2 debt item; does not block switch implementation | M2 (follow-up lifecycle story) |
| R4 | Medium | `create-dev-branch` now called — M4 from finalizeplan review is resolved | `feature/switch-dev` created before bundle; M4 is resolved for this feature | ✅ Resolved |
| R5 | Medium | Domain fallback must hard-stop without inferring a feature | SW-3 acceptance criteria explicitly require no-inference for domains mode | SW-3 |
| R6 | Low | Old test stub only covers prompt-start behavior | SW-12 wires focused regression across full switch semantics | SW-12 |
| R7 | Low | `test-switch-ops.py` may not yet exist | SW-12 blocked by SW-9; SW-9 acceptance criteria require test file creation or verification | SW-9, SW-12 |

---

## Sprint Sequencing Validation

| Sprint | Stories | Prerequisites Met | Parallel Safe |
|---|---|---|---|
| Sprint 1 (EP-1) | SW-1, SW-2, SW-3, SW-4 | ✅ Baseline surface available | SW-1, SW-2, SW-4 can run in parallel; SW-3 depends on SW-2 config |
| Sprint 2 (EP-2) | SW-5, SW-6, SW-7, SW-8, SW-9 | ✅ EP-1 complete | SW-5, SW-7, SW-9 can run in parallel; SW-6 depends on SW-5; SW-8 depends on SW-6 |
| Sprint 3 (EP-3) | SW-10, SW-11, SW-12 | EP-2 complete | SW-10 and SW-11 parallel; SW-12 blocked-by SW-9, SW-10 |

---

## Governance Items (Non-Blocking)

These items should be resolved in parallel with development but do not block sprint start:

1. **H2 — Constitution track gap:** Add `express` to `permitted_tracks` in the `new-codebase` service constitution. Assign to governance admin.
2. **M2 — feature-index sync gap:** Lifecycle phase completion commands should update both `feature-index.yaml` status and `feature.yaml` phase atomically. Track as a separate lifecycle tooling story.
3. **M4 — FinalizePlan dev-branch sub-step:** Document `create-dev-branch` as an explicit sub-step in the FinalizePlan SKILL.md three-step execution contract. Track as a skill update story.

---

## Definition of Done (Feature-Level)

- [ ] All 12 stories pass their acceptance criteria
- [ ] `test-switch-ops.py` passes all fixtures
- [ ] String-scan regression finds zero deprecated command references
- [ ] Governance files unchanged after any switch operation (verified by governance no-write test)
- [ ] `switch` appears consistently across all four command discovery surfaces
- [ ] `feature/switch-dev` PR opened against `main` in `crisweber2600/lens.core.src`
