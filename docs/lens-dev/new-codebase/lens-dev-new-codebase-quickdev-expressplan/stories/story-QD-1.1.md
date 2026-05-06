---
feature: lens-dev-new-codebase-quickdev-expressplan
story_id: "QD-1.1"
doc_type: story
status: done
title: "Add Public Prompt and Skill Surfaces"
priority: P0
story_points: 3
epic: "Epic 1 — Governed Quickdev Entry and Planning Gate"
depends_on: []
blocks: ["QD-1.2", "QD-1.3", "QD-1.4"]
updated_at: 2026-05-06T20:55:00Z
---

# Story QD-1.1 — Add Public Prompt and Skill Surfaces

**Feature:** `lens-dev-new-codebase-quickdev-expressplan`
**Epic:** `Epic 1 — Governed Quickdev Entry and Planning Gate`
**Priority:** P0 | **Points:** 3 | **Status:** done

---

## Goal

Create the public `lens-quickdev` prompt and owning skill surfaces in the target source repo so that users can invoke the wrapper through the normal Lens command surface without manually routing through `lens-bmad-skill`.

---

## Context

The current feature packet makes `lens-quickdev` an additive public command. The wrapper must remain conductor-only and must delegate implementation behavior to the existing `bmad-quick-dev` engine. The public command therefore needs two aligned surfaces in `TargetProjects/lens-dev/new-codebase/lens.core.src`:

- `_bmad/lens-work/prompts/lens-quickdev.prompt.md` — redirect-only prompt
- `_bmad/lens-work/skills/lens-quickdev/SKILL.md` — owning conductor contract

This story establishes those surfaces only. Discovery/help registration, lifecycle gating, and quickdev evidence logic are handled by later stories.

## Implementation Steps

1. Add `_bmad/lens-work/prompts/lens-quickdev.prompt.md` as a redirect-only prompt that runs prompt-start preflight and loads the owning skill.
2. Add `_bmad/lens-work/skills/lens-quickdev/SKILL.md` with the conductor contract, explicit dev-ready scope, and delegation to `bmad-quick-dev`.
3. Verify the prompt contains no command registration, lifecycle branching, or implementation logic beyond preflight and skill loading.
4. Verify the skill names the existing quick-dev engine as the only implementation path.

---

## Acceptance Criteria

- [x] `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-quickdev.prompt.md` exists and is redirect-only.
- [x] `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-quickdev/SKILL.md` exists and owns the wrapper contract.
- [x] The prompt runs prompt-start preflight before handing off.
- [x] The prompt contains no business logic beyond preflight and skill loading.
- [x] The skill documents delegation to `bmad-quick-dev` rather than introducing a second implementation engine.

---

## Dev Notes

### Target Files

- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-quickdev.prompt.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-quickdev/SKILL.md`

### Constraints

- Do not edit `lens.core/`; that repo copy is read-only.
- Keep the prompt redirect-only.
- Preserve the existing `bmad-quick-dev` workflow as the only implementation engine.

### Validation

- Inspect the prompt to confirm preflight is the first executable step.
- Inspect the skill contract to confirm it delegates to `bmad-quick-dev`.

## Governance Coordination Note

This story creates public command surfaces only. Any discovery/help registration belongs to QD-1.2 so this story does not silently expand command metadata scope.

## Dev Agent Record

### Agent Model Used

GitHub Copilot

### Debug Log References

- `uv run --with pytest python -m pytest _bmad/lens-work/scripts/tests/test-quickdev-conductor-contract.py` - 5 passed.
- `git diff --check` - passed with no output.
- Target repo commit: `099d2bc1` on `feature/quickdev-expressplan`.

### Completion Notes

- Added the public `/lens-quickdev` module prompt as a redirect-only surface that runs `lens-preflight` before loading the owning skill.
- Added the `lens-quickdev` skill contract with explicit dev-ready preconditions, scope rules, versioned evidence contract, and delegation to `bmad-quick-dev` as the only implementation engine.
- Added focused contract tests that protect the redirect-only prompt and conductor-only skill boundaries.

### File List

- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-quickdev.prompt.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-quickdev/SKILL.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/scripts/tests/test-quickdev-conductor-contract.py`
