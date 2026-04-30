---
feature: lens-dev-new-codebase-finalizeplan
doc_type: adversarial-review
phase: expressplan
source: phase-complete
verdict: pass-with-warnings
status: responses-recorded
critical_count: 0
high_count: 2
medium_count: 3
low_count: 1
carry_forward_blockers: []
updated_at: '2026-04-30T00:00:00Z'
review_format: abc-choice-v1
---

# ExpressPlan Adversarial Review — lens-dev-new-codebase-finalizeplan

**Source:** phase-complete  
**Artifacts reviewed:** business-plan.md, tech-plan.md, sprint-plan.md (supplementary)  
**Format:** abc-choice-v1. Response options A–E are presented per finding. Chosen responses are recorded below.

---

## Verdict: `pass-with-warnings`

The planning packet is coherent and dev-actionable. The business plan correctly frames the
three-conductor delivery (FinalizePlan + ExpressPlan + QuickPlan), the tech plan provides
sufficient system design to implement from, and the sprint plan maps the work into three
reviewable slices. No critical blockers were found.

Two high findings address sequencing risks: the rollout split between
"implementation done in prior session" and "planning done in this session" creates a
verification gap — the prior session's files must be read before Sprint 1 stories are
marked done, not assumed. A second high finding flags that the tech plan's FinalizePlan
activation gate describes `techplan-complete` OR `expressplan-complete` as valid
predecessors, but the SKILL.md only checks for `techplan`; this inconsistency must be
resolved during Sprint 1 validation.

Three medium findings address: QuickPlan's sprint-plan.md being referenced as a
QuickPlan output but the SKILL.md not explicitly specifying a sprint-plan artifact;
governance publish being mentioned in both the activation sequence and Step 1 without
clearly disambiguating which call happens when; and the absence of any constitution
check in the test suite.

One low finding notes the tech plan's rollout "Phase 2 — Formal Planning Cycle" is
described as "THIS SESSION" but once committed it will read as historical, which may
confuse future readers.

Selected responses applied below. No carry-forward blockers prevent FinalizePlan handoff.

---

## Response Record

| Option | Meaning |
| --- | --- |
| A / B / C | Accept the proposed resolution with its stated trade-offs |
| D | Provide a custom resolution after `D:` |
| E | Explicitly accept the finding with no action |

---

## Finding Summary

| ID | Severity | Title | Response |
| --- | --- | --- | --- |
| H1 | High | Prior-session files assumed valid — no read-and-verify pass scheduled before Sprint 1 done | **A** |
| H2 | High | FinalizePlan predecessor gate inconsistency: SKILL.md vs tech plan | **A** |
| M1 | Medium | QuickPlan sprint-plan.md output not explicitly in QuickPlan SKILL.md artifacts list | **B** |
| M2 | Medium | Governance publish sequence — on-activation vs Step 1 boundary not clearly disambiguated | **A** |
| M3 | Medium | Constitution check absent from test suite | **B** |
| L1 | Low | Rollout "THIS SESSION" annotation will age poorly in committed doc | **E** |

---

## Findings Detail

### H1 — Prior-session files assumed valid — no read-and-verify pass scheduled

**Observation:** The tech plan and sprint plan both describe the prior session implementation as
"DONE in prior session." Sprint 1 has a story `S1.1–S1.7: Read and validate…` but these are
framed as confirmation stories. If any file from the prior session is actually missing or
malformed, there is no recovery story in Sprint 1 that fixes it — only "document the gap."

**Resolution A (chosen):** Sprint 1 acceptance criteria explicitly requires that validation
stories find no structural gaps. If a gap is found, a new story is added to Sprint 1 before
moving to Sprint 2. The sprint plan exit criterion "All 8 stories done + tests passing" is
the guard. This is acceptable because the 34 tests passing in the prior session already
provide coverage.

---

### H2 — FinalizePlan predecessor gate inconsistency

**Observation:** The tech plan states FinalizePlan can be entered after `techplan` OR
`expressplan-complete`. But the SKILL.md only checks for `techplan` as the predecessor.
The express track enters FinalizePlan from `expressplan-complete`, so FinalizePlan will
fail its own activation gate for express-track features.

**Resolution A (chosen):** Sprint 1 story S1.1 must explicitly validate that the
FinalizePlan SKILL.md activation gate handles both `techplan` and `expressplan-complete`
as valid predecessors. If the SKILL.md only checks `techplan`, a remediation story is
added to Sprint 1 to update the gate condition.

---

### M1 — QuickPlan sprint-plan.md output not in SKILL.md artifacts list

**Observation:** The tech plan lists `sprint-plan.md` as a QuickPlan output artifact. The
SKILL.md for `bmad-lens-quickplan` may list only `business-plan.md` and `tech-plan.md` in
its artifacts section, with sprint-plan as a Phase C output not explicitly surfaced.

**Resolution B (chosen):** Accept the current SKILL.md structure. The sprint-plan is a
Phase C output and is implied by the "business plan → tech plan → sprint plan" pipeline
description. Add a note to Sprint 1 S1.3 to confirm the SKILL.md explicitly calls out
sprint-plan in its artifact or phase contract.

---

### M2 — Governance publish sequence ambiguity

**Observation:** FinalizePlan On Activation step 7 says "publish staged TechPlan artifacts
into governance before starting FinalizePlan outputs." Step 1 also says "commit and push
`{featureId}-plan` via `bmad-lens-git-orchestration commit-artifacts --push`." It is
unclear whether the governance publish (step 7) happens before the adversarial review or
after, and whether it is separate from the Step 1 commit.

**Resolution A (chosen):** The on-activation publish (step 7) is a TechPlan mirror
operation — it copies the TechPlan docs into governance as context for cross-feature
consumers. It runs before the FinalizePlan review begins. The Step 1 commit/push is the
FinalizePlan review output commit to the control repo plan branch. These are two distinct
operations with no ordering conflict. Sprint 1 story S1.1 should confirm this is explicitly
stated in the SKILL.md.

---

### M3 — Constitution check absent from test suite

**Observation:** The ExpressPlan skill has `requires_constitution_permission: true` for the
express track. No test currently verifies that the skill rejects activation when the
constitution does not permit the express track.

**Resolution B (chosen):** Accept the gap for now. The constitution check is a conditional
path that requires loading a constitution file. Add to Sprint 2 story S2.7 a sub-item to
document this as a known coverage gap with a note to add it as a mock-constitution test in
a future hardening sprint.

---

### L1 — Rollout "THIS SESSION" annotation

**Observation:** The tech plan section 7 says "Phase 2 — Formal Planning Cycle (THIS SESSION)."
This phrasing will be confusing to anyone reading the committed doc later.

**Resolution E (chosen):** Accepted without action. The doc is a point-in-time planning
artifact; historical phrasing is expected. Future readers will use the timestamp.

---

## Party-Mode Blind-Spot Challenge

> **Freya (UX/Product):** The business plan lists seven success criteria but five of them
> are infrastructure checks ("prompt stub exists," "module.yaml registration correct"). That's
> not a success criterion — that's a definition of done for a story. What does "success" mean
> to a user who typed `/lens-finalizeplan` for the first time? The one criterion that is
> user-observable is buried: "Adversarial review gate is hard-stop." Can we be more explicit
> about the user experience? What does the user see when they call `/lens-finalizeplan` on a
> feature in `expressplan-complete` phase?

> **Winston (Architect):** The tech plan describes the governance publish as happening "before
> starting FinalizePlan outputs" in On Activation step 7, but then step 1 also runs a commit
> and push. In my reading, the activation publish and the step 1 commit write to different
> locations (governance repo vs control repo plan branch) and should be idempotent. But if the
> publish CLI fails — for example because the TechPlan docs don't exist in the staged docs
> path for an express-track feature (they don't have techplan.md; they have tech-plan.md) —
> does FinalizePlan error with a useful message? The path naming difference (`tech-plan.md`
> vs `techplan.md`) could silently produce an empty governance publish. That's a real risk.

> **Bob (QA):** The sprint plan has 8 stories in Sprint 1 with a combined 12 points. That's
> a lot for a validation sprint. Stories S1.1–S1.7 are all "read and validate" stories. If
> you're already committed to "the 34 tests pass," why do you need 7 separate read-and-validate
> stories? The tests already cover structural correctness. The sprint is over-specified for
> what is essentially a "confirm and commit" sprint. Consider collapsing S1.1–S1.6 into a
> single "validate and confirm all conductor infrastructure" story to reduce ceremony.

> **Mary (Analyst):** Looking at the business plan risks table — "express track constitution
> permission not set, likelihood: Medium." If this is medium likelihood, why isn't there a
> story in Sprint 1 to check it and set it if missing? Right now the only mitigation is
> "Document in feature.yaml; add constitution check in tests." But if the constitution
> doesn't have express permission and a user runs `/lens-expressplan`, they get blocked with
> no obvious fix path. This should be Sprint 1 story, not a risk footnote.

**Agent responses to party-mode challenge:**

**On Freya's UX observation:** Added to business plan success criteria: the user-observable
outcome is "Activating `/lens-finalizeplan` on a feature in `expressplan-complete` proceeds
directly to Step 1 adversarial review without re-prompting for feature or mode." This is the
concrete UX signal.

**On Winston's publish risk:** The `tech-plan.md` vs `techplan.md` naming discrepancy is a
real risk for the governance publish path. Added to Sprint 1 story S1.1 a sub-check: confirm
the publish CLI's artifact name mapping for express-track features handles hyphenated names
(`tech-plan.md`) not the legacy slug form (`techplan.md`). If the CLI doesn't handle it,
flag as a tracked bug.

**On Bob's sprint consolidation:** Sprint 1 stories S1.1–S1.6 consolidated into a single
"validate and confirm all conductor infrastructure" story with checklist acceptance criteria.
Sprint 1 is now 5 stories instead of 8.

**On Mary's constitution risk:** Added Sprint 1 story: "Confirm express-track constitution
permission is set for the `lens-dev/new-codebase` domain. If absent, add it. Document the
permission in the constitution file."
