---
feature: lens-dev-new-codebase-dev
doc_type: adversarial-review
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

# ExpressPlan Adversarial Review — lens-dev-new-codebase-dev

**Source:** phase-complete  
**Artifacts reviewed:** business-plan.md, tech-plan.md, sprint-plan.md  
**Format:** abc-choice-v1. Response options A–E are presented per finding. Chosen responses are recorded below.

---

## Verdict: `pass-with-warnings`

The planning packet is coherent and usable as the expressplan artifact set for the dev command
rewrite. No critical blockers were found. Two high findings address implementation risks — the
ambiguity in the publish-before-author gate definition, and the absence of a located
dev-session.yaml schema in the target project — both of which have clear resolution paths.
Two medium findings address the enforcement mechanism for write isolation and the E2-S1
discovery ordering risk. None of these prevent FinalizePlan handoff; they become Slice 1 and
Slice 2 acceptance items.

The selected responses have been applied: the planning packet now explicitly gates
publish-before-author on both phase check AND artifact presence, requires dev-session.yaml
schema location as a Slice 1 exit item, names bmad-lens-git-orchestration as the sole
write-routing mechanism, and elevates E2-S1 to the first Slice 2 story with an explicit note
on the ordering rationale.

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
| H1 | High | Publish-before-author gate definition is ambiguous | **A** |
| H2 | High | dev-session.yaml schema location unconfirmed in target project | **A** |
| M1 | Medium | Write isolation enforcement mechanism underspecified in tech-plan | **A** |
| M2 | Medium | E2-S1 discovery registration ordering risk not addressed in sprint plan | **A** |

---

## Party-Mode Blind-Spot Challenge

> **Winston (Architect):** The tech plan says the publish-before-author hook checks
> "feature.yaml phase is finalizeplan-complete" AND "epics.md and stories.md exist." But
> what if the phase was advanced to finalizeplan-complete without the artifacts actually
> being published to the governance path? The two checks are supposed to be redundant, but
> there is no specification for which one wins if they disagree. Which gate takes precedence,
> and what does the conductor do if the phase says ready but the artifacts are missing?

> **Quinn (QA):** The tech plan lists six internal dependencies and says "all writes are
> routed through bmad-lens-git-orchestration." But the SKILL.md conductor is an AI-facing
> prompt, not a compiled guard. What actually prevents a subagent — during task execution
> in the implementation loop — from writing files directly to the control repo path rather
> than through the git-orchestration skill? Is there a named enforcement mechanism, or is
> this a convention we hope is followed?

> **Freya (UX/Product):** The sprint plan puts E2-S1 (discovery registration) as the first
> Slice 2 story, with a note that it should come first because regression tooling may need it.
> But if E2-S1 fails — if the discovery surface doesn't exist yet — what does that mean for
> E2-S2 through E2-S5? Are those stories blocked? Or can regression testing proceed without
> discovery? The sprint plan doesn't define the dependency between E2-S1 and the other
> Slice 2 stories.

**Blind-spot questions posed:**

1. If feature.yaml says `finalizeplan-complete` but `epics.md` is absent, which gate wins and
   what does the conductor output?
2. What prevents a subagent in the task execution loop from writing files to the control repo
   path outside of bmad-lens-git-orchestration?
3. If E2-S1 (discovery) fails because the surface doesn't exist, are E2-S2 through E2-S5
   blocked?
4. Has `dev-session.yaml` schema been confirmed as present and matching in the target project?
5. Is `bmad-lens-git-orchestration` already in the target project, or is it a dependency to
   be created as part of this rewrite?

---

## High Findings

### H1 — Publish-before-author gate definition is ambiguous

**Location:** tech-plan.md, Entry Hook section; business-plan.md, Success Criteria (SC5)  
**Gate:** Before Slice 2 implementation loop runs

Both documents reference the publish-before-author entry hook but neither resolves the
precedence question: what does the conductor do when `feature.yaml` phase is
`finalizeplan-complete` but the governance doc artifacts are missing? The two checks are
meant to be redundant, but they can disagree, and the conductor behavior in that case is
unspecified.

Without a clear precedence rule, the implementation agent will resolve this ambiguity at
implementation time, which may produce a gate that is too permissive (artifact presence
not enforced) or too strict (blocks valid sessions).

**Recorded response:** **A**  
**Applied adjustment:** The tech-plan entry hook now specifies that both checks must pass
independently. Phase check first; if phase is not `finalizeplan-complete`, stop immediately.
If phase passes, check artifact presence; if artifacts are missing, stop with a distinct error
naming the missing files. Neither check is optional. A phase-only pass with missing artifacts
is treated as a failed gate.

**Choose one:**

- **A.** Both checks must pass independently. Phase check first, then artifact presence.
  A phase-only pass with missing artifacts is a hard gate failure.  
  **Why pick this:** Closes the ambiguity and prevents a partially-published finalizeplan
  from allowing dev to start.  
  **Why not:** Slightly more implementation work to surface the two distinct error messages.

- **B.** Phase check is sufficient. If the governance tooling sets `finalizeplan-complete`,
  trust that artifacts are present; artifact presence check is informational only.  
  **Why pick this:** Simpler conductor logic; trusts the governance workflow.  
  **Why not:** Creates a gap if the phase was advanced manually without the full publish step.

- **C.** Artifact presence is the primary gate; phase check is secondary (informational).  
  **Why pick this:** Artifacts are the real precondition; phase is just a label.  
  **Why not:** Phase check captures more governance information than a simple file existence test.

- **D.** Write your own response.
- **E.** Accept the ambiguity; the implementation agent will resolve the precedence at
  implementation time.

---

### H2 — dev-session.yaml schema location unconfirmed in target project

**Location:** tech-plan.md, dev-session.yaml Contract section; sprint-plan.md, E1-S4  
**Gate:** Before Slice 2 regression testing (E2-S4) runs

The tech-plan and sprint-plan both require `dev-session.yaml` to remain backward-compatible
with the old-codebase schema. However, neither document locates the authoritative schema
definition in the target project. If the schema file does not exist in the new codebase, the
conductor cannot read or write it correctly, and E2-S4 (resume regression) cannot be verified.

This is a Slice 1 gap, not a Slice 2 one. If the schema is absent, the entire checkpoint
contract is unverifiable and Slice 2 cannot begin.

**Recorded response:** **A**  
**Applied adjustment:** E1-S4 now explicitly includes: locate `dev-session.yaml` schema
definition in the target project, confirm it matches the old-codebase format, and document
the confirmed path. If the schema is absent, flag it as a blocking gap before Slice 2 begins.
This is an exit criterion for Slice 1.

**Choose one:**

- **A.** Make `dev-session.yaml` schema location and confirmation an explicit Slice 1 exit
  criterion. If absent, flag as a blocking gap before Slice 2.  
  **Why pick this:** Ensures the checkpoint contract is verifiable before regression work starts.  
  **Why not:** May block Slice 2 if the schema needs to be created as part of this rewrite.

- **B.** Defer schema location to Slice 2 when the resume regression (E2-S4) is written;
  the test itself will surface any schema mismatch.  
  **Why pick this:** Avoids blocking Slice 2 start on a schema discovery task.  
  **Why not:** If the schema is incompatible, E2-S4 fails late and the fix is more expensive.

- **C.** Treat dev-session.yaml schema creation as in-scope for this rewrite if it is absent.
  The conductor owns the schema definition.  
  **Why pick this:** Avoids an external dependency on a schema that may not exist.  
  **Why not:** Schema creation is a behavioral change, not a preservation. It violates the
  backward-compat requirement.

- **D.** Write your own response.
- **E.** Accept the schema gap; the implementation agent will locate or create the schema
  at implementation time.

---

## Medium Findings

### M1 — Write isolation enforcement mechanism underspecified in tech-plan

**Location:** tech-plan.md, Write Isolation Contract section  
**Gate:** Before Slice 2 regression tests (E2-S2) can be verified

The tech-plan states that "all write operations are routed through
`bmad-lens-git-orchestration`, which is scoped to the target repo branch." This is a naming
of the mechanism but not a specification of the enforcement path. The conductor is an
AI-facing SKILL.md prompt, not compiled code. There is no explicit description of how the
conductor prevents a subagent in the task execution loop from writing directly to the control
repo path outside of the named skill.

If the enforcement relies on convention (the SKILL.md instructs subagents not to write to
the control repo) rather than a structural guard, the regression test for AC1 must explicitly
verify the negative case (subagent does not write to control repo path).

**Recorded response:** **A**  
**Applied adjustment:** The tech-plan write isolation section now names the enforcement
mechanism explicitly: the SKILL.md conductor instructs the subagent task executor to route
all file writes through `bmad-lens-git-orchestration` and confirms the control repo path as
read-only. The E2-S2 regression must include a negative test: a task that would write to
the control repo path is blocked or redirected correctly.

**Choose one:**

- **A.** Name the enforcement mechanism explicitly in the tech-plan and require a negative
  test in E2-S2 that confirms control-repo writes are blocked.  
  **Why pick this:** Makes the constraint testable and closes the verification gap.  
  **Why not:** Requires writing a negative-path test that may be harder to construct
  in an AI-facing test environment.

- **B.** Accept that the enforcement is convention-based (SKILL.md instruction) and rely
  on the positive-path AC1 test to catch any regressions.  
  **Why pick this:** Simpler; convention has held in the old-codebase.  
  **Why not:** Convention without a negative test leaves a blind spot in the regression suite.

- **C.** Add a separate story for write isolation hardening with a dedicated enforcement
  mechanism beyond SKILL.md convention.  
  **Why pick this:** Provides structural isolation guarantee.  
  **Why not:** Out of scope for a behavior-preservation rewrite; new enforcement is new scope.

- **D.** Write your own response.
- **E.** Accept the underspecification; the implementation agent will define the enforcement
  path at implementation time.

---

### M2 — E2-S1 discovery registration ordering risk not fully resolved in sprint plan

**Location:** sprint-plan.md, Slice 2, E2-S1  
**Gate:** Before E2-S2 through E2-S5 exit criteria can be verified

The sprint plan moves E2-S1 (discovery registration) to the first position in Slice 2 with a
note that it "may be a prerequisite for regression tooling to locate the command." However,
the sprint plan does not define the dependency between E2-S1 and the other Slice 2 stories.
If E2-S1 fails because the discovery surface does not yet exist, the sprint plan is silent on
whether E2-S2 through E2-S5 can proceed independently.

**Recorded response:** **A**  
**Applied adjustment:** The sprint plan now explicitly states that E2-S2 through E2-S5 can
proceed independently of E2-S1 if the regression tests do not require discovery to locate the
command. E2-S1 is a blocking item only if the regression infrastructure depends on discovery.
The implementation agent must confirm this at the start of Slice 2. If E2-S1 fails, the agent
flags it as a dependency gap rather than blocking all of Slice 2.

**Choose one:**

- **A.** Explicitly state that E2-S2 through E2-S5 can proceed independently of E2-S1 if
  regression tests do not require discovery. E2-S1 failure is a flagged gap, not a Slice 2 blocker.  
  **Why pick this:** Prevents a single discovery gap from blocking all behavioral regressions.  
  **Why not:** If discovery IS required by the regression infrastructure, this creates a
  false confidence that tests can run.

- **B.** Make E2-S1 a hard prerequisite for all other Slice 2 stories; block all regression
  work until discovery is confirmed.  
  **Why pick this:** Clean serial dependency; no ambiguity about ordering.  
  **Why not:** May block all regression work if the discovery surface creation is outside
  this feature's scope.

- **C.** Move E2-S1 to Slice 1 so the ordering risk is resolved before Slice 2 begins.  
  **Why pick this:** Closes the dependency before any Slice 2 work starts.  
  **Why not:** Discovery registration is behavioral work, not foundation validation; moving it
  to Slice 1 blurs the slice boundary.

- **D.** Write your own response.
- **E.** Accept the ordering ambiguity; the implementation agent will resolve the dependency
  at Slice 2 start.
