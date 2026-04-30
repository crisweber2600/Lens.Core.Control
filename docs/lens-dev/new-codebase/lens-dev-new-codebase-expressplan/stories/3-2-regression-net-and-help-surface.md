---
feature: lens-dev-new-codebase-expressplan
doc_type: story-file
story_id: 3.2
status: ready-for-dev
goal: "Protect the retained expressplan behavior with focused regressions and stable help surfaces"
depends_on:
  - 3.1
updated_at: 2026-04-27T22:50:00Z
---

# Story 3.2 - Regression Net and Help-Surface Consistency

## User Story

As a maintainer, I want focused regressions around the highest-risk seams so future refactors cannot silently break the express path.

## Acceptance Criteria

- Tests cover express-only gating, QuickPlan retention, review hard-stop behavior, and FinalizePlan reuse.
- Prompt/help/module surfaces continue to expose expressplan.
- The selected review artifact filename is asserted directly.
- Documentation and tests tell the same story about what expressplan owns and what FinalizePlan owns.

## Implementation Notes

- Prefer narrow executable checks over broad suites.
- Treat review filename drift as a regression, not as documentation trivia.
- Keep the help-surface check small enough to run during feature work.

## Validation Targets

- Focused expressplan regression command
- Help/module surface checks
- FinalizePlan reuse assertion