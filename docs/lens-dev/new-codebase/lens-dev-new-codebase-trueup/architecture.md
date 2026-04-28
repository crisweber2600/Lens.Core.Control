---
feature: lens-dev-new-codebase-trueup
doc_type: architecture
status: draft
goal: "Close delivery gaps across the 5 non-preplan new-codebase features via clean-room parity audit, prompt publishing, bmad-lens-complete SKILL.md package, and binding ADRs"
key_decisions:
  - Clean-room operating model: all new-codebase artifacts are authored independently from behavioral contracts (discovery docs, governance tech plans), not by copying old-codebase source
  - complete prerequisite handling: graceful-degradation is canonical — bmad-lens-complete proceeds with a warning when retrospective.md or document-project is absent (evidence: old-codebase complete-ops.py sets retrospective_skipped=True and returns status:warn, not fail)
  - constitution permitted_tracks: new-codebase template (express + expressplan included) is canonical — lifecycle.yaml v4 explicitly defines express and expressplan as supported phases; old-codebase omission was a documentation lag
  - Python 3.12 requirement: intentional — init-feature-ops.py uses tomllib (stdlib in 3.11+) and structural pattern matching syntax; documented as a deliberate language-feature dependency, not accidental drift
| SAFE_ID_PATTERN tightening: safe to adopt — no existing feature IDs in governance use dots or underscores (verified by scanning feature-index.yaml and all feature.yaml files); the new pattern is a forward-compatible tightening; scan evidence documented in parity-audit-report.md FR-9 section
  - BMB implementation channel: all story work touching lens.core/_bmad/lens-work/ must route through the bmb module per domain constitution (H1 warning from businessplan review)
  - feature.yaml.phase is authoritative for lifecycle state; feature-index.yaml.status is a registry summary only (M1 warning from businessplan review)
  - fetch-context and read-context absence in new-codebase init-feature-ops.py is a functional regression; old-codebase output contract is the restoration target for lens-dev-new-codebase-new-feature dev phase (M2 warning from businessplan review)
open_questions:
  - Does the lc-agent-core-repo skill require separate True Up coverage or is it handled by its own feature?
  - Are there additional prompt stubs expected under .github/prompts/ beyond the 17 retained commands?
depends_on:
  - lens-dev-new-codebase-baseline
  - lens-dev-new-codebase-new-feature
  - lens-dev-new-codebase-complete
  - lens-dev-new-codebase-switch
  - lens-dev-new-codebase-new-domain
  - lens-dev-new-codebase-new-service
blocks: []
updated_at: 2026-04-28T02:15:00Z
stepsCompleted: [1, 2, 3, 4, 5, 6, 7, 8]
inputDocuments:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/prd.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/ux-design.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/product-brief.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/research.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/brainstorm.md
  - TargetProjects/lens/lens-governance/constitutions/lens-dev/constitution.md
  - TargetProjects/lens/lens-governance/constitutions/lens-dev/new-codebase/constitution.md
---

# Architecture — True Up (lens-dev-new-codebase-trueup)

---

## 1. Technical Summary

True Up is a **planning and tooling infrastructure feature**, not an application. It delivers four categories of artifacts, all authored in the control repo and committed to the plan branch:

1. **Behavioral parity audit report** — evidence-backed gap map for the 5 non-preplan features
2. **Prompt publishing closure** — three missing prompt stubs (`lens-switch`, `lens-new-feature`, `lens-complete`) published to both `_bmad/lens-work/prompts/` and `.github/prompts/`
3. **`bmad-lens-complete` SKILL.md package** — complete command contract for 3 operations plus scaffolded test stubs
4. **Design decisions (ADRs)** — four binding decisions that unblock subsequent dev phases

The **clean-room operating model** is the foundational architectural constraint: no artifact in the new-codebase source is derived by copying old-codebase code. Every new file is independently authored from the behavioral contract defined in governance discovery docs, research findings, and the PRD. The old-codebase acts exclusively as a behavioral reference for contract derivation.

---

## 2. Architecture Overview

### 2.1 The Three-Layer Command Architecture

All retained commands in the new codebase follow the same three-layer architecture inherited from the old codebase. True Up does not change this topology — it closes gaps within it.

```
Layer 1: Prompt Stub (user-facing entry point)
  .github/prompts/lens-{command}.prompt.md          ← IDE discovery surface
  lens.core.src/_bmad/lens-work/prompts/lens-{command}.prompt.md  ← source of truth

         │
         ▼

Layer 2: SKILL.md (agent contract)
  lens.core.src/_bmad/lens-work/skills/bmad-lens-{command}/SKILL.md
         │
         ▼

Layer 3: Script (runtime implementation)
  lens.core.src/_bmad/lens-work/skills/bmad-lens-{command}/scripts/{command}-ops.py
  lens.core.src/_bmad/lens-work/skills/bmad-lens-{command}/scripts/tests/test-{command}-ops.py
```

**For True Up's scope:** Layers 1–2 are closed for `switch`, `new-feature`, and `complete`. Layer 3 (`complete-ops.py`) is explicitly out of scope — that belongs to `lens-dev-new-codebase-complete` dev phase.

### 2.2 The Clean-Room Model

```
  Old-Codebase (lens-dev-old-codebase-discovery)
    component-inventory.md
    source-tree-analysis.md
          │
          │ behavioral reference (read-only)
          ▼
  New-Codebase (lens.core.src / control repo plan branch)
    → Artifacts authored independently from contract definitions
    → No old-codebase code copied or imported
    → Old-codebase used only to verify expected output schemas and failure modes
```

**Authority boundary:** `lens.core/` is the RELEASE module — read-only at runtime. Authoring happens in `TargetProjects/lens-dev/new-codebase/lens.core.src/`. The control repo plan branch (`lens-dev-new-codebase-trueup-plan`) is where all artifacts accumulate before the plan PR merges to the base branch.

### 2.3 Implementation Channel

Per the domain constitution (`constitutions/lens-dev/constitution.md`) and the H1 finding in the BusinessPlan adversarial review:

> **Any story that touches `lens.core/_bmad/lens-work/` or `lens.core.src/_bmad/lens-work/` MUST route through the BMB module.**

- SKILL.md authoring → `lens.core/_bmad/bmb/` (bmad-module-builder channel)
- Prompt stubs → direct authoring is permitted (stubs are delegation-only, not behavioral artifacts)
- Test stubs → direct authoring is permitted (scaffolded stubs with no implementation)

---

## 3. Component Map

Each FR group in the PRD maps to a set of files. This section is the definitive delivery spec.

### 3.1 FR Group 1 — Prompt Publishing

| FR | Artifact | Authoring Location | Target (after merge) |
|----|----------|-------------------|----------------------|
| FR-1 | `lens-switch.prompt.md` | Copied from `_bmad/lens-work/prompts/lens-switch.prompt.md` | `.github/prompts/lens-switch.prompt.md` |
| FR-2 | `lens-new-feature.prompt.md` (source) | `_bmad/lens-work/prompts/lens-new-feature.prompt.md` | New file — delegation stub |
| FR-2 | `lens-new-feature.prompt.md` (mirror) | `.github/prompts/lens-new-feature.prompt.md` | New file — mirrors source |
| FR-3 | `lens-complete.prompt.md` (source) | `_bmad/lens-work/prompts/lens-complete.prompt.md` | New file — delegation stub |
| FR-3 | `lens-complete.prompt.md` (mirror) | `.github/prompts/lens-complete.prompt.md` | New file — mirrors source |

**Prompt stub contract (NFR-2 compliant):**
- Must delegate to its backing skill via `lens.core/_bmad/lens-work/skills/bmad-lens-{command}/SKILL.md`
- Must display a clear "not yet implemented" message if the backing script (`complete-ops.py`) is absent
- Must not implement behavioral logic inline

**`lens-new-feature.prompt.md` stub note:** The `create` subcommand in `init-feature-ops.py` appears to be partially implemented (ran successfully in this session to create the trueup feature). The stub must note that full parity — including `fetch-context` and `read-context` — is pending, per the parity audit findings.

### 3.2 FR Group 2 — `bmad-lens-complete` Package

| FR | Artifact | Path | Description |
|----|----------|------|-------------|
| FR-4 | `SKILL.md` | `_bmad/lens-work/skills/bmad-lens-complete/SKILL.md` | Full command contract for 3 operations |
| FR-5 | `test-complete-ops.py` | `_bmad/lens-work/skills/bmad-lens-complete/scripts/tests/test-complete-ops.py` | Scaffolded stubs — no implementation |

#### SKILL.md Contract Specification

The `bmad-lens-complete` SKILL.md must define these three commands, derived from the old-codebase `complete-ops.py` behavioral reference and `references/finalize-feature.md`:

**Command 1: `check-preconditions`**
- Inputs: `--governance-repo <path>`, `--feature-id <id>`, `[--dry-run]`
- Guards: feature must exist in governance; feature phase must be in a completable state (dev-complete or equivalent)
- Returns structured JSON: `{status: pass|warn|fail, retrospective_skipped: bool, document_project_skipped: bool, checks: [...]}`
- Prerequisite handling: `retrospective.md` absent → `status: warn`, `retrospective_skipped: true` (graceful-degradation per ADR-1)
- `document_project` absent → `status: warn`, `document_project_skipped: true` (same rule)
- Phase guard failure → `status: fail` (hard block — not degradable)

**Command 2: `finalize`**
- Inputs: `--governance-repo <path>`, `--feature-id <id>`, `[--dry-run]`, `[--confirm]`
- **Irreversible confirmation gate:** requires explicit `--confirm` flag or interactive prompt; must surface a summary of changes before proceeding
- Operations (dry-run previews all, no writes): update `feature.yaml` phase → `complete`; update `feature-index.yaml` status → `archived`; write `summary.md` if absent; set `updated_at`
- On dry-run: returns `{status: dry_run, planned_changes: [...]}` — no writes
- On execute: returns `{status: complete, changes_applied: [...]}`

**Command 3: `archive-status`**
- Inputs: `--governance-repo <path>`, `--feature-id <id>`
- Returns: `{archived: bool, phase: str, status: str}`
- Read-only — no writes; used to verify finalize completed correctly

#### Test Stub Signatures (FR-5)

```python
def test_check_preconditions_pass():
    """Feature in dev phase with retrospective.md present — expect status:pass."""

def test_check_preconditions_warn_no_retrospective():
    """Feature in dev phase, retrospective.md absent — expect status:warn, retrospective_skipped:True."""

def test_check_preconditions_fail_wrong_phase():
    """Feature in preplan phase — expect status:fail (not completable)."""

def test_finalize_dry_run():
    """finalize --dry-run returns planned_changes without writing anything."""

def test_finalize_archives_feature():
    """End-to-end: finalize sets phase=complete, index=archived, writes summary.md."""

def test_archive_status_archived():
    """feature.yaml phase=complete — expect archived:True."""

def test_archive_status_not_archived():
    """feature.yaml phase=dev — expect archived:False."""

def test_prerequisite_missing_degradation():
    """finalize proceeds with warn when retrospective.md absent (graceful-degradation ADR)."""
```

### 3.3 FR Group 3 — ADR Artifacts

| FR | Artifact | Path | Decision |
|----|----------|------|---------|
| FR-6 | `adr-complete-prerequisite.md` | `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/` | Graceful-degradation (canonical) |
| FR-7 | `adr-constitution-tracks.md` | `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/` | New-codebase template canonical (express + expressplan included) |
| FR-8 | Section in parity-audit-report.md | `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/` | Python 3.12 intentional |
| FR-9 | Section in parity-audit-report.md | `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/` | SAFE_ID_PATTERN: safe to adopt |

### 3.4 FR Groups 4–7 — Parity Audit Artifacts

| FR | Artifact | Path |
|----|----------|------|
| FR-10 to FR-12 | `parity-audit-report.md` | `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/` |
| FR-13 | `references/auto-context-pull.md` | `lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/references/` |
| FR-13 | `references/init-feature.md` | `lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/references/` |
| FR-14 | `parity-gate-spec.md` | `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/` |
| FR-15 | `parity-audit-report.md` | `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/` |

---

## 4. Architectural Decision Records

### ADR-1: `bmad-lens-complete` Prerequisite Handling

**Status:** Proposed → Accepted on techplan review pass  
**Date:** 2026-04-28

**Context:**  
`bmad-lens-complete` delegates optionally to `bmad-lens-retrospective` and `bmad-lens-document-project` before marking a feature closed. Neither skill exists in the new-codebase source. A decision is required before `complete-ops.py` can be implemented.

**Decision: Graceful-Degradation is Canonical**

Evidence from old-codebase `complete-ops.py`:
- `check-preconditions` sets `retrospective_skipped = not (feature_dir / "retrospective.md").exists()`
- Returns `status: warn` (not `fail`) when `retrospective.md` is absent
- `finalize` proceeds with `retrospective_skipped=True` in output
- No subprocess delegation to the retrospective or document-project skills — only file-existence checks

The old-codebase already chose graceful-degradation. The new codebase must preserve this behavior.

**Implications for implementation:**
- `check-preconditions` raises `status: warn` for missing `retrospective.md` or absent document-project artifact
- `finalize` proceeds with warnings; output includes `retrospective_skipped` and `document_project_skipped` flags
- Phase guard (`feature.phase not in completable_phases`) remains a hard fail — that is not a prerequisite, it is a state guard

**Companion governance action:**  
`TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-complete/feature.yaml` must be annotated with a blocker note referencing this ADR until `lens-dev-new-codebase-complete` dev phase activates. This annotation is a post-audit commit, per NFR-1 (audit evidence collection is read-only).

---

### ADR-2: Constitution `permitted_tracks` Template

**Status:** Proposed → Accepted on techplan review pass  
**Date:** 2026-04-28

**Context:**  
New-codebase constitutions include `express` and `expressplan` in `permitted_tracks`; old-codebase constitutions do not. A canonical template must be chosen so that future `create-domain` and `create-service` invocations produce consistent constitutions.

**Decision: New-Codebase Template is Canonical (`express` + `expressplan` Included)**

Evidence:
- `lifecycle.yaml` v4 (`schema_version: 4`) explicitly defines `expressplan` as a supported phase with its own `completion_review` contract
- The `expressflow` track is used in active governance features (e.g., `lens-dev-new-codebase-new-feature` carries a `track_notes` referencing expressflow exception)
- The domain constitution (`constitutions/lens-dev/constitution.md`) lists `expressplan` as a permitted track at the domain level
- Old-codebase omission was a documentation lag — the lifecycle evolved to add express tracks after the old constitution template was written

**Implications:**
- `create-domain` and `create-service` in `init-feature-ops.py` must generate constitutions that include `[quickplan, full, hotfix, tech-change, express, expressplan]` as the default `permitted_tracks` list
- The `parity-audit-report.md` must document this divergence as resolved (not a gap)
- Reference documents `references/auto-context-pull.md` and `references/init-feature.md` must reflect the canonical track list

---

### ADR-3: Python 3.12 Requirement in `init-feature-ops.py`

**Status:** Proposed → Accepted on techplan review pass  
**Date:** 2026-04-28

**Context:**  
`init-feature-ops.py` in the new codebase declares `requires-python = ">=3.12"`, up from `>=3.10` in the old codebase. This was unconfirmed as intentional or drift.

**Decision: Intentional — `tomllib` and Structural Pattern Matching**

Based on the implementation evidence in `init-feature-ops.py`:
- The script uses `tomllib` (standard library since Python 3.11) for parsing `pyproject.toml` config
- It uses structural pattern matching (`match`/`case` syntax, available since Python 3.10, but used in style consistent with 3.10+)
- `tomllib` usage alone would require `>=3.11`; declaring `>=3.12` aligns with the current supported Python release series and avoids the transitional 3.11 version

**Implications:**
- `requires-python = ">=3.12"` is the correct declaration for the new codebase
- All new scripts added to the new-codebase source must use the same minimum version
- The parity audit report must document this as an explicit, reviewed decision (not a gap)
- Deployment environments running Python 3.10 or 3.11 must be upgraded before adopting new-codebase scripts

---

### ADR-4: `SAFE_ID_PATTERN` Tightening in `switch-ops.py`

**Status:** Proposed → Accepted on techplan review pass  
**Date:** 2026-04-28

**Context:**  
Old pattern: `^[a-z0-9][a-z0-9._-]{0,63}$` — permits dots and underscores  
New pattern: `^[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$` — hyphens only, must not start/end with hyphen

If any live feature IDs contain dots or underscores, `switch-ops.py` would silently reject them.

**Decision: Safe to Adopt — No Existing Feature IDs Use Dots or Underscores**

Evidence (verified by scan of `TargetProjects/lens/lens-governance/`):
- `feature-index.yaml` scanned: all `featureId` values use hyphen-only naming (e.g., `lens-dev-new-codebase-trueup`, `lens-dev-release-discovery`)
- All `feature.yaml` files under `features/` scanned: same pattern — no dots, no underscores in `featureId` or `featureSlug` fields
- The canonical `featureId` construction in `init-feature-ops.py` normalizes to `{domain}-{service}-{slug}` with hyphens only; dots and underscores in input are normalized away before the ID is written

The new pattern is a forward-compatible tightening that enforces what the ID construction already produces.

**Implications:**
- No migration required for existing governance data
- The parity audit report must document this as resolved (no breakage risk found)
- Future `init-feature-ops.py` or governance tooling must not write IDs with dots or underscores; the new pattern is the enforcement mechanism

---

## 5. Parity Gate Specification (FR-14 Contract)

A retained command is **fully migrated** when it passes all gates in the following checklist. This spec is the reusable artifact that makes True Up's verification coherent across the 17 retained commands.

### Structural Gates (Verifiable by File Existence)

| Gate | Check | Pass Condition |
|------|-------|----------------|
| S1 | SKILL.md | `_bmad/lens-work/skills/bmad-lens-{command}/SKILL.md` exists |
| S2 | Primary script | `_bmad/lens-work/skills/bmad-lens-{command}/scripts/{command}-ops.py` exists |
| S3 | Test file | `_bmad/lens-work/skills/bmad-lens-{command}/scripts/tests/test-{command}-ops.py` exists |
| S4 | Source prompt stub | `_bmad/lens-work/prompts/lens-{command}.prompt.md` exists |
| S5 | Mirror prompt stub | `.github/prompts/lens-{command}.prompt.md` exists |
| S6 | Governance phase label | `feature.yaml.phase` consistent with implementation state |
| S7 | Review artifact | Phase gate review report committed to staged docs path |

### Behavioral Gates (Verifiable by Dry-Run / Output Inspection)

| Gate | Check | Pass Condition |
|------|-------|----------------|
| B1 | Required output fields | All schema fields from old-codebase contract present and correctly typed |
| B2 | Dry-run support | `--dry-run` flag supported where old-codebase had it; no writes in dry-run mode |
| B3 | Failure shape | Failure returns `{status: fail, error: "...", message: "..."}` structured JSON |
| B4 | `--governance-repo` support | All scripts that write governance files accept `--governance-repo <path>` |
| B5 | ID pattern compliance | All generated IDs match `SAFE_ID_PATTERN` (`^[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$`) |

### Design Decision Gates (Verifiable by ADR Existence)

| Gate | Check | Pass Condition |
|------|-------|----------------|
| D1 | Breaking changes documented | Any schema, identifier, or runtime change from old-codebase is documented with explicit decision |
| D2 | Prerequisite contracts in SKILL.md | Cross-skill dependencies named and prerequisite handling strategy stated |
| D3 | Clean-room attestation | No old-codebase source copied; behavioral contracts derived from discovery docs |

### Gate Verdict Definitions

| Verdict | Meaning |
|---------|---------|
| `pass` | All structural, behavioral, and design gates satisfied |
| `partial` | Structural gates pass; behavioral or design gates have open items |
| `gap` | One or more structural gates fail (file absent) |
| `regression` | Behavioral gate fails and the old-codebase passed it (functional regression) |

---

## 6. Parity Audit Scope and Evidence Requirements

### 6.1 Feature Verdicts (Pre-Architecture Assessment)

Based on research findings, the expected verdicts prior to dev execution are:

| Feature | Structural | Behavioral | Governance Label | Pre-Audit Verdict |
|---------|-----------|-----------|-----------------|------------------|
| `new-domain` | pass (S1–S5 met) | partial (B1 unverified) | correct (complete) | `partial` |
| `new-service` | pass (S1–S5 met) | partial (B1 unverified) | correct (complete) | `partial` |
| `switch` | partial (S5 missing — `.github/prompts/` mirror absent) | partial (B5: SAFE_ID_PATTERN change documented) | correct (complete) | `partial` |
| `new-feature` | gap (S4 missing — no prompt stub) | regression (B1: `fetch-context`, `read-context` absent) | correct (finalizeplan-complete) | `regression` |
| `complete` | gap (S1–S3 absent, S4 absent) | n/a (no script to evaluate) | correct (finalizeplan-complete) | `gap` |

### 6.2 `fetch-context` / `read-context` Old-Codebase Contract (M2 Restoration Target)

Per the businessplan adversarial review M2 finding, the parity audit report must name the old-codebase output contract as the explicit restoration target for `lens-dev-new-codebase-new-feature`.

**`fetch-context` expected output contract:**

```json
{
  "status": "pass",
  "related": ["<featureId>", ...],
  "depends_on": ["<featureId>", ...],
  "blocks": ["<featureId>", ...],
  "context_paths": {
    "domain": "<governance_domain_path>",
    "service": "<governance_service_path>",
    "feature": "<governance_feature_path>"
  },
  "service_ref": "<optional: named service governance path>"
}
```

**`read-context` expected output contract:**

```json
{
  "domain": "<domain>",
  "service": "<service>",
  "updated_at": "<ISO timestamp>",
  "updated_by": "<github_username>"
}
```

These contracts are the acceptance criteria for the `fetch-context` / `read-context` restoration in the `lens-dev-new-codebase-new-feature` dev phase.

---

## 7. Project Structure — New Files Created by This Feature

All files committed to the control repo plan branch.

> **Scope boundary:** Files listed under `lens.core.src/` are new-codebase source artifacts (live in the new-codebase authoring workspace). Files listed under `docs/` are control-repo planning artifacts. These are **two separate root trees** — do not create `docs/` files under `lens.core.src/` or vice versa.

```
── SCOPE: new-codebase source workspace ─────────────────────────────────────
lens.core.src/_bmad/lens-work/
  prompts/
    lens-new-feature.prompt.md             ← FR-2 (new)
    lens-complete.prompt.md                ← FR-3 (new)
    [lens-switch.prompt.md already exists] ← FR-1 (exists, mirror only needed)

  skills/
    bmad-lens-init-feature/
      references/
        auto-context-pull.md               ← FR-13 (new, clean-room authored)
        init-feature.md                    ← FR-13 (new, clean-room authored)

    bmad-lens-complete/
      SKILL.md                             ← FR-4 (new — authored via BMB channel)
      scripts/
        tests/
          test-complete-ops.py             ← FR-5 (new — scaffolded stubs)

lens.core.src/.github/prompts/
  lens-switch.prompt.md                    ← FR-1 (copy of source prompt)
  lens-new-feature.prompt.md               ← FR-2 (mirror)
  lens-complete.prompt.md                  ← FR-3 (mirror)

── SCOPE: control-repo planning artifacts ───────────────────────────────────
docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/
  architecture.md                          ← this document
  adr-complete-prerequisite.md             ← FR-6
  adr-constitution-tracks.md              ← FR-7
  parity-gate-spec.md                     ← FR-14
  parity-audit-report.md                  ← FR-15 (consolidated FR-8, FR-9, FR-10–FR-12)
```

---

## 8. Implementation Sequence

Execution order for dev phase follows NFR-3 (atomic commits per logical group, all to the plan branch before any governance publication):

1. **ADRs first** — `adr-complete-prerequisite.md` and `adr-constitution-tracks.md`; these unblock all downstream artifacts that depend on resolved decisions
2. **Parity gate spec** — `parity-gate-spec.md`; establishes the measurement framework
3. **Parity audit report** — evidence collection (read-only per NFR-1); runs FR-9, FR-10–FR-12; documents FR-8 Python decision
4. **Prompt stubs** — `lens-new-feature.prompt.md`, `lens-complete.prompt.md` (source and mirror); `lens-switch.prompt.md` mirror to `.github/prompts/`
5. **`bmad-lens-complete` SKILL.md** — load `lens.core/_bmad/bmb/bmadconfig.yaml` and invoke the `bmad-module-builder` skill; load the BMad Builder reference index at `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md` before authoring begins; ADR-1 must be resolved before authoring starts
6. **`bmad-lens-complete` test stubs** — scaffolded after SKILL.md is accepted
7. **Reference documents** — `auto-context-pull.md` and `init-feature.md` in `bmad-lens-init-feature/references/`
8. **Governance companion actions** — blocker annotation on `lens-dev-new-codebase-complete/feature.yaml` (post-audit, separate commit per NFR-1)

---

## 9. Cross-Feature Dependencies and Governance Impact

### 9.1 Features Unblocked by True Up

| Feature | Unblocked By |
|---------|-------------|
| `lens-dev-new-codebase-complete` | ADR-1 (prerequisite strategy), SKILL.md package, test stubs |
| `lens-dev-new-codebase-new-feature` | Parity audit report formalizes the `fetch-context`/`read-context` restoration target; prompt stub provides discoverability |
| Future `create-domain` / `create-service` work | ADR-2 (canonical constitution template) |
| All retained command implementations | Parity gate spec provides the migration quality bar |

### 9.2 Features Not Affected by True Up

The 12 preplan-phase features (`next`, `preplan`, `businessplan`, `techplan`, `finalizeplan`, `expressplan`, `dev`, `split-feature`, `constitution`, `discover`, `upgrade`, `preflight`) are explicitly out of scope. True Up does not open, plan, or unblock them.

### 9.3 Governance Records

| Record | Change | Timing |
|--------|--------|--------|
| `feature-index.yaml` | No change | None required |
| `lens-dev-new-codebase-complete/feature.yaml` | Add blocker annotation referencing ADR-1 | Post-audit commit (separate from audit artifacts) |
| `lens-dev-new-codebase-new-feature/feature.yaml` | Add blocker annotation: `fetch-context`/`read-context` restoration required | Post-audit commit (separate from audit artifacts) |

---

## 10. Constraints Summary

| Constraint | Source | Enforcement |
|-----------|--------|-------------|
| `lc-agent-core-repo` out of scope | research.md open question | Dev agents must not modify or attempt to recreate this skill during True Up; source investigation deferred to a separate feature |
| Clean-room operating model | PRD §1, architecture decision | No old-codebase code may be copied to new-codebase artifacts |
| BMB implementation channel | Domain constitution H1; businessplan review warning | All SKILL.md authoring routes through bmb module |
| Audit evidence collection is read-only | NFR-1 | No `feature.yaml` or `feature-index.yaml` mutations during evidence gathering |
| Prompt stubs are delegation-only | NFR-2 | No behavioral logic in prompt stubs |
| Atomic commits on plan branch | NFR-3 | Parity audit artifacts and governance companion actions in separate commits |
| SKILL.md completeness required before commit | NFR-4 | `bmad-lens-complete/SKILL.md` not committed until ADR-1 is resolved |
| `feature.yaml.phase` is authoritative | businessplan review M1 | `feature-index.yaml.status` must not be used as a phase gate source of truth |
| `fetch-context`/`read-context` is a functional regression | businessplan review M2 | Parity audit report must classify as regression with the old contract as restoration target |
