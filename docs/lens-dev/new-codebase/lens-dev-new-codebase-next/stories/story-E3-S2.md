# Story E3-S2: Add negative test for no-write behavior

**Feature:** lens-dev-new-codebase-next
**Epic:** Epic 3 — Delegation and Release Hardening
**Status:** ready-for-dev

---

## Story

As a Lens module maintainer,
I want a test confirming that `next-ops.py suggest` and the `bmad-lens-next/SKILL.md`
conductor produce no file system writes, no governance writes, and no control-doc writes
so that `next` remains a strictly read-only routing surface.

## Acceptance Criteria

1. Test script (or test case in the existing test suite) asserts that running
   `next-ops.py suggest` with a valid `feature.yaml` produces zero file system changes
2. Test also runs `next-ops.py suggest` with an invalid `feature.yaml` and confirms zero
   file system changes
3. No governance-repo files are modified by `next-ops.py` invocation
4. No control-repo files are modified by `next-ops.py` invocation
5. SKILL.md is audited: confirm no `create_file`, `replace_string_in_file`, or similar
   write-capable tool calls appear in the skill instructions
6. Test passes in CI

## Tasks / Subtasks

- [ ] Identify the test runner location (`lens.core/_bmad/lens-work/tests/` or equivalent)
- [ ] Write `test_next_no_writes.py` (or equivalent):
  - [ ] Snapshot current file system state (relevant dirs)
  - [ ] Run `next-ops.py suggest --feature-id {test_feature}` with a valid feature.yaml
  - [ ] Assert no new or modified files (AC #1, #3, #4)
  - [ ] Repeat with invalid feature.yaml (AC #2)
- [ ] Audit `bmad-lens-next/SKILL.md` text for write-capable tool calls (AC #5)
- [ ] Confirm test runs in CI workflow (AC #6)
- [ ] Commit test to `lens.core.src` develop branch

## Dev Notes

- **From party-mode Blind Spot 1:** The original tech-plan did not have an explicit
  no-write test. This story directly addresses that gap.
- **Audit pattern for SKILL.md:** grep for `create_file`, `write`, `git commit`, `git push`,
  `replace_string`, `edit`, and any governance-repo path references. Any match is a finding.
- **CI integration:** Follow the pattern of existing `lens.core` CI test registration.

### References
- [finalizeplan-review.md — Blind Spot 1 (no-write regression test missing)](../finalizeplan-review.md)
- [tech-plan.md — §6 No-write invariant](../tech-plan.md)
- [epics.md — Epic 3 no-write verification scope](../epics.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List
