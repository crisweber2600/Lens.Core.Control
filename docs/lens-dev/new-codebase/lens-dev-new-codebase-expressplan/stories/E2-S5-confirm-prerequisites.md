---
story_id: E2-S5
epic: E2
feature: lens-dev-new-codebase-expressplan
title: Confirm Shared Skill Prerequisites
priority: Medium
size: XS
status: not-started
updated_at: '2026-04-30T00:00:00Z'
---

# E2-S5 — Confirm Shared Skill Prerequisites

## Context

The expressplan SKILL.md depends on at least three shared skills:
- `bmad-lens-quickplan`
- `bmad-lens-bmad-skill`
- `bmad-lens-adversarial-review`

And on `validate-phase-artifacts.py` for the review-ready fast path.

## Tasks

1. Confirm each skill folder exists under
   `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/`.
2. Confirm `validate-phase-artifacts.py` exists under
   `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/scripts/`.
3. For any absent item, create a tracked gap issue or TODO comment in the test file.

## Acceptance Criteria

- [ ] All three shared skills confirmed present or absence noted.
- [ ] `validate-phase-artifacts.py` confirmed present or absence noted.
- [ ] Any absences are tracked (not silent).
