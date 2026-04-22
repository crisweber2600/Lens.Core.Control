---
feature: lens-dev-new-codebase-baseline
doc_type: brainstorm
status: draft
goal: "Define the 16-command reduced surface for lens-work rewrite with 100% backwards compatibility"
key_decisions:
  - 16 surviving published commands confirmed (switch, new-domain, new-service, new-feature, preflight, upgrade, preplan, businessplan, techplan, finalizeplan, expressplan, dev, complete, constitution, discover, next)
  - new-feature is canonical alias; init-feature is deprecated
  - preflight survives; onboard prompt deprecated (onboard skill retained internally)
  - new-project removed with no replacement
  - businessplan, techplan, finalizeplan retained (full track preserved)
  - All non-published internal skills (adversarial-review, git-orchestration, bmad-skill, etc.) retained as internal modules
open_questions:
  - Should new-domain serve as the entry point for brand-new project bootstrapping (replacing new-project)?
  - QuickPlan skill must remain internal for expressplan — confirm it is not deleted in rewrite
  - Confirm onboard vs preflight do not have overlapping workspace-setup responsibilities that need explicit merging
depends_on: []
blocks: []
updated_at: 2026-04-22T00:00:00Z
---

# Brainstorm — lens-work Rewrite: 16-Command Surface with 100% Backwards Compatibility

## Session Context

**Topic:** Rewriting lens-work with 100% backwards compatibility, evaluating the reduced published command surface.  
**Goal:** Outline what each surviving command does and ensure the contracts are correct before rewriting.

---

## Surviving Published Commands (16)

The following `.github/prompts/` stubs are retained as user-facing commands. All others are deprecated as published prompts but their underlying skill implementations are retained as internal modules.

| # | Command | Prompt File | Underlying Skill |
|---|---|---|---|
| 1 | `switch` | `lens-switch.prompt.md` | `bmad-lens-switch` |
| 2 | `new-domain` | `lens-new-domain.prompt.md` | `bmad-lens-init-feature` (create-domain) |
| 3 | `new-service` | `lens-new-service.prompt.md` | `bmad-lens-init-feature` (create-service) |
| 4 | `new-feature` | `lens-new-feature.prompt.md` | `bmad-lens-init-feature` (create) |
| 5 | `preflight` | `lens-preflight.prompt.md` | `bmad-lens-onboard` + scripts |
| 6 | `upgrade` | `lens-upgrade.prompt.md` | `bmad-lens-upgrade` |
| 7 | `preplan` | `lens-preplan.prompt.md` | `bmad-lens-preplan` |
| 8 | `businessplan` | `lens-businessplan.prompt.md` | `bmad-lens-businessplan` |
| 9 | `techplan` | `lens-techplan.prompt.md` | `bmad-lens-techplan` |
| 10 | `finalizeplan` | `lens-finalizeplan.prompt.md` | `bmad-lens-finalizeplan` |
| 11 | `expressplan` | `lens-expressplan.prompt.md` | `bmad-lens-expressplan` |
| 12 | `dev` | `lens-dev.prompt.md` | `bmad-lens-dev` |
| 13 | `complete` | `lens-complete.prompt.md` | `bmad-lens-complete` |
| 14 | `constitution` | `lens-constitution.prompt.md` | `bmad-lens-constitution` |
| 15 | `discover` | `lens-discover.prompt.md` | `bmad-lens-discover` |
| 16 | `next` | `lens-next.prompt.md` | `bmad-lens-next` |

---

## Deprecation Structural Finding

All `.github/prompts/` files are pure stubs — they run `light-preflight.py` and hand off to `lens.core/_bmad/lens-work/`. The real logic lives entirely in SKILL.md files. **Removing a `.github/prompts/` stub has zero impact on the internal functioning of any surviving command.** Deprecated internal skills (adversarial-review, git-orchestration, bmad-skill wrappers, etc.) must be retained as non-published internal modules callable by surviving skills.

---

## Command-by-Command Outline

### 1. `switch` → `bmad-lens-switch`

**What it does:** Session context switcher. Reads `feature-index.yaml`, presents a numbered menu of active features, loads cross-feature context for the selected one. Pure read — never modifies state.

**Key behaviors:**
- Numbered menu from feature-index.yaml
- Sets `branch_switched` field in session context
- Auto-loads constitution and governance docs for the newly selected feature

**Internal deps:** `switch-ops.py`, `feature-index.yaml`

**Backwards compat risk:** Low. No state mutations. Rewrite concern: if `feature-index.yaml` schema changes, menu rendering breaks.

---

### 2. `new-domain` → `bmad-lens-init-feature` `create-domain`

**What it does:** Creates `domain.yaml`, domain constitution stub, and workspace scaffold for a new domain.

**Key behaviors:**
- Derives domain slug
- Writes governance marker files
- Prompts for constitution bootstrap content

**Internal deps:** `init-feature-ops.py` (create-domain subcommand), `bmad-lens-constitution`

**Backwards compat risk:** Medium. The domain.yaml schema must remain stable. `new-domain` must remain a valid top-level user alias even if the underlying subcommand routing changes.

---

### 3. `new-service` → `bmad-lens-init-feature` `create-service`

**What it does:** Creates `service.yaml`, service constitution stub, scaffolds under an existing domain.

**Key behaviors:**
- Validates domain exists before proceeding
- Inherits domain constitution
- Writes service governance marker

**Internal deps:** `init-feature-ops.py` (create-service subcommand)

**Backwards compat risk:** Medium. Service constitution inheritance rules must be preserved. Same alias concern as new-domain.

---

### 4. `new-feature` → `bmad-lens-init-feature` `create` *(canonical; `init-feature` is the deprecated alias)*

**What it does:** Progressive disclosure feature creation. Derives canonical `featureId = {domain}-{service}-{featureSlug}`, creates 2-branch topology (`{featureId}` + `{featureId}-plan`), writes `feature.yaml`, registers in `feature-index.yaml`, writes initial `summary.md`, opens plan PR.

**Key behaviors:**
- featureId derivation from domain + service + featureSlug
- 2-branch topology creation
- feature-index.yaml registration
- Track selection (full vs express)

**Internal deps:** `init-feature-ops.py`, `bmad-lens-git-orchestration` (branch creation), `bmad-lens-target-repo` (repo provisioning — internal even if prompt deprecated)

**Backwards compat risk:** HIGH. The featureId formula (`{domain}-{service}-{featureSlug}`) is referenced across feature.yaml, branch names, governance paths, and all 16 commands. **The featureId formula must be frozen.** Any derivation change breaks all existing features.

---

### 5. `preflight` → `bmad-lens-onboard` + scripts *(onboard prompt deprecated; preflight is canonical)*

**What it does:** Workspace validation — checks governance repo structure, lifecycle.yaml currency, required tooling, surfaces role-aware onboarding guidance.

**Key behaviors:**
- Three overlapping scripts: `preflight.py`, `light-preflight.py`, onboard SKILL.md
- `light-preflight.py` is the fast cache-aware check invoked by every `.github/prompts/` stub
- Role-aware guidance surfaces different content depending on workspace state

**Internal deps:** `preflight.py`, `light-preflight.py`, `bmadconfig.yaml`

**Backwards compat risk:** Medium. **The `light-preflight.py` exit-code interface is frozen** — every stub calls it (exit 0 = proceed, non-zero = stop). Rewrite opportunity: consolidate the 3-script surface into one unified preflight implementation.

---

### 6. `upgrade` → `bmad-lens-upgrade`

**What it does:** Schema migration — detects installed lifecycle version, computes upgrade plan from `lifecycle.yaml` migration table, confirms with user, applies with `[LENS:UPGRADE]` commit. Routes to `bmad-lens-migrate` internally for legacy branch schema cases.

**Key behaviors:**
- Version detection
- Migration plan display and dry-run
- `[LENS:UPGRADE]` commit tag on apply
- `bmad-lens-migrate` routing (internal — migrate prompt is deprecated but skill retained)

**Internal deps:** `bmad-lens-migrate` (internal), `lifecycle.yaml` migrations table

**Backwards compat risk:** Low for users. High for rewrite: if the new codebase changes `lifecycle.yaml` structure or version numbering, upgrade paths must explicitly cover the gap from old to new schema.

---

### 7. `preplan` → `bmad-lens-preplan`

**What it does:** Brainstorm-first phase conductor. Starts with BMAD brainstorm setup questions, delegates artifact synthesis through `bmad-lens-bmad-skill` wrappers (brainstorm, research, product-brief). Stages `brainstorm.md`, `research.md`, `product-brief.md` in control repo. Runs adversarial review gate. Updates feature phase to `preplan-complete`.

**Key behaviors:**
- Brainstorm-first enforcement — setup questions before any artifact choice
- Governance-only grounding — never inspects implementation code
- Review-ready fast path — if `validate-phase-artifacts.py` returns pass, skip to adversarial review immediately
- Batch mode (2-pass): pass 1 writes `preplan-batch-input.md`, pass 2 resumes with approved answers

**Internal deps:** `bmad-lens-bmad-skill`, `bmad-lens-adversarial-review`, `bmad-lens-git-orchestration` (staging commits), `bmad-lens-constitution`, `validate-phase-artifacts.py`

**Backwards compat risk:** Low for users. Review-ready fast path contract must be preserved exactly.

---

### 8. `businessplan` → `bmad-lens-businessplan`

**What it does:** PRD + UX design phase. Publishes reviewed preplan artifacts to governance first (`publish-to-governance --phase preplan`), then delegates to `bmad-create-prd` and/or `bmad-create-ux-design` via wrappers. Stages `prd.md` and `ux-design.md`.

**Key behaviors:**
- Workflow selection menu: prd / ux-design / both
- Publish-before-author ordering (publish reviewed preplan to governance before staging businessplan artifacts)
- Next-handoff pre-confirmed contract — no redundant yes/no if delegated from `next`
- Review-ready fast path

**Internal deps:** `bmad-lens-bmad-skill`, `bmad-lens-adversarial-review`, `bmad-lens-git-orchestration` (`publish-to-governance`), `bmad-lens-constitution`

**Backwards compat risk:** Low. Critical constraint: businessplan must NEVER create governance files directly — publish-to-governance CLI is the only valid path.

---

### 9. `techplan` → `bmad-lens-techplan`

**What it does:** Architecture design phase. Publishes reviewed businessplan artifacts to governance, delegates to `bmad-create-architecture` via wrapper. Stages `architecture.md`.

**Key behaviors:**
- PRD reference required (artifact_validation rule)
- Publish-before-author ordering
- Review-ready fast path
- **TechPlan is the final milestone-completion trigger for the planning milestone** — phase-state transition to `techplan-complete` gates finalizeplan entry

**Internal deps:** `bmad-lens-bmad-skill`, `bmad-lens-adversarial-review`, `bmad-lens-git-orchestration` (`publish-to-governance`), `bmad-lens-constitution`

**Backwards compat risk:** Low. Milestone-completion transition must be preserved in rewrite.

---

### 10. `finalizeplan` → `bmad-lens-finalizeplan`

**What it does:** Three-step planning consolidation. Step 1: adversarial review + governance cross-check → commit/push on `{featureId}-plan`. Step 2: merge plan PR (`{featureId}-plan` → `{featureId}`). Step 3: downstream bundle (epics, stories, implementation-readiness, sprint-planning, story-files) → open final PR (`{featureId}` → `main`).

**Key behaviors:**
- Strict step ordering — no skipping or reordering
- No ad-hoc branch creation — branches must exist from `new-feature` init
- Publish TechPlan artifacts to governance at phase entry
- Bundle runs only after plan PR confirmed in step 2
- Step 3 opens final PR before dev can begin

**Internal deps:** `bmad-lens-adversarial-review`, `bmad-lens-git-orchestration` (commit, push, PR, publish-to-governance), `bmad-lens-bmad-skill` (epics/stories/readiness/sprint/story-files wrappers)

**Backwards compat risk:** HIGH. Most complex phase. The 3-step contract, bundle ordering, and PR topology are all load-bearing. **Rewrite must preserve step atomicity** — partial finalizeplan runs leave the repo in an ambiguous state.

---

### 11. `expressplan` → `bmad-lens-expressplan`

**What it does:** Express track only. Compresses the full planning track into one session: QuickPlan → adversarial review (hard gate) → FinalizePlan bundle. Single-session alternative to the full preplan → businessplan → techplan → finalizeplan chain.

**Key behaviors:**
- Express track gating — validates `feature.track=express`
- QuickPlan via `bmad-lens-bmad-skill` wrapper (QuickPlan skill retained internally even though prompt deprecated)
- Adversarial review as a hard gate (not a warning)
- FinalizePlan bundle runs inline

**Internal deps:** `bmad-lens-bmad-skill` (QuickPlan wrapper), `bmad-lens-adversarial-review`, `bmad-lens-finalizeplan` (bundle reuse), `bmad-lens-git-orchestration`

**Backwards compat risk:** Medium. **QuickPlan skill must be retained as an internal module** even though `lens-quickplan.prompt.md` is deprecated. If the QuickPlan skill is deleted, expressplan breaks silently.

---

### 12. `dev` → `bmad-lens-dev`

**What it does:** Delegation-only phase conductor. Publishes FinalizePlan artifacts to governance first, resolves target repo, prepares working branch (`feature/{featureId}` by default), delegates every task via `runSubagent`, per-task commits, adversarial code review after each story, final party-mode blind-spot challenge, opens single PR to target repo default branch.

**Key behaviors:**
- **NOT a lifecycle phase** — validates finalizeplan is complete as a pre-condition but creates no initiative branch
- Repo-scoped branching memory persisted per target repo in `dev-session.yaml`
- All code writes go to target repo — NEVER to control repo or release repo
- Per-task commits (never batched)
- Single final PR only (no story-level or epic-level PRs)
- `dev-session.yaml` checkpoint for crash recovery and resumability

**Internal deps:** `bmad-lens-git-orchestration` (`prepare-dev-branch`, `publish-to-governance`, commit, PR), `bmad-lens-adversarial-review`, `bmad-lens-constitution`, `bmad-lens-feature-yaml`

**Backwards compat risk:** Medium-high. Crash-recovery semantics of `dev-session.yaml` are critical. Rewrite must preserve resumability — a partially-run dev session must be continuable from the last checkpoint.

---

### 13. `complete` → `bmad-lens-complete`

**What it does:** Irreversible lifecycle endpoint. Sequence: check-preconditions → retrospective → document-project → finalize (archive). Writes final `summary.md`, updates `feature.yaml` phase to `complete`, updates `feature-index.yaml` status to `archived`.

**Key behaviors:**
- Retrospective-first (can be explicitly skipped with user confirmation)
- Document-before-archive — project docs must be captured before status changes
- Atomic archive write — feature-index + feature.yaml + summary.md updated together; no partial state
- User confirmation required before finalize (irreversible)
- `archived` is a terminal status — no command can transition out of it

**Internal deps:** `bmad-lens-retrospective` (internal), `bmad-lens-document-project` (internal), `complete-ops.py`

**Backwards compat risk:** Low operationally. High if feature-index.yaml schema changes — the `archived` terminal status must remain recognizable to switch, next, dashboard, and any other command that reads the index.

---

### 14. `constitution` → `bmad-lens-constitution`

**What it does:** Read-only 4-level hierarchy resolver: org → domain → service → repo. Additive merge rules — lower levels extend, never override higher levels. Progressive disclosure of applicable articles.

**Key behaviors:**
- 4-level traversal (org → domain → service → repo)
- Additive merge algorithm (never override)
- `gate_mode` per article: informational vs enforced
- Gracefully handles missing levels — partial hierarchy is normal (this workspace has no org-level constitution)

**Internal deps:** Constitution files in governance repo; no scripts

**Backwards compat risk:** Low. Rewrite must preserve the partial-hierarchy fallback — missing org-level constitution must not cause a failure.

---

### 15. `discover` → `bmad-lens-discover`

**What it does:** Bidirectional repo-inventory sync. Clones repos registered in `repo-inventory.yaml` that are missing locally. Registers untracked local repos found under `target_projects_path`. Auto-commits sync state to governance `main`.

**Key behaviors:**
- Bidirectional: pushes missing local repos to inventory AND clones missing registered repos locally
- **The ONLY command that auto-commits to governance `main` outside lifecycle phase transitions**
- Not gated on a feature phase — runs against the governance repo directly

**Internal deps:** `discover-ops.py`, `repo-inventory.yaml`, `profile.yaml`

**Backwards compat risk:** Low. Rewrite concern: the auto-commit-to-main behavior is architecturally unusual and must be explicitly preserved — it is not triggered by a feature phase gate.

---

### 16. `next` → `bmad-lens-next`

**What it does:** Single opinionated next-action router. Reads feature state from `feature.yaml` + `lifecycle.yaml`, picks ONE recommendation, auto-delegates when unblocked. Blockers surface before delegation.

**Key behaviors:**
- Runs `next-ops.py suggest`
- Never presents a menu of options — commits to one action
- Blocker-first — hard gates are reported before any recommendation
- Auto-delegates: when unblocked, loads the owning SKILL.md immediately
- **Next-handoff counts as user consent** — the delegated skill must NOT ask a redundant yes/no launch confirmation

**Internal deps:** `next-ops.py`, `lifecycle.yaml`, all phase SKILL.md files (routes into each)

**Backwards compat risk:** Medium. **Critical contract:** when `next` delegates to a phase skill, that skill must not re-ask for launch confirmation. This must be standardized as an explicit handoff flag in the rewrite — not left to each skill to implement independently.

---

## Cross-Cutting Observations

| # | Observation | Rewrite Action |
|---|---|---|
| 1 | **Prompt stubs are cosmetic** — all `.github/prompts/` files are thin wrappers; real logic is in SKILL.md | No functional change from deprecating non-surviving stubs |
| 2 | **featureId formula is a frozen contract** — `{domain}-{service}-{featureSlug}` referenced everywhere | Must not change; document as explicit frozen contract in rewrite spec |
| 3 | **publish-to-governance CLI is load-bearing** — businessplan, techplan, finalizeplan, dev all call it | Never replace with direct file writes; retain as explicit CLI boundary |
| 4 | **Next-handoff pre-confirmed contract** — delegated skills must skip launch confirmation when invoked from `next` | Standardize as an explicit context flag/marker rather than per-skill convention |
| 5 | **Review-ready fast path is universal** — preplan, businessplan, techplan all share the same pattern | Extract as a shared lifecycle utility in the rewrite, not copy-pasted per skill |
| 6 | **finalizeplan step atomicity** — 3 steps with explicit commit/PR checkpoints | Preserve step-boundary model; no step skipping allowed |
| 7 | **dev is not a lifecycle phase** — validates finalizeplan complete but creates no initiative branch | Must not be promoted to a phase in the rewrite lifecycle.yaml |
| 8 | **expressplan requires QuickPlan internally** — `lens-quickplan.prompt.md` deprecated but skill must survive | Retain `bmad-lens-quickplan` skill as internal module |
| 9 | **light-preflight.py interface is frozen** — every stub calls it; exit 0 = proceed, non-zero = stop | Interface cannot change; rewrite can consolidate other scripts but not this one |
| 10 | **constitution partial-hierarchy must not fail** — missing org-level is the normal state | All callers of `bmad-lens-constitution` must be tested with incomplete hierarchy |

---

## Deprecated Published Prompts (retained as internal skills)

The following prompts are removed from `.github/prompts/` but their underlying skill implementations **must be retained** as internal modules:

**Must remain internal (called by surviving commands):**
- `bmad-lens-adversarial-review` — called by preplan, businessplan, techplan, finalizeplan, expressplan, dev
- `bmad-lens-git-orchestration` — called by new-feature, preplan, businessplan, techplan, finalizeplan, dev
- `bmad-lens-bmad-skill` (all wrappers) — called by preplan, businessplan, techplan, finalizeplan, expressplan
- `bmad-lens-quickplan` — called internally by expressplan
- `bmad-lens-finalizeplan` bundle — called internally by expressplan
- `bmad-lens-target-repo` — called internally by new-feature and dev
- `bmad-lens-feature-yaml` — called by every phase skill
- `bmad-lens-retrospective` — called by complete
- `bmad-lens-document-project` — called by complete
- `bmad-lens-migrate` — called by upgrade
- `bmad-lens-batch` — called by preplan, businessplan, techplan, finalizeplan (batch mode)
- `bmad-lens-init-feature` (fetch-context) — called by preplan, businessplan, techplan, finalizeplan

**Can be fully removed:**
- `lens-adversarial-review.prompt.md` (stub only — skill retained)
- `lens-approval-status.prompt.md`
- `lens-audit.prompt.md`
- `lens-dashboard.prompt.md`
- `lens-feature-yaml.prompt.md` (stub only — skill retained)
- `lens-git-orchestration.prompt.md` (stub only — skill retained)
- `lens-git-state.prompt.md`
- `lens-help.prompt.md`
- `lens-init-feature.prompt.md` (deprecated alias for new-feature)
- `lens-log-problem.prompt.md`
- `lens-module-management.prompt.md`
- `lens-move-feature.prompt.md`
- `lens-new-project.prompt.md` (no replacement; new-domain may cover this)
- `lens-onboard.prompt.md` (preflight is the surviving alias)
- `lens-pause-resume.prompt.md`
- `lens-profile.prompt.md`
- `lens-quickplan.prompt.md` (stub only — skill retained internally)
- `lens-retrospective.prompt.md` (stub only — skill retained)
- `lens-rollback.prompt.md`
- `lens-sensing.prompt.md`
- `lens-split-feature.prompt.md`
- `lens-target-repo.prompt.md` (stub only — skill retained)
- `lens-theme.prompt.md`
- All `lens-bmad-*.prompt.md` wrappers (stubs only — bmad-lens-bmad-skill retained internally)
