---
feature: lens-dev-new-codebase-baseline
doc_type: adversarial-review
phase: preplan
source: phase-complete
verdict: pass-with-warnings
status: responses-recorded
critical_count: 2
high_count: 3
medium_count: 5
low_count: 2
carry_forward_blockers:
  - C1: schema version ambiguity must be closed before TechPlan begins
  - C2: help stub deprecation gap must be resolved before BusinessPlan closes
updated_at: 2026-04-22T00:00:00Z
review_format: abc-choice-v1
---

# PrePlan Adversarial Review — lens-dev-new-codebase-baseline

**Source:** phase-complete  
**Artifacts reviewed:** brainstorm.md, research.md, product-brief.md  
**Constitution:** lens-dev (informational), new-codebase (informational)  
**Cross-feature context:** lens-dev-old-codebase-discovery (expressplan), lens-dev-release-discovery (preplan)  
**Format:** Each finding presents options A/B/C (proposed solutions), D (write own), E (keep as-is). Respond by letter.

---

## How to Respond

For each finding below, reply with the letter of your chosen option. You may add a clarifying note after the letter. Your response is recorded and drives the carry-forward action for that finding.

| Option | Meaning |
|---|---|
| A / B / C | Accept the proposed resolution (with its stated trade-offs) |
| D | You will provide a custom resolution — write it after "D:" |
| E | Explicitly no action — this finding is accepted as-is |

---

## Verdict: `pass-with-warnings`

The phase can advance to BusinessPlan. No finding is a hard stop today. C1 and C2 are carry-forward blockers with explicit resolution gates. All other findings are resolved by your option selections below.

---

## Finding Summary

| ID | Severity | Title | Your Response |
|---|---|---|---|
| C1 | Critical | G2 contradicts open schema version question | **A** |
| C2 | Critical | `help` deprecated; module greeting still references `/help` | **C** |
| H1 | High | Mid-phase active features have undefined transition behavior | **A** |
| H2 | High | `dev-session.yaml` format open but G2 is closed | **A** |
| H3 | High | `pause-resume` classified as fully removable without audit | **A** |
| M1 | Medium | expressplan batch mode structurally different; utility under-specified | **D** |
| M2 | Medium | No upgrade regression test in success criteria | **A** |
| M3 | Medium | `approval-status` deprecated; finalizeplan step 2 unverified | **A** |
| M4 | Medium | BMAD wrapper equivalence not in success criteria | **A** |
| M5 | Medium | No deprecation notice plan for users with stub references | **B** |
| L1 | Low | `discover` auto-commit behavior absent from risk table | **A** |
| L2 | Low | `new-project` removal leaves bootstrapping gap | **A** |

---

## Critical Findings

---

### C1 — G2 contradicts the open schema version question

**Location:** product-brief.md §3 (G2, G5)  
**Gate:** TechPlan entry item 1

G2 states: "Zero migration required for users on the current codebase who upgrade."  
G5 states: "If v5.0, the `upgrade` command must include a v4→v5 migration."

These are mutually exclusive. If schema_version is bumped to v5.0, G2 is false — users *do* need to run `/upgrade`. Research open question 1 has been answered (drop-in replacement), but the contradiction in the product-brief has not been resolved in writing.

**Choose one:**

- **A.** Commit to v4.0 drop-in explicitly — remove the "If v5.0" clause from G5, update product-brief to state schema_version: 4 is the fixed target.  
  **Why pick this:** Closes the contradiction cleanly; aligns with the already-stated Q1 answer; G2 stands as written. Zero additional user burden.  
  **Why not:** If any field in feature.yaml or dev-session.yaml needs to change during the rewrite, this option over-commits and creates a future retraction.

- **B.** Commit to v5.0 with automated migration — reword G2 to: "Migration is automated by `/upgrade` with no data loss; users run `/upgrade` once."  
  **Why pick this:** Correct if the rewrite changes any schema field; makes upgrade a meaningful command with real work to do.  
  **Why not:** Contradicts the Q1 answer already given; adds a migration implementation obligation; forces active users to take an extra step.

- **C.** Make G2 explicitly conditional — reword to: "If v4.0 drop-in: zero migration. If v5.0: `/upgrade` automates migration with no data loss. Decision confirmed at TechPlan entry."  
  **Why pick this:** Honest about what's open; removes the contradiction without forcing a premature decision.  
  **Why not:** Defers rather than closes; TechPlan still must close it as item 1; adds product-brief wordiness for a question that already has a stated answer.

- **D.** Write your own response.
- **E.** Keep as-is — accept the contradiction in the product-brief as a known open item.

---

### C2 — `help` stub deprecated; module greeting still references `/help`

**Location:** brainstorm.md (deprecated list), module.yaml (module_greeting)  
**Gate:** Resolved before BusinessPlan closes

`lens-help.prompt.md` is in the "can be fully removed" column. `module.yaml` module_greeting instructs new users: *"Type `/help` to see available commands."* If the stub is removed and the greeting is unchanged, day-one discoverability is broken.

**Choose one:**

- **A.** Retain `lens-help.prompt.md` as a 17th published stub — update the surface count goal in product-brief G1 from 16 to 17.  
  **Why pick this:** Zero discoverability regression; `/help` is a universal shell convention that new users expect; no greeting change required.  
  **Why not:** Contradicts the explicit G1 goal of 16 commands; sets a precedent for other "just one more" exceptions; the 16-command surface was a deliberate scope decision.

- **B.** Redesignate `/next` as the discovery entry point — rewrite module_greeting to: *"Type `/next` to get started."*  
  **Why pick this:** `/next` is already the opinionated guide; making it the cold-start entry point unifies discovery and action into one command; no extra stub needed.  
  **Why not:** `/next` may require an active feature to be selected before it provides useful guidance; a user with no feature context may get a confusing response.

- **C.** Remove the `/help` reference from module_greeting entirely — rely on README and documentation for command discovery.  
  **Why pick this:** Simplest change; avoids coupling in-session greeting to any specific command name.  
  **Why not:** Leaves new users with no in-session discovery path; module_greeting becomes less useful, not more.

- **D.** Write your own response.
- **E.** Keep as-is — accept that the module greeting will reference a removed command until separately fixed.

---

## High Findings

---

### H1 — Mid-phase active features have undefined transition behavior during rewrite publication

**Location:** product-brief.md §8 (stakeholder constraints)  
**Context:** lens-dev-old-codebase-discovery is in expressplan; lens-dev-release-discovery is in preplan at time of writing.

The product-brief says "all existing features must remain fully operational" but does not define what a user experiences when they run `/expressplan` or `/preplan` the day after the new codebase is published while mid-phase.

**Choose one:**

- **A.** Stateless re-entry model — document that all phase sessions resume from the phase entry point; no intra-phase session state is required to continue. Add this to stakeholder constraints.  
  **Why pick this:** Most skills are already designed stateless (no intra-phase checkpoint files); this is consistent with current behavior and requires zero implementation cost.  
  **Why not:** If any skill does persist intra-phase state (batch-input.md, partial artifact staging), resuming from entry would re-run already-completed steps and potentially overwrite staged artifacts.

- **B.** Gate publication on all in-flight features reaching a phase boundary — add to release checklist: "Confirm lens-dev-old-codebase-discovery and lens-dev-release-discovery are not mid-phase before publishing."  
  **Why pick this:** Absolute certainty; no edge case; appropriate for a module claiming 100% backwards compatibility during an active workstream.  
  **Why not:** Could delay the rewrite indefinitely if active features take weeks to reach a phase boundary; operationally impractical if other teams are blocked on the rewrite.

- **C.** Add a compatibility notice to release notes only — document that users mid-phase should run their current phase command to completion before upgrading.  
  **Why pick this:** Practical trade-off; most users are not mid-phase session exactly when a version is published; minimal implementation cost.  
  **Why not:** "Notify and hope" is inconsistent with G2's "zero migration" promise; a mid-phase user who upgrades without reading release notes will experience a silent regression.

- **D.** Write your own response.
- **E.** Keep as-is — accept that mid-phase transition behavior is undefined; address it reactively if reported.

---

### H2 — `dev-session.yaml` checkpoint format open but G2 makes a closed promise

**Location:** product-brief.md §3 (G2), research.md §9 (Q3 — now answered: backwards compatible)  
**Note:** Research Q3 has been answered: dev-session.yaml checkpoint format is backwards compatible.

G2 promises "in-progress dev sessions must be fully operational after the rewrite." This is now resolved by Q3's answer — but the research open question answer is not yet reflected in product-brief or in a frozen contract entry.

**Choose one:**

- **A.** Close Q3 formally — add `dev-session.yaml checkpoint format` to the frozen contracts table in product-brief.md §3. Mark Q3 as resolved in research.md.  
  **Why pick this:** Converts the verbal Q3 answer into a binding written contract; makes the guarantee testable; resolves the H2 conflict.  
  **Why not:** If the Q3 answer was given without checking the actual checkpoint schema fields, this creates a false guarantee.

- **B.** Qualify G2 to be precise — reword to: "Dev sessions on the v4.0 schema resume without migration. Checkpoint format is forwards-compatible by design."  
  **Why pick this:** Accurate and bounded; states the guarantee covers the same schema version, not all future versions.  
  **Why not:** The qualification may read as weakening a promise that Q3 says is already fully met; adds hedging where none is needed.

- **C.** Add a checkpoint compatibility test to success criteria — create a fixture dev-session.yaml from the old codebase and verify the new codebase can resume from it.  
  **Why pick this:** Transforms the verbal guarantee into a testable criterion; confidence is proportional to the test passing.  
  **Why not:** Building the test fixture requires knowing the old checkpoint format in detail; may be medium effort for a finding that's already verbally resolved.

- **D.** Write your own response.
- **E.** Keep as-is — accept that the Q3 answer exists in research comments and does not need to be formalized further.

---

### H3 — `pause-resume` classified as "can be fully removed" without confirming dev crash-recovery coverage

**Location:** brainstorm.md (deprecated list — "can be fully removed")

`lens-pause-resume.prompt.md` is in the "fully removable" column. The dev SKILL.md describes `dev-session.yaml` as supporting crash recovery. If pause/resume semantics are currently routed through this stub and the dev skill does not independently cover them, removal causes a silent regression in crash-recovery behavior.

**Choose one:**

- **A.** Audit `bmad-lens-dev` SKILL.md now — verify pause/resume semantics are internally covered. If confirmed → classification stands. Document the audit result in brainstorm.md.  
  **Why pick this:** Lowest cost; if the SKILL.md already covers it (likely), the classification is validated in one read.  
  **Why not:** "Audit now" is easy to defer; without a tracking mechanism, the audit may never happen before dev begins.

- **B.** Reclassify as "stub removed, skill retained internally" pending audit — move to the middle column until `bmad-lens-dev` audit is complete.  
  **Why pick this:** Cautious; an extra internal skill with no consumer costs nothing; a missing crash-recovery handler costs a production regression.  
  **Why not:** Over-classifying skills as "retained" inflates the internal module inventory and creates dead code if the audit confirms full removal was safe.

- **C.** Remove immediately but add a regression test — delete the stub, add a success criterion: "dev crash-recovery test passes (dev session survives simulated mid-story failure)."  
  **Why pick this:** Evidence-based removal; the test becomes the verification; passes confidence to future maintainers.  
  **Why not:** Writing a crash-recovery test may be the highest-effort option here; the finding doesn't warrant it if option A resolves it cheaply.

- **D.** Write your own response.
- **E.** Keep as-is — accept the classification and defer crash-recovery audit to dev phase implementation review.

---

## Medium Findings

---

### M1 — expressplan batch mode is structurally different; `lens-batch-contract` utility is under-specified

**Location:** product-brief.md §7.3 (G3), research.md §9 (Q4 — answered: reevaluate batch to bring functionality forward)  
**Note:** Research Q4 has been answered: reevaluate the batch command to bring its functionality forward.

expressplan's batch mode wraps QuickPlan + FinalizePlan as a unit; the other four phases' batch mode wraps individual artifact authoring rounds. A single `lens-batch-contract` utility that covers both risks being lowest-common-denominator.

**Choose one:**

- **A.** Exclude expressplan from `lens-batch-contract` — expressplan carries its own batch pattern. The shared utility covers preplan, businessplan, techplan, and finalizeplan only.  
  **Why pick this:** Clean separation; no forced generalization; aligns with Q4 directive to reevaluate batch independently.  
  **Why not:** Two separate batch patterns are two things to maintain; if they converge naturally during TechPlan, the split was unnecessary.

- **B.** Split into two distinct utilities: `lens-phase-batch-contract` (4 planning phases) + `lens-express-batch-contract` (expressplan).  
  **Why pick this:** Both patterns are worth normalizing; two small clean utilities are better than one over-generalized one; each is independently testable.  
  **Why not:** Adds naming overhead for what may be a minor distinction; expressplan's batch pattern may not justify its own named utility.

- **C.** Unify under `lens-batch-contract` with an `expressplan_mode` flag — the flag triggers the QuickPlan+FinalizePlan bundle variant.  
  **Why pick this:** Single utility name; the reevaluation (Q4) may produce a unified model where the flag-driven path is the cleanest implementation.  
  **Why not:** A flag-driven utility is harder to understand for maintainers who only touch one path; flag branching in batch contracts is a common source of subtle behavioral differences.

- **D.** Write your own response.
- **E.** Keep as-is — accept that batch mode unification scope is under-specified and resolve during TechPlan architecture.

---

### M2 — No regression test for `upgrade` command in success criteria

**Location:** product-brief.md §5 (success criteria)

The success criteria table tests featureId, git-orchestration, next-action, and preflight. `upgrade` has no test. The risk register rates "lifecycle.yaml schema_version incremented without upgrade path" as Medium. An upgrade command that silently skips a migration is worse than one that fails loudly.

**Choose one:**

- **A.** Add `test-upgrade-ops.py` to success criteria — upgrade dry-run on a v4 feature reports no-op (correct behavior for a v4.0 drop-in target).  
  **Why pick this:** `bmad-lens-migrate` already has a tested dry-run/apply/verify pattern; the test is low-cost to write. Confirms the upgrade no-op is intentional.  
  **Why not:** Testing that a command does nothing is low value; if the rewrite is v4.0 drop-in (confirmed), upgrade correctness is trivially true.

- **B.** Add integration success criterion: "upgrade applied against a synthetic feature.yaml confirms zero schema mutation."  
  **Why pick this:** Functional confidence that upgrade does not corrupt existing features; higher signal than a dry-run no-op.  
  **Why not:** Building a synthetic feature.yaml fixture for an integration test is medium effort; may be over-engineered for a no-op command.

- **C.** No dedicated test — rely on preflight coverage and manual verification at release time.  
  **Why pick this:** upgrade is only meaningful when schema changes; testing a no-op costs more than it validates; manual check at release is sufficient.  
  **Why not:** An untested upgrade path that silently skips migrations is a production risk regardless of how unlikely the failure scenario seems.

- **D.** Write your own response.
- **E.** Keep as-is — accept that upgrade has no regression test in the success criteria table.

---

### M3 — `approval-status` deprecated; finalizeplan step 2 PR readiness check unverified

**Location:** brainstorm.md (deprecated — "can be fully removed"), finalizeplan SKILL.md step 2

Finalizeplan step 2 is "Confirm readiness to merge." If `lens-approval-status.prompt.md` was surfacing PR approval state as part of this step, its removal must be confirmed as covered inside `bmad-lens-finalizeplan`. This has not been audited.

**Choose one:**

- **A.** Audit `bmad-lens-finalizeplan` SKILL.md now — verify PR readiness checking (approval, CI state) is internally handled in step 2. If confirmed → classification stands. Document result in brainstorm.md.  
  **Why pick this:** Quick read; if the SKILL.md already handles it (likely), the audit takes minutes and closes the finding.  
  **Why not:** Same deferral risk as H3-A; easy to leave undone until it becomes a dev surprise.

- **B.** Retain `lens-approval-status.prompt.md` as a thin wrapper for one release cycle — move to a "deprecated but present" state, not "fully removed."  
  **Why pick this:** Zero risk of silently degrading a phase gate; the wrapper costs one extra stub file.  
  **Why not:** Undermines G1 surface reduction; one retained stub becomes two becomes five; establishes a "can't remove anything" precedent.

- **C.** Remove immediately and treat PR readiness as a user responsibility outside Lens — Lens does not own GitHub approval state.  
  **Why pick this:** Reduces scope; GitHub PR approval is a GitHub-native action; forcing it into a Lens command is over-coupling.  
  **Why not:** If finalizeplan currently gates on approval state as a phase integrity check, removing the visibility tool without the gate creates a user experience gap in the most critical pre-merge step.

- **D.** Write your own response.
- **E.** Keep as-is — accept that approval-status deprecation is unverified against finalizeplan step 2.

---

### M4 — BMAD wrapper equivalence not in success criteria

**Location:** product-brief.md §5, research.md §9 (Q2 — answered: reimplemented with known gaps; /preplan → brainstorming behavior significantly different)

Q2 has been answered: wrappers are being reimplemented and there are known behavioral gaps (e.g., `/preplan` → `bmad-brainstorming` diverges significantly from the old delegation). No success criterion currently tests delegation behavior.

**Choose one:**

- **A.** Add delegation equivalence test per phase skill to success criteria — each reimplemented wrapper must have at least one integration test confirming its delegation calls the intended BMAD skill with the correct context.  
  **Why pick this:** Known gaps exist (Q2 confirmed); untested wrappers with known behavioral divergence are a production regression waiting to land.  
  **Why not:** "At least one test per phase skill" is broad; some wrappers may have trivially obvious behavior that doesn't justify test infrastructure.

- **B.** Add success criterion for the highest-risk divergent wrappers only — start with preplan → brainstorming (confirmed divergent), then expand during TechPlan as audit uncovers others.  
  **Why pick this:** Focused effort on confirmed problems first; avoids writing tests for wrappers that may have no divergence issues.  
  **Why not:** Leaves unknown divergences undetected until dev; gaps in low-priority wrappers can still cause production issues.

- **C.** Document intended delegation behavior for each wrapper in TechPlan architecture — test coverage is a dev-phase responsibility, not a product-brief success criterion.  
  **Why pick this:** Architecture documentation is the right product-phase artifact; implementation testing belongs in the dev phase.  
  **Why not:** Documentation without tests means regressions land in production before anyone catches them; given known divergence, deferring tests to dev is high risk.

- **D.** Write your own response.
- **E.** Keep as-is — accept that wrapper equivalence is not a success criterion; address divergence as a dev-phase implementation concern.

---

### M5 — No deprecation notice plan for users with existing stub references

**Location:** product-brief.md §3 (G1, G2)

Users may have shell aliases, team READMEs, or internal documentation referencing deprecated stubs (e.g., `/lens-onboard`, `/lens-dashboard`, `/lens-init-feature`). The rewrite removes these with no stated communication strategy, despite G2's backwards-compatibility claim.

**Choose one:**

- **A.** Add one-release-cycle deprecation message — deprecated stubs surface a message ("This command is deprecated. Use `/next` for guidance.") for one release before full removal.  
  **Why pick this:** Smooth user experience; teams with existing references are warned before breakage; standard deprecation pattern.  
  **Why not:** The first rewrite publication may already be the "one release cycle" warning; this requires keeping the stubs alive longer, partially defeating G1.

- **B.** Add explicit non-goal: "No in-stub deprecation notices; deprecated commands are documented in release notes only."  
  **Why pick this:** Clean scope boundary; avoids implementation overhead; makes the trade-off explicit rather than implicit.  
  **Why not:** Users who don't read release notes will experience the removal as a sudden undocumented regression; conflicts with G2's spirit if not its letter.

- **C.** Auto-redirect deprecated stubs to their canonical replacement — e.g., `/lens-init-feature` forwards to `/new-feature` with a note.  
  **Why pick this:** Best user experience; existing workflows continue working; discovery of new canonical command is built-in.  
  **Why not:** Redirect stubs are essentially the same implementation cost as deprecation stubs; may prolong the cleanup indefinitely if no forced sunset date is set.

- **D.** Write your own response.
- **E.** Keep as-is — no deprecation notice plan; accept that removed stubs break without warning.

---

## Low Findings

---

### L1 — `discover` auto-commit to governance main is absent from risk table and goals

**Location:** research.md §6 (discover), product-brief.md §9 (risks)

`discover` is the only command that auto-commits to governance `main` outside lifecycle phase transitions. The research notes this as architecturally unusual. Neither the product-brief risk table nor the goals confirm whether this behavior is preserved, changed, or excluded from the publish-entry-hook consolidation.

**Choose one:**

- **A.** Add explicit note to product-brief confirming auto-commit behavior is preserved as-is — one sentence in §7 or stakeholder constraints.  
  **Why pick this:** Lowest cost; closes the gap with a sentence; makes the intent visible in the product-level document.  
  **Why not:** If discover's auto-commit behavior is actually under review as part of the rewrite, this option prematurely commits to preserving it.

- **B.** Add discover to the risk table with a "preserved unchanged, no architecture decision required" entry.  
  **Why pick this:** Risk table completeness; readers of the risk table will see that discover's unique behavior was explicitly evaluated, not overlooked.  
  **Why not:** A "no action" risk table entry adds noise; risk tables should contain actionable risks, not resolved ones.

- **C.** Move to a TechPlan architecture decision record (ADR) — the auto-commit behavior is a governance write-path policy decision that belongs in the architecture document.  
  **Why pick this:** ADRs are the right home for commit-behavior decisions; keeps product-brief focused on user value, not implementation policy.  
  **Why not:** This is a minor behavioral preservation note, not an architectural decision; an ADR is over-engineered for a one-sentence preservation confirmation.

- **D.** Write your own response.
- **E.** Keep as-is — accept that discover auto-commit behavior is not explicitly addressed in product-level documents.

---

### L2 — `new-project` removal leaves a gap for net-new project bootstrapping

**Location:** brainstorm.md (open question), product-brief.md §4 (non-goals)

The brainstorm open question "Should new-domain serve as the entry point for brand-new project bootstrapping?" is not closed. Non-goals say "No new commands" but don't answer the question. A user starting from scratch on a net-new project currently has no stated entry point in the 16-command surface.

**Choose one:**

- **A.** Add one sentence to product-brief: "new-domain is the replacement entry point for net-new project bootstrapping; its SKILL.md will include the bootstrapping guidance previously in new-project."  
  **Why pick this:** Closes the open question immediately; gives TechPlan a clear implementation target; no new command required.  
  **Why not:** Expands `new-domain`'s scope beyond its current definition ("create a new domain in governance"); may require more SKILL.md rewriting than anticipated.

- **B.** Add bootstrapping coverage as an explicit acceptance criterion for `new-domain` — "new-domain SKILL.md includes full net-new workspace setup narrative."  
  **Why pick this:** Makes the bootstrapping requirement testable and traceable, not just stated; harder to accidentally omit during implementation.  
  **Why not:** Acceptance criteria are already long; a sentence in the brief may be sufficient without promoting this to a tracked criterion.

- **C.** Accept the gap — document `new-domain` as "existing domain entry point only"; leave bootstrapping to external documentation (repo README, onboarding guide).  
  **Why pick this:** Honest about scope; avoids scope creep on new-domain; external documentation is a valid bootstrapping channel.  
  **Why not:** Leaves new users who try to start from scratch with no in-product guidance; the whole point of `preflight → new-domain → new-service → new-feature` progression is that it should be self-contained.

- **D.** Write your own response.
- **E.** Keep as-is — accept that new-project's bootstrapping narrative has no explicit successor; address when a new user raises the gap.

---

## Party-Mode Blind Spot Findings

*Three planning perspectives, one challenge round. These are rhetorical questions — no letter selection required. Address them in BusinessPlan scope if relevant.*

**Mary (Analyst):** Did you actually interview users about which commands they invoke regularly? The "54 stubs are cosmetic wrappers" finding describes what stubs *do*, not what users *use*. Teams may have built daily rituals around `/lens-dashboard`, `/lens-approval-status`, or `/lens-git-state`. Removing those causes zero internal regression but could disrupt real workflows. The product-brief has no user research backing the "rarely used" claim.

**Bob (Scrum Master):** The success criteria defines regression coverage as "all features in feature-index.yaml remain operational" — but there are only 3 features in the index. The rewrite touches 16 skills and introduces 3 shared utilities. The 4 existing test scripts were written before the rewrite was scoped. What is the plan for expanding test coverage *before* implementation begins — so tests are validators, not retrofitted post-hoc checks?

**Winston (Architect):** Extracting shared utilities from 4 copy-pasted implementations is a refactor, not just a rewrite. A single wrong implementation in `lens-phase-gate` fails all 3 planning phases simultaneously. "Single implementations" that are wrong fail everywhere at once. What is the formal verification strategy for shared utility extraction correctness before any phase skill switches to using it?

---

## Carry-Forward Blockers

| ID | Finding | Required By | Status |
|---|---|---|---|
| C1 | Close v4.0 vs v5.0 schema version contradiction | TechPlan entry item 1 | **A selected** — product-brief G5 + §7.2 updated; schema_version: 4 committed |
| C2 | Remove `/help` from module_greeting; stub removed | BusinessPlan close | **C selected** — greeting update required before BusinessPlan closes |
| H2 | Formalize dev-session.yaml as frozen contract | TechPlan | **A selected** — added to product-brief G2 frozen contracts; Q3 closed in research.md |
| H3 | Audit pause-resume coverage inside bmad-lens-dev | BusinessPlan close | **A selected** — audit bmad-lens-dev SKILL.md before BusinessPlan closes |
| M2 | Add upgrade regression test to success criteria | BusinessPlan close | **A selected** — test-upgrade-ops.py row added to success criteria table |

