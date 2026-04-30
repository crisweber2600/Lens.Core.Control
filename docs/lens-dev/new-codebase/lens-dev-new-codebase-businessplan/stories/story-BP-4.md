# Story EP-1.BP-4: Feature closeout and governance phase advance

**Feature:** lens-dev-new-codebase-businessplan  
**Epic:** EP-1 — Businessplan Conductor Rewrite  
**Status:** ready-for-dev

---

## Story

As a Lens feature lead,  
I want all planning artifacts committed to the control repo and governance updated to `finalizeplan-complete`,  
so that the feature is fully closed out and downstream features (4.4, 4.5) can be unblocked via governance state.

## Acceptance Criteria

1. Control repo `lens-dev-new-codebase-businessplan` branch contains all five planning artifacts: `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, `expressplan-review.md`, `finalizeplan-review.md`
2. `feature.yaml` phase is `finalizeplan-complete` with phase transition record
3. Governance mirror `features/lens-dev/new-codebase/lens-dev-new-codebase-businessplan/docs/` matches control repo docs path
4. Both commits pushed (control repo branch + governance main)

## Tasks / Subtasks

- [ ] Confirm all five planning artifacts are present in `docs/lens-dev/new-codebase/lens-dev-new-codebase-businessplan/` (AC: #1)
- [ ] Update `feature.yaml` phase to `finalizeplan-complete` via `bmad-lens-feature-yaml` (AC: #2)
  - [ ] Add phase transition record with timestamp
- [ ] Run `publish-to-governance --phase finalizeplan` to mirror docs to governance (AC: #3)
- [ ] Commit governance changes to governance repo `main` branch (AC: #4)
- [ ] Verify governance mirror matches control repo docs path (AC: #3)
- [ ] Push control repo changes (AC: #4)
- [ ] Signal baseline feature lead: BP-3 is green; stories 4.4 and 4.5 are unblocked

## Dev Notes

- **This is a governance closeout story, not a code implementation story** — no `lens.core.src` changes expected
- **`feature.yaml` update command:** Use `bmad-lens-feature-yaml` skill to advance phase; do not edit governance files directly with file tools
- **Publish command:** `uv run {module_path}/_bmad/lens-work/skills/bmad-lens-git-orchestration/scripts/git-orchestration-ops.py publish-to-governance --governance-repo {governance_repo} --control-repo {control_repo} --feature-id lens-dev-new-codebase-businessplan --phase finalizeplan`
- **After BP-4:** Signal `lens-dev-new-codebase-baseline` feature lead that stories 4.4 (finalizeplan rewrite) and 4.5 (expressplan rewrite) are unblocked.

### Project Structure Notes

- Control repo docs path: `docs/lens-dev/new-codebase/lens-dev-new-codebase-businessplan/`
- Governance feature path: `features/lens-dev/new-codebase/lens-dev-new-codebase-businessplan/`
- Governance docs mirror: `features/lens-dev/new-codebase/lens-dev-new-codebase-businessplan/docs/`

### References

- [Source: sprint-plan.md — BP-4 scope and acceptance criteria]
- [Source: implementation-readiness.md — Dev signal section]
- [Source: finalizeplan-review.md — Completed actions]

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List
