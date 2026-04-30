# Story E1-S4: Register `next` in discovery surfaces

**Feature:** lens-dev-new-codebase-next
**Epic:** Epic 1 — Prompt Chain and Discovery
**Status:** ready-for-dev

---

## Story

As a Lens user,
I want the `next` command to appear in all retained discovery surfaces (`module-help.csv`,
`module.yaml` prompts section, and any command-list surfaces)
so that the command is discoverable through standard help and auto-complete flows.

## Acceptance Criteria

1. `module-help.csv` contains a row for `next` matching the format of other retained commands
2. `module.yaml` prompts section lists `lens-next.prompt.md`
3. No duplicate entries with any other feature (check against current module.yaml)
4. If a `lens.agent.md` or similar command-list surface exists, `next` is present there too

## Precondition (M2)

Before completing this story, verify `lens-dev-new-codebase-trueup` has completed its
discovery-surface writes. This story's registration must not produce conflicts with
trueup's changes. **Document the outcome of this check in Dev Notes — Trueup Status below.**

## Tasks / Subtasks

- [ ] **GATE:** Confirm `lens-dev-new-codebase-trueup` discovery-surface writes are complete
  (check governance `feature.yaml` phase for that feature)
  - Document confirmation in Dev Notes below
- [ ] Locate `module-help.csv` in target repo and add row for `next` (AC #1)
- [ ] Locate `module.yaml` in target repo and add `lens-next.prompt.md` to prompts section (AC #2)
- [ ] Verify no duplicate entries in module.yaml (AC #3)
- [ ] Check for `lens.agent.md` or equivalent; add `next` if present (AC #4)
- [ ] Commit all discovery-surface changes to `lens.core.src` develop branch

## Dev Notes

### Trueup Status (fill in at runtime)

> **CHECK REQUIRED:** Before writing to discovery surfaces, run:
> `cat TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-trueup/feature.yaml | grep phase`
>
> Expected: `phase: finalizeplan-complete` or higher (indicating discovery-surface writes done)
> If the feature is not yet at that phase: coordinate with trueup owner before proceeding.

- Trueup phase confirmed: _(fill in)_
- Conflict check result: _(fill in)_

### References
- [tech-plan.md — §5 Discovery surfaces](../tech-plan.md)
- [business-plan.md — retained command surface requirement](../business-plan.md)
- [finalizeplan-review.md — M2 response (C): serialize with trueup](../finalizeplan-review.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List
