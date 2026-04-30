---
feature: lens-dev-new-codebase-discover
story_id: "5.4.1"
doc_type: story
status: not-started
title: "Finalize SKILL.md behavioral spec"
priority: P0
story_points: 3
epic: "Epic 5 — Discover Command Rewrite"
depends_on: []
blocks: ["5.4.2", "5.4.5", "5.4.9"]
updated_at: 2026-04-29T00:00:00Z
---

# Story 5.4.1 — Finalize SKILL.md Behavioral Spec

**Feature:** `lens-dev-new-codebase-discover`
**Epic:** 5 — Discover Command Rewrite
**Priority:** P0 | **Points:** 3 | **Status:** not-started

---

## Goal

The existing `bmad-lens-discover/SKILL.md` covers all behavioral paths without gaps. This story is the first to start and unblocks all downstream stories.

---

## Context

The `bmad-lens-discover` SKILL.md was created as part of the baseline rewrite stub. It may be incomplete with respect to headless mode, dry-run mode, config resolution priority, and the auto-commit exception section. This story audits and completes the spec.

**Pre-work item (H1 — from adversarial review):** Before making any edits, perform a bounded assessment: read the current SKILL.md end-to-end and produce a gap list against the tech-plan behavioral spec. This assessment is the first AC below and must be completed before any edits start.

**Deferred (OQ-FP2 E):** No-remote edge case is out of scope. If a repo has no git remote, the discover command's behavior is undefined for this sprint. The SKILL.md may note this in an out-of-scope section.

---

## Acceptance Criteria

- [ ] **Assessment first:** Current SKILL.md has been read and a gap list produced against the tech-plan spec (bounded assessment step, resolves H1)
- [ ] Interactive mode flow is documented with sample output
- [ ] Headless mode (`--headless`/`-H`) skips all confirmation prompts
- [ ] Dry-run mode (`--dry-run`) makes no file mutations and no git commits
- [ ] No-op path reports `[discover] Nothing to do` without committing
- [ ] Auto-commit section is clearly labelled as `## Auto-Commit (Governance-Main Exception)`
- [ ] Config resolution priority is documented: `.lens/governance-setup.yaml` → `bmadconfig.yaml` fallback
- [ ] Script subcommands used at each step are listed with their arguments (`scan`, `add-entry`, `validate`)
- [ ] Out-of-scope note added for no-remote repos (OQ-FP2 deferred)

---

## Implementation Notes

- File to edit: `lens.core/_bmad/lens-work/skills/bmad-lens-discover/SKILL.md`
- Use EW (Existing-file Write) or OW (Overwrite) via BMB for all lens-work file changes
- BMB-first: all edits to lens-work skill files must go through BMB authoring workflow
- Reference: `docs/lens-dev/new-codebase/lens-dev-new-codebase-discover/tech-plan.md` for behavioral spec
- Reference: `docs/lens-dev/new-codebase/lens-dev-new-codebase-discover/business-plan.md` for scope constraints

---

## Definition of Done

- [ ] SKILL.md review/edit complete and committed via BMB
- [ ] Gap list from assessment is documented (can be in a brief comment in the commit or in Story 5.4.1 notes)
- [ ] All 8 acceptance criteria checked off
