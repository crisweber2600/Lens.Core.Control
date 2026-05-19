---
feature: nextlens-src-bottomup
doc_type: preplan-adversarial-review
status: complete
phase: preplan
source: phase-complete
verdict: pass-with-warnings
reviewed_artifacts:
  - brainstorm.md
  - research.md
  - product-brief.md
updated_at: 2026-05-19T00:00:00Z
---

# Adversarial Review: nextlens-src-bottomup / preplan

**Reviewed:** 2026-05-19T00:00:00Z  
**Source:** phase-complete  
**Overall Rating:** pass-with-warnings

## Summary

The PrePlan package is strong enough to proceed to BusinessPlan. It defines a clear first slice for Bottom-Up LENS: a dedicated feature packet creator that captures one locally valuable feature, validates scope and non-inference constraints, preserves provenance, emits a non-effects receipt, and defers BMAD readiness, adjacency, Salmon, promotion, Landscape, and Graph behavior.

The package does not contain unresolved critical blockers. However, it carries material risks: the MVP boundary must remain narrow, the Two-Tree topology implications must not accidentally force a broader migration, BMAD readiness must stay separate from packet validity, and product language must avoid implying that archived packet evidence is Living Landscape truth.

## Findings

### Critical

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| — | — | No critical findings. | Proceed with warnings. |

### High

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| H1 | Coverage Gaps | The PrePlan correctly defers Salmon, adjacency, pressure, promotion, Landscape, and Graph behavior, but BusinessPlan may be tempted to include these because they are prominent in the conceptual model. | In PRD scope, explicitly mark these as non-goals for MVP and define acceptance criteria that prove they are not emitted. |
| H2 | Complexity and Risk | The integration of the Two-Tree Model introduces large topology concepts that could expand the feature beyond packet creation. | Treat Two-Tree compatibility as metadata/provenance readiness only; do not include archive migration, landscape ledgers, projection rebuild, or Auspex UI changes in MVP. |
| H3 | Logic Flaws | The product brief says packet validity and BMAD readiness are separate, but downstream BusinessPlan may collapse them into one flow for convenience. | Define two distinct states and validators: `packet_valid` and `bmad_ready`; make it legal to have a valid packet that is not BMAD-ready. |

### Medium

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| M1 | Assumptions and Blind Spots | The user/persona model is still broad. Individual builders, teams, operators, and Auspex stakeholders have different needs. | PRD should identify primary MVP user as the Lens/BMAD operator creating a packet; other personas should remain secondary. |
| M2 | Coverage Gaps | The packet schema is sketched but not yet expressed as a complete validation contract. | BusinessPlan should produce enough requirements for a JSON-schema-backed packet validator in TechPlan. |
| M3 | Cross-Feature Dependencies | Related features `nextlens-src-topdownlens`, `nextlens-src-implement`, and `nextlens-src-dogfoodnext` are mostly stub/archived context. There is limited confirmed predecessor detail. | BusinessPlan should not assume implementation behavior from those related features without reading their concrete artifacts if they become relevant. |
| M4 | Complexity and Risk | The non-effects receipt is central but may be hard to prove if implemented as narrative only. | Require machine-verifiable receipt fields and tests showing no forbidden output files or metadata updates are created. |
| M5 | Assumptions and Blind Spots | Planning-branch elimination from the prior topology session conflicts with current Lens lifecycle operations. | Treat planning-branch changes as future topology work; do not alter current control-repo lifecycle branch rules in this feature. |

### Low

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| L1 | Coverage Gaps | Auspex reporting fields are useful but premature. | Keep reporting fields minimal and read-only; do not add Auspex implementation scope. |
| L2 | Logic Flaws | The term “bottom-up track” could imply a complete lifecycle track rather than first packet support. | Use wording “future lane/track support; MVP packet creator” consistently. |

## Accepted Risks

No user-accepted risks were separately recorded during this review. The risks above are accepted only as documented warnings for BusinessPlan to resolve or constrain.

## Party-Mode Challenge

Mary (Business Analyst): The packet creator is compelling, but the brief still needs crisp persona focus. If the MVP user is everyone, BusinessPlan will drift into a platform story. Choose the Lens operator as primary and make downstream consumers secondary.

Winston (Architect): The Two-Tree Model is a strong backdrop, but it is dangerous gravitational mass. If TechPlan is asked to design archive, landscape, projection rebuild, doctor, Salmon, and Auspex integration now, this feature will fail. BusinessPlan must pin the implementation to packet file plus validators plus receipt.

Quinn (QA): The non-effects claim is testable only if the product requires observable proof. “Did not emit graph updates” must map to concrete file/path/state assertions. Otherwise the most important safety promise becomes an untested slogan.

## Gaps You May Not Have Considered

1. What exact filesystem/state checks prove no forbidden downstream outputs were created?
2. What is the smallest persona set that keeps PRD focused on packet creation?
3. How will the UX explain `packet_valid` vs. `bmad_ready` without confusing users?
4. What terms are banned because they imply system/domain/capability promotion too early?
5. Which packet fields are for future map rebuilds but must not be interpreted as current topology?

## Open Questions Surfaced

- Should the packet status enum include both `confirmed` and `bmad-ready`, or should readiness be stored as a separate validator result?
- Should deferred candidates live inside the packet, beside the packet, or only in session notes?
- What is the exact receipt schema for non-effects?
- What is the minimum acceptance test set for “writes one packet and nothing else”?
- How should BusinessPlan phrase Two-Tree compatibility without expanding MVP scope?
