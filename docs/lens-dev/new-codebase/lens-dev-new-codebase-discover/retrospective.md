# Retrospective: Discover Command

**Feature:** lens-dev-new-codebase-discover
**Completed:** 2026-04-30

## What Went Well

- **Single-sprint delivery.** All 8 stories (5.4.1–5.4.7, 5.4.9) delivered in one session on 2026-04-30 with no stories failing.
- **Integration tests were solid anchors.** Story 5.4.9 integration smoke tests were pre-existing and passing from the start, providing an early green signal that the discover orchestration chain worked end-to-end.
- **Express track kept scope tight.** The express lifecycle (no separate business-plan or tech-plan milestones) meant planning artifacts were lean and dev could start immediately with clear, scoped stories.
- **BMB workflow guidance was actionable.** The `bmad-workflow-builder` guidance enabled structured story execution with lint gates (scan-path-standards, scan-scripts) that caught issues before PR.
- **Adversarial review found real issues.** The `PurePosixPath` detour in `discover-ops.py` was flagged and simplified to a clean `.resolve()` path — a genuine correctness improvement.
- **Clean-room implementation worked cleanly.** The feature branch from `develop` had no conflicts and merged successfully as PR #8.

## What Didn't Go Well

- **Windows path friction.** `uv run --with pytest pytest` failed with a spawn permission error on Windows; workaround was to use `python -m pytest`. This is a recurring environment quirk.
- **Broad test suite noise.** Running the full `_bmad/lens-work` test suite surfaces 12 failures from non-discover skills (missing `lifecycle.yaml` in validate-phase-artifacts, unrelated git-orchestration expectations). These are inherited pre-existing failures, not regressions from this feature — but they add noise to the validation story.
- **Governance push contention.** Concurrent governance writes by other feature branches caused one rebase-required push rejection. Resolved cleanly with `pull --rebase` but adds friction in active-sprint contexts.
- **Target repo registration side-effect.** The `repo-inventory.py` CLI matched by remote URL and overwrote the existing old-codebase entry on first run; required manual YAML restoration. The CLI needs a `--create-only` guard.
- **`bmad-lens-document-project` unavailable.** The document-project step in the complete workflow delegates to a skill that does not yet exist in the release module, requiring the step to be satisfied by existing dev closeout artifacts.

## Key Learnings

- Always use `python -m pytest` inside `uv run --with pytest` on Windows rather than invoking pytest as a direct command.
- Before running `repo-inventory.py update` CLI, verify whether an entry with the same remote URL already exists to avoid side-effects on existing entries.
- Scope the test run to the skill under development (`_bmad/lens-work/skills/bmad-lens-discover/scripts/tests/`) rather than the full lens-work suite when validating a single skill's stories.
- Auto-commit hash guard (Story 5.4.2) is the single most important correctness property of the discover command — integration tests that exercise both the commit-fires and no-op paths should be required for any discover changes.
- The `bmad-lens-document-project` skill gap should be tracked as a follow-on feature to fully complete the `lens-complete` workflow.

## Metrics

- Planned duration: 1 sprint (express track, single session)
- Actual duration: 1 day (2026-04-30)
- Stories completed: 8 / 8
- Bugs found post-merge: 0 (PR merged same day, no post-merge reports)
- Tests passing at dev-complete: 12 discover-scoped unit + integration tests
- BMB lint gates: scan-path-standards PASS, scan-scripts PASS

## Action Items

- [ ] Add `--create-only` guard to `repo-inventory.py update` CLI to prevent accidental overwrite of entries with matching remote URLs (`lens-dev-new-codebase-baseline` backlog)
- [ ] Address broad lens-work suite failures in non-discover skills (missing `lifecycle.yaml` in validate-phase-artifacts; `lens-dev-new-codebase-trueup` backlog)
- [ ] Create `bmad-lens-document-project` skill to satisfy the document-project gate in `lens-complete` workflow (`lens-dev-new-codebase-baseline` or new feature)
- [ ] Investigate `uv run pytest` spawn issue on Windows to see if a config fix (e.g., `uv run python -m pytest` alias) can be standardized
