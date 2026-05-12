---
feature: lens-dev-new-codebase-flatten
doc_type: adversarial-review
phase: businessplan
source: manual-rerun
verdict: pass-with-warnings
review_format: abc-choice-v1
status: reviewed
critical_count: 0
high_count: 1
medium_count: 2
low_count: 0
carry_forward_blockers: []
updated_at: 2026-05-12T00:00:00Z
reviewed_at: 2026-05-12T00:00:00Z
---

# Adversarial Review: lens-dev-new-codebase-flatten / businessplan

## Selected response

**B — Pass with warnings**

## Response options

- **A — Pass:** Artifacts are complete and coherent; proceed with no material concerns.
- **B — Pass with warnings:** Artifacts support proceeding, but specific risks or gaps must be carried forward and addressed explicitly.
- **C — Needs revision:** Artifacts are directionally useful but require substantive revision before the next phase.
- **D — Major rework required:** Artifacts have significant structural or evidence gaps that block reliable planning.
- **E — Reject:** Artifacts are not sufficient to support phase progression.

---

**Reviewed:** 2026-05-12T00:00:00Z  
**Source:** manual-rerun  
**Overall Rating:** pass-with-warnings

## Summary

The BusinessPlan packet is strategically coherent and it materially closes the major PrePlan warnings. The business plan freezes the flat-mode governance invariants, rejects mixed-mode v1 behavior, blocks in-flight workflow-mode migration, and carries a concrete command/surface matrix across the affected control-repo lifecycle surfaces. The staged PRD and UX work also stay aligned with the feature's central promise: flat mode removes control-repo branch and PR enforcement, not lifecycle rigor.

The structural blocker from the prior review run is resolved. The staged `prd.md` and `ux-design-specification.md` now carry the BusinessPlan lifecycle frontmatter contract, and the shared artifact validator passes with both required artifacts found and no metadata errors. That is enough to remove the phase-blocking failure from the earlier run.

The packet still carries meaningful follow-up warnings into TechPlan. It does not yet encode the service constitution's BMB-first implementation channel for `lens.core.src` changes, `feature.yaml.target_repos` remains empty even though later lifecycle behavior is in scope, and the UX artifact is still staged under the accepted alias `ux-design-specification.md` without clarifying whether that alias is intended to remain canonical. These issues do not block the artifact set from moving forward, but they should be preserved as explicit implementation constraints.

## Findings

### Critical

*(None)*

### High

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| H1 | Coverage Gaps | The active `lens-dev/new-codebase` service constitution requires a BMB-first implementation channel for `lens.core.src` changes, but the BusinessPlan packet does not carry that requirement into the PRD or business-plan handoff. This feature necessarily changes `lens.core.src` lifecycle and orchestration surfaces, so omitting the execution channel now creates a governance-compliance gap later. | Add an explicit implementation-channel constraint before TechPlan begins: `lens.core.src` code changes must route through the service's BMB workflow, not ad hoc source edits. |

### Medium / Low

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| M1 | Cross-Feature Dependencies | `feature.yaml.target_repos` is still empty even though the packet now clearly scopes implementation across control-repo lifecycle surfaces. That is not a BusinessPlan blocker by itself, but without an explicit target-repo plan the later handoff can still arrive at FinalizePlan or Dev with no authoritative implementation destination. | Record the intended implementation repo mapping before dev-readiness, and preferably name it in downstream planning artifacts so the feature does not stay repo-ambiguous. |
| M2 | Assumptions and Blind Spots | The UX artifact is only staged as `ux-design-specification.md`, which the shared validator accepts as an alias. That is workable, but the packet never states whether this alias is the permanent canonical artifact name or a temporary workflow-output filename. Leaving that ambiguous invites future tooling drift between `ux-design.md` expectations and staged reality. | Decide whether `ux-design-specification.md` is the canonical BusinessPlan UX artifact for this feature or whether a normalized `ux-design.md` handoff artifact should be created before completion. |

## Accepted Risks

No explicit risk acceptances were recorded in this rerun. The remaining findings should be carried into TechPlan or resolved before dev-readiness, but they do not justify a blocking BusinessPlan verdict on their own.

## Party-Mode Challenge

John (Product): The packet now reads like a real BusinessPlan handoff instead of a generator transcript, which is the right correction. The remaining question is whether TechPlan will carry the implementation-channel rule forward explicitly, or quietly assume developers already know it.

Priya (Governance Auditor): The service constitution is quietly telling you how this work is allowed to be implemented. If the plan never says that `lens.core.src` changes must follow the BMB channel, you can pass architecture review on content and still fail compliance on execution path.

Amelia (Implementation Lead): I can build against this packet now, but I still need two things before execution gets real: which repo is authoritative for the implementation work, and whether `ux-design-specification.md` is the permanent artifact name or just an accepted alias.

## Gaps You May Not Have Considered

1. The frontmatter fix removes the structural blocker, but it does not itself tell downstream implementers which execution channel is governance-compliant.
2. The BMB-first rule is informational at the constitution layer, but it still becomes a real review finding once the feature scopes `lens.core.src` changes.
3. The feature already names later lifecycle surfaces as part of the product boundary, so leaving `target_repos` empty gets riskier as soon as TechPlan begins.
4. Accepting `ux-design-specification.md` as an alias without naming it as canonical can create subtle downstream tooling mismatches.

## Open Questions Surfaced

1. Where will the BMB-first implementation-channel rule be encoded so TechPlan cannot silently bypass it?
2. Which implementation repo or repos should be recorded for this feature before the finalizeplan/dev handoff?
3. Is `ux-design-specification.md` the intended canonical UX handoff artifact, or should this phase still produce `ux-design.md`?
4. Should the next phase update `feature.yaml.target_repos` as part of its readiness contract, or treat that as a finalizeplan requirement?