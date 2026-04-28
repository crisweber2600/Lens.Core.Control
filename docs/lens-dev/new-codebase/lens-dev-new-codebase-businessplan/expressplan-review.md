---
feature: lens-dev-new-codebase-businessplan
doc_type: expressplan-review
status: complete
review_format: abc-choice-v1
phase: expressplan
verdict: pass-with-warnings
reviewer: lens-adversarial
updated_at: 2026-04-28T00:00:00Z
---

# ExpressPlan Adversarial Review — lens-dev-new-codebase-businessplan

**Phase:** expressplan  
**Scope:** business-plan.md, tech-plan.md, sprint-plan.md  
**Method:** Blind Hunter + Edge Case Hunter + Acceptance Auditor  
**Verdict:** pass-with-warnings  

---

## Findings

### Finding 1 — BP-1 and BP-2 dependency ordering ambiguity

**Severity:** Low  
**Observed:** Sprint plan sequences BP-2 after BP-1 with "BP-1 merged and green" as a dependency, but tech-plan §7 notes they may share a single branch. Sequential merge ordering is stated as required but single-branch atomicity is also mentioned, creating ambiguity.

**Options:**

- **A** — Add a clarifying note to BP-2 that BP-1 and BP-2 may be implemented atomically in a single branch/PR; sequential merge ordering applies only when they are separate PRs.
- **B** — Keep as-is; developers can infer branching strategy.
- **C** — Merge BP-1 and BP-2 into a single story.
- **D** — Defer to implementer entirely.
- **E** — No action needed.

**Decision: A** — Adopted. BP-2 implementation notes updated to clarify atomic option.

---

### Finding 2 — Publish-before-author timing in BP-1 ACs

**Severity:** Low  
**Observed:** BP-1 AC #1 read "no direct governance writes anywhere in the SKILL.md flow" but did not explicitly state the timing requirement (publish must fire _before_ authoring, not just somewhere in the flow).

**Options:**

- **A** — Strengthen AC #1 to explicitly state "publish-to-governance is invoked _before_ any PRD or UX authoring step."
- **B** — Add a dedicated AC for sequence enforcement.
- **C** — Trust the regression test (governance-audit §5.2 already tests timing).
- **D** — Move timing rule to tech-plan only.
- **E** — No action needed.

**Decision: A** — Confirmed. Existing AC #1 in business-plan.md and sprint-plan.md already reads "before any PRD or UX authoring." No additional change required; this finding is resolved.

---

### Finding 3 — /next auto-delegation path not covered in story ACs

**Severity:** Medium  
**Observed:** Neither BP-1 nor BP-2 has an acceptance criterion covering behavior when invoked via `/next`. A SKILL.md rewrite could silently re-ask the run-confirmation prompt on auto-delegation, breaking continuous planning workflows.

**Options:**

- **A** — Add an explicit AC to BP-1 and BP-2: "When invoked via `/next`, no run-confirmation prompt appears; phase entry proceeds immediately."
- **B** — Add a note to BP-3 regression gate that wrapper-equivalence tests must cover the `/next`-delegated invocation path.
- **C** — Accept the gap; `/next` is tested end-to-end separately.
- **D** — Move to a separate regression story.
- **E** — No action needed.

**Decision: B** — Adopted. BP-3 implementation notes updated to explicitly require wrapper-equivalence tests for the `/next` auto-delegation path.

---

### Finding 4 — SKILL.md section template not validated in sprint

**Severity:** Low  
**Observed:** Tech-plan §4 defines a prescriptive section-order template for SKILL.md files. No BP-3 check enforces this structure — a rewrite could omit a required section.

**Options:**

- **A** — Add a note to BP-3 that section structure must be verified against §4 template before merge.
- **B** — Accept; BMB-first authoring channel (bmad-module-builder) enforces structure upstream.
- **C** — Add a linter check (scope increase).
- **D** — Remove the prescriptive template from tech-plan.
- **E** — No action needed.

**Decision: B** — No action needed; bmad-module-builder enforces SKILL.md structure as part of the authoring channel.

---

### Finding 5 — Pre-sprint checklist lacks a verification command

**Severity:** Low  
**Observed:** The pre-sprint checklist names three dependencies but provides no concrete verification step. A developer could misread dependency state and start BP-1 on an incomplete workspace.

**Options:**

- **A** — Add a verification command (e.g., validate-phase-artifacts.py) to the pre-sprint checklist.
- **B** — Reference the lens preflight command (`#prompt:lens-preflight.prompt.md`) as the verification step.
- **C** — Trust the developer to check story statuses manually.
- **D** — Move to automated CI gate.
- **E** — No action needed.

**Decision: B** — Adopted. Pre-sprint checklist updated to reference `#prompt:lens-preflight.prompt.md` as the verification command.

---

## Verdict Summary

| Finding | Severity | Decision | Action |
|---------|----------|----------|--------|
| 1 — BP-1/BP-2 branching ambiguity | Low | A | Applied to sprint-plan.md BP-2 notes |
| 2 — Publish-before-author timing | Low | A | Confirmed correct as written |
| 3 — /next path missing | Medium | B | Applied to sprint-plan.md BP-3 notes |
| 4 — SKILL.md template enforcement | Low | B | No action; BMB-first handles it |
| 5 — Pre-sprint checklist | Low | B | Applied to sprint-plan.md pre-sprint section |

**Overall verdict: pass-with-warnings**

All findings are Low–Medium severity. Two minor wording improvements applied to sprint-plan.md. No blocking issues found. The QuickPlan artifacts are ready for phase completion.
