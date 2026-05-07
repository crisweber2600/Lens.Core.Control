---
feature: lens-dev-new-codebase-flatten
doc_type: adversarial-review
phase: preplan
source: manual-rerun
verdict: pass-with-warnings
status: reviewed
critical_count: 0
high_count: 3
medium_count: 3
low_count: 1
carry_forward_blockers: []
updated_at: 2026-05-07T00:00:00Z
---

# PrePlan Adversarial Review - Governance-Controlled Flat Mode

## Review Summary

**Verdict:** `pass-with-warnings`

The PrePlan packet is coherent, grounded in the current codebase, and strong enough to advance into BusinessPlan. The research correctly identifies that the current control-repo branch and PR model is a repo-wide contract rather than a single helper behavior, and the product brief correctly reframes the request as a full control-repo workflow mode instead of a planning-only shortcut.

The remaining warnings are about decision sharpness, not viability. Flat mode still needs explicit treatment for governance rigor, in-flight feature behavior, and later lifecycle ownership so the feature does not promise a complete workflow change while only implementing the first few entry points.

## Findings

| ID | Severity | Dimension | Finding | Required Follow-Up |
|---|---|---|---|---|
| H1 | High | Coverage Gaps | Flat mode removes control-repo branch and PR enforcement, but the review and artifact contract that remains mandatory is still underdefined. | Freeze which governance behaviors stay unchanged in flat mode before BusinessPlan closes. |
| H2 | High | Complexity and Risk | Universal workflow-mode changes for in-flight features are undefined and could strand partially branched work. | Choose and document the v1 rule for mode changes while features already exist. |
| H3 | High | Cross-Feature Dependencies | The packet names the affected command surfaces, but not yet an ownership or acceptance map for each downstream dependency. | Carry a command/surface dependency matrix into BusinessPlan and TechPlan. |
| M1 | Medium | Logic Flaws | A governance-wide universal setting matches the ask, but it also removes mixed-mode flexibility across services unless that trade-off is made explicit. | Decide whether v1 intentionally forbids service-level override, or narrow the scope. |
| M2 | Medium | Coverage Gaps | Success criteria acknowledge later lifecycle work, but they do not yet define user-observable flat-mode outcomes for finalizeplan, dev, and complete. | Add later-lifecycle acceptance criteria before the feature is declared implementation-ready. |
| M3 | Medium | Assumptions and Blind Spots | Context persistence is mostly already solved via `feature_id`, but the possible `feature` alias can still trigger accidental schema churn if left loose. | Freeze `feature_id` as canonical early and treat any alias as optional compatibility sugar. |
| L1 | Low | Assumptions and Blind Spots | Effective workflow mode visibility is implied as useful for support, but not yet committed as a concrete output or diagnostic rule. | Decide the minimum debug surfacing for resolved mode. |

## High Findings

### H1 — Flat-Mode Governance Rigor Is Still Underdefined

**Location:** [product-brief.md](product-brief.md), [research.md](research.md)

The packet consistently says flat mode should remove control-repo branch and PR enforcement, not the lifecycle itself. That is the right direction. What is not yet fixed is exactly which controls remain mandatory in flat mode:

- adversarial review artifacts,
- publish-to-governance behavior,
- phase-completion gates,
- and any review evidence normally carried by a control-repo PR.

If BusinessPlan leaves those rules implicit, flat mode will be interpreted as "lighter governance" instead of "different control-repo git workflow," which would change the feature's meaning.

**Required follow-up:** BusinessPlan should explicitly define the invariant set that survives flat mode unchanged.

### H2 — Mode Changes For In-Flight Features Are Undefined

**Location:** [product-brief.md](product-brief.md), [research.md](research.md)

The feature is framed as a governance-controlled universal mode. That creates a hard operational question: what happens if the governance repo flips from structured to flat, or flat to structured, after features already exist with branch-derived state, branch names in metadata, or open lifecycle work?

Without a rule here, teams can end up in one of two bad states:

- structured-era features become unsupported after a flat switch,
- or flat-era features are suddenly judged by branch-state expectations they never created.

**Required follow-up:** pick a v1 rule before BusinessPlan exits. The safest candidates are: prohibit mode changes while active features exist, preserve existing topology per feature, or introduce an explicit migration path.

### H3 — Dependency Ownership Is Identified But Not Yet Operationalized

**Location:** [research.md](research.md), [product-brief.md](product-brief.md)

The research correctly names the affected surfaces: constitution resolution, feature initialization, feature switching, control git orchestration, preflight, git-state, and dev. That is enough to prove the feature is real and cross-cutting.

What is still missing is the next planning layer: who owns each dependency, what flat-mode behavior is expected from each one, and what evidence proves parity in structured mode while enabling flat mode. Without that matrix, later planning can unintentionally overweight `new-feature` and `switch` while under-scoping `git-state` and `lens-dev`.

**Required follow-up:** carry a command/surface behavior matrix into BusinessPlan and then into TechPlan test planning.

## Medium Findings

### M1 — Universal Scope Is A Real Trade-Off, Not Just A Storage Choice

**Location:** [product-brief.md](product-brief.md), [research.md](research.md)

The packet leans toward a governance-wide universal setting, likely resolved at the org constitution layer. That matches the stated request, but it also means teams cannot keep one service in structured mode and another in flat mode under the same governance installation.

That may still be the right v1 choice. The issue is that it should be stated as an intentional product trade-off, not discovered later as an architectural side effect.

### M2 — Later Lifecycle Success Is Still Defined Indirectly

**Location:** [product-brief.md](product-brief.md)

The brief is honest that flat mode is not complete until later lifecycle behavior is covered. That is a good PrePlan constraint. What it still lacks is user-observable completion criteria for:

- finalizeplan without control plan branches,
- dev without control dev branch activation,
- and cleanup/state reporting without branch-role assumptions.

BusinessPlan should convert those from cautionary notes into acceptance language.

### M3 — Context Schema Compatibility Needs An Early Freeze

**Location:** [product-brief.md](product-brief.md), [research.md](research.md)

The research correctly observes that the current context file already persists `domain`, `service`, and `feature_id`. That means the core request is about branch/PR coupling, not missing context persistence. Leaving the `feature` versus `feature_id` question open too long risks pulling the feature into a compatibility refactor that is unrelated to the main workflow change.

BusinessPlan should freeze `feature_id` as canonical unless a hard compatibility reason for aliasing emerges.

## Low Finding

### L1 — Effective Mode Visibility Is Useful But Still Vague

**Location:** [product-brief.md](product-brief.md), [brainstorm.md](brainstorm.md)

The packet repeatedly notes that surfacing the resolved mode would help support and debugging. That is reasonable, especially once commands can succeed in both structured and flat modes. Right now it is still phrased as a likely good idea instead of a bounded requirement.

This is not a planning blocker, but TechPlan should decide the minimal observable surface: command output, diagnostic payload, or local context metadata.

## Accepted Risks

No explicit risk acceptances were recorded during this review. All findings above should be treated as carry-forward clarification work for BusinessPlan and TechPlan.

## Party-Mode Blind-Spot Challenge

John (Product): The packet is strongest when it says this is not anti-governance, only anti-ceremony. If BusinessPlan does not pin exactly what governance rigor remains in flat mode, users will hear "skip process" instead of "change control-repo workflow."

Winston (Architecture): Constitution resolution is the right policy path, but universal settings are blunt. If v1 really means one mode for the whole governance installation, say that clearly and reject mixed-mode expectations early instead of letting them leak into implementation.

Amelia (Delivery): The hidden risk is not `/new-feature`; it is the first time someone reaches finalizeplan or dev and the tool still wants `-plan` or `-dev`. The acceptance model has to make those later lifecycle behaviors explicit before anyone calls the feature complete.

## Gaps You May Not Have Considered

1. Feature metadata and diagnostics may still refer to `plan_branch`, branch-role state, or PR expectations even when flat mode suppresses the underlying workflow.
2. A governance-wide mode switch may be easy to store but operationally expensive if teams already have active features with structured assumptions.
3. Review evidence currently piggybacks on branch/PR flow in user mental models; flat mode needs an equally explicit non-PR review story.
4. Support and troubleshooting get harder if command output does not reveal which workflow mode was actually resolved.

## Open Questions Surfaced

1. In flat mode, which review and publication behaviors remain mandatory and unchanged?
2. Is workflow mode immutable once a feature starts, or can active features survive a later governance-wide mode switch?
3. Is mixed-mode operation across services intentionally out of scope for v1?
4. What are the concrete flat-mode acceptance criteria for finalizeplan, dev, and complete?
5. Is `feature_id` the permanent canonical context key regardless of workflow mode?

## Gate Decision

Advance with warnings. The PrePlan packet is ready for BusinessPlan, but the warnings above should be converted into explicit product decisions before implementation planning begins.