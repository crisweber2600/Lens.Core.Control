---
feature: lens-dev-new-codebase-bugfix-bug-fixer-saves-planning-artifacts
doc_type: sprint-plan
status: approved
goal: "Deliver the expressplan bugfix slices that enforce docs path correctness and quick-dev-first flow ordering."
key_decisions:
  - Deliver path enforcement before ordering enforcement.
  - Verify outputs with explicit file existence checks in the feature docs folder.
open_questions:
  - Are additional regression checks needed for multi-bug batches?
depends_on:
  - business-plan.md
  - tech-plan.md
blocks: []
updated_at: '2026-05-03T19:30:00Z'
---

# Sprint Plan - Implementation Slices

## Slice 1 - Docs Path Enforcement

Objective:
- Ensure all expressplan artifacts resolve to and write under `feature.yaml.docs.path`.

Acceptance criteria:
- Artifacts created under `docs/lens-dev/new-codebase/lens-dev-new-codebase-bugfix-bug-fixer-saves-planning-artifacts/`.
- No duplicate artifact set appears in an alternate docs location.

## Slice 2 - Quick-Dev-First Ordering

Objective:
- Ensure bug-fixer attempts quick-dev first in express lane before fallback probing.

Acceptance criteria:
- Run evidence indicates quick-dev invocation is first.
- Fallback is only used after quick-dev failure with reason.

## Slice 3 - Review And Gate

Objective:
- Complete expressplan review and prepare handoff to finalizeplan.

Acceptance criteria:
- `expressplan-adversarial-review.md` exists in feature docs folder.
- Review verdict is `pass` or `pass-with-warnings`.
