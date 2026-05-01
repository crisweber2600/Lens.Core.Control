---
feature: lens-dev-new-codebase-dogfood
doc_type: implementation-readiness
status: draft
goal: "Assess implementation readiness for clean-room parity rebuild of lens.core.src across 5 epics and 27 stories."
key_decisions:
  - Feature is implementation-ready with conditions: target_repos must be registered before story execution begins (M1 response D).
  - Defect traceability from ExpressPlanBugs.md is explicit in this artifact per H1 response D.
  - Accepted risks from finalizeplan-review are carried forward and tracked.
open_questions: []
depends_on:
  - epics.md
  - stories.md
  - sprint-plan.md
  - tech-plan.md
  - finalizeplan-review.md
blocks: []
updated_at: '2026-05-01T14:30:00Z'
---

# Implementation Readiness — Dogfood Clean-Room Parity

## Readiness Assessment

### Overall Verdict: READY WITH CONDITIONS

The feature is ready for dev handoff. The planning set (business-plan, tech-plan, sprint-plan, expressplan-adversarial-review, finalizeplan-review) is coherent and reviewed. Epics, stories, and sprint assignments are defined. Two conditions must be met before Sprint 1 begins:

| Condition | Status | Action Required | Owner |
|-----------|--------|-----------------|-------|
| `feature.yaml.target_repos` populated with `lens.core.src` | PENDING — currently empty (M1 response D) | Run `bmad-lens-feature-yaml update --featureId lens-dev-new-codebase-dogfood` to register `lens.core.src` | Dev (before Sprint 1) |
| All story files include `## Implementation Channel` section | PENDING — story files created without section for non-lens-work stories | Every story touching `lens.core.src/_bmad/lens-work/` must name BMB path; exceptions must be declared (H2 response D) | Dev (before each story) |

---

## Defect Traceability (H1 — FinalizePlan Review Response D)

All 8 ExpressPlanBugs.md defects map to story IDs and acceptance criteria below. This table is the governance evidence that the defect intake was fully absorbed.

| # | Defect Summary | Story | Acceptance Criterion |
|---|---------------|-------|----------------------|
| 1 | Constitution resolver false negative for `express` track | E1-S2 (lifecycle contract) + E2-S4 (phase-start validation) | Constitution resolver correctly permits `express` track for `lens-dev/new-codebase`; regression fixture for this domain/service combination passes. |
| 2 | Editor glob / `rg` discovery assumptions — environment-dependent and brittle | E1-S3 (module config) | Config and feature discovery work without `rg` or any specific editor search provider; fallback path is tested. |
| 3 | Git Bash `/d/...` path form creates files outside workspace on Windows | E1-S3 (module config) + E2-S6 (express publish mapping) | File write and publish operations use OS-normalized absolute paths; no artifact is written to `C:/d/...` or similar on Windows. |
| 4 | Express publish mapping copies only the review artifact by default | E2-S6 (express publish artifact mapping) | `publish-to-governance --phase expressplan` copies `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, and the express review artifact; both `expressplan-adversarial-review.md` and legacy `expressplan-review.md` are recognized and reported. |
| 5 | `feature-index.yaml` goes stale after phase transitions | E2-S3 (feature-index sync) | A sanctioned sync operation updates `feature-index.yaml` after every `feature.yaml` phase change; tested with a fixture that exercises the stale-entry scenario. |
| 6 | Dirty governance state blocks phase advancement | E1-S4 (feature-yaml operations) + E2-S4 (phase-start validation) | Dirty-state detection pulls, stages, commits, and pushes relevant changes, then reports the SHA before continuing; blocking on uncommitted changes is not valid behavior. |
| 7 | Public prompt chain gap discovered after initial planning | E3-S1 (prompt stubs + release prompts) | Public stubs, release prompts, module metadata, and skill owners are validated as a single inventory; no retained command is parity-complete without a working public prompt chain. |
| 8 | Clean-room file hash verification happens outside VS Code | E5-S5 (parity report) | Parity report includes a clean-room compliance checkpoint: external hash comparison result for all touched `lens.core.src` files against `lens.core` counterparts is included as evidence. |

---

## Risk Assessment

### Risk Register

| ID | Risk | Likelihood | Impact | Mitigation | Status |
|----|------|-----------|--------|-----------|--------|
| R1 | `feature.yaml.target_repos` remains empty at Sprint 1 start | High — currently empty | High — Dev validation fails; cross-feature context broken | Register `lens.core.src` before story execution begins (M1 response D) | **OPEN — ACTION REQUIRED** |
| R2 | Sprint 4 Dev conductor work duplicates `lens-dev-new-codebase-dev` scope | Medium | High — authority boundary violation | S4.x story files include coordination note naming `lens-dev-new-codebase-dev` as canonical; dogfood scope is parity only (M2 response A) | TRACKED |
| R3 | Express review filename drift between `expressplan-adversarial-review.md` and `expressplan-review.md` | High — both exist in governance | Low — compatibility debt, not a blocker | ADR-5 accepted tech debt; E5-S6 documents mapping; accepted-risks in sprint-status (M3 response A) | ACCEPTED RISK |
| R4 | `feature-index.yaml` stale after expressplan-complete transition | Confirmed — live instance of BF-3 | Medium — stale routing | E2-S3 story resolves BF-3; update feature-index after this review is committed (L1 response A) | TRACKED |
| R5 | Sprint 1 foundations expose additional target gaps | Medium | Medium — scope may grow | Parity map (E1-S1) gates Sprint 2; new gaps added to backlog, not Sprint 1 | TRACKED |
| R6 | Missing prerequisite skills for phase conductors | Medium | High — conductors delegate to missing skills | E3-S3 confirms all prerequisites; any gaps flagged before Sprint 3 proceeds | TRACKED |
| R7 | Windows `uv run python -m pytest` environment issues | Low | Medium — test suite cannot run | E5-S1 uses Windows-validated pytest invocation; fallback documented | TRACKED |

### Accepted Risks

| ID | Risk | Decision | Record |
|----|------|----------|--------|
| AR1 | ADR-5 express review filename compatibility debt | Accepted as known tech debt | Sprint-status accepted-risks entry; E5-S6 documents mapping (M3 response A) |
| AR2 | Sprint 4 Dev stories may partially overlap `lens-dev-new-codebase-dev` | Accepted as parity scope; coordination note in all S4.x story files (M2 response A) | All E4 story files contain coordination note |

---

## Sprint Readiness Summary

| Sprint | Epic | Stories | Blockers | Status |
|--------|------|---------|----------|--------|
| 1 | Foundation Restoration | E1-S1 through E1-S5 | None after target_repos registration | READY |
| 2 | Git Orchestration | E2-S1 through E2-S6 | Depends on Sprint 1 foundations | BLOCKED ON SPRINT 1 |
| 3 | Command Surface | E3-S1 through E3-S5 | Depends on Sprints 1 and 2 | BLOCKED ON SPRINT 2 |
| 4 | Dev/Complete | E4-S1 through E4-S5 | Depends on Sprints 2 and 3 | BLOCKED ON SPRINT 3 |
| 5 | Parity Proof | E5-S1 through E5-S6 | All implementation sprints complete | BLOCKED ON SPRINT 4 |

---

## Acceptance Criteria Summary

The feature is implementation-complete when:

1. All 27 stories across 5 sprints have passed their acceptance criteria.
2. `feature.yaml.target_repos` includes `lens.core.src`.
3. All 8 ExpressPlanBugs.md defects have passing regression fixtures.
4. BF-1 through BF-6 bugfix stories have passing tests.
5. A parity report (E5-S5) is committed with green contracts and documented compatibility debt.
6. Clean-room rule is evidenced: no files copied from `lens.core`; external hash check result included in parity report.
7. Every story touching `lens-work` files names the required BMB implementation channel.
8. The express review filename compatibility mapping (E5-S6) is committed and linked from the parity report.

---

## Definition of Done

- [ ] All 27 story acceptance criteria pass.
- [ ] `uv run python -m pytest` runs and passes on Windows for all touched skills.
- [ ] No governance file was written directly by an agent; all governance writes went through the publish CLI or sanctioned operations.
- [ ] Governance repo history remains flat on `main`.
- [ ] Control repo work used the 2-branch feature topology (base + plan).
- [ ] Target `lens.core.src` used its configured branch strategy independently.
- [ ] Known naming drift (expressplan-review.md vs expressplan-adversarial-review.md) is documented, tested for compatibility, and linked from the parity report.
- [ ] Feature creation or phase readiness warns on missing `target_repos` for implementation-impacting features.
