---
feature: lens-dev-new-codebase-techplan
doc_type: tech-plan
status: draft
goal: "Implement bmad-lens-techplan as a clean, auditable SKILL.md with explicit shared-utility delegation, correct config path, and full behavioral parity with the old-codebase baseline"
key_decisions:
  - SKILL.md-only implementation — no new scripts; validate-phase-artifacts.py is the only script dependency
  - Config path: _bmad/bmadconfig.yaml (rewrite standard); old _bmad/config.yaml path is retired
  - Architecture authoring delegated exclusively to bmad-lens-bmad-skill → bmad-create-architecture
  - Review-ready fast path delegates to shared gate utility; not re-implemented inline
  - Batch mode 2-pass contract delegates to bmad-lens-batch; not re-implemented inline
  - Publish-before-author hook declared as explicit phase-entry step with no skip path
  - Adversarial review party-mode is a hard gate — no phase promotion without a pass or pass-with-warnings verdict
  - feature.yaml phase update to techplan-complete happens only after review passes
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
  - lens-dev-new-codebase-businessplan
blocks:
  - lens-dev-new-codebase-finalizeplan
updated_at: "2026-04-28T00:00:00Z"
---

# Tech Plan — TechPlan Command Implementation (lens-dev-new-codebase-techplan)

**Author:** crisweber2600
**Date:** 2026-04-28

---

## Executive Summary

This plan covers the technical implementation of the `bmad-lens-techplan` skill for the lens-work rewrite. The skill is a Lens phase conductor — it owns lifecycle gates, enforces write boundaries, and delegates architecture authoring to the native architecture workflow. It does not produce documents itself. The technical work is a clean SKILL.md specification that replaces three independently copy-pasted lifecycle patterns (review-ready fast path, batch mode 2-pass, publish-before-author) with explicit shared utility delegation points, corrects the config path, and states every behavioral contract unambiguously so future maintainers can trace and validate the implementation end to end.

The implementation is SKILL.md-only. No new scripts are required. The skill's one script dependency is `validate-phase-artifacts.py`, which already exists and is shared across all phase skills.

---

## 1. Current Architecture (Old Codebase Baseline)

### 1.1 Skill Location and Structure

```
old-codebase:
  _bmad/lens-work/skills/bmad-lens-techplan/
    SKILL.md    ← full implementation
    (no scripts)
```

### 1.2 Activation Sequence (Old Codebase)

1. Load config from `_bmad/config.yaml` and `_bmad/config.user.yaml`
2. Resolve `governance_repo` and `feature_id`
3. Load `feature.yaml` via `bmad-lens-feature-yaml`
4. Validate feature track includes techplan in its phases
5. Validate businessplan predecessor is complete (or track skips it)
6. Resolve staged docs path and governance docs mirror path from `feature.yaml.docs`
7. Determine mode: interactive or batch
8. **Inline batch check:** if batch and no `batch_resume_context`, write `techplan-batch-input.md` and stop
9. **Inline review-ready check:** run `validate-phase-artifacts.py --contract review-ready`; if pass and phase is `techplan`, skip handoff, go directly to adversarial review
10. In interactive mode with direct invocation and fail status: announce publish + architecture handoff, wait for confirmation
11. In interactive mode with `next` delegation and fail status: treat delegation as consent, proceed immediately
12. **Inline publish-before-author:** call `publish-to-governance --phase businessplan`
13. Load businessplan artifacts as authoring context
14. Load cross-feature context via `bmad-lens-init-feature fetch-context --depth full`
15. Load domain constitution via `bmad-lens-constitution`
16. Delegate to `bmad-lens-bmad-skill --skill bmad-create-architecture`

### 1.3 Phase Completion (Old Codebase)

1. Run `bmad-lens-adversarial-review --phase techplan --source phase-complete`
   - fail → stop; do not update feature.yaml
   - pass or pass-with-warnings → continue
2. Update `feature.yaml` phase to `techplan-complete` via `bmad-lens-feature-yaml`
3. Leave governance publication of architecture.md to FinalizePlan unless user explicitly requests now
4. Report: advance to `/finalizeplan`

### 1.4 Known Problems in Old Implementation

| Problem | Location | Impact |
|---|---|---|
| Config path hardcoded to `_bmad/config.yaml` | On Activation step 1 | Will fail if rewrite standardizes on `bmadconfig.yaml` |
| Review-ready fast path re-implemented inline | On Activation step 9 | Bug fixes do not propagate; behavioral drift across phases |
| Batch 2-pass contract re-implemented inline | On Activation step 8 | Same drift risk as review-ready fast path |
| Publish-before-author ordering re-implemented inline | On Activation step 12 | Same drift risk; ordering is a security boundary (governance writes must not precede authoring) |

---

## 2. Target Architecture (New Codebase)

### 2.1 Skill Location

```
new-codebase:
  lens.core/_bmad/lens-work/skills/bmad-lens-techplan/
    SKILL.md    ← rewritten implementation
    (no scripts)
```

### 2.2 Config Path Correction

The new skill loads from:
- `{project-root}/lens.core/_bmad/bmadconfig.yaml`
- `{project-root}/lens.core/_bmad/config.user.yaml` (optional override)

The old `_bmad/config.yaml` path is retired. Any activation step referencing it must be updated to `bmadconfig.yaml`.

### 2.3 Shared Utility Delegation Map

| Pattern | Old Codebase | New Codebase Target |
|---|---|---|
| Review-ready fast path | Inline in On Activation | Delegates to shared `lens-phase-gate` review-ready utility |
| Batch 2-pass contract | Inline in On Activation | Delegates to `bmad-lens-batch --target techplan` |
| Publish-before-author | Inline publish CLI call | Declared as explicit phase-entry step; routed through `bmad-lens-git-orchestration publish-to-governance --phase businessplan` |

Note: The `lens-phase-gate` utility is a candidate extraction from the three independently copy-pasted review-ready implementations in preplan, businessplan, and techplan. Its extraction is tracked separately under the shared-utilities extraction scope. Until the utility is implemented, the techplan SKILL.md must declare the review-ready gate as an explicit on-activation step that invokes `validate-phase-artifacts.py` directly — this is acceptable as a deterministic, auditable contract even before extraction.

### 2.4 Delegation Chain

```
lens-techplan.prompt.md (stub)
  └── lens.core/_bmad/lens-work/prompts/lens-techplan.prompt.md (redirect)
      └── bmad-lens-techplan/SKILL.md (conductor)
          ├── [phase entry] bmad-lens-git-orchestration publish-to-governance --phase businessplan
          ├── [authoring] bmad-lens-bmad-skill --skill bmad-create-architecture
          └── [completion gate] bmad-lens-adversarial-review --phase techplan --source phase-complete
```

### 2.5 Data Contract Surface

| Data | Source | Consumer |
|---|---|---|
| `feature.yaml` | governance repo, `features/{domain}/{service}/{featureId}/feature.yaml` | bmad-lens-feature-yaml |
| `prd.md` (staged) | control repo, `docs/{domain}/{service}/{featureId}/prd.md` | bmad-create-architecture (must reference) |
| `ux-design.md` (staged) | control repo, `docs/{domain}/{service}/{featureId}/ux-design.md` | bmad-create-architecture (authoring context) |
| `businessplan-adversarial-review.md` (staged) | control repo, same docs path | publish-to-governance --phase businessplan |
| `architecture.md` (staged output) | control repo, `docs/{domain}/{service}/{featureId}/architecture.md` | FinalizePlan handoff |
| `techplan-adversarial-review.md` (review output) | control repo, same docs path | FinalizePlan handoff |
| `techplan-batch-input.md` (batch mode only) | control repo, same docs path | Pass 2 resume |
| `lifecycle.yaml` | lens.core/_bmad/lens-work/lifecycle.yaml | validate-phase-artifacts.py, artifact_validation |
| `bmadconfig.yaml` | lens.core/_bmad/bmadconfig.yaml | config resolution |
| governance docs mirror | governance repo, `features/{domain}/{service}/{featureId}/docs/` | cross-feature consumers after publish |

### 2.6 Frozen Contract Obligations

These contracts must not change in the new implementation:

| Contract | Obligation |
|---|---|
| Publish-before-author | `publish-to-governance --phase businessplan` must complete before any `architecture.md` is staged |
| PRD reference | `architecture.md` artifact_validation rule `must_reference: prd.md` must remain enforced in lifecycle.yaml |
| Review-ready fast path | If `validate-phase-artifacts.py --contract review-ready` returns `status=pass` AND phase is still `techplan`, skip handoff and go directly to adversarial review |
| Next-handoff consent | Delegation from `next` is pre-confirmed consent; no redundant yes/no prompt |
| Batch mode 2-pass | Pass 1: write `techplan-batch-input.md`, stop. Pass 2: load approved answers, skip interactive confirmation |
| Adversarial review hard gate | A `fail` verdict from `bmad-lens-adversarial-review` blocks phase state update; no exceptions |
| Governance write boundary | No direct file writes or patches to the governance repo from within the skill; publish CLI is the only valid path |

---

## 3. Implementation Specification

### 3.1 SKILL.md Section Map

The new `bmad-lens-techplan/SKILL.md` must contain the following sections in this order:

```
---
name: bmad-lens-techplan
description: TechPlan phase — architecture and technical design for a feature with Lens governance.
---

# TechPlan — Feature Technical Design Phase

## Overview
## Identity
## Communication Style
## Principles
## On Activation          (steps 1–16, with shared utility delegation points explicit)
## Artifacts              (table: artifact, description, agent)
## Required Frontmatter   (architecture.md YAML block)
## Phase Completion       (adversarial review gate + feature.yaml update + finalize handoff)
## Integration Points     (table: skill/agent, role)
```

### 3.2 On Activation — Detailed Step Specification

| Step | Action | Notes |
|---|---|---|
| 1 | Load `bmadconfig.yaml` and optional `config.user.yaml` | Path: `lens.core/_bmad/bmadconfig.yaml` — not `_bmad/config.yaml` |
| 2 | Resolve `governance_repo` and `feature_id` | From config; `feature_id` may be passed as `--feature-id` arg |
| 3 | Load `feature.yaml` via `bmad-lens-feature-yaml` | Read-only at this point |
| 4 | Validate feature track includes techplan phases | Express track does not include techplan; stop if not applicable |
| 5 | Validate businessplan predecessor is complete | Check `feature.yaml.phase` for `businessplan-complete` or track-skip condition |
| 6 | Resolve staged docs path and governance mirror path | Default: `docs/{domain}/{service}/{featureId}` / `features/{domain}/{service}/{featureId}/docs` |
| 7 | Determine mode | `interactive` (default) or `batch` |
| 8 | Batch pass-1 check | If batch and no `batch_resume_context`: delegate to `bmad-lens-batch --target techplan`, write `techplan-batch-input.md`, stop |
| 9 | Batch pass-2 check | If batch and `batch_resume_context` present: load approved context, skip interactive confirmation |
| 10 | Review-ready gate | Run `validate-phase-artifacts.py --phase techplan --contract review-ready --lifecycle-path ... --docs-root ... --json` |
| 11 | Review-ready fast path | If `status=pass` AND phase is `techplan`: skip to adversarial review; do not re-run handoff or re-confirm |
| 12 | Interactive direct-invoke confirmation | If interactive, direct invoke, `status=fail`: announce publish + architecture handoff; wait for confirmation; stop if declined |
| 13 | Next-delegation fast path | If auto-delegated from `next`, `status=fail`: treat delegation as consent; proceed immediately |
| 14 | Publish-before-author | Call `bmad-lens-git-orchestration publish-to-governance --phase businessplan`; do not proceed to authoring until this completes successfully |
| 15 | Load authoring context | Load staged `prd.md`, `ux-design.md`, and `businessplan-adversarial-review.md` from control repo docs path |
| 16 | Load cross-feature context | `bmad-lens-init-feature fetch-context --depth full` |
| 17 | Load domain constitution | `bmad-lens-constitution` |
| 18 | Delegate to architecture workflow | `bmad-lens-bmad-skill --skill bmad-create-architecture`; do not author architecture content from conductor side |

### 3.3 Phase Completion — Step Specification

| Step | Action | Condition |
|---|---|---|
| 1 | Run adversarial review | `bmad-lens-adversarial-review --phase techplan --source phase-complete` with party-mode challenge |
| 2 | Evaluate verdict | `fail` → stop; report blocking findings; do not update feature.yaml |
| 3 | Continue on pass | `pass` or `pass-with-warnings` → proceed |
| 4 | Update feature.yaml phase | `bmad-lens-feature-yaml` → phase: `techplan-complete` |
| 5 | Defer architecture publication | Leave governance publication of `architecture.md` and `techplan-adversarial-review.md` to FinalizePlan unless user explicitly requests now |
| 6 | Report next action | "advance to `/finalizeplan`" with auto-advance signal per lifecycle.yaml |

### 3.4 Required Frontmatter for architecture.md

```yaml
---
feature: {featureId}
doc_type: architecture
status: draft | in-review | approved
goal: "{one-line goal}"
key_decisions: []
open_questions: []
depends_on: []
blocks: []
updated_at: {ISO timestamp}
---
```

### 3.5 Integration Points Table

| Skill / Agent | Role in TechPlan |
|---|---|
| `bmad-lens-feature-yaml` | Reads feature.yaml on activation; updates phase to `techplan-complete` after review passes |
| `bmad-lens-init-feature` | Loads cross-feature context via `fetch-context --depth full` |
| `bmad-lens-constitution` | Loads domain constitution for architectural constraints |
| `bmad-lens-git-orchestration` | Publishes reviewed businessplan artifacts to governance mirror; commits staged techplan artifacts |
| `bmad-lens-bmad-skill` | Routes architecture creation through Lens-aware BMAD wrapper; enforces write boundaries |
| `bmad-lens-adversarial-review` | Runs adversarial review + party-mode challenge as hard phase completion gate |
| `bmad-lens-batch` | Handles batch 2-pass contract; pass-1 write + pass-2 resume |
| `bmad-lens-theme` | Applies active persona overlay |
| `validate-phase-artifacts.py` | Evaluates review-ready contract; determines fast-path routing |

---

## 4. Rollout Strategy

### 4.1 Implementation Order

1. **SKILL.md draft** — write the new `bmad-lens-techplan/SKILL.md` against this tech plan
2. **Config path verification** — confirm `bmadconfig.yaml` loads correctly in activation step 1
3. **Parity review** — walk the old-codebase SKILL.md side by side with the new one; verify every behavioral contract is reproduced
4. **Regression anchors** — add focused regression coverage for the three techplan-specific contracts (see §4.2)
5. **Integration validation** — verify activation chain from prompt stub through SKILL.md to `bmad-lens-bmad-skill` delegation

### 4.2 Regression Coverage Plan

| Test | What It Validates |
|---|---|
| Architecture PRD reference regression | `architecture.md` produced without a `prd.md` reference fails `validate-phase-artifacts.py` artifact_validation |
| Wrapper equivalence regression | `bmad-lens-bmad-skill --skill bmad-create-architecture` is the only authoring path; no direct architecture writes from conductor |
| Publish-before-author regression | Confirm `publish-to-governance --phase businessplan` is called in step 14 before any `architecture.md` is staged |
| Review-ready fast path regression | If `validate-phase-artifacts.py` returns `status=pass` with phase `techplan`, next step is adversarial review, not architecture handoff |
| Next-handoff consent regression | When delegated from `next`, activation proceeds without a run-confirmation prompt |
| Adversarial review hard-gate regression | A `fail` review verdict from adversarial review does not advance `feature.yaml` phase |

### 4.3 Dependencies

| Dependency | Status | Notes |
|---|---|---|
| `validate-phase-artifacts.py` | Existing | Shared across all phase skills; no changes required |
| `bmad-lens-adversarial-review` | Existing (old codebase carry-over) | Must be available at day 1 |
| `bmad-lens-bmad-skill` | Existing (old codebase carry-over) | Must be available at day 1 |
| `bmad-lens-git-orchestration` | Existing (old codebase carry-over) | `publish-to-governance` subcommand must be available |
| `bmad-lens-batch` | Existing (old codebase carry-over) | Required for batch mode |
| `bmad-lens-feature-yaml` | Existing (old codebase carry-over) | Required for feature.yaml reads and writes |
| `bmad-lens-constitution` | Existing (old codebase carry-over) | Required for constitution load |
| `bmad-lens-init-feature` (fetch-context) | Existing (old codebase carry-over) | Required for cross-feature context |
| `lens-phase-gate` shared utility | Candidate extraction | Optional for day 1; inline fallback is acceptable; extraction tracked separately |

### 4.4 Day-1 Acceptance Conditions

1. `bmad-lens-techplan/SKILL.md` exists in `lens.core/_bmad/lens-work/skills/bmad-lens-techplan/`
2. Config path in On Activation step 1 references `bmadconfig.yaml`
3. All frozen contracts are explicitly stated and traceable in the SKILL.md sections
4. Integration points table is complete and includes all dependencies listed in §3.5
5. Parity review against old-codebase SKILL.md shows no missing behavioral contracts
6. Regression anchors for the three techplan-specific contracts are declared (even if not yet automated)

---

## 5. Architectural Notes and Decisions

### 5.1 ADR-1: SKILL.md-Only, No New Scripts

**Decision:** The new `bmad-lens-techplan` skill is implemented as a SKILL.md only. No new scripts are added.

**Rationale:** The techplan skill is a phase conductor. Its work is coordination, gate enforcement, and delegation. All automation is handled by existing scripts (`validate-phase-artifacts.py`) and existing skills (`bmad-lens-git-orchestration`, `bmad-lens-adversarial-review`). Adding new scripts would introduce new surface area without adding new capability.

**Consequences:** The skill's behavior is described in the SKILL.md natural language contract rather than enforced by code. Regression coverage validates the behavioral contract through integration tests, not unit tests on the skill itself.

### 5.2 ADR-2: Shared Utility Delegation Points Are Explicit Even Before Utility Exists

**Decision:** The SKILL.md specifies shared utility delegation points (review-ready fast path, batch contract, publish-before-author hook) even if the shared utility skills have not yet been extracted. An inline fallback is acceptable at day 1.

**Rationale:** Specifying the delegation point forces the SKILL.md to be written in a way that makes extraction safe later. A skill that inline-implements a pattern and declares the shared delegation point is structurally ready for extraction without behavioral change. A skill that inline-implements without declaring a delegation point requires a refactor to extract safely.

**Consequences:** At day 1, some delegation steps may still be inline. The SKILL.md must note this explicitly with a `TODO(shared-utility): extract to lens-phase-gate` annotation to make the extraction obligation visible to future maintainers.

### 5.3 ADR-3: Adversarial Review Is a Non-Optional Hard Gate

**Decision:** The adversarial review step in Phase Completion cannot be skipped, bypassed, or made conditional on any flag.

**Rationale:** The lifecycle contract states that all phase completions gate on `adversarial-review` with party mode. This is a fundamental axiom (FT1: planning artifacts must exist and be reviewed before code is written). Making the review optional would violate the lifecycle contract and defeat the purpose of the governance gate.

**Consequences:** Any invocation path that reaches Phase Completion must pass through the review gate. The SKILL.md must not include a `--skip-review` flag or equivalent.

### 5.4 ADR-4: Architecture Publication Deferred to FinalizePlan

**Decision:** `architecture.md` and `techplan-adversarial-review.md` are NOT published to the governance mirror by the techplan skill. Publication is deferred to FinalizePlan.

**Rationale:** This is the staged-then-published pattern used consistently across the full-track planning phases. BusinessPlan publishes reviewed PrePlan artifacts. TechPlan publishes reviewed BusinessPlan artifacts. FinalizePlan publishes reviewed TechPlan artifacts. Each phase is one step behind in governance publication, ensuring that governance always sees reviewed artifacts, not drafts.

**Consequences:** After `techplan-complete`, the governance mirror contains reviewed businessplan artifacts but not the architecture. Cross-feature consumers who need the architecture must wait for FinalizePlan publication.
