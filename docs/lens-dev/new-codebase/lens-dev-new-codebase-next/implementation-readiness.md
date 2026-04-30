---
feature: lens-dev-new-codebase-next
doc_type: implementation-readiness
status: draft
goal: "Assess readiness for dev handoff of the Next command clean-room rewrite."
key_decisions:
  - Ready for dev once the three gated conditions below are confirmed.
  - Paused-state behavior decision is deferred to E2-S1 — must be resolved before Slice 3 fixtures.
  - Constitution resolver dependency confirmed as formal gate (H1-A from finalizeplan-review).
open_questions:
  - Will lens-dev-new-codebase-constitution reach expressplan-complete before Slice 4 begins?
  - Who owns the constitution resolver allow-list fix?
depends_on:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-next/stories.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-next/finalizeplan-review.md
blocks: []
updated_at: 2026-04-30T22:15:00Z
---

# Implementation Readiness — Next Command Rewrite

## Readiness Assessment

### Overall Verdict: READY WITH CONDITIONS

The feature is ready for dev handoff on Slices 2 and 3. Slice 4 carries a formal entry
gate on the constitution resolver dependency. All planning artifacts are present, the
adversarial review has been completed and responses recorded, and the FinalizePlan review
is pass-with-warnings.

| Condition | Status | Owner | Blocks |
|-----------|--------|-------|--------|
| Trueup discovery-surface writes complete before E1-S4 | PENDING | `lens-dev-new-codebase-trueup` (M2) | E1-S4 |
| Paused-state behavior documented in E2-S1 before fixtures | PENDING | Dev (E2-S1) | E2-S4 paused fixture |
| Constitution resolver dependency confirmed or scoped (H1) | PENDING | Dev (E3-S3) | E3-S4 release gate |

---

## Risk Register

| ID | Risk | Likelihood | Impact | Mitigation | Status |
|----|------|-----------|--------|-----------|--------|
| R1 | Paused-state behavior undocumented — fixtures written to wrong semantics | High | Medium — Slice 3 parity test coverage is wrong | E2-S1 gate: document decision before E2-S4 writes paused-state fixture | OPEN |
| R2 | `lens-dev-new-codebase-constitution` has not reached `expressplan-complete` by Slice 4 | Medium | High — express-track routing fails constitution permission check | E3-S3: gate tracks state; scope fix into this feature if dependency not ready | OPEN |
| R3 | Discovery-surface write conflict with trueup | Medium | Medium — duplicate or overwritten module-help rows | M2 gate in E1-S4: check trueup state before registering `next` | OPEN |
| R4 | `next-ops.py` stubs lifecycle.yaml content instead of loading live file | Medium | High — routing diverges from actual installed lifecycle contract | E2-S1 AC explicitly requires live file load; Blind Spot 2 note in finalizeplan-review | OPEN |
| R5 | `next` command produces a governance or control-doc write (no-write regression) | Low | High — violates read-only routing contract | E3-S2 negative test | OPEN |

### Closed / Mitigated Risks

| ID | Risk | Mitigation Applied |
|----|------|-------------------|
| R6 | Express-track routing not covered by fixtures | Scoped explicitly into E2-S3 with AC requiring `auto_advance_to` coverage | CLOSED (design) |
| R7 | Pre-confirmed handoff triggers second confirmation prompt | E1-S3 and E3-S1 ACs prohibit second confirmation; verified via GWT | CLOSED (design) |
| R8 | Planning bundle naming drift from lifecycle contract | `finalizeplan-review.md` present, responses recorded, matches lifecycle contract naming | CLOSED |

---

## Prerequisites Checklist

### Planning prerequisites ✓
- [x] Business plan produced and reviewed
- [x] Tech plan produced and reviewed
- [x] Sprint plan produced and consolidated
- [x] ExpressPlan adversarial review completed (pass-with-warnings, all responses recorded)
- [x] FinalizePlan review completed (pass-with-warnings, responses H1=A, M1=A, M2=C, L1=A)
- [x] Epics defined
- [x] Stories with Given/When/Then acceptance criteria defined
- [x] Implementation readiness assessed
- [x] Planning PR merged into base branch

### Dev prerequisites (at sprint start)
- [ ] Confirm `lens-dev-new-codebase-trueup` discovery-surface writes complete (before E1-S4)
- [ ] Target repo `lens.core.src` develop branch is accessible
- [ ] `light-preflight.py` passes in target repo
- [ ] `lens-dev-new-codebase-constitution` governance state confirmed (before Slice 4)

---

## Slice Entry Gates

### Slice 2 entry (Epic 1)
- Planning PR merged: ✓
- Adversarial review responses recorded: ✓
- FinalizePlan review responses recorded: ✓

### Slice 3 entry (Epic 2)
- E1-S1 through E1-S4 complete: pending
- Paused-state behavior documented in E2-S1: pending

### Slice 4 entry (Epic 3)
- E2-S1 through E2-S4 complete: pending
- Paused-state decision confirmed in E2-S1: pending
- All parity fixtures pass: pending

### Release gate
- All stories E1-S1 through E3-S4 done: pending
- No-write negative test passes: pending
- Constitution resolver dependency documented: pending
