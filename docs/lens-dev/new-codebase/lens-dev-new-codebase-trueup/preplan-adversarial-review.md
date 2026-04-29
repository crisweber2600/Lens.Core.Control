---
feature: lens-dev-new-codebase-trueup
doc_type: adversarial-review
phase: preplan
source: phase-complete
verdict: pass-with-warnings
review_format: abc-choice-v1
reviewed_at: 2026-04-28T01:15:00Z
---

# Adversarial Review: lens-dev-new-codebase-trueup / preplan

## Selected response

**B — Pass with warnings**

## Response options

- **A — Pass:** Artifacts are complete and coherent; proceed with no material concerns.
- **B — Pass with warnings:** Artifacts support proceeding, but specific risks or gaps must be carried forward and addressed explicitly.
- **C — Needs revision:** Artifacts are directionally useful but require substantive revision before the next phase.
- **D — Major rework required:** Artifacts have significant structural or evidence gaps that block reliable planning.
- **E — Reject:** Artifacts are not sufficient to support phase progression.

---

**Reviewed:** 2026-04-28T01:15:00Z  
**Source:** phase-complete  
**Overall Rating:** pass-with-warnings

---

## Summary

The three preplan artifacts (brainstorm, research, product-brief) together form a coherent and evidence-grounded gap analysis. The inventory methodology is sound: filesystem enumeration of the new-codebase source combined with governance phase state produced a credible, specific gap map. The True Up framing — shared infrastructure first, then audit, then per-command features unlock — is architecturally correct.

However, the artifact set has two high-risk coverage gaps that must be acknowledged before businessplan begins: (1) the test harness strategy for Tier 1 shared infrastructure is completely unspecified, and (2) `publish-to-governance` touches governance-main without any documented safeguards, creating a silent-failure-equals-governance-corruption risk. Additionally, the product-brief's scope for `bmad-lens-adversarial-review` uses "minimal" without defining what minimal means, which is likely to cause scope creep during dev.

**Recommended next action:** Accept these as documented risks, carry three of them as open questions into businessplan, and proceed to businessplan phase. Do not let the warnings block businessplan — they are risk-acknowledgement items, not logic-failure items.

---

## Findings

### Critical

*(None — no finding blocks the preplan → businessplan handoff)*

---

### High

---

#### H1 — Test Harness Strategy Unspecified for `bmad-lens-complete`

**Dimension:** Coverage Gaps  
**Severity:** High  
**Finding:** SC-3 ("complete Delivery Package Scaffolded") requires test stubs for the happy path and the prerequisite-missing degradation path of `bmad-lens-complete`, but no specification of the test framework, fixtures, or how tests run is provided. An unspecified test strategy for a command with an irreversible finalization gate is a high risk: implementation will stall when the test question surfaces mid-sprint.  
**Recommendation:** Add "test harness strategy for `bmad-lens-complete` test stubs" as a required artifact or decision record in businessplan. At minimum: document whether existing `pytest` patterns (as used in init-feature and switch) are sufficient, or whether a new integration test harness is needed.

**Finding response options:**
- A) Add test harness strategy to businessplan as required artifact/decision record ✅
- B) Accept as known risk; document in parity-gate-spec
- C) Carry forward to the story that implements test stubs (TU-2.2)
- D) Write your own response
- E) Keep as-is — accept finding without action

**Selected: A — Carried into businessplan; addressed in TU-2.2 story spec.**

---

#### H2 — `publish-to-governance` Safeguards Unspecified (Carry-Forward)

**Dimension:** Complexity and Risk  
**Severity:** High  
**Finding:** `publish-to-governance` touches `governance-main` via `git commit + push`, but the product-brief explicitly places shared infrastructure (including `publish-to-governance`) **out of scope** for True Up. This finding is recorded for carry-forward: when `publish-to-governance` is scoped into an individual preplan feature planning cycle, safeguards must be specified — at minimum a dry-run flag, a test-governance-repo fixture path, and a pre-write pull + conflict-abort sequence.  
**Recommendation:** Carry forward to the preplan feature that owns `publish-to-governance`. Not a True Up blocker.

**Finding response options:**
- A) Carry forward to the preplan feature that owns `publish-to-governance` ✅
- B) Document safeguards in True Up architecture as advisory constraints
- C) Accept as out-of-scope; no action for True Up
- D) Write your own response
- E) Keep as-is — accept finding without action

**Selected: A — Carry forward to the owning feature's preplan.**

---

#### H3 — `bmad-lens-adversarial-review` Scope Undefined (Carry-Forward)

**Dimension:** Complexity and Risk  
**Severity:** High  
**Finding:** `bmad-lens-adversarial-review` is also explicitly **out of scope** for True Up (shared infrastructure deferred to individual preplan feature planning cycles). The scope definition question ("minimal" vs. port-faithful) must be resolved when the feature owning `bmad-lens-adversarial-review` activates.  
**Recommendation:** Carry forward to the preplan feature that owns `bmad-lens-adversarial-review`. Not a True Up blocker.

**Finding response options:**
- A) Carry forward to the preplan feature that owns `bmad-lens-adversarial-review` ✅
- B) Add "minimal vs. port-faithful" decision as a True Up ADR
- C) Accept as out-of-scope; no action for True Up
- D) Write your own response
- E) Keep as-is — accept finding without action

**Selected: A — Carry forward to the owning feature's preplan.**

---

### Medium

---

#### M1 — SC-2 Sequencing Dependency (Tier 1 Bootstrapping Loop)

**Dimension:** Logic Flaws  
**Severity:** Medium  
**Finding:** The product-brief lists "update governance phase labels for new-feature" (SC-2) as a True Up task. Updating governance labels uses `bmad-lens-feature-yaml` update operations — which is itself a Tier 1 True Up output. The task requires the tool it is building. This is a sequencing dependency within True Up: SC-2 audit tasks that require feature-yaml updates cannot be completed until Tier 1 is done. The current plan doesn't acknowledge this ordering.  
**Recommendation:** Explicitly sequence SC-2 audit tasks: the parity report (SC-2 first half) can be written before Tier 1, but governance phase label corrections (SC-2 second half) must wait until `bmad-lens-feature-yaml` update ops are implemented.

**Finding response options:**
- A) Explicitly sequence SC-2 in the parity audit story spec (first-half before Tier 1, second-half after) ✅
- B) Remove SC-2 governance label updates from True Up scope
- C) Carry forward as implementation constraint in TU-4.1
- D) Write your own response
- E) Keep as-is — accept finding without action

**Selected: A — Sequencing documented in TU-4.1 story spec.**

---

#### M2 — SC-3 Conflates Structural and Behavioral Verification

**Dimension:** Coverage Gaps  
**Severity:** Medium  
**Finding:** The product-brief describes a "parity checklist" (SC-3) as "runnable" via possible extension of `validate-phase-artifacts.py`. But `validate-phase-artifacts.py` checks for file existence, not behavioral parity. Behavioral parity — verifying that `init-feature-ops.py create` produces output matching the old-codebase contract — requires a different kind of test runner. SC-3 conflates two distinct verification modes (structural and behavioral).  
**Recommendation:** Split SC-3 into SC-3a (structural: file presence, SKILL.md present, prompts published) and SC-3b (behavioral: output contract tests). Acknowledge that SC-3b may require a new test fixture or remain as a manual checklist until a behavioral test harness is built.

**Finding response options:**
- A) Split SC-3 into SC-3a (structural) and SC-3b (behavioral) in the businessplan PRD ✅
- B) Scope only SC-3a (structural) for True Up; defer SC-3b to a future feature
- C) Accept as-is; note the distinction in parity-gate-spec
- D) Write your own response
- E) Keep as-is — accept finding without action

**Selected: A — Split acknowledged in parity-gate-spec (FR-14).**

---

#### M3 — `complete` Feature Coordination Gap

**Dimension:** Cross-Feature Dependencies  
**Severity:** Medium  
**Finding:** The `complete` feature is at `finalizeplan-complete` (dev-ready). True Up's Tier 4 task ("document graceful-degradation vs. hard-prerequisite decision for complete") implies the decision hasn't been made. If the complete feature starts dev before True Up publishes this decision, there will be a design conflict mid-sprint. No coordination mechanism is specified between True Up and the complete feature's dev activation.  
**Recommendation:** Add a dependency note to the complete feature's governance docs: do not activate dev until True Up publishes the architectural decision record for prerequisite handling. This should be documented as a blocker in `feature.yaml` for `lens-dev-new-codebase-complete`.

**Finding response options:**
- A) Add dependency blocker in `complete` feature governance before dev activates ✅
- B) Add carry-forward note in True Up finalizeplan artifacts (CF-3)
- C) Accept coordination risk; rely on sprint planning to manage
- D) Write your own response
- E) Keep as-is — accept finding without action

**Selected: A — Addressed in TU-5.1 blocker annotation story (CF-3, CF-5).**

---

#### M4 — RELEASE Module Assumed as Parity Ground Truth

**Dimension:** Assumptions and Blind Spots  
**Severity:** Medium  
**Finding:** The artifacts assume the RELEASE module (`lens.core/`) is the definitive ground truth for old-codebase behavior. But the old-codebase discovery docs describe an earlier state of the release module, and the RELEASE module may have been patched since. The baseline research.md 17-command traceability matrix is the authoritative reference — but it was written before the RELEASE module's current state was fully audited against that matrix.  
**Recommendation:** Acknowledge that the parity baseline is the governance baseline research.md traceability matrix, not the current RELEASE module binary. Any RELEASE module changes since the baseline research was written are out of scope unless they affect the new-codebase target behavior.

**Finding response options:**
- A) Acknowledge parity baseline is governance baseline research.md matrix (not RELEASE module binary) ✅
- B) Add RELEASE module delta audit as a businessplan research task
- C) Accept as-is; use RELEASE module pragmatically during parity audit
- D) Write your own response
- E) Keep as-is — accept finding without action

**Selected: A — Documented in parity-audit-report acceptance criteria.**

---

#### M5 — `lc-agent-core-repo` Not Analyzed

**Dimension:** Coverage Gaps  
**Severity:** Medium  
**Finding:** The `lc-agent-core-repo` skill exists in the new-codebase source (`skills/lc-agent-core-repo/`) with a full package (SKILL.md, scripts, assets, references). It is not mentioned anywhere in the brainstorm, research, or product-brief. Its relationship to the Lens lifecycle and to the 17 retained commands is unaddressed — it may be a separate non-Lens skill, a utility, or an undocumented 18th command.  
**Recommendation:** Add "investigate lc-agent-core-repo relationship to Lens lifecycle" as a businessplan research task. If it is out of scope for the Lens rewrite, document that explicitly.

**Finding response options:**
- A) Add lc-agent-core-repo investigation to businessplan research tasks ✅
- B) Mark as explicitly out of scope in True Up product brief
- C) Accept as-is; investigate opportunistically
- D) Write your own response
- E) Keep as-is — accept finding without action

**Selected: A — Added as businessplan research task.**

---

### Low

---

#### L1 — `bmad-lens-lessons` Has `.pyc` Files but No `.py` Source

**Dimension:** Coverage Gaps  
**Severity:** Low  
**Finding:** `bmad-lens-lessons` has compiled `.pyc` files in the new-codebase source but no `.py` source files. Research lists it as an open question. A `.pyc`-only skill is not usable without the source. This could be a gitignore issue or an abandoned partial delivery.  
**Recommendation:** Investigate before businessplan: check `.gitignore` in the new-codebase source for `*.py` exclusion patterns. If the source is gitignored, fix the `.gitignore`. If the skill is abandoned, explicitly mark it out of scope in governance.

**Finding response options:**
- A) Investigate `.gitignore` before businessplan and resolve ✅
- B) Explicitly mark `bmad-lens-lessons` out of scope in governance
- C) Accept as-is; log for later investigation
- D) Write your own response
- E) Keep as-is — accept finding without action

**Selected: A — Investigated during businessplan; resolved.**

---

#### L2 — `finalize-feature.md` Not Examined

**Dimension:** Logic Flaws  
**Severity:** Low  
**Finding:** `bmad-lens-complete/references/finalize-feature.md` was identified in the filesystem inventory but not examined. It could be a planning artifact from an earlier session that describes the complete command contract in detail — potentially resolving some of the SKILL.md gap flagged in the audit.  
**Recommendation:** Read `finalize-feature.md` during businessplan research to determine if it has implementation contract detail that should be incorporated into the SKILL.md scaffold task.

**Finding response options:**
- A) Read `finalize-feature.md` during businessplan research ✅
- B) Reference as optional input in TU-2.1 SKILL.md story spec
- C) Accept as-is; not a blocker
- D) Write your own response
- E) Keep as-is — accept finding without action

**Selected: A — Read during businessplan; incorporated in SKILL.md scaffold notes.**

---

#### L3 — Governance-Write Atomicity Not Specified

**Dimension:** Coverage Gaps  
**Severity:** Low  
**Finding:** The product-brief does not address operational monitoring for `publish-to-governance` failures. If a governance publish fails mid-commit, there may be partial writes to `feature.yaml` or `feature-index.yaml` that leave governance in an inconsistent state.  
**Recommendation:** Add governance-write atomicity as a non-functional requirement in businessplan. At minimum: `publish-to-governance` must emit a structured error on any failure and must not partially commit.

**Finding response options:**
- A) Add governance-write atomicity as a non-functional requirement in businessplan
- B) Carry forward with H2 to the preplan feature that owns `publish-to-governance` ✅
- C) Accept as-is; no action for True Up
- D) Write your own response
- E) Keep as-is — accept finding without action

**Selected: B — Carried forward with H2 to the owning feature's preplan.**

---

#### L4 — SKILL.md Not Explicitly Listed as Required Deliverable

**Dimension:** Assumptions and Blind Spots  
**Severity:** Low  
**Finding:** SKILL.md files are the agent-discovery mechanism — agents load SKILL.md to know how to invoke a skill. The product-brief tasks list building scripts and tests for Tier 1 infrastructure but does not explicitly list "SKILL.md for adversarial-review" or "SKILL.md for feature-yaml" as required deliverables. Without SKILL.md files, these skills are invisible to agent sessions even when their scripts work.  
**Recommendation:** Explicitly add SKILL.md creation as a required deliverable for each Tier 1 skill in the businessplan scope definition.

**Finding response options:**
- A) Add SKILL.md creation as explicit deliverable for each Tier 1 skill in businessplan scope ✅
- B) Accept as implicit deliverable — SKILL.md is standard practice
- C) Accept as-is; add to parity gate spec
- D) Write your own response
- E) Keep as-is — accept finding without action

**Selected: A — Added as explicit deliverable in businessplan PRD.**

---

## Accepted Risks

*(None pre-accepted — all findings are new; user should review and accept/reject at businessplan activation)*

---

## Party-Mode Challenge

**Dev (Infrastructure Engineer):** I'm looking at your SC-5 success criterion — "Tier 1 infrastructure passes integration smoke tests against a test feature." You're asking me to write `bmad-lens-adversarial-review`, `publish-to-governance`, and `bmad-lens-feature-yaml` update ops, plus test them against a fixture feature. That's four non-trivial components. Which one do you want me to start with, and how do I know when the "minimal" review-gate contract is done? Because right now "minimal" isn't defined anywhere in these documents.

**Product (Release Manager):** More practically — the `complete` feature is marked `finalizeplan-complete`, which means its dev can start at any time. But True Up owns the prerequisite decision for `complete`. If True Up takes 3 sprints to get to Tier 4, the complete feature is just sitting blocked with no governance blocker documented. That's a coordination gap. Who owns tracking that `complete` dev hasn't accidentally started? Is there a shared backlog or just a sticky note?

**QA (Parity Auditor):** Here's the thing I'd push on: your parity checklist says "SKILL.md present, script present, tests present, prompt published." But that's a structural checklist, not a behavioral one. The old codebase produced specific outputs for `new-domain`, `new-service`, and `switch` — specific schema fields, specific file names, specific governance registration behavior. How do you know the new-codebase outputs are *behaviorally equivalent* without comparing actual outputs? The checklist as described can pass with a SKILL.md that has completely wrong behavior, as long as the files exist.

---

## Gaps You May Not Have Considered

1. **What happens to the RELEASE module when new-codebase commands ship?** The research describes the new-codebase source as the "authoring workspace" but the RELEASE module as the "runtime." Is there a cutover mechanism? Or does the new codebase replace the release module file-by-file? The architecture of how the two coexist — or how they transition — is never described.

2. **Is there a governance schema version for feature.yaml?** The `bmad-lens-feature-yaml` update operations need to write to feature.yaml. But if the schema evolves (e.g., new fields added during businessplan), old feature.yaml files may be incompatible with the update operations. Schema version management is not mentioned in any preplan artifact.

3. **Who activates the 12 unimplemented retained-command features?** The product-brief says True Up doesn't own individual command implementation — but the 12 features are all sitting at `preplan` with no planned activation. After True Up unblocks Tier 1, who decides which feature to activate next, and in what order? There's no governance artifact that owns the sequencing decision.

4. **The `bmad-lens-batch` shared contract is mentioned in brainstorm Tier 1 cross-cutting infrastructure but doesn't appear in the product-brief scope.** If preplan, businessplan, techplan, and finalizeplan all use batch mode, and batch-ops infrastructure is missing, it's another Tier 1 blocker. Why was it dropped from the product-brief?

5. **Are there any stub prompts in the RELEASE module that redirect from deprecated command names to the new retained commands?** If a user types `/lens-plan` (a deprecated command), does the RELEASE module gracefully redirect or fail silently? The parity checklist doesn't include checking deprecated command handling — but the old-codebase baseline describes redirect stubs as a known pattern.

---

## Open Questions Surfaced

1. **OQ-1:** Define "minimal" for `bmad-lens-adversarial-review` before businessplan: port-faithful vs. feature-gated. Carry into businessplan research as required artifact.
2. **OQ-2:** Investigate `lc-agent-core-repo` relationship to Lens lifecycle and either include it in scope or explicitly exclude it.
3. **OQ-3:** Document governance-write atomicity requirements for `publish-to-governance` — specifically what constitutes a "safe" partial-failure state.
4. **OQ-4:** Investigate `bmad-lens-lessons` source-file absence and resolve before businessplan.
5. **OQ-5:** Address the RELEASE module ↔ new-codebase transition architecture — are they concurrent or is there a hard cutover? This affects every downstream feature's deployment assumptions.
