---
feature: lens-dev-new-codebase-techplan
doc_type: business-plan
status: draft
goal: "Deliver a clean, governed implementation of the techplan command that preserves all behavioral contracts, produces a reviewed architecture artifact, and gates FinalizePlan entry reliably"
key_decisions:
  - techplan is command #9 in the 17-command retained surface — full-track only, bridges businessplan to finalizeplan
  - architecture.md is the sole output artifact; the phase conductor does not author it — it delegates via bmad-lens-bmad-skill
  - publish-before-author ordering is a non-negotiable contract — reviewed businessplan artifacts go to governance before any architecture is staged
  - PRD reference is a lifecycle-enforced requirement for architecture.md — failure to reference prd.md is a validation error
  - review-ready fast path is shared — phase skip to adversarial review when validate-phase-artifacts.py returns pass
  - next-handoff pre-confirmed contract applies — no redundant yes/no prompt when auto-delegated from next
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
  - lens-dev-new-codebase-businessplan
blocks:
  - lens-dev-new-codebase-finalizeplan
updated_at: "2026-04-28T00:00:00Z"
---

# Business Plan — TechPlan Command (lens-dev-new-codebase-techplan)

**Author:** crisweber2600
**Date:** 2026-04-28

---

## Executive Summary

The `techplan` command is the final milestone-completion phase for the full-track planning arc. It follows `businessplan`, delegates architecture authoring to the native architecture workflow via the registered Lens BMAD wrapper, and produces a reviewed `architecture.md` that gates FinalizePlan entry. In the lens-work rewrite, the implementation goal is to preserve all observable techplan behavior from the old codebase while extracting three copy-pasted lifecycle patterns (review-ready fast path, batch 2-pass contract, publish-before-author entry hook) into shared utilities. The resulting skill is a clean, auditable phase conductor with an explicit activation contract, a single delegated authoring path, and a hard review gate before phase state promotion.

The rewrite does not change what `techplan` does for users. It changes how cleanly the behavior is specified, where shared patterns live, and how confidently future maintainers can trace what the command actually does. This is a maintainability and governance improvement inside a backwards-compatibility envelope.

---

## 1. Business Context

### 1.1 Product Position

`techplan` is the third of five full-track planning phases:

```
preplan → businessplan → techplan → finalizeplan → dev → complete
```

It is the final phase in the `techplan` milestone. When TechPlan completes, the feature's `phase` advances to `techplan-complete`, which is the prerequisite for FinalizePlan to begin. The milestone promotion is the lifecycle gate that separates planning from delivery governance.

### 1.2 User Value

For Lens users doing full-track feature development, `techplan` is the point at which product requirements (PRD) translate into a governed technical architecture. The user receives:

- A reviewed `architecture.md` that references the PRD
- Confirmation that businessplan artifacts have been published to the governance mirror
- Adversarial review gate with party-mode challenge to catch architectural assumptions before finalizeplan
- Automatic milestone promotion to `techplan-complete` on a passing verdict

Without a reliable `techplan`, the planning → dev chain breaks: FinalizePlan cannot proceed, governance cannot confirm the feature is ready for development, and the architecture never undergoes the required review gate.

### 1.3 Stakeholders

| Stakeholder | Concern | Impact of a broken techplan |
|---|---|---|
| **Lens users (full-track)** | Need architecture phase to produce a reviewed, governance-registered artifact | Feature stalls at techplan; cannot advance to finalizeplan |
| **Lens module maintainers** | Need a single-source, auditable techplan implementation | Copy-paste drift reintroduces behavioral inconsistencies across phase skills |
| **Feature governance reviewers** | Need businessplan artifacts in the governance mirror before reviewing architecture | If publish-before-author ordering breaks, governance sees stale or missing prior-phase docs |
| **Downstream phase skills** | FinalizePlan reads architecture.md; epics/stories reference its decisions | Architecture produced outside the governed path bypasses the review gate and PRD reference validation |

---

## 2. Problem Statement

The old-codebase `bmad-lens-techplan` SKILL.md contains three copy-pasted patterns that appear independently in preplan, businessplan, techplan, and finalizeplan:

1. **Review-ready fast path** — inline `validate-phase-artifacts.py` call with conditional skip-to-review logic
2. **Batch mode 2-pass contract** — pass-1 write + stop / pass-2 resume with `batch_resume_context`
3. **Publish-before-author ordering** — explicit publish CLI call before any local authoring begins

Because each phase implements these patterns independently, a fix in one does not propagate to the others. The rewrite must produce a techplan skill where all three patterns either delegate to shared utilities or are specified with a precision that makes independent re-implementation safe to maintain. The `bmad-lens-batch` shared skill already exists; the review-ready gate and publish-before-author hook are candidates for extraction.

A second, lower-priority issue is the config path. The old codebase loaded config from `_bmad/config.yaml`. The rewrite standard is `_bmad/bmadconfig.yaml`. The new techplan skill must reference the updated path.

---

## 3. Goals

### G1 — Behavioral Parity
The new techplan skill must preserve every user-observable behavior from the old codebase:
- publish-before-author ordering: reviewed businessplan artifacts to governance before architecture is staged
- PRD reference requirement: `architecture.md` must reference `prd.md` (enforced by lifecycle `artifact_validation`)
- review-ready fast path: if `validate-phase-artifacts.py` returns `status=pass` and phase is still `techplan`, skip handoff and go directly to adversarial review
- next-handoff pre-confirmed: no second yes/no prompt when delegated from `next`
- batch mode 2-pass: pass 1 writes `techplan-batch-input.md`, pass 2 resumes with pre-approved context

### G2 — Shared Utility Extraction
The three copy-pasted patterns (review-ready fast path, batch contract, publish-before-author hook) are extracted or delegated to shared contracts rather than re-implemented inline.

### G3 — Config Path Correction
The skill loads config from `_bmad/bmadconfig.yaml` (rewrite standard), not the old `_bmad/config.yaml`.

### G4 — Maintainability
The SKILL.md must be auditable end-to-end: activation steps, delegation path, artifact expectations, phase completion gate, and integration points are all explicitly stated and traceable.

---

## 4. Non-Goals

- **No new techplan behaviors.** This is a preservation rewrite, not a feature addition.
- **No architecture authoring inside the skill.** The phase conductor delegates; it never writes the architecture document itself.
- **No governance writes from the skill.** Publish-to-governance is the only valid path for moving artifacts into the governance mirror. The skill SKILL.md must not contain tool calls or patches that write directly to the governance repo.
- **No UI changes.** The interaction model (interactive + batch) is unchanged.
- **No lifecycle schema changes.** `feature.yaml`, `lifecycle.yaml`, and `architecture.md` frontmatter schemas remain stable.

---

## 5. Success Criteria

| Criterion | Measure |
|---|---|
| Behavioral parity | Old-codebase techplan SKILL.md behavior is fully reproduced in the new skill with no user-observable difference |
| Publish-before-author | `publish-to-governance --phase businessplan` is called before any architecture artifact is staged |
| PRD reference validation | `lifecycle.yaml` artifact_validation rule for `architecture.md must_reference prd.md` remains enforced |
| Review-ready fast path | `validate-phase-artifacts.py --contract review-ready` returns pass → skip to adversarial review immediately |
| Batch mode | Pass 1 writes `techplan-batch-input.md`; pass 2 resumes with approved answers as pre-loaded context |
| Next-handoff consent | No redundant confirmation when auto-delegated from `next` |
| Shared utilities | Review-ready pattern, batch contract, and publish-before-author hook delegate to shared logic rather than inline re-implementation |
| Config path | Skill loads from `_bmad/bmadconfig.yaml` |
| Review gate | Adversarial review with party-mode challenge must pass before phase state advances to `techplan-complete` |
| Regression coverage | Rewrite regression exists for architecture PRD reference, wrapper equivalence, and publish-before-author ordering |

---

## 6. Risk Assessment

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Copy-paste pattern diverges on next fix | High (pre-rewrite state) | Medium | Extract shared utilities; do not re-implement inline |
| Publish-before-author ordering accidentally dropped | Low | High | Explicit step-order in On Activation with no skip path |
| PRD reference validation removed from lifecycle | Low | High | lifecycle.yaml artifact_validation is frozen — rewrite must not change it |
| Review gate bypassed on refactor | Medium | High | Adversarial review is a non-optional step in Phase Completion contract |
| Config path mismatch causes loading failure | Low | Medium | Explicit config path update in On Activation step 1 |
| next-handoff double-confirmation reintroduced | Low | Medium | Explicit rule in Communication Style and On Activation |

---

## 7. Delivery Scope

### In Scope (Day 1)
- Updated `bmad-lens-techplan` SKILL.md with clean, complete activation contract
- Explicit shared utility delegation points for review-ready fast path, batch contract, and publish-before-author hook
- Config path corrected to `bmadconfig.yaml`
- Integration points table traceable to all internal dependencies
- Required frontmatter template for `architecture.md`
- Regression coverage plan for the three behavioral contracts specific to techplan

### Out of Scope (Post-Day-1)
- Shared utility skill implementations (separate features: `lens-dev-new-codebase-businessplan`, shared phase-gate utility)
- Test automation beyond focused regression coverage
- Persona overlay behavior changes (bmad-lens-theme)
