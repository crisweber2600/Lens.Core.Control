---
feature: lens-dev-new-codebase-finalizeplan
doc_type: implementation-readiness
status: draft
goal: "Assess implementation readiness for FinalizePlan, ExpressPlan, QuickPlan conductor delivery."
key_decisions:
  - Treat the feature as implementation-ready for code work while keeping predecessor-gate and constitution-permission risks as explicit tracked conditions.
  - Carry publish CLI hyphenation drift and missing prerequisite skills as explicit warnings rather than hidden assumptions.
open_questions: []
depends_on:
  - lens-dev-new-codebase-expressplan
blocks: []
updated_at: '2026-04-30T00:00:00Z'
---

# Implementation Readiness — FinalizePlan, ExpressPlan, QuickPlan Conductors

## Readiness Assessment

### Overall Verdict: READY WITH CONDITIONS

The feature is ready for dev handoff. The implementation infrastructure was created in
a prior session and 34 tests pass. Epics and stories are defined. Three conditions must
be met before Sprint 1 is marked done:

| Condition | Status | Owner |
|-----------|--------|-------|
| H2 predecessor gate (`expressplan-complete`) confirmed in FinalizePlan SKILL.md | UNVERIFIED — must check | Dev (E1-S1) |
| Express-track constitution permission for `lens-dev/new-codebase` set | UNVERIFIED — must check | Dev (E1-S2) |
| Infrastructure files committed to target source repo `develop` branch | PENDING | Dev (E1-S5) |

---

## Risk Assessment

### Risk Register

| ID | Risk | Likelihood | Impact | Mitigation | Status |
|----|------|-----------|--------|-----------|--------|
| R1 | FinalizePlan SKILL.md only accepts `techplan` as predecessor, not `expressplan-complete` | Medium | High — express-track FinalizePlan would fail at activation | E1-S1 checklist item: validate and remediate if found | OPEN |
| R2 | Constitution does not permit express track for `lens-dev/new-codebase` | Medium | High — ExpressPlan blocks at activation | E1-S2: confirm and add if missing | OPEN |
| R3 | `publish-to-governance` CLI silently produces empty mirror for express-track hyphenated artifact names | Medium | Low — governance mirror is informational only at this stage | E1-S3: document; track as CLI bug if found | OPEN |
| R4 | Duplicate `module.yaml` entries with `lens-dev-new-codebase-expressplan` feature | Low | Medium — module loading may fail or produce unexpected behavior | E1-S1 checklist: confirm no duplicates | OPEN |
| R5 | Missing prerequisite skills in target repo | Low | High — conductors delegate to skills that may not exist | E2-S3: confirm all prerequisites present | OPEN |

### Closed / Mitigated Risks

| ID | Risk | Mitigation Applied |
|----|------|-------------------|
| R6 | Governance writes going through direct file creation | Design decision: all governance writes route through publish CLI; no direct file creation in conductor SKILL.md files | CLOSED |
| R7 | Test regressions from conductor infrastructure additions | 34 tests passed in prior session after conductor files created | CLOSED (verify in E1-S4) |
| R8 | Express track unsupported in lifecycle.yaml | lifecycle.yaml v4.0 defines express track with `phases: [expressplan, finalizeplan]` | CLOSED |

---

## Prerequisites Checklist

### Planning prerequisites ✓
- [x] Business plan produced and reviewed
- [x] Tech plan produced and reviewed
- [x] Sprint plan produced and consolidated per party-mode feedback
- [x] ExpressPlan adversarial review completed (pass-with-warnings)
- [x] FinalizePlan review completed (pass-with-warnings)
- [x] Epics defined
- [x] Stories with acceptance criteria defined
- [x] Planning PR merged into base branch

### Implementation prerequisites (verify in Sprint 1)
- [ ] All conductor SKILL.md files structurally valid
- [ ] FinalizePlan predecessor gate handles `expressplan-complete`
- [ ] Express-track constitution permission set
- [ ] Publish CLI handles express-track artifact names
- [ ] All prerequisite skills present in target repo
- [ ] All infrastructure files committed to target source repo

### Dev handoff prerequisites (Sprint 3 exit)
- [ ] Discovery surface registration complete
- [ ] Test coverage gaps documented
- [ ] Final PR open
- [ ] `feature.yaml` phase = `finalizeplan-complete`

---

## Known Gaps and Out-of-Scope Items

| Gap | Type | Next Step |
|-----|------|-----------|
| End-to-end behavior test (activate `/lens-finalizeplan` in real session) | Test coverage | Future hardening sprint |
| Constitution check unit test (mock constitution fixture) | Test coverage | Future hardening sprint |
| FinalizePlan step-2 and step-3 integration tests | Test coverage | Future hardening sprint |
| Shared utility extraction (G3 from baseline) — review-ready fast path, batch 2-pass contract, publish-before-author hook | Architecture | Separate feature (`lens-dev-new-codebase-baseline` dev phase) |
