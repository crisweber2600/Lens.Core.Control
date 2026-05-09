---
feature: lens-dev-new-codebase-reopen-feature
doc_type: sprint-plan
status: draft
goal: "Deliver reopen capability in one focused slice with script + tests + skill docs updates."
key_decisions:
  - Single-slice implementation to minimize lifecycle risk.
  - Land tests with implementation in same change.
depends_on:
  - business-plan.md
  - tech-plan.md
blocks: []
updated_at: '2026-05-08T00:00:00Z'
---

# Sprint Plan - Reopen Capability

## Slice 1 - Implement and Verify

Tasks:
1. Add `reopen` command to feature-yaml ops.
2. Add focused tests for reopen pass/fail behavior.
3. Update skill documentation for the new command.
4. Run focused pytest for feature-yaml ops.

Exit criteria:
- Tests pass.
- Reopen command can be executed on a real archived feature.
- Resulting feature state supports return to express planning flows.
