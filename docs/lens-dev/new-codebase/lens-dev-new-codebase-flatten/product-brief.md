---
feature: lens-dev-new-codebase-flatten
doc_type: product-brief
status: draft
goal: "Define a governance-controlled control-repo workflow mode that preserves Lens lifecycle structure while allowing teams to choose either the current structured branch/PR model or a flat control-repo mode"
key_decisions:
  - The feature is a full control-repo workflow mode, not a planning-only shortcut.
  - Structured mode remains the default; flat mode is an explicit governance-controlled opt-in.
  - The scope change applies to the control repo only; governance publication and target-repo branch strategy remain separate concerns.
  - The strongest current policy surface for the setting is constitution resolution, which will need to recognize a new machine-readable workflow-mode key.
  - Delivery can be phased, but the user-facing contract must stay honest about later lifecycle behavior that still depends on control-branch topology.
open_questions:
  - Should flat mode preserve all review artifacts and review gates while removing only control-repo branch and PR enforcement?
  - If a governance repo changes workflow mode while features are already in progress, should Lens preserve existing branch topology or require migration rules?
  - Should `.lens/personal/context.yaml` keep only `feature_id`, or add a compatibility alias such as `feature` for readability?
depends_on: [brainstorm, research]
blocks: []
updated_at: 2026-05-07T00:00:00Z
---

# Product Brief — Governance-Controlled Flat Mode For Control Repo Workflow

## 1. Vision

Lens should support two legitimate control-repo operating styles without splitting into two different products:

- **Structured mode:** the current control-repo workflow with feature, plan, and dev branches plus PR-driven handoffs.
- **Flat mode:** a lighter control-repo workflow with direct context persistence and no control-repo branch or PR enforcement.

The purpose of this feature is not to weaken governance. It is to let teams adopt Lens lifecycle structure at the process level they can actually sustain. Teams that want the existing discipline keep it unchanged. Teams that are still building process maturity can use the same lifecycle model without control-repo branch choreography becoming a barrier to adoption.

The product outcome is a single governance-controlled workflow mode that changes how Lens behaves in the control repo while preserving feature identity, lifecycle phases, and downstream planning artifacts.

---

## 2. Problem Statement

The current new-codebase implementation assumes that control-repo branches and PRs are part of the Lens contract across multiple commands, not just one git helper. Code inspection shows this coupling in:

- `init-feature-ops.py`, which emits control-branch creation and planning PR follow-up commands
- `switch-ops.py`, which writes context but always tries to check out `{featureId}-plan`
- `git-orchestration-ops.py`, which hard-codes `{featureId}`, `{featureId}-plan`, and `{featureId}-dev`
- `preflight.py`, which includes fallback logic based on deleted feature branches and `-dev` branches
- `git-state-ops.py`, which treats plan/dev branches as lifecycle evidence
- `lens-dev`, which hard-stops unless `{featureId}-dev` is activated

This creates a real workflow-fit problem for small teams, early Lens adopters, and tightly coupled multi-service efforts. Those teams often want the feature context, phase discipline, and governance visibility that Lens provides, but they do not want control-repo branch choreography and PR ceremony for every stage of planning and execution.

Today Lens does not offer that choice. The result is that users who are willing to adopt the lifecycle may still bounce off the tool because the control-repo workflow is more rigid than their team can support.

---

## 3. Target Users

| User | Current Pain | Desired Outcome |
|---|---|---|
| **New Lens adopters** | Control-repo branches and PRs feel like up-front ceremony before the team has internalized the lifecycle | Start using Lens with lighter control-repo process while keeping feature context and phase structure |
| **Small or fast-moving teams** | Control-repo coordination overhead is high relative to team size | Work directly in the control repo while still producing the required planning and review artifacts |
| **Multi-service teams** | Switching one control context across several related repos feels harder than the underlying work | Keep reliable `domain`, `service`, and `feature_id` context without forced control-repo branch checkout |
| **Disciplined review-oriented teams** | Risk of regression if a lighter mode weakens existing rigor | Retain current structured behavior unchanged as the default mode |
| **Lens maintainers** | Branch/PR assumptions are currently scattered across many layers | Centralize workflow-mode resolution so behavior is testable and supportable |

---

## 4. Product Goals

### G1 — Governance-Controlled Workflow Choice

Introduce one centrally resolved control-repo workflow mode that can be set by governance and read consistently across Lens commands. The choice must be universal for the governed installation, not a per-user override.

### G2 — Preserve Structured Mode As-Is

The current branch-and-PR control-repo workflow remains the default behavior and must not regress. Existing teams using structured mode should see no change in branch topology, PR flow, or command expectations.

### G3 — Make Flat Mode A Real Control-Repo Mode

Flat mode must mean more than a lighter `/new-feature` experience. It must remove control-repo branch and PR enforcement as a workflow assumption, while preserving feature identity, lifecycle phase progression, and control-repo artifacts.

### G4 — Keep Control Scope Separate From Other Concerns

This feature changes only the control-repo workflow. It does not redesign governance publication, target-repo branch strategy, or the meaning of lifecycle artifacts.

### G5 — Centralize Policy Resolution

The workflow mode must be resolved once through the existing governance/policy path and reused everywhere. Commands should not guess the mode independently or duplicate fallback logic.

### G6 — Support Honest Incremental Delivery

Implementation may land in phases, but the product definition must state that the full requirement reaches through later lifecycle behavior as well. Users should not be told flat mode is complete if finalizeplan, dev, or complete still require structured-only control-branch semantics.

---

## 5. In Scope

- Add one governance-controlled control-repo workflow mode with at least `structured` and `flat` semantics.
- Extend the current governance resolution path so the mode is machine-readable and available to downstream commands.
- Update `new-feature`, `switch`, `preflight`, and shared control git orchestration/state logic to behave correctly in flat mode.
- Preserve `.lens/personal/context.yaml` as the active user context mechanism, including `domain`, `service`, and feature identity.
- Define expected behavior for later lifecycle commands that currently depend on `{featureId}-plan` and `{featureId}-dev`.
- Document the command behavior matrix for structured vs flat control-repo workflows.
- Add regression coverage proving structured mode remains unchanged and flat mode suppresses control-repo branch/PR assumptions where required.

---

## 6. Out of Scope

- Changing governance repo publication flow or replacing governance as the policy source of truth.
- Redesigning target-project branch modes or target-repo PR behavior.
- Replacing lifecycle phases, feature identifiers, or planning artifact types.
- Introducing per-user workflow overrides that conflict with governance-level behavior.
- Changing feature ownership or docs storage conventions.
- Requiring teams in structured mode to adopt any new workflow steps.

---

## 7. Key Product Decisions

### 7.1 Product Boundary

This feature is a **full control-repo workflow mode**, not a planning-only variant. The research evidence shows that later lifecycle stages still depend on the same control-branch model, so describing flat mode as complete before those stages are addressed would create a misleading contract.

### 7.2 Default And Opt-In Behavior

Structured mode stays default. Flat mode must be an explicit opt-in through governance. This protects current users and makes lighter workflow a conscious policy decision rather than an accidental downgrade.

### 7.3 Policy Surface

The most compatible current policy surface is constitution resolution. Existing `service.yaml` and `domain.yaml` files are metadata-oriented and do not currently participate in shared workflow-policy resolution. The constitution path already exists, but it must be extended to recognize a new workflow-mode key.

### 7.4 Context Persistence

The existing session model already persists `domain`, `service`, and `feature_id`. Flat mode should build on that instead of inventing a new context mechanism. Any human-friendly alias such as `feature` should be treated as a compatibility question, not a replacement for the current schema.

### 7.5 Review And Lifecycle Semantics

The working assumption for planning is that flat mode removes control-repo branch and PR enforcement, not lifecycle meaning. Review artifacts, stage outputs, and governance visibility should remain recognizable unless later planning explicitly decides otherwise.

---

## 8. Success Criteria

| Criterion | Measure |
|---|---|
| Governance mode exists | A centrally resolved control-repo workflow mode can be read by relevant commands |
| Structured behavior preserved | Existing structured-mode tests remain green with no behavioral regression |
| Flat mode suppresses control-branch coupling | `new-feature` and `switch` no longer require control-repo branch creation or checkout in flat mode |
| Early lifecycle compatibility | `preflight` and shared control git/state paths no longer fail solely because `{featureId}-plan` or `{featureId}-dev` do not exist in flat mode |
| Honest lifecycle definition | Product and technical planning explicitly state any remaining structured-only lifecycle surfaces before the feature is considered complete |
| Scope isolation preserved | Target-repo branch strategy and governance publication behavior remain unchanged |
| Diagnostics are understandable | Command output or logs make the effective workflow mode visible enough for support and debugging |

---

## 9. Risks And Constraints

| Risk | Impact | Why It Matters |
|---|---|---|
| Flat mode implemented only in early planning | High | Later commands would unexpectedly reintroduce structured-only assumptions |
| Workflow mode added to governance but ignored in resolution | High | The setting would exist on paper but have no behavioral effect |
| Control and target repo workflows get conflated | High | Scope expands into a broader branching redesign unrelated to the user problem |
| Structured mode regresses | Critical | Existing Lens users lose the workflow the codebase is currently optimized for |
| Mode changes mid-feature are undefined | Medium | Teams could end up with partial branch topology and unclear cleanup rules |
| Context schema changes break compatibility | Medium | Existing commands and tests already rely on `feature_id` |

Key constraints from the current codebase:

- `feature_id` in `.lens/personal/context.yaml` is already an established contract.
- Current control git logic assumes `{featureId}`, `{featureId}-plan`, and `{featureId}-dev` in multiple places.
- There is already a target-repo precedent for `requires_pr = false`, but not yet for the control repo.
- Governance must remain the source of truth for the effective workflow mode.

---

## 10. Recommended Delivery Shape

The feature should be planned and communicated as one product capability with phased implementation:

### Phase 1 — Shared Mode Resolution And Early Flow

- Add governance-resolved workflow mode support.
- Make `new-feature`, `switch`, `preflight`, and common control git/state helpers flat-mode aware.
- Preserve structured mode unchanged.

### Phase 2 — Full Lifecycle Parity

- Extend flat-mode semantics through finalizeplan, dev, complete, and any remaining cleanup or discrepancy logic.
- Ensure later lifecycle commands no longer depend on control-repo plan/dev branches when flat mode is active.

This keeps the delivery incremental without misrepresenting the finished requirement.

---

## 11. Open Questions To Carry Forward

1. Should review artifacts remain fully mandatory in flat mode even though control-repo PRs are removed?
2. What is the exact compatibility rule if governance changes workflow mode after features already exist?
3. Should the effective workflow mode be surfaced only in command outputs, or also persisted in user/session context for diagnostics?
4. What is the acceptance boundary for calling flat mode complete if dev-phase behavior is the last major structured-only surface?