---
feature: lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be
doc_type: business-plan
status: draft
track: express
updated_at: "2026-05-03T23:40:00Z"
depends_on: []
blocks: []
key_decisions: []
open_questions: []
---

# Business Plan — Bugbash: Environment, Orchestration and Tooling Fixes

## Problem

Six distinct operational bugs were observed across multiple Lens agent sessions. They degrade developer experience, waste turns, corrupt artifacts, and introduce ambiguous git state. Left unaddressed they will continue to surface in every bug-fixer and expressplan run.

| Bug | Severity | Symptom |
|-----|----------|---------|
| Agent uses `rg` despite AGENTS.md | Minor | Every `rg` invocation fails before fallback to `grep`, wasting a turn |
| `commit-artifacts` step3 wrong branch | High | FinalizePlan step3 routes commit to wrong git branch, risking data loss |
| `commit-artifacts` no branch mismatch warning | Medium | Silently proceeds when resolved branch ≠ current branch; user unaware |
| `feature-yaml-ops.py` missing `--pull-request` flag | Medium | CLI error on any call that attempts PR field update |
| `gh pr create` no history check | Minor | PR occasionally targets wrong base due to diverged history |
| PowerShell bulk-replace injects `\r\n` tokens | Medium | Prompt files contain literal backslash-r-backslash-n text after replace |

## Target Users

- **Lens agent** (primary) — the automated agent that executes Lens lifecycle commands; bugs surface as runtime failures.
- **Developer / DX user** — the human running Lens sessions; experiences wasted turns, silent failures, corrupted artifacts.

## Business Goals

1. **Zero avoidable retries** — each bug, once fixed, must not require a fallback or retry turn.
2. **Artifact integrity** — no committed artifact should contain literal escape sequences or point at the wrong branch.
3. **Transparent orchestration** — branch routing decisions must be logged; mismatches must surface as warnings.
4. **CLI completeness** — all `feature-yaml-ops.py` flags documented in the skill spec must exist in code.

## Scope

**In scope:**
- Fix `rg` usage: replace with `grep` in all agent-executed shell commands
- Fix `commit-artifacts` step3 branch routing
- Add branch-mismatch warning to `commit-artifacts`
- Add `--pull-request` flag to `feature-yaml-ops.py`
- Add history check before `gh pr create`
- Fix PowerShell bulk-replace `\r\n` injection (convert to Python replacement)

**Out of scope:**
- Refactoring other CLI commands not listed
- Changing git topology or branch naming conventions
- Upgrading dependencies

## Dependencies

- Access to `_bmad/lens-work/scripts/` for `commit-artifacts`, `feature-yaml-ops.py` and related scripts
- Access to `_bmad/lens-work/skills/` for any agent prompt files referencing `rg`
- PowerShell→Python migration affects any bulk-replace helper used in skill workflows

## Risks

| Risk | Likelihood | Mitigation |
|------|-----------|------------|
| Step3 branch routing fix breaks other phases | Low | Read all `commit-artifacts` callers and add regression test cases |
| Python replacement changes line endings unexpectedly | Low | Explicitly preserve original line endings |
| Missing `--pull-request` flag already has workarounds that break after fix | Low | Check all callers of `feature-yaml-ops.py` for `--pull-request` |

## Success Criteria

- `rg` no longer appears in any agent shell command in lens-work skills or scripts
- `commit-artifacts --phase finalizeplan --phase-step step3` commits to the correct branch
- A branch mismatch triggers a logged warning and user prompt before proceeding
- `feature-yaml-ops.py --pull-request <value>` executes without CLI error
- `gh pr create` verifies shared history before targeting base
- Prompt files updated via bulk-replace contain no literal `\r\n` text
