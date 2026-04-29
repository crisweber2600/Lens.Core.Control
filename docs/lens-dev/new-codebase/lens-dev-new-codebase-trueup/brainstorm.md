---
feature: lens-dev-new-codebase-trueup
doc_type: brainstorm
status: draft
goal: "Map the gap between old-codebase capabilities and delivered new-codebase modules; surface what a true-up initiative must close"
key_decisions:
  - Analysis is governance-only — uses old-codebase discovery docs and completed-feature artifacts; no source-tree inspection
  - Scope is the 17 retained commands plus cross-cutting shared infrastructure
  - Completed is defined as phase=complete or phase=finalizeplan-complete in governance feature.yaml
open_questions:
  - Are there observable parity failures in completed commands that weren't caught by existing tests?
  - Which of the 12 unimplemented retained commands have hidden cross-command infrastructure dependencies that must be resolved first?
  - Is publish-to-governance implemented in git-orchestration-ops.py in the new codebase?
  - Does complete-ops.py properly degrade when bmad-lens-retrospective and bmad-lens-document-project are not yet available?
  - Has fetch-context in init-feature-ops.py been fully implemented (it was listed as an open question in the new-feature tech plan)?
depends_on: []
blocks: []
updated_at: 2026-04-28T00:41:40Z
---

# Brainstorm — True-Up Gap Analysis (lens-dev-new-codebase-trueup)

**Session date:** 2026-04-28  
**Governance context sources:**
- `lens-dev-old-codebase-discovery` (component-inventory.md, source-tree-analysis.md)
- `lens-dev-new-codebase-baseline` (research.md — 17-command traceability matrix)
- Completed new-codebase feature tech plans: new-domain, new-service, new-feature, switch, complete

---

## 1. Grounding: What the Old Codebase Had

The old codebase (`lens-dev-old-codebase-discovery`) documented a published surface of:
- **41 skills** across planning conductors, execution, lifecycle utilities, governance/reporting, setup/migration, and foundation layers
- **57 prompts** (stub chains to skills)

The baseline planning reduction identified **17 retained published commands** — the survival set for the rewrite. The remaining ~40 commands/stubs were either deprecated, absorbed into surviving skills, or deferred.

The 17 retained commands form three functional clusters:

| Cluster | Commands |
|---------|----------|
| Container scaffolding | preflight, new-domain, new-service, new-feature |
| Session management | switch, next, complete |
| Planning lifecycle | preplan, businessplan, techplan, finalizeplan, expressplan, dev, split-feature, constitution, discover, upgrade |

---

## 2. Delivery Snapshot — What's Complete in the New Codebase

Based on governance phase state as of 2026-04-28:

### 2.1 Phase: `complete` (fully shipped)

| Feature | Command | Core Script | Key Behaviors Confirmed |
|---------|---------|-------------|------------------------|
| `lens-dev-new-codebase-new-domain` | `new-domain` (#2) | `init-feature-ops.py create-domain` | domain.yaml, constitution.md, optional scaffolds, context.yaml (service=null), duplicate fail-fast, dry-run, --execute-governance-git |
| `lens-dev-new-codebase-new-service` | `new-service` (#3) | `init-feature-ops.py create-service` | service.yaml, service constitution, domain-must-exist guard, optional scaffolds, context.yaml (domain+service), shared create-domain helpers reused |
| `lens-dev-new-codebase-switch` | `switch` (#5) | `switch-ops.py list / switch / context-paths` | numbered menu, stale detection (30-day), context.yaml session pointer, optional branch checkout, no governance mutation |

### 2.2 Phase: `finalizeplan-complete` (dev-ready, not yet coded)

| Feature | Command | Core Script | Notes |
|---------|---------|-------------|-------|
| `lens-dev-new-codebase-new-feature` | `new-feature` (#4) | `init-feature-ops.py create` | Full feature identity, 2-branch topology, governance registration; delegates to git-orchestration create-feature-branches and switch for activation; fetch-context parity marked as open question in tech plan |
| `lens-dev-new-codebase-complete` | `complete` (#13) | `complete-ops.py check-preconditions / finalize / archive-status` | Closure skill with irreversible confirmation gate; delegates to retrospective + document-project as prerequisites; those skills are not yet implemented in the new codebase — potential degradation risk |

**Delivered: 5 of 17 retained commands** (2 fully shipped, 2 dev-ready, 1 confirmed-complete for switch)  
**Gap: 12 of 17 retained commands** have existing feature entries at `preplan` with no active implementation work.

---

## 3. Gap Inventory — Unimplemented Retained Commands

All 12 of the following features exist as governance entries at `preplan` phase. None have tech plans, sprint plans, or implementation artifacts.

| # | Command | Feature ID | Cluster | Internal Dependencies It Needs |
|---|---------|------------|---------|-------------------------------|
| 1 | `preflight` | `lens-dev-new-codebase-preflight` | Container scaffolding | `light-preflight.py` (frozen, likely exists), workspace health checks |
| 6 | `next` | `lens-dev-new-codebase-next` | Session management | `next-ops.py suggest`, `feature.yaml` phase routing, `lifecycle.yaml` rules |
| 7 | `preplan` | `lens-dev-new-codebase-preplan` | Planning lifecycle | `validate-phase-artifacts.py` (exists), `bmad-lens-bmad-skill` wrappers, `bmad-lens-adversarial-review`, `bmad-lens-batch`, `bmad-lens-feature-yaml` updates |
| 8 | `businessplan` | `lens-dev-new-codebase-businessplan` | Planning lifecycle | `publish-to-governance` in git-orchestration, `bmad-lens-bmad-skill`, PRD + UX wrappers, adversarial review, feature-yaml updates |
| 9 | `techplan` | `lens-dev-new-codebase-techplan` | Planning lifecycle | `publish-to-governance`, `bmad-lens-bmad-skill`, architecture wrapper, adversarial review |
| 10 | `finalizeplan` | `lens-dev-new-codebase-finalizeplan` | Planning lifecycle | `publish-to-governance`, plan PR topology, epics/stories/implementation-readiness/sprint-status bundle, adversarial review |
| 11 | `expressplan` | `lens-dev-new-codebase-expressplan` | Planning lifecycle | express-track gate, quickplan internal behavior, finalizeplan bundle reuse, adversarial review hard gate |
| 12 | `dev` | `lens-dev-new-codebase-dev` | Planning lifecycle | `dev-session.yaml` checkpoint management, `prepare-dev-branch` in git-orchestration, `publish-to-governance`, target-repo operations, constitution load |
| 14 | `split-feature` | `lens-dev-new-codebase-split-feature` | Session management | `split-feature-ops.py validate-split / create-split-feature / move-stories`, feature-index updates, summary stub creation, in-progress-blocked rule |
| 15 | `constitution` | `lens-dev-new-codebase-constitution` | Planning lifecycle | `constitution-ops.py` (or equivalent), 4-level additive hierarchy resolution, partial-hierarchy fallback |
| 16 | `discover` | `lens-dev-new-codebase-discover` | Governance/reporting | `discover-ops.py`, `repo-inventory.yaml`, governance-main auto-commit contract, bidirectional sync |
| 17 | `upgrade` | `lens-dev-new-codebase-upgrade` | Setup/migration | `bmad-lens-migrate` delegation, lifecycle schema version detection, `[LENS:UPGRADE]` commit contract |

---

## 4. Cross-Cutting Infrastructure Gaps

Beyond individual commands, the new codebase must provide shared infrastructure that multiple retained commands depend on. Current status based on governance artifacts:

### 4.1 Confirmed Present (inferred from successful script invocations in this session)

| Component | Used by | Evidence |
|-----------|---------|---------|
| `light-preflight.py` | All 17 stubs | Successfully ran in this session |
| `validate-phase-artifacts.py` | preplan, businessplan, techplan, finalizeplan | Successfully ran in this session |
| `init-feature-ops.py` | new-domain, new-service, new-feature | All three commands use it; new-domain and new-service are complete |
| `switch-ops.py` | switch | switch feature is complete |
| `git-orchestration-ops.py create-feature-branches` | new-feature | Successfully ran in this session |

### 4.2 Unknown / Likely Missing

| Component | Used by | Risk Level | Notes |
|-----------|---------|-----------|-------|
| `git-orchestration-ops.py publish-to-governance` | businessplan, techplan, finalizeplan, dev | **High** | Not referenced in any completed feature tech plan; without it the entire planning-phase chain cannot publish to governance |
| `next-ops.py suggest` | next | **High** | next is still at preplan; no implementation exists |
| `bmad-lens-adversarial-review` skill/script | preplan, businessplan, techplan, finalizeplan, expressplan, dev | **High** | Every phase completion gate depends on this; no new-codebase implementation visible |
| `bmad-lens-feature-yaml` update operations | Every phase skill | **High** | Phase transitions via `bmad-lens-feature-yaml` are required by every planning phase skill; only reads have been confirmed |
| `bmad-lens-batch` shared contract | preplan, businessplan, techplan, finalizeplan | **Medium** | Batch pass-1/pass-2 flow needed; unclear if batch-ops infrastructure exists |
| `split-feature-ops.py` | split-feature | **Medium** | Dedicated script with complex validate-first + move-stories contract |
| `constitution-ops.py` | constitution | **Medium** | 4-level hierarchy resolution; the file is open in the editor — appears to be in the OLD codebase location (`lens.core/_bmad`), not the new codebase |
| `discover-ops.py` | discover | **Medium** | Bidirectional inventory sync with governance-main auto-commit |
| `bmad-lens-retrospective` skill | complete (prerequisite) | **Medium** | complete delegates to retrospective; without it, complete degrades or blocks |
| `bmad-lens-document-project` skill | complete (prerequisite) | **Medium** | complete delegates to document-project; same risk as retrospective |
| `dev-session.yaml` checkpoint management | dev | **Medium** | Resume checkpoints are a frozen compatibility surface |
| `git-orchestration-ops.py prepare-dev-branch` | dev | **Medium** | Dev phase branch preparation; distinct from create-feature-branches |

### 4.3 Fetch-Context Parity Risk

The `new-feature` tech plan explicitly flags:  
> *"Whether fetch-context is required in this delivery slice for full command parity"*

The `fetch-context` subcommand of `init-feature-ops.py` is used by preplan, businessplan, techplan, and finalizeplan to load cross-feature context. If the new-codebase implementation is incomplete, all four planning phase skills will be unable to load related feature context — a significant behavioral regression.

---

## 5. Parity Risk Assessment for Completed Features

Even for features already marked complete, there may be observable parity gaps not caught by the tests that exist.

### 5.1 new-domain / new-service (both complete)

**Risk level: Low**  
Both tech plans specify frozen schemas and explicitly document what the old-codebase behavior was. The shared init-feature family design is clean.  
**Residual risk:** The `domain.yaml` and `service.yaml` schemas are "frozen" but the test coverage assertions haven't been verified against the old-codebase outputs in this session.

### 5.2 switch (complete)

**Risk level: Low-Medium**  
Switch tech plan notes parity validation requirements around:
- Deprecated command-name references (old stubs that linked to switch should not be referenced)
- No-inference behavior (numbered menu must never infer from branch/file/history)

**Residual risk:** These are behavioral contracts that require observational testing, not just schema checks.

### 5.3 new-feature (finalizeplan-complete)

**Risk level: Medium**  
Two open risks from the tech plan:
1. `fetch-context` parity (marked as open question)
2. Integration with `bmad-lens-target-repo` when the feature needs a new implementation repo — this workflow hasn't been tested in the new codebase

### 5.4 complete (finalizeplan-complete)

**Risk level: High**  
The complete skill delegates to `bmad-lens-retrospective` and `bmad-lens-document-project` as prerequisites. Neither skill has a new-codebase feature that is past `preplan`. If `complete` is implemented before these prerequisites exist, it will either:
- Fail with a missing delegate error
- Silently skip the prerequisite steps (data loss risk — no retrospective, no project docs before archival)

This is the most significant parity risk in the delivered set.

---

## 6. Priority Ordering for the True-Up

Based on the dependency graph and risk levels identified above:

### Tier 1: Unblock the Entire Planning Chain

These are blocking blockers — nothing in the planning lifecycle (commands 7–12) can work without them:

1. **`git-orchestration-ops.py publish-to-governance`** — Prerequisite for businessplan, techplan, finalizeplan, dev
2. **`bmad-lens-adversarial-review`** — Required completion gate for every planning phase
3. **`bmad-lens-feature-yaml` update operations** — Phase transitions required by every planning skill

### Tier 2: High-Value Session Commands

These are user-facing commands with no planning-phase dependencies — they can be delivered independently:

4. **`preflight`** — Simplest retained command; entry point for all Lens sessions
5. **`next`** — High-frequency routing command; needed to navigate between phases without manual lookup
6. **`constitution`** — Needed before any phase skill runs; constitution-ops.py appears to exist in old codebase but needs a new-codebase version

### Tier 3: Core Planning Phases (in dependency order)

7. **`preplan`** (needs adversarial-review, validate-phase-artifacts, bmad-lens-batch, feature-yaml updates)
8. **`businessplan`** (needs preplan + publish-to-governance + PRD wrappers)
9. **`techplan`** (needs businessplan + publish-to-governance + architecture wrapper)
10. **`finalizeplan`** (needs techplan + publish-to-governance + plan PR + bundle generation)
11. **`expressplan`** (needs all of the above, compressed)
12. **`dev`** (needs finalizeplan + prepare-dev-branch + dev-session.yaml)

### Tier 4: Dependent Utilities

13. **`split-feature`** — Needs feature-index.yaml and summary.md patterns; depends on init-feature being stable
14. **`discover`** — Standalone; needs inventory sync + governance auto-commit
15. **`upgrade`** — Delegates to migrate; depends on lifecycle.yaml version detection

### Tier 5: Close Complete's Prerequisite Gaps

16. **`bmad-lens-retrospective`** — Prerequisite for complete (#13, already finalizeplan-complete)
17. **`bmad-lens-document-project`** — Prerequisite for complete (#13, already finalizeplan-complete)

---

## 7. Hidden Gaps Beyond the 17 Retained Commands

The old-codebase had ~40 additional commands/stubs that were deprecated or absorbed. Some of these are still needed as **internal** components even though they are not user-facing retained commands:

| Old Command | Retention Decision | Risk if Absent in New Codebase |
|-------------|-------------------|-------------------------------|
| `bmad-lens-quickplan` | Internal only (absorbed by expressplan) | expressplan cannot function without quickplan's internal behavior |
| `bmad-lens-target-repo` | Internal helper for new-feature and dev | new-feature's target-repo handoff and dev phase both need it |
| `bmad-lens-retrospective` | Internal prerequisite for complete | complete blocks without it |
| `bmad-lens-document-project` | Internal prerequisite for complete | complete blocks without it |
| `bmad-lens-batch` | Internal shared contract | Batch mode for all planning phases needs it |
| `bmad-lens-theme` | Internal overlay | Activated on session start; may be optional but referenced in SKILL.md activation |
| `lens-sprintplan.prompt.md` | Listed in old source tree but not in 17 retained | May have been folded into finalizeplan; needs verification |

---

## 8. Brainstorm Summary: What "True-Up" Means

The **True Up** feature's job is not to implement the 12 missing commands — those are owned by their respective features. Instead, True Up is a cross-cutting gap-closure initiative that should:

1. **Audit completed features for observable parity failures** — specifically the `fetch-context` gap in new-feature and the prerequisite-delegation gap in complete
2. **Identify and build the shared infrastructure blockers in Tier 1** — publish-to-governance, adversarial-review, feature-yaml updates — without which 10 of 12 remaining commands are unimplementable
3. **Document the dependency order** so that the 12 remaining preplan features can be activated in the correct sequence
4. **Define what "true-up complete" looks like** — a checklist of observable behaviors that the new codebase must match against the old-codebase baseline for all 17 commands to be declared equivalent

The True Up is essentially a **gap-closure and unblocking initiative** that bridges the 5 completed commands to the 12 unimplemented ones, ensuring the entire new-codebase rewrite can reach functional parity.

---

## 9. Open Questions to Carry Into Research

1. Is `publish-to-governance` already implemented in the new-codebase's `git-orchestration-ops.py`? If so, the Tier 1 blocker may be partially resolved.
2. Is there a new-codebase version of `constitution-ops.py`? The active editor shows an old-codebase version — is the new-codebase version identical or does it differ?
3. What is the current implementation state of `bmad-lens-adversarial-review` in the new codebase? Does the new-codebase version inherit the old implementation?
4. Can `complete` be safely released before `bmad-lens-retrospective` and `bmad-lens-document-project` exist? Is there a graceful degradation path?
5. Which of the 12 preplan-phase features have hidden sprint-plan or story artifacts in the control repo that weren't mirrored to governance?
