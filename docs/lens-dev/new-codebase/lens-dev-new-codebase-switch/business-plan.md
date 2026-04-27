---
feature: lens-dev-new-codebase-switch
doc_type: business-plan
status: approved
goal: "Define the clean-room product requirements for the retained Lens switch command"
key_decisions:
  - Preserve switch as one of the 17 retained published Lens commands.
  - Treat the old published stub as the behavioral entrypoint reference while avoiding direct file copying.
  - Make feature selection explicit; never infer a target from branch names, open files, or recent activity.
  - Keep governance state read-only during switch; local session context and control-repo checkout are the only switch side effects.
open_questions: []
depends_on: [lens-dev-new-codebase-baseline]
blocks: []
updated_at: 2026-04-27T00:00:00Z
---

# Business Plan - Switch Command

## 1. Purpose

The `switch` command is the Lens context-navigation command. It lets a user move the active Lens session from one feature to another without mutating feature lifecycle state. In the reduced 17-command surface, `switch` is a retained command because users need a reliable way to orient the agent before running phase work, review work, or implementation handoff commands.

This feature plans the new-codebase `switch` behavior from clean-room inputs. The source prompt from the old codebase establishes the public stub contract: run `light-preflight.py` first, stop on failure, then load the release-module prompt. The baseline rewrite docs establish the broader invariant: every retained command must map from prompt stub to release prompt, owning skill, scripts, shared contracts, outputs, and validation without changing user-visible behavior.

## 2. User Problem

Lens can track many concurrent features across domains and services. Without a deterministic switch command, users and agents drift into ambiguous context: the current git branch, an open file, or recent discussion can appear to imply a feature, but those signals are not authoritative. That ambiguity creates wrong-branch work, stale planning context, and accidental phase execution against the wrong feature.

`switch` solves this by making feature context explicit. It lists available features from governance, asks the user to choose when no target is supplied, validates the chosen feature, switches the control repo to the feature plan branch when available, persists local domain/service context, and reports any context files that should be loaded.

## 3. Stakeholders

| Stakeholder | Need |
|---|---|
| Lens users | Choose the active feature quickly and understand the resulting phase, branch, and stale-context status. |
| Planning agents | Resolve the active feature deterministically before producing or reviewing artifacts. |
| Dev agents | Start from the correct feature branch and related context before touching target repos. |
| Lens maintainers | Preserve prompt-start parity, no-write governance behavior, and regression coverage while reducing the published command surface. |

## 4. Scope

### In Scope

- Published stub parity for `lens-switch.prompt.md`: run the shared light preflight before loading release-module logic.
- Release prompt path anchoring under `lens.core/_bmad/lens-work/`.
- Config resolution from `lens.core/_bmad/lens-work/bmadconfig.yaml` plus `.lens/governance-setup.yaml` override.
- Feature listing from `feature-index.yaml` with a domain/service inventory fallback when no features exist yet.
- Numbered menu selection with explicit user choice or clean cancellation.
- Switch execution by feature id, including feature validation, context derivation, stale warning, context file path output, local context persistence, and control-repo checkout to `{featureId}-plan`.
- Preservation of read-only governance behavior during switch.
- Regression coverage for list, selection, invalid ids, missing state, target repo summary, stale context, and branch checkout result reporting.

### Out of Scope

- Creating features, branches, domains, or services. Missing branches are reported and routed to feature initialization.
- Updating `feature.yaml`, `feature-index.yaml`, lifecycle phase, or governance docs.
- Inferring a feature from current branch, open files, recent paths, or previous conversation when the user has not selected one.
- Loading every related document by default. The command returns targeted context paths; callers load only the files that exist and matter.

## 5. Requirements

| ID | Requirement | Acceptance Criteria |
|---|---|---|
| SW-B1 | Prompt-start sync is mandatory. | The published stub runs `uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py` before any config lookup or switch script call; non-zero exit stops the command. |
| SW-B2 | Listing is governance-index driven. | `list` reads `feature-index.yaml`, excludes archived features by default, supports `all` and `archived` filters, and emits stable 1-based `num` fields. |
| SW-B3 | Empty-index environments remain orientable. | If `feature-index.yaml` is absent, `list` returns `mode: domains` with domain/service inventory and stops instead of guessing a feature. |
| SW-B4 | Selection is explicit. | With no supplied feature id, the command displays a numbered menu, accepts a number or `q`, rerenders on invalid input, and never infers from git state or editor state. |
| SW-B5 | Switching validates before acting. | The target feature id is sanitized and must exist in `feature-index.yaml`; missing or malformed targets fail with actionable errors. |
| SW-B6 | Switch side effects are bounded. | The command may write `.lens/personal/context.yaml` and attempt `git checkout {featureId}-plan`; it does not mutate governance feature state. |
| SW-B7 | Context loading is proportional. | Related features produce summary paths; `depends_on` and `blocks` features produce full tech-plan paths; missing files are skipped with warning by the caller. |
| SW-B8 | Stale context is visible but non-blocking. | Features whose `updated` timestamp is more than 30 days old return `stale: true`; the user sees an inline warning and the switch still succeeds. |
| SW-B9 | Target repo execution state is surfaced. | When feature metadata contains target repo details, list and switch responses include working branch and final PR state summary. |
| SW-B10 | Output parity is testable. | Regression tests cover feature listing, domain fallback, menu numbering, existing feature switch, context file write, target repo state, invalid identifiers, dependency context paths, and branch checkout reporting. |

## 6. Success Metrics

- `switch` remains available in the retained 17-command surface.
- A user can switch to an existing feature in one explicit action and see phase, branch, and stale-context status.
- No switch path writes governance lifecycle state.
- Existing active features continue to be listed and switchable after the rewrite.
- Focused switch tests pass and become the parity anchor for future refactors.

## 7. Clean-Room Parity Notes

The implementation must not copy old-codebase prompt or skill files directly. It should preserve observable behavior by rebuilding from documented contracts:

- The old stub's public obligation is prompt-start preflight followed by release prompt handoff.
- The baseline rewrite docs require end-to-end traceability for retained commands.
- The new switch skill owns the behavior contract, while `switch-ops.py` owns deterministic JSON operations.
- Any string or help text that still points to deprecated command names must be treated as a parity defect against the 17-command surface.
