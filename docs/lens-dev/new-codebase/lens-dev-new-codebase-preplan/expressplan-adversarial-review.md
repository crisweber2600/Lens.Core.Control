---
feature: lens-dev-new-codebase-preplan
doc_type: adversarial-review
phase: expressplan
source: phase-complete
review_format: abc-choice-v1
verdict: pass-with-warnings
finding_count: 4
reviewed_artifacts:
  - business-plan.md
  - tech-plan.md
reviewers:
  - adversarial
  - party-mode-challenge
updated_at: 2026-04-28T00:00:00Z
---

# ExpressPlan Adversarial Review — Preplan Command

**Feature:** `lens-dev-new-codebase-preplan`
**Phase:** expressplan
**Verdict:** pass-with-warnings
**Findings:** 4 (2 medium, 2 low)

---

## Adversarial Analysis

### Finding F-1 — Test Location Inconsistency [MEDIUM]

**Location:** `tech-plan.md` — ADR 1 vs. Testing Strategy section

**Description:** ADR 1 states "bmad-lens-preplan has no owned script layer" and explicitly no `scripts/` directory. The Testing Strategy section then places parity tests at `_bmad/lens-work/skills/bmad-lens-preplan/scripts/tests/test-preplan-parity.py`. These two statements are irreconcilable: tests cannot live in a directory that the design explicitly forbids.

**Options:**
- **A** — Resolve by placing tests in a dedicated top-level test target (e.g., the skills-level integration test suite used by other parity tests), removing the scripts path from the tech plan, and updating ADR 1 to note the test-only exception explicitly.
- **B** — Resolve by creating a lightweight `scripts/tests/` directory that holds only test files (no implementation scripts), and updating ADR 1 to clarify the "no scripts" rule applies to implementation scripts, not test fixtures.
- **C** — Resolve by deferring the preplan test location decision to the Sprint 1.3 story, making it an open question in the tech plan with a placeholder.
- **D** — Accept the inconsistency and allow the test path to drift; document it as a known deviation.
- **E** — Reject both approaches and investigate how existing phase parity tests (e.g., for switch, init-feature) are located to follow the established pattern.

**Recommended:** E → determine the established pattern from existing parity tests in the new codebase, then apply consistently. This is a quality gate, not a blocker.

---

### Finding F-2 — Missing `/next` Pre-Confirmed Handoff Contract [MEDIUM]

**Location:** Both `business-plan.md` and `tech-plan.md`

**Description:** Research.md section 4.5 documents the `next-handoff pre-confirmed contract`: when `/next` delegates to a phase skill, that phase skill must not ask a redundant launch confirmation. This is a user-experience invariant documented in both the research corpus and the `next` SKILL.md. Neither the business plan nor the tech plan mentions this requirement. If the preplan conductor is implemented without this invariant, a `/next` → `/preplan` handoff will present a superfluous "would you like to start preplan?" prompt, breaking the confirmed user experience.

**Options:**
- **A** — Add the pre-confirmed handoff contract as an explicit success criterion in the business plan and as a behavioral constraint in the tech plan (under On Activation and under Testing Strategy with a dedicated parity test).
- **B** — Add it to the open_questions list in the business plan and defer to the SKILL.md authoring phase.
- **C** — Handle it exclusively in Sprint 2.1 (activation) without surfacing it in the business plan or tech plan.
- **D** — Treat it as a `/next` responsibility, not a preplan responsibility, and exclude it from scope.
- **E** — Add a note in the tech plan's "Technical Constraints" section only, without updating the business plan.

**Recommended:** A — the contract is explicitly documented in the research corpus and failure to implement it would be a user-observable regression.

---

### Finding F-3 — `fetch-context` Availability Unverified [LOW]

**Location:** `tech-plan.md` — Dependency Map

**Description:** The dependency map lists `bmad-lens-init-feature (fetch-context)` as used in "On Activation step 7." However, the tech plan does not note whether `fetch-context` is implemented in `TargetProjects/lens-dev/new-codebase/lens.core.src`. If absent, the preplan conductor activation will silently skip cross-feature context loading — a behavioral gap that is not covered by any of the proposed parity tests.

**Options:**
- **A** — Add a prerequisite verification step to Sprint 1 (PP-1.1 or a new PP-1.0): confirm `fetch-context` status in the new codebase before writing conductor code that depends on it.
- **B** — Add `fetch-context` availability as an open question in the tech plan with a note that the step may need to be guarded by a capability check at runtime.
- **C** — Exclude `fetch-context` from the parity scope explicitly and add it as an open question in the business plan (following the same pattern as `new-feature` used for its `fetch-context` deferral).
- **D** — Assume it works and let the integration test catch it if it doesn't.
- **E** — Add a failing test for `fetch-context` in PP-1.3 to surface the status early in Sprint 1.

**Recommended:** C or E — the `new-feature` feature already set the pattern for deferring `fetch-context` with explicit documentation; following that pattern provides consistency.

---

### Finding F-4 — Clean-Room Verification Mechanism Unspecified [LOW]

**Location:** `business-plan.md` — Success Criteria

**Description:** The clean-room assurance criterion states "Implementation and docs are authored from the behavioral specification; no old-codebase files are copied." There is no named verification step or gate. Without an explicit mechanism — such as a code review checklist item, a diff comparison, or a reviewer sign-off in the story definition of done — this criterion is aspirational rather than measurable.

**Options:**
- **A** — Add "code reviewer confirms no old-codebase source files appear in PR diff" as a specific definition-of-done item in the sprint-plan's Definition of Done section.
- **B** — Add a CI gate that checks for known old-codebase file hash patterns in PR diffs.
- **C** — Treat it as an honor-system reviewer responsibility with no formal gate.
- **D** — Remove the criterion from the success criteria table since it cannot be automated.
- **E** — Add it as a PR review checklist step in the relevant sprint stories (PP-2.1 through PP-3.3) and in the overall Definition of Done.

**Recommended:** A or E — a simple PR review checklist item is proportionate to the risk.

---

## Party-Mode Blind-Spot Challenge

### Winston (Architect) Perspective

The tech plan correctly defers governance writes to businessplan via ADR 5. However, it does not address in-flight state: if a preplan session is interrupted after artifact authoring but before phase completion, staged artifacts accumulate in the docs path across sessions. The next preplan invocation will find partial artifacts and the review-ready check may return `status=fail` or misleadingly `status=pass` depending on what accumulated. A cleanup or idempotency contract should be added to the tech plan, or an explicit note that in-flight accumulation is acceptable and handled by the review-ready check's per-artifact validation.

### John (PM) Perspective

The scope section states `bmad-lens-init-feature` is used for cross-feature context loading but does not confirm this skill is available and callable from the new codebase's preplan activation chain. If `bmad-lens-init-feature` is not yet fully implemented in `lens.core.src`, calling it during preplan activation will fail silently or throw. This is an implicit external dependency that should appear in the sprint plan's prerequisite table or the tech plan's dependency map with an explicit availability note.

---

## Verdict Summary

| Finding | Severity | Status |
|---|---|---|
| F-1: Test location inconsistency | Medium | Warning — resolve in PP-1.2/PP-1.3 |
| F-2: Missing `/next` pre-confirmed handoff | Medium | Warning — resolve in BP and TP before Sprint 2 |
| F-3: fetch-context availability unverified | Low | Warning — decide scope by PP-1.1 |
| F-4: Clean-room verification unspecified | Low | Warning — add to Definition of Done |

**Overall Verdict: pass-with-warnings**

The business plan and tech plan are architecturally sound. The clean-room constraint is correctly framed, the shared-utility delegation is well-specified, and the dependency ordering reflects the baseline architecture requirements. The two medium findings are documentation gaps that can be resolved before Sprint 2 without rework. Advancing to FinalizePlan is permitted.
