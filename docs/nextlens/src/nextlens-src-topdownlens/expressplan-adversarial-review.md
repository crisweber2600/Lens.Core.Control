---
feature: nextlens-src-topdownlens
doc_type: adversarial-review
phase: expressplan
source: manual-rerun
verdict: pass-with-warnings
status: responses-recorded
critical_count: 0
high_count: 2
medium_count: 5
low_count: 0
carry_forward_blockers: []
updated_at: '2026-05-14T02:58:59Z'
review_format: abc-choice-v1
---

# Adversarial Review: nextlens-src-topdownlens / expressplan

**Reviewed:** 2026-05-14T02:58:59Z  
**Source:** manual-rerun  
**Overall Rating:** pass-with-warnings

## Summary

This manual rerun confirms that the terminology pass improved the packet materially. The business plan, tech plan, and sprint plan now align on a clear top-down hierarchy of `System -> Product Area -> Outcome -> Journey -> Capability -> Feature`, and they consistently treat `feature` as the unit selected for BMAD planning. No critical blocker was introduced by the rename. The main carry-forward risks are still scope control, output-root ambiguity, promotion and signal-routing thresholds, and a now-sharper identity collision between the current Lens control-repo feature lifecycle container and the future TopDownLens `feature.<slug>` operational unit. The `nextlens` domain and `src` service constitutions are informational and do not add blocking constraints beyond review enforcement and the presence of a business plan.

## Findings

### Critical

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| - | - | No critical blockers found. | Proceed to FinalizePlan with warnings carried forward. |

### High

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| H1 | Complexity and Risk | The module thesis is still broad enough to sprawl into a full framework rewrite if FinalizePlan does not hold the first implementation increment to one demonstrable workflow and a thin set of supporting contracts. | FinalizePlan should keep the first dev scope to schemas, storage topology, one top-down walkthrough, one bottom-up compatibility example, BMAD packet generation, minimal derived graph rebuild, and Salmon signal recording. |
| H2 | Coverage Gaps | The planned storage topology still names `_bmad-output/lens/` as the future module home while the current Lens workflow writes under `docs/`. The rename to `feature` did not resolve where authoritative files live first. | FinalizePlan should make an explicit ADR for the initial output root, bridge behavior, and migration rules between staged control-repo docs and future module-owned files. |

### Medium / Low

| # | Severity | Dimension | Finding | Recommendation |
|---|----------|-----------|---------|----------------|
| M1 | Medium | Assumptions and Blind Spots | Promotion remains conceptually correct but operationally vague. The new hierarchy now requires concrete evidence gates not only for capability promotion, but also for when a bottom-up feature earns a product area or broader system placement. | Add a promotion evidence matrix in FinalizePlan with repeated-pressure categories, example counts, and explicit no-promotion cases. |
| M2 | Medium | Coverage Gaps | Salmon is well-framed as upstream consistency validation, but routing from signal severity to action is still early. The hierarchy expansion adds more possible upstream targets without defining which outcomes are advisory versus blocking. | FinalizePlan should require a signal routing table covering feature, journey, outcome, product area, and landscape targets, with default actions and blocking thresholds. |
| M3 | Medium | Logic Flaws | The new `feature` terminology is clearer for TopDownLens, but it now collides more directly with the existing Lens control-repo concept of a feature. The tech plan starts to separate `nextlens-src-topdownlens` from `feature.<slug>`, but the handoff contract is not explicit enough yet. | FinalizePlan should add a compatibility ADR or story that defines the mapping between control-repo `featureId`, TopDownLens `feature.<slug>` IDs, docs paths, and BMAD traceability fields. |
| M4 | Medium | Cross-Feature Dependencies | The review attempted to load cross-feature context with `fetch-context --depth full`, but this installed runtime still does not expose the subcommand. | Carry this as an implementation environment gap; do not make TopDownLens depend on unavailable context loading until the runtime surface is restored or replaced. |
| M5 | Low | Cross-Feature Dependencies | `feature.yaml.target_repos` is still empty even though FinalizePlan will need an authoritative implementation target before dev-ready handoff. | FinalizePlan should resolve and persist the target repo mapping before generating implementation-readiness artifacts. |

## Accepted Risks

No risks were explicitly accepted by the user during this rerun. The warnings above should be treated as carry-forward planning constraints for FinalizePlan.

## Party-Mode Challenge

John (Product): The hierarchy is much easier to explain now, but a hierarchy is not yet product value. The first sprint must show one visible journey becoming a BMAD-ready feature, or stakeholders will experience this as renaming rather than progress.

Winston (Architect): The rename clarifies the product model, but it also creates a naming collision with the current Lens feature lifecycle. If FinalizePlan does not make the control-feature versus operational-feature mapping explicit, the module will blur governance identity and product topology.

Quinn (QA): The new hierarchy adds more traceability power, which means more ways to drift. Promotion rules, Salmon routing, and doctor checks all need executable examples or the first implementation will look deterministic on paper and ambiguous in practice.

## Gaps You May Not Have Considered

1. Which file or contract is the canonical mapping between the current control-repo `featureId` and future TopDownLens `feature.<slug>` entities?
2. When does a bottom-up feature gain a product area without inventing one too early?
3. What is the smallest user-visible demo that proves TopDownLens is more than a renamed planning taxonomy?
4. How will story files encode both BMAD traceability and TopDownLens feature identity without duplication or drift?
5. Which Salmon outcomes are blocking by default versus advisory annotations only?

## Open Questions Surfaced

- FinalizePlan should decide whether `_bmad-output/lens/` or `docs/` is the first authoritative module output root.
- FinalizePlan should define the mapping between control-repo feature lifecycle IDs and TopDownLens `feature.<slug>` identities.
- FinalizePlan should define the minimum relationship lifecycle states required for MVP implementation.
- FinalizePlan should decide whether bottom-up `lens feature new` ships in the first dev increment or remains a documented compatibility constraint.
- FinalizePlan should address the missing `fetch-context` runtime subcommand if cross-feature context is required by the first implementation.
- FinalizePlan should define what evidence is required for capability and product-area promotion suggestions.
- FinalizePlan should resolve `feature.yaml.target_repos` before dev-ready handoff.
