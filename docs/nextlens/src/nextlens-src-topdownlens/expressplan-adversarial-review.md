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

**Reviewed:** 2026-05-14T03:15:00Z  
**Source:** manual-rerun-post-dogfooding-integration  
**Overall Rating:** pass-with-warnings

## Summary

This rerun reviews the ExpressPlan packet after the dogfooding and self-hosting integration. The business plan, tech plan, and sprint plan now align on a clear top-down hierarchy of `System -> Product Area -> Outcome -> Journey -> Capability -> Feature`, and they add a dedicated self-hosting layer covering a `nextlens-control` / `nextlens-governance` / `nextlens-release` repo split, additive constitution layering for TopDownLens, a `lens-core-bugfix`-style correction loop, and three GitHub Actions pipelines (`promote-to-release`, `publish-to-governance`, `regression-and-doctor`). The integration is internally consistent and explicitly defers physical repo standup to the first dev increment. No critical blocker was introduced. The main carry-forward risks are scope control, output-root ambiguity, promotion and signal-routing thresholds, the control-feature versus operational-feature identity mapping, and new dogfooding-specific risks: bootstrapping deadlock, pipeline boundary drift between governance and release, and constitution drift during the migration window. The `nextlens` domain and `src` service constitutions remain informational and do not add blocking constraints beyond review enforcement and the presence of a business plan.

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
| M6 | Medium | Self-Hosting | Bootstrapping deadlock: TopDownLens must be designed using TopDownLens, but the first dev increment must deliver a usable command set before the dogfooding loop can close. If the first increment slips, the next feature cannot be planned by TopDownLens itself. | FinalizePlan must size the first dev increment to deliver capture, synthesize, prepare-bmad, and doctor checks at a minimum, and treat TL-12 as the explicit closure gate for self-hosting. |
| M7 | Medium | Self-Hosting | Pipeline boundary drift: the design separates `publish-to-governance` (metadata, feature-index, mirrored artifacts) from `promote-to-release` (module payload). It is easy for future contributors to add governance behavior into the release pipeline or vice versa. | Add explicit guardrails to each workflow that fail the run if it tries to write outside its boundary, and assert this in regression tests added by TL-11. |
| M8 | Medium | Self-Hosting | Constitution drift: during the migration window, `Lens.Core.Governance` hosts `nextlens` constitutions while `nextlens-governance` is being stood up. Without a one-time reconciliation, the additive resolution order can return inconsistent results. | TL-9 must include a one-time copy and a doctor check that detects content drift between the two governance locations until incubation ends. |
| M9 | Low | Self-Hosting | The `lens-core-bugfix` pattern is reused conceptually, but the sprint plan does not yet specify whether TopDownLens forks the skill or wires it via configuration. | TL-10 should default to configuration reuse and explicitly forbid forking the `lens-core-bugfix` skill payload. |

## Accepted Risks

No risks were explicitly accepted by the user during this rerun. The warnings above should be treated as carry-forward planning constraints for FinalizePlan.

## Party-Mode Challenge

John (Product): The hierarchy is much easier to explain now, but a hierarchy is not yet product value. The first sprint must show one visible journey becoming a BMAD-ready feature, or stakeholders will experience this as renaming rather than progress.

Winston (Architect): The rename clarifies the product model, but it also creates a naming collision with the current Lens feature lifecycle. If FinalizePlan does not make the control-feature versus operational-feature mapping explicit, the module will blur governance identity and product topology.

Quinn (QA): The new hierarchy adds more traceability power, which means more ways to drift. Promotion rules, Salmon routing, and doctor checks all need executable examples or the first implementation will look deterministic on paper and ambiguous in practice. The added self-hosting pipelines also need fail-closed assertions, or the dogfooding loop will look green while quietly bypassing publication boundaries.

Sally (Release Engineer): The split between `promote-to-release` and `publish-to-governance` is correct, but two workflows in two repos with overlapping triggers is exactly where boundary creep happens. The first regression run must include a negative test that proves neither workflow can write into the other repo's surface.

## Gaps You May Not Have Considered

1. Which file or contract is the canonical mapping between the current control-repo `featureId` and future TopDownLens `feature.<slug>` entities?
2. When does a bottom-up feature gain a product area without inventing one too early?
3. What is the smallest user-visible demo that proves TopDownLens is more than a renamed planning taxonomy?
4. How will story files encode both BMAD traceability and TopDownLens feature identity without duplication or drift?
5. Which Salmon outcomes are blocking by default versus advisory annotations only?

## Open Questions Surfaced

- FinalizePlan should decide whether `_bmad-output/lens/` or `docs/` is the first authoritative module output root. - docs/
- FinalizePlan should define the mapping between control-repo feature lifecycle IDs and TopDownLens `feature.<slug>` identities. yes for now
- FinalizePlan should define the minimum relationship lifecycle states required for MVP implementation. you decide
- FinalizePlan should decide whether bottom-up `lens feature new` ships in the first dev increment or remains a documented compatibility constraint. yes ship it
- FinalizePlan should address the missing `fetch-context` runtime subcommand if cross-feature context is required by the first implementation.you decide
- FinalizePlan should define what evidence is required for capability and product-area promotion suggestions. you decide
- FinalizePlan should resolve `feature.yaml.target_repos` before dev-ready handoff. 
- FinalizePlan should confirm that the first dev increment is sized to close the dogfooding loop (TL-12) without requiring a second sprint.
- FinalizePlan should document the negative pipeline tests that prove `promote-to-release` cannot write to `nextlens-governance` and vice versa.
- FinalizePlan should specify the one-time reconciliation step for moving `nextlens` constitutions from `Lens.Core.Governance` to `nextlens-governance`.
