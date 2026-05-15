# Adversarial Review: nextlens-src-dogfoodnext / expressplan

**Reviewed:** 2026-05-15T00:00:00Z  
**Source:** phase-complete  
**Overall Rating:** pass-with-warnings

## Summary

The required ExpressPlan artifacts are present, and the packet is coherent enough to proceed to FinalizePlan. The plan correctly scopes implementation to a future NextLens bugfix skill, preserves the three-part intake contract, carries the target-only write boundary, and acknowledges story-backed dev and peer review gates. No critical blocker was found. The material risks are handoff-level: the plan assumes a target-repo skill can read control-repo design context without defining path resolution, and it does not yet specify how the new bugfix workflow composes with the existing NextLens Doctor and Salmon surfaces without creating parallel correction logic. FinalizePlan should convert these warnings into explicit stories, acceptance criteria, and validation checks.

## Findings

### Critical

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| - | - | No critical blockers found. Required current artifacts `business-plan.md` and `tech-plan.md` are present. | Proceed to FinalizePlan with warnings carried forward. |

### High

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| H1 | Cross-Feature Dependencies | The plan requires the future target-repo skill to read design context from `docs/nextlens/src`, but the current NextLens module configuration defaults its docs root inside the consuming project. Without an explicit resolver, the skill may work only in this control workspace and fail when installed or invoked from `TargetProjects/nextlens/src/NextLens` or another project. | FinalizePlan should add a story or acceptance criteria for `docs_path` resolution, overrides, failure behavior, and tests proving the bugfix skill can locate approved context from both control-repo and installed-module execution modes. |
| H2 | Logic Flaws | The packet positions the bugfix skill near Salmon, Doctor, validation, and correction routing, but it does not define whether bugfix creates a new correction path or composes with the existing `nextlens-salmon` and `nextlens-doctor` capabilities. This can fork signal schemas, duplicate deduplication rules, or bypass established correction evidence. | FinalizePlan should define an integration contract: bugfix intake produces a fix specification, reuses existing Salmon normalization/deduplication where a signal exists, invokes or references Doctor validation rather than reimplementing it, and records which component owns closure evidence. |

### Medium / Low

| # | Severity | Dimension | Finding | Recommendation |
|---|----------|-----------|---------|----------------|
| M1 | Medium | Coverage Gaps | Chat history is a required input and the business plan notes sensitivity risk, but the sprint slices do not yet require redaction, minimization, or evidence-reference rules before durable artifacts are written. | Add acceptance criteria for summarizing sensitive transcripts, preserving source references, rejecting secrets where feasible, and avoiding unnecessary raw chat persistence. |
| M2 | Medium | Complexity and Risk | The TopDownLens bugfix guidance includes branch discipline, PR evidence, verification, and Salmon closure, while this plan focuses mainly on fix-spec generation and delegation. The handoff from fix spec to branch/PR/evidence capture remains under-specified. | Add a dedicated implementation slice or story criteria for correction branch preparation, validation evidence references, PR URL recording, and approved Salmon closure or supersession routing. |
| M3 | Medium | Coverage Gaps | Registration is called out for `module.yaml`, help metadata, discovery, and doctor validation, but the packet does not identify the generated surfaces that must stay synchronized or how duplicate aliases such as `bmad-nextlens-bugfix` and `nextlens-bugfix` are validated. | FinalizePlan should require a registration matrix covering skill folder, command alias, module capability, help output, setup refresh, and doctor/module validation checks. |
| M4 | Medium | Assumptions and Blind Spots | The design-context loader is expected to select relevant guidance from a broad docs tree, but the selection rules are not testable yet. A noisy loader could overfit stale TopDownLens docs or miss the bugfix-flow constraints that justify the feature. | Require deterministic context selection rules, source-path reporting, conflict reporting, and fixture tests that prove the loader selects the bugfix guide and example without treating governance or release mirrors as write targets. |
| L1 | Low | Complexity and Risk | Windows and POSIX path-normalization tests are mentioned, but path casing and symlink or relative-path escape cases are not named explicitly. | Include path traversal, case variation, relative segment, and symlink or resolved-path cases in boundary enforcement tests where the platform supports them. |

## Accepted Risks

No risks were explicitly accepted by the operator during this delegated review. Treat the findings above as carry-forward planning constraints for FinalizePlan.

## Party-Mode Challenge

Winston (Architect): The target write boundary is crisp, but the read boundary is not. If the skill runs from the target repo, `docs/nextlens/src` is no longer an obvious local path, so the first story needs a real resolver contract rather than a friendly assumption.

Quinn (QA): The plan says noisy chat history should be summarized or referenced, but no test fails if raw transcript material is copied into durable output. That is exactly the kind of gap that looks fine in a happy-path demo and hurts later in review.

Sally (Release Engineer): The bugfix flow inherits branch, PR, and closure expectations from TopDownLens, but those expectations are not yet wired into the sprint slices. Someone needs to own the evidence trail from defect intake through validation and approved signal closure.

## Gaps You May Not Have Considered

1. What is the canonical way for an installed NextLens skill to locate control-repo design context when the project root is the target repo?
2. Should bugfix routing create Salmon signals, consume existing Salmon signals, or support both with separate evidence rules?
3. Which exact artifact stores the fix specification, and is it durable by default or only passed in memory to an implementation delegate?
4. What transcript content is forbidden from durable artifacts, and what evidence reference is sufficient when raw chat cannot be copied?
5. Which command or validator proves the command alias, module metadata, help text, shared scripts, and doctor checks are synchronized?

## Open Questions Surfaced

- FinalizePlan should decide whether the bugfix skill's design-context path is configured, discovered from Lens feature metadata, or supplied explicitly by the operator.
- FinalizePlan should define the ownership boundary between `nextlens-bugfix`, `nextlens-salmon`, and `nextlens-doctor`.
- FinalizePlan should specify whether high and blocking Salmon inputs automatically require a dedicated correction branch or only require the generated fix specification to recommend one.
- FinalizePlan should define durable evidence storage rules for summarized chat history and validation output references.
- FinalizePlan should require registration and discovery validation before any implementation delegation behavior is added.