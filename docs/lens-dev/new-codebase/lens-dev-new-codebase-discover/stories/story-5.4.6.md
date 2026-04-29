---
feature: lens-dev-new-codebase-discover
story_id: "5.4.6"
doc_type: story
status: not-started
title: "Verify prompt stub and release prompt chain"
priority: P1
story_points: 1
epic: "Epic 5 — Discover Command Rewrite"
depends_on: []
blocks: []
updated_at: 2026-04-29T00:00:00Z
---

# Story 5.4.6 — Verify Prompt Stub and Release Prompt Chain

**Feature:** `lens-dev-new-codebase-discover`
**Epic:** 5 — Discover Command Rewrite
**Priority:** P1 | **Points:** 1 | **Status:** not-started

---

## Goal

The full `discover` command chain is wired and functional: stub → release prompt → SKILL.md.

---

## Context

Lens commands are invoked via a two-level prompt chain:
1. A `.github/prompts/` stub that runs `light-preflight.py` and redirects to the release module prompt
2. The release module prompt at `lens.core/_bmad/lens-work/prompts/lens-discover.prompt.md` that loads config and delegates to SKILL.md

Both levels must exist and be correctly linked. This is an independent story that can run any time after Story 5.4.1 is settled.

---

## Prompt Chain

```
.github/prompts/lens-discover.prompt.md
  → lens.core/_bmad/lens-work/prompts/lens-discover.prompt.md
    → lens.core/_bmad/lens-work/skills/bmad-lens-discover/SKILL.md
```

---

## Acceptance Criteria

- [ ] `.github/prompts/lens-discover.prompt.md` exists as a standard Lens stub
- [ ] Stub runs `light-preflight.py` and redirects to the release prompt path (`lens.core/_bmad/lens-work/prompts/lens-discover.prompt.md`)
- [ ] Release prompt at `lens.core/_bmad/lens-work/prompts/lens-discover.prompt.md` exists and resolves config, then delegates to SKILL.md
- [ ] A manual smoke test confirms the chain executes without errors (no SKILL.md load failure, no missing config)
- [ ] If either file is missing, create it following the standard Lens stub template

---

## Implementation Notes

- Standard stub template reference: `.github/prompts/lens-discover.prompt.md` (examine existing stubs in `.github/prompts/` for format)
- Standard release prompt template reference: `lens.core/_bmad/lens-work/prompts/` (examine existing prompts for format)
- If creating: use the exact same stub pattern as other Lens command stubs
- A manual smoke test means: invoke the stub from Copilot Chat and confirm the SKILL.md loads without error

---

## Definition of Done

- [ ] Both prompt files exist and are correctly linked
- [ ] Manual smoke test passes
- [ ] All 5 acceptance criteria checked off
- [ ] Any new/edited files committed
