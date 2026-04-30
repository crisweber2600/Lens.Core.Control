---
feature: lens-dev-new-codebase-dev
doc_type: tech-plan
status: draft
goal: "Specify the technical design of the dev conductor rewrite: prompt chain, entry hook, internal dependency chain, checkpoint semantics, and final PR model."
key_decisions:
  - "Three-hop prompt chain: .github stub → lens.core release prompt → bmad-lens-dev SKILL.md. Same shape as all other retained commands."
  - "bmad-lens-git-orchestration owns the target repo branch preparation, per-task commit mechanics, and final PR. The conductor routes all writes through this dependency exclusively."
  - "dev-session.yaml remains the single authoritative checkpoint store. The conductor reads and writes it. No migration is introduced."
  - "Publish-before-author entry hook: the conductor validates that finalizeplan artifacts exist and the feature phase is finalizeplan-complete before entering the implementation loop."
open_questions:
  - "Does the publish-before-author gate check feature.yaml phase == 'finalizeplan-complete', artifact presence in governance docs path, or both? (Recommendation: both, in that order.)"
  - "Is the target repo path resolved from feature.yaml target_repos field or from the workspace inventory? (If target_repos is empty, interactive confirmation is required.)"
depends_on:
  - lens-dev-new-codebase-finalizeplan
blocks: []
updated_at: '2026-04-30T00:00:00Z'
---

# Technical Plan — Dev Command Rewrite

## Overview

The `dev` conductor is a thin orchestration skill that receives a feature context, validates
preconditions, and executes an ordered task loop through story files in the governance docs
path. It depends on six internal skills and owns three behavioral contracts: target-repo write
isolation, per-task commit atomicity, and checkpoint resumability. The rewrite must preserve
all three contracts without schema changes to `dev-session.yaml` or changes to the
user-visible command surface.

---

## Prompt Chain

```
.github/prompts/lens-dev.prompt.md         (stub — runs light-preflight.py, then delegates)
  └── lens.core/_bmad/lens-work/prompts/lens-dev.prompt.md    (thin redirect)
        └── lens.core/_bmad/lens-work/skills/bmad-lens-dev/SKILL.md   (full conductor)
```

The stub pattern is identical to all other retained-command stubs: run `light-preflight.py`,
then load the full prompt from the release module path. The stub must not contain any
business logic.

---

## Entry Hook: Publish-Before-Author

Before the dev conductor begins the implementation loop, it must verify that the feature is
ready for implementation:

1. Load `feature.yaml` for the active feature (resolved from the governance path).
2. Confirm `feature.yaml` phase is `finalizeplan-complete`.
3. Confirm `epics.md` and `stories.md` exist in the feature's governance docs path.
4. Confirm `sprint-status.yaml` exists and has at least one story with `status: not-started`.
5. If any check fails, surface a clear error message naming the failed gate and stop.

This hook is the hard boundary between planning and implementation. It prevents the dev
conductor from running against an incomplete plan or an unpublished finalizeplan bundle.

---

## Conductor Chain

```
bmad-lens-dev (SKILL.md)
  1. Load feature context (feature.yaml, governance docs path)
  2. Run publish-before-author entry hook (gate: finalizeplan-complete + artifact presence)
  3. Resolve target repo path (from feature.yaml target_repos or interactive confirmation)
  4. Load or create dev-session.yaml (resume vs. fresh start)
  5. prepare-dev-branch via bmad-lens-git-orchestration
  6. Load constitution via bmad-lens-constitution
  7. For each story in sprint-status.yaml (not-started or in-progress):
       a. Load story file from governance docs stories/ path
       b. Execute tasks via subagent (implementation work)
       c. Validate implementation (bmad-lens-adversarial-review code review gate)
       d. Per-task commit via bmad-lens-git-orchestration
       e. Update dev-session.yaml checkpoint (mark story complete, record commit ref)
       f. Do not modify sprint-status.yaml during dev; track story progress only in dev-session.yaml, and route any governance status updates through a separate governance-writing workflow
  8. Open final target-repo PR via bmad-lens-git-orchestration
  9. Write dev-session.yaml status: complete with final PR reference
```

---

## Internal Dependencies

| Dependency | Role | Write Access |
|---|---|---|
| `bmad-lens-git-orchestration` | Branch prep, per-task commits, final PR | Target repo only |
| `bmad-lens-constitution` | Governance gate at dev start | None (read governance) |
| `bmad-lens-feature-yaml` | Phase awareness, feature state reads | None (read only) |
| `bmad-lens-adversarial-review` | Code review gate per-story | None (review output only) |
| `bmad-lens-git-state` | Read-only git truth source for dev session | None (read only) |
| `bmad-lens-target-repo` | Target repo path resolution and provisioning | None (read only) |

All code writes are exclusively routed through `bmad-lens-git-orchestration`. No other
dependency has write access to the target repo. No dependency has write access to the
control repo at any point.

---

## dev-session.yaml Contract

The dev conductor reads and writes `dev-session.yaml` as the checkpoint store.

| Requirement | Specification |
|---|---|
| Schema compatibility | No schema changes from old-codebase format. Existing fields must remain valid. |
| Fresh start | Create a new session with: feature context, sprint-plan reference, status: in-progress |
| Resume | Load existing session, skip stories with status: complete, continue from first not-started |
| Checkpoint write | After each per-task commit: update story status, record commit SHA, write session |
| Interruption safety | The last written checkpoint is always safe; no partial-task state is persisted |
| Session close | Write status: complete with final PR number and timestamp |

The conductor must detect an existing `dev-session.yaml` and confirm it matches the active
feature before resuming. If the session references a different feature, stop with an error.

---

## Write Isolation Contract

The dev conductor enforces a strict write isolation rule at all times:

- **Target repo**: Code changes, implementation artifacts, task-level commits, final PR.
- **Control repo**: Read-only. Governance docs, planning artifacts, sprint-status.yaml,
  story files are referenced but never modified during a dev session.
- **Release repo**: Not touched by dev at any point.

This constraint is enforced structurally: all write operations are routed through
`bmad-lens-git-orchestration`, which is scoped to the target repo branch. The conductor
does not invoke any file-write tool directly against the control or release repo paths.

---

## Per-Task Commit Semantics

Each story task that completes successfully produces a single atomic commit to the target
repo branch:

| Property | Specification |
|---|---|
| Trigger | Immediately after a task passes the code review gate |
| Scope | Only files changed by that task (no cross-task bundling) |
| Message format | `[{featureId}] {storyId}: {task title}` |
| History | Per-task commits are not squashed or rebased during the session |
| Recovery | If a task fails review, no commit is made; the task is retried or flagged |

---

## Foundation Layer Validation

Before the rewrite implementation begins, the following preconditions must be confirmed
present in the target project:

- [ ] `bmad-lens-git-orchestration` skill is present
- [ ] `bmad-lens-constitution` skill is present
- [ ] `bmad-lens-adversarial-review` skill is present
- [ ] `bmad-lens-feature-yaml` skill is present
- [ ] `bmad-lens-git-state` skill is present
- [ ] `bmad-lens-target-repo` skill is present
- [ ] `dev-session.yaml` schema definition matches old-codebase shape
- [ ] Sprint-status and story file discovery paths are confirmed

If any dependency is absent, it must be flagged as a blocking gap before Slice 2 work begins.

---

## Workstreams

| Workstream | Focus | Blocking Dependencies |
|---|---|---|
| 1 — Prompt chain | Stub, redirect, module.yaml registration | None |
| 2 — Entry hook | Publish-before-author gate, constitution check, target repo resolution | `bmad-lens-feature-yaml`, `bmad-lens-constitution` |
| 3 — Checkpoint semantics | dev-session.yaml compat, resume logic, session close | dev-session.yaml schema definition |
| 4 — Per-task commit | Atomic commits, write isolation routing | `bmad-lens-git-orchestration` |
| 5 — Final PR | Single PR, session complete write | `bmad-lens-git-orchestration` |

Workstreams 2–5 are all gated on Workstream 1 (prompt chain confirmed valid). Workstreams
3–5 are additionally gated on foundation layer validation passing.
