---
feature: lens-dev-new-codebase-dogfood
epic: 4
story_id: E4-S2
sprint_story_id: S4.2
title: Add dev-session.yaml compatibility layer
type: new
points: M
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E4-S1]
blocks: [E5-S4]
target_repo: lens.core.src
target_branch: develop
---

# E4-S2 — Add dev-session.yaml compatibility layer

## Context

The old codebase's `dev-session.yaml` format has schema differences vs. the new format expected by the E4-S1 conductor. A compatibility layer is required so old-format sessions can be read by the new conductor without a hard migration.

## Implementation Steps

1. Document the old and new `dev-session.yaml` schemas side by side.
2. Implement a read-time compatibility shim: detect old format, translate to new format in-memory only.
3. All writes always emit the new format.
4. Write tests: old-format read → translates to new, new-format read → passthrough, write always new format.

## Acceptance Criteria

- [ ] Old `dev-session.yaml` format reads without error using in-memory translation.
- [ ] New-format reads pass through without modification.
- [ ] All writes emit new format.
- [ ] Schema differences documented.
- [ ] Tests cover old-format read, new-format passthrough, and write-always-new.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill).

## Governance Coordination Note

This story reproduces reference behavior for clean-room parity testing. First-class Dev command authoring belongs to `lens-dev-new-codebase-dev`; any overlap with that feature's eventual PRD must be surfaced as explicit acceptance criteria alignment before implementation proceeds.

## Dev Notes

- Reference: tech-plan, old-codebase dev-session.yaml format in `TargetProjects/lens-dev/old-codebase/`.
- Compatibility shim is read-time only; do not write old-format files.

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
