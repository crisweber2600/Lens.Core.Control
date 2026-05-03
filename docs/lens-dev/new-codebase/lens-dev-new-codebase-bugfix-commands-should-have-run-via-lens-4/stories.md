---
feature: lens-dev-new-codebase-bugfix-commands-should-have-run-via-lens-4
doc_type: stories
status: draft
goal: "Story list for Phase 4 delegation clarification in bmad-lens-bug-fixer/SKILL.md."
key_decisions:
  - One story — XS text-only fix already implemented.
open_questions: []
depends_on: []
blocks: []
updated_at: '2026-05-03T00:00:00Z'
---

# Stories — Commands Should Have Run Via Lens

## Epic 1 — Phase 4 Delegation Clarification

---

### E1-S1 — Update Phase 4 and Error Recovery in bmad-lens-bug-fixer/SKILL.md

**Type:** fix  
**Points:** 1

**Story:** As a LENS workbench user running `/lens-bug-fixer --fix-all-new`, I want Phase 4
instructions to unambiguously activate the expressplan skill, so that Phase 4 completes
without a `command not found` failure.

**Acceptance Criteria:**

1. `bmad-lens-bug-fixer/SKILL.md` Phase 4:
   - [ ] Step 18 reads: "Collect planning input: read each Inprogress bug file and concatenate title + description fields as a single planning context string."
   - [ ] Step 19 contains "Load `{project-root}/lens.core/_bmad/lens-work/skills/lens-expressplan/SKILL.md`" and "Follow its `On Activation` section"
   - [ ] Step 19 contains explicit note: "Do NOT run `lens-expressplan` or any variant as a shell command"
   - [ ] Step 20 handles expressplan failure gracefully (bugs remain Inprogress)

2. `bmad-lens-bug-fixer/SKILL.md` Error Recovery:
   - [ ] Step 3 uses: "Load `{project-root}/lens.core/_bmad/lens-work/skills/lens-expressplan/SKILL.md` and follow its `On Activation` section"
   - [ ] Step 3 no longer says "Delegate expressplan manually" with implicit shell execution

3. Verified:
   - [ ] Commit exists on source repo branch (`fix/preflight-old-patterns` or successor)
   - [ ] Manual test: `/lens-bug-fixer --fix-all-new` Phase 4 completes without `command not found`

**Implementation Notes:**

File: `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-bug-fixer/SKILL.md`

Status: ALREADY IMPLEMENTED — commit `56b1be33` on branch `fix/preflight-old-patterns` in `lens.core.src`.
This story is ready for verification, not implementation.
