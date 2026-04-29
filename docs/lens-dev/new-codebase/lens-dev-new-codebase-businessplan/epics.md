---
feature: lens-dev-new-codebase-businessplan
doc_type: epics
status: approved
goal: "Rewrite the businessplan command as a thin conductor delegating shared patterns to canonical utilities."
key_decisions:
  - Single epic covers all implementation work for this feature (express track, focused scope)
  - Techplan rewrite deferred to lens-dev-new-codebase-techplan (separate feature, canonical scope owner)
  - Stories ordered by dependency chain: BP-1 → BP-3 → BP-4
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline (story 1.4, 3.1, 4.1 must be merged in lens.core.src/develop)
blocks:
  - lens-dev-new-codebase-baseline (story 4.4 — finalizeplan rewrite)
  - lens-dev-new-codebase-baseline (story 4.5 — expressplan rewrite)
updated_at: 2026-04-28T02:00:00Z
---

# Epics — Rewrite businessplan Command

**Feature:** lens-dev-new-codebase-businessplan  
**Track:** express  
**Author:** crisweber2600  
**Date:** 2026-04-28  

---

## Epic List

---

### EP-1 — Businessplan Conductor Rewrite

**Goal:** Rewrite `bmad-lens-businessplan` SKILL.md and `lens-businessplan.prompt.md` as a thin conductor that delegates all shared patterns (batch, review-ready, publish-before-author) to canonical shared implementations. Validate correctness via regression gate. Close out governance phase transition.

**Scope:**
- Rewrite the businessplan skill and prompt (thin conductor pattern)
- Regression gate covering wrapper-equivalence and governance-audit categories
- Feature closeout: artifacts committed, governance phase advanced to `finalizeplan-complete`

**Stories in this epic:**

| Story ID | Title | Points | Priority |
|----------|-------|--------|----------|
| BP-1 | Rewrite businessplan command as thin conductor | 5 | P0 |
| BP-3 | Regression gate and discovery surface verification | 2 | P0 |
| BP-4 | Feature closeout and governance phase advance | 1 | P0 |

**Total estimated effort:** 8 story points

**Entry condition:** Pre-sprint checklist complete — stories 1.4, 3.1, and 4.1 merged to `develop` in `lens.core.src` (confirmed via preflight, not just feature-index phase status).

**Exit condition:** BP-3 regression gate green and BP-4 governance closeout complete. Unblocks lens-dev-new-codebase-baseline stories 4.4 and 4.5.

**Dependencies:**
- `lens-dev-new-codebase-baseline` story 1.4 — `publish-to-governance` shared CLI entry hook
- `lens-dev-new-codebase-baseline` story 3.1 — constitution partial hierarchy fix
- `lens-dev-new-codebase-baseline` story 4.1 — preplan rewrite (staged artifacts consumed by businessplan publish step)

**Architecture reference:** [tech-plan.md](tech-plan.md) §3–§5

---

## Summary

This feature contains a single focused epic. The express track was chosen because the planning scope (one command rewrite with established architecture contract from the baseline feature) does not require the full preplan/businessplan/techplan sequence. All planning context was produced in a single express session, and the implementation path is well-defined in the business plan and tech plan.

The three stories are tightly sequenced:
1. **BP-1** creates the deliverable (thin conductor implementation)
2. **BP-3** validates it (regression gate — must be green before merge)
3. **BP-4** closes the feature in governance

No parallel story execution is planned; the dependency chain is linear.
