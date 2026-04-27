---
feature: lens-dev-new-codebase-switch
story_id: SW-4
epic: EP-1
sprint: 1
title: Remove Deprecated Public Command References
estimate: S
status: not-started
blocked_by: []
carry_forward: H1
assignee: crisweber2600
doc_type: story
updated_at: 2026-04-27T00:00:00Z
---

# SW-4 — Remove Deprecated Public Command References

## Context

The 17-command surface removes `init-feature` as a public command name. Switch user-facing messages, fallback help text, and inline guidance must not reference it. Any "missing branches" guidance must direct users to the retained new-feature command alias.

**Carry-forward from:** H1 (expressplan-review), H1 (finalizeplan-review)

## Task

1. Coordinate with the `lens-dev-new-codebase-new-feature` team to confirm the exact retained command alias for initializing a new feature (e.g., `/new-feature`, `/lens-new-feature`, or similar). Do not assume the name — verify it.
2. Search all switch-visible surfaces for the string `init-feature`:
   - `lens-switch.prompt.md` (stub and release)
   - `switch-ops.py` output messages
   - Switch skill references and help text
3. Replace any occurrence with the confirmed retained command alias.
4. Add a string-scan regression to `test-switch-ops.py` (created in SW-9/SW-12) that fails if `init-feature` appears in any switch output string.

## Files

- `lens.core/_bmad/lens-work/prompts/lens-switch.prompt.md`
- `lens.core/_bmad/lens-work/skills/bmad-lens-switch/scripts/switch-ops.py`
- `lens.core/_bmad/lens-work/skills/bmad-lens-switch/SKILL.md`

## Acceptance Criteria

- [ ] Retained command alias confirmed with new-feature team before changes are made.
- [ ] String scan of prompt, script output, and skill references returns zero occurrences of `init-feature` as a user-facing command name.
- [ ] "Missing branches" guidance uses the confirmed retained alias.
- [ ] String-scan regression added to test suite; it fails if `init-feature` appears in switch output.

## Dev Notes

- The string scan should cover user-visible output strings in `switch-ops.py` return payloads (e.g., `message` fields) as well as prompt text.
- Internal variable names or comments that happen to contain `init-feature` are not in scope for replacement.
- This story can run in parallel with SW-1 and SW-2 but should complete before SW-12.
