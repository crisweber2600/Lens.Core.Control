# Story EP-1.BP-1: Rewrite businessplan command as thin conductor

**Feature:** lens-dev-new-codebase-businessplan  
**Epic:** EP-1 — Businessplan Conductor Rewrite  
**Status:** ready-for-dev

---

## Story

As a Lens maintainer,  
I want the businessplan command rewritten as a thin conductor that delegates shared patterns to canonical shared utilities,  
so that businessplan behavior is preserved while eliminating inline copy-pasted batch logic, review-ready logic, and direct governance writes.

## Acceptance Criteria

1. `publish-to-governance --phase preplan` is invoked before any PRD or UX authoring; no direct governance writes anywhere in the SKILL.md flow
2. Batch pass 1 delegates to `bmad-lens-batch --target businessplan`, writes `businessplan-batch-input.md`, and stops; pass 2 resumes with pre-approved context
3. Review-ready fast path invokes `validate-phase-artifacts.py --phase businessplan --contract review-ready`; on `status=pass` it jumps to adversarial review without re-running the authoring delegation
4. PRD authoring routes to `bmad-lens-bmad-skill --skill bmad-create-prd`
5. UX design authoring routes to `bmad-lens-bmad-skill --skill bmad-create-ux-design`
6. Adversarial review gate blocks `businessplan-complete` transition on `fail` verdict
7. SKILL.md has no inline batch logic, no inline artifact existence checks, and no direct governance file writes
8. When businessplan is invoked via `/next`, no redundant run-confirmation prompt appears and phase entry proceeds immediately

## Tasks / Subtasks

- [ ] Rewrite `lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/SKILL.md` via `bmad-module-builder` (AC: #1, 2, 3, 4, 5, 6, 7)
  - [ ] Confirm publish-before-author hook is first step in activation (AC #1)
  - [ ] Delegate batch logic to `bmad-lens-batch --target businessplan` (AC #2)
  - [ ] Delegate review-ready check to `validate-phase-artifacts.py` (AC #3)
  - [ ] Wire PRD delegation to `bmad-lens-bmad-skill --skill bmad-create-prd` (AC #4)
  - [ ] Wire UX delegation to `bmad-lens-bmad-skill --skill bmad-create-ux-design` (AC #5)
  - [ ] Gate `businessplan-complete` transition on `fail` verdict from adversarial review (AC #6)
  - [ ] Remove all inline batch/artifact-existence/governance-write logic (AC #7)
- [ ] Rewrite `lens.core/_bmad/lens-work/prompts/lens-businessplan.prompt.md` via `bmad-workflow-builder` (AC: #8)
  - [ ] Verify `/next` auto-delegation path produces no run-confirmation prompt (AC #8)
- [ ] Verify `.github/prompts/lens-businessplan.prompt.md` stub chain is intact (AC: all)

## Dev Notes

- **Implementation channel:** `bmad-module-builder` for SKILL.md; `bmad-workflow-builder` for prompt — do NOT copy-paste from old codebase
- **Old codebase path (reference only):** `TargetProjects/lens-dev/old-codebase/lens.core.src/` — consult for behavioral reference; do not copy content
- **Clean-room constraint:** All SKILL.md and prompt content must be freshly authored against the architecture contract defined in `lens-dev-new-codebase-baseline`
- **Pre-flight requirement (FP-2):** Before starting, confirm baseline stories 1.4, 3.1, and 4.1 are merged in `lens.core.src/develop` via `#prompt:lens-preflight.prompt.md`. Check merge state, not just feature-index phase status.
- **Architecture contract:** [tech-plan.md](../tech-plan.md) §3 (delegation contract), §4 (thin conductor pattern), §5 (regression categories)
- **Business context:** [business-plan.md](../business-plan.md) §4 (success criteria), §5 (scope)

### Project Structure Notes

- Target files:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/SKILL.md`
  - `lens.core/_bmad/lens-work/prompts/lens-businessplan.prompt.md`
  - `.github/prompts/lens-businessplan.prompt.md` (stub — verify chain only; do not rewrite)
- Working branch: `feature/businessplan-conductor` in `lens.core.src`

### References

- [Source: business-plan.md §4 — Success Criteria]
- [Source: tech-plan.md §3 — Delegation contract: shared utilities]
- [Source: tech-plan.md §4 — Thin conductor pattern definition]
- [Source: sprint-plan.md — BP-1 Implementation Notes (FP-2 preflight note)]

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List
