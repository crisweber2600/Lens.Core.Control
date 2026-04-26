---
feature: lens-dev-new-codebase-new-domain
doc_type: implementation-readiness
status: pass-with-conditions
verdict: READY TO PROCEED — conditions documented below must be addressed at story start
decision_basis: finalizeplan-review.md (pass-with-warnings verdict)
reviewed_artifacts:
  - business-plan.md
  - tech-plan.md
  - finalizeplan-review.md
updated_at: 2026-04-26T00:00:00Z
---

# Implementation Readiness Report — new-domain Command

**Feature:** `lens-dev-new-codebase-new-domain`  
**Prepared by:** Lens FinalizePlan Bundle  
**Date:** 2026-04-26  
**Based on:** finalizeplan-review.md (party-mode cross-review verdict: **pass-with-warnings**)

---

## Summary Verdict

**READY TO PROCEED — with conditions.** The planning artifacts are coherent and sufficiently detailed for implementation to begin. There are no critical blockers. One high-severity finding (Winston-P: operation order) must be addressed in Story 1.2 before any other story in Epic 1 proceeds past design review.

---

## Gate Checklist

| Gate | Status | Notes |
|---|---|---|
| Business plan present | ✅ Pass | `business-plan.md` complete |
| Tech plan present | ✅ Pass | `tech-plan.md` complete with ADRs, API contract, schemas |
| Final review complete | ✅ Pass | `finalizeplan-review.md` verdict: pass-with-warnings |
| Critical blockers | ✅ None | 0 critical findings |
| High severity findings | ⚠️ 1 (addressed) | Winston-P — operation order — carry-forward to Story 1.2 |
| Schema specifications frozen | ✅ Pass | All three output schemas specified verbatim in tech-plan |
| Test strategy defined | ✅ Pass | 6 integration tests + unit + parity specified |
| Dependencies identified | ✅ Pass | Blocks: new-service, new-feature; Blocked by: baseline |

---

## Findings Register — Implementation Tracking

Each finding from finalizeplan-review.md is mapped to a story with a disposition.

### F3 / R1 (Medium) — SAFE_ID_PATTERN Discrepancy

**Original finding status:** The current `business-plan.md` and `tech-plan.md` both show `^[a-z0-9][a-z0-9._-]{0,63}$`; there is no longer a plan-to-plan discrepancy in the reviewed artifacts.

**Disposition:** Treat this as a source-of-truth confirmation step in Story 1.1 (first story), not as an active artifact mismatch. Developer must open old-codebase `init-feature-ops.py`, read the actual constant value, and embed it with a citation comment. The confirmed pattern must be documented in `docs/lens-dev/new-codebase/SHARED_CONTRACTS.md` before `new-service` or `new-feature` begin their validators.

**Condition:** Story 1.1 cannot be marked complete until the pattern used for implementation is confirmed against the old-codebase source and cited.

---

### Winston-P (High) — Duplicate Check After Pull

**Original finding:** Architecture diagram shows duplicate_check before sync_governance_main, creating a race condition. The corrected operation order is: `validate_safe_id` → `sync_governance_main` → `duplicate_check` → `write_artifacts` → `governance_git_sequence`.

**Disposition:** This is the single highest-priority correctness requirement. Embedded as an explicit code-ordering requirement in Story 1.2. Phrased as a non-negotiable AC line: "Duplicate check runs AFTER sync_governance_main pull."

**Condition:** Story 1.2 design review (code walkthrough) must verify the operation order before merge.

---

### F9 / Murat (Medium) — Integration Test Isolation

**Original finding:** Integration tests must use isolated temp-dir governance fixtures (pytest `tmp_path`). The story spec says tests must not reference any real governance repo path.

**Disposition:** Embedded in Story 1.5 ACs. Every integration test AC is phrased with explicit `pytest tmp_path` requirement. Reviewer must check that no test imports or references a real `TargetProjects/lens/lens-governance` path.

**Condition:** Story 1.5 review must verify no real governance repo is referenced.

---

### F10 / Winston (Medium) — Constitution Body Parity Fixture

**Original finding:** Parity test for constitution body must use a spec-derived fixture, not a copy from old-codebase source. This is the clean-room constraint applied to testing.

**Disposition:** Embedded in Story 1.4 with the verbatim constitution body template defined in this document (and in stories.md) as the authoritative fixture. Developer must define the expected body as an inline Python string constant, not by reading from old-codebase source.

**Condition:** Story 1.4 review must verify the test fixture is not loaded from a file path.

---

### John-P (Medium) — Slug Derivation UX Confirmation

**Original finding:** Slug is derived from display name input. The user must see the derived slug and explicitly confirm before any write occurs.

**Disposition:** Embedded in Story 2.3 as explicit AC: "Derived slug is shown to user before invocation: 'Domain slug will be: `{slug}`. Proceed? [Y/n/edit]'". User must confirm or edit before script invocation.

**Condition:** Story 2.3 must include a manual walkthrough verification step.

---

### Mary-P (Medium) — Cross-Feature SAFE_ID_PATTERN Dependency

**Original finding:** `new-service` and `new-feature` must not implement their slug validators until SAFE_ID_PATTERN is resolved in Story 1.1 and committed as a shared constant.

**Disposition:** Blocking dependency declared in epics.md dependency diagram and in Story 1.1 AC. Story 1.1 includes the `SHARED_CONTRACTS.md` deliverable for cross-feature communication.

**Condition:** `new-service` and `new-feature` feature initiation must not begin validator implementation until `SHARED_CONTRACTS.md` is committed to main.

---

### R3 (Medium) — name Defaults to Slug

**Original finding:** When `--name` is omitted, `domain.yaml.name` should equal the slug. This was not explicit in the original tech-plan API contract.

**Disposition:** Embedded in Story 1.4 as `test_create_domain_name_defaults_to_slug` AC.

**Condition:** Story 1.4 must include the name-defaults-to-slug test case.

---

## Architecture Concerns — Cleared

| Concern | Resolution |
|---|---|
| context.yaml inclusion scope | ADR-3 accepted: context.yaml is optional, only created when `--personal-folder` is provided |
| governance_git vs manual | ADR-2 accepted: `--execute-governance-git` flag controls this; default is manual |
| Scaffold creation atomicity | No atomicity requirement for `.gitkeep` files; they are idempotent |

---

## Pre-Implementation Checklist (for Sprint 1 kickoff)

Before any code is written in Sprint 1:

- [ ] Developer has read this document and finalizeplan-review.md
- [ ] Developer has located old-codebase `init-feature-ops.py` and confirmed it is readable at `TargetProjects/lens-dev/old-codebase/lens.core.src/`
- [ ] Winston-P finding is understood: operation order is validate → sync → duplicate-check → write → git
- [ ] SAFE_ID_PATTERN discrepancy (F3/R1) is understood and Story 1.1 is the first story started
- [ ] `SHARED_CONTRACTS.md` placeholder is created before Sprint 2 begins
