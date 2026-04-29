---
feature: lens-dev-new-codebase-trueup
doc_type: adversarial-review
phase: businessplan
source: phase-complete
verdict: pass-with-warnings
review_format: abc-choice-v1
reviewed_at: 2026-04-28T01:46:22Z
---

# Adversarial Review: lens-dev-new-codebase-trueup / businessplan

## Selected response

**B — Pass with warnings**

## Response options

- **A — Pass:** Artifacts are complete and coherent; proceed with no material concerns.
- **B — Pass with warnings:** Artifacts support proceeding, but specific risks or gaps must be carried forward and addressed explicitly.
- **C — Needs revision:** Artifacts are directionally useful but require substantive revision before the next phase.
- **D — Major rework required:** Artifacts have significant structural or evidence gaps that block reliable planning.
- **E — Reject:** Artifacts are not sufficient to support phase progression.

---

**Reviewed:** 2026-04-28T01:46:22Z  
**Source:** phase-complete  
**Overall Rating:** pass-with-warnings

## Summary

The revised BusinessPlan artifact set is now materially stronger than the first draft. The PRD explicitly promotes `fetch-context` and `read-context` into the old init-feature contract, treats their absence as a functional regression, fixes the `bmad-lens-complete` test scaffold path, and separates audit evidence gathering from later governance writes. The `ux-design.md` N/A declaration is acceptable for this feature because True Up has no end-user UI surface and the only interaction surface it touches is prompt discoverability.

The remaining issues are no longer phase-blocking. They are governance and execution-channel warnings that should be carried into TechPlan: the PRD does not yet encode the mandatory BMad Builder and BMB-first implementation channel required by the active domain and service constitutions; it does not explicitly declare whether `feature.yaml` phase or `feature-index.yaml` status is authoritative when the two disagree; and the `fetch-context` regression is correctly elevated to a requirement but still needs an explicit old-contract acceptance target so TechPlan cannot underspec the restoration. BusinessPlan can move forward, but these warnings should be preserved as explicit implementation constraints.

## Findings

### Critical

*(None)*

### High

---

#### H1 — BMB Implementation Channel Not Named in BusinessPlan

**Dimension:** Coverage Gaps  
**Severity:** High  
**Finding:** The PRD scopes multiple edits under the Lens source surface: prompt stubs, `SKILL.md` files, reference docs, and test scaffolds. The active constitutions for `lens-dev` and `lens-dev/new-codebase` require two things for this kind of work: agents must consult the BMad Builder reference index in governance external docs, and lens-work source changes must use the BMB implementation channel. The BusinessPlan does not currently mention either requirement, which creates a direct compliance gap for the very work it proposes.  
**Recommendation:** Add an explicit implementation-channel requirement before TechPlan begins: any work on `lens.core/_bmad/lens-work/` or the new-codebase `lens.core.src` equivalent must load the BMad Builder reference index and route through the appropriate BMB workflow/skill. Treat this as a TechPlan constraint, not an optional note.

**Finding response options:**
- A) Add BMB implementation-channel requirement before TechPlan begins; treat as hard constraint ✅
- B) Document as advisory; rely on developer judgment
- C) Carry forward to TechPlan as an implementation constraint
- D) Write your own response
- E) Keep as-is — accept finding without action

**Selected: A — TechPlan implementation channel constraint documented in architecture.md §2.3.**

---

### Medium

---

#### M1 — `feature.yaml` vs `feature-index.yaml` Source of Truth Undefined

**Dimension:** Logic Flaws  
**Severity:** Medium  
**Finding:** The review packet surfaced a live governance nuance: the same new-codebase features show `status` values like `preplan` or `archived` in `feature-index.yaml`, while their per-feature `feature.yaml` files carry lifecycle `phase` values such as `finalizeplan-complete`. The PRD now correctly focuses on governance label verification, but it still does not state which document is authoritative when those two disagree. That ambiguity matters because the parity audit and later blocker annotations could update the wrong record.  
**Recommendation:** Add a short source-of-truth note in TechPlan or the parity audit report: `feature.yaml.phase` is authoritative for lifecycle state; `feature-index.yaml.status` is a registry summary and must not be treated as the phase gate source of truth.

**Finding response options:**
- A) Add source-of-truth clarification: `feature.yaml.phase` is authoritative for lifecycle state ✅
- B) Define governance hierarchy in the ADR for constitution tracks (FR-7)
- C) Carry forward to TU-4.1 parity audit as an explicit implementation note
- D) Write your own response
- E) Keep as-is — accept finding without action

**Selected: A — Documented in architecture.md §5.2 and parity-audit acceptance criteria.**

---

#### M2 — `fetch-context` / `read-context` Restoration Lacks Output Contract

**Dimension:** Cross-Feature Dependencies  
**Severity:** Medium  
**Finding:** The PRD correctly elevates the missing `fetch-context` and `read-context` commands into a required finding, but the acceptance target is still mostly narrative. The old codebase defines a concrete CLI contract for `fetch-context` (`related`, `depends_on`, `blocks`, `context_paths`, service-ref support) and `read-context` (`domain`, `service`, `updated_at`, `updated_by`). Without naming that output contract as the restoration target, TechPlan could still produce an underspecified replacement and call the regression closed.  
**Recommendation:** Extend the parity audit report or TechPlan acceptance criteria with the old `fetch-context` / `read-context` output contract as the measurable restoration target. Presence/absence alone is not enough for parity.

**Finding response options:**
- A) Extend parity audit AC / TechPlan with the old `fetch-context`/`read-context` output contract as restoration target ✅
- B) Add as a TechPlan architecture section on old-contract restoration targets
- C) Accept as-is; rely on parity audit to surface specifics
- D) Write your own response
- E) Keep as-is — accept finding without action

**Selected: A — Output contracts documented in architecture.md §6.**

---

#### M3 — ADR Framing for Constitution Tracks Ignores Live Governance State

**Dimension:** Assumptions and Blind Spots  
**Severity:** Medium  
**Finding:** FR-7 frames the constitution `permitted_tracks` divergence as a fresh ADR decision, but the active org, domain, and service constitutions already permit `express` and `expressplan`. The real question is not whether the tracks exist in the abstract; it is whether the current constitutions should be confirmed as canonical or superseded by a tighter template. As written, the ADR framing risks ignoring current governance reality and re-litigating a decision that is already live.  
**Recommendation:** Reframe the ADR in TechPlan as "confirm or supersede the currently active constitution state," using the current org/domain/service constitutions as the starting point.

**Finding response options:**
- A) Reframe ADR-2 (FR-7) as "confirm or supersede current constitution state" ✅
- B) Remove FR-7 and treat current constitution state as final
- C) Accept as-is; ADR can reference current state as starting point
- D) Write your own response
- E) Keep as-is — accept finding without action

**Selected: A — ADR-2 framed as confirm/supersede in architecture.md §5.**

---

#### M4 — `fetch-context` Absence Treated as Parity-Only, Not General Workflow Requirement

**Dimension:** Complexity and Risk  
**Severity:** Medium  
**Finding:** The review itself had to bypass the intended Auto-Context Pull route because the new-codebase surface does not implement `fetch-context`. This did not block the current feature because `trueup` has no explicit dependencies and nearby feature context was recoverable from governance reads, but the same workaround will not scale to all future planning features.  
**Recommendation:** Record in TechPlan that direct governance reads are only a temporary review-time fallback. Restoring `fetch-context` is still required for the general planning workflow, not just for `new-feature` parity on paper.

**Finding response options:**
- A) Record in TechPlan that direct governance reads are a temporary fallback; restore `fetch-context` for general workflow ✅
- B) Accept as-is; treat `fetch-context` restoration as a new-feature parity item only
- C) Defer to new-feature dev phase entirely
- D) Write your own response
- E) Keep as-is — accept finding without action

**Selected: A — Documented as architectural constraint in architecture.md §6.**

---

### Low

---

#### L1 — Governance Mirror Path Reflects Only Preplan Stage

**Dimension:** Assumptions and Blind Spots  
**Severity:** Low  
**Finding:** The governance mirror path for `lens-dev-new-codebase-trueup` currently contains only the preplan artifacts, while the businessplan artifacts exist only in the staged control-repo docs path. This is consistent with the lifecycle contract, but it means anyone reading governance alone will not yet see the updated PRD, UX declaration, or this review artifact.  
**Recommendation:** Keep the parity audit and subsequent TechPlan documents explicit about whether they are staged-only or governance-published. Do not assume the governance mirror reflects BusinessPlan until the publish step actually occurs.

**Finding response options:**
- A) Keep explicit staged-vs-governance-mirror distinction in TechPlan and parity audit artifacts ✅
- B) Accept as-is; governance mirror reflects lifecycle contract
- C) Publish businessplan artifacts to governance now
- D) Write your own response
- E) Keep as-is — accept finding without action

**Selected: A — Stage-vs-governance-mirror distinction documented in architecture.md §7.**

## Accepted Risks

*(None explicitly accepted in this review run. The warnings above should remain open until TechPlan incorporates them or the user accepts them.)*

## Party-Mode Challenge

Winston (Architect): The revised PRD is finally shaped like something TechPlan can consume, but the missing implementation-channel rule is not a cosmetic omission. If TechPlan starts specifying direct edits under `lens-work` without the BMB workflow, you are baking a governance violation into the architecture.

Priya (Governance Auditor): The feature-state ambiguity worries me more than the code gap. If `feature-index.yaml` says one thing and `feature.yaml` says another, the audit must explicitly name which file governs lifecycle truth. Otherwise the next person will “correct” the wrong document and create more drift.

Amelia (Implementation Lead): Treating `fetch-context` as a blocker is the right move, but I still need a target. If TechPlan just says “restore fetch-context,” that is not enough for an implementation team. I need to know whether I am rebuilding the old CLI output contract or inventing a new one.

## Gaps You May Not Have Considered

1. The constitutions already permit `express` and `expressplan`, so the track ADR is about present governance, not just template theory.
2. The current review only succeeded without `fetch-context` because this feature has no explicit dependencies; that workaround does not generalize.
3. The parity audit work now depends on two different governance state carriers (`feature.yaml` and `feature-index.yaml`) with different semantics, and the PRD does not yet codify the boundary.
4. If TechPlan does not encode the BMB-first rule, later compliance review can reject a technically correct implementation on process grounds alone.

## Open Questions Surfaced

1. Should TechPlan explicitly declare `feature.yaml.phase` the source of truth for lifecycle state and treat `feature-index.yaml.status` as secondary metadata?
2. Will the restoration target for `fetch-context` and `read-context` be the old CLI output contract, or is a narrower replacement acceptable?
3. Where in the implementation plan will the mandatory BMad Builder reference load and BMB execution channel be enforced for `lens-work` source edits?
4. Does the constitution-tracks ADR intend to ratify the currently active constitutions, or intentionally narrow them and trigger a governance migration?