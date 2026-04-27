---
feature: lens-dev-new-codebase-new-service
doc_type: finalizeplan-review
phase: finalizeplan
source: manual-rerun
verdict: pass-with-warnings
updated_at: 2026-04-27T15:00:00Z
reviewed_at: 2026-04-27T15:00:00Z
track: express
reviewer: Lens FinalizePlan Conductor
responses_recorded_at: 2026-04-27T15:00:00Z
---

# FinalizePlan Review: lens-dev-new-codebase-new-service

**Reviewed:** 2026-04-27T00:00:00Z  
**Responses Recorded:** 2026-04-27T15:00:00Z  
**Source:** manual-rerun  
**Overall Rating:** pass-with-warnings — all findings addressed, advancing to Step 3

## Summary

The express planning set for `new-service` — comprising `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, and `expressplan-review.md` — is coherent, internally consistent, and scoped precisely to restoring the retained `/new-service` command as a clean-room implementation of the service-container boundary. All high-severity findings have been responded to: (1) H1 accepted — ADR-3 delegation boundary documented in `tech-plan.md`, `create-service` delegates to `create-domain` helpers for parent container creation; (2) H2 modified — BMB-first rule updated to reference `.github/skills/bmad-module-builder` and `.github/skills/bmad-workflow-builder` as the specific skill implementation channels; (3) H3 confirmed stable — governance git execution path in `new-domain` is verified stable. Service constitution and both init-feature-ops.py templates updated to permit `express` track. Governance mirror naming drift corrected. `new-feature` dependency recorded. Planning set advances to Step 3.

## Findings

### Critical

No critical findings.

### High

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| H1 | Coverage Gaps | `new-domain` is `phase: complete`. The plan depends on "nearby helper patterns" without naming which functions, helpers, or modules will be shared vs. re-implemented. If both `create-domain` and `create-service` independently create domain markers, there are two mutation paths for the same governance entity. | Before implementation (NS-4): explicitly map which `new-domain` script helpers `create-service` will reuse, and document the boundary. The auto-establish-missing-parent-domain path (ADR-3) should call existing `create-domain` helpers rather than duplicate domain-marker logic. |

**Response (H1): A. Accept.** ADR-3 updated in `tech-plan.md`: `create-service` explicitly delegates to existing `create-domain` helper functions for parent domain marker and domain constitution creation. No duplicate mutation paths. Delegation boundary documented before NS-4.

| H2 | Assumptions & Blind Spots | The service constitution's BMB-first rule states: "changes to `lens.core.src` must be implemented through skill implementation channels." Stories NS-5 through NS-10 all write to `lens.core.src` (init-feature-ops.py, SKILL.md, release prompt, module-help) but no story references the skill implementation channels explicitly. | Skill modifications (SKILL.md updates, NS-8) route through `.github/skills/bmad-module-builder`; workflow and prompt artifacts (NS-9) route through `.github/skills/bmad-workflow-builder`. These are the canonical BMB implementation channels for this service. Direct edits to `lens.core.src` that bypass these channels are a governance deviation — recorded as accepted for this cycle per `gate_mode: informational`. |

**Response (H2): Modified.** BMB-first rule clarified to name `.github/skills/bmad-module-builder` (skill updates) and `.github/skills/bmad-workflow-builder` (prompt/workflow updates) as the specific implementation channels. `tech-plan.md` and `sprint-plan.md` updated accordingly. Deviation accepted for this cycle.

| H3 | Complexity & Risk | The plan assumes the governance git execution path tested in `new-domain` is stable and reusable. | Governance git execution path confirmed stable. The `new-domain` implementation has been delivered and verified. `create-service` may reuse the same governance git helpers without adding a dedicated regression to NS-7 scope. |

**Response (H3): Confirmed stable.** No additional NS-7 scope required for governance git regression.

### Medium

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| M1 | Logic Flaws | `sprint-plan.md` didn't reference skill implementation channels. | NS-13 updated: handoff notes must record the implementation channel decision: skill updates via `.github/skills/bmad-module-builder`, prompt/workflow via `.github/skills/bmad-workflow-builder`. |

**Response (M1): Applied (see H2).** Sprint-plan.md updated.

| M2 | Cross-Feature Dependencies | `lens-dev-new-codebase-new-feature` had no formal dependency on `new-service`. | `new-service` feature.yaml updated with `depended_by: [lens-dev-new-codebase-new-feature]`; `new-feature` feature.yaml updated with `depends_on: [lens-dev-new-codebase-new-service]`. |

**Response (M2): Recommendation applied.** Both feature.yaml files updated.

| M3 | Coverage Gaps | Retry-safety of `--execute-governance-git` was unspecified. | NS-7 scope updated: governance git path must be idempotent — a failed and retried run must not create duplicate service markers, domain markers, or constitution files. |

**Response (M3): Recommendation applied.** NS-7 in sprint-plan.md updated.

| M4 | Assumptions & Blind Spots | `--dry-run + --execute-governance-git` mutual exclusion was untested. | NS-1 test matrix updated to explicitly include the `--dry-run + --execute-governance-git` rejection case as a required test scenario. |

**Response (M4): Recommendation applied.** NS-1 in sprint-plan.md updated.

### Low

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| L1 | Complexity & Risk | NS-13 (handoff notes) was treated as optional polish. | NS-13 promoted to required story. Also added as a test gate in NS-12: the suite must confirm that NS-13 handoff notes exist and reference the correct files, test commands, clean-room constraints, and implementation channel decisions before verification is considered complete. |

**Response (L1): Documentation updates are always required. Added as a test.** NS-13 is now required; NS-12 validates its existence.

| L2 | Coverage Gaps | No test for the sequential flow where domain already exists when `new-service` runs. | NS-2 scope updated to include an integration test variant starting from a pre-existing domain container (idempotent parent-domain case). |

**Response (L2): Recommendation applied.** NS-2 in sprint-plan.md updated.

| L3 | Logic Flaws | Governance mirror held `expressplan-adversarial-review.md`; control repo staged `expressplan-review.md`. | Governance mirror file renamed to `expressplan-review.md` (GI-2 fix applied). |

**Response (L3): Fixed.** See GI-2.

## Governance Impact Findings

| # | Area | Finding | Action Item |
|---|------|---------|-------------|
| GI-1 | Track Constitution | The `new-codebase` service constitution permitted `[quickplan, full, hotfix, tech-change]` but not `express`. | **Fixed.** Service constitution updated to add `express` to `permitted_tracks`. Both `init-feature-ops.py` constitution templates (release module `lens.core` and new-codebase source) updated to include `express` in the default `permitted_tracks` list so all future service and domain constitutions permit it by default. |

**Response (GI-1): Fixed.** Constitution template and live service constitution updated.

| GI-2 | Cross-Service Naming | `expressplan-adversarial-review.md` in governance mirror vs. `expressplan-review.md` in control repo caused naming drift. | **Fixed.** Governance mirror file renamed from `expressplan-adversarial-review.md` to `expressplan-review.md` via `git mv`. |

**Response (GI-2): Fixed.** Governance mirror corrected.

| GI-3 | Feature Sequencing | `lens-dev-new-codebase-new-feature` had no formal `depends_on: new-service` recorded. | **Fixed.** `new-feature` feature.yaml updated with `depends_on: [lens-dev-new-codebase-new-service]`; `new-service` feature.yaml updated with `depended_by: [lens-dev-new-codebase-new-feature]`. |

**Response (GI-3): Fixed.** Both feature.yaml files updated.

## Accepted Risks

| Finding | Acceptance Rationale |
|---------|----------------------|
| GI-1 (Express track not in constitution) | Fixed: constitution and templates updated to include `express`. No retroactive replan needed for this feature. |
| H2 (BMB-first rule — direct edits bypass skill channels) | BMB-first rule clarified: skill modifications via `bmad-module-builder`, workflow/prompt artifacts via `bmad-workflow-builder`. Direct edits to `lens.core.src` for NS-5–NS-7 (script, context, governance git) accepted for this cycle per `gate_mode: informational`; NS-13 handoff notes must record this deviation explicitly. |
| H3 (Governance git path stability) | Confirmed stable by owner. `new-domain` delivery is complete; `create-service` may reuse the same path without additional regression coverage. |

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
