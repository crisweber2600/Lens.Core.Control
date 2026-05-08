---
feature: lens-dev-new-codebase-flatten
doc_type: business-plan
status: draft
goal: "Convert the PrePlan findings for governance-controlled flat mode into explicit BusinessPlan requirements that preserve lifecycle rigor while removing control-repo branch and PR enforcement"
key_decisions:
  - v1 remains a full control-repo workflow mode, not a planning-only variant.
  - Structured mode stays the default; flat mode is an explicit governance-controlled opt-in.
  - Flat mode changes only control-repo git workflow; review artifacts, publish-to-governance, and phase-completion gates remain required.
  - v1 is governance-wide with no mixed-mode service override.
  - v1 blocks workflow-mode changes while active features exist rather than attempting mid-flight migration.
  - `feature_id` remains the canonical session-context key.
open_questions: []
depends_on: [brainstorm, research, product-brief, preplan-adversarial-review]
blocks: []
updated_at: 2026-05-08T00:00:00Z
---

# Business Plan - Governance-Controlled Flat Mode

## 1. Purpose

This business plan translates the flatten feature's PrePlan packet and adversarial-review findings into explicit BusinessPlan requirements. Its job is to remove ambiguity before PRD and UX work begin.

The feature is not a request to weaken governance. It is a request to separate **control-repo git ceremony** from **Lens lifecycle discipline** so teams can keep the latter without being forced into the former.

In v1, Lens will support two control-repo workflow modes:

- **Structured**: the current branch-and-PR control-repo workflow.
- **Flat**: no control-repo feature/plan/dev branches and no control-repo PR enforcement.

The product promise is that both modes preserve the same lifecycle identity and review discipline. The difference is the control-repo operating model, not the meaning of phases.

---

## 2. Problem Statement

The current new-codebase implementation hard-codes control-repo branch expectations across multiple layers: feature creation, feature switching, preflight sync, control git orchestration, git-state diagnostics, and dev-phase branch activation. That makes the current control-repo workflow feel mandatory even for teams that want the Lens structure but not the control-repo PR ceremony.

PrePlan established that this is a real codebase constraint, not a preference issue. It also established three BusinessPlan gaps that must be closed before implementation planning is credible:

1. The governance and review behaviors that remain invariant in flat mode were not yet frozen.
2. The rule for workflow-mode changes while features are already active was not defined.
3. The affected command surfaces were named, but not yet expressed as an ownership and acceptance matrix.

This plan closes those gaps for v1.

---

## 3. Stakeholders

| Stakeholder | Need |
|---|---|
| New Lens adopters | Use Lens lifecycle structure without immediate control-repo branch and PR overhead |
| Small or fast-moving teams | Keep planning and execution artifacts while working directly in the control repo |
| Multi-service teams | Preserve reliable domain/service/feature context without forced control-branch checkout |
| Disciplined review-oriented teams | Keep the current structured workflow unchanged and fully supported |
| Lens maintainers | Centralize workflow-mode policy and regression coverage instead of scattering exceptions across commands |

---

## 4. v1 Scope Decisions

### 4.1 Governance Rigor Stays Intact

Flat mode removes **control-repo branch and PR enforcement only**. It does not remove:

- lifecycle phases,
- required planning artifacts,
- adversarial-review gates,
- publish-to-governance handoff behavior,
- or phase-completion rules.

### 4.2 Universal Scope Is Intentional

The user request is for a governance-level universal mode. v1 therefore supports one effective control-repo workflow mode per governed installation. Service-level or per-user overrides are out of scope.

### 4.3 Active-Feature Mode Changes Are Blocked In v1

v1 does not attempt branch-topology migration for in-flight features. If active features already exist, Lens should reject workflow-mode changes with an actionable explanation rather than trying to reinterpret partially structured or partially flat state.

### 4.4 Context Compatibility Is Preserved

`.lens/personal/context.yaml` keeps `feature_id` as the canonical feature-identity field. A `feature` alias is not required for v1 and should not drive schema changes in downstream callers.

### 4.5 Later Lifecycle Support Is Part Of The Product Boundary

Flat mode is not considered complete when only `new-feature` and `switch` work. BusinessPlan must treat finalizeplan, dev, and complete as part of the same product capability, even if delivery remains phased.

---

## 5. Requirements

| ID | Requirement | Acceptance Criteria |
|---|---|---|
| FL-B1 | Workflow mode is governance-resolved and explicit. | The effective control-repo workflow mode is resolved from the governance policy path, defaults to `structured`, and is available to every mode-sensitive command through one shared resolution contract. |
| FL-B2 | Structured mode remains behaviorally unchanged. | Existing structured-mode branch creation, checkout, review, publication, and phase-completion behavior remains valid with no new required user steps. |
| FL-B3 | Flat mode preserves governance rigor. | In flat mode, review artifacts, publish-to-governance behavior, phase-completion gates, and lifecycle state progression remain required; only control-repo branch and PR enforcement are removed. |
| FL-B4 | v1 is universal rather than mixed-mode. | A governance installation resolves to one effective control-repo workflow mode at a time; service-level mixed-mode override is not supported in v1. |
| FL-B5 | Mode changes are safe by refusal in v1. | If active features exist, Lens rejects a workflow-mode change with an actionable message instead of attempting migration of existing control-branch expectations. |
| FL-B6 | Session context stays compatible. | `.lens/personal/context.yaml` continues to write `domain`, `service`, and canonical `feature_id`; flat mode does not require a schema-breaking replacement field. |
| FL-B7 | Early control flows honor flat mode. | In flat mode, `new-feature` and `switch` do not require control-repo branch creation or checkout, while still producing valid feature identity and user context. |
| FL-B8 | Shared control git and state layers honor flat mode. | `git-orchestration`, `preflight`, git-state diagnostics, and branch-derived metadata paths no longer treat missing `{featureId}-plan` or `{featureId}-dev` as automatic failure when flat mode is active. |
| FL-B9 | Later lifecycle behavior is explicitly defined. | BusinessPlan and downstream PRD/UX outputs specify the user-visible flat-mode behavior for finalizeplan, dev, and complete, including how they operate without control plan/dev branches. |
| FL-B10 | Effective mode is visible for support and debugging. | Mode-sensitive commands surface the resolved workflow mode in user-visible output or diagnostic payloads sufficient to explain why structured or flat behavior was chosen. |
| FL-B11 | Regression coverage proves both modes. | Acceptance planning includes focused regression tests showing structured mode parity and flat-mode branchless behavior across all affected command surfaces. |

---

## 6. Command And Surface Matrix

| Surface | Structured Baseline | Flat-Mode Requirement | Validation Anchor |
|---|---|---|---|
| Constitution resolution | Returns current governance policy fields; unknown workflow-mode keys are ignored today | Returns effective workflow mode through the shared resolved policy contract | Resolver tests proving default and explicit mode handling |
| `init-feature-ops.py` | Creates control feature/plan/dev branches and emits planning PR follow-up behavior | Skips control-branch creation and control planning PR follow-ups while still creating valid feature state | Focused init-feature regression for structured vs flat outputs |
| `switch-ops.py` | Writes context and attempts checkout of `{featureId}-plan` | Writes context without requiring control-branch checkout; branch state becomes informational, not blocking | Focused switch regression on flat-mode context persistence |
| `git-orchestration-ops.py` | Treats control branches as required phase infrastructure | Suppresses or short-circuits control-branch requirements when flat mode is active, while preserving publication and phase gates | Focused orchestration regression for branch-required vs branchless paths |
| `preflight.py` | Uses control branch topology during sync and cleanup decisions | Does not treat missing control branches as a sync failure in flat mode | Focused preflight regression for flat-mode sync |
| `git-state-ops.py` | Uses plan/dev branch presence as lifecycle evidence | Reports lifecycle state coherently without requiring plan/dev branch evidence in flat mode | Focused git-state regression for alternate lifecycle expectations |
| FinalizePlan / Dev / Complete conductors | Assume control plan/dev branches as part of normal execution and reporting | Define branchless control-repo behavior explicitly before implementation is called complete | Phase-specific PRD and TechPlan acceptance coverage |
| Session context contract | `feature_id` is canonical | `feature_id` remains canonical in both modes | Context-schema compatibility regression |
| User-facing diagnostics | Branch-related behavior is implicitly explained by current structured flow | Resolved mode is explicitly surfaced in command output or diagnostics | UX and output acceptance review |

---

## 7. BusinessPlan Deliverables Required Next

### 7.1 PRD Must Cover

- The exact governance-level workflow-mode contract and its default behavior.
- The invariant behaviors that remain mandatory in flat mode.
- The no-mixed-mode v1 rule.
- The active-feature guard for mode changes.
- The full affected-surface behavior matrix, including later lifecycle phases.
- Acceptance criteria for structured parity and flat-mode behavior.

### 7.2 UX Design Must Cover

- How Lens explains the current resolved mode to users.
- How mode-sensitive commands describe branchless behavior without sounding like failures.
- The user-facing error or warning when someone attempts a mode change while active features exist.
- How finalizeplan/dev/complete communicate branchless control-repo behavior in flat mode.

---

## 8. Out of Scope

- Target-project branch strategy changes or target-repo PR redesign.
- Per-service or per-user workflow-mode overrides in v1.
- Automatic migration of active structured features into flat mode, or flat features into structured mode.
- Replacing `feature_id` with a new canonical session-context field.
- Removing or weakening required planning/review artifacts.

---

## 9. Success Criteria

- BusinessPlan closes the governance-rigor question explicitly rather than leaving it implied.
- BusinessPlan closes the in-flight mode-change rule explicitly rather than deferring it to implementation.
- PRD and UX planning start from one shared command/surface matrix instead of rediscovering scope command by command.
- Later lifecycle acceptance is defined before the feature is considered implementation-ready.
- The feature remains clearly about control-repo workflow, not a broader branching-policy rewrite.

---

## 10. Definition Of Done For This Handoff

- The PrePlan review findings H1, H2, and H3 are converted into explicit BusinessPlan requirements.
- Medium findings M1, M2, and M3 are either resolved as v1 decisions or translated into acceptance criteria.
- The next phase can produce PRD and UX outputs without reopening first-principles debate about scope, invariants, or compatibility.