---
feature: lens-dev-new-codebase-discover
doc_type: business-plan
status: draft
goal: "Deliver a fully specified, regression-backed rewrite of the discover command that preserves bidirectional repo inventory sync and the governance-main auto-commit exception"
key_decisions:
  - discover retains its explicit auto-commit-to-governance-main exception — this is not delegated to publish-to-governance CLI
  - Implementation channel is BMB-first for all SKILL.md and script authoring
  - Regression coverage is a hard exit criterion before dev-complete
open_questions: []
depends_on: [lens-dev-new-codebase-baseline]
blocks: []
updated_at: 2026-04-28T00:00:00Z
---

# Business Plan — Discover Command

**Feature:** `lens-dev-new-codebase-discover`
**Author:** CrisWeber
**Date:** 2026-04-28
**Split from:** `lens-dev-new-codebase-baseline` (Story 5.4)

---

## Executive Summary

The `discover` command synchronises the governance repository's `repo-inventory.yaml` against local `TargetProjects/` disk state. It closes inventory drift in both directions — cloning repos that are registered but absent locally, and registering repos that exist locally but are not yet inventoried. After reconciliation it commits and pushes any inventory changes directly to governance `main`. This direct governance commit is the only explicitly approved exception to the rule that planning commands must not write governance files outside the `publish-to-governance` CLI.

This feature delivers the `discover` command as a fully specified, independently testable work package separated from the broader baseline rewrite. Its scope is precisely bounded: rewrite the command's specification, script interface, and behavioral contracts to the new stable-surface standard, add regression coverage, and verify that the auto-commit exception is preserved without being propagated to other commands.

---

## Problem Statement

### The Inventory Drift Problem

Lens Workbench governance relies on `repo-inventory.yaml` as the authoritative registry of all cloned repositories in a workspace. Without active reconciliation, two kinds of drift emerge:

1. **Missing clones** — a repo appears in the inventory (typically after a team member added it) but has not been cloned to the current machine. This blocks `dev` and other commands that reference local working copies.
2. **Unregistered repos** — a repo exists locally (perhaps cloned manually or via another workflow) but is absent from the inventory. This creates invisible workspace state that the governance model cannot reason about.

Manual detection and correction of both forms of drift is tedious and error-prone at scale. Users working across multiple machines or teams routinely encounter stale inventory state.

### The Governance Write Exception

`discover` is the only Lens command permitted to commit directly to governance `main` outside the `publish-to-governance` CLI. This exception exists because inventory synchronisation is not a planning-phase artifact publication — it is an operational maintenance operation. The exception must be preserved verbatim and explicitly isolated so that other commands cannot inadvertently acquire the same write path.

### What This Feature Must Solve

As a split work package from `lens-dev-new-codebase-baseline`, this feature must:

1. Deliver a fully specified, tested `discover` command that satisfies the behavioral baseline established in the baseline PRD and architecture.
2. Prove that bidirectional inventory sync executes correctly in both directions.
3. Prove that the governance-main auto-commit fires only when inventory changes exist and that it produces no commit on no-op runs.
4. Prove that the auto-commit exception is contained within `discover` and not replicated in any other command.

---

## Stakeholders

| Stakeholder | Interest |
|---|---|
| Lens users (workspace operators) | A reliable `discover` command means they can onboard a new machine or clone missing repos with a single invocation — no manual inventory management required |
| Lens maintainers | Clean inventory state is a prerequisite for reliable `dev` and `complete` phases; a regression-backed discover reduces the risk of stale inventory state accumulating silently |
| Reviewers / quality gate | Need confidence that the auto-commit exception is correctly scoped and not accidentally spread during the rewrite |
| Future contributors | A clearly bounded command with documented behavioral contracts and regression tests is safe to modify without unintended side effects |

---

## Business Context

`discover` is command 16 of the 17 retained published commands in the `lens-work` stable-surface rewrite. Its behavioral contract is well understood from the old codebase. The rewrite challenge is not functionality invention — it is specification precision and regression coverage.

Two specific behaviors make `discover` distinct from all other commands:

1. **Bidirectional sync** — the command resolves drift in both directions simultaneously, not just one.
2. **Governance-main auto-commit** — uniquely, this command is allowed to commit the `repo-inventory.yaml` change directly to governance `main` without going through the `publish-to-governance` CLI. Every other planning command that touches governance artifacts does so only via that CLI.

The old codebase established both behaviors. The new codebase must preserve them exactly. The isolation constraint — no spreading of the auto-commit path — is a governance axiom for this domain.

---

## Scope

### In Scope

- Specification and review of the `bmad-lens-discover` SKILL.md behavioral contract
- The `discover-ops.py` script interface: `scan`, `add-entry`, and `validate` subcommands
- Conditional auto-commit logic: commit and push to governance `main` only when inventory file is modified
- No-op reporting: clean output without side effects when disk and inventory are already in sync
- Regression tests for all three behavioral scenarios: bidirectional sync, conditional auto-commit, and no-op
- BMB-first authoring of all lens-work skill and script changes
- Prompt stub (`lens-discover.prompt.md`) and release prompt wiring

### Out of Scope

- Changes to `publish-to-governance` CLI or any other command's governance write path
- Introducing new inventory fields beyond what the current `repo-inventory.yaml` schema supports
- UI or dashboard integrations for inventory display
- Any multi-team or cross-workspace inventory federation scenarios

---

## Success Criteria

| # | Criterion | Validation |
|---|---|---|
| SC-1 | `discover` identifies any repo listed in `repo-inventory.yaml` that is absent from `TargetProjects/` on disk and routes it into the clone path | Script-level regression test (`test-discover-ops.py`): populate inventory with a mock repo entry, assert the missing repo is detected and the clone action is requested/emitted |
| SC-2 | `discover` adds any repo found on disk to `repo-inventory.yaml` when it is not already registered | Regression test: create an untracked local git repo, assert add-entry is triggered and inventory is updated |
| SC-3 | Inventory changes are committed and pushed to governance `main` exactly once after reconciliation | Script-level regression test (`test-discover-ops.py`): assert an inventory-modified run emits the commit-required / commit-path behavior exactly once |
| SC-4 | A run with no drift produces no commit and reports `[discover] Nothing to do` | Script-level regression test (`test-discover-ops.py`): run with disk and inventory in sync; assert no commit-path is requested and correct output is emitted |
| SC-5 | No other command in the 17-command surface acquires or references the governance-main direct-commit path | Architecture review: audit `publish-to-governance` call sites in all other command SKILL.md files |
| SC-6 | The SKILL.md headless and dry-run modes function correctly | Regression test: verify `--dry-run` produces no file mutations and no git commits |
| SC-7 | All script-level regression tests in `test-discover-ops.py` pass in the control repo CI before `dev-complete` milestone | CI gate: `uv run --with pytest pytest lens.core/_bmad/lens-work/skills/bmad-lens-discover/scripts/tests/ -q` |

---

## Risks and Mitigations

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Auto-commit fires even when no inventory entries changed, creating noisy governance history | Medium | High | Implement an explicit diff check before `git commit`; add a no-op regression test as a hard gate |
| Auto-commit exception pattern is replicated in a different command during a later rewrite package | Low | High | Architectural review checklist item: verify no other command SKILL.md references direct governance commit outside publish-to-governance |
| `discover-ops.py` path resolution breaks on Windows due to drive-letter differences in path comparisons | Medium | Medium | Use `Path.resolve()` for all path comparisons; add a regression test with TargetProjects-relative paths on different root structures |
| Regression tests require a real git repo, making them slow or fragile in CI | Low | Low | Use `tempfile.TemporaryDirectory` with local `git init` for all tests — already established in the existing test file |
| Headless mode bypasses confirmation, silently cloning unexpected repos in automated contexts | Low | Medium | Document headless mode clearly in SKILL.md; add a dry-run-first recommendation in the onboarding flow |

---

## Definition of Done

- [ ] SKILL.md is authored via BMB and covers all behavioral paths (interactive, headless, dry-run, no-op, auto-commit)
- [ ] `discover-ops.py` exposes `scan`, `add-entry`, and `validate` subcommands with stable JSON output
- [ ] Conditional auto-commit is explicit: commit only when `repo-inventory.yaml` is actually modified
- [ ] Regression tests exist for: bidirectional sync (both directions independently), no-op, auto-commit conditional, dry-run no-mutation
- [ ] All tests pass via `uv run --with pytest` in CI
- [ ] Prompt stub and release prompt chain is verified end-to-end
- [ ] Architecture review confirms auto-commit exception is not present in any other command
