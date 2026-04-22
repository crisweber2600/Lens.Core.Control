---
feature: lens-dev-new-codebase-baseline
doc_type: research
status: draft
goal: "Technical research to ground the lens-work rewrite in the current architecture, constraints, and backwards-compat obligations"
key_decisions:
  - lifecycle.yaml v4.0 schema is the state-of-truth contract; rewrite must remain v4-compatible
  - All 17 retained published command SKILL.md contracts have been read and documented
  - Internal skill boundary (publish-to-governance CLI, light-preflight exit code) must not change
open_questions:
  - Does the new codebase target lifecycle schema v4.0 or introduce a v5.0 migration?
  - Will the BMAD skill wrappers (bmad-lens-bmad-skill) be reimplemented or reused verbatim?
  - Is dev-session.yaml checkpoint format part of the backwards-compat surface?
depends_on: [brainstorm]
blocks: []
updated_at: 2026-04-22T00:00:00Z
---

# Research — lens-work Rewrite Technical Grounding

## 1. Current Architecture Summary

### 1.1 Module Identity

- **Module code:** `lens`
- **Module version:** `4.0.0` (source: `lens.core/_bmad/lens-work/module.yaml`)
- **Lifecycle contract version:** `schema_version: 4` (source: `lifecycle.yaml`)
- **Type:** standalone module; depends on `core`, optional deps on `cis` and `tea`

### 1.2 Lifecycle Schema v4.0 Key Changes (from prior versions)

| Version | Change |
|---|---|
| v3.0 | Git-derived state replaces legacy file-based state entirely |
| v3.3 | Cross-feature visibility via feature-index.yaml, required frontmatter |
| v3.4 | 2-branch topology (featureId + featureId-plan), feature.yaml as state source |
| v3.5 | FinalizePlan replaces DevProposal + SprintPlan chain |
| v4.0 | Local personal state lives in `.lens/`; active generated docs live under `docs/` |

### 1.3 Fundamental Design Axioms (from lifecycle.yaml — non-negotiable)

| Axiom | Statement |
|---|---|
| FT1 | Planning artifacts must exist and be reviewed before code is written |
| FT2 | AI agents must work within disciplined constraints, not freestyle |
| FT3 | Multi-service initiatives must have coordinated lifecycle governance |
| A1 | Git is the only source of truth for shared workflow state. No git-ignored runtime state. |
| A2 | PRs are the only gating mechanism. No side-channel approval. |
| A3 | Authority domains must be explicit. Every file belongs to exactly one authority. |
| A4 | Sensing must be automatic at lifecycle gates, not manual discovery. |
| A5 | The control repo is an operational workspace, not a code repo. |

These axioms are constraints the rewrite inherits unchanged.

### 1.4 Related Discovery Inputs

The related feature `lens-dev-old-codebase-discovery` produced two reverse-engineered documents that are approved verification references for retained-command outcome checks and dependency coverage audits:

- `TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/deep-dive-lens-work-module.md` — command and skill inventory, lifecycle contracts, and skill-level behavioral notes
- `TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/dependency-mapping.md` — dependency graph, IPO catalog, shared resource map, and end-to-end journey flows

For the rewrite, every surviving published command should be specified from the rewrite contract first, then reviewed against approved legacy references to verify expected outcomes and dependency completeness. These documents confirm parity — they do not drive implementation.

---

## 2. Lifecycle Contract Snapshot (phases and utility surfaces the retained commands must implement)

### 2.1 Full Track Phase Order

```
preplan → businessplan → techplan → finalizeplan → dev → complete
```

### 2.2 Express Track

```
expressplan → dev → complete
```

### 2.3 Milestone Structure

| Milestone | Phases | Entry Gate |
|---|---|---|
| `techplan` | preplan, businessplan, techplan | adversarial-review (party mode) per phase |
| `finalizeplan` | finalizeplan | adversarial-review (party mode) |
| `dev-ready` | (none — gate only) | constitution-gate |
| `dev-complete` | (none — tracked state) | dev-complete-validation |

### 2.4 Phase Artifact Requirements

| Phase | Required Artifacts | Review Report |
|---|---|---|
| preplan | product-brief, research, brainstorm | preplan-adversarial-review.md |
| businessplan | prd, ux-design | businessplan-adversarial-review.md |
| techplan | architecture | techplan-adversarial-review.md |
| finalizeplan | review-report, epics, stories, implementation-readiness, sprint-status, story-files | finalizeplan-review.md |
| expressplan | (compresses all of the above) | expressplan-review.md |

All phase completions gate on `adversarial-review` mode `party`.

---

## 3. Internal Architecture: The Stub-Skill Split

### 3.1 Structure

Every user-facing command has two layers:

```
.github/prompts/lens-{command}.prompt.md   ← Stub: runs light-preflight.py, hands off
lens.core/_bmad/lens-work/prompts/         ← Thin redirect to SKILL.md
lens.core/_bmad/lens-work/skills/          ← Full implementation (SKILL.md + scripts/)
```

This means:
- **Deprecating a `.github/prompts/` stub has zero functional impact** on surviving commands
- **The SKILL.md layer is the real published API** — it must be backwards compatible
- The `lens.core/_bmad/lens-work/prompts/` middle layer is a pure redirect and can be simplified in the rewrite

### 3.2 The light-preflight.py Interface (frozen contract)

Every `.github/prompts/` stub runs:
```bash
uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py
```
- Exit 0 → proceed to skill
- Non-zero → stop and surface failure

This interface is called by **every** surviving command stub and cannot change.

### 3.3 The publish-to-governance CLI (frozen contract)

All planning phases call:
```bash
bmad-lens-git-orchestration publish-to-governance --phase {phase}
```

This is the only valid path for moving staged control-repo artifacts into the governance mirror. Direct file writes to the governance repo from phase skills are explicitly prohibited by every planning phase SKILL.md.

---

## 4. Frozen Contracts (must not change in rewrite)

### 4.1 featureId Formula

```
featureId = {normalized-domain}-{normalized-service}-{featureSlug}
```

Referenced in:
- `feature.yaml` (identity key)
- Branch names: `{featureId}`, `{featureId}-plan`
- Governance paths: `features/{domain}/{service}/{featureId}/`
- `feature-index.yaml` registry key
- Control repo docs path: `docs/{domain}/{service}/{featureId}/`

**Any derivation change breaks all existing features. This formula is frozen.**

### 4.2 2-Branch Topology

```
{featureId}-plan  →  (plan PR)  →  {featureId}  →  (final PR)  →  main
```

Created by `new-feature` (init-feature create). No other command creates feature branches. FinalizePlan validates both branches exist before proceeding.

### 4.3 feature.yaml Schema (key fields)

```yaml
featureId: {domain}-{service}-{slug}
featureSlug: {slug}
domain: {domain}
service: {service}
track: full | express
phase: preplan | businessplan | techplan | finalizeplan | expressplan | dev | complete
docs:
  path: docs/{domain}/{service}/{featureId}
  governance_docs_path: features/{domain}/{service}/{featureId}/docs
target_repos: []
```

### 4.4 feature-index.yaml Terminal States

```yaml
status: preplan | businessplan | techplan | finalizeplan | expressplan | dev | complete | archived | paused
```

`archived` is terminal — no command can transition out. All commands that read `feature-index.yaml` must treat `archived` as read-only.

### 4.5 The next-handoff Pre-Confirmed Contract

When `next` delegates to a phase skill, **that phase skill must not ask a redundant yes/no launch confirmation**. This is documented explicitly in the businessplan, techplan, and next SKILL.md files. It is a user-experience contract, not just a convention.

---

## 5. Internal Skills and Retained Command Requirement Maps

These are currently published stubs being deprecated, but their skill implementations are internal dependencies of surviving commands:

| Skill | Surviving Commands That Need It |
|---|---|
| `bmad-lens-adversarial-review` | preplan, businessplan, techplan, finalizeplan, expressplan, dev |
| `bmad-lens-git-orchestration` | new-feature, preplan, businessplan, techplan, finalizeplan, dev |
| `bmad-lens-bmad-skill` (all wrappers) | preplan, businessplan, techplan, finalizeplan, expressplan |
| `bmad-lens-quickplan` | expressplan (via bmad-lens-bmad-skill) |
| `bmad-lens-target-repo` | new-feature, dev |
| `bmad-lens-feature-yaml` | every phase skill |
| `bmad-lens-batch` | preplan, businessplan, techplan, finalizeplan (batch mode) |
| `bmad-lens-retrospective` | complete |
| `bmad-lens-document-project` | complete |
| `bmad-lens-migrate` | upgrade |
| `bmad-lens-init-feature` (fetch-context) | preplan, businessplan, techplan, finalizeplan |

---

### 5.1 Reusable Retained-Command Mapping Template

To keep the rewrite grounded in the rewrite contracts, every surviving published command should be mapped using the same BMB-derived ideation structure. The structure borrows from BMB workflow-builder Phases 1-3: discover intent, classify the workflow boundary, and gather dependencies, outputs, and validation. Old-codebase discovery artifacts are consulted after design to verify that expected outcomes and dependencies are fully covered — not before.

Every retained-command map should answer these fields:

| Field | What must be captured |
|---|---|
| Intent + boundary | What user-facing job the command owns, what phase or context gates it obeys, and what it must explicitly not mutate or bypass |
| Prompt chain | `.github/prompts/lens-{command}.prompt.md` -> `lens.core/_bmad/lens-work/prompts/lens-{command}.prompt.md` -> owning `bmad-lens-*` skill |
| Orchestration path | Internal delegates, scripts, wrappers, and external BMAD skills touched end to end |
| Shared contracts + data | Lifecycle contracts, governance files, branch rules, session files, and frozen interfaces the command depends on |
| Outputs + authority | What artifacts, state changes, PRs, or session context the command is allowed to produce and which authority owns them |
| Validation map | Existing regression tests where they exist, plus rewrite-era test obligations for uncovered behavior |

Shared prompt precondition for all 17 commands: each published prompt stub runs `uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py` before loading the release prompt. That prompt-start contract is global and is not repeated in every row below.

### 5.2 End-to-End Requirement Map for Retained Commands

| Command | Intent + boundary | Prompt + owner | End-to-end path | Shared contracts + data | Outputs + authority | Validation map |
|---|---|---|---|---|---|---|
| `preflight` | Validate workspace and onboarding state before feature work; must not mutate feature lifecycle state | `lens-preflight.prompt.md` -> `bmad-lens-onboard` | prompt stub -> release prompt -> onboard skill -> `preflight.py` and `light-preflight.py` -> guidance or setup surfaces | `bmadconfig.yaml`, `lifecycle.yaml`, governance setup paths, light-preflight exit code contract | `.lens/` and setup guidance only; no feature.yaml phase mutation | Preserve `light-preflight.py` exit semantics; keep `test-setup-control-repo.py` green; add prompt-start regression for success/failure routing |
| `new-domain` | Create a new domain scaffold and constitution entry; must not skip governance marker creation | `lens-new-domain.prompt.md` -> `bmad-lens-init-feature` (`create-domain`) | prompt stub -> release prompt -> init-feature skill -> `init-feature-ops.py create-domain` -> constitution bootstrap | domain slug rules, governance path conventions, constitution hierarchy | governance `domain.yaml`, domain constitution stub, workspace scaffold | Keep init-feature regression coverage for domain creation; add rewrite regression for domain constitution scaffold and naming stability |
| `new-service` | Create a service beneath an existing domain; must enforce domain existence and inheritance | `lens-new-service.prompt.md` -> `bmad-lens-init-feature` (`create-service`) | prompt stub -> release prompt -> init-feature skill -> `init-feature-ops.py create-service` | service slug rules, domain/service governance hierarchy, inherited constitution rules | governance `service.yaml`, service constitution stub, service scaffold | Keep init-feature regression coverage for service creation; add rewrite regression for inheritance behavior and path stability |
| `new-feature` | Create feature identity, two-branch topology, and governance registration; must preserve canonical featureId formula | `lens-new-feature.prompt.md` -> `bmad-lens-init-feature` (`create`) | prompt stub -> release prompt -> init-feature skill -> `init-feature-ops.py create` -> `bmad-lens-git-orchestration` -> optional `bmad-lens-target-repo` | frozen `featureId = {domain}-{service}-{featureSlug}`, `feature.yaml`, `feature-index.yaml`, two-branch topology, target repo inventory | governance `feature.yaml` + `feature-index.yaml` entry + `summary.md`; control repo branches `{featureId}` and `{featureId}-plan` | Preserve `test-init-feature-ops.py`; preserve git-orchestration branch tests; add rewrite regression for target-repo handoff and duplicate detection |
| `switch` | Change active feature context in session; must remain read-only against governance and code repos | `lens-switch.prompt.md` -> `bmad-lens-switch` | prompt stub -> release prompt -> switch skill -> `switch-ops.py` -> constitution/context load | `feature-index.yaml`, feature summaries, constitution hierarchy, session context conventions | session context only; no feature or branch mutation | Add rewrite regression that verifies feature-index listing, selection, and no-write behavior |
| `next` | Choose exactly one next action and auto-delegate when unblocked; must be blocker-first and never offer a menu | `lens-next.prompt.md` -> `bmad-lens-next` | prompt stub -> release prompt -> next skill -> `next-ops.py suggest` -> owning phase skill | `lifecycle.yaml` routing, `feature.yaml` phase state, blocker rules, next-handoff pre-confirmed contract | delegated skill load only; no direct artifact authoring | Preserve `test-next-ops.py`; add rewrite regression that delegated phase skills do not re-ask launch confirmation |
| `preplan` | Run analysis-phase artifact creation; must stay governance-grounded and preserve brainstorm-first behavior | `lens-preplan.prompt.md` -> `bmad-lens-preplan` | prompt stub -> release prompt -> preplan skill -> `bmad-lens-bmad-skill` -> `bmad-brainstorming`, research, product-brief flows -> adversarial review -> feature phase update | `validate-phase-artifacts.py`, `feature.yaml`, constitution rules, batch contract, review-ready fast path | control-repo `brainstorm.md`, `research.md`, `product-brief.md`, `preplan-adversarial-review.md`; phase -> `preplan-complete` | Preserve review-ready gate behavior; keep wrapper-equivalence tests; add regression for brainstorm-first sequencing and batch pass-1/pass-2 behavior |
| `businessplan` | Produce PRD and UX design after publishing reviewed preplan artifacts; must not write governance docs directly | `lens-businessplan.prompt.md` -> `bmad-lens-businessplan` | prompt stub -> release prompt -> businessplan skill -> `bmad-lens-git-orchestration publish-to-governance --phase preplan` -> `bmad-lens-bmad-skill` -> `bmad-create-prd` and `bmad-create-ux-design` -> adversarial review -> phase update | publish-to-governance CLI, preplan artifacts, `feature.yaml`, review-ready fast path, next-handoff consent | governance mirror of reviewed preplan docs; control-repo `prd.md`, `ux-design.md`, `businessplan-adversarial-review.md`; phase -> `businessplan-complete` | Preserve publish-before-author ordering; add wrapper-equivalence regressions for PRD and UX; keep zero direct governance writes |
| `techplan` | Produce architecture after businessplan is complete; must preserve PRD reference requirement and publish-before-author entry | `lens-techplan.prompt.md` -> `bmad-lens-techplan` | prompt stub -> release prompt -> techplan skill -> publish businessplan artifacts -> `bmad-lens-bmad-skill` -> `bmad-create-architecture` -> adversarial review -> phase update | `prd.md`, `ux-design.md`, `architecture.md`, publish-to-governance CLI, review-ready fast path | governance mirror of reviewed businessplan docs; control-repo `architecture.md`, `techplan-adversarial-review.md`; phase -> `techplan-complete` | Add rewrite regression for architecture `must_reference` PRD, wrapper equivalence, and publish-before-author ordering |
| `finalizeplan` | Consolidate planning, create plan PR, generate downstream bundle, and open final PR; must preserve strict step atomicity | `lens-finalizeplan.prompt.md` -> `bmad-lens-finalizeplan` | prompt stub -> release prompt -> finalizeplan skill -> publish techplan artifacts -> adversarial review -> `bmad-lens-git-orchestration` commit/push/PR -> `bmad-lens-bmad-skill` bundle for epics, stories, readiness, sprint status -> final PR | architecture + planning artifacts, plan PR topology, feature milestone updates, finalizeplan 3-step contract | control-repo bundle artifacts, plan PR `{featureId}-plan -> {featureId}`, final PR `{featureId} -> main`, `dev-ready` milestone | Preserve git-orchestration regressions; add rewrite regression for step ordering, PR topology, and no partial step skipping |
| `expressplan` | Compress planning into one express-track flow; must enforce express gating and retain internal QuickPlan behavior | `lens-expressplan.prompt.md` -> `bmad-lens-expressplan` | prompt stub -> release prompt -> expressplan skill -> `bmad-lens-bmad-skill` -> `bmad-lens-quickplan` -> adversarial review hard gate -> finalizeplan bundle reuse | express track gate in `feature.yaml`, quickplan contract, finalizeplan reuse, adversarial review hard-stop | compressed business + tech planning artifacts plus finalizeplan bundle; phase/milestone advance | Preserve internal QuickPlan retention; add rewrite regression for express-only gating, hard-stop review failure, and finalizeplan bundle delegation |
| `dev` | Execute implementation in target repos story by story; must never write code into control or release repos | `lens-dev.prompt.md` -> `bmad-lens-dev` | prompt stub -> release prompt -> dev skill -> publish finalizeplan artifacts -> `bmad-lens-git-orchestration prepare-dev-branch` -> constitution load -> subagent task execution -> code review -> final target-repo PR | `dev-session.yaml`, target repo inventory, `feature.yaml`, constitution rules, per-task commit rules | target repo branch `feature/{featureId}` or equivalent, target repo commits and PR, `.lens`/session checkpoints | Preserve `dev-session.yaml` compatibility; add rewrite regression for resume checkpoints, per-task commits, and no control-repo code writes |
| `complete` | Close the feature irreversibly; must document before archive and preserve terminal archived status | `lens-complete.prompt.md` -> `bmad-lens-complete` | prompt stub -> release prompt -> complete skill -> retrospective -> document-project -> `complete-ops.py` finalize/archive | `feature.yaml`, `feature-index.yaml`, summary/retrospective conventions, terminal archive semantics | retrospective, project docs, final summary, `feature.yaml` phase `complete`, `feature-index.yaml` status `archived` | Add rewrite regression for archive atomicity, retrospective/document-before-archive ordering, and terminal-state recognition |
| `split-feature` | Split an existing feature into a new first-class feature; must validate first and block in-progress stories from moving | `lens-split-feature.prompt.md` -> `bmad-lens-split-feature` | prompt stub -> release prompt -> split-feature skill -> `split-feature-ops.py validate-split` -> `create-split-feature` -> optional `move-stories` | source and target `feature.yaml`, `feature-index.yaml`, summary stub on `main`, validate-first contract, atomic split ordering, in-progress-blocked rule | new feature governance directory, new `feature.yaml`, new feature-index entry, summary stub, and moved story files when requested | Preserve `test-split-feature-ops.py`; add rewrite regression for validate-first gating, in-progress blockers, summary stub creation, feature-index updates, and dry-run behavior |
| `constitution` | Resolve applicable constitutions read-only; must support partial hierarchy without failure | `lens-constitution.prompt.md` -> `bmad-lens-constitution` | prompt stub -> release prompt -> constitution skill -> hierarchy resolution (org -> domain -> service -> repo) | constitution files, additive merge rules, `gate_mode`, partial-hierarchy fallback | resolved guidance only; no writes | Add rewrite regression with missing org-level constitution and additive-merge expectations |
| `discover` | Sync repo inventory between governance and local clones; must preserve governance-main auto-commit behavior | `lens-discover.prompt.md` -> `bmad-lens-discover` | prompt stub -> release prompt -> discover skill -> `discover-ops.py` -> local repo scan + clone/register loop | `repo-inventory.yaml`, `profile.yaml`, `target_projects_path`, governance-main auto-commit contract | local clone changes plus governance inventory commit on `main` | Add rewrite regression for bidirectional sync and explicit preservation of auto-commit-to-governance-main |
| `upgrade` | Apply lifecycle migrations or report no-op; must preserve v4 drop-in behavior for this rewrite | `lens-upgrade.prompt.md` -> `bmad-lens-upgrade` | prompt stub -> release prompt -> upgrade skill -> lifecycle version detection -> `bmad-lens-migrate` when needed | `lifecycle.yaml` schema version, migration table, `[LENS:UPGRADE]` commit contract, v4 drop-in policy | no-op for v4 -> v4; migration plan and commit only when schema actually changes | Preserve `test-upgrade-ops.py` no-op behavior on v4 features; add rewrite regression for migration routing only when schema differs |

The table above is the retained-command traceability baseline for the rewrite. Any reimplemented prompt, skill, or shared utility should be checked against these 17 rows before a command is declared functionally equivalent to the old codebase.

---

## 6. Shared Patterns to Extract in the Rewrite

### 6.1 Review-Ready Fast Path (copy-pasted across 3 phases — should become shared utility)

**Current state:** preplan, businessplan, and techplan each independently implement the same pattern:
1. Run `validate-phase-artifacts.py --contract review-ready`
2. If status=pass AND phase still active → skip authoring, run adversarial review immediately
3. If status=fail → continue normal authoring flow

**Rewrite opportunity:** Extract as a shared `lens-phase-gate` utility callable by all phase skills.

### 6.2 Batch Mode 2-Pass Contract (copy-pasted across 4 phases)

**Current state:** preplan, businessplan, techplan, and finalizeplan each independently implement:
- Pass 1: write `{phase}-batch-input.md`, stop
- Pass 2: resume with `batch_resume_context`, skip interactive confirmations

**Rewrite opportunity:** Extract as `bmad-lens-batch` shared lifecycle contract. The underlying `bmad-lens-batch` skill already exists — phase skills should delegate to it uniformly.

### 6.3 publish-before-author ordering (copy-pasted across businessplan, techplan, finalizeplan, dev)

**Current state:** Each phase independently performs: publish prior-phase artifacts → load them as authoring context → stage new artifacts.

**Rewrite opportunity:** Standardize as a phase-entry hook: `on_activate: publish_prior_phase_artifacts`.

---

## 7. Governance Hierarchy Findings

### 7.1 Hierarchy Levels

```
org (missing in this workspace)
└── domain: lens-dev  (constitution.md present)
    └── service: new-codebase  (constitution.md present)
        └── repo: lens.core.src  (no repo-level constitution)
```

### 7.2 Constitution Resolution Behavior

- Missing org-level is the **normal state** for most workspaces
- `bmad-lens-constitution` must handle partial hierarchy without failing
- gate_mode per article: `informational` (warning) vs `enforced` (hard gate)
- domain constitution for `lens-dev`: `gate_mode: informational`, `enforce_stories: true`, `enforce_review: true`

### 7.3 Cross-Feature Context

- `feature-index.yaml` contains 3 active features at time of research:
  - `lens-dev-release-discovery` (preplan)
  - `lens-dev-old-codebase-discovery` (expressplan)
  - `lens-dev-new-codebase-baseline` (preplan — this feature)
- All three are in the `lens-dev` domain, indicating coordinated lens-dev work
- The rewrite feature (`lens-dev-new-codebase-baseline`) is a peer of the discovery features — the discovery output should feed rewrite scoping

---

## 8. Risk Register

| Risk | Severity | Mitigations |
|---|---|---|
| featureId formula changes in rewrite | Critical | Document as explicit frozen contract; regression test against all existing features in feature-index.yaml |
| dev-session.yaml format changes | High | Version the checkpoint format; add migration in upgrade paths |
| light-preflight.py exit-code contract breaks | High | Add integration test covering all 17 stub invocations |
| QuickPlan skill deleted (breaks expressplan) | High | Retain bmad-lens-quickplan as internal module even without published stub |
| finalizeplan step atomicity broken | High | Preserve 3-step contract with explicit commit/push at step 1 boundary |
| next-handoff pre-confirmed contract not implemented | Medium | Standardize as explicit context flag in phase SKILL.md contracts |
| Partial constitution hierarchy causes failure | Medium | Add test fixture with missing org-level constitution |
| lifecycle.yaml schema_version incremented without upgrade path | Medium | If rewrite targets v5.0, add upgrade migration from v4.0 in `upgrade` skill |
| publish-to-governance CLI replaced with direct writes | Medium | Code review gate: no direct governance file writes in phase skills |

---

## 9. Open Research Questions

1. **New codebase target version:** Does the rewrite target lifecycle schema v4.0 (fully backwards compatible) or introduce v5.0 with a migration path via `upgrade`? This determines whether the rewrite is a drop-in replacement or a versioned upgrade. - it's a dropin replacement

2. **BMAD skill wrapper strategy:** Will `bmad-lens-bmad-skill` wrappers be reimplemented in the new codebase or reused verbatim? If reimplemented, each wrapper's delegation behavior (which BMAD skill it invokes, how it passes governance context) must be tested for equivalence. - It's being reimplemented as there are some noticed gaps. for example the behavior of /preplan and it moving on to brain storm and lens.core/_bmad/core/bmad-brainstorming are extremely different. 

3. **dev-session.yaml checkpoint format:** ~~Is this part of the public backwards-compat surface?~~ **ANSWERED (H2-A):** Yes. The format is backwards compatible; the rewrite must not change any checkpoint fields. Added to product-brief.md G2 frozen contracts.

4. **Batch mode architecture:** **ANSWERED (M1-D — custom definition):** Batch mode works by looking ahead at all questions the phase will ask, then writing them to a `{phase}-batch-input.md` markdown file. The user fills in the answers. The LLM reads this completed file and processes the full phase in one pass. This is a look-ahead question pre-fill pattern, not a step-by-step execution loop. The `lens-batch-contract` utility must implement this pattern faithfully for all supported phases (preplan, businessplan, techplan, finalizeplan). expressplan batch mode wraps a different unit (QuickPlan + FinalizePlan) and must be evaluated separately before inclusion.

---

## 10. Adversarial Review Format Convention

All adversarial reviews produced under this feature must present every finding using the following structured choice format. The reviewer selects a response; the agent records it and advances.

### 10.1 Required Format Per Finding

After the finding description and its severity/location metadata, every finding must include a response menu with exactly these five slots:

- **A.** *[Proposed solution 1]* — **Why pick this:** [reason] / **Why not:** [reason]
- **B.** *[Proposed solution 2]* — **Why pick this:** [reason] / **Why not:** [reason]
- **C.** *[Proposed solution 3]* — **Why pick this:** [reason] / **Why not:** [reason]
- **D.** Write your own response — provide a custom resolution not covered by A, B, or C
- **E.** Keep as-is — explicitly record that no action will be taken on this finding

Options A, B, and C are always distinct, concrete proposed resolutions — not variations of "look into it." Each must stand alone as a complete action. D and E are constant and appear verbatim on every finding.

### 10.2 Rationale

This format exists to prevent adversarial reviews from producing prose that requires more prose to resolve. By pre-proposing concrete options with their trade-offs, the review becomes a decision table. The reviewer's job is to choose, not to reason from scratch. D and E are mandatory options — a valid response is always available even when the proposed solutions are wrong.

### 10.3 Scope

This convention applies to all phase adversarial reviews: `preplan-adversarial-review.md`, `businessplan-adversarial-review.md`, `techplan-adversarial-review.md`, `finalizeplan-review.md`, and `expressplan-review.md`. It does not apply to party-mode blind-spot questions (which are rhetorical) or the carry-forward blockers summary table.

---

## Section 11: BMAD Wrapper Behavioral Delta Analysis

> **Research note** — This section was added after the initial TechPlan artifacts were completed. Section 9 Q2 acknowledged that BMAD wrappers are being reimplemented with "noticed gaps" but did not enumerate those gaps. This section documents the behavioral delta between each registered base BMAD skill and the Lens wrapper contract that must be implemented in the rewrite. All source analysis was conducted against old-codebase skill files as verification references only, per the clean-room rule.

---

### 11.1 Purpose

A clean-room implementer of `bmad-lens-bmad-skill` and its phase conductors must know, for each delegated BMAD skill, exactly what behavior the base skill provides and what additional contracts the Lens rewrite must impose. Without this documentation, developers would be forced to read old-codebase implementation files — violating the clean-room mandate.

This section provides that documentation. It is normative: any gap marked **Critical** must be resolved in the rewrite before the wrapper is considered correct.

---

### 11.2 Config Baseline (Verified)

All BMM-module skills load config from `{project-root}/_bmad/bmm/bmadconfig.yaml` at initialization. The verified values in this file are:

```yaml
planning_artifacts: "{project-root}/docs/planning-artifacts"
implementation_artifacts: "{project-root}/docs/implementation-artifacts"
output_folder: docs
```

The `bmad-brainstorming` skill (Core module) loads from `{project-root}/_bmad/core/bmadconfig.yaml`, which contains:

```yaml
output_folder: docs
```
(No `planning_artifacts` or `implementation_artifacts` — the Core config does not define them.)

**Implication:** When `lens_context.planning_artifacts` is injected by the wrapper, it must explicitly take precedence over the config-loaded `planning_artifacts` for every BMM skill. For Core module skills (brainstorming), there is no `planning_artifacts` value to override — the base skill's output path is computed from `output_folder` alone, making it fundamentally incompatible with the Lens path contract. See Section 11.5.

---

### 11.3 Config Precedence Rule (All Skills)

**Rewrite contract:** When `lens_context` is present and contains `planning_artifacts`, `output_path`, or `write_scope`, those values take absolute precedence over every config-loaded path variable. The base skill initialization must not overwrite lens_context-provided paths with config values.

This rule applies to all 11 registered skills. The rewrite must document and enforce which initialization variables are overridable by `lens_context` and in what order of precedence.

---

### 11.4 Per-Skill Delta Summary

| Skill | Phase | Base output path (from config baseline) | Lens required output path | Gap severity | Wrapper must add |
|---|---|---|---|---|---|
| `bmad-brainstorming` | preplan | `docs/brainstorming/brainstorming-session-{date}-{time}.md` | `{planning_artifacts}/brainstorm.md` | **Critical** — path architecture incompatible | Target-file override; filename normalization; Lens frontmatter injection; governance-scoped elicitation |
| `bmad-product-brief` | preplan | `{planning_artifacts}/product-brief.md` → resolves to `docs/planning-artifacts/product-brief.md` | `{planning_artifacts}/product-brief.md` | Minor — config precedence only | Config precedence enforcement; Lens frontmatter spec |
| `bmad-technical-research` / `bmad-domain-research` / `bmad-market-research` | preplan | `{planning_artifacts}/research.md` → resolves to `docs/planning-artifacts/research.md` | `{planning_artifacts}/research.md` | Minor — config precedence only | Config precedence enforcement; Lens frontmatter spec |
| `bmad-create-prd` | businessplan | `{planning_artifacts}/prd.md` → resolves to `docs/planning-artifacts/prd.md` | `{planning_artifacts}/prd.md` | Minor — config precedence only | Config precedence enforcement; Lens frontmatter spec |
| `bmad-create-ux-design` | businessplan | `{planning_artifacts}/ux-design.md` → resolves to `docs/planning-artifacts/ux-design.md` | `{planning_artifacts}/ux-design.md` | Minor — config precedence only | Config precedence enforcement; Lens frontmatter spec |
| `bmad-create-architecture` | techplan | `{planning_artifacts}/architecture.md` → resolves to `docs/planning-artifacts/architecture.md` | `{planning_artifacts}/architecture.md` | Minor — config precedence only | Config precedence enforcement; Lens frontmatter spec; target-repo path injection |
| `bmad-create-epics-and-stories` | finalizeplan | `{planning_artifacts}/epics.md` + `{planning_artifacts}/stories/*.md` | same, with `planning_artifacts` overridden to feature docs path | Minor — config precedence only | Config precedence enforcement; story naming convention (`{NNN}-{NNN}-{slug}.md`); Lens frontmatter spec |
| `bmad-check-implementation-readiness` | finalizeplan | `{planning_artifacts}/implementation-readiness.md` | same, with `planning_artifacts` overridden to feature docs path | Minor — config precedence only | Config precedence enforcement; Lens frontmatter spec |
| `bmad-sprint-planning` | finalizeplan | `story_location = {implementation_artifacts}` = `docs/implementation-artifacts`; `status_file = {implementation_artifacts}/sprint-status.yaml` = `docs/implementation-artifacts/sprint-status.yaml` | `story_location = {planning_artifacts}/stories/`; `status_file = {planning_artifacts}/sprint-status.yaml` | **Critical** — `implementation_artifacts` is not in `lens_context`; both story discovery and status output write to the wrong path | Inject `implementation_artifacts` override to `{planning_artifacts}` AND explicit `story_location = {planning_artifacts}/stories/` and `status_file = {planning_artifacts}/sprint-status.yaml` overrides |
| `bmad-create-story` | finalizeplan / dev | `{planning_artifacts}/stories/{filename}` | `{planning_artifacts}/stories/{NNN}-{NNN}-{slug}.md` | Minor — naming convention | Config precedence enforcement; story naming convention injection |

---

### 11.5 The Brainstorming Delta (Critical)

**Base behavior (verified):** `bmad-brainstorming` computes its output path as:
```
{output_folder}/brainstorming/brainstorming-session-{date}-{time}.md
```
Using the Core config baseline, this resolves to:
```
docs/brainstorming/brainstorming-session-2026-04-22-1430.md
```

The path has three problems for Lens:
1. The subdirectory is `brainstorming/`, not the feature's docs path.
2. The filename is a timestamped session name, not the canonical `brainstorm.md`.
3. The base skill has no `planning_artifacts` variable — the Core config does not define it — so no `lens_context.planning_artifacts` injection can redirect the base path. The path formula `{output_folder}/brainstorming/...` is computed from `output_folder` only.

**Additional behavioral delta:** The base `bmad-brainstorming` skill targets generative ideation with a 100+ ideas goal and anti-bias protocol. Lens preplan brainstorming is governance-grounded feature ideation: constitution context and cross-feature state are loaded before delegation; the scope is feature-specific exploration, not open-ended creative generation.

**Rewrite options — the wrapper must resolve this with one of:**

**Option A — Wrapper-level path injection:** Extend `lens_context` with an explicit `brainstorm_target_file` field set to `{planning_artifacts}/brainstorm.md`. The base skill's initialization must check for `brainstorm_target_file` and use it instead of computing the timestamped path. This requires modifying the base skill's initialization to accept target-file override — not clean-room if it requires adapting the old brainstorming skill in place.

**Option B — Post-processing normalization:** The wrapper allows the base skill to write its timestamped output, then runs a post-processing step that: (a) reads the timestamped file, (b) prepends Lens frontmatter, (c) writes the content to `{planning_artifacts}/brainstorm.md`, (d) deletes the timestamped file. This works without modifying the base skill but adds fragility (path-discovery of the timestamped file depends on consistent date format).

**Option C — Lens-native brainstorming skill (recommended):** The rewrite implements `bmad-lens-brainstorming` as a purpose-built skill that uses the feature context loaded by the phase conductor, targets `{planning_artifacts}/brainstorm.md` as its canonical output path, injects required Lens frontmatter, and scopes ideation to governance-bounded feature exploration. The creative technique library from the base skill's step files may be used as a verification reference for expected technique coverage. This is the cleanest alignment with the clean-room mandate: it produces a new artifact rather than patching a generic one.

**Recommendation:** Option C. Document the decision in the rewrite's `bmad-lens-preplan` SKILL.md. The `lens-bmad-skill-registry.json` entry for `bmad-brainstorming` should be updated in the rewrite to point to `bmad-lens-brainstorming` if Option C is chosen.

---

### 11.6 The Sprint Planning Delta (Critical)

**Base behavior (verified):** `bmad-sprint-planning` reads two path variables from `bmadconfig.yaml`:
- `story_location = {implementation_artifacts}` → resolves to `docs/implementation-artifacts/`
- `status_file = {implementation_artifacts}/sprint-status.yaml` → resolves to `docs/implementation-artifacts/sprint-status.yaml`
- `epics_location = {planning_artifacts}` → resolves to `docs/planning-artifacts/` (path override works here)

**Lens actual paths (confirmed):**
- Stories: `docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/stories/` — under `planning_artifacts`
- Sprint status: `docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/sprint-status.yaml` — under `planning_artifacts`

**Root cause:** The current `lens_context` block injected by `bmad-lens-bmad-skill` contains `planning_artifacts` but **does not** contain `implementation_artifacts`. The sprint-planning skill reads `story_location` and `status_file` from `{implementation_artifacts}`, which is loaded from bmadconfig.yaml with no `lens_context` override available.

**Rewrite contract:** The wrapper's `lens_context` block must be extended to include:
```
implementation_artifacts: {planning_artifacts}   # redirect to feature docs path
story_location: {planning_artifacts}/stories/    # explicit override
status_file: {planning_artifacts}/sprint-status.yaml  # explicit override
```

All three overrides must take precedence over config-loaded values via the config precedence rule (Section 11.3).

Note on epics discovery: the base skill uses a glob pattern `*epic*.md` to find epics. Since the Lens epics file is named `epics.md`, this pattern matches (`epics` contains the substring `epic`). No additional override is required for epics discovery.

---

### 11.7 Lens Frontmatter Specification (All Planning-Docs Skills)

All planning-docs skills — when running under `lens_context` — must produce output with the following frontmatter fields in addition to whatever the base skill generates:

```yaml
---
feature: {feature_id}
doc_type: brainstorm | product-brief | research | prd | ux-design | architecture | epics | stories | implementation-readiness | sprint-status
status: draft
updated_at: {ISO-8601 timestamp}
---
```

The `key_decisions`, `open_questions`, `depends_on`, and `blocks` fields are present in current feature docs but are not required from the wrapper — the base skill's interactive elicitation may produce some of them, and the phase conductor fills in the rest. The `feature` and `doc_type` fields are the minimum wrapper-injected additions.

**Rewrite contract:** `bmad-lens-bmad-skill` must post-process each planning-docs output to ensure `feature` and `doc_type` are present in frontmatter, and must set or update `updated_at` on every write.

---

### 11.8 Summary of Rewrite Contracts

| Contract | Scope | Priority |
|---|---|---|
| Config precedence rule: `lens_context` paths win over config-loaded values | All 11 registered skills | Required |
| Brainstorming path normalization: canonical `brainstorm.md` output | `bmad-brainstorming` | Critical |
| Brainstorming governance scope: feature-bounded elicitation, not open-ended ideation | `bmad-brainstorming` | Critical |
| `implementation_artifacts` override added to `lens_context` | `bmad-sprint-planning` | Critical |
| `story_location` explicit override → `{planning_artifacts}/stories/` | `bmad-sprint-planning` | Critical |
| `status_file` explicit override → `{planning_artifacts}/sprint-status.yaml` | `bmad-sprint-planning` | Critical |
| Lens frontmatter injection: `feature` + `doc_type` + `updated_at` minimum | All planning-docs skills | Required |
| Story naming convention: `{NNN}-{NNN}-{slug}.md` | `bmad-create-epics-and-stories`, `bmad-create-story` | Required |
| Target-repo path injection for architecture context | `bmad-create-architecture` | Required |
