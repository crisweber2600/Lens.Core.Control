---
feature: nextlens-src-bottomup
doc_type: adversarial-review
status: approved
phase: businessplan
source: phase-complete
verdict: pass-with-warnings
reviewed_artifacts:
  - docs/nextlens/src/nextlens-src-bottomup/prd.md
  - docs/nextlens/src/nextlens-src-bottomup/ux-design.md
depends_on:
  - docs/nextlens/src/nextlens-src-bottomup/preplan-adversarial-review.md
updated_at: 2026-05-19T00:00:00Z
---

# Adversarial Review: nextlens-src-bottomup / businessplan

**Reviewed:** 2026-05-19T00:00:00Z  
**Source:** phase-complete  
**Overall Rating:** pass-with-warnings

## Summary

BusinessPlan artifacts are coherent and sufficient for TechPlan handoff. The PRD and UX design preserve the core Bottom-Up LENS constraint: create one feature packet as archive evidence without creating Landscape, Graph, Salmon, promotion, adjacency, or BMAD execution side effects. The strongest residual risk is that the artifacts specify operator workflow and validation behavior in detail but intentionally leave implementation architecture, packet storage topology, schema mechanics, and receipt verification design for TechPlan. Proceed to TechPlan with these risks explicitly carried forward.

## Findings

### Critical

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| — | — | No critical findings. | Proceed; no phase-blocking defect found. |

### High

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| — | — | No high-severity findings. | Proceed; validate architecture decisions in TechPlan. |

### Medium / Low

| # | Severity | Dimension | Finding | Recommendation |
|---|----------|-----------|---------|----------------|
| M1 | Medium | Complexity and Risk | The non-effects receipt is central to trust, but the BusinessPlan does not define how changed files, forbidden paths, and run metadata are captured. | TechPlan must define receipt verification mechanics, forbidden-path detection, and repeatable validation strategy. |
| M2 | Medium | Coverage Gaps | Packet storage is described as active docs path for this planning feature and future Feature Archive for real bottom-up runs, but the transition boundary is not architecturally resolved. | TechPlan must choose the MVP write path and future archive compatibility model without violating Lens write-scope rules. |
| M3 | Medium | Assumptions and Blind Spots | The UX assumes users will understand the difference between packet validity and BMAD readiness when plain-language labels are used. | TechPlan/FinalizePlan should preserve separate validator states and ensure examples cover `packet_valid=true` / `bmad_ready=false`. |
| L1 | Low | Cross-Feature Dependencies | Related NextLens features may later affect archive/landscape/graph terminology, but no direct dependency is declared. | Carry the relationship as contextual awareness rather than a blocking dependency. |

## Accepted Risks

- The BusinessPlan intentionally does not design packet schema internals, receipt verification implementation, or filesystem-diff mechanics; these belong in TechPlan.
- The BusinessPlan intentionally defers BMAD execution, adjacency detection, pressure detection, promotion, Salmon routing, Landscape updates, and Graph projection to post-MVP or future features.
- The BusinessPlan accepts command/prompt-native UX rather than visual mockups because the MVP surface is operator workflow, not a standalone UI.

## Party-Mode Challenge

John (Product Manager): The PRD is strong on restraint, but the next phase must not dilute the promise. If TechPlan cannot make non-effects verifiable, the product loses its differentiator. Treat receipt verification as core, not auxiliary.

Sally (UX Designer): The emotional design is coherent, especially relief and permission. The risk is copy drift during implementation. Preserve the exact plain-language patterns: “Start from one feature,” “not ready yet,” and “Ready for BMAD: not yet / ready.”

Winston (Architect): The architecture must draw a hard boundary between archive evidence and promoted truth. Do not let convenience turn the packet path into a Landscape or Graph write. Model the derived graph as a projection/cache from the start.

## Gaps You May Not Have Considered

1. What is the exact source of truth for changed-file lists used by receipt verification?
2. How will the command prevent or detect writes attempted by delegated helpers or future integrations?
3. How will schema version migration work when early bottom-up packets already exist?
4. What is the smallest packet archive path that does not imply promoted domain/service ownership?
5. How will examples stay synchronized with the validator as schema fields evolve?

## Open Questions Surfaced

- Should the MVP packet be JSON, YAML, or a paired human-readable markdown plus machine-readable data artifact?
- Should `bmad_ready` be computed during preview, after write, or both?
- Should receipt verification be part of the same command surface or a separate explicit subcommand?
- What minimum run metadata is required to prove a false receipt claim?
- Which forbidden paths are fixed by policy and which should be resolved from lifecycle/config metadata?