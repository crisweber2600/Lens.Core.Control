---
feature: lens-dev-new-codebase-split-feature
epic: 3
story_id: E3-S2
title: Run full test suite and confirm all test classes pass
type: confirm
points: 2
status: not-started
phase: dev
updated_at: '2026-05-01T00:00:00Z'
depends_on: [E1-S1, E1-S2, E1-S3, E3-S1]
blocks: [E4-S2]
target_repo: lens.core.src
target_branch: develop
---

# E3-S2 — Run full test suite and confirm all test classes pass

## Context

After all script fixes (E1-S1, E1-S2, E1-S3) and test additions (E3-S1) are complete,
this story runs the full test suite and verifies that all 10 test class categories
from tech-plan §6 have passing coverage.

**File:** `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/scripts/tests/test-split-feature-ops.py`

## Implementation Steps

### 1. Run the full test suite

```bash
cd lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/scripts
uv run tests/test-split-feature-ops.py
```

### 2. Verify all 10 test class categories from tech-plan §6

Cross-check the test output against the 10 required test classes:

| Test Class | Coverage Requirement | Covered By |
|-----------|---------------------|-----------|
| validate-first regression | No create/move without passing validate-split | Existing tests |
| In-progress blocking | story with in-progress in sprint-plan YAML, embedded YAML, frontmatter | Existing + E3-S1 |
| Atomic ordering regression | create-split-feature before move-stories; source unmodified until complete | Existing tests |
| Story-file format regression | .md and .yaml story files | Existing tests |
| Dry-run regression | --dry-run produces no writes | Existing tests |
| Governance completeness regression | feature.yaml, feature-index entry, summary stub all written | Existing tests |
| Identifier validation regression | Invalid IDs (uppercase, spaces, path traversal) rejected | Existing tests |
| Status delimiter normalization | in_progress, IN_PROGRESS, in progress trigger hard-stop | E3-S1 |
| Sprint plan format fallback | Unrecognized format falls back to story frontmatter | E3-S1 |
| Duplicate feature-index detection | create-split-feature with existing index entry exits 1 | E3-S1 |

### 3. Fix any failures found

If any test fails:
- Identify the root cause
- Fix the implementation
- Re-run the full suite

### 4. Document final test count

Record total test count in completion notes.

## Acceptance Criteria Checklist

```
[ ] uv run tests/test-split-feature-ops.py exits with zero failures
[ ] All 10 tech-plan §6 test class categories have at least one passing test
[ ] Total test count is ≥ 95
[ ] Final test count documented in completion notes
[ ] No regressions from E1 or E3-S1 changes
```
