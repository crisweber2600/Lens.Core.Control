---
feature: lens-dev-new-codebase-switch
doc_type: finalizeplan-review
phase: finalizeplan
source: manual-rerun
verdict: pass-with-warnings
status: reviewed
critical_count: 0
high_count: 2
medium_count: 4
low_count: 2
carry_forward_blockers: []
updated_at: 2026-04-27T00:00:00Z
---

# FinalizePlan Review — Switch Command

## Review Summary

**Verdict:** `pass-with-warnings`

The combined planning set is coherent and implementation-ready. The three ExpressPlan artifacts — business plan, tech plan, and sprint plan — are mutually consistent and sufficient to begin `/dev`. All 12 sprint stories trace to tech-plan work items and business-plan requirements. No critical blockers remain.

The warnings below inherit and extend the expressplan review's carry-forward items. They are implementation concerns, not planning blockers. Two new governance-scope findings are added from the cross-check.

---

## Cross-Artifact Coherence Assessment

| Pair | Coherence | Notes |
|---|---|---|
| business-plan ↔ tech-plan | ✓ strong | All 10 requirements (SW-B1–SW-B10) map to explicit tech-plan sections or JSON contract fields |
| tech-plan ↔ sprint-plan | ✓ strong | The 6 tech-plan work items are fully covered across Sprint 1–3 stories |
| business-plan ↔ sprint-plan | ✓ strong | Sprint acceptance criteria use the same behavioral language as requirement acceptance criteria |
| expressplan-review ↔ sprint-plan | ✓ addressed | SW-4 (deprecated command name cleanup) directly responds to H1; M1 is addressed in sprint risks; M2 is a sprint carry-forward |

---

## Findings

| ID | Severity | Source | Finding | Required Follow-Up |
|---|---|---|---|---|
| H1 (carry-forward) | High | Expressplan review | Switch user-facing fallback guidance may still reference deprecated `init-feature` prompt name. | SW-4 must test all user-facing messages against the retained command surface. Add a string-scan regression. |
| H2 (new) | High | Governance cross-check | The `express` track is not listed in `permitted_tracks` in the `new-codebase` service constitution. The feature reached `expressplan-complete` through the express track, leaving the constitution and lifecycle history inconsistent. | Add `express` to `permitted_tracks` in the service constitution, or document explicitly why the express track entry was permitted outside the constitution during initial onboarding. This is a governance hygiene issue; it does not block switch implementation. |
| M1 (carry-forward) | Medium | Expressplan review | Phrase "switching context never modifies state" is misleading given local context write and branch checkout side effects. | Sprint risk table documents this. Business plan section 4 should be tightened to distinguish governance read-only from local side effects. |
| M2 (carry-forward) | Medium | Expressplan review | feature-index.yaml `status` and feature.yaml `phase` can diverge; switch surfaces both but lifecycle tools are not guaranteed to keep them in sync. | Lifecycle phase completion commands must update both sources. Known tooling gap: no `update-feature-index` command in the publish CLI. Accepted as a follow-up lifecycle tooling story. |
| M3 (carry-forward) | Medium | Expressplan review | Domain fallback (mode: domains) must hard-stop without inferring a feature. | Sprint 1 story SW-3 covers this. Regression test must verify domains mode produces no feature selection. |
| M4 (new) | Medium | Governance cross-check | The `-dev` branch creation step is described in bugfixes.md as a FinalizePlan responsibility, but the FinalizePlan three-step execution contract in SKILL.md does not explicitly call `create-dev-branch`. | After the planning PR merges into `{featureId}`, call `git-orchestration-ops.py create-dev-branch` before beginning the downstream bundle. Document this in the FinalizePlan skill as an explicit sub-step. |
| L1 (carry-forward) | Low | Expressplan review | Old stub only covers prompt-start behavior, not full switch semantics. | Sprint 3 story SW-12 wires the focused regression command. Clean-room parity requirement is well documented in business plan and tech plan. |
| L2 (new) | Low | Tech-plan validation section | `test-switch-ops.py` is referenced in the validation plan but may not yet exist in the codebase. | Verify the test file path before dev begins. If absent, SW-9/SW-10 tests will need it created as part of dev story acceptance criteria. |

---

## Governance Impact Cross-Check

### Impacted Services / Related Features

| Feature / Service | Relationship | Impact |
|---|---|---|
| `lens-dev-new-codebase-baseline` | Parent (`split_from`) | Stable; switch inherits clean-room rewrite contract and 17-command surface from baseline. No action required. |
| `lens-dev-new-codebase-new-feature` | Sibling command | The deprecated `init-feature` reference in switch fallback text should be replaced with the retained command alias used by `new-feature`. Coordinate on final retained command name before closing SW-4. |
| `lens-dev-new-codebase-next` | Sibling command | Both switch and next consume `feature-index.yaml` for feature listing. A governance tooling gap in feature-index sync affects both. No blocking dependency. |
| feature-index.yaml registry | Cross-cutting | Switch feature's own entry is still `status: preplan` (confirmed in bugfixes.md). This is the M2 drift issue in practice. Lifecycle tooling gap must be addressed; see M2 carry-forward. |

### Constitution Impact

The `new-codebase` service constitution lists `permitted_tracks: [quickplan, full, hotfix, tech-change]`. The `express` track is not listed. See H2 above. Recommend adding `express` to the constitution's permitted tracks to close the governance history gap.

---

## Party-Mode Blind-Spot Challenge

**John (Product):** The business plan's scope is clean, but the 12-story sprint is dense for a single retained command. The risk of scope creep exists if the "deprecated command name" cleanup expands beyond switch's own output. Sprint 3 story SW-10 touches command discovery surfaces — ensure it is bounded to switch-visible strings only.

**Winston (Architecture):** The three-layer architecture is appropriate. The concern is the feature-index.yaml and feature.yaml synchronization gap flagged in M2. The switch script exposes drift but cannot fix it. The fix belongs in lifecycle completion commands, not in switch. Accept M2 as a cross-feature tooling debt item and do not block switch dev for it.

**Sally (UX):** The numbered menu design is good, but the user-visible output for `branch_switched: false` needs to be unambiguous. If a plan branch is missing, the user should see a specific action: "run /new-feature to initialize branches." This is SW-8's acceptance criteria and should be verified against the retained command surface.

**Bob (Scrum Master):** Sprint 3 (story SW-12) depends on Sprint 2 test infrastructure being in place. Story SW-12 must not be started until `test-switch-ops.py` exists and passes. Mark SW-12 as blocked-by SW-9.

---

## Blind-Spot Questions

1. Has SW-4's deprecated command name replacement been confirmed against every user-facing string in the prompt and skill files, or only the fallback help message?
2. Is the 30-day stale threshold for feature context still the right value for the release cadence being planned?
3. Does `create-dev-branch` in the git orchestration CLI create the branch from `{featureId}` (after planning PR merge), or from `{featureId}-plan`?
4. Will the `-dev` branch be pushed to remote by `create-dev-branch` or does a separate `push` call follow?
5. Is the governance constitution update (adding `express` to permitted tracks) expected before or after the switch dev story begins?

---

## Gate Decision

**Advance to dev with warnings.** All planning artifacts are coherent and sufficient for implementation. Carry-forward items H1, M1–M4 are scoped to dev story acceptance criteria. H2 (constitution track gap) and M4 (dev branch creation in FinalizePlan) are governance housekeeping items that can be resolved in parallel with dev.

**Next action:** `/dev` — after planning PR merges into `lens-dev-new-codebase-switch` and dev branch is created.
