---
feature: lens-dev-new-codebase-reopen-feature
doc_type: adversarial-review
phase: finalizeplan
review_format: abc-choice-v1
status: responses-recorded
updated_at: '2026-05-08T00:00:00Z'
---

# Adversarial Review: lens-dev-new-codebase-reopen-feature / finalizeplan

**Reviewed:** 2026-05-08T00:00:00Z
**Source:** phase-complete
**Overall Rating:** pass-with-warnings

## Summary

The reopen-feature planning bundle is coherent and implementable. The feature goal (adding a governed `reopen` subcommand to `feature-yaml-ops.py`) is well-scoped and low-complexity. Three issues must be addressed before dev handoff: `target_repos` must be populated in `feature.yaml`, the open question about a `/lens-reopen` conductor must be formally deferred with rationale, and governance artifact state after reopen must be acknowledged as out-of-scope with a tracked follow-up item. Additional medium-risk gaps are documented below with accepted or deferred dispositions.

## Findings

### Critical

_None._

### High

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| H1 | Coverage Gaps | `target_repos: []` — the tech-plan names `TargetProjects/lens-dev/new-codebase/lens.core.src/...` as the implementation surface, but this path is not registered in `feature.yaml`. The `/dev` conductor cannot resolve the target repository without it. | Populate `target_repos` in `feature.yaml` before dev handoff as part of post-bundle metadata reconciliation. |

### Medium / Low

| # | Severity | Dimension | Finding | Recommendation |
|---|----------|-----------|---------|----------------|
| M1 | Medium | Coverage Gaps | The `open_questions` field in `business-plan.md` contains an unresolved question: "Should a dedicated /lens-reopen command be added later?" This was flagged as residual risk in `expressplan-adversarial-review.md` and remains dangling. | Close as explicit deferral with rationale before bundle commit. Record in this review as deferred. |
| M2 | Medium | Coverage Gaps | Governance mirror state after reopen is unspecified. The prior complete cycle left governance artifacts; after reopen, whether those artifacts remain valid context or are superseded is not addressed. | Acknowledge as out-of-scope for this feature; add follow-up tracking item. |
| M3 | Medium | Logic Flaws | `completed_at` removal is conditional ("if present") without explaining when a terminal feature would lack this field. | Clarify in tech-plan: either make removal unconditional or document the absent-field scenario. |
| M4 | Medium | Logic Flaws | Business-plan does not enumerate the complete set of terminal states (`complete`, `archived`, or both). Tech-plan says `complete/archived` but business-plan leaves this implicit. | Update business-plan scope section to explicitly list terminal states. |
| M5 | Medium | Complexity and Risk | Feature-index sync helper's behavior for `archived → active` transitions is not explicitly validated in the test plan; only the reopen pass/fail paths are covered. | Add a focused test asserting the index status is set to the expected active value after reopen. |
| M6 | Medium | Coverage Gaps | No rollback path defined: if reopen succeeds but the new dev cycle fails, re-closure from a reopened state is a gap in the lifecycle contract. | Acknowledge as out-of-scope for this slice; record as follow-up. |
| L1 | Low | Complexity and Risk | Track validation says "valid for track" but does not handle null/missing track case. | Add null-track guard in `--to-phase` validation logic. |
| L2 | Low | Assumptions and Blind Spots | PR branch topology after reopen (prior plan/dev branches may exist or be deleted) is not defined. | Add a note in tech-plan about branch state expectations on reopen. |

## Accepted Risks

- **M2** (governance mirror state): Accepted as out-of-scope for this feature slice. Prior-cycle governance artifacts are treated as historical context; the new dev cycle will publish fresh artifacts on its own path. Follow-up item: add a reopen guidance note to the governance publication runbook.
- **M6** (rollback path): Accepted as out-of-scope. Forward-only lifecycle philosophy means re-closure is handled by a new `complete` transition, not a revert. No additional work needed.
- **L2** (branch topology): Accepted as low risk. The standard branch cleanup process applies; no change needed in this slice.

## Deferred Items

- **M1** (`/lens-reopen` conductor open question): **Explicitly deferred.** The `feature-yaml-ops.py reopen` subcommand is sufficient for this slice. A dedicated `/lens-reopen` conductor is a future UX improvement and does not block the current implementation. This deferral is recorded here and the `open_questions` entry in `business-plan.md` is updated to reflect this disposition.

## Pre-Review Fixes Applied

_None required before this review ran._

## Party-Mode Challenge

**Alex (Governance Auditor):** The governance mirror state gap (M2) is real. You're reopening a feature that already published artifacts. If those artifacts are "still valid" implicitly, say so. If not, define the versioning contract. Right now you're pretending this is someone else's problem.

**Jordan (Implementer):** Your test exit criterion says "reopen command can be executed on a real archived feature." But the only real archived feature available is this feature itself. You need a fixture — a test-only fake `feature.yaml` in a temp directory — so the exit criterion is repeatable and doesn't depend on circular self-reference.

**Sam (Dev Phase Lead):** `target_repos: []`. The entire plan points at `TargetProjects/lens-dev/new-codebase/lens.core.src/...` but the metadata doesn't register it. Populate this before handoff or the dev conductor fails at activation.

## Gaps You May Not Have Considered

1. Does reopening a feature affect its PR topology? Prior plan/dev branches may still exist or be deleted.
2. What happens to prior-cycle sprint-status.yaml and story files in the staged docs path?
3. Is there a minimum time fence or governance approval before a feature can be reopened?
4. Can a split feature (parent + children) be reopened? Does reopen apply to parent, child, or both?

## Open Questions Surfaced

1. Test fixture: use a temp-dir fake `feature.yaml` for the "real archived feature" test exit criterion to avoid circular dependency.
2. Index sync: add assertion that `feature-index.yaml` status field equals `active` after reopen (not just that sync ran).
