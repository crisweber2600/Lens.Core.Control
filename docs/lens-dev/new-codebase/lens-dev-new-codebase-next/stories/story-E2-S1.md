# Story E2-S1: Implement `next-ops.py` core routing logic

**Feature:** lens-dev-new-codebase-next
**Epic:** Epic 2 — Routing Engine Parity
**Status:** ready-for-dev

---

## Story

As a Lens module maintainer,
I want `next-ops.py suggest` to read `feature.yaml` and `lifecycle.yaml` and return a
structured JSON recommendation
so that the `next` conductor has a deterministic, script-testable routing engine.

## Acceptance Criteria

1. Script at `_bmad/lens-work/skills/bmad-lens-next/scripts/next-ops.py` exists
2. `suggest` subcommand accepts `--feature-id` and optional `--governance-repo` and
   `--control-repo` arguments
3. Reads `feature.yaml` for current `phase` and `track`
4. Reads `lifecycle.yaml` from the installed module path (live file, not stubbed)
5. Resolves `auto_advance_to` from lifecycle.yaml for the current phase; uses that as the
   recommendation if set
6. Returns JSON: `{ "status": "unblocked"|"blocked"|"fail", "recommendation": "/phase",
   "blockers": [], "warnings": [], "phase": "...", "track": "...", "error": "..." }`
   — `error` must be populated with a descriptive message when `status=fail`; empty string
   or omitted for `status=unblocked` or `status=blocked`
7. On unknown phase or track: `status=fail` with descriptive message
8. On missing `feature.yaml`: `status=fail` with descriptive message
9. Produces no side effects — no file writes, no git operations

## Paused-State Gate (M1)

Before this story is marked **done**, the selected paused-state behavior must be documented
in the **Paused-State Decision** section below. Options:

- **Option A:** Report paused state as a blocker — `status=blocked, blockers=["feature is paused; use the internal pause-resume skill or the retained recovery path"]`
- **Option B:** Load an internal pause-resume skill route — `status=unblocked, recommendation=/pause-resume` (only valid if an internal skill is retained and loaded intentionally; do not expose a public stub)
- **Option C:** Fail with a descriptive message if no retained recovery path exists — `status=fail, error="feature is paused and no recovery path is available"`

The E2-S4 paused-state fixture **must not be written** until this decision is recorded and
committed in this story file.

## Tasks / Subtasks

- [ ] Create `_bmad/lens-work/skills/bmad-lens-next/scripts/` directory
- [ ] Implement `next-ops.py` with `suggest` subcommand (AC #1, #2)
- [ ] Implement `feature.yaml` reader (AC #3)
- [ ] Implement `lifecycle.yaml` reader from live module path (AC #4) — do NOT stub lifecycle contents
- [ ] Implement `auto_advance_to` resolution (AC #5)
- [ ] Implement JSON output schema (AC #6)
- [ ] Add error paths: unknown phase/track (AC #7), missing feature.yaml (AC #8)
- [ ] Audit script: confirm no side effects (AC #9)
- [ ] **Document paused-state decision** in Dev Notes below before marking done

## Dev Notes

### Paused-State Decision (fill in before marking done)

> **Selected behavior: A**
> Rationale: No pause-resume skill exists in the target repo. Reporting the paused state as
> a blocker is the safest and most transparent behavior — it surfaces the issue clearly
> without implying a recovery path that doesn't exist yet.
> Fixture outcome: E2-S4 will write a paused-state fixture implementing Option A behavior:
> `status=blocked, blockers=["feature is paused; use the pause-resume skill or the retained recovery path"]`

### Implementation Notes

- **Live lifecycle.yaml path:** Resolve from `bmadconfig.yaml` `module_path` key:
  `{module_path}/_bmad/lens-work/lifecycle.yaml`
- **express-track `auto_advance_to`:** In lifecycle.yaml, the express track defines
  `phases: [expressplan, finalizeplan]` and each phase may have `auto_advance_to` pointing
  to the next phase. Load and follow this field — do not hard-code phase transitions.
- **JSON schema is strict:** Callers (`SKILL.md`) depend on the exact field names.

### References
- [tech-plan.md — §3 Routing engine contract](../tech-plan.md)
- [lifecycle.yaml — express track definition](`TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/lifecycle.yaml` — target-project path, not present in this repo)
- [finalizeplan-review.md — M1 (paused-state decision gate), Blind Spot 2 (live lifecycle.yaml)](../finalizeplan-review.md)
- [epics.md — Epic 2 scope](../epics.md)

## Dev Agent Record

### Agent Model Used

Claude Sonnet 4.6

### Debug Log References

None — implementation verified via `uv run next-ops.py suggest --feature-id lens-dev-new-codebase-next`.
Output confirmed `status=blocked` with constitution dependency blocker and `recommendation=/dev`.

### Completion Notes List

- Implemented `next-ops.py` with `suggest` subcommand, argparse, pyyaml inline deps
- Workspace root detection walks up from `__file__` to find `TargetProjects/` directory
- Reads `bmadconfig.yaml` for `governance_repo_path` and `release_repo_root`
- Finds `feature.yaml` by scanning `features/` recursively for matching feature-id directory
- Resolves `auto_advance_to` from live `lifecycle.yaml` (no stubs)
- Paused-state: Option A (blocked report) — rationale documented above
- Surfaces `feature.yaml.warnings` field as response warnings
- No side effects verified: see E3-S2

### File List

- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-next/scripts/next-ops.py` (created)
