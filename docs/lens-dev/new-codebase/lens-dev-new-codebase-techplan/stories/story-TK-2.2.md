---
feature: lens-dev-new-codebase-techplan
story_id: "TK-2.2"
doc_type: story
status: complete
title: "Implement lens-techplan Public Stub"
priority: P0
story_points: 2
epic: "Epic 2 — Target-Project Command Surface"
depends_on: ["TK-2.1"]
blocks: ["TK-2.3"]
updated_at: 2026-04-29T00:00:00Z
---

# Story TK-2.2 — Implement `lens-techplan` Public Stub

**Feature:** `lens-dev-new-codebase-techplan`  
**Epic:** 2 — Target-Project Command Surface  
**Priority:** P0 | **Points:** 2 | **Status:** not-started

---

## Goal

Create the public-facing prompt stub for `lens-techplan` in the target project. The stub runs shared preflight and delegates to the release prompt — no implementation logic.

---

## Context

The stub is the user-facing entry point for the `techplan` command. It must follow the same pattern as existing stubs (e.g., `lens-new-domain.prompt.md`) and must stop fast if preflight fails. Functional behavior comes from the release prompt and conductor skill, not the stub.

**Clean-room rule:** Pattern derived from existing target-project stubs. No old-codebase stub prose reproduced.

---

## Acceptance Criteria

- [ ] File `TargetProjects/lens-dev/new-codebase/lens.core.src/.github/prompts/lens-techplan.prompt.md` exists.
- [ ] The stub runs `uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py` (or the equivalent shared preflight script confirmed in TK-2.1) as its first step.
- [ ] The stub stops if preflight exits non-zero and surfaces the failure clearly.
- [ ] On success, the stub loads `lens.core/_bmad/lens-work/prompts/lens-techplan.prompt.md`.
- [ ] The stub follows the same frontmatter and delegation pattern as existing stubs in the target project.
- [ ] No implementation logic resides in the stub — delegation only.

---

## Dev Notes

- Reference: `TargetProjects/lens-dev/new-codebase/lens.core.src/.github/prompts/lens-new-domain.prompt.md` for the exact stub pattern.
- The release prompt (`_bmad/lens-work/prompts/lens-techplan.prompt.md`) is created in TK-2.3. This story creates only the stub.
- **Note for TK-2.3:** Document any assumptions the stub makes about the release prompt location so TK-2.3 can match them exactly.

---

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

- `TargetProjects/lens-dev/new-codebase/lens.core.src/.github/prompts/lens-techplan.prompt.md`
