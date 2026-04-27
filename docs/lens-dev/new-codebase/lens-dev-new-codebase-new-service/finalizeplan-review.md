---
feature: lens-dev-new-codebase-new-service
doc_type: finalizeplan-review
phase: finalizeplan
source: manual-rerun
verdict: pass-with-warnings
updated_at: 2026-04-27T00:00:00Z
reviewed_at: 2026-04-27T00:00:00Z
track: express
reviewer: Lens FinalizePlan Conductor
---

# FinalizePlan Review: lens-dev-new-codebase-new-service

**Reviewed:** 2026-04-27T00:00:00Z  
**Source:** manual-rerun  
**Overall Rating:** pass-with-warnings

## Summary

The express planning set for `new-service` — comprising `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, and `expressplan-review.md` — is coherent, internally consistent, and scoped precisely to restoring the retained `/new-service` command as a clean-room implementation of the service-container boundary. The prior expressplan review returned `pass-with-warnings` (track conversion, command-surface metadata, and clean-room parity obligations). This FinalizePlan review extends that verdict with three additional high-severity findings: (1) the BMB-first constitution rule is not reflected in the sprint stories, (2) the implicit dependency on the completed `new-domain` implementation is undocumented and could cause shared-helper confusion, and (3) the governance git execution stability path from `new-domain` is assumed stable but not confirmed. No critical blockers remain. The planning set may proceed to the downstream bundle (epics, stories, readiness, sprint) with the warnings below explicitly accepted.

## Findings

### Critical

No critical findings.

### High

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| H1 | Coverage Gaps | `new-domain` is `phase: complete`. The plan depends on "nearby helper patterns" without naming which functions, helpers, or modules will be shared vs. re-implemented. If both `create-domain` and `create-service` independently create domain markers, there are two mutation paths for the same governance entity. | Before implementation (NS-4): explicitly map which `new-domain` script helpers `create-service` will reuse, and document the boundary. The auto-establish-missing-parent-domain path (ADR-3) should call existing `create-domain` helpers rather than duplicate domain-marker logic. |
| H2 | Assumptions & Blind Spots | The service constitution's BMB-first rule states: "changes to `lens.core.src` must be implemented through the `lens.core/_bmad/bmb` module." Stories NS-5 through NS-10 all write to `lens.core.src` (init-feature-ops.py, SKILL.md, release prompt, module-help) but no story references BMB as the implementation channel. | Add a BMB routing note to the implementation definition of done. The `gate_mode: informational` means this is advisory for this feature, but it must be recorded as accepted deviation. |
| H3 | Complexity & Risk | The plan assumes the governance git execution path tested in `new-domain` is stable and reusable. `new-domain` completed development (phase: complete) but no governance-git regression test suite is documented that proves the exact path `create-service` intends to reuse is covered. | Before NS-7, confirm that the governance git path exercised by `create-domain` has passing test coverage. If not, add a regression test to the NS-7 scope. |

### Medium

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| M1 | Logic Flaws | `sprint-plan.md` doesn't mention BMB routing anywhere. The constitution's BMB-first rule is informational, but implementation stories should record the decision explicitly so reviewers don't flag it during code review. | Add a single-line acceptance note to the implementation handoff (NS-13): "Scope excludes BMB routing per informational gate_mode; direct edits to `lens.core.src` are acknowledged." |
| M2 | Cross-Feature Dependencies | `lens-dev-new-codebase-new-feature` is at `status: preplan`. It is not formally listed as `blocked_by: new-service`, but the user setup path (domain → service → feature) implies it is. A delay in `new-service` delivery silently blocks `new-feature` planning. | Record `new-service` as an implicit prerequisite for `new-feature` in the feature-index or via a `blocks: [lens-dev-new-codebase-new-feature]` update in feature.yaml. |
| M3 | Coverage Gaps | The error-recovery flow for `--execute-governance-git` is unspecified. What happens when git automation fails mid-operation? The plan mentions "preserve separate git command groups" but doesn't specify whether partial writes are safe to retry. | Add explicit retry-safety language to NS-7: the governance git path should be idempotent so a failed run can be retried without duplicate marker creation. |
| M4 | Assumptions & Blind Spots | `dry-run` and `--execute-governance-git` should be mutually exclusive, but this combination is not tested explicitly. NS-11 targets `create_service` broadly — the mutual-exclusion contract may be assumed rather than asserted. | Confirm NS-1's test matrix explicitly covers the `--dry-run + --execute-governance-git` rejection case. |

### Low

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| L1 | Complexity & Risk | Sprint 4 has 5 points across 3 verification stories. NS-13 ("prepare implementation handoff notes", 1 point) is the only documentation delivery. If schedule pressure hits, this story is at highest risk. | Treat NS-13 as a requirement, not a polish step. Handoff notes are the primary artifact connecting `/dev` to the implementation agent. |
| L2 | Coverage Gaps | No explicit test for the sequential flow: `new-domain` runs first (domain marker exists), then `new-service` adds the service layer. NS-2 tests the boundary in isolation; it should also cover the idempotent case. | Add an integration test variant in NS-2 that starts from a pre-existing domain container. |
| L3 | Logic Flaws | Governance discrepancy: The governance mirror holds `expressplan-adversarial-review.md` (older publish name) while the control repo staging path holds `expressplan-review.md` (correct lifecycle convention name). The publish-to-governance CLI does not map `expressplan-review.md` to the review-report artifact lookup. | Rename the governance copy to `expressplan-review.md` via a governance repo patch or republish after updating the CLI's artifact-candidate list. Track as a separate infra fix; do not block FinalizePlan. |

## Governance Impact Findings

| # | Area | Finding | Action Item |
|---|------|---------|-------------|
| GI-1 | Track Constitution | The `new-codebase` service constitution permits `[quickplan, full, hotfix, tech-change]` but not `express`. The feature completed `expressplan` before this was caught. Constitution `gate_mode: informational` — not a hard block. | Feature owner must explicitly accept the track deviation. Record acceptance in this review. No replan required. |
| GI-2 | Cross-Service Naming | `expressplan-adversarial-review.md` in governance mirror vs. `expressplan-review.md` in control repo is a naming drift that will cause future publish failures. | Create a tracked infra bug: update the CLI artifact-candidate list or rename the governance file. |
| GI-3 | Feature Sequencing | `lens-dev-new-codebase-new-feature` has no formal `depends_on: new-service` recorded. | Update `new-feature` feature.yaml to add the implicit dependency before planning starts. |

## Accepted Risks

| Finding | Acceptance Rationale |
|---------|----------------------|
| GI-1 (Express track not in constitution) | Feature is already at `expressplan-complete`. Reversing the track would require a full replanning cycle, which is disproportionate for a retained command with a clear, well-scoped plan. The `gate_mode: informational` makes this advisory. Accepted with owner acknowledgement. |
| H2 (BMB-first rule not in sprint) | The `gate_mode: informational` makes the BMB-first rule advisory for this cycle. Implementation should note the deviation in NS-13 handoff notes. The rule will be enforced strictly on future cycles. |

## Party-Mode Challenge

**John (PM):** The definition of done says "`new-service` appears in the retained prompt surface and module help" — but NS-10 is listed last in Sprint 3 and is 2 points. It will be the story that gets dropped if the sprint runs over. I'd feel better if NS-10 had an explicit acceptance gate: "retained command not shipped until it is discoverable from all listed entry points."

**Winston (Architect):** ADR-3 says `create-service` should auto-establish the missing parent domain container. The `new-domain` feature is complete. Why does `create-service` re-implement domain container logic instead of calling `create-domain` as a library function? Two mutation paths for the same governance entity is a design smell. If we're already sharing helpers for YAML writes and git automation, domain-marker creation should be delegated to the same code path.

**Murat (TEA):** NS-1 writes contract tests before implementation — that's the right order. But there's a missing scenario in NS-2's boundary test: what happens when someone runs `/new-service` and a domain marker already exists from a real `/new-domain` run? The governance file is already there. Does `create-service` correctly detect it, skip creation, and continue? That's the most common real-world setup path and it needs a green test.

## Gaps You May Not Have Considered

1. **Delegation boundary in ADR-3:** If `create-service` re-implements domain container creation internally rather than delegating to `create-domain` logic, two mutation paths exist for the same governance artifact. Does the implementation explicitly prevent double-write when the domain marker already exists?
2. **Discovery surface completeness:** Are there any discovery surfaces beyond `lens-new-service.prompt.md` and `module-help.csv` that need updating? (e.g., `agents/lens.agent.md` shell menu, `help-topics.yaml`) The baseline notes that command discovery spans three surfaces in sync.
3. **Lifecycle gate re-entry:** If the implementation is delivered and a regression is found in governance git automation, is there a recovery path that doesn't require re-running the full init-feature test suite? Is the governance git path tested in isolation?
4. **Constitution permission for `express` track:** The service constitution does not list `express` in `permitted_tracks`. Should the constitution be updated to formally permit express for retained-command parity work, or is this a one-time deviation?
5. **NS-13 handoff notes scope:** The handoff notes are 1 story point. Does that include identifying the exact files to modify, the test runner command, and the clean-room constraint restatement? If the `/dev` agent starts from NS-13 only, is it enough context to implement without referencing the full planning set?

## Open Questions Surfaced

1. Should `create-service`'s ADR-3 implementation delegate to `create-domain` helpers, or is re-implementation acceptable given the clean-room constraint?
2. Is the `new-domain` governance git execution path covered by passing tests that `create-service` can rely on?
3. Should the service constitution be amended to permit the `express` track, or should this remain a documented one-time deviation?
4. Should `lens-dev-new-codebase-new-feature` be formally marked as `depends_on: [lens-dev-new-codebase-new-service]` now, or deferred to when new-feature planning begins?
