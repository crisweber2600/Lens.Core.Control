---
feature: lens-dev-new-codebase-baseline
doc_type: research
status: draft
goal: "Technical research to ground the lens-work rewrite in the current architecture, constraints, and backwards-compat obligations"
key_decisions:
  - lifecycle.yaml v4.0 schema is the state-of-truth contract; rewrite must remain v4-compatible
  - All 16 published command SKILL.md contracts have been read and documented
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

---

## 2. Lifecycle Contract Snapshot (phases the 16 commands must implement)

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

## 5. Internal Skills That Must Survive as Non-Published Modules

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
| light-preflight.py exit-code contract breaks | High | Add integration test covering all 16 stub invocations |
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
