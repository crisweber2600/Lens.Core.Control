---
feature: lens-dev-new-codebase-bugbash
doc_type: architecture
status: draft
goal: "Design the bugbash meta-workflow for tracking and fixing bugs in lens-work rebuild; define conductor pattern, governance integration, and batch processing semantics"
key_decisions:
  - Bug storage organized by status (New/Inprogress/Fixed) with markdown frontmatter
  - N Bugs → 1 Feature mapping with millisecond-timestamp + random-suffix featureId (high-entropy; collision-safe for concurrent use)
  - Per-item failure isolation: failed bugs remain in prior valid state (New); successful bugs proceed independently
  - Status mutations at phase boundaries (New → Inprogress → Fixed) with explicit commits
  - Self-service developer assignment (runner becomes primary assignee)
  - Explicit feature-index sync via publish-to-governance (BF-3 workaround)
  - Bugbash implemented as LENS Conductor Pattern (thin orchestrator delegating to shared utilities)
  - Release prompts authored via bmad-workflow-builder; SKILL.md via bmad-module-builder (BMB-first)
open_questions: []
depends_on:
  - prd.md (this feature)
  - lens-dev-new-codebase-baseline architecture.md
blocks: []
updated_at: 2026-05-03T00:00:00Z
stepsCompleted: [1, 2, 3, 4, 6]
inputDocuments:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-bugbash/prd.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/architecture.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/research.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/bugfixes.md
  - TargetProjects/lens/lens-governance/features/lens-dev/service.yaml
  - TargetProjects/lens/lens-governance/features/lens-dev/domain.yaml
---

# Architecture Decision Document

_This document builds collaboratively through step-by-step discovery. Sections are appended as we work through each architectural decision together._

---

## Project Context Analysis

### Requirements Overview

**Functional Requirements:**

The bugbash feature is a meta-workflow designed to track and fix bugs in the lens-work rebuild itself:

- **Bug Report Creation:** Accept bug descriptions and associated chat logs; create governance-backed bug records with title, description, and status frontmatter
- **Bug Storage:** Persist bugs in governance repo (`bugs/` folder) with markdown frontmatter metadata (title, description, status: New/Inprogress/Fixed, featureId)
- **Batch Bug Fixing:** Query governance for all bugs with status=New; auto-generate a feature per bug; execute expressplan workflow on each bug's feature; update bug status to Inprogress during execution
- **Feature Generation & Mutation:** Create feature entries with linked featureId; populate featureId frontmatter when bug is created; update featureId in bug record upon fix completion
- **Two-Developer Workflow:** Assign two lens developers to all bugs created in a batch; ensure consistent team composition across generated features

**Non-Functional Requirements:**

- **Cleanroom Scope:** Only new-codebase artifacts; no cross-service dependencies
- **Governance Consistency:** Bug records must integrate seamlessly with existing feature-index.yaml and governance structure
- **Lifecycle Integration:** Bugbash must coordinate with lens-work express-track phase contract and frozen interfaces (light-preflight.py, publish-to-governance CLI)
- **Batch Atomicity:** Multiple bugs processed in one batch should have consistent status updates and feature creation semantics
- **Known Gap Accommodation:** Must work within the constraints of three open bugs (BF-1: branch lifecycle, BF-2: username config, BF-3: feature-index sync) or explicitly flag blockers

### Technical Constraints & Dependencies

**Frozen Interfaces (must not change):**
- `light-preflight.py` exit-code contract (exit 0 = proceed, non-zero = stop)
- `publish-to-governance` CLI path for governance mutation
- `feature.yaml` schema v4 (all fields read-only for this rewrite)
- `featureId` formula: `{domain}-{service}-{featureSlug}` (immutable)
- 2-branch topology: `{featureId}` and `{featureId}-plan`

**Governance Integration Points:**
- Bug storage paths (operational state — direct script writes, not publish-to-governance): `governance_repo/bugs/New/{slug}.md`, `governance_repo/bugs/Inprogress/{slug}.md`, `governance_repo/bugs/Fixed/{slug}.md`
- Feature registry (governance mirror — publish-to-governance only): `governance_repo/features/lens-dev/new-codebase/{featureId}/`
- Feature-index sync: BF-3 gap — explicit `publish-to-governance --update-feature-index` call after feature creation

> **Write authority note:** `bugs/` is **operational state** written directly by bugbash scripts. It is not a feature docs mirror and does not go through the `publish-to-governance` CLI. Feature docs mirrors (under `features/`) continue to use publish-to-governance exclusively. This distinction is intentional: bug status mutations require direct file moves; a publish-CLI layer would break the status-folder model.

**Lens-Work Dependency:**
- Bugbash is a **consumer of expressplan**, not a replacement or modification to it
- Each bug fix triggers expressplan execution with bug content as context
- Finalizeplan bundle reuse applies to all features generated by bugbash

### Scale & Complexity Assessment

- **Project complexity:** Medium (workflow orchestration + governance mutation + lifecycle coordination)
- **Primary technical domain:** Workflow lifecycle management + Governance integration
- **Estimated architectural components:** 6 major components
  1. Bug Reporter (governance-backed markdown + frontmatter schema)
  2. Bug Store (governance repo storage with persistence)
  3. Feature Generator (feature.yaml creation + feature-index registration)
  4. ExpressPlan Launcher (bug-to-feature routing + phase execution)
  5. Status Updater (frontmatter state mutation on completion)
  6. Batch Orchestrator (multi-bug processing with atomic consistency)

### Cross-Cutting Concerns Identified

**Concerns that will affect multiple components:**

1. **Frontmatter as State Contract** — Bug status lives in bug markdown frontmatter. All status mutations must be consistent across bug reporter, status updater, and governance queries.

2. **FeatureId Injection & Tracking** — FeatureId must be injected at bug creation, preserved through feature generation, and updated in bug frontmatter during completion. This is a single data contract spanning three workflows (reporter → generator → updater).

3. **Governance Repo Consistency** — Feature docs mirrors (`features/`) use publish-to-governance exclusively. Bug operational state (`bugs/`) is written directly by scripts — status moves require direct file operations that a publish-CLI layer cannot support. BF-3 (feature-index sync) is a known gap; resolved by explicit `publish-to-governance --update-feature-index` after feature creation.

4. **Batch Atomicity & Error Handling** — If one bug fails during batch fix execution, the batch should not partially complete with inconsistent states. Decisions needed on retry strategy and rollback behavior.

5. **Developer Assignment & Lifecycle** — Two developers assigned at bug creation must be preserved through feature generation, expressplan execution, and final completion. This is a carryover requirement from PRD developer-count specification.

6. **Known Gaps as Blockers** — BF-1 (branch lifecycle), BF-2 (username config), and BF-3 (feature-index sync) are open issues in lens-work that bugbash must either work around or escalate as architectural blockers.

---

## Starter Template Evaluation

### Primary Technology Domain

**Workflow Lifecycle Conductor** — not a traditional web app or API, but a governance-integrated workflow orchestrator within the LENS Workbench ecosystem.

### Technology Stack Foundation

Based on existing lens-work architecture:

- **Language:** Python (consistent with lens-work scripts, git-orchestration-ops.py)
- **Orchestration model:** BMAD conductor pattern (skill-based workflow delegation)
- **Governance integration:** Direct feature.yaml, feature-index.yaml, publish-to-governance CLI
- **Execution model:** Prompt stub → SKILL.md → orchestration script chain
- **Data contracts:** Frozen schemas (v4 lifecycle)

### Starter Template Selection: LENS Conductor Pattern

Bugbash follows the **established conductor pattern** from existing lens-work commands (finalizeplan, expressplan, dev).

**Architectural Foundation Structure:**

```
lens-dev-new-codebase-bugbash/
├── .github/prompts/
│   ├── lens-bugbash.prompt.md              (stub: light-preflight gate)
│   ├── lens-bug-reporter.prompt.md
│   └── lens-bug-fixer.prompt.md
├── lens.core/_bmad/lens-work/
│   ├── skills/
│   │   ├── bmad-lens-bugbash/              (main entry conductor)
│   │   │   └── SKILL.md
│   │   ├── bmad-lens-bug-reporter/
│   │   │   └── SKILL.md
│   │   └── bmad-lens-bug-fixer/
│   │       └── SKILL.md
│   ├── scripts/                            (shared — authored directly, not via bmad-module-builder)
│   │   ├── bugbash-ops.py                  (status summary + routing)
│   │   ├── bug-reporter-ops.py             (bug creation + governance storage)
│   │   └── bug-fixer-ops.py                (batch processing + status mutations)
│   └── prompts/
│       ├── lens-bugbash.prompt.md
│       ├── lens-bug-reporter.prompt.md
│       └── lens-bug-fixer.prompt.md
```

**Architectural Decisions Provided by Conductor Pattern:**

**Governance Integration:**
- Bug records stored in governance repo (`bugs/` folder) with markdown + frontmatter
- Feature generation via existing feature.yaml + feature-index.yaml infrastructure
- `bugs/` is **operational state** written directly by bugbash scripts (not via publish-to-governance); feature docs mirrors under `features/` use publish-to-governance exclusively

**Workflow Orchestration:**
- Two entry points: bug reporter (intake) and bug fixer (batch processing)
- Batch 2-pass contract: accumulate bugs → process all with pre-approved context
- Delegation to existing BMAD skills (feature-yaml, expressplan, git-orchestration)

**Developer Assignment Model:**
- Two developers assigned at bug creation
- Preserved through feature generation
- Injected into generated feature.yaml

**Known Gap Workarounds:**
- BF-1: Branch creation delegated to git-orchestration (bugbash does not directly create branches)
- BF-2: Username from existing lens context (not stored separately)
- BF-3: Explicit publish-to-governance call after feature creation to sync feature-index

---

## Core Architectural Decisions

### Decision 1: Bug Storage Structure

**Choice:** Status-organized file hierarchy

**Storage Model:**
```
governance_repo/bugs/
├── New/
│   ├── auth-system-jwt-validation.md
│   ├── feature-index-sync-gap.md
│   └── {descriptive-slug}.md
├── Inprogress/
│   ├── branch-lifecycle-missing-dev.md
│   └── ...
└── Fixed/
    ├── username-config-storage.md
    └── ...
```

**Rationale:** Status in folder path mirrors state machine visually; enables efficient batch queries (iterate `bugs/New/` for all New bugs); descriptive slugs make bugs discoverable.

**Implications:**
- Status mutation = file move between folders
- Filename slugification required
- FeatureId generated independently, stored in frontmatter
- **Related Decision:** Status mutation contract (Decision 4)

---

### Decision 2: Bug-to-Feature Mapping

**Choice:** N Bugs → 1 Feature (batch grouping at fix time)

**Mapping Logic:**
- At bug creation: no feature association
- At bug-fix time: query all bugs WHERE status=New
- Generate single feature: `lens-dev-new-codebase-bugfix-{timestamp}`
- All bugs in batch assigned same featureId
- Execute single expressplan for entire batch

**Rationale:** Automatic grouping (no user selection); atomic batch execution; efficient single feature for N bugs; millisecond timestamp + random 4-char hex suffix prevents collisions even for concurrent same-second runs.

**Implications:**
- All bugs in batch succeed or fail together
- Single featureId spans entire batch
- Feature title: "Bugbash Batch Fix - {timestamp}"
- Bug description consolidated in feature description
- **Related Decision:** Batch processing model (Decision 3)

---

### Decision 3: Batch Processing Error Handling

**Choice:** Per-Item Isolation

**Error Handling Model:**
```
Phase 2 (feature creation):
  - If feature creation fails: stop; no bugs touched; report error

Phase 3 (status -> Inprogress):
  - For each bug independently:
      - Success: move to Inprogress, featureId set
      - Failure: bug remains in New; error recorded in per-item report; continue with next bug

Phase 4 (expressplan):
  - If expressplan fails: all bugs remain Inprogress
  - Retry via --complete {featureId} or new --fix-all-new run

Fixed transition:
  - Only via explicit --complete {featureId} command
  - Per-item: each linked bug independently moved to Fixed; failures recorded
```

**Rationale:** Per-item isolation prevents one failure from blocking all bugs; failed bugs remain in prior valid state (New) as required by PRD NFR8/NFR9 and FR20-FR24; consistent with per-item outcome reporting contract.

**Implications:**
- Individual failures do not abort the batch
- Failed bugs stay in New; successful bugs advance to Inprogress
- Per-item error report required on every batch run
- Expressplan failure does not roll back Inprogress promotions
- ⚠️ Partial Inprogress state is expected and recoverable via --complete or retry

---

### Decision 4: Status Mutation Contract

**Choice:** Move-at-Phase-Boundary (three-commit lifecycle)

**Status Transition Flow:**

```
fix-all-new Phase 2: Feature Creation
  - Feature.yaml created and published; feature-index synced
  - Commit: "[BUGBASH] Batch {featureId} feature created"

fix-all-new Phase 3: Status → Inprogress
  - Per-bug (independently): bugs/New/{name}.md → bugs/Inprogress/{name}.md
  - Frontmatter update: status=Inprogress, featureId={featureId}, updated_at={timestamp}
  - Commit: "[BUGBASH] Batch {featureId} moved to Inprogress"

--complete Phase: Status → Fixed
  - Per-bug (independently): bugs/Inprogress/{name}.md → bugs/Fixed/{name}.md
  - Frontmatter update: status=Fixed, updated_at={timestamp}
    (featureId field already set during Inprogress transition; unchanged at completion)
  - Commit: "[BUGBASH] Batch {featureId} completed"
```

**Rationale:** Feature creation committed before bugs promoted to Inprogress prevents orphaned featureId references; clear phase boundaries in git history; explicit transitions; Fixed state only reachable via explicit --complete command.

**Implications:**
- Two commits per batch (acceptable trade-off)
- Inprogress state queryable for monitoring
- If crash between phases: manual recovery needed
- File moves tracked in git

---

### Decision 5: Developer Assignment Model

**Choice:** Open/Self-Service (no pre-assignment)

**Assignment Logic:**
- Bug creation: no developers assigned (team frontmatter empty or omitted)
- Bug fix time: any developer can run `/lens-bugbash --fix-all-new`
- Feature.yaml team field populated: `team: [current_runner, backup_default]`
- Optional override: `--assign-team {dev1} {dev2}`

**Rationale:** Self-service reduces coordination; runner becomes primary assignee (built-in accountability); flexible override for special cases; no pre-configuration needed.

**Implications:**
- Requires user context at runtime
- Backup developer must be configurable or defaulted
- Feature team automatically reflects who executed the fix
- Open to any developer (lower friction)

---

### Decision 6: Governance Sync Strategy (BF-3 Workaround)

**Choice:** Explicit Feature-Index Update

**Sync Model:**
```
After feature creation:
  1. Feature.yaml created via bmad-lens-feature-yaml
  2. Explicitly call: publish-to-governance --update-feature-index
  3. Ensure feature-index entry created/updated
  4. Commit: "[BUGBASH] Sync feature-index for batch feature"
```

**Rationale:** Works around BF-3 gap; prevents stale index entries; maintains governance consistency; explicit dependency makes gap visible.

**Implications:**
- Requires feature-index CLI endpoint; if endpoint unavailable Story 2.1 is blocked until endpoint is verified or an alternative is documented (no direct write fallback — governance rules prohibit direct feature docs mutations)
- Extra CLI call per batch (minimal overhead)
- BF-3 dependency documented as assumption
- Visibility into when workaround is in effect

---

### Decision Impact Analysis

**Implementation Sequence (dependency order):**

1. Bug schema & storage (foundation)
2. Bug reporter command (intake)
3. Feature generator (batch mapping)
4. Status updater & file mover (mutation)
5. Batch orchestrator (expression orchestration)
6. Feature-index sync (governance consistency)
7. Bug fixer command (full workflow)

**Cross-Component Dependencies:**

```
Bug Reporter
  → creates bug in bugs/New/
    → relies on frontmatter schema
    
Bug Fixer (batch)
  → queries bugs/New/ (status storage design)
    → generates feature (bug-to-feature mapping)
    → moves files to Inprogress (status mutation phase 1)
    → executes expressplan (batch processing)
    → syncs feature-index (governance sync)
    → moves files to Fixed (status mutation phase 3)
    → updates bug frontmatter with featureId (feature mapping)
```

**Architectural Constraints Respected:**

- ✅ Frozen interfaces: light-preflight.py, publish-to-governance CLI
- ✅ Governance rules: bugs/ written directly by scripts (operational state); features/ via publish-to-governance CLI only
- ✅ Feature.yaml schema: v4 frozen, no field changes
- ✅ FeatureId formula: preserved
- ✅ Cleanroom scope: new-codebase only
- ✅ 2-branch topology: delegated to git-orchestration

---

## Implementation Roadmap

### Component Architecture (Conductor Pattern)

**Three-layer command chain for each bugbash command:**

```
.github/prompts/lens-{command}.prompt.md         (Agent-created stub)
  → lens.core/_bmad/lens-work/prompts/lens-{command}.prompt.md  (Release prompt)
    → skills/bmad-lens-{command}/SKILL.md        (Thin conductor)
      → shared utilities + scripts
```

### Bugbash Commands & Implementations

| Command | Purpose | Stub | Release Prompt | Skill | Script |
|---------|---------|------|---|---|---|
| `lens-bugbash` | Main entry (list/fix/report) | `.github/prompts/lens-bugbash.prompt.md` | `lens-work/prompts/lens-bugbash.prompt.md` | `skills/bmad-lens-bugbash/SKILL.md` | `scripts/bugbash-ops.py` |
| `lens-bug-reporter` | Report new bug | `.github/prompts/lens-bug-reporter.prompt.md` | `lens-work/prompts/lens-bug-reporter.prompt.md` | `skills/bmad-lens-bug-reporter/SKILL.md` | `scripts/bug-reporter-ops.py` |
| `lens-bug-fixer` | Fix batch of bugs | `.github/prompts/lens-bug-fixer.prompt.md` | `lens-work/prompts/lens-bug-fixer.prompt.md` | `skills/bmad-lens-bug-fixer/SKILL.md` | `scripts/bug-fixer-ops.py` |

### Authority & Implementation Channels

| Artifact | Creator | Channel | Location |
|---------|---------|---------|----------|
| `.github/prompts/lens-bugbash.prompt.md` | Agent | Standard (established pattern) | `.github/prompts/` |
| `lens-work/prompts/lens-bugbash.prompt.md` | Agent | `bmad-workflow-builder` | `lens.core/_bmad/lens-work/prompts/` |
| `skills/bmad-lens-bugbash/SKILL.md` | Agent | `bmad-module-builder` (BMB-first) | `lens.core/_bmad/lens-work/skills/` |
| `scripts/bugbash-ops.py` | Agent | Direct (Python — not bmad-module-builder) | `lens.core/_bmad/lens-work/scripts/` |
| `bugs/{status}/*.md` | Agent (via bug-reporter) | Control repo artifacts | `governance_repo/bugs/{New\|Inprogress\|Fixed}/` |
| `feature.yaml` updates | CLI (publish-to-governance) | Governance CLI only | `governance_repo/features/...` |
| `feature-index.yaml` sync | CLI (publish-to-governance) | Governance CLI only | `governance_repo/` |

### Workflow: Fix All New Bugs

**Entry:** User runs `/lens-bugbash --fix-all-new`

**Phase 1: Discovery & Batch Formation**
1. Query `governance_repo/bugs/New/*.md`
2. Parse frontmatter (title, description, featureId="" initially)
3. Group into single batch
4. Generate featureId: `lens-dev-new-codebase-bugfix-{ms-timestamp}-{random4hex}`
   (no commit — batch formation is in-memory only)

**Phase 2: Feature Creation (before status promotion)**
1. Generate feature via bmad-lens-feature-yaml
2. Populate feature.yaml: team = [current_runner, backup_developer]
3. Call publish-to-governance --update-feature-index (BF-3 workaround)
4. **Commit Phase 2:** `[BUGBASH] Batch {featureId} feature created`
5. If feature creation fails: stop; all bugs remain New; report error

**Phase 3: Status Transition to Inprogress (only after feature exists)**
1. For each bug independently:
   - Move `bugs/New/{slug}.md` → `bugs/Inprogress/{slug}.md`
   - Update frontmatter: status=Inprogress, featureId={featureId}, updated_at={timestamp}
   - On per-bug failure: record error, leave bug in New, continue with next bug
2. **Commit Phase 3:** `[BUGBASH] Batch {featureId} moved to Inprogress`

**Phase 4: Expressplan Execution**
1. Execute expressplan workflow on feature
   - Run all phase states (businessplan → techplan → finalizeplan → expressplan)
   - Generate all downstream artifacts (stories, implementation-readiness, sprint-status)
2. Bugs remain Inprogress after this phase
3. [Workflow ends here; Fixed transition reserved for --complete {featureId}]

**Error Handling (per-item isolation):**
- Phase 2 failure: all bugs remain New; no commits for this batch
- Phase 3 per-bug failure: failed bugs remain New; other bugs proceed to Inprogress
- Phase 4 failure: all bugs remain Inprogress; retry via --complete or new --fix-all-new run
- Per-item error report always printed at end of run

### Success Criteria (Validation Gates)

| Component | Validation | Pass Condition |
|-----------|-----------|---|
| Bug schema | Frontmatter parses | title, description, status, featureId all present |
| Storage structure | Folders exist | `bugs/New/`, `bugs/Inprogress/`, `bugs/Fixed/` all writable |
| Feature generation | Feature.yaml v4 valid | featureId matches formula; team array populated |
| Status transitions | File moves atomic | No orphaned bugs; all files moved or none |
| Expressplan execution | Workflow completes | All phase artifacts generated; sprint-status.yaml valid |
| Feature-index sync | Entry created | Feature appears in `feature-index.yaml` |
| Governance commit | Git history clean | Two commits per phase; messages follow pattern |

### Testing Strategy

**Unit Tests:**
- Frontmatter parser (valid/invalid schemas)
- FeatureId generation (timestamp collision detection)
- Status state machine (allowed transitions, forbidden moves)
- File mover (atomic move, cleanup on failure)

**Integration Tests:**
- Full workflow: bug creation → batch fix → feature generated → expressplan executed
- Error scenarios: network failure, permission denied, parse failure during any phase
- Recovery: resume from Inprogress state after crash

**Governance Validation:**
- Feature-index entry created and discoverable
- Commit history auditable
- Rollback procedure documented (manual process)

---

## Finalization Checklist

- [x] All 6 core decisions documented with rationale
- [x] Frozen interfaces honored (light-preflight.py, publish-to-governance CLI)
- [x] Authority domains validated (agent creates `.github/` stubs as established pattern)
- [x] Known gaps worked around (BF-1, BF-2, BF-3)
- [x] Cross-dependencies validated
- [x] Implementation roadmap defined
- [x] Conductor pattern components mapped
- [x] Workflow phases sequenced
- [x] Success criteria specified

**Architecture Review Result:** ✅ **VALIDATED** — Ready for Epics & Stories generation

---

**Next Steps:**
1. Generate Epics & Stories from architecture decisions
2. Create sprint plan
3. Implement skills and scripts (BMB-first)
4. Validation testing
5. Merge and publish to governance
