# Adversarial Review: nextlens-src-topdownlens / expressplan

**Reviewed:** 2026-05-14T01:40:00Z  
**Source:** phase-complete  
**Overall Rating:** pass-with-warnings

## Summary

The ExpressPlan artifact set is coherent enough to proceed to FinalizePlan. The business plan, tech plan, and sprint plan all converge on the same module thesis: TopDownLens should reframe LENS around feature orchestration, top-down discovery, bottom-up growth, living landscape topology, BMAD bridging, and Salmon upstream validation. No critical blocker was found. The main risks are scope control, storage-location ambiguity, promotion-threshold ambiguity, and the lack of runtime cross-feature context support in the installed `init-feature-ops.py`.

## Findings

### Critical

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| - | - | No critical blockers found. | Proceed to FinalizePlan with warnings carried forward. |

### High

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| H1 | Complexity and Risk | The module thesis is broad enough to sprawl into a full framework rewrite if FinalizePlan does not constrain the first implementation increment. | FinalizePlan should keep the first dev scope to schemas, storage topology, one top-down walkthrough, bottom-up compatibility rules, BMAD packet generation, minimal derived graph rebuild, and Salmon signal recording. |
| H2 | Coverage Gaps | The planned storage topology names `_bmad-output/lens/` as a future module home, but the current Lens feature workflow writes under `docs/`. | FinalizePlan should make an explicit ADR for initial storage location and migration/bridge behavior. |

### Medium / Low

| # | Severity | Dimension | Finding | Recommendation |
|---|----------|-----------|---------|----------------|
| M1 | Medium | Assumptions and Blind Spots | Promotion thresholds are described conceptually, but the sprint plan does not yet define concrete evidence counts or decision gates. | Add story acceptance criteria for repeated pressure categories and examples before implementing promotion suggestions. |
| M2 | Medium | Coverage Gaps | Salmon is well-framed as upstream consistency validation, but routing from signal severity to action is still early. | FinalizePlan should require a signal routing table covering local note, landscape update, BMAD correct-course, split feature, and block promotion. |
| M3 | Medium | Cross-Feature Dependencies | The review attempted to load cross-feature context with `fetch-context --depth full`, but this installed runtime does not expose the subcommand. | Carry this as an implementation environment gap; do not make TopDownLens depend on unavailable context-loading until the runtime surface is restored or replaced. |
| M4 | Low | Logic Flaws | The business and tech plans intentionally demote domain/service/repo as planning roots, but compatibility with the current Lens feature lifecycle is only stated at a high level. | FinalizePlan should include a compatibility story or ADR so current `/new-feature` flows can map to future `feature.<slug>` identity without breaking governance. |

## Accepted Risks

No risks were explicitly accepted by the user during this review. The warnings above should be treated as carry-forward planning constraints for FinalizePlan.

## Party-Mode Challenge

John (Product): The artifact set has a strong philosophy, but the user value of the first build must stay concrete. If the first sprint only creates schemas, stakeholders may not feel the new Lens is useful. FinalizePlan should include one demonstrable top-down workflow from raw context to BMAD packet.

Winston (Architect): The source-of-truth split is correct, but it is also the highest-risk technical decision. If archive, landscape, and graph can drift, the module will create more confusion than it removes. The rebuild path and Doctor checks need to be first-class, not later polish.

Quinn (QA): Salmon is the differentiator, but it needs testable behavior. A signal that says an implementation invalidated an upstream assumption must have fixture examples, expected routing, and a clear non-mutating validation mode before agents rely on it.

## Gaps You May Not Have Considered

1. What is the smallest user-visible demo that proves TopDownLens is more than a document taxonomy?
2. Which artifact is allowed to rename or reorganize a landscape entity while preserving stable IDs?
3. How will the module prevent weak adjacencies from being interpreted as approved architecture?
4. What happens when BMAD architecture contradicts an earlier LENS journey or capability hypothesis?
5. Should Salmon signals block dev, block promotion, or only annotate the landscape by default?

## Open Questions Surfaced

- FinalizePlan should decide whether `_bmad-output/lens/` or `docs/` is the first authoritative module output root.
- FinalizePlan should define the minimum relationship lifecycle states required for MVP implementation.
- FinalizePlan should decide whether bottom-up `lens feature new` ships in the first dev increment or remains a documented compatibility constraint.
- FinalizePlan should address the missing `fetch-context` runtime subcommand if cross-feature context is required by the first implementation.
- FinalizePlan should define what evidence is required for capability promotion suggestions.