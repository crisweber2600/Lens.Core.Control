---
feature: lens-dev-new-codebase-baseline
doc_type: product-brief
status: draft
goal: "Define the product vision, user value, and success criteria for the lens-work rewrite"
key_decisions:
  - Rewrite scope: 17 published commands, explicitly retaining split-feature, 100% backwards compatibility, no user-visible behavior changes
  - Deprioritized commands: all deprecated stubs — removed from .github/prompts/, retained as internal skills
  - Internal skill consolidation: shared utilities extracted from copy-pasted phase skill patterns
open_questions:
  - Does the rewrite target lifecycle schema v4.0 (drop-in) or v5.0 (versioned migration)?
  - Will the rewrite ship as a new Lens.Core.Release tag, or as a breaking change requiring user upgrade?
depends_on: [brainstorm, research]
blocks: []
updated_at: 2026-04-22T00:45:00Z
---

# Product Brief — lens-work Rewrite: 17-Command Stable Surface

## 1. Problem Statement

The current lens-work codebase has accumulated significant surface area: 54 published `.github/prompts/` stubs covering commands ranging from core lifecycle operations to rarely-used utilities. Many of these stubs are cosmetic wrappers for internal skills that users rarely invoke directly. The resulting command surface is hard to navigate, hard to maintain, and hard to onboard onto.

At the same time, the internal implementation of lens-work has several copy-pasted patterns (review-ready fast path, batch mode 2-pass contract, publish-before-author ordering) that exist independently in each phase skill rather than as shared utilities. This makes behavioral consistency across phases brittle — a fix in one phase's review-ready logic does not propagate to others.

The objective of this rewrite is to **reduce surface area without changing any user-observable behavior**, and to **consolidate internal patterns** without breaking existing features or workflows.

---

## 2. Target Users

| User | Current Pain | Rewrite Benefit |
|---|---|---|
| **New Lens users** | 54 commands with no clear entry point; `/help` surfaces rarely-used commands alongside core workflow commands | 17 clearly-named commands with natural progression plus feature reshaping (`new-domain → new-service → new-feature → next`, plus `split-feature` when scope must be carved out) |
| **Existing Lens users (mid-feature)** | No behavioral change expected; must be able to resume existing features and in-progress dev sessions without rerun | Zero behavioral regression; all existing feature.yaml, branch topology, and session state must remain readable |
| **Lens module maintainers** | Fixing a phase contract bug requires updating 3-4 independent SKILL.md implementations of the same pattern | Shared utilities (`lens-phase-gate`, batch lifecycle contract, publish-entry hook) are single-source-of-truth |
| **bmad-work-system integrators** | Unclear which prompts are load-bearing vs cosmetic; hard to determine what can be safely ignored | Explicit split: 16 published (user-facing) + N internal skills (not published stubs) |

---

## 3. Goals

### G1 — Surface Reduction
Reduce published `.github/prompts/` stubs from 54 to 17. The 17 surviving commands cover the complete lifecycle for both full and express tracks while retaining `split-feature` as the published feature-reshaping workflow.

### G2 — 100% Backwards Compatibility
All existing features (tracked in `feature-index.yaml`), `feature.yaml` schemas, branch topologies, governance path conventions, and in-progress dev sessions must be fully operational after the rewrite. Zero migration required for users on the current codebase who upgrade.

**Critical frozen contracts:**
- featureId formula: `{domain}-{service}-{featureSlug}` — immutable
- 2-branch topology: `{featureId}-plan` → `{featureId}` → `main` — immutable
- `light-preflight.py` exit-code interface — immutable
- `publish-to-governance` CLI as the only governance write path — immutable
- `next` → phase skill handoff without redundant confirmation — immutable
- `dev-session.yaml` checkpoint format — immutable (backwards compatible; rewrite must not change any checkpoint fields)

### G3 — Internal Consolidation (no user-visible behavior change)
Three copy-pasted patterns extracted as shared utilities:
1. **Review-ready fast path** — shared across preplan, businessplan, techplan, (expressplan)
2. **Batch mode 2-pass contract** — shared across preplan, businessplan, techplan, finalizeplan
3. **Publish-before-author entry hook** — shared across businessplan, techplan, finalizeplan, dev

### G4 — Deprecated Stubs With Retained Internals
All deprecated `.github/prompts/` stubs are removed. Their underlying skill implementations are explicitly retained as internal modules. A definitive inventory distinguishes "can be fully removed" from "stub removed but skill retained."

### G5 — Explicit Rewrite Target Version
The rewrite targets lifecycle schema **v4.0 (drop-in)**. `schema_version: 4` is the fixed target. No upgrade path is required; no schema fields in `feature.yaml`, `feature-index.yaml`, or `dev-session.yaml` may change during the rewrite. This was confirmed during PrePlan (C1-A resolved).

### G6 — End-to-End Requirement Mapping For Retained Commands
Every surviving published Lens prompt and its kept owning skill must have a documented end-to-end requirement map. Each map must trace the user-facing command from prompt stub to prompt redirect, owning SKILL.md contract, internal delegates and scripts, shared data contracts, artifact outputs, governance touchpoints, and regression coverage. The mapping baseline comes from the related `lens-dev-old-codebase-discovery` deep-dive and dependency-mapping docs, and the decomposition for each retained command should follow a BMB-style build rubric: intent, boundaries, dependencies, outputs, and validation.

---

## 4. Non-Goals

- **No new commands.** The rewrite adds nothing to the 17-command surface. New commands are a future feature.
- **No new lifecycle phases.** The full-track (preplan → businessplan → techplan → finalizeplan → dev → complete) and express-track (expressplan → dev → complete) are unchanged.
- **No featureId formula changes.** This is explicitly out of scope.
- **No governance schema changes.** `feature.yaml`, `feature-index.yaml`, and `repo-inventory.yaml` schemas do not change.
- **No target-repo branching topology changes.** `feature/{featureSlug}` branches in target repos are out of scope.
- **No BMAD core module changes.** This rewrite is limited to `lens-work`. Changes to `core`, `cis`, or `tea` modules are not in scope.
- **No in-stub deprecation notices.** Deprecated stubs are removed without runtime warnings. Removals are communicated in release notes only.
- **No new-project replacement stub.** `new-project` is fully removed. Its bootstrapping guidance is documented in `new-domain`'s SKILL.md; no runtime stub is added.

---

## 5. Success Criteria

| Criterion | Measure |
|---|---|
| Published command count | Exactly 17 `.github/prompts/` stubs on completion |
| Backwards compatibility | All features in `feature-index.yaml` at time of rewrite remain fully operational |
| featureId regression | `test-init-feature-ops.py` passes unchanged |
| git-orchestration regression | `test-git-orchestration-ops.py` passes unchanged |
| next-action regression | `test-next-ops.py` passes unchanged |
| preflight regression | `test-setup-control-repo.py` passes unchanged |
| Internal skill inventory | Definitive "retained as internal" vs "fully removed" list documented and verified |
| Shared utility coverage | Review-ready fast path, batch contract, and publish-entry hook each have single implementations |
| Schema version declared | `module.yaml` and `lifecycle.yaml` explicitly declare rewrite target schema version |
| Upgrade regression | `test-upgrade-ops.py` dry-run on a v4 feature reports no-op |
| BMAD wrapper delegation equivalence | Per-phase test confirms each reimplemented wrapper produces equivalent output to its BMAD counterpart |
| End-to-end retained-command traceability | All 17 surviving published commands have a documented requirement map covering stub, skill, delegates, scripts, data contracts, outputs, governance touchpoints, and validation |
| No governance direct writes | Code review confirms zero phase-skill direct writes to governance repo files |

---

## 6. Command Surface Map

### 6.1 Surviving Published Commands (17)

| Command | Track | Phase | Primary User Action |
|---|---|---|---|
| `preflight` | all | onboarding | Validate workspace before first use |
| `new-domain` | all | scaffolding | Create a new domain in governance |
| `new-service` | all | scaffolding | Create a new service under a domain |
| `new-feature` | all | scaffolding | Create a new feature with 2-branch topology |
| `switch` | all | navigation | Switch active feature in session |
| `next` | all | navigation | Get the one opinionated next action |
| `preplan` | full | analysis | Brainstorm, research, product brief |
| `businessplan` | full | planning | PRD, UX design |
| `techplan` | full | planning | Architecture document |
| `finalizeplan` | full | planning | Consolidate, bundle, open final PR |
| `expressplan` | express | planning | Compressed single-session planning |
| `dev` | all | execution | AI-guided story-by-story development |
| `complete` | all | closure | Retrospective, document, archive |
| `split-feature` | all | reshaping | Split a feature into a new initiative and move eligible stories |
| `constitution` | all | governance | Resolve 4-level constitution hierarchy |
| `discover` | all | governance | Sync repo inventory bidirectionally |
| `upgrade` | all | maintenance | Apply lifecycle schema migrations |

### 6.2 Track Coverage

```
Full track:
  preflight → new-domain → new-service → new-feature
  → preplan → businessplan → techplan → finalizeplan
  → dev → complete

Express track:
  preflight → new-domain → new-service → new-feature
  → expressplan → dev → complete

Cross-cutting (any time):
  switch, next, split-feature, constitution, discover, upgrade
```

---

## 7. Key Architecture Decisions

### 7.1 Internal Skill Inventory Policy

The rewrite introduces an explicit two-category classification for all skills:

| Category | Published Stub | Skill Retained |
|---|---|---|
| **Published command** | Yes (17 total) | Yes |
| **Internal module** | No | Yes (required by surviving commands) |
| **Fully removed** | No | No (no surviving command depends on it) |

No skill that appears in the dependency table of any surviving command may be fully removed. The inventory (established in brainstorm.md) is the authoritative reference.

### 7.2 Schema Version Strategy (closed — v4.0 drop-in)

**Decision (PrePlan C1-A):** The rewrite targets v4.0 drop-in. `schema_version: 4` is the fixed target. Users who pull the new release need no upgrade action. No schema fields in `feature.yaml`, `feature-index.yaml`, or `dev-session.yaml` may change. The `upgrade` command reports a no-op for v4→v4.

### 7.3 Shared Utility Extraction Scope

Three utilities are confirmed for extraction:

1. **`lens-phase-gate` utility** — wraps `validate-phase-artifacts.py` invocation + review-ready fast path logic. Called by preplan, businessplan, techplan, (expressplan).

2. **`lens-batch-contract` utility** — look-ahead pre-fill pattern: on first pass, the skill scans ahead to identify all questions it will ask and writes them to `{phase}-batch-input.md`; the user fills in answers offline; on second pass, the LLM reads the completed file and processes the full phase in one pass. Called by preplan, businessplan, techplan, finalizeplan. expressplan batch mode requires separate evaluation before inclusion (M1-D).

3. **`lens-phase-entry-hook` utility** — wraps publish-prior-phase-artifacts + governance context load. Called at entry to businessplan, techplan, finalizeplan, dev.

Each utility is an internal module, not a published command.

---

## 8. Stakeholder Constraints

- **Lens.Core.Release (`alpha` branch):** The rewrite output will be promoted to the release repo. The PR is already open ([PROMOTE] Full BMAD + lens-work v). The rewrite must produce a clean publishable artifact.
- **Existing features in `feature-index.yaml`:** `lens-dev-release-discovery`, `lens-dev-old-codebase-discovery`, and `lens-dev-new-codebase-baseline` must all be operable after the rewrite ships. The first two are in active use and cannot be disrupted.
- **Old-codebase discovery corpus:** `deep-dive-lens-work-module.md` and `dependency-mapping.md` from `lens-dev-old-codebase-discovery` are the authoritative reverse-engineered baseline for retained-command behavior, dependency chains, and end-to-end mapping coverage.
- **Installed prompt surface parity:** `setup.py`, installer surfaces, and module help currently ship `lens-split-feature.prompt.md`. The rewrite must either retain that command consistently across all surfaces or explicitly remove it everywhere. This spec chooses retention.
- **Mid-phase transition model:** Phase skills are stateless at their entry point. A user mid-phase on the old codebase can resume by re-entering the phase skill from its start. No mid-phase checkpoint migration is required (H1-A).
- **`discover` auto-commit behavior:** The `discover` command's auto-commit to governance `main` outside lifecycle phase transitions is preserved as-is. This behavior is not subject to the `publish-entry-hook` consolidation (L1-A).

---

## 9. Risks Summary

| Risk | Impact | Owner |
|---|---|---|
| featureId formula drift | Critical — breaks all existing features | TechPlan: freeze as architectural axiom |
| Schema version ambiguity | High — wrong option strands active features | TechPlan: explicit decision required |
| QuickPlan skill deleted (silently breaks expressplan) | High | FinalizePlan: retained-internal inventory check |
| split-feature removed while install/help surfaces still publish it | High — retained surface becomes internally inconsistent | TechPlan: keep installer, help, docs, and regression coverage aligned |
| dev-session.yaml checkpoint incompatibility | High — breaks active dev sessions | TechPlan: version checkpoint format |
| Review-ready utility diverges from phase behavior | Medium | TechPlan: define shared utility contract |
| next-handoff contract not standardized | Medium — degrades UX | TechPlan: define handoff flag |

---

## 10. Review Process Conventions

### 10.1 Adversarial Review Response Format

All adversarial reviews produced for this feature must present each finding as a structured choice set. The reviewer selects a response option; they are not required to write prose resolutions.

**Required format for every finding:**

- **A.** *[Proposed solution 1]* — **Why pick this:** [reason] / **Why not:** [reason]
- **B.** *[Proposed solution 2]* — **Why pick this:** [reason] / **Why not:** [reason]
- **C.** *[Proposed solution 3]* — **Why pick this:** [reason] / **Why not:** [reason]
- **D.** Write your own response
- **E.** Keep as-is — no action taken on this finding

A, B, and C are concrete proposed resolutions with their trade-offs stated explicitly. D is always available for a custom response. E is always available to explicitly record a non-action decision.

This convention applies to all phase adversarial reviews in this feature. See `research.md §10` for the full rationale and scope definition.
