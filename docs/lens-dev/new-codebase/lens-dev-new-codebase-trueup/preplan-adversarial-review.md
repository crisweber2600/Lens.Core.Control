---
phase: preplan
verdict: B
review_format: abc-choice-v1
reviewed: 2026-04-28T01:15:00Z
source: phase-complete
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

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| H1 | Coverage Gaps | SC-3 ("complete Delivery Package Scaffolded") requires test stubs for the happy path and the prerequisite-missing degradation path of `bmad-lens-complete`, but no specification of the test framework, fixtures, or how tests run is provided. An unspecified test strategy for a command with an irreversible finalization gate is a high risk: implementation will stall when the test question surfaces mid-sprint. | Add "test harness strategy for `bmad-lens-complete` test stubs" as a required artifact or decision record in businessplan. At minimum: document whether existing `pytest` patterns (as used in init-feature and switch) are sufficient, or whether a new integration test harness is needed. |
| H2 | Complexity and Risk | `publish-to-governance` touches `governance-main` via `git commit + push`, but the product-brief explicitly places shared infrastructure (including `publish-to-governance`) **out of scope** for True Up. This finding is recorded for carry-forward: when `publish-to-governance` is scoped into an individual preplan feature planning cycle, safeguards must be specified — at minimum a dry-run flag, a test-governance-repo fixture path, and a pre-write pull + conflict-abort sequence. | Carry forward to the preplan feature that owns `publish-to-governance`. Not a True Up blocker. |
| H3 | Complexity and Risk | `bmad-lens-adversarial-review` is also explicitly **out of scope** for True Up (shared infrastructure deferred to individual preplan feature planning cycles). The scope definition question ("minimal" vs. port-faithful) must be resolved when the feature owning `bmad-lens-adversarial-review` activates. | Carry forward to the preplan feature that owns `bmad-lens-adversarial-review`. Not a True Up blocker. |

---

### Medium

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| M1 | Logic Flaws | The product-brief lists "update governance phase labels for new-feature" (SC-2) as a True Up task. Updating governance labels uses `bmad-lens-feature-yaml` update operations — which is itself a Tier 1 True Up output. The task requires the tool it is building. This is a sequencing dependency within True Up: SC-2 audit tasks that require feature-yaml updates cannot be completed until Tier 1 is done. The current plan doesn't acknowledge this ordering. | Explicitly sequence SC-2 audit tasks: the parity report (SC-2 first half) can be written before Tier 1, but governance phase label corrections (SC-2 second half) must wait until `bmad-lens-feature-yaml` update ops are implemented. |
| M2 | Coverage Gaps | The product-brief describes a "parity checklist" (SC-3) as "runnable" via possible extension of `validate-phase-artifacts.py`. But `validate-phase-artifacts.py` checks for file existence, not behavioral parity. Behavioral parity — verifying that `init-feature-ops.py create` produces output matching the old-codebase contract — requires a different kind of test runner. SC-3 conflates two distinct verification modes (structural and behavioral). | Split SC-3 into SC-3a (structural: file presence, SKILL.md present, prompts published) and SC-3b (behavioral: output contract tests). Acknowledge that SC-3b may require a new test fixture or remain as a manual checklist until a behavioral test harness is built. |
| M3 | Cross-Feature Dependencies | The `complete` feature is at `finalizeplan-complete` (dev-ready). True Up's Tier 4 task ("document graceful-degradation vs. hard-prerequisite decision for complete") implies the decision hasn't been made. If the complete feature starts dev before True Up publishes this decision, there will be a design conflict mid-sprint. No coordination mechanism is specified between True Up and the complete feature's dev activation. | Add a dependency note to the complete feature's governance docs: do not activate dev until True Up publishes the architectural decision record for prerequisite handling. This should be documented as a blocker in `feature.yaml` for `lens-dev-new-codebase-complete`. |
| M4 | Assumptions and Blind Spots | The artifacts assume the RELEASE module (`lens.core/`) is the definitive ground truth for old-codebase behavior. But the old-codebase discovery docs describe an earlier state of the release module, and the RELEASE module may have been patched since. The baseline research.md 17-command traceability matrix is the authoritative reference — but it was written before the RELEASE module's current state was fully audited against that matrix. | Acknowledge that the parity baseline is the governance baseline research.md traceability matrix, not the current RELEASE module binary. Any RELEASE module changes since the baseline research was written are out of scope unless they affect the new-codebase target behavior. |
| M5 | Coverage Gaps | The `lc-agent-core-repo` skill exists in the new-codebase source (`skills/lc-agent-core-repo/`) with a full package (SKILL.md, scripts, assets, references). It is not mentioned anywhere in the brainstorm, research, or product-brief. Its relationship to the Lens lifecycle and to the 17 retained commands is unaddressed — it may be a separate non-Lens skill, a utility, or an undocumented 18th command. | Add "investigate lc-agent-core-repo relationship to Lens lifecycle" as a businessplan research task. If it is out of scope for the Lens rewrite, document that explicitly. |

---

### Low

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| L1 | Coverage Gaps | `bmad-lens-lessons` has compiled `.pyc` files in the new-codebase source but no `.py` source files. Research lists it as an open question. A `.pyc`-only skill is not usable without the source. This could be a gitignore issue or an abandoned partial delivery. | Investigate before businessplan: check `.gitignore` in the new-codebase source for `*.py` exclusion patterns. If the source is gitignored, fix the `.gitignore`. If the skill is abandoned, explicitly mark it out of scope in governance. |
| L2 | Logic Flaws | `bmad-lens-complete/references/finalize-feature.md` was identified in the filesystem inventory but not examined. It could be a planning artifact from an earlier session that describes the complete command contract in detail — potentially resolving some of the SKILL.md gap flagged in the audit. | Read `finalize-feature.md` during businessplan research to determine if it has implementation contract detail that should be incorporated into the SKILL.md scaffold task. |
| L3 | Coverage Gaps | The product-brief does not address operational monitoring for `publish-to-governance` failures. If a governance publish fails mid-commit, there may be partial writes to `feature.yaml` or `feature-index.yaml` that leave governance in an inconsistent state. | Add governance-write atomicity as a non-functional requirement in businessplan. At minimum: `publish-to-governance` must emit a structured error on any failure and must not partially commit. |
| L4 | Assumptions and Blind Spots | SKILL.md files are the agent-discovery mechanism — agents load SKILL.md to know how to invoke a skill. The product-brief tasks list building scripts and tests for Tier 1 infrastructure but does not explicitly list "SKILL.md for adversarial-review" or "SKILL.md for feature-yaml" as required deliverables. Without SKILL.md files, these skills are invisible to agent sessions even when their scripts work. | Explicitly add SKILL.md creation as a required deliverable for each Tier 1 skill in the businessplan scope definition. |

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
