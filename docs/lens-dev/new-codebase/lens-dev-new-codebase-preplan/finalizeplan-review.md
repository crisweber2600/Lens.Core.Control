---
feature: lens-dev-new-codebase-preplan
doc_type: finalizeplan-review
phase: finalizeplan
source: manual-rerun
review_format: abc-choice-v1
verdict: pass-with-warnings
finding_count: 5
reviewed_artifacts:
  - business-plan.md
  - tech-plan.md
  - sprint-plan.md
  - expressplan-adversarial-review.md
governance_impact:
  - lens-dev-new-codebase-finalizeplan
  - lens-dev-new-codebase-businessplan
  - lens-dev-new-codebase-baseline
reviewers:
  - adversarial
  - party-mode-challenge
  - governance-cross-check
updated_at: 2026-04-28T00:00:00Z
---

# FinalizePlan Review — Preplan Command

**Feature:** `lens-dev-new-codebase-preplan`
**Phase:** finalizeplan
**Verdict:** pass-with-warnings
**Findings:** 5 (1 high, 2 medium, 2 low)
**Expressplan open warnings addressed:** F-1, F-2 (still open — surfaced in this review)

---

## Cross-Artifact Coherence Assessment

The business plan, tech plan, and sprint plan are internally consistent on the core architecture: no script layer, brainstorm-first ordering at conductor level, shared-utility delegation for batch / review-ready / adversarial-review, no governance writes in preplan, and constitution load as a hard Sprint 2 prerequisite. The three documents cross-reference each other correctly on the ADR decisions and external-prerequisite tables.

**Gap identified:** The expressplan adversarial review's `pass-with-warnings` verdict included F-1 (test location) and F-2 (/next pre-confirmed handoff) as outstanding medium findings. Neither has been updated in the business plan or tech plan since the expressplan phase completed. Both are carried forward into this review as distinct findings.

---

## Adversarial Analysis

### Finding F-5 — `/businessplan` Forward Dependency Not Declared [HIGH]

**Location:** `sprint-plan.md` — PP-4.1, External Prerequisites table; `business-plan.md` — Out-of-Scope section

**Description:** PP-4.1 specifies: "conductor reports 'advance to `/businessplan`'." This creates a **runtime forward dependency** on the `bmad-lens-businessplan` command being available in the new codebase when the preplan conductor completes. The External Prerequisites table lists only `lens-dev-new-codebase-baseline` stories 1-2, 1-3, and 3-1; it does not include `lens-dev-new-codebase-businessplan`. However, governance sensing shows `lens-dev-new-codebase-businessplan` is currently at `expressplan-complete` — it has not reached `finalizeplan-complete`, meaning no dev-ready story set exists yet for the businessplan command. If preplan implementation completes before the businessplan command is available in the new codebase, the advance will fail at runtime.

**Options:**
- **A** — Add `lens-dev-new-codebase-businessplan` as an explicit external prerequisite to PP-4.1 with a note that preplan's phase completion only routes to `/businessplan` if businessplan is confirmed available in `lens.core.src`; add a runtime availability check or guard to PP-4.1's acceptance criteria.
- **B** — Decouple the advance notification from the actual businessplan availability: the preplan conductor reports "advance to `/businessplan`" as a message only, not as a live routing call, and defer businessplan integration to a later story.
- **C** — Add a PP-4.3 story that handles the advance routing and makes businessplan-availability a dependency of that specific story rather than PP-4.1.
- **D** — Accept the dependency and document it in the sprint-plan's "Cross-Sprint Dependencies" section without gating PP-4.1 on it.
- **E** — Treat the businessplan advance as out of scope for this feature (preplan completes by updating feature.yaml only) and update the sprint plan to remove the advance messaging from PP-4.1.

**Recommended:** B or E — decoupling the routing message from the actual businessplan availability is the safest approach. The preplan conductor should report phase completion and the expected next command by name without making a live routing call; actual routing is the user's action.

---

### Finding F-1 (Carried Forward) — Test Location Inconsistency [MEDIUM]

**Location:** `tech-plan.md` — ADR 1 vs. Testing Strategy

**Description:** Carried from expressplan review. ADR 1 prohibits a `scripts/` directory; the Testing Strategy places test files at `_bmad/lens-work/skills/bmad-lens-preplan/scripts/tests/`. This remains unresolved. The sprint plan's PP-1.3 story creates "parity test skeletons" but does not specify where they live, leaving implementation risk that the developer chooses the prohibited path by default.

**Options:**
- **A** — Update `tech-plan.md` Testing Strategy to specify the correct test location (e.g., the existing integration-test collection pattern for new-codebase skills), and update PP-1.3 acceptance criteria to name the exact test file path.
- **B** — Update ADR 1 to permit a `scripts/tests/` subpath for test-only fixtures with no implementation code.
- **C** — Add a note to PP-1.3 that the test location decision is the first task of that story.
- **D** — No change; defer to developer judgment.
- **E** — Investigate where the `switch` and `init-feature` parity tests live in the new codebase and use that as the canonical pattern; update the tech plan accordingly.

**Recommended:** E → A — following the established parity-test pattern for peer skills and then updating the tech plan prevents per-developer inconsistency.

---

### Finding F-2 (Carried Forward) — Missing `/next` Pre-Confirmed Handoff Contract [MEDIUM]

**Location:** `business-plan.md` — Success Criteria; `tech-plan.md` — On Activation

**Description:** Carried from expressplan review. The `/next` pre-confirmed handoff invariant (when `/next` routes to preplan, no redundant launch confirmation is shown) is not documented in the business plan's success criteria or in the tech plan's behavioral constraints. It does not appear in any sprint story's acceptance criteria. Without it, PP-2.1 (conductor activation) may implement an activation confirmation prompt that contradicts the `/next` UX contract.

**Options:**
- **A** — Add the pre-confirmed handoff contract as a success criterion in the business plan and as a behavioral constraint in the tech plan's "On Activation" section; add a parity test in PP-1.3 and an acceptance criterion in PP-2.1.
- **B** — Add it to PP-2.1's acceptance criteria only, with no business plan or tech plan update.
- **C** — Create a dedicated PP-2.1a sub-task for this behavior.
- **D** — Defer to the SKILL.md authoring phase (PP-1.2); the contract will be expressed in the SKILL.md.
- **E** — Accept the risk and rely on code review to catch any activation confirmation prompt.

**Recommended:** A — this is a user-observable regression risk with a concrete, documented precedent in the research corpus.

---

### Finding F-6 — Sprint 3 Gate Not Automated [LOW]

**Location:** `sprint-plan.md` — Sprint 3 section

**Description:** The Sprint 3 gate ("Baseline stories 1-2, 1-3, and 3-1 must pass in `TargetProjects/lens-dev/new-codebase/lens.core.src` before this sprint starts") is stated as a prose requirement but has no automated verification mechanism. The sprint plan lists no CI check, no test command, and no definition of "confirmed green" for these external prerequisites. Without a concrete gate, teams may start Sprint 3 while baseline stories are still in progress, causing integration failures mid-sprint.

**Options:**
- **A** — Add a concrete "gate verification command" to the Sprint 3 prerequisite note: e.g., `uv run --with pytest pytest lens.core.src/_bmad/lens-work/skills/... -k "baseline_1_2 or baseline_1_3 or baseline_3_1"` that must pass before Sprint 3 can begin.
- **B** — Add a PP-2.4 story (or a Sprint 3 gating task) that explicitly runs the baseline test suite and reports pass/fail before any Sprint 3 story begins.
- **C** — Accept the prose gate as sufficient; rely on the sprint planner to enforce it manually.
- **D** — Remove the gate from the sprint plan and replace it with a dependency note in feature.yaml.
- **E** — Add the gate to the Definition of Done for Sprint 2 (Sprint 2 cannot close until the prerequisite gate is confirmed).

**Recommended:** E — treating baseline story confirmation as the exit criteria for Sprint 2 ensures the gate is checked at the natural sprint boundary.

---

### Finding F-7 — No Governance-Write Parity Test Named in PP-3.3 [LOW]

**Location:** `sprint-plan.md` — PP-3.3; `tech-plan.md` — ADR 5

**Description:** ADR 5 states "preplan makes no publish-to-governance call" and the tech plan's Testing Strategy includes "no-governance-write invariant" as a parity test category. The sprint plan places this test in PP-4.1 ("no-governance-write parity test passes"). However, PP-3.3 wires the adversarial review at phase completion — the exact point where the no-governance-write invariant could be inadvertently violated if the adversarial review implementation includes a governance write. There is no mention of the invariant check in PP-3.3's acceptance criteria.

**Options:**
- **A** — Add "no-governance-write invariant test still passes after adversarial review wiring" to PP-3.3's acceptance criteria.
- **B** — Keep the invariant test solely in PP-4.1 but add a note to PP-3.3 that the invariant must not be broken.
- **C** — Rely on PP-4.1's test to catch any regression introduced in PP-3.3.
- **D** — Create an always-on integration test in the CI pipeline for this invariant.
- **E** — No change; PP-4.1 is sufficient.

**Recommended:** A — a lightweight AC addition to PP-3.3 prevents the governance-write invariant from being broken and only caught a sprint later.

---

## Governance Cross-Check

| Related Feature | Phase | Impact |
|---|---|---|
| `lens-dev-new-codebase-baseline` | (split-from, not listed as feature) | Hard external prerequisite for Sprint 3; no feature.yaml entry visible — must verify baseline stories 1-2, 1-3, 3-1 are in baseline's sprint plan |
| `lens-dev-new-codebase-businessplan` | `expressplan-complete` | F-5 — businessplan command not dev-ready; preplan advance must decouple from businessplan availability |
| `lens-dev-new-codebase-finalizeplan` | `preplan` | The finalizeplan command is being designed in parallel; any preplan phase-completion routing that calls `/finalizeplan` indirectly (after businessplan completes) may hit a version gap if the finalizeplan command is not yet available in the new codebase |
| `lens-dev-new-codebase-next` | `preplan` | The `/next` pre-confirmed handoff contract (F-2) depends on the next command routing correctly; if `next` is also under active development, handoff behavior should be coordinated |

---

## Party-Mode Blind-Spot Challenge

### Winston (Architect) Perspective

The tech plan correctly avoids a script layer. However, the `On Activation` sequence in the tech plan calls `bmad-lens-init-feature fetch-context` at step 7 (after docs-path resolution). There is no fallback defined if `fetch-context` returns no context or errors. The conductor's activation will silently succeed even if cross-feature context is unavailable — which is correct per the clean-room constraint, but could hide a misconfiguration. A trace-level log or a `context_loaded: false` signal should be emitted to make the absence explicit and distinguishable from a configuration error.

### John (PM) Perspective

The business plan's success criterion "Output parity with old-codebase" is evaluated via the parity test suite (PP-1.3). However, the parity tests are written by the same team implementing the conductor. There is no independent acceptance run — e.g., a QA pass by a second reviewer running the full preplan flow from scratch before the feature closes. The Definition of Done should include at least one independent run of the preplan flow end-to-end against a test feature to confirm behavioral parity without developer bias.

### Sally (UX) Perspective

The sprint plan does not mention the "no-governance-write" user-facing message. If the preplan conductor runs and completes the adversarial review but then emits no output confirming "phase complete — no governance write occurred," users may be confused about what state the feature is in. A brief confirmatory message in the phase completion output (PP-4.1 or PP-4.2) would align with the UX of peer commands.

---

## Verdict Summary

| Finding | Severity | Status |
|---|---|---|
| F-5: `/businessplan` forward dependency undeclared | High | Blocking warning — must resolve before Sprint 4 begins |
| F-1: Test location inconsistency (carried) | Medium | Warning — resolve in PP-1.3 |
| F-2: Missing `/next` pre-confirmed handoff (carried) | Medium | Warning — resolve before Sprint 2 |
| F-6: Sprint 3 gate not automated | Low | Warning — recommend E: add to Sprint 2 Definition of Done |
| F-7: No-governance-write check absent from PP-3.3 | Low | Warning — add to PP-3.3 acceptance criteria |

**Overall Verdict: pass-with-warnings**

The combined planning set is implementable. The architecture is sound and the shared-utility delegation strategy is well-specified. F-5 (businessplan forward dependency) is a high-severity warning requiring a decision before Sprint 4 stories are authored, but does not block Sprint 1–3 execution. All other findings are documentation gaps addressable before their respective sprint stories begin. Advancing to the planning PR and downstream bundle is permitted.
