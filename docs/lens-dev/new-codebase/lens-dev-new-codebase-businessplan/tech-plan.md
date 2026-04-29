---
feature: lens-dev-new-codebase-businessplan
doc_type: tech-plan
status: draft
goal: "Define the technical design for rewriting businessplan as a thin conductor: shared utility delegation, publish-before-author entry hook, BMB-first authoring, and wrapper-equivalence regression coverage. TechPlan rewrite deferred to lens-dev-new-codebase-techplan."
key_decisions:
  - Businessplan command follows the thin-conductor pattern; delegation chain authored from scratch
  - No inline batch logic in SKILL.md — delegate to bmad-lens-batch
  - No inline review-ready logic in SKILL.md — delegate to validate-phase-artifacts.py
  - publish-to-governance is the sole governance write path for businessplan
  - SKILL.md changes authored via bmad-module-builder (BMB-first); release prompts via bmad-workflow-builder
  - Regression coverage required before merge: wrapper-equivalence + governance-audit
open_questions: []
depends_on:
  - business-plan.md (this feature)
  - lens-dev-new-codebase-baseline architecture.md
blocks: []
updated_at: 2026-04-28T00:00:00Z
---

# Technical Plan — Rewrite businessplan Command

**Feature:** lens-dev-new-codebase-businessplan  
**Author:** crisweber2600  
**Date:** 2026-04-28  
**References:** [Business Plan](./business-plan.md), [Baseline Architecture](../../lens-dev-new-codebase-baseline/architecture.md)

---

## 1. System Design

The businessplan command follows the invariant 3-hop command resolution chain:

```
.github/prompts/lens-{command}.prompt.md        (stub — user entry point)
  → lens.core/_bmad/lens-work/prompts/lens-{command}.prompt.md  (release prompt)
    → skills/bmad-lens-{command}/SKILL.md        (owning skill — conductor)
      → shared utilities + bmad-lens-bmad-skill  (delegation targets)
```

Each SKILL.md is a **thin conductor** — it orchestrates shared utilities by delegation; it does not implement shared logic inline.

---

## 2. Thin Conductor Pattern

The businessplan command implements the delegation chain with the following phase-specific parameters:

### 2.1 Shared Delegation Points

| Pattern | Old codebase | New codebase |
|---------|-------------|--------------|
| Batch 2-pass intake | Inline if/else in SKILL.md | `bmad-lens-batch --target businessplan` |
| Review-ready fast path | Inline artifact existence checks in SKILL.md | `validate-phase-artifacts.py --phase businessplan --contract review-ready` |
| Publish prior-phase artifacts to governance | Direct writes in some paths | `git-orchestration-ops.py publish-to-governance --phase preplan` |
| Authoring delegation | Direct persona references in some paths | `bmad-lens-bmad-skill --skill {authoring-skill}` |
| Adversarial review gate | Inline or direct call | `bmad-lens-adversarial-review --phase businessplan --source phase-complete` |

### 2.2 businessplan Conductor Flow

```
[entry]
  → validate: phase includes businessplan, preplan is complete
  → if batch and no context: bmad-lens-batch --target businessplan → stop (pass 1)
  → if batch with context: load batch context as pre-approved, continue
  → validate-phase-artifacts.py --phase businessplan --contract review-ready
      ├── status=pass: jump to adversarial review (review-ready fast path)
      └── status=fail: continue to publish + delegate
  → git-orchestration-ops.py publish-to-governance --phase preplan
  → interactive: confirm workflow scope (prd | ux-design | both)
  → bmad-lens-bmad-skill --skill bmad-create-prd        [if prd or both]
  → bmad-lens-bmad-skill --skill bmad-create-ux-design  [if ux-design or both]
  → bmad-lens-adversarial-review --phase businessplan --source phase-complete
      ├── fail: stop, do not update feature.yaml
      └── pass/pass-with-warnings: continue
  → bmad-lens-feature-yaml: update phase to businessplan-complete
[done]
```

### 2.3 techplan Conductor Flow

```
[entry]
  → validate: phase includes techplan, businessplan-complete
  → if batch and no context: bmad-lens-batch --target techplan → stop (pass 1)
  → if batch with context: load batch context as pre-approved, continue
  → validate-phase-artifacts.py --phase techplan --contract review-ready
      ├── status=pass: jump to adversarial review (review-ready fast path)
      └── status=fail: continue to publish + delegate
  → git-orchestration-ops.py publish-to-governance --phase businessplan
  → bmad-lens-bmad-skill --skill bmad-create-architecture
      (PRD reference rule: architecture must reference prd.md from staged docs path)
  → bmad-lens-adversarial-review --phase techplan --source phase-complete
      ├── fail: stop, do not update feature.yaml
      └── pass/pass-with-warnings: continue
  → bmad-lens-feature-yaml: update phase to techplan-complete
[done]
```

---

## 3. Files to Create / Modify

### 3.1 businessplan

| File | Action | Channel |
|------|--------|---------|
| `lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/SKILL.md` | Rewrite | bmad-module-builder (BMB-first) |
| `lens.core/_bmad/lens-work/prompts/lens-businessplan.prompt.md` | Rewrite | bmad-workflow-builder |
| `.github/prompts/lens-businessplan.prompt.md` | Verify/update stub chain | Direct (stub only) |

### 3.2 Shared Dependencies (read-only references — not modified in this feature)

| Dependency | Why Referenced |
|-----------|----------------|
| `scripts/validate-phase-artifacts.py` | Review-ready delegation target |
| `skills/bmad-lens-batch/` | Batch 2-pass delegation target |
| `skills/bmad-lens-git-orchestration/scripts/git-orchestration-ops.py` | Publish-to-governance CLI |
| `skills/bmad-lens-bmad-skill/` | Authoring delegation router |
| `skills/bmad-lens-adversarial-review/` | Review gate |
| `skills/bmad-lens-feature-yaml/` | Phase transition writer |
| `lifecycle.yaml` | Phase contracts, completion review config |

---

## 4. SKILL.md Design Contract

The businessplan SKILL.md follows this structural template:

```
## Overview        — 2-3 lines: what the command does, track constraint, args
## Identity        — 1 paragraph: conductor role, what it does NOT do
## Communication Style — 4-6 bullets: phase prefix pattern, confirmation behavior, fast-path behavior
## Principles      — 6-8 bullets: wrapper-first, stage-then-publish, feature-docs-authority, etc.
## On Activation   — ordered steps (5-8): load config → validate track → validate predecessor →
                     resolve docs path → check batch → validate-phase-artifacts.py → 
                     publish-to-governance → delegate → adversarial review → update phase
## Artifacts       — table: artifact name, description, producing agent
## Required Frontmatter — yaml block
## Phase Completion — ordered steps: adversarial review gate → feature-yaml update → report next
## Integration Points — table of downstream skill integrations
```

---

## 5. Regression Coverage

Three regression categories are required before merge:

### 5.1 Wrapper-Equivalence

Verifies each command routes authoring to the same skill as the old codebase:

| Test | businessplan | techplan |
|------|-------------|---------|
| PRD authoring via bmad-create-prd | ✓ | n/a |
| UX authoring via bmad-create-ux-design | ✓ | n/a |
| Architecture authoring via bmad-create-architecture | n/a | ✓ |
| Review-ready fast path invokes validate-phase-artifacts.py | ✓ | ✓ |
| Batch pass 1 writes {phase}-batch-input.md and stops | ✓ | ✓ |
| Batch pass 2 resumes with pre-approved context | ✓ | ✓ |

### 5.2 Governance-Audit

Verifies no direct governance writes occur outside the publish CLI:

| Test | Expected |
|------|----------|
| businessplan: no direct writes to governance before publish-to-governance call | PASS |
| techplan: no direct writes to governance before publish-to-governance call | PASS |
| businessplan: publish-to-governance called before first PRD/UX authoring step | PASS |
| techplan: publish-to-governance called before architecture authoring step | PASS |

### 5.3 Architecture-Reference

Verifies techplan enforces the PRD dependency rule:

| Test | Expected |
|------|----------|
| architecture.md references prd.md from staged docs path | PASS |
| Lifecycle artifact_validation rule for PRD reference is checked | PASS |

---

## 6. BMB-First Authoring Protocol

Per the `lens-dev/new-codebase` service constitution:

> Anytime `lens.core.src` is being modified, SKILL.md updates must be authored through `.github/skills/bmad-module-builder` and release prompt or workflow artifacts must be authored through `.github/skills/bmad-workflow-builder`.

**Implementation sequence:**
1. Use `bmad-module-builder` to generate / rewrite `bmad-lens-businessplan/SKILL.md`
2. Use `bmad-workflow-builder` to generate / rewrite `lens-businessplan.prompt.md`
3. Verify stub chain integrity (the `.github/prompts/` stub still runs `uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py` before handing off)
4. Run regression coverage
5. Commit to `develop` in `lens.core.src` via the standard per-commit workflow

---

## 7. Rollout Strategy

| Step | Description |
|------|-------------|
| Pre-condition check | Confirm stories 1.4, 3.1, and 4.1 are `done` in `lens.core.src` before starting |
| BP-1: businessplan rewrite | Rewrite SKILL.md + release prompt via BMB channels; verify stub chain |
| BP-3: regression gate | Run both regression categories; block merge if any fail |
| BP-4: discovery surface verification | Confirm `module-help.csv` and `agents/lens.agent.md` entries unchanged (`businessplan` remains in 17-command surface) |
| Governance sync | Commit businessplan rewrite to `develop`; update feature.yaml via governance |

---

## 8. Key Architectural Constraints from Baseline

From `lens-dev-new-codebase-baseline/docs/architecture.md`:

> **businessplan:** Cannot be rewritten independently from the publish-before-author hook. Direct governance writes remain prohibited.

> **techplan:** The architecture generator, publish-entry hook, and phase completion behavior are one dependency bundle and must be rewritten together.

Both commands are separate deliverables. Rewriting businessplan in this feature leaves the planning pipeline in a partially governed state for full-track features until `lens-dev-new-codebase-techplan` is delivered. This is an accepted known limitation of the scope split.
