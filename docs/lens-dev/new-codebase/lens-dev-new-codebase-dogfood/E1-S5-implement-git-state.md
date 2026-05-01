---
feature: lens-dev-new-codebase-dogfood
epic: 1
story_id: E1-S5
sprint_story_id: S1.5
title: Implement read-only git-state operations
type: new
points: M
status: done
phase: dev
updated_at: '2026-05-01T18:22:00Z'
depends_on: [E1-S1, E1-S3]
blocks: [E2-S1, E2-S4]
target_repo: lens.core.src
target_branch: develop
---

# E1-S5 — Implement read-only git-state operations

## Context

The target module has no `bmad-lens-git-state` skill. Phase conductors and cross-feature context loading depend on the ability to inspect branch topology, active features, and git-vs-yaml discrepancies without performing any writes.

## Implementation Steps

1. Create `_bmad/lens-work/skills/bmad-lens-git-state/SKILL.md` in the target.
2. Implement branch topology reporting: current branch, all feature branches present, which features have plan/dev branches open.
3. Implement active feature reporting: reads governance feature-index, reports phase per feature.
4. Implement discrepancy reporting: compares `feature.yaml` phase to branch state and flags mismatches.
5. Enforce strict read-only constraint: no git writes, no file mutations.
6. Write focused tests for branch detection, discrepancy reporting, and read-only constraint.

## Acceptance Criteria

- [x] `bmad-lens-git-state` skill exists in target `_bmad/lens-work/skills/`.
- [x] Reports: current branch, all feature branches, which features have plan/dev branches open.
- [x] Reports: active features from governance feature-index with phase.
- [x] Reports: git-vs-yaml discrepancies (bidirectional: both missing branches and orphan branches).
- [x] Strictly read-only: no writes, no mutations.
- [x] Focused tests cover all reporting modes and read-only constraint (10 tests: 5 original + 5 new).

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill).

Load the BMB documentation index at `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md` before authoring.

## Dev Notes

- Reference: tech-plan Git State Contract section.
- Read-only constraint is strict: if any code path could mutate the repo, it must be rejected at design time.
- Discrepancy reporting should surface the specific field mismatch (e.g., `feature.yaml.phase=dev` but no `{featureId}-dev` branch exists).

## Dev Agent Record

### Agent Model Used
GitHub Copilot (implementation), GitHub Copilot (adversarial review)

### Debug Log References
- Implemented read-only git state operations: branch-state, active-features, discrepancies, feature-state.
- Unit tests pass: 5 passed via pytest.
- Real command validation: branch-state and feature-state succeed with pass/read_only JSON.

### Review Gate Findings (FAIL)

**CRITICAL:**
1. Discrepancy detection is one-directional. Detects: "phase=dev but no dev branch". Missing: "dev branch exists but phase≠dev". This violates AC #4 (git-vs-yaml discrepancy reporting).
2. Subprocess timeout missing. `subprocess.run()` in `run_git_read_only()` has no timeout; script can hang indefinitely on network-mounted repos or stalled git processes.

**HIGH:**
3. Error message information disclosure. Raw git stderr leaked in JSON responses; can expose paths/commands from malicious repos.
4. Missing test coverage for `feature-state` command (one of four main commands).
5. YAML parsing has no size/complexity limits; DoS risk on malicious governance repo.

**MEDIUM:**
6. Test helper `init_repo()` uses `check=True` without try/except; fails ungracefully if git unavailable.
7. No test for missing feature-index.yaml error path.
8. Config discovery path resolution is implicit; can resolve to wrong governance repo.

**RECOMMENDATION:** Hold pending rework on critical and high findings before marking complete.

### Completion Notes List
- Implemented `bmad-lens-git-state` skill with CLI-backed JSON operations.
- branch-state: current branch, feature branches, plan/dev topology.
- active-features: governance feature-index with phase from feature.yaml or index fallback.
- discrepancies: phase-vs-branch mismatches (bidirectional: detects both missing branches and orphan branches).
- Strict read-only: syntactic enforcement via git allowlist; semantic constraints enforced.
- Unit tests: 10 tests (5 original + 5 rework), all passing:
  - Original: branch topology, active features, phase mismatches, read-only allowlist
  - Rework: inverse discrepancies, subprocess timeout, error sanitization, feature-state, YAML DoS guard
- Critical fixes applied after first review: inverse discrepancy checks, subprocess timeout=30s, error sanitization, YAML 10MB guard.
- Rework review verdict: PASS (all findings addressed, no regressions).

### File List
- `_bmad/lens-work/skills/bmad-lens-git-state/SKILL.md`
- `_bmad/lens-work/skills/bmad-lens-git-state/scripts/git-state-ops.py`
- `_bmad/lens-work/skills/bmad-lens-git-state/scripts/tests/test-git-state-ops.py`

**Target Commits:**
- Initial: ef38c573 (review failed: critical gaps found)
- Rework: 7da6a51c (review passed: all critical and high findings fixed)
