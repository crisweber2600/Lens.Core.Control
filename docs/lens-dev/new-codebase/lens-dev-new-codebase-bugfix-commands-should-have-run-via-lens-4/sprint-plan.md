---
feature: lens-dev-new-codebase-bugfix-commands-should-have-run-via-lens-4
title: "Bugbash: Commands Should Have Run Via Lens 4F788235"
doc_type: sprint-plan
status: draft
track: express
phase: expressplan
created_at: 2026-05-03
---

# Sprint Plan — Commands Should Have Run Via Lens

## Sprint 1 (and only sprint)

### Story 1 — Update Phase 4 delegation wording in bmad-lens-bug-fixer/SKILL.md

**Acceptance criteria:**
- Phase 4 steps 18–20 use unambiguous language: "Load SKILL.md and execute" not "delegate to skill"
- Explicit note: do NOT run as a shell command
- Error Recovery section updated to match

**File:** `_bmad/lens-work/skills/bmad-lens-bug-fixer/SKILL.md`
**Branch:** Current `fix/preflight-old-patterns` (or a new `fix/bug-fixer-phase4-delegation`)
**Effort:** XS — text change only

**Implementation steps:**
1. Open `bmad-lens-bug-fixer/SKILL.md` in source repo.
2. Replace Phase 4 steps 18–20 with the clarified text from `tech-plan.md`.
3. Update Error Recovery section (step 3: "Delegate expressplan manually") with the same
   explicit SKILL.md load instruction.
4. Commit: `fix: clarify Phase 4 expressplan delegation in bug-fixer SKILL.md`
5. Push. PR to develop.

## Dependencies

None. Single-file text change.

## Unresolved Risks

- Other bugs in `bugs/Inprogress/` from prior failed runs (e.g., `finalizeplan-final-pr-*`
  and `lens-dev-does-not-create-pr-*`) are stranded. These require manual recovery via
  Error Recovery steps in the SKILL.md — not in scope for this sprint.
