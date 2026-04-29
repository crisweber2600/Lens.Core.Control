---
feature: lens-dev-new-codebase-trueup
story_id: "TU-2.2"
story_key: "TU-2-2-author-test-complete-ops-stubs"
epic: "EP-2"
title: "Author test-complete-ops.py Scaffolded Stubs"
status: ready-for-dev
priority: must
story_points: 3
depends_on: []
updated_at: 2026-04-29T00:00:00Z
---

# Story TU-2.2: Author test-complete-ops.py Scaffolded Stubs

Status: ready-for-dev

## Story

As a Lens Workbench maintainer,
I want scaffolded test stubs and a `conftest.py` fixture for `complete-ops.py` committed to the new-codebase source,
so that future implementation of `complete-ops.py` has a ready test harness and contributors can follow the stub → implementation TDD pattern.

## Acceptance Criteria

1. `lens.core.src/_bmad/lens-work/skills/bmad-lens-complete/scripts/tests/test-complete-ops.py` created and committed.

2. `lens.core.src/_bmad/lens-work/skills/bmad-lens-complete/scripts/tests/conftest.py` created and committed (CF-10):
   - Contains at least one pytest fixture for feature.yaml fixture loading (e.g., `@pytest.fixture def sample_feature_yaml(tmp_path): ...`)
   - The fixture creates a minimal feature.yaml in a temporary path for test isolation

3. Eight test stubs present (docstring-only bodies, `pass` implementation):
   - `test_check_preconditions_pass`
   - `test_check_preconditions_warn_no_retrospective`
   - `test_check_preconditions_fail_wrong_phase`
   - `test_finalize_dry_run`
   - `test_finalize_archives_feature`
   - `test_archive_status_archived`
   - `test_archive_status_not_archived`
   - `test_prerequisite_missing_degradation`

4. Each stub has a one-line docstring describing the scenario being tested.

5. File imports pass without errors when run with `python -m pytest --collect-only` (no implementation required, stubs allowed to use `pass`).

## Tasks / Subtasks

- [ ] Review existing test files in `lens.core.src/_bmad/lens-work/skills/` for format reference (pytest conventions, import patterns).
- [ ] Author `conftest.py` with feature.yaml fixture scaffold.
- [ ] Author `test-complete-ops.py` with 8 stubs and docstrings.
- [ ] Run `python -m pytest --collect-only` from the test directory to verify all 8 stubs are collected without import errors.
- [ ] Commit with message: `[DEV] TU-2.2 — author test-complete-ops.py stubs + conftest.py fixture scaffold`.

## Dev Notes

- These are **stubs only** — no test implementation is expected or required.
- The 8 stub names reflect the command surface in TU-2.1 (SKILL.md): `check-preconditions` (3 stubs), `finalize` (2 stubs), `archive-status` (2 stubs), and prerequisite degradation path (1 stub).
- If TU-2.1 has not been completed yet, use the command surface described in `architecture.md` Section 3.2 as the reference.
- The `conftest.py` fixture is critical for future implementation — it establishes test isolation via `tmp_path` so no test writes to the real governance repo.

### References

- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/architecture.md` — Section 3.2 (test stub signatures)
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/finalizeplan-review.md` — CF-10 (conftest.py fixture requirement)
- TU-2.1 story (companion — `bmad-lens-complete` SKILL.md, command surface reference)
- Existing test files in `lens.core.src/_bmad/lens-work/` (format and import convention reference)

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
