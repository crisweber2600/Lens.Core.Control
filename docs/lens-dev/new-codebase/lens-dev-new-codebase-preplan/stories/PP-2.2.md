---
feature: lens-dev-new-codebase-preplan
story_id: PP-2.2
epic: PP-E2
title: Implement analyst activation and brainstorm mode selection
estimate: M
sprint: 2
status: not-started
depends_on: [PP-2.1]
blocks: [PP-2.3, PP-3.1]
updated_at: 2026-04-28T00:00:00Z
---

# PP-2.2 — Implement analyst activation and brainstorm mode selection

## Story

**As a** Lens user running preplan interactively,  
**I want** the conductor to activate `bmad-agent-analyst` before presenting brainstorm mode choices,  
**so that** requirements context is grounded before any brainstorming session begins.

## Context

This story implements the brainstorm-first orchestration heart of the preplan conductor. The ordering invariant is strict:

1. `bmad-agent-analyst` activates first — frames goals, constraints, and known assumptions for the feature.
2. After analyst framing completes, the conductor presents the user with a mode choice.
3. User chooses `bmad-brainstorming` (divergent ideation) or `bmad-cis` (structured innovation suite).
4. Selected mode runs to completion through `bmad-lens-bmad-skill`.
5. `brainstorm.md` must exist in the docs path before any research or product-brief wrapper is offered.

Both brainstorm modes must be fully tested — the parity tests for both paths must turn green in this story.

The "brainstorm-first" invariant means the `brainstorm.md` existence check is the gate that separates Phase A (brainstorming) from Phase B (research + product-brief). This check is enforced at the conductor level, not inside the BMAD wrappers.

## Implementation Target

`TargetProjects/lens-dev/new-codebase/lens.core.src/`

## Acceptance Criteria

- [ ] In interactive mode, `bmad-agent-analyst` is activated before any brainstorm wrapper is invoked; analyst framing (goals, constraints, assumptions) completes before mode selection is presented.
- [ ] After analyst framing, the conductor presents user with: `bmad-brainstorming` (divergent ideation) or `bmad-cis` (structured innovation); both described in user-facing terms.
- [ ] Both modes route through `bmad-lens-bmad-skill`; neither is invoked directly.
- [ ] `brainstorm.md` must exist in the docs path before research or product-brief wrappers are callable, regardless of which brainstorm mode was selected.
- [ ] Parity tests that turn green in this story: `test_analyst_activation_ordering`, `test_brainstorm_mode_choice_brainstorming_path`, `test_brainstorm_mode_choice_cis_path`, `test_brainstorm_first_ordering`.
- [ ] `/next` pre-confirmed handoff parity test (`test_next_pre_confirmed_handoff`) remains green.
- [ ] No other parity tests regress.

## Definition of Done

- Analyst activation and brainstorm mode selection merged to feature branch.
- All four parity tests listed above green.
- PR reviewed by at least one other team member.
