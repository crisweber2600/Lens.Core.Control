---
feature: lens-dev-new-codebase-bugfix-commands-should-have-run-via-lens-4
doc_type: epics
status: draft
goal: "Clarify Phase 4 expressplan skill delegation in bmad-lens-bug-fixer/SKILL.md to prevent conductor misinterpretation as a shell command."
key_decisions:
  - Single epic with one story — XS text-only fix.
  - No new scripts, tests, or infra required.
open_questions: []
depends_on: []
blocks: []
updated_at: '2026-05-03T00:00:00Z'
---

# Epics — Commands Should Have Run Via Lens

## Epic 1 — Phase 4 Delegation Clarification

**Goal:** Update `bmad-lens-bug-fixer/SKILL.md` Phase 4 instructions so a conductor
unambiguously loads and follows the `lens-expressplan` SKILL.md instead of attempting
to run a shell command.

**Scope:**
- Replace Phase 4 steps 18–20 with explicit SKILL.md-load delegation language.
- Update Error Recovery step 3 with the same explicit delegation pattern.
- No changes to scripts, other skills, or test infrastructure.

**Exit Criteria:**
- Steps 18–20 read "Load `{project-root}/lens.core/_bmad/lens-work/skills/lens-expressplan/SKILL.md`
  and follow its `On Activation` section."
- Explicit note: "Do NOT run `lens-expressplan` as a shell command."
- Error Recovery step 3 uses identical delegation language.
- Commit present on `lens.core.src` branch.

---

### Story 1.1: Update Phase 4 and Error Recovery in bmad-lens-bug-fixer/SKILL.md

As a LENS workbench user running `/lens-bug-fixer --fix-all-new`,
I want Phase 4 instructions to unambiguously activate the expressplan skill,
So that Phase 4 completes without a `command not found` failure.

**Acceptance Criteria:**

**Given** `bmad-lens-bug-fixer/SKILL.md` is loaded by a conductor  
**When** the conductor reaches Phase 4 step 19  
**Then** it loads `{project-root}/lens.core/_bmad/lens-work/skills/lens-expressplan/SKILL.md`  
**And** follows its `On Activation` section rather than running a shell command  
**And** Error Recovery step 3 uses the same explicit delegation language
