---
feature: lens-dev-new-codebase-baseline
doc_type: finalizeplan-review
phase: finalizeplan
source: phase-complete
verdict: pass-with-warnings
reviewed_artifacts: [brainstorm, research, product-brief, prd, architecture, techplan-adversarial-review]
updated_at: 2026-04-23T00:00:00Z
---

# FinalizePlan Adversarial Review — lens-dev-new-codebase-baseline

**Phase:** finalizeplan  
**Source:** phase-complete  
**Reviewed planning set:** `brainstorm.md`, `research.md`, `product-brief.md`, `prd.md`, `architecture.md`, `techplan-adversarial-review.md`  
**Verdict:** **pass-with-warnings**

---

## Summary

The full planning set is internally consistent, complete, and coherent. The planning chain from brainstorm → research → product-brief → prd → architecture forms a traceable line: every goal stated in the product brief is answered by a PRD requirement, and every PRD requirement is resolved in the architecture by a work package with a parity gate. The TechPlan adversarial review resolved all high and medium findings before phase transition.

Three carry-forward items are identified below. All are advisory or scoped to downstream work packages — none block the FinalizePlan phase transition.

**Findings summary (this review):** 0 critical · 0 high · 1 medium · 2 low  
**Carry-forward from TechPlan:** F-02 (compliance audit) acknowledged; F-06 (in-progress feature transition) scoped to WP-17

---

## Cross-Artifact Consistency Check

### Brainstorm → Research → Product Brief

All three artifacts align on the core product mandate: reduce published command surface from 54 to 17, extract shared utilities from copy-pasted phase patterns, and deliver 100% behavioral backwards compatibility. No contradictions or scope drift were found across this layer.

### Product Brief → PRD

| Product Brief Goal | PRD Resolution | Consistent? |
|---|---|---|
| G1 — 17-command surface, split-feature retained | §2.1 17-command table, split-feature row present | ✅ |
| G2 — 100% backwards compatibility, frozen contracts | §2.2 frozen contracts enumerated verbatim | ✅ |
| G3 — Shared utility extraction (3 patterns) | §3 + ADR-001 extract validate-phase-artifacts, bmad-lens-batch, publish-to-governance | ✅ |
| G4 — Deprecated stubs with retained internals | §4 keep/remove mandate, inventory-as-deliverable requirement | ✅ |
| G5 — Schema v4.0 drop-in | §2.2, G5 explicit: schema v4, no migration required | ✅ |
| G6 — End-to-end command maps | Table 3.2 regression anchors + §6 traceability requirement | ✅ |
| Open Q: schema v4.0 vs v5.0? | Resolved in PRD G5: v4.0 drop-in | ✅ closed |

**Unresolved from product-brief:** One product brief open question remains unanswered in the PRD and architecture: *"Will the rewrite ship as a new Lens.Core.Release tag, or as a breaking change requiring user upgrade?"* The PRD scopes the rewrite as schema-v4 drop-in (no migration) but does not state the release delivery mechanism. This is flagged as FP-01 below.

### PRD → Architecture

| PRD Requirement | Architecture Resolution | Consistent? |
|---|---|---|
| 17 retained commands | §5 + §8 — each command has dependency inventory and work package | ✅ |
| Frozen contracts | §10 Data Model — feature.yaml, feature-index.yaml, dev-session.yaml all declared frozen | ✅ |
| Retained-vs-removed inventory as deliverable | §7 matrix (24 skills across keep/remove decisions) — confirmed in ADR-004 | ✅ |
| G6 end-to-end maps | §5 per-command dependency tables (stub → skill → scripts → data contracts) | ✅ |
| Shared utility extraction | §3 (3 patterns mapped to single implementations), ADR-001 | ✅ |
| Review-ready gate (validate-phase-artifacts.py) | §3.1 + §11.2 — single implementation, per-phase delegation | ✅ |
| Publish-before-author hook | §3.3 — single CLI call, no direct governance writes permitted | ✅ |
| WP-15 prerequisite constraint | §6 — explicit intra-tier ordering constraint documented | ✅ |
| BMB governance clarification | §10 + ADR-003 — corrected from TechPlan adversarial review | ✅ |

### TechPlan Adversarial Review Carry-Forward

ADR-004 states: "Any post-TechPlan modifications to keep/remove decisions require a FinalizePlan gate item." No changes to the §7 keep/remove decisions have been proposed between TechPlan and FinalizePlan. The inventory stands as confirmed.

F-02 carry-forward: Lens workflow compliance audit remains code-review-enforced. No automated detection mechanism is added in this rewrite scope. Accepted.

F-06 carry-forward: In-progress feature transition strategy at release time is explicitly scoped to WP-17 (upgrade) or a separate release ops document. No coverage exists in the current planning set. Flagged as FP-02 below.

---

## Findings

### FP-01 [MEDIUM] Release delivery mechanism is unresolved across the full planning set

**Scope:** Product Brief open questions, PRD, architecture  
**Finding:** The product brief lists "Will the rewrite ship as a new Lens.Core.Release tag, or as a breaking change requiring user upgrade?" as an open question. The PRD and architecture confirm schema v4.0 drop-in (no field migration required), but neither document states the release delivery mechanism: how the rewritten `lens.core/` payload reaches users, whether a new release tag is required, what version label it carries, and who triggers the release promotion.

This gap does not block the planning phase or any work package. However, a developer completing WP-17 (upgrade) will need to know whether the release is a tag bump or a forced upgrade before writing the release notes or planning the deployment sequence.

**Impact:** Medium. Unresolved at planning close. Risk increases if WP-17 is the last package and the release mechanism becomes a blocking question at that point.

**Recommendation:** Scope explicitly to WP-17 (upgrade). Add a release delivery note to the WP-17 rewrite scope column in §8.3 or in a companion release-notes stub. Either answer (new tag vs forced upgrade) is acceptable; the constraint is that the decision is written before WP-17 executes.

**Status:** Open — scoped to WP-17.

---

### FP-02 [LOW] In-progress feature transition is unowned in the planning set

**Scope:** PRD, architecture §9 (inherited from F-06 TechPlan)  
**Finding:** Architecture §9 documents the "post-upgrade stub behavior" as a hard cut: removed stubs are deleted, no redirect, users informed via upgrade notes. However, it does not name a WP or scope item that owns the upgrade note content or the user-communication plan at release time. F-06 acknowledged this as "release-day concern outside the rewrite architecture scope" but did not assign ownership.

**Impact:** Low. Behavioral contract is clear (hard cut). The gap is operational: who writes the upgrade notes, and when?

**Recommendation:** Add one sentence to the WP-17 scope column: "Owner lane produces release upgrade notes documenting the 37 removed stubs." This closes the ownership gap without adding scope to any WP.

**Status:** Open — advisory; scoped to WP-17.

---

### FP-03 [LOW] Split features lack explicit `depends_on` linkage in governance

**Scope:** Governance cross-check  
**Finding:** 18 sibling features in `lens-dev/new-codebase` were created by splitting from `lens-dev-new-codebase-baseline`. All are currently at `phase: preplan`. None have `depends_on: [lens-dev-new-codebase-baseline]` in their `feature.yaml`. The architectural dependency (baseline must complete planning before work packages begin) is documented in the architecture work package table but is not encoded in the governance data.

**Impact:** Low. Today this is only one team and `next` can surface the correct ordering from `lifecycle.yaml` and the architecture. In a multi-agent or multi-developer scenario, a developer picking up `lens-dev-new-codebase-constitution` from governance alone would see no declared dependency on the baseline being finalized.

**Recommendation:** Populate `depends_on: [lens-dev-new-codebase-baseline]` in each split feature's `feature.yaml` before their planning phases begin. The baseline's `depended_by` list should mirror this. This is a governance hygiene item, not a planning blocker.

**Status:** Advisory — not a FinalizePlan blocker. Recommended for WP-00 or as a governance clean-up on the first sprint.

---

## Governance Impact Cross-Check

| Artifact | Governance-impacted surface | Risk |
|---|---|---|
| `feature.yaml` schema | All 18 sibling features + all future features | Frozen at v4 per ADR-002. No impact. |
| `feature-index.yaml` | Service index under `lens-dev/new-codebase` | Frozen. No impact. |
| `dev-session.yaml` | All in-progress dev sessions | Frozen. No impact. |
| `.github/prompts/` stubs | All users of the 37 removed commands | Hard cut at upgrade. WP-17 release notes required. |
| `module-help.csv` + `lens.agent.md` | Command discovery surfaces | Must stay in sync with 17-command surface (ADR-005). Rewrite-atomic constraint already in architecture. |
| `publish-to-governance` CLI | All planning phases (businessplan, techplan, finalizeplan, dev) | Single implementation owned by `bmad-lens-git-orchestration`. No direct governance writes permitted. |
| WP-15 sequencing | All 18 planning conductor + sibling features | Constitution bug fix must precede any sibling-feature planning-tier work. Enforced by architecture §6 prerequisite. |

### Related governance features and dependency status

| Feature ID | Phase | Depends on baseline planning? | Action |
|---|---|---|---|
| `lens-dev-new-codebase-constitution` | preplan | Yes — WP-15 is prerequisite for WP-07–11 | Begin immediately after FinalizePlan; first to complete |
| `lens-dev-new-codebase-preplan` | preplan | Yes — after constitution complete | Tier 3 → 4 planning conductor |
| `lens-dev-new-codebase-businessplan` | preplan | Yes — planning conductor | Tier 3 → 4 |
| `lens-dev-new-codebase-techplan` | preplan | Yes — planning conductor | Tier 3 → 4 |
| `lens-dev-new-codebase-finalizeplan` | preplan | Yes — planning conductor | Tier 3 → 4 |
| `lens-dev-new-codebase-expressplan` | preplan | Yes — planning conductor | Tier 3 → 4 |
| `lens-dev-new-codebase-new-feature` | preplan | Yes — identity root | Tier 2 → 3 |
| `lens-dev-new-codebase-switch` | preplan | Yes — navigation | Tier 2 → 3 |
| `lens-dev-new-codebase-next` | preplan | Yes — navigation | Tier 2 → 3 |
| `lens-dev-new-codebase-preflight` | preplan | Yes — scaffolding | Tier 1 |
| `lens-dev-new-codebase-dev` | preplan | Yes — execution | Tier 4 → 5 |
| `lens-dev-new-codebase-complete` | preplan | Yes — closure | Tier 5 |
| `lens-dev-new-codebase-split-feature` | preplan | Yes — feature reshaping | Tier 5 |
| `lens-dev-new-codebase-discover` | preplan | Yes — inventory sync | Tier 5 |
| `lens-dev-new-codebase-upgrade` | preplan | Yes — compatibility | Tier 5 |

**Critical ordering constraint:** `lens-dev-new-codebase-constitution` must begin and deliver WP-15 before any of the five planning conductor features begin their `preplan` → `dev` flow.

---

## Party-Mode Blind-Spot Challenge

Three reviewers were asked to surface gaps the document set may have missed.

---

**Quinn (QA, Test Architect):** "The parity gate for each work package is defined as a specific test file passing (e.g., `test-init-feature-ops.py` for WP-04). But the architecture defines no cross-tier integration test — only per-WP unit-level parity gates. Tier transition is gated on WP-N parity tests passing, but those tests only validate the WP in isolation. When WP-04 (`new-feature`) and WP-10 (`finalizeplan`) both interact with `feature.yaml`, what test tells you their shared state is coherent after both are rewritten? The current test inventory has no named integration regression. A broken cross-WP state model would pass all per-WP gates and fail only in end-to-end usage."

**Winston (Architect):** "The architecture specifies `publish-to-governance` as the only valid governance write path and locates its single implementation in `bmad-lens-git-orchestration`. But what happens when a publish-to-governance bug is discovered mid-rewrite — say, during WP-08 (businessplan)? `bmad-lens-git-orchestration` is a tier 2 shared primitive: it should be stable before tier 3 begins. But tier 2 rewrite scope (WP-04 through WP-06) doesn't include an explicit bug-fix contract for `publish-to-governance`. The implication is that a bug found in a tier 2 primitive during tier 3 execution creates an unplanned WP against an 'already stable' package. Is there a lightweight path for post-stabilization fixes to tier 2 primitives?"

**Amelia (Dev):** "I'm picking up the first sibling feature that starts after FinalizePlan — `lens-dev-new-codebase-constitution`. The architecture says WP-15 must be done before planning conductors begin. But WP-15 is not a planning conductor itself — it's in tier 5. When I open its feature.yaml, it says `phase: preplan` and `track: full`. That means before I can start the WP-15 bug fix, I have to go through preplan → businessplan → techplan → finalizeplan myself. That's the old preplan command I'm supposed to be rewriting. Which comes first: fix the constitution engine so the planning conductors don't break, or rewrite the planning conductors so the WP-15 feature can plan itself? The architecture calls out the prerequisite constraint but doesn't name a resolution for this bootstrapping dependency."

---

### Blind-Spot Questions — Answered (2026-04-23)

**Q1 — Cross-tier integration testing gap:**  
The per-WP parity gates are necessary but not sufficient for cross-WP behavioral coherence. The architecture's validation strategy (§12) lists 8 regression anchors; none explicitly cover cross-WP integration state. A cross-tier integration test that runs an end-to-end feature lifecycle (new-feature → preplan → businessplan → techplan → finalizeplan → dev → complete) through the rewritten surface is needed before the rewrite is declared done. This integration test is not a separate WP — it is the release acceptance gate. **Recommendation:** Add "End-to-end lifecycle regression (all 17 commands in sequence)" as an explicit acceptance criterion in the WP-17 parity gate or in a release-gate checklist produced by the FinalizePlan bundle.

**Q2 — Post-stabilization bug-fix path for tier 2 primitives:**  
The architecture's WP tier model assumes tier N is stable before tier N+1 begins, but does not define a bug-fix path for primitives that have already been declared stable. The correct resolution is to treat any bug in a tier 2 primitive discovered during tier 3 execution as a hot-fix against the tier 2 WP (same owner lane, same parity gate). The hot-fix follows the same Lens workflow process as any other change. The architectural principle is: tier gate passing means the WP is stable, not frozen. Post-stabilization bug fixes are allowed without re-running the full tier sequence. **Note:** This interpretation should be stated in the sprint planning artifact or implementation readiness document.

**Q3 — WP-15 bootstrapping dependency:**  
This is the most operationally significant blind spot. WP-15 must precede planning conductors (WP-07–WP-11). But WP-15 itself needs to go through the planning phases. On the old codebase, `preflight` → `preplan` → `businessplan` → `techplan` → `finalizeplan` still exist and run correctly. The resolution is: WP-15 (`lens-dev-new-codebase-constitution`) runs its planning phases on the **current (unrewritten) lens-work surface**. The rewritten planning conductors (WP-07 through WP-11) are not required to plan WP-15 — they're only required before WP-15's dev phase completes. The bootstrapping order is: run WP-15 planning on the old surface → complete WP-15 dev (fix the constitution bug) → begin WP-07–WP-11 dev. **This resolution should be recorded as a sprint planning note, not an architecture change.**

---

## Action Items (FinalizePlan Gate Items)

| Item | Priority | Owner | Status |
|---|---|---|---|
| FP-01: Release delivery mechanism | Medium | WP-17 owner lane | Open — scope to WP-17 rewrite column |
| FP-02: Release upgrade notes ownership | Low | WP-17 owner lane | Open — one-line WP-17 scope addition |
| FP-03: Split feature `depends_on` linkage | Low | Sprint 1 governance clean-up | Open — advisory |
| Q1 answer: End-to-end lifecycle regression | Medium | WP-17 parity gate or release gate | Open — add to implementation readiness |
| Q2 answer: Hot-fix path for tier 2 primitives | Low | Sprint planning note | Open — record in sprint-status.yaml |
| Q3 answer: WP-15 bootstrapping resolution | Medium | Sprint 1 note | Open — record in sprint-status.yaml |
| ADR-004 reminder: §7 changes require amendment | Medium | All WP authors | Tracked — gate cleared at TechPlan, new changes require amendment |

---

## Verdict

**pass-with-warnings**

The planning set is complete, internally consistent, and ready for bundle generation. No critical or high blockers were found. The full chain from brainstorm → product-brief → PRD → architecture is coherent and traceable. All TechPlan findings were resolved before this review.

Three new findings (FP-01, FP-02, FP-03) are medium or low and are scoped to WP-17 or advisory governance hygiene. The party-mode blind-spot challenge surfaced one operationally significant bootstrapping dependency (Q3: WP-15 on old surface) with a clear resolution path that requires a sprint planning note, not an architecture change.

**Phase transition:** `techplan-complete` → `finalizeplan` → `finalizeplan-complete` may proceed.

**Next action after bundle:** `/dev` — begin `lens-dev-new-codebase-constitution` (WP-15) on the existing lens-work surface.
