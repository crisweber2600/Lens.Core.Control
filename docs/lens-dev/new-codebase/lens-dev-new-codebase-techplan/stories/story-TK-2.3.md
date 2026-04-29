---
feature: lens-dev-new-codebase-techplan
story_id: "TK-2.3"
doc_type: story
status: not-started
title: "Implement Release Prompt and Conductor Skill"
priority: P0
story_points: 3
epic: "Epic 2 — Target-Project Command Surface"
depends_on: ["TK-2.2"]
blocks: ["TK-2.4", "TK-2.5"]
updated_at: 2026-04-29T00:00:00Z
---

# Story TK-2.3 — Implement Release Prompt and Conductor Skill

**Feature:** `lens-dev-new-codebase-techplan`  
**Epic:** 2 — Target-Project Command Surface  
**Priority:** P0 | **Points:** 3 | **Status:** not-started

---

## Goal

Create the release prompt and the `bmad-lens-techplan` conductor skill in the target project. The skill must enforce publish-before-author ordering and the PRD reference rule without implementing architecture generation inline.

---

## Context

The release prompt is a thin redirect. All logic lives in the conductor skill. The skill resolves feature context and delegates architecture authoring through the Lens BMAD wrapper (`bmad-lens-bmad-skill`). Two shared utility surfaces that the skill depends on (publish hook and BMAD wrapper) are delivered in Epic 3 (TK-3.1). This story documents those dependencies explicitly rather than creating local workarounds.

**Clean-room rule:** Implementation derived from `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-3-rewrite-techplan.md` and the baseline architecture docs. No old-codebase skill prose reproduced.

---

## Acceptance Criteria

**Release Prompt:**
- [ ] File `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-techplan.prompt.md` exists.
- [ ] The release prompt is a thin redirect to `bmad-lens-techplan/SKILL.md` — no inline logic.

**Conductor Skill:**
- [ ] File `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md` exists.
- [ ] The skill is conductor-only: resolves feature context and delegates architecture authoring through the Lens BMAD wrapper. No inline architecture generation.
- [ ] The skill documents the publish-before-author dependency: "Reviewed `businessplan` artifacts must be published before architecture authoring begins via `bmad-lens-git-orchestration publish-to-governance --phase businessplan`. This hook is delivered in TK-3.1."
- [ ] The skill documents the BMAD wrapper dependency: "Architecture authoring is delegated through `bmad-lens-bmad-skill`. This wrapper is delivered in TK-3.1."
- [ ] The skill enforces the PRD reference rule: architecture generation is explicitly gated on locating and referencing the authoritative PRD. If the PRD cannot be found, the skill stops and reports the missing reference.
- [ ] The skill does not write governance files directly. All governance writes are routed through the publish hook or `bmad-lens-feature-yaml`.
- [ ] Conductor-only rule is verifiable by inspection: skill content contains only delegation instructions, context resolution, and governance gates — no architecture prose.

---

## Dev Notes

- Reference: `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/4-3-rewrite-techplan.md` for the full runtime contract.
- The PRD reference rule is load-bearing and must not become optional or advisory — enforce it as a hard gate in the skill's activation sequence.
- When documenting publish-before-author dependency: use a visible `> **Prerequisite:** [TK-3.1]` or equivalent marker so implementers in TK-3.1 can find all dependency hooks easily.
- When documenting BMAD wrapper dependency: same pattern — make the TK-3.1 prerequisite visible.

---

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-techplan.prompt.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md`
