---
feature: lens-dev-new-codebase-dogfood
epic: 3
story_id: E3-S4
sprint_story_id: S3.4
title: Normalize Lens wrapper output-path precedence
type: fix
points: L
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E2-S2, E3-S2]
blocks: [E4-S1]
target_repo: lens.core.src
target_branch: develop
---

# E3-S4 — Normalize Lens wrapper output-path precedence

## Context

Lens wrapper scripts produce output paths inconsistently: sometimes the caller-provided path wins, sometimes a module default wins, sometimes they conflict and the wrong file is written. The correct precedence is: caller-supplied > feature.yaml override > module default. This must be enforced across all output-producing operations.

## Implementation Steps

1. Enumerate all wrapper scripts that produce output paths.
2. Define canonical precedence: caller-supplied > feature.yaml override > module default.
3. Refactor each wrapper to apply this precedence explicitly; add inline comment at each path-resolution point.
4. For Windows: ensure all path construction uses `pathlib.Path` or equivalent to avoid separator issues.
5. Write parametrized tests: caller-supplied wins, feature.yaml override wins when no caller arg, module default used when neither is set.

## Acceptance Criteria

- [ ] All output-producing wrappers apply: caller-supplied > feature.yaml override > module default.
- [ ] Path construction is Windows-safe throughout.
- [ ] Parametrized tests covering all three precedence levels pass.
- [ ] No silent path resolution: the resolved path is logged.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill).

## Dev Notes

- Reference: tech-plan ADR-3, Defect 3 (Windows path normalization from E2-S6 affects the same code paths).
- Inline comments are required at every explicit precedence decision point, not just at the top of the function.

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
