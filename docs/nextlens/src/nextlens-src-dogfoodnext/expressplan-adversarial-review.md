# Adversarial Review: nextlens-src-dogfoodnext / expressplan

**Reviewed:** 2026-05-15T00:00:00Z  
**Source:** phase-complete  
**Overall Rating:** pass-with-warnings

## Summary

The required ExpressPlan artifacts are present, and the packet is coherent enough to proceed to FinalizePlan. The plan correctly scopes implementation to a future Lens-owned NextLens bugfix skill, preserves the three-part intake contract, carries the target-only runtime write boundary, and acknowledges story-backed dev and peer review gates. No critical blocker was found. The material risks are handoff-level: the plan requires a `lens.core.src` skill to read control-repo design context and operate on the NextLens target repo without defining path resolution, and it does not yet specify how the new bugfix workflow composes with the existing NextLens Doctor and Salmon surfaces without creating parallel correction logic. FinalizePlan should convert these warnings into explicit stories, acceptance criteria, and validation checks.

Clarification captured after review: the new skill source belongs in `lens.core.src`. The skill's runtime fix target is `TargetProjects/nextlens/src/NextLens`.

Additional clarification captured after review: when the skill creates a bug report, it should store the artifact under a `nextlens` bug namespace such as `bugs/nextlens/QuickDev/{slug}.md`. FinalizePlan should preserve the deeper `/lens-core-bugfix` mechanics in this design: stable content-hash slug, fresh branch derived from the slug, mandatory target implementation commit, push and PR creation by the conductor, PR recording on the bug artifact, and closeout into the namespaced `Fixed` folder only after validation evidence exists.

## Findings

### Critical

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| - | - | No critical blockers found. Required current artifacts `business-plan.md` and `tech-plan.md` are present. | Proceed to FinalizePlan with warnings carried forward. |

### High

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| H1 | Cross-Feature Dependencies | The plan requires a Lens-owned skill in `lens.core.src` to read design context from `docs/nextlens/src` and then operate on `TargetProjects/nextlens/src/NextLens`. Without an explicit resolver, the skill may work only in this control workspace and fail when invoked from a different cwd or installed Lens module context. | FinalizePlan should add a story or acceptance criteria for `docs_path` and target-repo path resolution, overrides, failure behavior, and tests proving the bugfix skill can locate approved context while constraining runtime fixes to the NextLens target root. |
| H2 | Logic Flaws | The packet positions the bugfix skill near Salmon, Doctor, validation, and correction routing, but it does not define whether bugfix creates a new correction path or composes with the existing `nextlens-salmon` and `nextlens-doctor` capabilities. This can fork signal schemas, duplicate deduplication rules, or bypass established correction evidence. | FinalizePlan should define an integration contract: bugfix intake produces a fix specification, reuses existing Salmon normalization/deduplication where a signal exists, invokes or references Doctor validation rather than reimplementing it, and records which component owns closure evidence. |

### Medium / Low

| # | Severity | Dimension | Finding | Recommendation |
|---|----------|-----------|---------|----------------|
| M1 | Medium | Coverage Gaps | Chat history is a required input and the business plan notes sensitivity risk, but the sprint slices do not yet require redaction, minimization, or evidence-reference rules before durable artifacts are written. | Add acceptance criteria for summarizing sensitive transcripts, preserving source references, rejecting secrets where feasible, and avoiding unnecessary raw chat persistence. |
| M2 | Medium | Complexity and Risk | The TopDownLens bugfix guidance includes branch discipline, PR evidence, verification, and Salmon closure, while this plan focuses mainly on fix-spec generation and delegation. The handoff from fix spec to branch/PR/evidence capture remains under-specified. | Add a dedicated implementation slice or story criteria for correction branch preparation, validation evidence references, PR URL recording, and approved Salmon closure or supersession routing. |
| M3 | Medium | Coverage Gaps | Registration is called out, but the packet does not yet identify the Lens-generated surfaces that must stay synchronized or how aliases for the `lens.core.src` skill are validated. | FinalizePlan should require a registration matrix covering Lens skill folder, prompt stub or command alias, help output, release-sync metadata, setup refresh, and validation checks. |
| M4 | Medium | Assumptions and Blind Spots | The design-context loader is expected to select relevant guidance from a broad docs tree, but the selection rules are not testable yet. A noisy loader could overfit stale TopDownLens docs or miss the bugfix-flow constraints that justify the feature. | Require deterministic context selection rules, source-path reporting, conflict reporting, and fixture tests that prove the loader selects the bugfix guide and example without treating governance or release mirrors as write targets. |
| M5 | Medium | Coverage Gaps | The original packet did not specify where the generated bug report lives. Reusing the generic Lens core queue would blur ownership and make closeout ambiguous. | Store NextLens bug reports under a `nextlens` namespace below the bug store, mirror the existing status folders, and update create/record/close operations to resolve namespaced artifacts. |
| L1 | Low | Complexity and Risk | Windows and POSIX path-normalization tests are mentioned, but path casing and symlink or relative-path escape cases are not named explicitly. | Include path traversal, case variation, relative segment, and symlink or resolved-path cases in boundary enforcement tests where the platform supports them. |

## Accepted Risks

No risks were explicitly accepted by the operator during this delegated review. Treat the findings above as carry-forward planning constraints for FinalizePlan.

## Party-Mode Challenge

Winston (Architect): The runtime target write boundary is crisp, but the source/read boundary is not. If the skill is authored in `lens.core.src`, it still needs a real resolver contract for `docs/nextlens/src` and `TargetProjects/nextlens/src/NextLens` rather than a friendly cwd assumption.

Quinn (QA): The plan says noisy chat history should be summarized or referenced, but no test fails if raw transcript material is copied into durable output. That is exactly the kind of gap that looks fine in a happy-path demo and hurts later in review.

Sally (Release Engineer): The bugfix flow inherits branch, PR, and closure expectations from TopDownLens, but those expectations are not yet wired into the sprint slices. Someone needs to own the evidence trail from defect intake through validation and approved signal closure.

## Gaps You May Not Have Considered

1. What is the canonical way for a `lens.core.src` skill to locate control-repo design context and the NextLens target repo from different execution contexts? its in nextlens-src
2. Should bugfix routing create Salmon signals, consume existing Salmon signals, or support both with separate evidence rules? no not yet
3. Which exact artifact stores the fix specification, and is it durable by default or only passed in memory to an implementation delegate? it works the same as lens-core-bugfix
4. What transcript content is forbidden from durable artifacts, and what evidence reference is sufficient when raw chat cannot be copied? not a concern
5. Which namespaced bug artifact operation creates, records the PR, and closes `bugs/nextlens/...` without regressing existing Lens core bug reports? because its a different source code being modified this is not a concern

## Open Questions Surfaced

- FinalizePlan should decide whether the bugfix skill's design-context path and target repo path are configured, discovered from Lens feature metadata, or supplied explicitly by the operator. it should be explicit. this is a purpose built skill.
- FinalizePlan should define the ownership boundary between `nextlens-bugfix`, `nextlens-salmon`, and `nextlens-doctor`. nextlens-bugfix is completely separate than other nextlens skills because its truly lens-nextlens-bugfix
- FinalizePlan should include the `bugs/nextlens/{status}/{slug}.md` namespace and specify whether it is implemented by extending existing bug reporter operations or by a NextLens-specific wrapper. nextlens specific
- FinalizePlan should specify whether high and blocking Salmon inputs automatically require a dedicated correction branch or only require the generated fix specification to recommend one. not in scope
- FinalizePlan should define durable evidence storage rules for summarized chat history and validation output references. not a concern
- FinalizePlan should require registration and discovery validation before any implementation delegation behavior is added. 