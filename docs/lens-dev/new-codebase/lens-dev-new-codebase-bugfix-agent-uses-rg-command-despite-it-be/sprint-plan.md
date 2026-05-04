---
feature: lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be
doc_type: sprint-plan
status: draft
track: express
updated_at: "2026-05-04T00:08:00Z"
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

Single sprint: 6 bugs, 3 stories, 1 branch (`feature/lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be` from `develop`).

All stories target `TargetProjects/lens-dev/new-codebase/lens.core.src` and `D:/Lens.Core.Control - Copy/AGENTS.md`.

---

## Story Map

| Story | Bugs | Files | Type | Repo |
|-------|------|-------|------|------|
| S1 | B1 (rg), B2 (PS \r\n), B5 (gh pr) | `AGENTS.md`, `git-orchestration-ops.py` (B5 code fix) | Documentation + code | control repo (AGENTS.md) + source repo (B5) |
| S2 | B3 (step3 routing), B6 (branch mismatch hard error) | `git-orchestration-ops.py` | Code + tests | source repo |
| S3 | B4 (--pull-request flag) | `feature-yaml-ops.py` | Code + tests | source repo |

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
- `git-orchestration-ops.py` `create-pr` subcommand adds a merge-base timestamp comparison: if the merge-base with `develop` is more recent than the merge-base with `main`, use `--base develop`; otherwise use `--base main`
- When no shared history exists with the selected base, the command exits non-zero with an explicit error message

**Changes:**
- `D:/Lens.Core.Control - Copy/AGENTS.md`: update the `Common Terminal Errors & Fixes` section
- `_bmad/lens-work/skills/lens-git-orchestration/scripts/git-orchestration-ops.py`: add merge-base timestamp check to `create-pr`

**Implementation sequence (B5 code fix):**
1. In `create-pr` handler, run `git merge-base HEAD main` and `git merge-base HEAD develop` to get merge-base SHAs
2. Compare their commit timestamps using `git log -1 --format="%ct"` on each SHA
3. Select `develop` as base when its merge-base is more recent; fall back to `main`
4. If no candidate branch (neither `main` nor `develop`) can resolve a merge-base SHA with HEAD, emit a structured `no_common_ancestor` error and exit non-zero

**Estimated effort:** S (2–4 hours total: XS doc + XS code)

---

## S2 — git-orchestration-ops.py: Fix step3 routing and add branch mismatch warning

**Bugs covered:** B3 (HIGH), B6 (MEDIUM)  
**Type:** Code + unit tests  
**Branch:** feature/lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be  
**File:** `_bmad/lens-work/skills/lens-git-orchestration/scripts/git-orchestration-ops.py`

**Acceptance criteria:**
- `branch_for_phase_write("X", "finalizeplan", "step3")` returns `("X", "finalizeplan_step_3_to_feature")`
- `branch_for_phase_write("X", "finalizeplan", "step1")` still returns `("X-plan", ...)`
- When expected_branch ≠ current_branch, `commit-artifacts` exits non-zero and returns a structured JSON error with `"status": "error"`, `"error": "branch_mismatch"`, `"current_branch"`, `"expected_branch"`, and a human-readable `"detail"` field — no `--allow-branch-mismatch` bypass flag is added
- **B3 manual-checkout requirement:** `commit-artifacts` does NOT auto-checkout; for `--phase finalizeplan --phase-step step3` the agent must explicitly checkout the `{featureId}` branch before calling `commit-artifacts`

**Implementation sequence:**
1. Update `branch_for_phase_write()`: change step3 return to `feature_id, "finalizeplan_step_3_to_feature"`
2. Update branch mismatch check logic to emit the structured `status: error` JSON and exit non-zero (remove any warn+proceed path)
3. Update existing tests for step3 routing; add mismatch hard-error tests

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

## Sequencing and Dependencies

```
S1 (docs + B5 code fix) — independent; control-repo AGENTS.md commit + source-repo git-orchestration-ops.py commit
S2 (git-orch B3+B6) — independent of S3; source-repo only
S3 (feature-yaml B4) — independent of S2; source-repo only
```

Note: S1 requires two separate commit operations in two repos. S2 and S3 are both source-repo commits on the same feature branch, targeting a PR against `develop`.

## Risk Register

| Risk | Story | Mitigation |
|------|-------|-----------|
| Step3 routing fix breaks non-test callers | S2 | Read all call sites in skill SKILL.md files before applying |
| Branch mismatch hard error blocks callers that relied on warn+proceed | S2 | Confirm no existing callers depend on silent-proceed; document exit-code contract |
| --pull-request field creates duplicate with existing links.issues | S3 | Check feature.yaml schema; links.pull_request is already a declared field |

## Definition of Done

- S1–S3 code/doc changes committed on feature branch
- All existing tests pass
- New tests added for B3 and B4
- PR opened against `develop`
- Feature.yaml updated to `expressplan-complete` via `/finalizeplan`
