---
feature: lens-dev-new-codebase-new-feature
doc_type: sprint-plan
status: draft
goal: "Sequence clean-room implementation of new-feature command parity"
key_decisions:
  - Deliver tests and command-surface scaffolding before write-mode implementation.
  - Keep create-domain regression coverage in every sprint because the initializer is shared.
  - Treat fetch-context as a planned hardening item unless accepted into the implementation slice.
open_questions:
  - Whether fetch-context parity is required before declaring the command complete.
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T14:15:58Z
---

# Sprint Plan — New Feature Command

## Sprint Overview

| Sprint | Goal | Stories | Complexity Total | Risks |
|---|---|---|---|---|
| 1 | Establish command surface and parity tests | NF-1.1, NF-1.2, NF-1.3 | M + M + S | Test expectations may expose missing shared helpers |
| 2 | Implement script-level feature creation | NF-2.1, NF-2.2, NF-2.3 | L + M + M | Governance write ordering and duplicate detection must remain atomic |
| 3 | Wire git handoff and command integration | NF-3.1, NF-3.2, NF-3.3 | M + M + S | Branch commands must stay delegated; express PR deferral is easy to regress |
| 4 | Harden context and release-surface parity | NF-4.1, NF-4.2, NF-4.3 | M + S + M | Scope may expand if fetch-context is considered mandatory parity |

## Sprint 1

**Goal:** Make `new-feature` visible in the new codebase as a planned command and lock down parity expectations before implementation.

| Story | Title | Estimate | Acceptance Criteria Summary | Risks |
|---|---|---:|---|---|
| NF-1.1 | Add command prompt surfaces | M | Installed stub and release prompt exist; stub runs light preflight; release prompt loads `bmad-lens-init-feature` | Path resolution must stay relative to `lens.core/` in installed use |
| NF-1.2 | Expand skill contract for new-feature | M | SKILL.md documents progressive disclosure, explicit track selection, create invocation, outputs, and failure behavior | Contract could overpromise fetch-context if not in scope |
| NF-1.3 | Add parity test skeletons | S | Tests fail red for `create`, full-track start phase, express PR deferral, invalid IDs, duplicate feature, and dry-run no-write | Existing create-domain tests must remain unchanged |

## Sprint 2

**Goal:** Implement clean-room governance feature creation and core data model parity.

| Story | Title | Estimate | Acceptance Criteria Summary | Risks |
|---|---|---:|---|---|
| NF-2.1 | Implement identity and feature data builders | L | Local slug/canonical ID resolution, featureSlug persistence, feature.yaml, summary.md, and index entry match required schema | Feature ID drift is critical |
| NF-2.2 | Implement duplicate and validation gates | M | Invalid domain/service/slug fail before writes; duplicate feature fails before writes; missing track returns explicit error | Validation order must be deterministic |
| NF-2.3 | Preserve dry-run behavior | M | Dry-run returns planned paths and command groups without creating files | Dry-run may accidentally call filesystem writes through shared helpers |

## Sprint 3

**Goal:** Restore git and lifecycle handoff parity.

| Story | Title | Estimate | Acceptance Criteria Summary | Risks |
|---|---|---:|---|---|
| NF-3.1 | Add lifecycle start-phase routing | M | Start phase resolves from lifecycle; recommended command and `/next` router command are returned | Track aliases must be handled deliberately |
| NF-3.2 | Generate control-repo and activation commands | M | Branch creation routes through git-orchestration; activation routes through switch; personal folder is preserved | Manual branch commands would break default-branch safety |
| NF-3.3 | Implement track-aware PR command behavior | S | Non-express tracks return `gh_commands`; express returns none and sets `planning_pr_created: false` | Express regression would create empty planning PRs |

## Sprint 4

**Goal:** Harden release readiness and adjacent command parity.

| Story | Title | Estimate | Acceptance Criteria Summary | Risks |
|---|---|---:|---|---|
| NF-4.1 | Implement governance git execution | M | Clean governance repo auto-commits/pushes and returns SHA; dirty repo fails before writes | Partial writes after failed git preflight would violate Lens git discipline |
| NF-4.2 | Align help/manifests if owned here | S | Module help and prompt manifests include `new-feature` consistently if this feature owns surface registration | Broader 17-command sweep may own this instead |
| NF-4.3 | Decide and implement fetch-context parity | M | Either implement `fetch-context` with tests or record it as a follow-up dependency with a blocking rationale | Downstream phases may expect context loading immediately after init |

## Cross-Sprint Dependencies

| Dependency | Blocks |
|---|---|
| NF-1.3 parity tests | NF-2.1, NF-2.2, NF-2.3 |
| NF-2.1 identity/data builders | NF-3.1, NF-3.2, NF-3.3 |
| NF-3.2 command group generation | NF-4.1 governance git execution |
| Fetch-context scope decision | NF-4.3 and final acceptance |

## Definition of Done

- The command is available through the installed prompt stub and release prompt.
- `bmad-lens-init-feature` documents the new-feature flow and script invocation.
- `init-feature-ops.py create` supports dry-run and write mode.
- Governance files are created only under the governance repo and only committed to `main` when `--execute-governance-git` succeeds.
- Control-repo branch creation is returned as a git-orchestration command, not implemented with raw checkout commands.
- Express-track feature creation defers planning PR creation.
- All focused initializer tests pass from the new-codebase root.
- No old-codebase implementation file is copied into the new codebase.
