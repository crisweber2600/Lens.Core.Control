# Story E1-S3: Scaffold bmad-lens-next/SKILL.md conductor shell

**Feature:** lens-dev-new-codebase-next
**Epic:** Epic 1 — Prompt Chain and Discovery
**Status:** done

---

## Story

As a Lens module maintainer,
I want a thin conductor `bmad-lens-next/SKILL.md` that loads config, resolves the current
feature state by invoking `next-ops.py suggest`, and delegates to the recommended phase
skill or surfaces blockers
so that the `next` command produces deterministic routing outcomes.

## Acceptance Criteria

1. `_bmad/lens-work/skills/bmad-lens-next/SKILL.md` exists in the target repo
2. On activation, the skill loads `bmadconfig.yaml` and resolves `{governance_repo}`,
   `{control_repo}`, and `{feature_id}`
3. Invokes `next-ops.py suggest --feature-id {feature_id}` and reads the JSON result
4. On `status=fail`: surfaces the error, stops
5. On `status=blocked`: lists blockers, stops; no downstream delegation
6. On `status=unblocked`: delegates to the recommended phase skill via `bmad-lens-bmad-skill`
   without surfacing a second confirmation prompt
7. Contains no inline routing logic (all routing in `next-ops.py`)
8. Contains no governance writes or control-doc writes

## Tasks / Subtasks

- [ ] Create `_bmad/lens-work/skills/bmad-lens-next/` directory
- [ ] Author `SKILL.md` with config-load activation block (AC #2)
- [ ] Wire `next-ops.py suggest` invocation (AC #3) — note: `next-ops.py` created in E2-S1;
  scaffold the invocation contract now, full integration in E2+
- [ ] Implement `status=fail` path: surface error, stop (AC #4)
- [ ] Implement `status=blocked` path: list blockers, stop (AC #5)
- [ ] Implement `status=unblocked` path: delegate via `bmad-lens-bmad-skill` without
  confirmation prompt (AC #6)
- [ ] Audit SKILL.md: confirm no inline routing, no write tool calls (AC #7, #8)

## Dev Notes

- **Target file:** `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-next/SKILL.md`
- **Implementation channel:** Use `bmad-module-builder` for SKILL.md authoring
- **Delegation pattern:** `bmad-lens-bmad-skill --skill bmad-lens-{phase}` where `{phase}` comes from `next-ops.py` JSON `recommendation` field (strip leading `/`)
- **No-write rule:** This skill must be read-only. Pre-confirmed handoff is completed in E3-S1.
- **Architecture contract:** [tech-plan.md](../tech-plan.md) §3 (delegation contract), §4 (thin conductor pattern)

### References
- [tech-plan.md — §3 Routing engine contract, §4 Thin conductor](../tech-plan.md)
- [epics.md — Epic 1 scope, Epic 3 pre-confirmed handoff note](../epics.md)
- [stories.md — E3-S1 pre-confirmed handoff story](../stories.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List
