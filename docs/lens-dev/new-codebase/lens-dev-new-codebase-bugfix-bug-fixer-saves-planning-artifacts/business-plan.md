---
feature: lens-dev-new-codebase-bugfix-bug-fixer-saves-planning-artifacts
doc_type: business-plan
status: approved
goal: "Ensure bug-fixer writes plan artifacts to the feature docs path and enforces quick-dev-first within express lane orchestration."
key_decisions:
  - Treat feature.yaml docs.path as the canonical output location for all expressplan artifacts.
  - Keep bug-fixer in express track and avoid fallback flows that bypass expressplan contracts.
  - Require quick-dev-first attempt before broader exploratory/fallback command probes.
open_questions:
  - Should quick-dev-first be hard-fail or soft-fail before fallback is allowed?
  - Should fallback probe count be capped and logged for diagnostics?
depends_on:
  - TargetProjects/lens/lens-governance/bugs/Inprogress/bug-fixer-saves-planning-artifacts-to-wrong-docs-path-and-sk-2f97dc43.md
blocks: []
updated_at: '2026-05-03T19:30:00Z'
---

# Business Plan - Docs Path And Quick-Dev-First

## Problem

The bug-fixer flow created planning artifacts in an incorrect location and did not reliably honor quick-dev-first behavior within the expressplan lane. This causes governance inconsistency, review friction, and unnecessary probe noise during runs.

## Desired Outcome

1. Expressplan artifacts are always written to:
   `docs/lens-dev/new-codebase/<featureId>/`
2. Bug-fixer remains in express lane for this bug class.
3. Quick-dev-first is attempted before any broad fallback search/probing path.

## Scope

In scope:
- Enforce docs output path from `feature.yaml.docs.path`.
- Enforce express lane in bug-fixer orchestration.
- Add quick-dev-first ordering requirement to flow logic and acceptance criteria.

Out of scope:
- Broader refactors of unrelated lifecycle commands.
- Full redesign of bug-fixer architecture beyond this ordering and path contract.

## Success Criteria

- Business, tech, sprint, and review artifacts appear under the feature docs path.
- No artifacts are written outside the configured feature docs folder.
- Orchestration evidence shows quick-dev-first attempt before fallback steps.
- Express lane remains the selected track for the feature lifecycle.
