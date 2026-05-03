---
feature: lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be
doc_type: sprint-plan
status: draft
track: express
updated_at: "2026-05-03T23:44:00Z"
depends_on: []
blocks: []
key_decisions:
  - "All 6 bugs are independent and can be worked in parallel on the same branch"
  - "B3 requires FinalizePlan contract verification before code change — confirmed: step3 → {featureId}"
  - "B4 and B6 are code changes to the same files as B3 — group into one story"
open_questions: []
---

# Sprint Plan — Environment, Orchestration and Tooling Fixes

## Sprint Overview

Single sprint: 6 bugs, 4 stories, 1 branch (`feature/lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be` from `develop`).

All stories target `TargetProjects/lens-dev/new-codebase/lens.core.src` and `D:/Lens.Core.Control - Copy/AGENTS.md`.

---

## Story Map

| Story | Bugs | Files | Type |
|-------|------|-------|------|
| S1 | B1 (rg), B2 (PS \r\n), B5 (gh pr) | `AGENTS.md` | Documentation only |
| S2 | B3 (step3 routing), B6 (branch mismatch warning) | `git-orchestration-ops.py` | Code + tests |
| S3 | B4 (--pull-request flag) | `feature-yaml-ops.py` | Code + tests |
| S4 | Regression test sweep | `scripts/tests/` | Tests only |

---

## S1 — AGENTS.md: Document unavailable commands and unsafe patterns

**Bugs covered:** B1, B2, B5  
**Type:** Documentation  
**Branch:** feature/lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be

**Acceptance criteria:**
- `AGENTS.md` `Common Terminal Errors & Fixes` section formats all three known errors in the `**Error** / **Cause** / **Fix**` block format
- `rg` unavailability is clearly stated with fix = "use grep"
- PowerShell bulk-replace restriction is documented with Python alternative
- `gh pr create` history check requirement is documented

**Changes:**
- `D:/Lens.Core.Control - Copy/AGENTS.md`: update the `Common Terminal Errors & Fixes` section

**Estimated effort:** XS (< 1 hour)

---

## S2 — git-orchestration-ops.py: Fix step3 routing and add branch mismatch warning

**Bugs covered:** B3 (HIGH), B6 (MEDIUM)  
**Type:** Code + unit tests  
**Branch:** feature/lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be  
**File:** `_bmad/lens-work/skills/lens-git-orchestration/scripts/git-orchestration-ops.py`

**Acceptance criteria:**
- `branch_for_phase_write("X", "finalizeplan", "step3")` returns `("X", "finalizeplan_step_3_to_feature")`
- `branch_for_phase_write("X", "finalizeplan", "step1")` still returns `("X-plan", ...)`
- When expected_branch ≠ current_branch **and** `--allow-branch-mismatch` is NOT passed, `commit-artifacts` returns `status: warn` JSON with `warning: branch_mismatch`, current and expected branch names, and actionable detail
- When `--allow-branch-mismatch` is passed, commit proceeds with warning in output
- New `--allow-branch-mismatch` CLI flag is added to the `commit-artifacts` subparser

**Implementation sequence:**
1. Update `branch_for_phase_write()`: change step3 return to `feature_id, "finalizeplan_step_3_to_feature"`
2. Add `--allow-branch-mismatch` to `ca` (commit-artifacts) argparser
3. Update branch mismatch check logic to emit `status: warn` vs hard error based on flag
4. Update existing tests for step3 routing; add mismatch warning tests

**Estimated effort:** S (2–4 hours)

---

## S3 — feature-yaml-ops.py: Add --pull-request flag

**Bug covered:** B4 (MEDIUM)  
**Type:** Code + unit test  
**Branch:** feature/lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be  
**File:** `_bmad/lens-work/skills/lens-feature-yaml/scripts/feature-yaml-ops.py`

**Acceptance criteria:**
- `feature-yaml-ops.py update --pull-request <url>` exits 0 and sets `links.pull_request` in feature.yaml
- `feature-yaml-ops.py --help` shows `--pull-request` in the `update` subcommand
- No regression on existing `update` flags

**Implementation sequence:**
1. Add `update_parser.add_argument("--pull-request", ...)` after line ~559
2. In update handler: if `args.pull_request` is set, set `feature_data.setdefault("links", {})["pull_request"] = args.pull_request`
3. Add unit test: invoke with `--pull-request https://example.com` and assert field is set in written YAML

**Estimated effort:** XS (< 1 hour)

---

## S4 — Regression tests for B3 routing fix

**Type:** Tests only  
**Branch:** feature/lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be  
**File:** `_bmad/lens-work/scripts/tests/test-*.py` (add or update)

**Acceptance criteria:**
- Test suite passes for all changed code paths
- step3 routing test added (passes with new behavior, would have failed pre-fix)
- Branch mismatch warning test added

**Estimated effort:** XS (covered in S2 work)

---

## Sequencing and Dependencies

```
S1 (docs) — independent, can be done first
S2 (git-orch) — independent of S3
S3 (feature-yaml) — independent of S2
S4 (tests) — part of S2; no separate sprint item needed
```

All stories can be worked concurrently on the same branch.

## Risk Register

| Risk | Story | Mitigation |
|------|-------|-----------|
| Step3 routing fix breaks non-test callers | S2 | Read all call sites in skill SKILL.md files before applying |
| --allow-branch-mismatch flag causes agents to bypass branch checks | S2 | Make flag opt-in only; do not set as default |
| --pull-request field creates duplicate with existing links.issues | S3 | Check feature.yaml schema; links.pull_request is already a declared field |

## Definition of Done

- S1–S3 code/doc changes committed on feature branch
- All existing tests pass
- New tests added for B3 and B4
- PR opened against `develop`
- Feature.yaml updated to `expressplan-complete` via `/finalizeplan`
