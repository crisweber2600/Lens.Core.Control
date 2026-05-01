---
feature: lens-dev-new-codebase-dogfood
doc_type: code-review
story: E4-S3
commit: 600d9cb0
status: approved
updated_at: "2025-07-17"
---

# Code Review — E4-S3: Branch Preparation Script

## Story

Implement `scripts/branch-prep.py` — a CLI tool and importable module that handles target-repo branch creation and resumption using configurable naming strategies.

## Changes

- `_bmad/lens-work/scripts/branch-prep.py` — script with three strategies and CLI
- `_bmad/lens-work/scripts/branch_prep.py` — importable copy (underscores, for pytest)
- `_bmad/lens-work/scripts/tests/test-branch-prep.py` — 10 tests

## Review

### Correctness

- `flat` strategy returns base_branch unchanged. ✅
- `feature-stub` derives last segment of feature_id. ✅
- `feature-user` raises `ValueError` when username is empty. ✅
- Unknown strategy raises `ValueError`. ✅
- `VALID_STRATEGIES` tuple is exported and tests confirm it. ✅
- `prepare_branch` returns correct `action` values: `"flat"`, `"created"`, `"resumed"`. ✅
- Dry-run mode does not call git commands. ✅

### Security

- `branch_exists_remote` and `branch_exists_local` call git via subprocess with list args (no shell injection). ✅
- CLI uses argparse — no raw string interpolation. ✅

### Test Coverage

10 tests; all pass. ✅

### Issues

None.

## Verdict: APPROVED
