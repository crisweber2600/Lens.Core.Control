---
feature: lens-dev-new-codebase-expressplan
doc_type: expressplan-review
phase: expressplan
source: phase-complete
verdict: pass-with-warnings
status: responses-recorded
critical_count: 0
high_count: 2
medium_count: 2
low_count: 0
carry_forward_blockers: []
updated_at: '2026-04-30T00:00:00Z'
review_format: abc-choice-v1
---

# ExpressPlan Review — lens-dev-new-codebase-expressplan

**Source:** phase-complete  
**Artifacts reviewed:** business-plan.md, tech-plan.md, sprint-plan.md (supplementary)  
**Format:** abc-choice-v1. Response options A–E are presented per finding. Chosen responses are recorded below.

---

## Verdict: `pass-with-warnings`

The staged packet is coherent and usable as the expressplan artifact set for the expressplan
command rewrite. No critical blockers were found. Two high findings address implementation
risks — foundation validation recovery and discovery surface identity — both of which have
clear resolution paths. Two medium findings address test coverage assumptions and shared
prerequisite confirmation. None of these prevent FinalizePlan handoff; they become Slice 1
and Slice 2 acceptance items.

The selected responses have been applied: the planning packet now explicitly acknowledges
that discovery wiring is the highest-priority Slice 2 item, that foundation validation failure
requires a fix-and-revalidate pass before Slice 2 can begin, and that test file contents must
be confirmed against the party-mode and fail-verdict coverage obligations.

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
| H1 | High | Foundation validation has no defined recovery path | **A** |
| H2 | High | Discovery surface identity is fully deferred | **A** |
| M1 | Medium | Test file contents unverified; party-mode coverage assumed | **A** |
| M2 | Medium | Shared skill prerequisites in target project unconfirmed | **B** |

---

## Party-Mode Blind-Spot Challenge

> **Freya (UX/Product):** The plan talks about restoring the expressplan command, but it says
> "discovery wiring is to be confirmed." If a Lens user types `/expressplan` and gets nothing
> because discovery isn't wired, the feature isn't usable at all. The plan should name at least
> one concrete path to check — not just defer it. What does "lens-expressplan appears in help"
> actually mean in the new codebase? Is there a concrete file you can point to right now?

> **Winston (Architect):** The tech plan lists the previous-session infrastructure as
> "status: Created in previous session" but doesn't read or verify those files. The entire
> Slice 1 workstream says "validate the conductor skill" but if the SKILL.md is missing the
> express-eligibility gate or the party-mode enforcement, Slice 1 would catch it — but then
> what? There's no recovery slice defined. The plan assumes validation passes. What happens
> if it doesn't?

> **Bob (QA):** You have one test file, `test-expressplan-ops.py`, created without this review
> reading it. The plan says it "covers prompt-start and wrapper-equivalence." Does it? Have you
> read it? Party-mode enforcement coverage is listed as a regression expectation but no test is
> named for it. If the review-gate can produce a fail verdict, who catches that the test for the
> fail-verdict behavior is missing?

**Blind-spot questions posed:**

1. What is the exact discovery file where `lens-expressplan` should appear?
2. If SKILL.md validation fails (eligibility gate missing), what does recovery look like?
3. Has `test-expressplan-ops.py` been confirmed to cover party-mode enforcement and fail-verdict blocking?
4. Do `bmad-lens-quickplan`, `bmad-lens-bmad-skill`, and `bmad-lens-adversarial-review` already
   exist in the target project?
5. Is `validate-phase-artifacts.py` present in the target project?

---

## High Findings

### H1 — Foundation validation has no defined recovery path

**Location:** sprint-plan.md, Slice 1; tech-plan.md, Workstream 1  
**Gate:** Before Slice 2 can begin

The sprint plan designates Slice 1 as foundation validation but does not define what happens
if validation fails. If the previous-session SKILL.md is missing the express-eligibility gate,
party-mode enforcement, or the correct phase-advance invocation, Slice 2 cannot begin — but
there is no defined Slice 0 fix-and-revalidate path.

**Recorded response:** **A**  
**Applied adjustment:** The implementation plan now explicitly gates Slice 2 on Slice 1 exit
criteria. If validation in Slice 1 discovers a gap in the conductor skill, a remediation pass
is required before Slice 2 begins. Discovery and regression work are not started until the
foundation is confirmed valid.

**Choose one:**

- **A.** Make Slice 2 entry explicitly conditional on Slice 1 exit criteria; treat any
  foundation gap as a blocking remediation item before regression work begins.  
  **Why pick this:** Prevents regression work from being built on an unvalidated foundation.  
  **Why not:** Adds a potential gate-within-slice that could stall discovery if
  infrastructure needs rework.

- **B.** Accept the validation risk; proceed to discovery and regressions in parallel since
  the created infrastructure is based on a well-established reference (techplan feature).  
  **Why pick this:** Avoids over-sequencing when the infrastructure is likely correct.  
  **Why not:** Parallel progress on an invalid foundation creates rework debt.

- **C.** Split foundation validation into its own explicit pre-slice checklist that must
  be signed off before any Slice 1 delivery items are marked complete.  
  **Why pick this:** Makes the gate visible and auditable.  
  **Why not:** Adds governance overhead for a task that should be a quick read-and-confirm.

- **D.** Write your own response.
- **E.** Accept the implicit risk; the reference material is strong enough to assume the
  infrastructure is correct.

---

### H2 — Discovery surface identity is fully deferred

**Location:** tech-plan.md, Remaining Implementation Scope, Discovery Wiring; sprint-plan.md, Slice 2  
**Gate:** Before Slice 2 exit criteria can be verified

Both the tech plan and sprint plan state the discovery file will be "confirmed as part of the
first implementation slice." This is not a resolution — it is a deferred question. If the
target project has no retained command registry yet, discovery wiring cannot happen without
first creating that surface, which is outside this feature's scope.

**Recorded response:** **A**  
**Applied adjustment:** The planning packet now designates discovery file identification as
the highest-priority Slice 2 item. The implementation agent must identify the file before any
other Slice 2 work begins. If no registry exists, the agent is to flag it as a dependency gap
rather than proceeding as if the file will appear.

**Choose one:**

- **A.** Make discovery file identification the first action in Slice 2; if no registry
  exists, flag the gap explicitly rather than deferring to implementation intuition.  
  **Why pick this:** Surfaces the risk early when it can still be addressed as a tracked item.  
  **Why not:** Requires looking at the target project state before implementation can plan
  forward — a minor overhead.

- **B.** Accept the deferral and let the implementation agent resolve it; the baseline
  research likely contains the answer.  
  **Why pick this:** Avoids over-specifying implementation details in the planning packet.  
  **Why not:** The planning packet becomes less useful as a handoff artifact if this remains
  genuinely unknown.

- **C.** Treat discovery wiring as out-of-scope for this feature and define it as a
  prerequisite gap for a separate retained-command discovery feature.  
  **Why pick this:** Keeps this feature's scope clean.  
  **Why not:** Leaves the expressplan command undiscoverable after the feature is declared done.

- **D.** Write your own response.
- **E.** Accept the deferral explicitly; discovery wiring is an implementation detail
  the agent can resolve without planning-packet guidance.

---

## Medium Findings

### M1 — Test file contents unverified; party-mode coverage assumed

**Location:** sprint-plan.md, Slice 2; tech-plan.md, Regression and Validation  
**Gate:** Before Slice 3 exit criteria can be verified

The planning packet lists `test-expressplan-ops.py` as covering "prompt-start and
wrapper-equivalence" regressions, but no planning document has read or confirmed the test file
contents. Party-mode enforcement and fail-verdict blocking behavior are listed as regression
expectations but no test is named or verified for either.

**Recorded response:** **A**  
**Applied adjustment:** Slice 1 now includes reading `test-expressplan-ops.py` and confirming
which expectations are covered and which are missing. The test file review is part of the
foundation validation step, not deferred to Slice 2.

**Choose one:**

- **A.** Read `test-expressplan-ops.py` as part of Slice 1 foundation validation and
  identify any coverage gaps against the defined regression expectations.  
  **Why pick this:** Closes the assumption before regression hardening begins.  
  **Why not:** Slightly expands Slice 1 scope.

- **B.** Trust the test file covers the stated expectations based on the previous session
  context; confirm only if a gap is surfaced during Slice 2.  
  **Why pick this:** Avoids redundant reading of a recently created file.  
  **Why not:** The review cannot verify this assumption without reading the file.

- **C.** Defer test coverage verification to the implementation agent; the test file is an
  implementation artifact, not a planning artifact.  
  **Why pick this:** Keeps the planning packet focused on intent rather than verification.  
  **Why not:** Leaves a material gap in the handoff quality.

- **D.** Write your own response.
- **E.** Accept the assumption explicitly; the test file is new and unlikely to have coverage
  gaps given the reference material used.

---

### M2 — Shared skill prerequisites in target project unconfirmed

**Location:** tech-plan.md, Required Runtime Behavior; sprint-plan.md, Slice 2  
**Gate:** Before end-to-end expressplan execution can be validated

The expressplan conductor delegates to `bmad-lens-bmad-skill --skill bmad-lens-quickplan` and
invokes `bmad-lens-adversarial-review`. The planning packet does not confirm whether these
shared skills exist in the new-codebase target project. If they are absent, end-to-end
execution cannot be validated even if the expressplan surface is complete.

**Recorded response:** **B**  
**Applied adjustment:** The planning packet treats shared skill availability as an explicit
prerequisite check item in Slice 2. The implementation agent should confirm whether
`bmad-lens-quickplan`, `bmad-lens-bmad-skill`, and `bmad-lens-adversarial-review` exist in the
target project before claiming end-to-end execution is possible.

**Choose one:**

- **A.** Absorb the shared skill verification into this feature's Slice 2 scope with
  explicit go/no-go criteria for end-to-end validation.  
  **Why pick this:** Makes end-to-end completion a real, not assumed, outcome for this feature.  
  **Why not:** Significantly expands scope beyond the command-surface delivery.

- **B.** Treat shared skill availability as a prerequisite check; if absent, flag as a
  dependency gap without absorbing the delivery of those skills into this feature.  
  **Why pick this:** Keeps this feature's scope focused on expressplan command surface;
  avoids scope creep.  
  **Why not:** Leaves end-to-end validation dependent on a prerequisite that may not be
  closed before this feature completes.

- **C.** Assume shared skills are present based on the techplan feature pattern and
  proceed without explicit verification.  
  **Why pick this:** Avoids over-checking prerequisites that are likely in place.  
  **Why not:** This is an assumption not supported by any evidence in the planning packet.

- **D.** Write your own response.
- **E.** Accept the risk; the implementation agent can resolve this at execution time.
