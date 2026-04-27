---
feature: lens-dev-new-codebase-new-feature
doc_type: finalizeplan-review
phase: finalizeplan
source: phase-complete
verdict: pass-with-warnings
status: draft
critical_count: 0
high_count: 1
medium_count: 3
low_count: 1
updated_at: 2026-04-27T00:00:00Z
review_format: abc-choice-v1
---

# FinalizePlan Review — lens-dev-new-codebase-new-feature

**Reviewed:** 2026-04-27T00:00:00Z  
**Source:** phase-complete (party-mode challenge + governance cross-check)  
**Artifacts Reviewed:** business-plan.md, tech-plan.md, expressplan-review.md, sprint-plan.md  
**Overall Verdict:** **pass-with-warnings**

---

## How to Respond

For each finding below, reply with the letter of your chosen option. You may add a clarifying note after the letter. Your response is recorded and drives the carry-forward action for that finding.

| Option | Meaning |
|---|---|
| A / B / C | Accept the proposed resolution (with its stated trade-offs) |
| D | You will provide a custom resolution — write it after "D:" |
| E | Explicitly no action — this finding is accepted as-is |

---

## Finding Summary

| ID | Severity | Title | Your Response |
|---|---|---|---|
| H1 | High | `fetch-context` parity scope remains a gating open question | |
| M1 | Medium | Shared script coordination with `lens-dev-new-codebase-new-service` | |
| M2 | Medium | Constitution compliance — BMB-first rule not addressed in sprint stories | |
| M3 | Medium | Sprint 4 sizing risk if `fetch-context` remains in scope | |
| L1 | Low | Quickplan alias parity test not called out in acceptance criteria | |

---

## Prior Review Resolution

| Finding | Prior Verdict | Resolution |
|---------|--------------|-----------|
| H1 — Phase metadata mismatch (full/preplan with expressflow artifacts) | High | **Resolved.** Expressflow exception documented; feature.yaml advanced to `techplan` via governance update (commit `683c5ed`) with explicit `expressflow_exception` note and phase transition records. |
| H2 — `fetch-context` parity open | High | **Carried forward as H1 below.** Sprint NF-4.3 addresses it but the scope decision is still open. |
| M1 — Shared initializer regression risk | Medium | **Mitigated in sprint plan.** NF-1.3 adds parity test skeletons; cross-sprint dependency chain ensures tests run first. |
| M2 — Cross-feature dependency on git-orchestration and switch | Medium | **Addressed.** ADR 4 explicitly delegates branch creation to git-orchestration; returned command strings are tested without execution. |
| M3 — Help/manifests ownership unclear | Medium | **Carried.** NF-4.2 defers with "if owned here" qualifier. Acceptable for this delivery slice. |
| L1 — Legacy quickplan alias | Low | **Addressed.** ADR 3 resolves track aliases from lifecycle.yaml, avoiding hardcoded routing. |

---

## Final Planning Review

### Critical

| # | Dimension | Finding |
|---|-----------|---------|
| — | — | No critical blockers found in the combined planning artifact set. Continue. |

---

### High

---

#### H1 — `fetch-context` parity scope remains a gating open question

**Location:** sprint-plan.md (NF-4.3), Definition of Done  
**Gate:** Must be resolved before dev starts

NF-4.3 defers the scope decision on `fetch-context` parity. The Definition of Done ("all focused initializer tests pass") can technically be satisfied without fetch-context if the decision is deliberately recorded. The risk is that downstream feature planners (e.g., `/businessplan` callers) may silently depend on context output that this delivery will not produce.

**Choose one:**

- **A.** Explicitly defer `fetch-context` — update the Definition of Done to say "fetch-context is out of scope in this slice"; record the defer decision in implementation notes; NF-4.3 becomes a spike or backlog item.  
  **Why pick this:** Closes the ambiguity; any downstream caller gets a clear "not implemented" signal; sprint 4 scope is immediately clarified.  
  **Why not:** If fetch-context is a silent dependency for `/businessplan` callers, deferred delivery may create a gap that is not caught until downstream planners invoke it.

- **B.** Include `fetch-context` in scope — update NF-4.3 with explicit acceptance criteria for full fetch-context parity; commit to this as a delivery requirement.  
  **Why pick this:** Eliminates any downstream dependency surprise; the feature delivers true initializer parity; no follow-up feature required.  
  **Why not:** Sprint 4 is already the heaviest sprint; adding fetch-context in scope risks a sprint slip that blocks the entire delivery.

- **C.** Create a follow-up feature entry for `fetch-context` — record an explicit new feature or backlog entry before dev-start; this delivery is explicitly "initializer parity minus fetch-context."  
  **Why pick this:** Honest scope boundary; the follow-up is tracked and actionable; downstream planners can reference the follow-up feature.  
  **Why not:** If the follow-up feature is never prioritized, the gap persists indefinitely; requires creating and maintaining a separate governance entry.

- **D.** Write your own response.
- **E.** Keep as-is — leave fetch-context scope undefined in the Definition of Done; resolve at NF-4.3 implementation time.

---

### Medium

---

#### M1 — Shared script coordination with `lens-dev-new-codebase-new-service`

**Location:** sprint-plan.md (all sprints), tech-plan.md (init-feature-ops.py)  
**Context:** `lens-dev-new-codebase-new-service` is at `expressplan-complete` and will also extend `init-feature-ops.py`.

Both `new-feature` and `new-service` will modify the same script in separate dev branches, creating a near-certain merge conflict window.

**Choose one:**

- **A.** Assign PR ordering now — designate `new-service` as the merge-base; `new-feature` dev branch rebases on top of the `new-service` merge before opening its PR.  
  **Why pick this:** Eliminates the conflict by design; `new-service` is closer to dev-start and the ordering is documented before work begins.  
  **Why not:** If `new-service` slips, `new-feature` is blocked from merging until `new-service` lands; creates a hard sequencing dependency.

- **B.** Coordinate via shared feature branch — create a temporary integration branch that both features target; merge from integration to main after both land.  
  **Why pick this:** Allows both features to develop in parallel; the integration branch absorbs the conflict; no ordering dependency.  
  **Why not:** Integration branches require additional governance overhead; conflict resolution still happens, just at a different point.

- **C.** Defer coordination to dev-start — record this as an action item; assign an owner to coordinate before both features enter the same sprint.  
  **Why pick this:** Low overhead now; the action item is visible and tracked; coordination happens when the timeline is concrete.  
  **Why not:** If both features enter dev without coordination, the conflict materializes and blocks one PR; deferral risks the issue becoming urgent rather than planned.

- **D.** Write your own response.
- **E.** Keep as-is — accept the merge conflict risk; resolve when it occurs.

---

#### M2 — Constitution compliance — BMB-first rule not addressed in sprint stories

**Location:** sprint-plan.md (NF-1.x through NF-4.x), new-codebase service constitution

The new-codebase service constitution requires edits to `lens.core.src` to go through the `lens.core/_bmad/bmb` module (BMB-first rule). The sprint plan has no story for BMB scaffolding or module-routed implementation.

**Choose one:**

- **A.** Audit sprint stories for `lens.core.src` direct edits — before dev starts, identify which NF-* stories involve direct edits vs. adding new files; if direct edits are required, add a BMB-routed scaffolding step to the affected story.  
  **Why pick this:** Targeted; only adds overhead to stories that actually touch `lens.core.src` directly; avoids blanket BMB scaffolding if none is needed.  
  **Why not:** If the audit reveals multiple direct-edit stories, the scaffolding cost may be higher than anticipated; sprint plan may need re-sequencing.

- **B.** Document a BMB-first exception for this feature — confirm with the lead that the new-feature implementation will NOT directly edit `lens.core.src`; record the scope boundary in implementation notes.  
  **Why pick this:** If the feature only adds new files, no BMB scaffolding is needed; the exception is documented and auditable.  
  **Why not:** If the scope boundary is wrong (some stories do require direct edits), the exception creates a compliance gap discovered late in dev.

- **C.** Add a BMB scaffolding story to Sprint 1 — regardless of which stories need it, scaffold the BMB-routed module path in Sprint 1 so all subsequent stories can route through it.  
  **Why pick this:** Proactive compliance; the scaffolding is done once and benefits all subsequent stories; no story-level audit required.  
  **Why not:** Adds a Sprint 1 story that may be unnecessary if no direct `lens.core.src` edits are actually needed; adds sprint cost speculatively.

- **D.** Write your own response.
- **E.** Keep as-is — accept that BMB-first compliance is informational; if flagged during compliance review, address it then.

---

#### M3 — Sprint 4 sizing risk if `fetch-context` remains in scope

**Location:** sprint-plan.md (Sprint 4: NF-4.1, NF-4.2, NF-4.3)

Sprint 4 carries three M-sized stories (NF-4.1 governance git execution, NF-4.2 help/manifests, NF-4.3 fetch-context), all with hard dependencies on earlier sprints. If fetch-context is fully deferred, NF-4.3 becomes a spike; if it is in scope, Sprint 4 is the heaviest sprint.

**Choose one:**

- **A.** Re-scope Sprint 4 at dev-start after H1 is resolved — once the fetch-context decision is recorded, immediately re-sequence Sprint 4; if NF-4.3 is deferred, redistribute the freed capacity to NF-4.1 or NF-4.2.  
  **Why pick this:** Keeps the sprint plan accurate to actual scope; the H1 decision drives Sprint 4 re-scoping automatically.  
  **Why not:** Re-scoping at dev-start requires a planning session before Sprint 4; adds overhead at a phase transition point.

- **B.** Split NF-4.2 into a separate follow-up — if the 17-command sweep owns help/manifests registration, defer NF-4.2 to a sweep feature now; Sprint 4 carries only NF-4.1 and NF-4.3.  
  **Why pick this:** Reduces Sprint 4 to two stories regardless of the fetch-context decision; removes the ownership ambiguity proactively.  
  **Why not:** If this feature does own help/manifests, splitting it out creates an extra governance artifact and tracking overhead for one story.

- **C.** Accept Sprint 4 as currently scoped — proceed with three M-sized stories; re-scope only if sprint 4 is projected to slip during execution.  
  **Why pick this:** No pre-emptive overhead; the risk may not materialize; many M-sized stories complete faster than estimated.  
  **Why not:** All three NF-4.x stories have hard upstream dependencies; if any Sprint 3 story slips, Sprint 4 has no buffer and the entire sprint may slip.

- **D.** Write your own response.
- **E.** Keep as-is — accept the Sprint 4 sizing risk; address during sprint execution.

---

### Low

---

#### L1 — Quickplan alias parity test not called out in acceptance criteria

**Location:** sprint-plan.md (NF-1.3, NF-3.1), tech-plan.md (ADR 3)  
**Context:** ADR 3 resolves track aliases from lifecycle.yaml; other tracks (full, express) have explicit acceptance criteria.

The plan confirms ADR 3 resolves track aliases from lifecycle.yaml, but no specific test skeleton is called out for the quickplan alias path. Other tracks have explicit acceptance criteria; quickplan does not.

**Choose one:**

- **A.** Add a quickplan-alias parity test to NF-1.3 — explicitly include a test skeleton that verifies the alias routes correctly; the test documents the expected behavior.  
  **Why pick this:** Closes the coverage gap; the test doubles as documentation; consistent with how other tracks are covered.  
  **Why not:** If the alias is confirmed removed in the new codebase, the test will be written only to be deleted; adds short-lived test maintenance overhead.

- **B.** Explicitly remove the quickplan alias in the Definition of Done — document that the new codebase does not retain `quickplan` as an alias; users are expected to use `express` directly.  
  **Why pick this:** Deliberate behavior delta is better than an undocumented removal; makes the change visible in the DoD.  
  **Why not:** If any existing users rely on the quickplan alias, this commits to removing it without a migration notice or deprecation path.

- **C.** Confirm alias status in lifecycle.yaml before Sprint 1 — verify whether `quickplan` is listed in lifecycle.yaml; if present, add the parity test (option A); if absent, document the removal (option B).  
  **Why pick this:** Data-driven; the test or removal decision is made based on the actual lifecycle.yaml state, not assumption.  
  **Why not:** Requires a pre-sprint confirmation step; if lifecycle.yaml has not been updated for the new codebase, the confirmation may be provisional.

- **D.** Write your own response.
- **E.** Keep as-is — accept the missing quickplan test; address if the alias path is confirmed retained.

---

## Party-Mode Challenge Round

The following challenge angles were applied to surface blind spots not caught by standard review.

### Devil's Advocate — "What if it ships and the initializer is wrong?"

**Challenge:** The entire feature hangs on `init-feature-ops.py create` producing bit-identical governance outputs to the old initializer. If the canonical ID formula or the feature-index entry shape drifts by a single field, every downstream command that reads feature.yaml will silently get different data. The parity tests in NF-1.3 are planned but not written. What is the fallback if late test discovery reveals the schema is more complex than expected?

**Finding:** The sprint plan sequences parity test skeletons before implementation (Sprint 1 before Sprint 2), which is correct. However, there are no acceptance criteria for the test *passing* in Sprint 2 — only that the skeleton exists in Sprint 1. If the skeleton exists but the tests remain red by Sprint 2 completion, the sequencing assumption breaks.

**Recommended action:** NF-2.1 acceptance criteria should explicitly state that the parity tests written in NF-1.3 are green before NF-2.1 is accepted, not just that implementation is authored.

### Skeptic — "The expressplan output isn't equivalent to full planning"

**Challenge:** This feature was planned with a compressed expressflow and the prior review flagged it as an H1. The phase mismatch is now documented, but the artifacts still differ from what a true full-track planning pass would produce: there is no preplan brainstorm, no PRD, no UX design, and no architecture document. The constitution requires `business-plan` (present) and `stories` (not yet present). The missing artifacts are explicitly out of scope for the full track's planning phases — but the expressflow exception does not explain why those standard phases were skipped.

**Finding:** The constitution requirement (`business-plan` + `stories`) is satisfied by the presence of business-plan.md and the sprint plan's story list. The missing preplan artifacts (brainstorm, research, product-brief) are acceptable omissions for a baseline-split feature. This is not a blocker. The expressflow exception rationale should appear in the implementation notes.

**Recommended action:** Add one sentence to the implementation notes (or to the feature.yaml description) explaining why preplan artifacts were not produced for this split feature. This satisfies the auditable rationale requirement.

### Forecaster — "What comes right after this?"

**Challenge:** FinalizePlan opens the feature PR to main. After that, `/dev` starts. What does the first dev session actually need to do, and is the sprint plan specific enough to guide it without further planning?

**Finding:** Sprint 1 is well-specified: add command prompt surfaces (stub + release prompt), expand skill contract, add parity test skeletons. These are low-ambiguity tasks that a dev session can execute directly. The dependency on `lens-dev-new-codebase-new-service` dev ordering is the most likely first-day blocker (see M1 above). Otherwise the sprint plan provides adequate guidance.

**No additional action required** beyond the M1 merge-ordering recommendation.

---

## Governance Cross-Check

### Impacted Services and Features

| Feature | Relationship | Risk | Action |
|---------|-------------|------|--------|
| `lens-dev-new-codebase-new-domain` | Shares `bmad-lens-init-feature`; currently **complete** | Low — base is stable | No action; reuse as behavioral reference |
| `lens-dev-new-codebase-new-service` | Shares `bmad-lens-init-feature`; at **expressplan-complete** | **High** — concurrent script modifications | Coordinate dev PR ordering (see M1) |
| `lens-dev-new-codebase-baseline` | Parent split; defines retained command surface | Low — informational | Confirm `new-feature` is listed in retained 17-command surface |
| `lens-dev-new-codebase-switch` | Used for activation handoff in returned commands | Medium — parity depends on switch being operational | Validate switch command is implemented before integration tests |
| `lens-dev-new-codebase-preflight` | `light-preflight.py` is called by the installed stub | Low — preflight is working (passes in CI) | No action |

### Related Governance Docs

- The `bugs.md` / `bugfixes.md` entry recently added to `features/lens-dev/new-codebase/` was pulled in during the preflight sync and does not affect this feature.
- No cross-domain impacts found for `lens-dev-new-codebase-new-feature`. The command creates governance files under the calling user's domain/service context, not under `lens-dev/new-codebase`.

---

## Action Items Before Dev Start

| # | Owner | Action | Priority |
|---|-------|--------|---------|
| A1 | Lead | Decide fetch-context scope; update Definition of Done before dev-start | High |
| A2 | Lead | Coordinate PR merge order with `lens-dev-new-codebase-new-service` | High |
| A3 | Lead | Add NF-2.1 acceptance criteria: parity tests from NF-1.3 must be green | Medium |
| A4 | Lead | Add one-line preplan-skip rationale to feature.yaml or implementation notes | Low |
| A5 | Lead | Confirm BMB-first rule scope (informational gate vs. blocking) for sprint stories | Medium |

---

## Accepted Risks

| Risk | Rationale for Acceptance |
|------|--------------------------|
| fetch-context not in scope | Deferral is deliberate and tracked in NF-4.3; downstream planners can file a follow-up |
| No preplan artifacts | Split-from-baseline features may bypass preplan with documented rationale |
| Quickplan alias not explicitly tested | Acceptable if the alias is confirmed removed or covered by track-resolution tests |
