---
feature: lens-dev-new-codebase-techplan
doc_type: adversarial-review
phase: expressplan
source: phase-complete
verdict: pass-with-warnings
status: responses-recorded
critical_count: 1
high_count: 2
medium_count: 2
low_count: 0
carry_forward_blockers: []
updated_at: 2026-04-29T02:08:41Z
review_format: abc-choice-v1
---

# ExpressPlan Adversarial Review — lens-dev-new-codebase-techplan

**Source:** phase-complete  
**Artifacts reviewed:** business-plan.md, tech-plan.md, sprint-plan.md  
**Format:** Responses have been recorded. Each finding retains its A/B/C, D, and E options so the chosen resolution remains auditable.

---

## Verdict: `pass-with-warnings`

The staged packet remains coherent and usable as a clean-room expressplan artifact set for the `techplan` rewrite. The selected responses have now been folded into the planning packet: the governance feature record has been updated to `track: express`, the feature now absorbs the missing shared utility surfaces into scope, discovery and test-harness acceptance items have moved into the first implementation slice, parity has been defined explicitly, and the packet is treated as review-complete. Remaining work is implementation scope, not unresolved review intake.

---

## Response Record

The following responses were recorded and applied to the planning packet:

| Option | Meaning |
| --- | --- |
| A / B / C | Accept the proposed resolution with its stated trade-offs |
| D | Provide a custom resolution after `D:` |
| E | Explicitly accept the finding with no action |

---

## Finding Summary

| ID | Severity | Title | Your Response |
| --- | --- | --- | --- |
| C1 | Critical | Governance track still blocks literal expressplan execution | **A** |
| H1 | High | Shared dependencies remain prerequisite-only, not executable | **C** |
| H2 | High | Discovery and test harness ownership are still undefined | **A** |
| M1 | Medium | The packet needs an explicit parity check against the future skill output | **A** |
| M2 | Medium | FinalizePlan handoff criteria depend on the governance switch being closed | **A** |

---

## Critical Findings

### C1 — Governance track still blocks literal expressplan execution

**Location:** governance `feature.yaml` for `lens-dev-new-codebase-techplan`  
**Gate:** Before any lifecycle automation rerun

The docs now describe an expressplan path, but the governance feature record still says `track: full`. That means the packet is valid as staged planning, but invalid as a literal expressplan execution path until the sanctioned metadata update is applied.

**Recorded response:** **A**  
**Applied adjustment:** The sanctioned `feature-yaml` update has been run and the governance feature record now carries `track: express`.

**Choose one:**

- **A.** Run the sanctioned `feature-yaml` update to set `track=express` before any further lifecycle automation.  
	**Why pick this:** Aligns governance state with the staged packet; removes the main operational blocker cleanly.  
	**Why not:** Requires an intentional governance write and review discipline.

- **B.** Keep governance state unchanged and treat this packet as planning-only input for now.  
	**Why pick this:** Avoids an immediate governance mutation while preserving the packet for review.  
	**Why not:** Leaves the user’s requested express path unusable in automation.

- **C.** Split the planning route onto a separate express-track feature instead of switching this feature.  
	**Why pick this:** Preserves the existing feature record untouched and makes track semantics explicit.  
	**Why not:** Introduces new feature-management overhead and was not requested.

- **D.** Write your own response.
- **E.** Keep as-is.

---

## High Findings

### H1 — Shared dependencies remain prerequisite-only, not executable

**Location:** tech-plan.md, Shared Dependencies and Implementation Sequence sections  
**Gate:** Before target-project code work is declared end-to-end complete

The plan correctly refuses to duplicate shared utilities, but that means the implementation cannot finish until `bmad-lens-git-orchestration`, `bmad-lens-bmad-skill`, `bmad-lens-adversarial-review`, and constitution loading are available in the target project.

**Recorded response:** **C**  
**Applied adjustment:** The technical and sprint plans now absorb these missing shared utility surfaces into this feature’s scope instead of treating them as external prerequisites.

**Choose one:**

- **A.** Keep these as explicit prerequisites and sequence implementation behind them.  
	**Why pick this:** Preserves architectural integrity and avoids local clones of shared behavior.  
	**Why not:** Delivery may wait on outside teams or sibling features.

- **B.** Allow narrow compatibility shims only at the prompt and skill layer, but still block any local governance or review logic forks.  
	**Why pick this:** Lets early slices land without pretending the runtime is complete.  
	**Why not:** Adds temporary surfaces that must be cleaned up later.

- **C.** Fold the missing shared utilities into this feature.  
	**Why pick this:** Reduces external waiting.  
	**Why not:** Breaks the stated rewrite architecture and widens the feature beyond control.

- **D.** Write your own response.
- **E.** Keep as-is.

### H2 — Discovery and test harness ownership are still undefined

**Location:** business-plan.md open questions; tech-plan.md open questions  
**Gate:** Before implementation slice scoping closes

The packet now names the missing target-project files, but it still does not assign ownership for command discovery updates or for the focused regression harness that should verify prompt-start and wrapper-equivalence behavior.

**Recorded response:** **A**  
**Applied adjustment:** Discovery wiring and focused test-harness acceptance items now belong to the first implementation slice in the business, technical, and sprint plans.

**Choose one:**

- **A.** Add discovery and test-harness acceptance items to the first implementation slice.  
	**Why pick this:** Makes the landing path executable and testable from the start.  
	**Why not:** Slightly broadens the first coding slice.

- **B.** Defer discovery updates to the release or discovery feature, but require the test harness path now.  
	**Why pick this:** Keeps the implementation slice narrow while still closing the riskiest ambiguity.  
	**Why not:** Leaves some usability wiring for later.

- **C.** Defer both discovery and test ownership until after the prompt chain exists.  
	**Why pick this:** Minimizes planning effort upfront.  
	**Why not:** Increases the chance of ad hoc, inconsistent follow-up work.

- **D.** Write your own response.
- **E.** Keep as-is.

---

## Medium Findings

### M1 — The packet needs an explicit parity check against the future skill output

**Location:** business-plan.md goals; tech-plan.md validation plan  
**Gate:** Before implementation is called parity-complete

The packet states that future skill work should be measured against these docs, but it does not yet define the exact comparison point. Without that, “output parity” can devolve into a subjective review.

**Recorded response:** **A**  
**Applied adjustment:** Parity is now defined as reproducing `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, and `expressplan-adversarial-review.md` with equivalent routing, gates, and delivery slices.

**Choose one:**

- **A.** Define parity as reproducing the same four staged artifacts with equivalent routing, gates, and delivery slices.  
	**Why pick this:** Gives the future skill a concrete acceptance target.  
	**Why not:** Requires reviewers to compare structure as well as content.

- **B.** Define parity only at the behavior level: same gates, same downstream files, no content comparison.  
	**Why pick this:** Easier to validate in automation.  
	**Why not:** Risks content drift that still passes behavior checks.

- **C.** Treat this packet as illustrative and skip parity enforcement.  
	**Why pick this:** Lowest process overhead.  
	**Why not:** Conflicts with the user’s requirement for output parity in the new skill.

- **D.** Write your own response.
- **E.** Keep as-is.

### M2 — FinalizePlan handoff still depends on the governance switch being closed

**Location:** sprint-plan.md critical path  
**Gate:** Before the packet is treated as finalizeplan-ready

The sprint plan correctly names the governance switch as step 2 on the critical path. That means the packet is ready for planning review, but not yet ready for an automatic handoff into finalizeplan.

**Recorded response:** **A**  
**Applied adjustment:** The sprint plan now records the packet as review-complete, notes that governance track alignment is already applied, and separates finalizeplan readiness from the earlier track-switch blocker.

**Choose one:**

- **A.** Treat the packet as review-complete but not finalizeplan-ready until the track switch is applied.  
	**Why pick this:** Matches the actual state without overstating readiness.  
	**Why not:** Requires one more explicit milestone check.

- **B.** Manually hand the packet to finalizeplan review while the governance update is queued.  
	**Why pick this:** Keeps momentum.  
	**Why not:** Risks reviewers assuming the lifecycle state is already aligned.

- **C.** Pause after expressplan review until governance metadata is updated.  
	**Why pick this:** Safest operationally.  
	**Why not:** Adds waiting time even though the packet itself is now coherent.

- **D.** Write your own response.
- **E.** Keep as-is.

---

## Accepted Risks

None recorded yet.

---

## Party-Mode Challenge

**Winston (Architect):** If the governance feature record stays `track: full`, the packet is structurally correct but operationally dead. Close that gap before anyone mistakes documentation completeness for lifecycle completeness.

**John (PM):** The packet now distinguishes the planning route from the runtime command under rewrite. Keep that distinction visible in the implementation story so no one rewrites `techplan` as an express-only shortcut.

**Mary (Analyst):** Output parity is now plausible, but only if the future skill is judged against these exact staged artifacts rather than against memory or a looser summary.
