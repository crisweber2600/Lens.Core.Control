---
feature: lens-dev-new-codebase-complete
doc_type: handoff-notes
status: complete
updated_at: "2026-06-23T00:00:00Z"
---

# Handoff Notes — lens-dev-new-codebase-complete

## What Was Built

This feature implements the `/complete` command lifecycle — the terminal archival step that marks a feature as `complete` in the governance repo.

### Deliverables

| Artifact | Location (target repo) | Purpose |
|---|---|---|
| `complete-ops.py` | `_bmad/lens-work/lens-complete/scripts/complete-ops.py` | Core archival script — three subcommands |
| `test-complete-ops.py` | `_bmad/lens-work/lens-complete/scripts/tests/test-complete-ops.py` | 21-test regression suite |
| `SKILL.md` | `_bmad/lens-work/lens-complete/SKILL.md` | Authoritative command contract |

### Script Subcommands

1. **`check-preconditions`** — validates that the feature is in a completable phase (`dev` or `dev-complete`) and that `retrospective.md` exists with `status: approved` in frontmatter. Returns `status: pass|warn|fail` as JSON.

2. **`finalize`** — requires `--confirm` or `--dry-run`. Runs precondition check inline. On confirm, atomically writes exactly three files:
   - `feature.yaml` — sets `phase: complete`, adds `completed_at`, appends to `phase_transitions`
   - `feature-index.yaml` — sets feature entry `status: archived`
   - `summary.md` — writes archival summary text

3. **`archive-status`** — read-only inspection. Returns `{"archived": bool, "phase": ..., "index_status": ..., "completed_at": ...}`.

### Key Contract Points

- **Retrospective is a blocker**: `retrospective.md` missing → `retrospective_missing` error, status `fail`. Not approved → `retrospective_not_approved`, status `fail`. This is the authoritative contract from `SKILL.md` — not advisory.
- **Document-project is advisory**: missing project documentation produces `warn` status but does not block finalization.
- **`summary.md` only** — `final-summary.md` is NOT referenced anywhere in the script or prompts (CP-7).
- **Confirmation gate** — `finalize` without `--confirm` or `--dry-run` returns `error: confirmation_required` and writes nothing (CP-10).
- **Boundary discipline** — `finalize` writes only the three files above. `constitutions/` and all other governance paths are untouched (CP-2).
- **Branch cleanup is NOT part of the `/complete` script contract.** Branch archival/deletion is a separate concern outside `complete-ops.py`.

## Behavioral References

- **`lens-dev-new-codebase-new-service`** — reference example for governance write patterns
- **`lens-dev-new-codebase-switch`** — reference example for phase transition mechanics  
- **`lens-dev-new-codebase-baseline`** — upstream dependency; baseline lifecycle contract defines the expressplan/finalizeplan artifact naming that this feature inherits

## Dependencies

- `depends_on: [lens-dev-new-codebase-baseline]`
- Python `pyyaml>=6.0` (declared in `complete-ops.py` script block)

## Test Run Summary

All 21 tests pass under `python -m pytest`. No xfail stubs remain. Coverage spans:
- `check-preconditions` pass/fail/phase guard (CP-1, CP-4)
- All 5 planning phases blocked (CP-4 parametrized)
- `finalize` dry-run / confirm / confirmation gate (CP-1, CP-5, CP-10)
- Archive boundary: no unexpected file writes, no new YAML keys, constitutions untouched (CP-2)
- `archive-status` read-only / archived / not-archived / terminal state (CP-6, CP-12)
- `final-summary.md` absence guard (CP-7)
- Advisory-prerequisite degradation
