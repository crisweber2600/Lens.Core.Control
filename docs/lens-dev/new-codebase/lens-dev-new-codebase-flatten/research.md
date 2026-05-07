---
feature: lens-dev-new-codebase-flatten
doc_type: research
status: draft
goal: "Establish a codebase-grounded design baseline for introducing a governance-controlled flat control-repo workflow mode without relying on outside research"
key_decisions:
  - Current control-repo branch and PR behavior is enforced in multiple implementation layers, not just one git helper.
  - The existing session context already persists `domain`, `service`, and `feature_id`; the requested flat mode mainly changes branch/PR coupling, not basic context persistence.
  - The cleanest existing governance-level configuration surface is the constitution hierarchy, but the constitution resolver currently ignores unknown machine-readable keys.
  - Product scope should treat flat mode as a full control-repo workflow mode, not as a planning-only user-facing variant.
  - Delivery should still be phased: shared mode resolution plus early control flows first, then finalizeplan/dev/complete parity.
  - Any flat mode that applies beyond early planning must account for dev-phase and git-state logic that currently require `{featureId}-dev` and `{featureId}-plan` branches.
open_questions:
  - Does the user requirement mean a literal `feature:` field in `.lens/personal/context.yaml`, or is the existing `feature_id:` field acceptable?
  - If governance mode changes while a feature is already in progress, should existing branch topology be preserved or migrated?
  - Should review artifacts remain mandatory in flat mode even when PR creation is removed?
  - What is the smallest implementation milestone that still feels coherent to users before the full-lifecycle flat mode is complete?
depends_on: [brainstorm]
blocks: []
updated_at: 2026-05-07T00:00:00Z
---

# Research — Governance-Controlled Flat Mode (Codebase Grounding)

**Session date:** 2026-05-07  
**Research method:** local codebase inspection only; no external web or market research  
**Evidence sources:**
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-init-feature/scripts/init-feature-ops.py`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-switch/scripts/switch-ops.py`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-git-orchestration/scripts/git-orchestration-ops.py`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/scripts/preflight.py`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-git-state/scripts/git-state-ops.py`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-constitution/scripts/constitution-ops.py`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-dev/SKILL.md`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-init-feature/scripts/tests/test-init-feature-ops.py`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-switch/scripts/tests/test-switch-ops.py`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/scripts/tests/test-preflight.py`
- `TargetProjects/lens/lens-governance/constitutions/org/constitution.md`
- `TargetProjects/lens/lens-governance/constitutions/lens-dev/new-codebase/constitution.md`
- `TargetProjects/lens/lens-governance/features/lens-dev/domain.yaml`
- `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/service.yaml`

---

## 1. Executive Finding

The user request matches a real codebase pain point, but the current implementation does **not** treat control-repo branching as one isolated policy. It is a cross-cutting contract enforced in:

1. feature initialization,
2. feature switching,
3. control-repo git orchestration,
4. preflight sync behavior,
5. git-state diagnostics,
6. and dev-phase runtime expectations.

That means a flat mode is feasible, but only if it becomes an explicit, centrally resolved workflow mode that all of those layers read consistently.

### 1.1 Scope Refinement Verdict

The codebase evidence supports this refined answer to the open question:

- The **product contract** should be a **full control-repo workflow mode**. The user request is about how Lens works in the control repo overall, not only how early planning feels.
- A **planning-only user-facing mode** would be misleading because later lifecycle commands still depend on the same control branch topology and would reintroduce branch/PR behavior unexpectedly.
- The **implementation plan** should still be phased. Early work can land shared mode resolution and the first branch-coupled entry points, but the feature should be described as incomplete until finalizeplan/dev/complete also honor the selected mode.

In short: **full-lifecycle as the requirement, phased delivery as the implementation strategy**.

---

## 2. What The Codebase Does Today

### 2.1 Feature initialization assumes control branches exist

`init-feature-ops.py create` writes governance state and then emits control-repo follow-up commands when a separate control repo is provided:

- `remaining_commands` includes `git-orchestration-ops.py create-feature-branches`
- `remaining_commands` then includes `switch-ops.py switch`
- `planning_pr_followup_commands` includes deferred `gh pr create --head {featureId}-plan --base {featureId}`

The implementation therefore assumes that:

- a control repo feature base branch exists,
- a `{featureId}-plan` branch exists,
- planning PRs are part of the control workflow,
- and activation should land on the plan branch.

This is not just prose. Tests explicitly assert:

- `feature-index.yaml` records `plan_branch = {featureId}-plan`
- planning PR creation is deferred but still emitted as a `gh pr create` follow-up command

### 2.2 Branch creation is hard-coded as a 3-branch control topology

`git-orchestration-ops.py` declares the control feature model directly in code and implements `cmd_create_feature_branches` by creating:

- `{featureId}`
- `{featureId}-plan`
- `{featureId}-dev`

from the control repo default branch.

Other subcommands then depend on that topology:

- `cmd_commit_artifacts` refuses phase writes when required control branches are missing
- `cmd_validate_phase_start` fails if the required branches do not exist
- `cmd_merge_plan` requires both `{featureId}` and `{featureId}-plan`
- `cmd_cleanup_branch` assumes step branches are deleted and the next branch is checked out and pulled

This confirms that control-repo branching is an implementation contract, not just a planning convention.

### 2.3 Switching features already writes context, but it is coupled to plan-branch checkout

`switch-ops.py` already persists the user context the request wants:

- `domain`
- `service`
- `feature_id`

via `write_context_yaml(...)`, and `cmd_switch(...)` passes the active feature id into that file.

The important nuance is that `cmd_switch(...)` also always:

- computes `plan_branch = {featureId}-plan`
- attempts `git checkout {featureId}-plan`
- reports `branch_switched`, `checked_out_branch`, and `branch_error`

The tests confirm this is expected behavior:

- successful switch writes `feature_id` to context and reports `checked_out_branch == auth-login-plan`
- missing branch returns `branch_not_found` and the message `Run /new-feature to initialize branches.`

**Implication:** the "put domain, service, feature into context.yaml" part is mostly already satisfied. The real change is removing control-branch dependence from the switch flow. The only unresolved detail is whether the requirement wants a literal `feature:` key instead of the existing `feature_id:` schema.

### 2.4 Preflight also encodes control-branch assumptions

`preflight.py` contains branch cleanup logic specific to the control topology. When a control feature branch no longer exists remotely, `pre_request_sync(...)` can fall back to `{featureId}-dev`.

The test `test_pre_request_sync_switches_to_dev_branch_when_feature_remote_was_deleted` proves this behavior is intentional: it expects the sync decision to switch from the deleted feature branch to the `-dev` branch.

**Implication:** flat mode cannot be implemented only in `/new-feature` and `/switch`. Preflight sync behavior must also become mode-aware.

### 2.5 Git-state and dev-phase logic also depend on branch topology

`git-state-ops.py` classifies feature branches by role:

- base branch
- `-plan` branch
- `-dev` branch

and then treats branch presence as lifecycle evidence:

- planning phases expect a plan branch
- `dev` expects a dev branch
- `dev` plus an open plan branch is considered a discrepancy

The `lens-dev` skill goes even further. Its contract requires the control repo to activate `{feature_id}-dev` before reading story files or sprint state, and it treats failure to do so as a hard-stop error.

**Implication:** if flat mode is intended for the whole lifecycle, not just planning, then `lens-dev` and `lens-git-state` are part of the required design surface. If flat mode is planning-only, the design must say that explicitly.

### 2.6 There is already a no-PR precedent, but only for target repos

The current codebase already supports a direct-write mode for **target repos**, not the control repo:

- `cmd_prepare_dev_branch(...)` supports `mode == direct-default`
- in that mode it returns `working_branch = default_branch` and `requires_pr = False`
- `switch-ops.py normalize_target_repo_state(...)` translates `dev_branch_mode == direct-default` into `final_pr_state = not-required`

This is useful because it proves the codebase already has a local vocabulary for "skip branch/PR ceremony". It just has not been applied to the control repo yet.

---

## 3. Where A Governance-Level Switch Can Live

### 3.1 Constitution hierarchy is the best existing machine-readable governance surface

The governance repo already has a centrally resolved hierarchy:

- org constitution
- domain constitution
- service constitution
- optional repo constitution

`constitution-ops.py` reads and merges those levels and returns one `resolved_constitution` payload. This is already the repo's machine-readable governance channel.

That makes constitutions the strongest fit for a workflow-mode flag because they are:

- governance-owned,
- already centrally resolved,
- already hierarchical,
- already used by multiple lifecycle skills.

### 3.2 But the constitution resolver must be extended first

Right now `constitution-ops.py` only recognizes this fixed frontmatter key set:

- `permitted_tracks`
- `required_artifacts`
- `gate_mode`
- `sensing_gate_mode`
- `additional_review_participants`
- `enforce_stories`
- `enforce_review`

Unknown keys are deliberately collected into `_unknown_keys` and ignored during merge.

**Implication:** if we add something like `control_repo_workflow_mode: flat`, it will currently be ignored everywhere until:

1. `KNOWN_CONSTITUTION_KEYS` is extended,
2. merge behavior is defined,
3. and downstream callers start reading the new resolved field.

### 3.3 `service.yaml` and `domain.yaml` are weaker fits today

The active governance files are intentionally minimal:

- `domain.yaml` contains identity/status metadata
- `service.yaml` contains identity/status metadata

No current shared resolver merges workflow settings from those files, and no code path in this research slice reads them as a policy channel.

**Implication:** storing the switch in `service.yaml` or `domain.yaml` would require inventing a new configuration resolution surface. Storing it in constitutions reuses an existing one.

### 3.4 Universal scope means org-level, not service-level

The user requirement says the switch should be set at the governance repo level so it is universal.

In the current hierarchy, the only truly universal level is:

- `constitutions/org/constitution.md`

The active service constitution (`lens-dev/new-codebase`) is service-wide, but not governance-wide.

**Recommendation:** if the intent is truly universal across the whole governance repo, put the new setting at the org constitution level. If the intent is only universal for Lens new-codebase work, service-level constitution is still viable but is a narrower scope than requested.

---

## 4. Feasibility Assessment

### 4.1 Feasible, but only as a shared mode resolution

The codebase supports the idea conceptually, but not with one small patch. A workable design needs one resolved setting that all relevant layers can read.

### 4.2 Likely affected implementation surfaces

| Surface | Current behavior | Flat-mode implication |
|---|---|---|
| `constitution-ops.py` | Ignores unknown policy keys | Add workflow-mode key and merge rule |
| `init-feature-ops.py` | Emits branch-creation and planning-PR follow-up commands | Suppress or replace those follow-ups in flat mode |
| `switch-ops.py` | Always tries to check out `{featureId}-plan` | Write context without checkout in flat mode |
| `git-orchestration-ops.py` | Requires 3 control branches for multiple operations | Either short-circuit control-branch operations or make them mode-aware |
| `preflight.py` | Contains feature/dev branch cleanup behavior | Skip branch topology assumptions in flat mode |
| `git-state-ops.py` | Treats plan/dev branch presence as lifecycle evidence | Needs alternate expectations in flat mode |
| `lens-dev` | Requires `{feature_id}-dev` activation | Either remain structured-only or gain flat-mode semantics |

### 4.3 Full-lifecycle flat mode is the correct requirement boundary

If the request is implemented exactly as stated, the correct boundary is "no control-repo branching or PR enforcement in flat mode across the lifecycle," not merely "planning feels lighter." That is the only interpretation that avoids a split mental model where:

- `/new-feature` and `/switch` behave flat,
- but FinalizePlan, Dev, or Complete later insist on control branches and PRs.

From the inspected code, a planning-only user-facing mode would be internally inconsistent because the later lifecycle still depends on the same topology assumptions.

### 4.4 Full-lifecycle flat mode is materially larger to implement

Treating flat mode as the requirement boundary does not make it small. To satisfy that contract, the design must also change:

- `lens-dev` branch activation rules
- git-state discrepancy logic
- finalize/cleanup expectations
- any skill or test that assumes `{featureId}-dev` is the authoritative docs branch

That is a larger feature than just removing planning PRs.

### 4.5 Recommended delivery shape

The most defensible implementation shape is:

1. **Phase 1:** add governance-mode resolution and make `init-feature`, `switch`, `preflight`, and shared git-state/orchestration layers mode-aware.
2. **Phase 2:** extend the same mode through `finalizeplan`, `dev`, `complete`, and related cleanup/PR logic.

This keeps the product definition honest while still letting implementation land incrementally.

---

## 5. Recommended Technical Direction

### 5.1 Add a machine-readable governance field

Add a new constitution field such as:

```yaml
control_repo_workflow_mode: structured | flat
```

Suggested semantics:

- `structured` = current behavior
- `flat` = no control-repo feature branch creation, no control-repo PR enforcement, no control-repo checkout requirement during switch

### 5.2 Resolve it centrally

Extend `constitution-ops.py` so `resolved_constitution` includes the new field. Do not let each skill invent its own fallback or file read.

### 5.3 Keep `feature_id` in context unless there is a deliberate schema change

Current code and tests already rely on `feature_id` in `.lens/personal/context.yaml`.

If the user requirement meant the literal key `feature`, the safest path is:

- keep `feature_id` for compatibility
- optionally add `feature` as an alias

Replacing `feature_id` outright would be a schema change, not just a workflow change.

### 5.4 Reuse existing no-PR vocabulary where possible

The target-repo branch prep code already uses a direct mode (`direct-default`) that implies no PR is required. That precedent can inform naming and payload design for the control repo.

### 5.5 Keep target-project branching separate

Current code clearly separates control-repo topology from target-repo branch modes. The new flat mode should preserve that separation. Otherwise the feature scope expands into an unrelated target-repo workflow redesign.

---

## 6. Concrete Design Constraints For Next Phase

Any implementation plan should preserve these constraints from the current codebase:

1. Governance remains the source of truth for mode resolution.
2. One shared resolver decides the mode; commands do not guess independently.
3. Structured mode must remain behaviorally unchanged.
4. Context persistence should remain available even when branch checkout is skipped.
5. Flat mode must not silently leave branch-dependent commands in a half-broken state.

---

## 7. Recommended Follow-On For Product Brief

The product brief should frame this as:

- a workflow-fit feature for new Lens adopters,
- with one governance-controlled operating mode,
- preserving existing structured behavior,
- and explicitly deciding whether flat mode is planning-only or full-lifecycle.

The main technical unknown is no longer "is this possible?" The main unknown is scope: how far into the lifecycle the flat control-repo behavior should extend in the first release.