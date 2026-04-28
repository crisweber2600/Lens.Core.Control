---
feature: lens-dev-new-codebase-businessplan
doc_type: finalizeplan-review
status: in-review
review_format: abc-choice-v1
phase: finalizeplan
verdict: pass-with-warnings
reviewer: lens-finalizeplan
updated_at: 2026-04-28T00:00:00Z
---

# FinalizePlan Adversarial Review — lens-dev-new-codebase-businessplan

**Phase:** finalizeplan  
**Scope:** business-plan.md, tech-plan.md, sprint-plan.md, expressplan-review.md  
**Method:** Blind Hunter + Edge Case Hunter + Acceptance Auditor + Party-Mode Challenge + Governance Cross-Check  
**Verdict:** pass-with-warnings  

---

## Context Summary

This feature rewrites `bmad-lens-businessplan` and `bmad-lens-techplan` as thin conductors that delegate shared patterns (batch intake, review-ready fast path, publish-before-author) to canonical shared utilities. The expressplan review (`pass-with-warnings`) is incorporated as prior art; its findings (BP-1/BP-2 branching ambiguity, `/next` path, SKILL.md template, pre-sprint checklist) are all resolved or addressed. This FinalizePlan review examines the combined planning set with fresh adversarial eyes at higher altitude — scope completeness, governance entanglement, acceptance criteria strength, and sequencing risks.

---

## Findings

---

### Finding FP-1 — `lens-dev-new-codebase-techplan` scope overlap in governance

**Severity:** High  
**Layer:** Governance Cross-Check (Sensing)  

**Observed:** A separate registered feature `lens-dev-new-codebase-techplan` (status: `preplan`, track: `full`) exists in `feature-index.yaml`. It was split from `lens-dev-new-codebase-baseline` and represents a dedicated techplan command rewrite feature. This feature's business-plan.md explicitly states "Both businessplan and techplan rewrites are scoped in this feature given their hard dependency chain" — meaning BP-2 in this sprint also rewrites the techplan command. Without explicit governance reconciliation, `lens-dev-new-codebase-techplan` remains active as a competing scope owner for the same deliverable.

**Risk:** If `lens-dev-new-codebase-techplan` is ever picked up by another session or another contributor, it would produce a duplicate or conflicting techplan rewrite against work already delivered by BP-2 in this sprint.

**Options:**

- **A** — Mark `lens-dev-new-codebase-techplan` as `superseded` in `feature-index.yaml` and `feature.yaml`; add a `superseded_by: lens-dev-new-codebase-businessplan` note. This is the most complete resolution — one canonical scope owner, clear governance trail.
- **B** — Mark `lens-dev-new-codebase-techplan` as `archived`; no superseded_by link. Simpler but loses the provenance trail for future audits.
- **C** — Keep `lens-dev-new-codebase-techplan` active as a future placeholder for techplan capabilities beyond parity (scope not yet defined). Accept the overlap risk with a comment in its feature.yaml.
- **D** — Split BP-2 out of this feature and move it to `lens-dev-new-codebase-techplan`. Preserves clean per-feature scope but breaks the hard dependency chain rationale from the business plan.
- **E** — No action needed; the feature-index is considered a draft registry and risk is acceptable.

**Decision: A** — Mark `lens-dev-new-codebase-techplan` as superseded by this feature. The business-plan rationale for co-scoping is sound (shared conductor pattern, tightly coupled dependency chain). The governance trail is more valuable than leaving an orphaned feature in `preplan` indefinitely.

**Action Item:** Before committing finalizeplan artifacts, update `feature-index.yaml` and the `lens-dev-new-codebase-techplan` feature.yaml to `status: superseded` with `superseded_by: lens-dev-new-codebase-businessplan`.

---

### Finding FP-2 — Dependency verification gap before BP-1

**Severity:** Medium  
**Layer:** Edge Case Hunter  

**Observed:** The sprint plan pre-sprint checklist requires three baseline stories to be `done` before BP-1 may start (1.4 publish-to-governance hook, 3.1 constitution partial hierarchy fix, 4.1 preplan rewrite). The checklist references `#prompt:lens-preflight.prompt.md` as the verification step. However, the `lens-dev-new-codebase-baseline` feature shows status `preplan` in `feature-index.yaml` — its story completion state is not governance-visible. A developer running preflight could receive a false-negative if preflight doesn't inspect target-repo story merge state for the baseline feature.

**Risk:** BP-1 starts against an incomplete dependency surface, causing rework when the shared utilities it depends on don't exist or behave differently.

**Options:**

- **A** — Add a governance-scoped dependency check to this feature's `feature.yaml` `depends_on` list, naming the three baseline stories explicitly. Preflight can then surface dependency gaps at planning-PR time.
- **B** — Accept the preflight verification step as-is; add an explicit note to BP-1 Implementation Notes that the developer must confirm target-repo story merge state (not just feature phase state) before starting.
- **C** — Block BP-1 via a conditional story: "Story 0 — Dependency Gate" that is marked done only when baseline stories 1.4, 3.1, 4.1 are confirmed merged in target repo.
- **D** — Defer to the developer; pre-sprint checklist is advisory only.
- **E** — No action needed.

**Decision: B** — Accept preflight as the verification mechanism. Add a clarifying note to BP-1 Implementation Notes that preflight must confirm target-repo story _merge_ state for baseline dependencies, not just feature phase status. Creating a dependency gate story (C) is scope increase; the preflight prompt covers the intent.

**Action Item:** Update BP-1 Implementation Notes in `sprint-plan.md` to add: "Preflight must confirm target-repo merge state for stories 1.4, 3.1, and 4.1 — not just feature-index phase status — before BP-1 begins."

---

### Finding FP-3 — `feature-index.yaml` stale status for this feature

**Severity:** Low  
**Layer:** Governance Cross-Check  

**Observed:** `feature-index.yaml` shows `lens-dev-new-codebase-businessplan` as `status: preplan, track: full`. The actual state is `expressplan-complete, express`. The stale entry would cause any governance tooling that queries the index for active express-track features to miss this one, and would cause preflight to show a false preplan state.

**Options:**

- **A** — Update `feature-index.yaml` and `feature.yaml` via `bmad-lens-feature-yaml` to reflect actual state (`expressplan-complete`, track `express`) before the planning PR is opened.
- **B** — Accept stale state; feature.yaml in the governance mirror is the authoritative state source; feature-index is a convenience cache.
- **C** — Update only feature.yaml; leave feature-index for the next full index rebuild.
- **D** — No action needed.
- **E** — No action needed.

**Decision: A** — Update both `feature.yaml` and `feature-index.yaml` before the planning PR. The index should reflect current state — governance tooling and sensing depend on it being accurate.

**Action Item:** Run `bmad-lens-feature-yaml` to update `feature.yaml` phase to `finalizeplan` and update `feature-index.yaml` status to `finalizeplan` / track `express`.

---

### Finding FP-4 — Architecture-reference regression test scope ambiguity

**Severity:** Low  
**Layer:** Acceptance Auditor  

**Observed:** BP-3 regression gate includes an `architecture-reference` category: "PRD reference enforcement — architecture.md references prd.md." This is correct for full-track techplan invocations. However, the express-track does not produce a `prd.md` — it produces `business-plan.md` and `tech-plan.md`. If BP-3 regression tests run against the express-track artifact set, the architecture-reference check would either false-fail (no prd.md present) or false-pass (prd.md not required in express track). The test scope must be constrained to full-track invocations.

**Options:**

- **A** — Add a clarifying note to BP-3: "Architecture-reference regression applies to full-track techplan invocations only. Express-track does not produce prd.md; this test category is skipped for express-track test scenarios."
- **B** — Add a separate express-track regression scenario to BP-3 that verifies architecture (or tech-plan) references the business-plan.md when PRD is not present.
- **C** — Accept the ambiguity; test authors will know the context.
- **D** — No action needed.
- **E** — No action needed.

**Decision: A** — Add the clarifying scope note to BP-3. Option B (separate express-track regression scenario) is a good future enhancement but is scope expansion for this sprint.

**Action Item:** Update BP-3 Implementation Notes in `sprint-plan.md` to clarify architecture-reference regression scope is full-track only.

---

### Finding FP-5 — Downstream unblock signal for stories 4.4 and 4.5 undefined

**Severity:** Low  
**Layer:** Blind Hunter  

**Observed:** The business-plan and tech-plan both correctly document that this feature blocks baseline stories 4.4 (finalizeplan rewrite) and 4.5 (expressplan rewrite). However, neither document defines the signal or trigger that unblocks them. In practice, a developer starting story 4.4 needs to know: "What must be true in the target repo for this feature to be considered 'done enough'?" The current answer is implicit (BP-1 and BP-2 merged to develop), but it is not stated anywhere.

**Options:**

- **A** — Add an "Unblocking Signal" section to the business-plan: "Stories 4.4 and 4.5 may begin when BP-1 and BP-2 are merged to develop in lens.core.src AND the regression gate (BP-3) passes." Update `feature.yaml` dependency links accordingly.
- **B** — Add an unblocking signal note to BP-3 Implementation Notes only.
- **C** — Add the dependency link in `feature-index.yaml` related_features block for stories 4.4 and 4.5.
- **D** — Leave it to the developer; the `blocks` field in the docs is sufficient.
- **E** — No action needed.

**Decision: B** — Add an unblocking signal note to BP-3 Implementation Notes. Updating the business-plan (A) is higher overhead and the sprint-plan is where implementation sequencing lives.

**Action Item:** Update BP-3 Implementation Notes in `sprint-plan.md` to add: "On BP-3 green: this unblocks baseline stories 4.4 (finalizeplan rewrite) and 4.5 (expressplan rewrite) in lens.core.src. Signal the baseline feature lead when BP-3 passes."

---

## Party-Mode Blind Spot Challenge

The following challenge questions were posed in the party-mode round. No new findings were raised.

**Q1 (Winston — Architect):** The thin conductor SKILL.md pattern assumes `validate-phase-artifacts.py` always returns `status=pass` or `status=fail`. Is there a third state (e.g., `status=warning`) that could bypass the fast-path logic?  
→ No: `validate-phase-artifacts.py` emits only `pass`/`fail`; the SKILL.md design is clean.

**Q2 (John — PM):** Does the success criterion "no direct governance writes" have an observable test that isn't just "we read the code"?  
→ Yes: governance-audit regression category in BP-3 explicitly tests this via instrumented run detection (tech-plan §5.2). Already accounted for.

**Q3 (Mary — Analyst):** The expressplan track for this feature and the full track that other features use are both producing artifacts into the same control-repo docs path. Could a full-track follow-on feature overwrite expressplan artifacts?  
→ Not relevant for this feature (no follow-on planned); but the observation is valid for the governance docs path design generally. Logged as a future constitution enhancement, not a blocker here.

**Q4 (Sally — UX):** Neither command (businessplan nor techplan) has UX or interaction documentation. Are the prompts themselves considered the UX?  
→ Correct: the prompts are the user-facing surface; no separate UX spec is required for internal tooling commands. Out of scope.

---

## Governance Impact Summary

| Impact | Severity | Decision |
|--------|----------|----------|
| `lens-dev-new-codebase-techplan` active with overlapping scope | High | A — Mark superseded before planning PR |
| `feature-index.yaml` stale status for this feature | Low | A — Update to `finalizeplan`/`express` before planning PR |
| Baseline dependency state not governance-visible | Medium | B — Clarify BP-1 notes; preflight confirms merge state |

---

## Required Actions Before Planning PR

1. Mark `lens-dev-new-codebase-techplan` as `superseded` in governance (FP-1)
2. Update this feature's `feature-index.yaml` and `feature.yaml` to `finalizeplan`/`express` (FP-3)

## Required Actions Before Dev-Ready

3. Update BP-1 Implementation Notes — preflight must confirm target-repo merge state (FP-2)
4. Update BP-3 Implementation Notes — architecture-reference scope note (FP-4)
5. Update BP-3 Implementation Notes — unblocking signal for stories 4.4 and 4.5 (FP-5)

---

## Verdict Summary

| Finding | Severity | Decision | Action |
|---------|----------|----------|--------|
| FP-1 — lens-dev-new-codebase-techplan scope overlap | High | A | Mark superseded in governance before planning PR |
| FP-2 — Dependency verification gap | Medium | B | Update BP-1 notes re: preflight confirms merge state |
| FP-3 — feature-index.yaml stale status | Low | A | Update feature.yaml + feature-index.yaml before planning PR |
| FP-4 — Architecture-reference regression scope | Low | A | Add scope clarification to BP-3 notes |
| FP-5 — Unblock signal undefined | Low | B | Add unblocking signal note to BP-3 |

**Overall verdict: pass-with-warnings**

One high-severity governance finding (FP-1 scope overlap) and one medium-severity dependency risk (FP-2) require action before planning PR opens. Neither is a blocker to proceeding — both are resolved by targeted governance updates and a documentation note. All expressplan artifacts are well-formed, the thin conductor pattern is sound, and the regression coverage plan is comprehensive. Ready to proceed to planning PR after governance actions in FP-1 and FP-3 are applied.
