# Story NF-1.3: Add Parity Test Skeletons

**Feature:** lens-dev-new-codebase-new-feature  
**Epic:** 1 — Command Surface and Parity Foundation  
**Estimate:** S  
**Sprint:** 1  
**Status:** backlog  
**Depends on:** NF-1.1, NF-1.2  
**Blocks:** NF-2.1, NF-2.2, NF-2.3 (tests must be red-failing before any implementation begins)  
**Updated:** 2026-04-27

---

## Goal

Add red-failing parity test skeletons to the existing `test-init-feature-ops.py` in the new codebase. These tests define the exact parity contract that Sprint 2–4 implementations must satisfy. Tests must fail **red** (not be skipped) before Sprint 2 begins.

---

## Acceptance Criteria

- [ ] All skeleton tests below exist in `test-init-feature-ops.py` and **fail red** when run before NF-2.1 implementation
- [ ] Existing `create-domain` tests remain **green** (zero regressions)

### Required Skeleton Tests — `create` subcommand

| Test | Expected Behavior |
|---|---|
| `test_create_full_track_start_phase` | Full track returns `starting_phase: preplan` and `recommended_command: /preplan` |
| `test_create_express_track_start_phase` | Express track returns `starting_phase: expressplan` and `recommended_command: /expressplan` |
| `test_create_feature_track_start_phase` | `feature` track returns `starting_phase: businessplan` |
| `test_create_quickplan_alias_start_phase` | `quickplan` resolves to same `starting_phase` as `feature` track |
| `test_create_express_track_no_gh_commands` | Express track: `gh_commands` is `[]` and `planning_pr_created` is `false` |
| `test_create_non_express_track_gh_commands_present` | Non-express track: `gh_commands` is non-empty and `planning_pr_created` is `true` |
| `test_create_invalid_domain_slug_rejected` | Invalid domain slug → `status: fail` before any file write |
| `test_create_invalid_service_slug_rejected` | Invalid service slug → `status: fail` before any file write |
| `test_create_invalid_feature_slug_rejected` | Invalid feature slug → `status: fail` before any file write |
| `test_create_duplicate_feature_rejected` | Feature already in index → `status: fail` before any file write |
| `test_create_missing_track_rejected` | No `--track` argument → `status: fail` with clear error |
| `test_create_dry_run_no_files_created` | `--dry-run` returns planned paths and commands, creates no files |
| `test_create_dry_run_returns_paths` | `--dry-run` output includes `feature_yaml_path`, `index_path`, `summary_path` |

### Required Skeleton Tests — `fetch-context` subcommand

| Test | Expected Behavior |
|---|---|
| `test_fetch_context_feature_not_found` | Unknown `--feature-id` → `status: fail` |
| `test_fetch_context_summaries_depth_returns_summary_paths` | `--depth summaries` → related features return `summary.md` paths only |
| `test_fetch_context_full_depth_returns_feature_yaml_and_docs` | `--depth full` → related features return `feature.yaml` + docs paths |
| `test_fetch_context_depends_on_always_full_depth` | `depends_on` features always return full depth regardless of `--depth summaries` |
| `test_fetch_context_blocks_always_full_depth` | `blocks` features always return full depth regardless of `--depth summaries` |
| `test_fetch_context_missing_service_in_missing_refs` | Non-existent service name → appears in `missing_service_refs`, not `status: fail` |
| `test_fetch_context_explicit_service_ref` | `--service-ref` populates `service_context_paths` with service.yaml + docs |
| `test_fetch_context_service_ref_text_detection` | `--service-ref-text` with text containing service name → populates `detected_service_refs` and `service_context_paths` |

---

## Technical Context

### Test File Location

```
TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/test-init-feature-ops.py
```

This file already exists with `create-domain` test coverage. Add new test functions without modifying or removing existing ones.

### Focused Test Run Command

```bash
# Run from: TargetProjects/lens-dev/new-codebase/lens.core.src/
uv run --with pytest --with pyyaml pytest \
  _bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/test-init-feature-ops.py -q
```

### Test Invocation Pattern

Tests must invoke the real script via subprocess call or direct import — not mock the entire output. The recommended pattern (already used in create-domain tests) is subprocess:

```python
import subprocess, json, sys

def run_create(args: list[str]) -> dict:
    result = subprocess.run(
        [sys.executable, "path/to/init-feature-ops.py", "create"] + args,
        capture_output=True, text=True
    )
    return json.loads(result.stdout)
```

For skeleton tests: call `run_create(...)` or `run_fetch_context(...)` and assert on the expected output field. The assertions will fail red because `create` and `fetch-context` subcommands do not yet exist in the new codebase.

### What "Red-Failing" Means

Each test must actively fail an assertion — not raise an unexpected exception or be skipped. If the subprocess call fails because the subcommand does not exist, the test should catch that and explicitly fail:

```python
def test_create_full_track_start_phase():
    result = run_create(["--governance-repo", "/tmp/gov", "--domain", "test",
                         "--service", "svc", "--name", "my-feat", "--track", "full"])
    assert result.get("status") == "ok"          # fails red: subcommand not found
    assert result["starting_phase"] == "preplan"
```

### Old Codebase Tests as Reference

```
TargetProjects/lens-dev/old-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/test-init-feature-ops.py
```

Use the old tests as a reference for test structure and fixture conventions. Do not copy test logic for `create` or `fetch-context` — rewrite from scratch against the new codebase script contract.

### Governance Fixture

Tests that require a governance repo should use `tmp_path` (pytest fixture) to create a minimal in-memory governance structure:
```
tmp_path/governance/
    feature-index.yaml       # minimal: one existing feature
    features/
        lens-dev/
            test-service/
                lens-dev-test-service-existing/
                    feature.yaml
                    summary.md
```

This prevents tests from touching the real governance repo at `TargetProjects/lens/lens-governance`.

---

## Definition of Done

- All skeleton test functions added to `test-init-feature-ops.py`
- Focused test run shows: all new skeleton tests **FAIL**, all existing create-domain tests **PASS**
- Test run output attached to story (or described in commit message)
- No `pytest.skip()` or `@pytest.mark.skip` used — all tests must be active
