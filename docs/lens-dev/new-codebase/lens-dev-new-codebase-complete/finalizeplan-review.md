---
feature: lens-dev-new-codebase-complete
doc_type: finalizeplan-review
phase: finalizeplan
source: manual-rerun
verdict: pass-with-warnings
status: responses-recorded
critical_count: 0
high_count: 1
medium_count: 2
low_count: 1
carry_forward_blockers: []
updated_at: 2026-04-27T22:55:00Z
review_format: abc-choice-v1
---

# FinalizePlan Review - lens-dev-new-codebase-complete

**Source:** manual-rerun  
**Artifacts reviewed:** business-plan.md, tech-plan.md, sprint-plan.md, expressplan-adversarial-review.md  
**Goal under review:** Finalize the express planning bundle for the `/complete` retained command, confirm governance alignment, and produce a dev-ready implementation package.

---

## Governance Sensing

Cross-feature scan for the `lens-dev/new-codebase` service:

| Feature | Phase | Track | Impact |
|---|---|---|---|
| `lens-dev-new-codebase-baseline` | finalizeplan-complete | full | **Depends-on dependency.** Complete relies on baseline for lifecycle graph and feature record schemas. Baseline is at finalizeplan-complete and stable. No conflict. |
| `lens-dev-new-codebase-new-service` | complete | express | Informational. New-service reached the `complete` phase via express track — proving the archive path works for express features and providing a live behavioral reference. |
| `lens-dev-new-codebase-switch` | complete | express | Informational. Switch also completed via express track. Two prior archived express features confirm archive-state readers already recognize terminal express features. |
| `lens-dev-new-codebase-new-domain` | complete | full | Informational. Full-track archived feature. Terminal-state reader behavior confirmed for full-track too. |
| `lens-dev-new-codebase-new-feature` | finalizeplan-complete | full | Adjacent at finalizeplan-complete. No overlap with the closure domain. No conflict. |
| All other features | preplan | full | No planning-stage features have scope overlap with `complete` command behavior. |

**Sensing result:** No cross-feature sequencing conflicts. The `baseline` dependency is stable. Prior `complete`-phase features confirm that `archive-status`, `feature.yaml`, `feature-index.yaml`, and `summary.md` are already readable in the governance registry.

---

## H1 - Archive Summary Naming Ambiguity Carried From ExpressPlan Review

**Location:** tech-plan.md ADR 3, sprint-plan.md CP-7  
**Problem:** The expressplan adversarial review flagged that older discovery prose references `final-summary.md`, while the current implementation target is `summary.md`. The sprint plan includes CP-7 to audit this, but no story yet locks a blocking check that prevents the old name from silently surviving into implementation. If CP-7 is treated as cleanup rather than a gate, implementation could still start with undiscovered `final-summary` references in tests or help text.

- **A.** Mark CP-7 as a blocker in the implementation handoff notes: audit must pass before any story that writes archive output can be considered done.
- **B.** Accept `summary.md` as canonical and skip the audit because existing archived features already use it.
- **C.** Create a separate governance issue to track the legacy name.
- **D.** Provide a custom approach.
- **E.** Accept the ambiguity.

**Selected:** A  
**Why:** The archive summary is a durable external contract. The audit is low-cost and prevents divergence between implementation stories and any readers that still reference the old name. CP-7 must be a gate, not cleanup.

---

## M1 - Confirmation Gate Verification Not Bound to a Test Artifact

**Location:** sprint-plan.md CP-10, tech-plan.md orchestration section  
**Problem:** The sprint plan identifies CP-10 as the story that proves explicit confirmation is required before finalize runs. The expressplan review noted this gap and recommended a narrow wrapper-level regression or scripted walkthrough check. Neither the tech plan nor the sprint plan names the concrete file, harness, or CLI command that would provide that verification. Leaving it implicit means CP-10's done condition is subjective.

- **A.** Add a concrete done-condition to CP-10 in the implementation handoff: name the test file and the assertion type that proves confirmation is enforced.
- **B.** Leave CP-10 as described and let the implementor define done criteria at story creation time.
- **C.** Merge CP-10 into CP-5 (atomic archive writes) since confirmation and finalize are in the same execution path.
- **D.** Provide a custom verification approach.
- **E.** Accept the ambiguity.

**Selected:** A  
**Why:** The confirmation gate is a trust boundary for an irreversible command. An ambiguous done condition on CP-10 risks shipping a `/complete` that has untested confirmation flow. Naming the test artifact here eliminates that risk.

---

## M2 - Branch Cleanup Ownership Not Resolved in Planning Artifacts

**Location:** expressplan-adversarial-review.md H2, tech-plan.md ADR 2 scope section  
**Problem:** The expressplan adversarial review flagged that discovery prose is ambiguous about whether `/complete` orchestration includes branch cleanup or whether that is documented as adjacent operational follow-up. Neither the tech plan nor the sprint plan explicitly settles this. If an implementation story interprets branch cleanup as in-scope, it could extend the archive mutation boundary beyond what the script contract covers.

- **A.** Resolve in the implementation handoff notes: explicitly state that branch cleanup is NOT part of the `/complete` script contract and is documented only as post-archive operational follow-up.
- **B.** Include branch cleanup as an optional step in CP-5 so it is in scope but skippable.
- **C.** Defer the decision to implementation and allow the dev to choose.
- **D.** Provide a custom scope note.
- **E.** Accept the ambiguity.

**Selected:** A  
**Why:** The tech plan's split-responsibility model (script owns archive mutations, skill orchestrates) is clear. Branch cleanup is not an archive mutation. Encoding this explicitly in handoff notes prevents scope creep in CP-5 and keeps the script boundary clean.

---

## L1 - Live PR Orchestration Not Exercised in This Planning Session

**Location:** FinalizePlan step-2 and step-3 expectations  
**Problem:** This planning session produces staged docs only. The plan PR and final PR steps run through the orchestration CLI, but the two-branch topology (base branch, plan branch, merge readiness) has not been exercised with actual artifact content in place.

- **A.** Accept this as normal for staged-docs finalizeplan. Note in handoff that live PR topology is implementation-verified through the orchestration CLI contract tests, not through a manual planning session replay.
- **B.** Block progression until the plan-PR and final-PR are manually exercised before opening the implementation PR.
- **C.** Remove PR topology from the complete feature scope.
- **D.** Provide a custom topology verification approach.
- **E.** Accept the ambiguity.

**Selected:** A  
**Why:** The plan PR and final PR flow through a well-exercised CLI (`git-orchestration-ops.py`). Treating staged-docs sessions as the PR topology proof would misrepresent what was done. The handoff note is sufficient.

---

## Blind-Spot Challenge

**Winston (Architect):** The split between skill orchestration and script mutation is correct in the tech plan, but no story explicitly validates that the skill respects the script's precondition gate output. What happens when `check-preconditions` returns `fail` — does the skill stop, or does it continue and prompt anyway?

**Murat (Test Architect):** If `finalize` is atomic and irreversible, the test harness must be able to snapshot or restore state between test runs. How does the test design prevent real governance state from being mutated in CI?

**John (Product Manager):** From the user's perspective, the sequence is: confirm → retrospective → document → archive. What does the command output look like if the user skips the retrospective explicitly? Is it clear from the archive that the retrospective was skipped, not lost?

### Direct Questions

1. Does `check-preconditions` returning `warn` still allow the user to continue, and what confirmation gate language distinguishes a warn-continue from a pass?
2. Is there an integration test that runs the full `check-preconditions → finalize → archive-status` path with a simulated governance fixture, or only unit-level tests per subcommand?
3. Should the handoff notes for this feature reference the `new-service` and `switch` archived express features as behavioral reference examples for the archive output format?
