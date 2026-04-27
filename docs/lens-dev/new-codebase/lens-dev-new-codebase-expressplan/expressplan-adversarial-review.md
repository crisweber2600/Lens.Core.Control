---
feature: lens-dev-new-codebase-expressplan
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
updated_at: 2026-04-27T22:50:00Z
review_format: abc-choice-v1
---

# ExpressPlan Adversarial Review - lens-dev-new-codebase-expressplan

**Source:** phase-complete  
**Artifacts reviewed:** business-plan.md, tech-plan.md, sprint-plan.md  
**Cross-feature context:** lens-dev-new-codebase-baseline, lens-dev-new-codebase-complete  
**Format:** Each finding includes A/B/C options, D for a custom response, and E to keep as-is.

---

## How Responses Were Recorded

This staged review records the selected resolution for each finding so the feature can move into FinalizePlan planning. The selected option is shown explicitly under each finding.

| Option | Meaning |
|---|---|
| A / B / C | Accept the proposed resolution and its trade-offs |
| D | Supply a custom resolution |
| E | Accept the risk with no action |

---

## H1 - Review Artifact Naming Drift

**Location:** lifecycle metadata, older discovery notes, and prior governance docs  
**Problem:** Current lifecycle metadata expects `expressplan-adversarial-review.md`, while some retained notes still mention `expressplan-review.md`. If the implementation tries to satisfy both informally, validators and future maintainers will drift.

- **A.** Standardize the implementation and tests on `expressplan-adversarial-review.md`, and document `expressplan-review.md` as older compatibility debt.
- **B.** Teach validators to accept both filenames indefinitely.
- **C.** Rename the lifecycle contract back to `expressplan-review.md` and update every newer reference.
- **D.** Provide a custom naming rule.
- **E.** Keep the mismatch unresolved.

**Selected:** A  
**Why:** The current lifecycle contract is the executable source of truth. Carrying two filenames forward permanently invites silent breakage.

## H2 - Express-Track Gating Could Be Softened Illegally

**Location:** expressplan skill activation and feature-state validation  
**Problem:** A convenience implementation may silently rewrite or accept full-track features to make demos smoother. That would invalidate the lifecycle model and hide a governance decision inside a prompt flow.

- **A.** Allow full-track features but log a warning.
- **B.** Block unsupported track usage and require an explicit conversion or different planning path outside expressplan.
- **C.** Auto-convert the feature to express the first time the command runs.
- **D.** Provide a custom gating strategy.
- **E.** Leave the behavior unspecified.

**Selected:** B  
**Why:** Track changes are governance state, not prompt convenience. Silent conversion would make later phase reasoning unreliable.

## M1 - QuickPlan Retention Is Easy To Lose

**Location:** wrapper delegation and skill registry  
**Problem:** QuickPlan is internally retained, but it is no longer a top-level published command. That makes it easy for maintainers to remove or “simplify away,” which would break expressplan without an obvious compile failure.

- **A.** Keep QuickPlan as an internal dependency and cover its presence with an expressplan-focused regression.
- **B.** Inline QuickPlan behavior directly into expressplan.
- **C.** Re-publish QuickPlan as a user-visible command.
- **D.** Provide a custom retention strategy.
- **E.** Accept the deletion risk.

**Selected:** A  
**Why:** Internal retention preserves the published command surface while still protecting the dependency explicitly.

## M2 - FinalizePlan Reuse Could Turn Into Copy-Paste Bundle Logic

**Location:** expressplan step-3 handoff boundary  
**Problem:** The easiest implementation path is to duplicate downstream bundle logic in expressplan. That would create two planning bundles that must stay in sync.

- **A.** Route expressplan into FinalizePlan reuse and add a regression that proves the handoff.
- **B.** Allow a second bundle implementation optimized for express mode.
- **C.** Skip story/readiness generation from the express path entirely.
- **D.** Provide a custom bundle strategy.
- **E.** Leave the handoff ambiguous.

**Selected:** A  
**Why:** FinalizePlan already owns the complex downstream bundle and PR topology. Reuse is cheaper and safer than duplication.

---

## Blind-Spot Challenge

**Murat (Test Architect):** What breaks first if `expressplan` writes the right files but forgets to block on a `fail` review verdict?

**Winston (Architect):** Where is the one place future maintainers are most likely to accidentally duplicate FinalizePlan logic instead of reusing it?

**John (PM):** If an operator starts expressplan on the wrong feature type, what message will convince them to take the correct path instead of treating the command as broken?

### Direct Questions

1. Do we need an explicit compatibility note for older `expressplan-review.md` references in help or docs?
2. Should unsupported-track failures point the user to `/businessplan` or to a governance conversion path first?
3. Which focused regression is the minimum acceptable proof that FinalizePlan reuse is real and not only described in prose?