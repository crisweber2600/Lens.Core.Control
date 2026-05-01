---
feature: lens-dev-new-codebase-dogfood
epic: 5
story_id: E5-S2
sprint_story_id: S5.2
title: Run command-trace validation for all 17 commands
type: confirm
points: M
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E3-S5, E5-S1]
blocks: [E5-S3, E5-S5]
target_repo: lens.core.src
target_branch: develop
---

# E5-S2 — Run command-trace validation for all 17 commands

## Context

The parity map from E1-S1 defined 17 commands. Sprint 5 requires tracing each command end-to-end to confirm it resolves to a registered skill, produces the expected output artifact, and does not fall through to an unhandled path. QuickPlan classification (E3-S5) determines whether it is included in the public trace.

## Implementation Steps

1. Load the parity map from E1-S1.
2. For each of the 17 commands, execute a trace invocation (dry-run or real) and record the output.
3. Verify: command resolves to a registered skill without error, output artifact type matches contract, no unhandled-path fallthrough.
4. For QuickPlan: if classified public, include in traces; if internal, confirm it does not appear in public help.
5. Commit trace results to `docs/lens-dev/new-codebase/lens-dev-new-codebase-dogfood/command-traces.md`.

## Acceptance Criteria

- [ ] All 17 commands traced.
- [ ] Each trace confirms: registered skill, correct output artifact, no unhandled fallthrough.
- [ ] QuickPlan surface classification confirmed.
- [ ] Trace results committed to `command-traces.md`.

## Dev Notes

- Reference: tech-plan parity map, E1-S1 parity-map.md, E3-S5 QuickPlan decision.

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
