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
responses_recorded: true
updated_at: '2026-05-01T00:00:00Z'
---

# Adversarial Review: lens-dev-new-codebase-dogfood / expressplan

**Reviewed:** 2026-05-01T00:00:00Z  
**Source:** phase-complete  
**Overall Rating:** pass-with-warnings

## Summary

The business, technical, and sprint plans are coherent and usable for a clean-room dogfood implementation slice. The packet correctly centers the baseline 17-command contract, observed target gaps, and bugfix backlog. The review passes with warnings because no critical contradiction prevents planning handoff, but two high-risk issues must remain visible: the target is missing foundational state/dev capabilities, and the control repo topology has changed from the earlier 2-branch/flat discussion to an explicit three-branch model.

## Response Record

| Finding / Question | Response | Recorded action |
| --- | --- | --- |
| H1 | D | Include stories for the missing foundational files and skills: lifecycle contract, module config, feature-yaml, git-state, and Dev conductor. |
| H2 | D | Control repo now uses `{featureId}`, `{featureId}-plan`, and `{featureId}-dev`; planning before FinalizePlan goes to plan, FinalizePlan goes to base, FinalizePlan step 3 goes to dev, and branches are cleaned locally/remotely after PR merge. |
| M1 | A | Keep target QuickPlan internal-only and document that product decision unless a later feature changes it. |
| M2 | A | Canonicalize on current lifecycle naming while reading legacy express review names; record this as retrospective tech debt. |
| M3 | D | Always commit and push if dirty before continuing. The dirty governance `bugfixes.md` change was committed and pushed before metadata updates. |
| M4 | A | Update `target_repos` through feature-yaml and add a fix so implementation-impacting features cannot silently omit target repo registration. |
| L1 | A | Standardize `uv run python -m pytest` for Windows validation. |
| L2 | A | Add duplicate/missing inventory validation for module metadata, prompt stubs, release prompts, and help surfaces. |
| Gap 1 | No | A target command is not parity-complete without the public prompt chain. |
| Gap 2 | External | Clean-room file hash checks happen outside VS Code. |
| Gap 3 | Best judgement | Prefer a sanctioned sync operation attached to feature-yaml or git-orchestration; choose based on implementation fit. |
| Gap 4 | Not valid | Flat topology is not valid for the control repo. Governance is flat; target projects may choose flat, `feature/{featureStub}`, or `feature/{featureStub}-{username}` strategies. |
| Gap 5 | Best judgement | Implement the smallest Dev conductor that validates FinalizePlan handoff, target repo resolution, branch prep, and resumable session state before expanding story execution depth. |

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

**Response:** D - Include the needed stories in order to create these missing files. The sprint plan now explicitly includes stories for `lifecycle.yaml`, `bmadconfig.yaml`, `bmad-lens-feature-yaml`, `bmad-lens-git-state`, and `bmad-lens-dev`.

**Response Options**

- **A.** Keep foundations as blocking P0 scope - **Why pick this:** Prevents shallow parity claims. / **Why not:** Delays visible command polish.
- **B.** Split foundations into a separate feature - **Why pick this:** Reduces this feature's scope. / **Why not:** Leaves dogfood unable to complete its own workflow.
- **C.** Implement only stubs for missing foundations - **Why pick this:** Speeds metadata parity. / **Why not:** Creates runtime failures at the next gate.
- **D.** Write your own response - provide a custom resolution not covered by A, B, or C.
- **E.** Keep as-is - explicitly record that no action will be taken on this finding.

#### H2 - Topology policy contradiction can break branch semantics

**Dimension:** Logic Flaws  
**Finding:** The earlier packet mixed baseline 2-branch language with BF-6 flat-control-topology language. The resolved model is now: governance repo is always flat; control repo has three feature branches; target projects have independent branch strategy choices.  
**Recommendation:** Implement the control repo as `{featureId}`, `{featureId}-plan`, and `{featureId}-dev`. Planning items before FinalizePlan go to `{featureId}-plan`; FinalizePlan goes to `{featureId}`; FinalizePlan step 3 goes to `{featureId}-dev`. After each PR lands, clean up local and remote branches, switch to the correct next branch, and pull before continuing.

**Response:** D - Apply the three-branch control topology and keep target-project branch strategy separate.

**Response Options**

- **A.** Implement the three-branch control topology immediately - **Why pick this:** Matches the selected model. / **Why not:** Requires updates across git-state, git-orchestration, FinalizePlan, and cleanup logic.
- **B.** Stage the three-branch topology behind compatibility checks - **Why pick this:** Reduces migration risk. / **Why not:** Slows down the dogfood correction.
- **C.** Keep legacy topology temporarily and document migration blockers - **Why pick this:** Avoids a rushed topology migration. / **Why not:** Leaves the known topology issue unresolved.
- **D.** Write your own response - provide a custom resolution not covered by A, B, or C.
- **E.** Keep as-is - explicitly record that no action will be taken on this finding.

### Medium

#### M1 - QuickPlan parity shape is ambiguous

**Dimension:** Assumptions and Blind Spots  
**Finding:** The reference module describes QuickPlan as an end-to-end planning conductor, while the target QuickPlan is internal-only for ExpressPlan. Implementation cannot leave both meanings in circulation without documentation and tests.  
**Recommendation:** Keep target QuickPlan internal-only for this dogfood scope and align prompt inventory, registry, help text, and tests to that decision.

**Response:** A - Keep target QuickPlan internal-only and document the product decision.

#### M2 - Express review artifact naming drift remains unresolved

**Dimension:** Cross-Feature Dependencies  
**Finding:** Current lifecycle and the loaded ExpressPlan skill use `expressplan-adversarial-review.md`, while baseline and several existing artifacts reference `expressplan-review.md`. Tooling that assumes only one name can miss artifacts.  
**Recommendation:** Treat `expressplan-adversarial-review.md` as current for this feature and add compatibility mapping for existing `expressplan-review.md` references where publishing or validation reads older packets.

**Response:** A - Canonicalize on the current lifecycle artifact while adding compatibility reads. Record this as tech debt for the retrospective.

#### M3 - Governance working tree is dirty before phase advancement

**Dimension:** Complexity and Risk  
**Finding:** The governance repo had an existing uncommitted modification to `features/lens-dev/new-codebase/bugfixes.md`. The selected response changes the operating rule: dirty control or governance repo changes should be committed and pushed before continuing.  
**Recommendation:** Always commit and push dirty repo changes before phase advancement, metadata updates, or publication steps.

**Response:** D - Always commit and push if dirty. The dirty governance backlog change was committed and pushed before the feature metadata update.

#### M4 - Target repository registration is absent from dogfood feature state

**Dimension:** Coverage Gaps  
**Finding:** The dogfood `feature.yaml` has `target_repos: []` even though the implementation target is `TargetProjects/lens-dev/new-codebase/lens.core.src`. Cross-feature and Dev tooling will have less context until that field is populated through the sanctioned feature-yaml path.  
**Recommendation:** Add a feature-yaml operation or follow-up step to register `lens.core.src` as a target repo after governance preconditions are clean.

**Response:** A - Update `target_repos` through feature-yaml after preconditions are clean and include a fix so this omission does not recur. The dogfood feature metadata now registers `lens.core.src`; implementation stories require missing-target-repo validation for implementation-impacting features.

### Low

#### L1 - Windows pytest invocation needs standardization

**Dimension:** Complexity and Risk  
**Finding:** The discover retrospective reports `uv run --with pytest pytest` spawn friction on Windows and recommends `uv run python -m pytest`.  
**Recommendation:** Use the Windows-safe command form in implementation stories and validation notes.

**Response:** A - Standardize `uv run python -m pytest`.

#### L2 - Target module metadata already contains drift

**Dimension:** Coverage Gaps  
**Finding:** Target `module.yaml` lists `lens-expressplan.prompt.md` twice and does not represent the full retained surface.  
**Recommendation:** Add an inventory validation story that rejects duplicates and missing retained commands across module metadata, prompt stubs, release prompts, and help surfaces.

**Response:** A - Add duplicate/missing inventory validation.

## Accepted Risks

- QuickPlan remains internal-only for this dogfood scope; any broader standalone QuickPlan parity is compatibility debt unless a later feature reopens it.
- Express review filename compatibility remains tech debt for the retrospective: current lifecycle naming is canonical, but older `expressplan-review.md` artifacts must be recognized when read or published.
- File hash clean-room verification is handled outside VS Code and is not implemented in this planning packet.

## Party-Mode Challenge

John (PM): The plan is honest about the gap, but it risks turning dogfood into a full rebuild of everything. What is the first user-visible success signal? I would define it as: a retained command can run from stub to skill, resolve feature context, validate artifacts, and stop with a useful blocker rather than crashing.

Winston (Architect): The target has conductors before it has foundations. That ordering is backwards for runtime reliability. I want Sprint 1 to make the architecture boring: config resolves, lifecycle loads, feature state reads, branch state reports. Then the conductors can become thinner and safer.

Bob (QA): The baseline says 17 commands, but the target has mismatched stubs, prompts, and module metadata. A command inventory validator should be a test, not a checklist. Otherwise every sprint will reintroduce drift.

## Gaps You May Not Have Considered

1. A target command is not parity-complete if it cannot be invoked through the public prompt chain.
2. Clean-room file hash checks will happen outside VS Code.
3. Use best judgement for feature-index synchronization placement; prefer the sanctioned operation that keeps phase transitions and index state atomic.
4. Flat topology is not valid for the control repo. Governance is flat, and target projects independently support flat, `feature/{featureStub}`, or `feature/{featureStub}-{username}` strategies.
5. Use best judgement for the minimum Dev conductor: start with FinalizePlan validation, target repo resolution, branch prep, and resumable session state before expanding full story execution.

## Open Questions Surfaced

- Keep QuickPlan internal-only for this dogfood scope and document any broader parity debt.
- Implement the three-branch control repo topology and keep target-project branch strategy separate.
- Commit and push dirty repo changes before phase advancement or publish operations.
- Keep `lens.core.src` registered in feature metadata and add validation so implementation-impacting features do not omit target repos.
- Add inventory validation to prevent prompt/module/help drift from becoming a recurring parity bug.
