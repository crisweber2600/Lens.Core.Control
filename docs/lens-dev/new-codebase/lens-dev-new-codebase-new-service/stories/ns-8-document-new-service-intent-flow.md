# Story NS-8: Document `new-service` intent flow in SKILL.md

Status: ready-for-dev

## Story

As a Lens agent reading the init-feature skill,
I want a documented `new-service` intent flow in `bmad-lens-init-feature/SKILL.md`,
so that I can guide a user through service creation without referencing the script directly.

## Acceptance Criteria

1. SKILL.md has a new section or subsection documenting the `new-service` / `create-service` intent flow
2. Intent flow covers all 7 steps from `tech-plan.md` Skill Intent Contract section
3. Intent flow documents the parent-domain auto-establish behavior (ADR-3): when domain is absent, `create-service` creates the missing parent container
4. **Implementation channel:** this change is authored through `.github/skills/bmad-module-builder`
5. NS-3 SKILL.md discovery check turns green (or passes; SKILL.md already existed, only an existing entry check is needed)

## Tasks / Subtasks

- [ ] Task 1: Author SKILL.md update through `bmad-module-builder` channel (AC: 1–4)
  - [ ] Open `.github/skills/bmad-module-builder/SKILL.md` and follow its workflow for adding a skill intent section
  - [ ] Add `new-service` intent section to `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md`
  - [ ] Document steps: resolve config, resolve/ask domain, ask display name, derive slug, confirm slug, invoke `create-service`, report SHA
  - [ ] Add a note on ADR-3: if parent domain is absent, the command auto-creates domain marker and constitution
- [ ] Task 2: Verify NS-3 skill check turns green (AC: 5)

## Dev Notes

- **Implementation channel rule (H2):** skill artifact changes to `lens.core.src` route through `.github/skills/bmad-module-builder`. Open that skill and follow its pattern for authoring a new intent section.
- SKILL.md target: `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md`
- The intent flow in SKILL.md should be minimal and agent-readable — it is not a user manual; it is the guidance the Lens agent reads when a user says `/new-service`
- Model after the existing `new-domain` intent flow in the same SKILL.md (look for `create-domain` section)

### Project Structure Notes

- SKILL.md is inside `lens.core/` (release module payload) — verify the path via the module source under `TargetProjects/lens-dev/new-codebase/lens.core.src/`
- If `lens.core` is read-only at runtime, author the change in the new-codebase source (`TargetProjects/lens-dev/new-codebase/lens.core.src/`) and note it for release sync

### References

- [tech-plan.md § Skill Intent Contract](../tech-plan.md)
- [tech-plan.md § ADR-1 Keep new-service in bmad-lens-init-feature](../tech-plan.md)
- [finalizeplan-review.md § H2 response — BMB-first rule](../finalizeplan-review.md)
- [stories.md § NS-8](../stories.md)

## Dev Agent Record

### Agent Model Used

_to be filled by dev agent_

### Debug Log References

### Completion Notes List

### File List
