---
feature: lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be
doc_type: epics
status: draft
stepsCompleted: [1, 2, 3, 4]
inputDocuments:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be/business-plan.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be/tech-plan.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be/sprint-plan.md
updated_at: "2026-05-04T00:30:00Z"
---

# Environment, Orchestration and Tooling Fixes — Epic Breakdown

## Overview

This document decomposes the 6-bug fix batch into epics and stories. The bugs span two concern domains: agent environment/documentation (B1, B2, B5) and script-level orchestration logic (B3, B4, B6). Both epics target the source repo `TargetProjects/lens-dev/new-codebase/lens.core.src`.

---

## Requirements Inventory

### Functional Requirements

| ID | Requirement |
|----|-------------|
| FR1 | Replace all `rg` invocations with `grep` in agent-executed commands; document in AGENTS.md |
| FR2 | Prohibit PowerShell heredoc for multi-file text replacement; provide Python alternative; document in AGENTS.md |
| FR3 | Fix `branch_for_phase_write()` step3 routing: return `feature_id` (not `feature_id + "-dev"`) for `finalizeplan step3` |
| FR4 | Add `--pull-request` flag to `feature-yaml-ops.py update` subparser; wire to set `links.pull_request` in feature.yaml |
| FR5 | Add merge-base timestamp comparison to `git-orchestration-ops.py create-pr`; auto-select base branch; error on no shared history |
| FR6 | Update branch-mismatch error in `commit-artifacts` to emit structured JSON with `error: branch_mismatch`, `current_branch`, `expected_branch`, and `detail`; always hard-error, no bypass flag |

### Non-Functional Requirements

| ID | Requirement |
|----|-------------|
| NFR1 | All changes are backward-compatible — no existing callers broken |
| NFR2 | All code changes include unit tests covering happy path and error path |
| NFR3 | JSON output shapes for `git-orchestration-ops.py` preserve existing field names; only additive changes |

### Additional Requirements

- AGENTS.md documentation changes apply to the control repo (`D:/Lens.Core.Control - Copy/AGENTS.md`)
- Source repo changes apply only to `TargetProjects/lens-dev/new-codebase/lens.core.src`
- B3 fix requires explicit branch checkout by the agent before calling `commit-artifacts --phase-step step3` — the script does not auto-checkout

### UX Design Requirements

None — no UI surface changes.

### FR Coverage Map

| FR | Epic | Story |
|----|------|-------|
| FR1 | Epic 1 | S1 |
| FR2 | Epic 1 | S1 |
| FR5 | Epic 1 | S1 |
| FR3 | Epic 2 | S2 |
| FR6 | Epic 2 | S2 |
| FR4 | Epic 2 | S3 |

---

## Epic List

1. **Epic 1 — Agent Environment and Documentation Hardening** — FR1, FR2, FR5
2. **Epic 2 — Orchestration Script Correctness and CLI Completeness** — FR3, FR4, FR6

---

## Epic 1: Agent Environment and Documentation Hardening

Fix all agent-observable environment gaps and documentation deficiencies that cause wasted turns, corrupted artifacts, or PR targeting errors. All changes in this epic are additive (new formatted error blocks in AGENTS.md) plus one code enhancement to `git-orchestration-ops.py create-pr`.

### Story 1.1: Document unavailable commands and unsafe patterns in AGENTS.md

As a **Lens agent**,  
I want AGENTS.md to clearly document `rg` unavailability, PowerShell heredoc prohibition, and the `create-pr` base-branch requirement,  
So that I do not waste turns retrying unavailable commands or corrupt prompt files with literal escape sequences.

**Acceptance Criteria:**

**Given** the AGENTS.md `Common Terminal Errors & Fixes` section exists  
**When** I add three new formatted error blocks (rg, PowerShell, gh pr history)  
**Then** each block uses `**Error** / **Cause** / **Fix**` format matching the existing documented example

**And** the `rg` block states: Error = `rg: command not found`, Cause = `ripgrep (rg) is not installed`, Fix = use `grep`

**And** the PowerShell block states: Error = prompt files contain literal `\r\n`, Cause = PowerShell heredoc does not expand escape sequences, Fix = use Python with `pathlib` and explicit `encoding="utf-8"`

**And** the `gh pr create` block states: Error = no common ancestor, Cause = branch created from `develop` but `--base main` was passed, Fix = use `create-pr` in `git-orchestration-ops.py` which performs merge-base selection

### Story 1.2: Add merge-base history check to git-orchestration-ops.py create-pr

As a **Lens agent**,  
I want `git-orchestration-ops.py create-pr` to automatically select the correct base branch by comparing merge-base timestamps,  
So that PRs never target a base branch that shares no history with the head.

**Acceptance Criteria:**

**Given** I call `create-pr --head <branch> --base <branch>` with an explicit `--base`  
**When** the command runs  
**Then** it verifies the head branch shares history with the specified base

**Given** I call `create-pr` without specifying `--base` (auto-detect mode)  
**When** the command runs  
**Then** it performs `git merge-base HEAD main` and `git merge-base HEAD develop`, compares commit timestamps of the resulting SHAs, and selects the candidate with the more recent merge-base

**And** if no candidate branch resolves a merge-base SHA, the command exits non-zero with `{"status": "error", "error": "no_common_ancestor", "detail": "No shared history found with any candidate base branch."}`

**And** the existing `--base` argument continues to work; auto-detect only applies when `--base` is omitted or a new `--auto-detect-base` flag is passed

---

## Epic 2: Orchestration Script Correctness and CLI Completeness

Fix three script-level defects in `git-orchestration-ops.py` and `feature-yaml-ops.py` that cause incorrect branch routing, unstructured error messages, and missing CLI flags.

### Story 2.1: Fix branch_for_phase_write step3 routing and add branch-mismatch hard error

As a **Lens agent**,  
I want `commit-artifacts --phase finalizeplan --phase-step step3` to route to the `{featureId}` branch and emit a structured hard error on any branch mismatch,  
So that downstream bundles are committed to the correct branch and mismatches are immediately detectable.

**Acceptance Criteria:**

**Given** `branch_for_phase_write("feat-abc", "finalizeplan", "step3")` is called  
**When** the function executes  
**Then** it returns `("feat-abc", "finalizeplan_step_3_to_feature")`

**And** `branch_for_phase_write("feat-abc", "finalizeplan", "step1")` still returns `("feat-abc-plan", ...)` (no regression)

**Given** `commit-artifacts` is called and `expected_branch != current_branch`  
**When** the mismatch is detected  
**Then** the command exits non-zero and emits:
```json
{
  "status": "error",
  "error": "branch_mismatch",
  "current_branch": "<current>",
  "expected_branch": "<expected>",
  "detail": "Phase '<phase>' step '<step>' requires branch '<expected>'. Currently on '<current>'. Checkout '<expected>' before committing."
}
```

**And** no `--allow-branch-mismatch` flag is added; the mismatch is always a hard error

**And** unit tests cover: step3 routing, step1/step2 no-regression, and branch-mismatch hard error with correct JSON fields

### Story 2.2: Add --pull-request flag to feature-yaml-ops.py update

As a **Lens agent**,  
I want `feature-yaml-ops.py update --pull-request <url>` to set `links.pull_request` in feature.yaml,  
So that the PR link is stored in the governance record without a CLI error.

**Acceptance Criteria:**

**Given** I call `feature-yaml-ops.py update --feature-id X --pull-request https://github.com/org/repo/pull/42 --governance-repo Y`  
**When** the command executes  
**Then** it exits 0 and sets `links.pull_request: "https://github.com/org/repo/pull/42"` in the feature.yaml

**And** `feature-yaml-ops.py update --help` lists `--pull-request` in the `update` subcommand options

**And** no regression on existing `--phase`, `--target-repos`, `--docs-path`, and other existing flags

**And** a unit test verifies `--pull-request` sets the correct field and existing flags are unaffected
