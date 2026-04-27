# Story NS-13: Prepare implementation handoff notes (required)

Status: ready-for-dev

## Story

As the `/dev` agent picking up implementation,
I want a self-contained handoff document that names every file to touch, every test to run, and every constraint to honor,
so that I do not need to re-read the full planning set to begin implementation.

## Acceptance Criteria

1. Handoff notes name all files to create or modify
2. Handoff notes include the exact test runner command for focused service tests
3. Handoff notes include the exact test runner command for full init-feature regression
4. Handoff notes record the clean-room constraint
5. Handoff notes record implementation channel decisions
6. Handoff notes record accepted deviations for direct `lens.core.src` edits
7. **This story is a required gate** — NS-13 must be complete before NS-12 can be marked done

## Tasks / Subtasks

- [ ] Task 1: Fill in the Completion Notes List below with all 7 required items

## Dev Notes

This file IS the handoff document. Fill in the Completion Notes List when implementation is complete.

### Pre-dev Setup Required

Before Sprint 1 begins:
1. Clone `lens.core.src` into `TargetProjects/lens-dev/new-codebase/lens.core.src/` (if not already done)
2. Clone `lens-governance` into `TargetProjects/lens/lens-governance/` (if not already done)
3. Run `/discover` with `domain: lens-dev, service: new-codebase`

### Files to Create or Modify

| File | Action | Story |
|------|--------|-------|
| `_bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/test_create_service.py` | Create | NS-1, NS-2, NS-3 |
| `_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py` | Modify — add helpers | NS-4 |
| `_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py` | Modify — add parser route and handler | NS-5 |
| `_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py` | Modify — extend context writer | NS-6 |
| `_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py` | Modify — governance git wiring | NS-7 |
| `_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md` | Modify — add new-service intent flow | NS-8 |
| `_bmad/lens-work/prompts/lens-new-service.prompt.md` | Create | NS-9 |
| `_bmad/lens-work/module-help.csv` | Modify — add new-service row | NS-10 |

### Test Runner Commands

**Focused service parity tests (NS-11):**
```bash
cd TargetProjects/lens-dev/new-codebase/lens.core.src
uv run --with pytest pytest _bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/ -k create_service -q
```

**Full init-feature regression (NS-12):**
```bash
cd TargetProjects/lens-dev/new-codebase/lens.core.src
uv run --with pytest pytest _bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/ -q
```

### Clean-Room Constraint

No source may be copied verbatim from the old codebase (`TargetProjects/lens-dev/old-codebase/`). Implementation must be written from the observable contract defined by the NS-1 tests and the ADRs in `tech-plan.md`.

### Implementation Channel Decisions (H2, BMB-first rule)

| Artifact type | Channel |
|---------------|---------|
| SKILL.md updates (NS-8) | `.github/skills/bmad-module-builder` |
| Release prompt (NS-9) | `.github/skills/bmad-workflow-builder` |
| Script changes (NS-4–NS-7) | Direct edit to `lens.core.src` — accepted deviation |

### Accepted Deviations

Direct edits to `lens.core.src` for NS-4, NS-5, NS-6, and NS-7 (script implementation stories) are accepted per `gate_mode: informational` for this cycle. This deviation is explicitly recorded here as required by the FinalizePlan review (H2 response).

### ADR-3 Delegation Boundary

`create-service` must NOT re-implement domain marker or domain constitution creation. All parent-domain creation must delegate to the existing `create-domain` helpers:
- `make_domain_yaml(...)` — domain marker YAML
- `make_domain_constitution_md(...)` — domain constitution
- `get_domain_marker_path(...)` — governance path for domain marker
- `get_domain_constitution_path(...)` — governance path for domain constitution

This is the single code path for domain-marker creation. Verify before NS-4 is merged that no duplicate path exists.

### Project Structure Notes

- All files above are in: `TargetProjects/lens-dev/new-codebase/lens.core.src/`
- Context file path: `.lens/personal/context.yaml` (resolved from `--personal-folder` arg)

### References

- [tech-plan.md](../tech-plan.md)
- [sprint-plan.md](../sprint-plan.md)
- [epics.md](../epics.md)
- [stories.md](../stories.md)
- [finalizeplan-review.md](../finalizeplan-review.md)

## Dev Agent Record

### Agent Model Used

_to be filled by dev agent_

### Debug Log References

### Completion Notes List

_Fill in when implementation is complete. Must include all 7 items from Acceptance Criteria before NS-12 can be marked done._

1. Files created or modified: [ see table above ]
2. Focused service parity test command: `uv run --with pytest pytest ... -k create_service -q`
3. Full regression command: `uv run --with pytest pytest ... -q`
4. Clean-room constraint: no verbatim copy from old codebase
5. Implementation channel decisions: SKILL.md via bmad-module-builder; prompt via bmad-workflow-builder
6. Accepted deviations: direct `lens.core.src` edits for NS-4–NS-7 accepted per `gate_mode: informational`
7. ADR-3 delegation boundary: documented in code; `create-service` calls `create-domain` helpers only

### File List
