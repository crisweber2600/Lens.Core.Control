---
feature: lens-dev-new-codebase-trueup
doc_type: implementation-readiness
status: approved
goal: "Confirm PRD, Architecture, Epics, and Stories are complete and aligned before dev phase"
key_decisions:
  - Overall readiness: READY — all planning artifacts complete; no blocking gaps found
  - TU-2.1 requires BMB channel pre-load before authoring; story spec enforces this
  - TU-4.1 is the longest story (8 points); parity audit work is research-intensive
  - EP-1 through EP-3 are parallelizable; EP-5 must wait on EP-4
open_questions: []
depends_on:
  - prd.md
  - architecture.md
  - epics.md
  - stories.md
  - finalizeplan-review.md
blocks: []
updated_at: 2026-04-29T00:00:00Z
---

# Implementation Readiness — lens-dev-new-codebase-trueup (True Up)

## Readiness Verdict: ✅ READY

All planning artifacts are complete, internally consistent, and all prior review carry-forward constraints have been allocated to dev stories. The feature is cleared to enter the dev phase.

---

## Artifact Completeness

| Artifact | Status | Notes |
|----------|--------|-------|
| `product-brief.md` | ✅ Complete | |
| `brainstorm.md` | ✅ Complete | |
| `research.md` | ✅ Complete | |
| `prd.md` | ✅ Complete | 14 FRs; stepsCompleted all 11 steps |
| `ux-design.md` | ✅ Complete | |
| `architecture.md` | ✅ Complete | 4 ADRs, 8 steps, scope boundary documented |
| `preplan-adversarial-review.md` | ✅ Complete | pass-with-warnings |
| `businessplan-adversarial-review.md` | ✅ Complete | pass-with-warnings |
| `techplan-adversarial-review.md` | ✅ Complete | pass-with-warnings; 5 carry-forwards allocated |
| `finalizeplan-review.md` | ✅ Complete | pass-with-warnings; 11 carry-forwards (CF-1 to CF-11) allocated |
| `epics.md` | ✅ Complete | 5 epics |
| `stories.md` | ✅ Complete | 11 stories, 33 points |

---

## PRD → Architecture Traceability

| FR | Description | Architecture Artifact | Story |
|----|-----------|-----------------------|-------|
| FR-1 | lens-switch.prompt.md | Section 3.1 (Layer 1 prompt) | TU-1.1 |
| FR-2 | lens-new-feature.prompt.md | Section 3.1 | TU-1.2 |
| FR-3 | lens-complete.prompt.md | Section 3.1 | TU-1.3 |
| FR-4 | bmad-lens-complete SKILL.md | Section 3.2, BMB channel | TU-2.1 |
| FR-5 | test-complete-ops.py stubs | Section 3.2 (test stub signatures) | TU-2.2 |
| FR-6 | adr-complete-prerequisite.md | ADR-1 (Section 4) | TU-3.1 |
| FR-7 | adr-constitution-tracks.md | ADR-2 (Section 4) | TU-3.2 |
| FR-8 | Python 3.12 section (parity audit) | ADR-3 (Section 4) | TU-4.1 |
| FR-9 | SAFE_ID_PATTERN scan evidence | ADR-4 (Section 4) | TU-4.1 |
| FR-10 | switch parity audit | Section 3.4 | TU-4.1 |
| FR-11 | new-domain parity audit | Section 3.4 | TU-4.1 |
| FR-12 | new-service parity audit | Section 3.4 | TU-4.1 |
| FR-13 | Reference docs (auto-context-pull, init-feature) | Section 3.4 | TU-4.2 |
| FR-14 | parity-gate-spec.md | Section 3.4 | TU-4.3 |
| FR-15 | Governance companion actions | Section 5 / Step 8 | TU-5.1 |

**Traceability status: 100% — all 14 FRs mapped to stories.**

---

## Carry-Forward Constraint Allocation

| CF # | Constraint | Allocated To |
|------|-----------|-------------|
| CF-1 | BMB invocation path in FR-4 story AC | TU-2.1 |
| CF-2 | FR-9 scan evidence as explicit AC | TU-4.1 |
| CF-3 | Step 8 read-first + idempotency | TU-5.1 |
| CF-4 | parity-gate-spec.md "How to Apply" section | TU-4.3 |
| CF-5 | Blocker annotation timing gap documented | TU-5.1, TU-4.1 |
| CF-6 | `.github/prompts/` NOT in dev agent scope | TU-1.1, TU-1.2, TU-1.3 |
| CF-7 | 14-FR completion checklist in final story | TU-5.1 |
| CF-8 | Parallel sequencing in sprint plan | `sprint-status.yaml` |
| CF-9 | Parity audit review window before EP-5 | `sprint-status.yaml` |
| CF-10 | conftest.py fixture scaffold in FR-5 | TU-2.2 |
| CF-11 | Constitution reference for parity-gate-spec | TU-4.3 |

**Allocation status: 11/11 carry-forward constraints allocated.**

---

## Risk Register

| # | Risk | Severity | Mitigation |
|---|------|---------|-----------|
| R1 | TU-4.1 (parity audit) scope creep — research uncovers new gaps | Medium | Scope is bounded to the 5 in-scope features; additional gaps are documented in the report and flagged for separate features, not absorbed into True Up |
| R2 | TU-2.1 BMB channel not available or mis-configured | Medium | Story requires pre-check: load `bmadconfig.yaml` and verify BMB skill is accessible before authoring begins |
| R3 | FR-9 SAFE_ID_PATTERN scan reveals IDs with dots/underscores in governance | High | If found, halt and surface as a critical finding before continuing. ADR-4 adopted based on clean scan — a positive scan result would invalidate the ADR and require re-decision. This story becomes blocking until re-decision is recorded. |
| R4 | TU-5.1 governance write conflict (another author has modified the target feature.yaml) | Low | Read-first check (CF-3) will surface the conflict; merge manually before writing annotation |
| R5 | EP-4 parity audit results challenge governance phase labels for `new-domain` or `new-service` | Medium | If audit reveals phases are incorrect, document in parity-audit-report.md and open a separate governance correction feature; True Up does not update governance phases directly (only the two regression blockers for new-feature and complete) |

---

## Story Sequencing Constraints

```
EP-1 (TU-1.1, TU-1.2, TU-1.3)  ─────┐
EP-2 (TU-2.1, TU-2.2)           ─────┤──→ EP-4 (after EP-3 ADRs)──→ EP-5
EP-3 (TU-3.1, TU-3.2)           ─────┘
```

- EP-1, EP-2, EP-3 are fully parallelizable
- TU-4.1 (parity audit report) requires TU-3.1 and TU-3.2 (ADRs) to be complete first (it references them in design-decision sections)
- TU-4.2 and TU-4.3 are independent within EP-4
- TU-5.1 must run after all EP-4 stories are complete
- Parity audit review window: TU-4.1 commit → [review period] → TU-5.1 runs

---

## Constitution Compliance

| Requirement | Status |
|-------------|--------|
| BMB-first rule for SKILL.md changes | ✅ Enforced in TU-2.1 AC |
| `stories` artifact present | ✅ `stories.md` |
| Peer review enforced | ✅ Plan PR #27 created; plan PR into base branch required before dev |
| `express` and `expressplan` in permitted_tracks | ✅ Service constitution confirmed |
