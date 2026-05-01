---
feature: lens-dev-new-codebase-dogfood
doc_type: finalizeplan-review
phase: finalizeplan
source: manual-rerun
verdict: pass-with-warnings
status: review-complete
critical_count: 0
high_count: 2
medium_count: 3
low_count: 2
review_format: abc-choice-v1
responses_recorded: true
updated_at: '2026-05-01T14:00:00Z'
---

# FinalizePlan Review: lens-dev-new-codebase-dogfood

**Reviewed:** 2026-05-01T14:00:00Z  
**Phase:** finalizeplan  
**Source:** manual-rerun  
**Overall Verdict:** pass-with-warnings

## Summary

The combined express-track planning set (business-plan, tech-plan, sprint-plan, expressplan-adversarial-review) is coherent and ready for downstream planning bundle execution. The packet correctly identifies the 5-sprint implementation path, centers the 17-command baseline contract, integrates the BF-1 through BF-6 bugfix backlog, and absorbs ExpressPlanBugs.md defects into the delivery plan. The planning handoff can proceed to epics, stories, implementation-readiness, sprint-status, and story files.

Two high-risk gaps must be carried into implementation: ExpressPlanBugs.md defect-to-story traceability is implicit rather than explicit, and sprint stories do not name BMB as the required implementation channel despite the domain and service constitution mandating it for lens-work artifact changes. Three medium issues round out the warnings. No critical blockers prevent progression.

---

## Governance Cross-Check

### Related Features Assessed

| Feature | Status | Impact on Dogfood |
|---------|--------|-------------------|
| `lens-dev-new-codebase-finalizeplan` | archived / dev-complete | **Positive precedent.** Already built `bmad-lens-finalizeplan`, `bmad-lens-expressplan`, and `bmad-lens-quickplan` conductors with 38 passing tests. Dogfood stories covering conductor parity (S3 range) must test against this existing implementation, not re-implement from scratch. |
| `lens-dev-new-codebase-techplan` | archived | Provides publish orchestration, BMAD wrapper, and constitution loading patterns. Dogfood's S2/S3 stories should reference this as the authoritative precedent for publish and adversarial review integration. |
| `lens-dev-new-codebase-dev` | preplan | **Sequencing risk.** Sprint 4 (S4.1 Dev conductor) overlaps with this unstarted governance feature. Sprint 4 must coordinate with or explicitly not duplicate `lens-dev-new-codebase-dev` planning work to avoid authority conflicts. |
| `lens-dev-new-codebase-split-feature` | techplan | Still in planning. No immediate impact on dogfood, but any split-feature implementation that modifies git-orchestration behavior could conflict with S2.1-S2.5 topology work. |
| `lens-dev-new-codebase-next` | archived | Express-track peer that completed FinalizePlan. Provides a same-track template for the downstream bundle shape. |
| `lens-dev-new-codebase-discover` | archived | Established the bidirectional inventory sync and governance-main auto-commit exception patterns. Dogfood's S2.3 (BF-3 feature-index sync) should align with this precedent. |

### Feature-Index Stale State

The feature-index.yaml currently shows dogfood status as `expressplan` (not `expressplan-complete`). This is a live instance of BF-3 and S2.3. The feature-index entry must be updated after this review is committed and pushed.

### Target Repo Registration

`feature.yaml.target_repos` is still `[]`. The expressplan review recorded response A (M4): update through feature-yaml and add validation. This must be actioned before implementation begins. The `lens.core.src` local path is `TargetProjects/lens-dev/new-codebase/lens.core.src`.

---

## Response Record

| Finding | Response | Recorded Action |
|---------|----------|-----------------|
| H1 | D | Add an explicit traceability table in the implementation-readiness artifact that maps each ExpressPlanBugs.md defect to the story or acceptance criterion that covers it. |
| H2 | D | Add a `## Implementation Channel` section to every story file that names BMB as the required channel for lens-work artifact changes, and flags when a story does NOT touch lens-work so reviewers can verify the exception. |
| M1 | D | Register `lens.core.src` as target repo in `feature.yaml` via `bmad-lens-feature-yaml update` before story execution begins. |
| M2 | A | Sprint 4 stories must include a governance coordination note that dogfood's Dev conductor work must not duplicate the `lens-dev-new-codebase-dev` feature scope, and any overlap must be tracked as explicit acceptance criteria alignment. |
| M3 | A | ADR-5 (express review filename) is accepted tech debt; record it explicitly in sprint-status under accepted-risks and add a story in Sprint 5 (parity report) to document the compatibility mapping. |
| L1 | A | Add an explicit `feature-index.yaml` sync step as part of the commit sequence after this review is pushed. |
| L2 | A | Update `feature.yaml` phase to `finalizeplan` via the publish CLI before opening the plan PR. |

---

## Findings

### Critical

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| — | — | No critical blockers found. | Proceed with the warnings below carried into implementation. |

---

### High

#### H1 — ExpressPlanBugs.md Defect-to-Story Traceability is Implicit

**Dimension:** Coverage Gaps  
**Finding:** The tech-plan lists 8 ExpressPlanBugs.md defects in a table and maps them to architectural decisions and ADRs. The sprint plan assigns stories that address most defects but does not include an explicit defect → story cross-reference. Defect 1 (constitution resolver false negative), Defect 3 (Windows path normalization), and Defect 6 (dirty-state explicit handling) have no uniquely named stories; their coverage is inferred from story scope descriptions.  
**Recommendation:** Add a defect traceability table to the implementation-readiness artifact. Every one of the 8 defects must map to either a story ID and acceptance criterion, or an explicitly accepted risk. This is the governance evidence that the defect intake was fully absorbed.

**Response:** D — Include an explicit defect traceability table in `implementation-readiness.md` that maps all 8 ExpressPlanBugs.md defects to story IDs and acceptance criteria before the first implementation sprint begins.

**Response Options**

- **A.** Add a defect table to the sprint-plan — **Why pick:** Easy location. / **Why not:** Sprint plan is already sequenced; adding a new artifact is cleaner.
- **B.** Rely on the tech-plan ADR table as sufficient traceability — **Why pick:** Already written. / **Why not:** ADRs cover design decisions, not acceptance-level story coverage gaps.
- **C.** Accept the implicit traceability as good enough for this dogfood scope — **Why pick:** Saves authoring time. / **Why not:** Directly contradicts the purpose of ExpressPlanBugs.md as a defect intake source.
- **D.** Write your own response — provide a custom resolution not covered by A, B, or C.
- **E.** Keep as-is — explicitly record that no action will be taken on this finding.

---

#### H2 — BMB Implementation Channel Not Named in Sprint Stories

**Dimension:** Authority and Governance Compliance  
**Finding:** The domain constitution (`lens-dev`) and service constitution (`new-codebase`) both mandate BMB (`.github/skills/bmad-module-builder` for SKILL.md changes, `.github/skills/bmad-workflow-builder` for release prompts/workflows) as the implementation channel for any story touching `lens.core.src` lens-work files. The sprint plan does not name BMB as the required channel in any story. Stories S1.2 (lifecycle contract), S1.3 (module config), S1.4 (feature-yaml), S3.1 (prompt stubs), S3.2 (module.yaml), S3.3 (skills), and S4.1 (Dev conductor) all touch lens-work; failing to declare BMB means implementation agents may write directly and create a governance deviation.  
**Recommendation:** Each story file generated by this FinalizePlan bundle must include an `Implementation Channel` section that names the required BMB path. Stories that do NOT touch lens-work must also declare the exception explicitly.

**Response:** D — Every story file produced in Step 3 of this FinalizePlan must include an `## Implementation Channel` section declaring the required BMB path or documenting the exception. Implementation agents must load the referenced BMB documentation before authoring any skill, workflow, or prompt artifact.

**Response Options**

- **A.** Add a global note to the sprint-plan — **Why pick:** One-time fix. / **Why not:** Story files are the authoritative per-story contract; a sprint-plan note can be missed.
- **B.** Rely on the service constitution being loaded at session start — **Why pick:** Avoids repetition. / **Why not:** Story files are handed to dev agents who may not load the constitution unprompted.
- **C.** Require only the most affected stories (S1.2, S3.1, S4.1) to name BMB — **Why pick:** Reduces noise. / **Why not:** Hard to know at planning time which stories will touch lens-work without reading every story.
- **D.** Write your own response — provide a custom resolution not covered by A, B, or C.
- **E.** Keep as-is — explicitly record that no action will be taken on this finding.

---

### Medium

#### M1 — Target Repo Registration Remains Unresolved

**Dimension:** Completeness  
**Finding:** `feature.yaml.target_repos` is empty despite the expressplan review (M4) recording response A to register `lens.core.src`. Implementation agents cannot resolve target repositories without this field, and Dev validation will fail at story execution time.  
**Recommendation:** Update `feature.yaml` via `bmad-lens-feature-yaml update --featureId lens-dev-new-codebase-dogfood` to register `lens.core.src` before story file creation in Step 3.

**Response:** D — Register the target repo immediately: update `feature.yaml` with `target_repos` entry for `lens.core.src` before the downstream bundle runs.

**Response Options**

- **A.** Register target repo now — **Why pick:** Unblocks Dev. / **Why not:** None; this is correct.
- **B.** Register target repo in Sprint 1 as story — **Why pick:** Keeps planning artifacts clean. / **Why not:** Missing registration is a metadata gap, not an implementation story; it should be fixed now.
- **C.** Leave empty until Dev starts — **Why pick:** Deferred effort. / **Why not:** Breaks Dev validation and cross-feature context at the worst time.
- **D.** Write your own response — provide a custom resolution not covered by A, B, or C.
- **E.** Keep as-is — explicitly record that no action will be taken on this finding.

---

#### M2 — Sprint 4 Dev Conductor Coordination with `lens-dev-new-codebase-dev`

**Dimension:** Cross-Feature Dependencies  
**Finding:** The `lens-dev-new-codebase-dev` feature is at preplan status with no expressplan or finalizeplan artifacts. Sprint 4 of dogfood (S4.1-S4.5) re-implements Dev conductor behavior for parity. If `lens-dev-new-codebase-dev` later proceeds, the two feature scopes could conflict, duplicate effort, or create two competing Dev conductor implementations in the target codebase.  
**Recommendation:** Sprint 4 story files must include a coordination note linking to `lens-dev-new-codebase-dev` and declaring the dogfood intent: parity testing against the existing reference implementation, not a fresh authoring of a net-new Dev skill from `lens-dev-new-codebase-dev`'s eventual PRD.

**Response:** A — Include explicit coordination notes in S4.1-S4.5 story files naming `lens-dev-new-codebase-dev` as the canonical Dev feature, and scoping dogfood Dev stories as parity-implementation work against the reference module rather than first-class feature authoring.

**Response Options**

- **A.** Add coordination note to Sprint 4 story files — **Why pick:** Correct scope; avoids duplication. / **Why not:** None.
- **B.** Merge Sprint 4 scope into `lens-dev-new-codebase-dev` — **Why pick:** Consolidates governance. / **Why not:** `lens-dev-new-codebase-dev` is at preplan; waiting for it would block dogfood for multiple sprints.
- **C.** Ignore the overlap and let implementation agents reconcile — **Why pick:** Faster to start. / **Why not:** Guaranteed to create authority boundary violations at implementation time.
- **D.** Write your own response — provide a custom resolution not covered by A, B, or C.
- **E.** Keep as-is — explicitly record that no action will be taken on this finding.

---

#### M3 — ADR-5 Express Review Filename Compatibility Remains Open

**Dimension:** Assumptions and Blind Spots  
**Finding:** ADR-5 resolves the current lifecycle naming (`expressplan-adversarial-review.md`) but leaves the compatibility mapping for older `expressplan-review.md` references as tech debt. This tech debt has no story assigned and no acceptance criterion. If the compatibility mapping is not implemented, publish-to-governance and validation tooling will silently miss express review artifacts from older features.  
**Recommendation:** Add an explicit acceptance criterion to S2.6 (express publish artifact mapping) or create a new story in Sprint 5 that tests both filenames are recognized by publish-to-governance and validation operations.

**Response:** A — Add an acceptance criterion to S2.6 story file: publish-to-governance must recognize both `expressplan-adversarial-review.md` and legacy `expressplan-review.md` when resolving express track artifacts, and must report which filename was matched.

**Response Options**

- **A.** Add AC to S2.6 — **Why pick:** Direct coverage in the right story. / **Why not:** None.
- **B.** Defer to Sprint 5 parity report — **Why pick:** Consolidates retrospective items. / **Why not:** If not implemented, it will appear as a known gap in the parity report with no owner.
- **C.** Accept ADR-5 as final — **Why pick:** Avoids backward compat work. / **Why not:** Other features in governance still use `expressplan-review.md`; read failures are real.
- **D.** Write your own response — provide a custom resolution not covered by A, B, or C.
- **E.** Keep as-is — explicitly record that no action will be taken on this finding.

---

### Low

#### L1 — Feature-Index Entry is Stale

**Dimension:** Governance Hygiene  
**Finding:** The feature-index.yaml entry for `lens-dev-new-codebase-dogfood` shows `status: expressplan` and `updated_at: '2026-04-30T22:22:28Z'`. The feature advanced to `expressplan-complete` at `2026-05-01T12:47:54Z`. This is exactly the BF-3 scenario that S2.3 targets.  
**Recommendation:** Run the feature-index sync operation after this review is committed and pushed. Until S2.3 lands, manually ensure the governance commit includes an updated feature-index entry for dogfood.

**Response:** A — Sync feature-index.yaml as part of the FinalizePlan commit sequence. Record the stale entry as a live acceptance criterion example for S2.3.

---

#### L2 — Phase Transition to `finalizeplan` Not Yet Recorded in Feature YAML

**Dimension:** Governance Hygiene  
**Finding:** `feature.yaml` still shows `phase: expressplan-complete`. The FinalizePlan phase should be recorded in governance before the plan PR is opened to establish a clear timeline.  
**Recommendation:** Update `feature.yaml` via the `bmad-lens-feature-yaml` update operation before opening the planning PR.

**Response:** A — Update `feature.yaml` phase to `finalizeplan` and push to governance main before the plan PR.

---

## Party-Mode Challenge

**John (PM):** Five sprints is honest. My concern is that the user does not see evidence of working behavior until Sprint 5. Can Sprint 1 produce something observable — even a passing test or a command that can resolve a feature and report its phase — so that dogfood validates itself before reaching story execution?

**Winston (Architect):** The three-branch topology is correct and the sprint order is right. But Sprint 3 carries XL estimates for command surface work, and Sprint 4 is also XL for Dev. That's two consecutive XL sprints with heavy authority-boundary risk. I want to see S3 and S4 broken into M-sized stories with explicit acceptance criteria before implementation starts, not discovered during execution.

**Mary (Analyst):** My blind spot is the clean-room boundary under pressure. When Sprint 3 agents are comparing behavior between `lens.core` and `lens.core.src`, how does the governance record show that no prose or code was copied? The plan asserts it won't happen, but the acceptance criteria for clean-room compliance is "comparison only" — and that's hard to validate after the fact. I want a concrete clean-room checkpoint step added to Sprint 5 before the parity report is marked complete.

**Recorded Gap — Clean-Room Compliance Checkpoint:** Add a clean-room compliance step to S5.5: before the parity report is finalized, run an external hash check on touched `lens.core.src` files against `lens.core` counterparts and include the negative result as evidence in the parity report.

---

## Accepted Risks

- QuickPlan remains internal-only for dogfood scope; any broader standalone QuickPlan parity is compatibility debt.
- Express review filename compatibility (`expressplan-review.md` vs `expressplan-adversarial-review.md`) is recorded tech debt; current lifecycle naming is canonical.
- Clean-room file hash verification is handled outside VS Code; a compliance checkpoint is now added to Sprint 5 (S5.5) rather than remaining implicit.
- `lens-dev-new-codebase-dev` at preplan is a coordination risk for Sprint 4; Sprint 4 stories are scoped as parity work, not first-class Dev feature authoring.
