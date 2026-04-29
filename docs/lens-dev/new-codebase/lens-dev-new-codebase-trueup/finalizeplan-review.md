---
feature: lens-dev-new-codebase-trueup
doc_type: finalizeplan-review
phase: finalizeplan
source: phase-complete
verdict: pass-with-warnings
review_format: abc-choice-v1
reviewed_artifacts:
  - prd.md
  - architecture.md
  - ux-design.md
  - product-brief.md
  - research.md
  - brainstorm.md
  - preplan-adversarial-review.md
  - businessplan-adversarial-review.md
  - techplan-adversarial-review.md
reviewed_at: 2026-04-29T00:00:00Z
predecessor_context:
  - techplan-adversarial-review.md (pass-with-warnings, 5 carry-forward constraints)
  - feature.yaml phase: techplan-complete
---

# FinalizePlan Review — lens-dev-new-codebase-trueup (True Up)

## Review Summary

**Verdict: pass-with-warnings**

| Severity | Count | Resolved in this Review |
|----------|-------|------------------------|
| Critical | 0 | — |
| High | 1 | H1 resolved (action item added to story spec template) |
| Medium | 3 | M1 carry-forward to sprint plan; M2 carry-forward to step-8 story; M3 carry-forward to epics |
| Low | 2 | Both carry-forward noted |

All planning artifacts are present, internally consistent, and grounded in evidence from prior review rounds. The four ADRs are accepted and binding. The five carry-forward constraints from techplan review (CF-1 through CF-5) are acknowledged and allocated to dev stories. No finding blocks phase advancement.

---

## Findings

### H1 — `.github/prompts/` Authority Domain Conflict (Resolved)

**Dimension:** Assumptions and Blind Spots
**Severity:** High
**Description:** Architecture Section 3.1 and FR-2/FR-3 specify that the dev agent must publish prompt stubs to both `_bmad/lens-work/prompts/` and `.github/prompts/`. The control repo authority domain rules (`.github/instructions/lens-control-repo-git.instructions.md`) classify `.github/` as the **Copilot adapter domain — human user only, not modified during feature work**. As written, the architecture implicitly delegates a `.github/` write to the dev agent. This conflicts with the authority domain boundary.

**Resolution:** The implementation steps in architecture Sections 7 and 8 are clarified in this review: publishing prompt stubs to `.github/prompts/` is a **human-performed step** that follows the dev phase. The dev agent is responsible only for authoring and committing the source stubs under `_bmad/lens-work/prompts/` in the feature's target repo. The human operator mirrors them to `.github/prompts/` in a separate commit per the authority domain rules.

**Action for story spec (FR-2, FR-3):** Story acceptance criteria must state: "Dev agent authors `_bmad/lens-work/prompts/lens-{command}.prompt.md`. Publishing to `.github/prompts/` is a post-dev human action and is NOT an acceptance criterion for these stories."

**Finding response options:**
- A) Accept with story spec restriction as described ✅
- B) Escalate to constitution to add an explicit `.github/prompts/` exception for prompt-publishing features
- C) Accept architecture as-is — treat `.github/prompts/` publishing as in-scope for the dev agent
- D) Defer `.github/prompts/` stubs to a separate feature
- E) Reject finding — existing pattern in `.github/prompts/` demonstrates that prompt publishing is already done by agents

**Selected: A — Story specs for FR-2 and FR-3 must restrict dev agent scope to `_bmad/lens-work/prompts/` only.**

---

### M1 — No Master Definition of Done for True Up (Carry-Forward)

**Dimension:** Coverage Gaps
**Severity:** Medium
**Description:** The PRD specifies 14 Functional Requirements and the architecture maps all 14 to an 8-step implementation sequence. Neither document contains an explicit "True Up is complete when…" statement. A dev agent working through the last story in Step 8 has no master completion gate to verify before requesting dev-complete status.

**Resolution Status:** Carry-forward to sprint planning. The sprint plan (`sprint-status.yaml`) must include an acceptance criterion block under the final story that enumerates all 14 FRs and confirms each is satisfied before the feature advances to dev-complete. This is the governing Definition of Done.

**Finding response options:**
- A) Carry-forward: sprint plan final story must include a 14-FR completion checklist ✅
- B) Add a Definition of Done section to the PRD before finalizing
- C) Accept the implicit completion gate (all stories done = feature done)
- D) Delegate completion check to `bmad-lens-complete check-preconditions`
- E) Reject finding — FR list serves as the definition of done

**Selected: A — Sprint plan final story must enumerate 14-FR completion gate.**

---

### M2 — Step 8 Blocker Annotation Partial-Failure Recovery Undefined (Carry-Forward)

**Dimension:** Logic Flaws
**Severity:** Medium
**Description:** Architecture Step 8 writes blocker annotations to two feature.yaml files (`lens-dev-new-codebase-new-feature`, `lens-dev-new-codebase-complete`). This step is explicitly sequenced last for atomicity. However, no recovery procedure is specified: if the dev session writes the annotation for `new-feature` but fails before writing the annotation for `complete`, the governance repo is in a partial state. The annotation for `complete` would be missing without any in-flight indicator.

**Resolution Status:** Carry-forward to the Step 8 story spec. The story must: (a) read both target feature.yaml files before writing any annotation (CF-3 from techplan review); (b) include an idempotency note — running Step 8 twice produces the same governance state without duplication.

**Finding response options:**
- A) Carry-forward: Step 8 story spec must include read-first and idempotency notes ✅
- B) Split Step 8 into two atomic stories, one per feature
- C) Add a rollback command to the governance tooling
- D) Accept the partial-state risk — blocker annotations are low-criticality
- E) Reject finding — sequential write with commit-per-write is sufficient protection

**Selected: A — Step 8 story spec must address read-first and idempotency.**

---

### M3 — FR-14 Parity Gate Spec Cross-Feature Discovery Gap (Carry-Forward)

**Dimension:** Coverage Gaps
**Severity:** Medium
**Description:** The `parity-gate-spec.md` artifact (FR-14) is authored in True Up's docs path and governs all future retained-command migrations. No plan exists to surface this spec from the governance docs mirror or the service constitution after True Up is complete. Future feature authors running new migrations will need to find this spec; the architecture does not specify how.

**Resolution Status:** Carry-forward to epics. The epic for FR-14 should include a post-merge action: the `parity-gate-spec.md` path is referenced in the service-level `constitution.md` under a new "Migration Standards" section (informational). This reference makes the spec discoverable by any future feature author.

**Finding response options:**
- A) Carry-forward: FR-14 epic adds a constitution reference action ✅
- B) Publish parity-gate-spec.md to the governance docs index directly
- C) Create a separate "migration governance" feature to house the spec
- D) Reference from `feature-index.yaml` externaldocs field
- E) Reject finding — the governance docs mirror is a sufficient discovery surface

**Selected: A — FR-14 epic includes a constitution "Migration Standards" reference amendment.**

---

### L1 — CF-5 Blocker Annotation Governance-Visibility Timing (Acknowledged)

**Dimension:** Assumptions and Blind Spots
**Severity:** Low
**Description:** The party-mode finding from techplan review (CF-5) noted that the blocker annotation is the governance-visible record of the `new-feature` regression verdict and is sequenced last. For any governance reviewer tracking True Up progress mid-sprint, the regression verdict on `new-feature` is invisible in governance until Step 8 completes. This is acceptable per the atomicity principle but should be documented.

**Resolution:** Document in the Step 8 story spec: "Until this story completes, the regression verdict for `lens-dev-new-codebase-new-feature` is recorded only in the parity audit report and is NOT reflected in governance. This is expected and correct — blocker annotation is a post-audit write."

**Finding response options:**
- A) Acknowledge; document timing in the Step 8 story spec ✅
- B) Write a partial annotation marker to governance during parity audit (Step 3)
- C) Add a progress note to feature.yaml description during parity audit
- D) Require governance reviewer sign-off before Step 8 can run
- E) Reject finding — visibility timing is a non-issue for an informational blocker annotation

**Selected: A — Step 8 story spec documents the expected timing gap.**

---

### L2 — FR-9 SAFE_ID_PATTERN Scan Evidence Commitment (Acknowledged)

**Dimension:** Coverage Gaps
**Severity:** Low
**Description:** CF-2 from techplan review obligates `parity-audit-report.md` FR-9 section to document the SAFE_ID_PATTERN scan scope, date, and result. The architecture marks this as a carry-forward to the parity audit story. However, the parity audit (Step 3) covers all 5 features and may be a multi-sub-task story. The FR-9 section could be overlooked within a large story if not explicitly called out as an acceptance criterion.

**Resolution:** The Step 3 story spec acceptance criteria must include a checklist item: "[ ] FR-9 section in parity-audit-report.md documents: scan scope (all feature.yaml + feature-index.yaml in TargetProjects/lens/lens-governance/), scan date, and scan result (pass/fail)."

**Finding response options:**
- A) Acknowledge; embed as an explicit AC in Step 3 story spec ✅
- B) Create a separate sub-story for FR-9 SAFE_ID_PATTERN scan evidence
- C) Accept CF-2 as sufficient — no additional action required
- D) Defer scan evidence to the feature-complete review
- E) Reject finding — FR-9 is already called out in CF-2 and the architecture

**Selected: A — Explicit AC added to Step 3 story spec.**

---

## Party-Mode Blind-Spot Challenge (Conducted)

Three planning perspectives applied to the consolidated planning set:

**Winston (Architect):** Flagged that the 8-step implementation sequence assumes serial execution but FR-6 (ADR-1), FR-7 (ADR-2), FR-8 (ADR-3 section), and FR-9 (ADR-4 section) are mutually independent of FR-1 through FR-3 (prompt stubs and parity audit). The sprint plan should expose this parallelism so story assignments don't create artificial serialization.
→ **Carry-forward:** Sprint plan should separate ADR stories from parity-audit and prompt-stub stories to allow parallel sprinting if two contributors are available.

**Mary (Analyst):** Flagged that the 5 impacted features (`new-domain`, `new-service`, `switch`, `new-feature`, `complete`) are governance stakeholders of True Up. Their teams (or solo lead) should be notified when the parity audit report is committed, before the governance PR is opened, so they can contest findings before they become permanent governance record.
→ **Carry-forward:** Sprint plan should include a "parity audit review window" between Step 3 completion and Step 8 blocker annotations.

**Amelia (Dev):** Flagged that the SKILL.md contract specification in architecture Section 3.2 is detailed enough to author directly, but the test stub signatures (FR-5) use `"""docstring"""` stubs without any mock/fixture patterns. A dev agent may author tests that are structurally valid but not runnable (missing conftest.py, missing mock for feature.yaml reads). Test stubs must include fixture hints.
→ **Carry-forward:** FR-5 story spec must include a fixture scaffold requirement: the test file must include at least one `conftest.py` fixture for feature.yaml fixture loading before stubs can be called passing.

---

## Governance Impact Cross-Check

| Impacted Feature | Impact Type | True Up Action | Status |
|-----------------|------------|----------------|--------|
| `lens-dev-new-codebase-new-feature` | Blocker annotation | Step 8 writes `blocker` to feature.yaml | Planned in architecture |
| `lens-dev-new-codebase-complete` | Blocker annotation | Step 8 writes `blocker` to feature.yaml | Planned in architecture |
| `lens-dev-new-codebase-switch` | Phase validation | Parity audit confirms `complete` is accurate | Step 3 |
| `lens-dev-new-codebase-new-domain` | Phase validation | Parity audit confirms `complete` is accurate | Step 3 |
| `lens-dev-new-codebase-new-service` | Phase validation | Parity audit confirms `complete` is accurate | Step 3 |
| Service constitution | Migration standards | FR-14 epic adds a constitution reference | Carry-forward (M3) |
| `parity-gate-spec.md` consumers (future) | Discoverability | Referenced in constitution (M3) | Carry-forward |

**No external service dependencies introduced by True Up.** All impacts are internal to the `lens-dev/new-codebase` service.

---

## Consolidated Carry-Forward Constraints for Dev Phase

| # | Constraint | Source | Required Artifact / Action |
|---|-----------|--------|---------------------------|
| CF-1 | FR-4 story spec must reference exact BMB invocation path (load `lens.core/_bmad/bmb/bmadconfig.yaml`, invoke `bmad-module-builder`, load BMad Builder reference index) | TechPlan H1 | Story spec for FR-4 (SKILL.md) |
| CF-2 | FR-9 section in `parity-audit-report.md` must document scan scope, date, and result (explicit AC in Step 3 story) | TechPlan H2 + L2 | `parity-audit-report.md` FR-9 section |
| CF-3 | Step 8 story spec: read both target feature.yaml files before writing annotations; include idempotency note | TechPlan M2 + M2 (this review) | Story spec for Step 8 |
| CF-4 | `parity-gate-spec.md` must include "How to Apply" section (2–3 paragraphs) | TechPlan party (John) | `parity-gate-spec.md` FR-14 |
| CF-5 | Step 3 story spec: document that blocker annotation is the governance-visible record; timing gap until Step 8 is expected | TechPlan party (Mary) + L1 (this review) | Story spec for Step 3 |
| CF-6 | Story specs for FR-2 and FR-3: restrict dev agent scope to `_bmad/lens-work/prompts/`; `.github/prompts/` publishing is a post-dev human action | H1 (this review) | Story specs for FR-2, FR-3 |
| CF-7 | Sprint plan final story: enumerate 14-FR completion checklist as master Definition of Done gate | M1 (this review) | Sprint plan final story |
| CF-8 | Sprint plan: separate ADR stories from parity-audit and prompt-stub stories for parallelism | Party (Winston, this review) | Sprint plan story sequencing |
| CF-9 | Sprint plan: include parity audit review window between Step 3 and Step 8 | Party (Mary, this review) | Sprint plan milestone |
| CF-10 | FR-5 story spec: test file must include conftest.py fixture scaffold for feature.yaml reading | Party (Amelia, this review) | Story spec for FR-5 |
| CF-11 | FR-14 epic: add post-merge action to reference `parity-gate-spec.md` in service constitution | M3 (this review) | Epic for FR-14 |

---

## Verdict Declaration

**PASS-WITH-WARNINGS**

Phase `finalizeplan` for `lens-dev-new-codebase-trueup` may advance.

- All required planning artifacts present ✅
- All prior review carry-forward constraints (CF-1 through CF-5) allocated to dev stories ✅
- High finding (H1) resolved with story-spec restriction ✅
- No critical findings ✅
- Seven new carry-forward constraints (CF-6 through CF-11) noted for dev story authoring ✅
- Governance impact cross-check complete — no external dependencies ✅

Next action: step 2 plan PR from `lens-dev-new-codebase-trueup-plan` → `lens-dev-new-codebase-trueup`, then step 3 planning bundle.
