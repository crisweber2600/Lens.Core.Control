---
story_id: E1-S4
epic: E1
feature: lens-dev-new-codebase-expressplan
title: Audit Test Coverage Gaps
priority: Medium
size: S
status: not-started
updated_at: '2026-04-30T00:00:00Z'
---

# E1-S4 — Audit Test Coverage Gaps

## Context

`_bmad/lens-work/skills/bmad-lens-expressplan/scripts/tests/test-expressplan-ops.py` was
created in the previous session. It needs to be read and compared against the three-step
contract to identify which regression expectations are covered and which are missing (to be
filled in Epic 2).

## Tasks

1. Open `test-expressplan-ops.py` in the target source repo.
2. List every test function / test class found.
3. Map each to the three-step contract:
   - Eligibility gate
   - QuickPlan delegation
   - Adversarial review invocation
   - Phase advance (pass)
   - Phase advance blocked (fail)
4. List any missing coverage as gap items (these become the input for E2-S2, E2-S3, E2-S4).
5. Ensure the file is committed.

## Acceptance Criteria

- [ ] All test functions listed and mapped.
- [ ] Coverage gaps explicitly documented in a dev note or inline comment.
- [ ] `test-expressplan-ops.py` is committed in the target source repo.
