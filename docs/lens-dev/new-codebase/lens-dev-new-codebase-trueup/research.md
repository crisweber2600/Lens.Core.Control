---
feature: lens-dev-new-codebase-trueup
doc_type: research
status: draft
goal: "Establish a verified, evidence-backed gap map between old-codebase capabilities and the current state of the new-codebase rewrite, covering scripts, prompt stubs, SKILL.md files, and shared infrastructure"
key_decisions:
  - New-codebase file inventory conducted via filesystem enumeration (non-git files only)
  - Old-codebase discovery docs and governance tech plans used as comparison baseline
  - Governance phase labels are treated as planning status only; actual implementation status is verified against filesystem evidence
  - The scripts that Lens lifecycle runs at runtime come from the RELEASE module (lens.core/) — new-codebase source (TargetProjects/lens-dev/new-codebase/lens.core.src) is the authoring workspace
open_questions:
  - Why does init-feature-ops.py 'create' appear to work (we ran it in this session) while governance lists new-feature as 'finalizeplan-complete' rather than 'complete'?
  - Does bmad-lens-lessons source exist somewhere outside the tracked working tree (pyc files exist but .py sources are absent from the inventory)?
  - Are there additional prompt stubs published outside '_bmad/lens-work/prompts/' and '.github/prompts/' paths?
  - Is finalize-feature.md in bmad-lens-complete/references/ a planning-phase doc or an early implementation scaffold?
depends_on: [brainstorm]
blocks: []
updated_at: 2026-04-28T01:00:00Z
---

# Research — True-Up Gap Analysis (lens-dev-new-codebase-trueup)

**Session date:** 2026-04-28  
**Evidence sources:**
- New-codebase file inventory: `TargetProjects/lens-dev/new-codebase/lens.core.src` (filesystem enumeration, non-git files)
- Old-codebase discovery docs: component-inventory.md, source-tree-analysis.md
- Baseline traceability matrix: `docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/research.md`
- Completed-feature governance tech plans: new-domain, new-service, new-feature, switch, complete

---

## 1. New-Codebase File Inventory (Verified)

### 1.1 Scripts Confirmed Present

| Script | Path | Evidence |
|--------|------|---------|
| `init-feature-ops.py` | `_bmad/lens-work/skills/bmad-lens-init-feature/scripts/` | File exists; ran successfully in this session |
| `merge-config.py` | `_bmad/lens-work/skills/bmad-lens-init-feature/scripts/` | File exists |
| `merge-help-csv.py` | `_bmad/lens-work/skills/bmad-lens-init-feature/scripts/` | File exists |
| `light-preflight.py` | `_bmad/lens-work/skills/bmad-lens-preflight/scripts/` | File exists; ran successfully in this session |
| `switch-ops.py` | `_bmad/lens-work/skills/bmad-lens-switch/scripts/` | File exists |
| `scaffold-standalone-module.py` | `.github/skills/bmad-module-builder/scripts/` | File exists |
| `init-sanctum.py` | `skills/lc-agent-core-repo/assets/` | File exists (lc-agent-core-repo skill) |
| `merge-config.py` (lc-agent) | `skills/lc-agent-core-repo/scripts/` | File exists |
| `merge-help-csv.py` (lc-agent) | `skills/lc-agent-core-repo/scripts/` | File exists |

**Tests present:**
- `test-init-feature-ops.py` — `bmad-lens-init-feature/scripts/tests/`
- `test-create-service-ops.py` — `bmad-lens-init-feature/scripts/tests/`
- `test-switch-ops.py` — `bmad-lens-switch/scripts/tests/`

### 1.2 Scripts Confirmed ABSENT

These are scripts expected by the lifecycle or tech plans but absent from the new-codebase source tree:

| Script | Expected in | Risk Level | Notes |
|--------|------------|-----------|-------|
| `git-orchestration-ops.py` | `bmad-lens-git-orchestration/scripts/` | **CRITICAL** | No `bmad-lens-git-orchestration` directory exists at all. The working version at runtime is the RELEASE module copy in `lens.core/`. |
| `complete-ops.py` | `bmad-lens-complete/scripts/` | HIGH | `bmad-lens-complete/scripts/` directory does not exist; only `references/finalize-feature.md` is present |
| `validate-phase-artifacts.py` | `_bmad/lens-work/scripts/` | HIGH | No top-level `scripts/` folder in new-codebase source. The working version comes from the RELEASE module. |
| `next-ops.py` | `bmad-lens-next/scripts/` | HIGH | No `bmad-lens-next` skill directory exists |
| `adversarial-review-ops.py` (or equivalent) | `bmad-lens-adversarial-review/scripts/` | **CRITICAL** | No `bmad-lens-adversarial-review` directory exists |
| `bmad-lens-feature-yaml` update operations | `bmad-lens-feature-yaml/scripts/` | **CRITICAL** | No `bmad-lens-feature-yaml` directory exists |
| `split-feature-ops.py` | `bmad-lens-split-feature/scripts/` | MEDIUM | No `bmad-lens-split-feature` directory |
| `constitution-ops.py` | `bmad-lens-constitution/scripts/` | MEDIUM | No `bmad-lens-constitution` directory |
| `discover-ops.py` | `bmad-lens-discover/scripts/` | MEDIUM | No `bmad-lens-discover` directory |
| `upgrade-ops.py` (or migrate delegation) | `bmad-lens-upgrade/scripts/` | MEDIUM | No `bmad-lens-upgrade` directory |
| `dev-session.py` (or checkpoint manager) | `bmad-lens-dev/scripts/` | MEDIUM | No `bmad-lens-dev` directory |

### 1.3 SKILL.md Inventory

| Skill | SKILL.md Present | Scripts Present | Tests Present |
|-------|-----------------|----------------|--------------|
| `bmad-lens-init-feature` | ✅ | ✅ (3 scripts + 2 tests) | ✅ |
| `bmad-lens-switch` | ✅ | ✅ (1 script + 1 test) | ✅ |
| `bmad-lens-complete` | ❌ | ❌ (reference doc only) | ❌ |
| `bmad-lens-preflight` | ❌ | ✅ (light-preflight.py only) | ❌ |
| `bmad-lens-lessons` | ❌ | ⚠️ (compiled .pyc only, source absent) | ❌ |

**Finding:** Only 2 of 5 skill directories that exist have a SKILL.md. 3 directories exist but are incomplete stubs.

### 1.4 Published Prompt Stubs

Prompt files serve as the user-facing entry points for each retained command. They live in `_bmad/lens-work/prompts/` and are mirrored to `.github/prompts/`.

| Prompt Stub | `_bmad/lens-work/prompts/` | `.github/prompts/` | Status |
|------------|---------------------------|-------------------|--------|
| `lens-new-domain.prompt.md` | ✅ | ✅ | COMPLETE |
| `lens-new-service.prompt.md` | ✅ | ✅ | COMPLETE |
| `lens-switch.prompt.md` | ✅ | ❌ | PARTIAL (not mirrored to .github) |
| `lens-new-feature.prompt.md` | ❌ | ❌ | MISSING |
| `lens-preflight.prompt.md` | ❌ | ❌ | MISSING (script exists but no stub) |
| `lens-complete.prompt.md` | ❌ | ❌ | MISSING |
| `lens-next.prompt.md` | ❌ | ❌ | MISSING |
| `lens-preplan.prompt.md` | ❌ | ❌ | MISSING |
| `lens-businessplan.prompt.md` | ❌ | ❌ | MISSING |
| `lens-techplan.prompt.md` | ❌ | ❌ | MISSING |
| `lens-finalizeplan.prompt.md` | ❌ | ❌ | MISSING |
| `lens-expressplan.prompt.md` | ❌ | ❌ | MISSING |
| `lens-dev.prompt.md` | ❌ | ❌ | MISSING |
| `lens-split-feature.prompt.md` | ❌ | ❌ | MISSING |
| `lens-constitution.prompt.md` | ❌ | ❌ | MISSING |
| `lens-discover.prompt.md` | ❌ | ❌ | MISSING |
| `lens-upgrade.prompt.md` | ❌ | ❌ | MISSING |

**Finding:** 14 of 17 retained commands have no published prompt stub in the new-codebase source. `lens-switch.prompt.md` is present in `_bmad/` but not mirrored to `.github/prompts/`, which is where IDE integrations look for prompts.

---

## 2. Gap Analysis: Old Codebase vs. New Codebase

### 2.1 Structural Comparison

The old codebase (per old-codebase discovery docs) had:
- **41 skills** under `_bmad/lens-work/skills/`, each with a SKILL.md and typically at least one script
- **57 prompts** under `_bmad/lens-work/prompts/` (1 per retained command plus utility stubs)

The new codebase currently has:
- **5 skill directories** under `_bmad/lens-work/skills/` (down from 41)
- Only **2 skills** with complete packages (SKILL.md + scripts + tests)
- **3 prompt stubs** out of 17 retained commands (down from 57)

This is **12% structural completeness** (5/41 skills, ~18% of prompts).

### 2.2 The Release-Module Paradox

**Critical finding:** The version of the Lens tools that actually runs during live Lens sessions (including this one) is the RELEASE module at `lens.core/_bmad/lens-work/`, NOT the new-codebase source. This means:

- `git-orchestration-ops.py` used in this session → came from `lens.core/_bmad/lens-work/skills/bmad-lens-git-orchestration/`
- `validate-phase-artifacts.py` → came from `lens.core/_bmad/lens-work/scripts/`
- `light-preflight.py` → came from `lens.core/_bmad/lens-work/skills/bmad-lens-preflight/` (same file exists in both)

The new-codebase source is the **authoring workspace** for the rewrite, not the runtime. The gap is therefore between what the RELEASE module has (old codebase) and what the new-codebase source currently contains for eventual replacement.

### 2.3 Functional Coverage by Command

| # | Command | New-Codebase Script | New-Codebase SKILL.md | New-Codebase Prompt | Governance Phase |
|---|---------|--------------------|-----------------------|---------------------|-----------------|
| 1 | preflight | ✅ `light-preflight.py` | ❌ | ❌ | preplan |
| 2 | new-domain | ✅ in `init-feature-ops.py` | ✅ | ✅ (mirrored) | complete |
| 3 | new-service | ✅ in `init-feature-ops.py` | ✅ | ✅ (mirrored) | complete |
| 4 | new-feature | ✅ in `init-feature-ops.py` | ✅ | ❌ | finalizeplan-complete |
| 5 | switch | ✅ `switch-ops.py` | ✅ | ✅ (not mirrored) | complete |
| 6 | next | ❌ | ❌ | ❌ | preplan |
| 7 | preplan | ❌ | ❌ | ❌ | preplan |
| 8 | businessplan | ❌ | ❌ | ❌ | preplan |
| 9 | techplan | ❌ | ❌ | ❌ | preplan |
| 10 | finalizeplan | ❌ | ❌ | ❌ | preplan |
| 11 | expressplan | ❌ | ❌ | ❌ | preplan |
| 12 | dev | ❌ | ❌ | ❌ | preplan |
| 13 | complete | ❌ (ref doc only) | ❌ | ❌ | finalizeplan-complete |
| 14 | split-feature | ❌ | ❌ | ❌ | preplan |
| 15 | constitution | ❌ | ❌ | ❌ | preplan |
| 16 | discover | ❌ | ❌ | ❌ | preplan |
| 17 | upgrade | ❌ | ❌ | ❌ | preplan |

**Fully covered:** new-domain, new-service, switch (3/17 = 18%)  
**Partially covered:** preflight (script only), new-feature (no prompt), complete (reference only) (3/17 = 18%)  
**Not covered:** next, preplan, businessplan, techplan, finalizeplan, expressplan, dev, split-feature, constitution, discover, upgrade (11/17 = 65%)

---

## 3. Investigating the 5 Open Questions from Brainstorm

### Q1: Is `publish-to-governance` implemented in git-orchestration-ops.py?

**Finding: Unresolvable from new-codebase source — no git-orchestration-ops.py exists there.**

The `git-orchestration-ops.py` used at runtime is from the RELEASE module (`lens.core/`). The new-codebase source has no `bmad-lens-git-orchestration` directory at all. Therefore:

- Whether `publish-to-governance` is implemented cannot be determined from new-codebase inspection
- The RELEASE module's `git-orchestration-ops.py` is the current runtime implementation — that file should be inspected (in-scope for research since it is the existing reference implementation)
- When the new-codebase writes its own `git-orchestration-ops.py`, `publish-to-governance` must be included or the planning chain cannot publish

**Implication:** True Up must include a task to verify `publish-to-governance` status in the RELEASE module and document whether it needs to be rewritten for the new codebase or can be carried over.

### Q2: Does a new-codebase version of `constitution-ops.py` exist?

**Finding: No — there is no `bmad-lens-constitution` skill directory in the new-codebase source.**

The old codebase had `bmad-lens-constitution` as a full skill package. The new-codebase has not started this skill. All constitution operations currently run through the RELEASE module.

**Implication:** Constitution is correctly categorized as Tier 2 — high value, independently deliverable, not started.

### Q3: What is the state of `bmad-lens-adversarial-review` in the new codebase?

**Finding: No `bmad-lens-adversarial-review` skill directory exists in the new-codebase source.**

This is one of the most significant infrastructure gaps. Every planning phase skill (preplan, businessplan, techplan, finalizeplan, expressplan, dev) terminates with an adversarial review gate. Without this infrastructure, none of the planning phase skills can complete their review cycles.

**Implication:** Adversarial review is correctly Tier 1 — it must be delivered before any planning-phase feature can reach `phase-complete` in the new codebase.

### Q4: Can `complete` be safely released without `bmad-lens-retrospective` and `bmad-lens-document-project`?

**Finding: Not safely as-is.**

`bmad-lens-complete` has only `references/finalize-feature.md` in the new codebase — no SKILL.md, no script, no tests. The feature is at `finalizeplan-complete`, meaning it is dev-ready but not yet implemented.

The `complete` command's tech plan delegates to retrospective and document-project. However:
- Neither `bmad-lens-retrospective` nor `bmad-lens-document-project` directories exist in the new-codebase source
- The RELEASE module has both (old-codebase implementations)

**Two mitigation options documented in the brainstorm:**
1. Graceful degradation: `complete-ops.py` checks if delegate skills are available; if not, logs a warning and proceeds to archive-status-only mode
2. Hard prerequisite: `complete-ops.py` refuses to proceed without retrospective + document-project, blocking completion

Neither option has been decided. This is a **design decision** that must be made before `complete` can be dev'd.

**Implication:** True Up should document this as an open architectural decision that must be resolved as a prerequisite to dev'ing `bmad-lens-complete`.

### Q5: Which of the 12 preplan features have hidden control-repo artifacts?

**Finding: The 12 features at preplan phase in governance have NO planning artifacts in the control repo docs path.**

The control repo docs paths (`docs/lens-dev/new-codebase/lens-dev-new-codebase-{feature}/`) were checked by the governance phase survey. All 12 features (preflight, businessplan, constitution, dev, discover, expressplan, finalizeplan, next, preplan, split-feature, techplan, upgrade) are at `preplan` with feature.yaml and summary.md stubs only. None have brainstorm, research, or product-brief docs.

**Implication:** True Up has no competing or overlapping planning work for the 12 unimplemented features. Its cross-cutting scope does not conflict with any in-flight feature work.

---

## 4. Infrastructure Dependency Tree

The dependency tree below shows what must be built before each command can reach functional parity:

```
TIER 0: Already built (RELEASE module — maintained, carries over)
├── light-preflight.py
├── init-feature-ops.py (create-domain, create-service, create)
├── switch-ops.py
├── git-orchestration-ops.py (create-feature-branches — confirmed)
└── validate-phase-artifacts.py

TIER 1: Shared infrastructure — must be built first
├── bmad-lens-adversarial-review
│   └── Unblocks: preplan, businessplan, techplan, finalizeplan, expressplan, dev
├── git-orchestration-ops.py publish-to-governance
│   └── Unblocks: businessplan, techplan, finalizeplan, dev
└── bmad-lens-feature-yaml (update operations: phase transitions)
    └── Unblocks: ALL phase-completion workflows (every phase)

TIER 2: High-value independent commands
├── light-preflight.py SKILL.md + tests (adds SKILL.md wrapper to existing script)
│   └── Unblocks: complete preflight parity
├── next-ops.py
│   └── Unblocks: next command, autonomous phase routing
└── constitution-ops.py
    └── Unblocks: constitution command, phase skill context loading

TIER 3: Planning phases (in dependency order)
├── preplan skill
│   └── Requires: adversarial-review, validate-phase-artifacts, feature-yaml updates
├── businessplan skill
│   └── Requires: preplan-complete state, publish-to-governance, PRD wrapper
├── techplan skill
│   └── Requires: businessplan-complete state, publish-to-governance, architecture wrapper
├── finalizeplan skill
│   └── Requires: techplan-complete state, publish-to-governance, plan PR, epics bundle
├── expressplan skill
│   └── Requires: all of the above (compressed path)
└── dev skill
    └── Requires: finalizeplan-complete, prepare-dev-branch, dev-session.yaml, target-repo ops

TIER 4: Dependent session management
├── split-feature-ops.py
│   └── Requires: stable init-feature + feature-yaml + governance integration
├── discover-ops.py
│   └── Requires: governance-main auto-commit contract
└── upgrade-ops.py (or migrate delegation)
    └── Requires: lifecycle.yaml version detection + bmad-lens-migrate integration

TIER 5: complete prerequisites
├── bmad-lens-retrospective
│   └── Requires: nothing from tiers 1-4; but needed before complete can finalize
└── bmad-lens-document-project
    └── Requires: nothing from tiers 1-4; but needed before complete can finalize
```

---

## 5. Governance Phase Label Anomaly

**Observation:** The governance phase label `finalizeplan-complete` for new-feature is inconsistent with observed behavior.

We successfully ran `init-feature-ops.py create` in this session to create the `lens-dev-new-codebase-trueup` feature. The command completed successfully, including governance registration, branch creation, and context-file writing. This is `new-feature` functionality.

Yet governance labels new-feature as `finalizeplan-complete` — which should mean "planning is done, dev has not started."

**Three possible explanations:**
1. **`create` was implemented as part of another feature's delivery** — e.g., it was implemented as part of the `init-feature` family alongside new-domain/new-service, and governance tracking was not updated to reflect this
2. **Governance phase labels lag actual implementation state** — the new-feature feature's phase was set to `finalizeplan-complete` when planning docs were published, but no process exists to advance it to `complete` when code was actually shipped
3. **The `create` subcommand is partially implemented** — enough to work for the happy path but with open gaps (e.g., fetch-context)

**Implication for True Up:** This is a parity audit finding. True Up should include a task to validate the governance phase state of all 5 "delivered" features against their actual script behavior, and update phase labels where implementation has outpaced planning tracking.

---

## 6. Prompt Stub Gap: A Systemic Issue

The `lens-switch.prompt.md` exists in `_bmad/lens-work/prompts/` but is NOT mirrored to `.github/prompts/`. The other two published prompts (new-domain, new-service) are correctly mirrored to both locations.

This reveals a **systemic process gap**: there is no automated or checked process that ensures prompt files are published to both locations. When a developer creates a prompt file in `_bmad/lens-work/prompts/`, it is not automatically mirrored to `.github/prompts/`, which is what IDE agents use to discover prompts.

**Implication for True Up:** The true-up parity checklist must include a prompt-publishing verification step: all retained command prompts must be present in BOTH `_bmad/lens-work/prompts/` and `.github/prompts/`.

---

## 7. Key Research Findings Summary

| Finding | Category | Risk | Actionable for True Up |
|---------|---------|------|----------------------|
| No `git-orchestration-ops.py` in new-codebase | Script gap | **CRITICAL** | Verify `publish-to-governance` in RELEASE module; create task to port/rewrite |
| No `bmad-lens-adversarial-review` in new-codebase | Infrastructure gap | **CRITICAL** | Must be delivered before any phase skill can complete |
| No `bmad-lens-feature-yaml` update ops | Infrastructure gap | **CRITICAL** | Phase transitions need this in every phase skill |
| 14/17 prompt stubs missing | Prompt gap | HIGH | Add to parity checklist; 14 stubs to create |
| `lens-switch.prompt.md` not mirrored to `.github/` | Process gap | MEDIUM | Add prompt-mirroring verification to parity checklist |
| `complete-ops.py` not started, delegate skills missing | Design gap | HIGH | Resolve graceful-degradation vs. hard-prerequisite decision before dev |
| New-feature governance phase may lag actual state | Tracking gap | MEDIUM | Audit all 5 delivered features; update governance phase labels where needed |
| `bmad-lens-preflight` has script but no SKILL.md or tests | Incomplete delivery | LOW-MEDIUM | Add SKILL.md + tests as a parity item for preflight |
| `bmad-lens-lessons` pyc-only (source absent) | Unclear state | LOW | Investigate if source is gitignored or genuinely missing |
| 12% structural completeness (5/41 skills, 3/17 prompts) | Progress tracking | Informational | Confirms scale of remaining work |
