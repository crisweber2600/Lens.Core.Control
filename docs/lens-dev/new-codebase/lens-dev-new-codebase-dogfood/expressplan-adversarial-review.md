---
feature: lens-dev-new-codebase-dogfood
doc_type: adversarial-review
phase: expressplan
source: phase-complete
verdict: pass-with-warnings
status: review-complete
critical_count: 0
high_count: 2
medium_count: 4
low_count: 2
review_format: abc-choice-v1
updated_at: '2026-05-01T00:00:00Z'
---

# Adversarial Review: lens-dev-new-codebase-dogfood / expressplan

**Reviewed:** 2026-05-01T00:00:00Z  
**Source:** phase-complete  
**Overall Rating:** pass-with-warnings

## Summary

The business, technical, and sprint plans are coherent and usable for a clean-room dogfood implementation slice. The packet correctly centers the baseline 17-command contract, observed target gaps, and bugfix backlog. The review passes with warnings because no critical contradiction prevents planning handoff, but two high-risk issues must remain visible: the target is missing foundational state/dev capabilities, and topology policy is unresolved between the baseline 2-branch invariant and BF-6 flat-default request.

## Findings

### Critical

| # | Dimension | Finding | Recommendation |
| --- | --- | --- | --- |
| - | - | No critical blockers found. | Continue only with the warnings below carried into implementation. |

### High

#### H1 - Foundational parity gap is larger than a command-surface gap

**Dimension:** Coverage Gaps  
**Finding:** The target has some phase conductors, but it lacks `lifecycle.yaml`, `bmadconfig.yaml`, `bmad-lens-feature-yaml`, `bmad-lens-git-state`, and `bmad-lens-dev`. Without those foundations, retained commands can exist as files while failing at runtime.  
**Recommendation:** Keep Sprint 1 and Sprint 4 as mandatory parity foundations. Do not declare ExpressPlan or FinalizePlan parity complete until the state and Dev handoff layers exist.

**Response Options**

- **A.** Keep foundations as blocking P0 scope - **Why pick this:** Prevents shallow parity claims. / **Why not:** Delays visible command polish.
- **B.** Split foundations into a separate feature - **Why pick this:** Reduces this feature's scope. / **Why not:** Leaves dogfood unable to complete its own workflow.
- **C.** Implement only stubs for missing foundations - **Why pick this:** Speeds metadata parity. / **Why not:** Creates runtime failures at the next gate.
- **D.** Write your own response - provide a custom resolution not covered by A, B, or C.
- **E.** Keep as-is - explicitly record that no action will be taken on this finding.

#### H2 - Topology policy contradiction can break branch semantics

**Dimension:** Logic Flaws  
**Finding:** The baseline freezes a 2-branch topology, while BF-6 says the control repo should default to flat topology with no mandatory plan PR. The plan proposes configuration, but implementation must decide exact defaults and compatibility behavior before git-orchestration changes land.  
**Recommendation:** Make topology an explicit config field with tests for both flat and 2-branch behavior. Do not let branch creation silently infer topology.

**Response Options**

- **A.** Add topology config and support both modes - **Why pick this:** Reconciles baseline compatibility with dogfood feedback. / **Why not:** More tests and branching logic.
- **B.** Keep 2-branch as default until migration is designed - **Why pick this:** Lowest compatibility risk. / **Why not:** Ignores the critical friction report.
- **C.** Switch to flat only - **Why pick this:** Simplifies new workflows. / **Why not:** Breaks existing feature assumptions.
- **D.** Write your own response - provide a custom resolution not covered by A, B, or C.
- **E.** Keep as-is - explicitly record that no action will be taken on this finding.

### Medium

#### M1 - QuickPlan parity shape is ambiguous

**Dimension:** Assumptions and Blind Spots  
**Finding:** The reference module describes QuickPlan as an end-to-end planning conductor, while the target QuickPlan is internal-only for ExpressPlan. Implementation cannot leave both meanings in circulation without documentation and tests.  
**Recommendation:** Decide whether QuickPlan is public-compatible or internal-only, then align prompt inventory, registry, help text, and tests.

#### M2 - Express review artifact naming drift remains unresolved

**Dimension:** Cross-Feature Dependencies  
**Finding:** Current lifecycle and the loaded ExpressPlan skill use `expressplan-adversarial-review.md`, while baseline and several existing artifacts reference `expressplan-review.md`. Tooling that assumes only one name can miss artifacts.  
**Recommendation:** Treat `expressplan-adversarial-review.md` as current for this feature and add compatibility mapping for existing `expressplan-review.md` references where publishing or validation reads older packets.

#### M3 - Governance working tree is dirty before phase advancement

**Dimension:** Complexity and Risk  
**Finding:** The governance repo has an existing uncommitted modification to `features/lens-dev/new-codebase/bugfixes.md`. The planning packet reads it as user-provided input, but phase advancement, governance publication, or commits should not proceed until the user-owned change is resolved.  
**Recommendation:** Stop before governance state updates. Ask for the user change to be committed, stashed, or explicitly included through the proper governance workflow before ExpressPlan step 3.

#### M4 - Target repository registration is absent from dogfood feature state

**Dimension:** Coverage Gaps  
**Finding:** The dogfood `feature.yaml` has `target_repos: []` even though the implementation target is `TargetProjects/lens-dev/new-codebase/lens.core.src`. Cross-feature and Dev tooling will have less context until that field is populated through the sanctioned feature-yaml path.  
**Recommendation:** Add a feature-yaml operation or follow-up step to register `lens.core.src` as a target repo after governance preconditions are clean.

### Low

#### L1 - Windows pytest invocation needs standardization

**Dimension:** Complexity and Risk  
**Finding:** The discover retrospective reports `uv run --with pytest pytest` spawn friction on Windows and recommends `uv run python -m pytest`.  
**Recommendation:** Use the Windows-safe command form in implementation stories and validation notes.

#### L2 - Target module metadata already contains drift

**Dimension:** Coverage Gaps  
**Finding:** Target `module.yaml` lists `lens-expressplan.prompt.md` twice and does not represent the full retained surface.  
**Recommendation:** Add an inventory validation story that rejects duplicates and missing retained commands across module metadata, prompt stubs, release prompts, and help surfaces.

## Accepted Risks

No risks have been explicitly accepted by the user in this review. The packet may proceed to implementation planning with the high and medium findings carried forward as sprint acceptance criteria.

## Party-Mode Challenge

John (PM): The plan is honest about the gap, but it risks turning dogfood into a full rebuild of everything. What is the first user-visible success signal? I would define it as: a retained command can run from stub to skill, resolve feature context, validate artifacts, and stop with a useful blocker rather than crashing.

Winston (Architect): The target has conductors before it has foundations. That ordering is backwards for runtime reliability. I want Sprint 1 to make the architecture boring: config resolves, lifecycle loads, feature state reads, branch state reports. Then the conductors can become thinner and safer.

Bob (QA): The baseline says 17 commands, but the target has mismatched stubs, prompts, and module metadata. A command inventory validator should be a test, not a checklist. Otherwise every sprint will reintroduce drift.

## Gaps You May Not Have Considered

1. Should a target command be considered parity-complete if it produces the right artifact but cannot be invoked from the public prompt chain?
2. How will implementation agents prove clean-room behavior when the easiest comparison is a reference file with tempting structure?
3. Should `feature-index.yaml` sync happen inside feature-yaml operations, git-orchestration, or a dedicated governance-index operation?
4. Does flat topology change `next`, `finalizeplan`, and `complete` routing semantics, or only branch creation?
5. What is the minimum Dev conductor needed to dogfood this rebuild without implementing the entire story execution loop at once?

## Open Questions Surfaced

- Decide QuickPlan public-versus-internal shape before Sprint 3 closes.
- Decide topology default and compatibility behavior before Sprint 2 branch work lands.
- Resolve governance dirty state before phase advancement or publish operations.
- Register `lens.core.src` as a target repo through sanctioned feature-yaml flow once governance writes are allowed.
- Add inventory validation to prevent prompt/module/help drift from becoming a recurring parity bug.
