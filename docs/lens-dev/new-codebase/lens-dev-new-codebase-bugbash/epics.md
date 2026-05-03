---
feature: lens-dev-new-codebase-bugbash
doc_type: epics
status: draft
stepsCompleted: [step-01-validate-prerequisites, step-02-design-epics, step-03-create-stories]
inputDocuments:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-bugbash/prd.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-bugbash/architecture.md
key_decisions:
  - Bug intake creates one artifact per run
  - Fixbugs batch-processes all New bugs in one run (N bugs -> 1 feature)
  - All-or-nothing atomicity per bug; failed bugs remain New
  - Status mutations at phase boundaries with explicit commits
  - Self-service developer assignment (runner becomes primary)
  - Conductor pattern with bmad-workflow-builder for prompts, bmad-module-builder for SKILL.md
open_questions: []
depends_on: [prd.md, architecture.md]
blocks: []
updated_at: 2026-05-03T00:00:00Z
---

# Bugbash - Epic Breakdown

## Overview

This document provides the complete epic and story breakdown for the **bugbash** feature, decomposing requirements from the PRD and Architecture into implementable stories for a 2-developer team.

---

## Requirements Inventory

### Functional Requirements

FR1: Lens developers can create a single bug record per intake run.
FR2: Lens developers can provide a bug title during intake.
FR3: Lens developers can provide a bug description during intake.
FR4: Lens developers can provide a pasted chat log during intake.
FR5: The workflow can persist each accepted bug record as a markdown artifact in the governance bugs folder scoped to new-codebase.
FR6: The workflow can reject intake when required frontmatter fields are missing.
FR7: The workflow can assign status New to newly created bug artifacts.
FR8: The workflow can enforce allowed bug statuses as New, Inprogress, and Fixed.
FR9: The workflow can update status from New to Inprogress when fix work is initiated.
FR10: The workflow can update status from Inprogress to Fixed when fix completion is confirmed.
FR11: The workflow can prevent invalid status transitions that violate lifecycle rules.
FR12: The workflow can preserve prior valid status when a transition operation fails.
FR13: The fix workflow can discover all bug artifacts with status New within new-codebase scope.
FR14: The fix workflow can create an express-track feature for each discovered New bug.
FR15: The fix workflow can execute expressplan using each bug artifact's description and chat log content.
FR16: The fix workflow can write featureId linkage into bug frontmatter after feature creation.
FR17: The workflow can prevent promotion to Inprogress when featureId linkage is not established.
FR18: The completion workflow can resolve linked bug records by featureId before setting Fixed.
FR19: Lens developers can run fixbugs as a single batch operation across all eligible New bugs.
FR20: The workflow can process bugs independently within a batch run.
FR21: The workflow can report per-bug success or failure outcomes for each batch run.
FR22: The workflow can continue processing remaining bugs when one bug fails in batch.
FR23: The workflow can support idempotent reruns without duplicating bug artifacts.
FR24: The workflow can leave failed or unprocessed bugs in a recoverable state for subsequent reruns.
FR25: The workflow can restrict all reads and writes to new-codebase artifacts only.
FR26: The workflow can block cross-domain or cross-service mutation attempts.
FR27: The workflow can preserve governance artifact history through normal git-traceable file updates.
FR28: The workflow can keep bug records as canonical lifecycle state for intake, active fix work, and completion.
FR29: Two Lens developers can operate the workflow concurrently without changing the lifecycle model.
FR30: Developers can inspect bug records to determine current status and linked feature work.
FR31: Developers can identify in-progress remediation by reading status plus featureId from bug artifacts.
FR32: Developers can distinguish completed fixes from active fixes using frontmatter state only.

### Non-Functional Requirements

NFR1 (Performance): Intake run can validate and persist a single bug artifact within 5 seconds under normal repository conditions.
NFR2 (Performance): Fixbugs batch run can produce a per-item outcome summary for all discovered New bugs in a single execution.
NFR3 (Performance): Frontmatter update operations can complete without blocking unrelated bug records in the same batch.
NFR4 (Security): Workflow can prevent writes outside the new-codebase scope through mandatory path validation before mutation.
NFR5 (Security): Bug artifacts can preserve chat-log content without exposing data outside governance-authorized repositories.
NFR6 (Security): Only authorized Lens developer workflows can mutate bug status or featureId fields.
NFR7 (Security): Feature linkage mutations can be auditable via git history for each changed bug artifact.
NFR8 (Reliability): Batch processing can isolate per-item failures so one failed bug does not abort successful updates for other eligible bugs.
NFR9 (Reliability): Failed items can remain in their prior valid lifecycle state (typically New) with explicit error reporting.
NFR10 (Reliability): Reruns can be idempotent for already-processed items unless state has changed intentionally.
NFR11 (Reliability): Completion updates can require successful bug-to-feature resolution before status promotion to Fixed.
NFR12 (Integration): Fix workflow can integrate with feature creation using express track and consume returned featureId values.
NFR13 (Integration): Fix workflow can integrate with expressplan execution using each bug artifact's description and chat log body.
NFR14 (Integration): Completion workflow can integrate with fix completion signals to update linked bug records to Fixed.
NFR15 (Scalability): MVP can support concurrent operation by two Lens developers without lifecycle corruption.
NFR16 (Scalability): Workflow design can scale to larger bug sets through repeatable batch execution and deterministic filtering by status New.

### Additional Requirements (Architecture)

- Conductor pattern: all commands follow 3-hop chain (.github/stub → release prompt → SKILL.md)
- SKILL.md authored via bmad-module-builder (BMB-first); release prompt via bmad-workflow-builder
- Bug storage: status-organized folders (bugs/New/, bugs/Inprogress/, bugs/Fixed/)
- featureId formula: `lens-dev-new-codebase-bugfix-{ms-timestamp}-{random4hex}` (immutable after generation)
- N bugs → 1 feature per batch; timestamp prevents collisions
- Three-commit lifecycle: feature-created commit (fix-all-new), →Inprogress commit (fix-all-new), →Fixed commit (--complete only)
- bugs/ is operational state written directly by scripts (no publish-to-governance for status moves); feature docs mirrors under features/ use publish-to-governance exclusively
- Explicit feature-index sync via publish-to-governance after feature creation (BF-3 workaround)
- All 3 commands (lens-bugbash, lens-bug-reporter, lens-bug-fixer) use same chain topology
- Scripts reside under `lens.core/_bmad/lens-work/scripts/`

### UX Design Requirements

N/A — no UI; command-line/prompt-invoked workflow only.

### FR Coverage Map

| FR | Epic | Story |
|----|------|-------|
| FR1–FR6 | Epic 1 | Story 1.1 (intake prompt + schema) |
| FR7–FR8 | Epic 1 | Story 1.2 (status schema enforcement) |
| FR9–FR12 | Epic 2 | Story 2.2 (status mutation logic) |
| FR13–FR18 | Epic 2 | Story 2.1 (fix discovery + feature creation) |
| FR19–FR24 | Epic 2 | Story 2.3 (batch execution + idempotency) |
| FR25–FR28 | Epic 1 | Story 1.3 (scope guard + governance write) |
| FR29–FR32 | Epic 3 | Story 3.1 (traceability + status inspection) |

---

## Epic List

1. **Epic 1: Bug Intake & Storage Foundation** — Bug reporter command, governance storage, schema enforcement, scope guard
2. **Epic 2: Batch Fix Orchestration** — Fix workflow, feature generation, expressplan execution, status mutations, completion path
3. **Epic 3: Observability & Conductor Wiring** — Main entry prompt, traceability, per-bug reporting, conductor chain assembly

---

## Epic 1: Bug Intake & Storage Foundation

**Goal:** Establish the intake command that captures one bug per run as a governance artifact with validated frontmatter, and enforce the scope guard that restricts all writes to new-codebase only.

### Story 1.1: Bug Reporter Intake Prompt & Artifact Creation

As a Lens developer,
I want to run a bug intake prompt that accepts title, description, and chat log input,
So that each bug is captured as a single canonical markdown artifact in the governance bugs folder.

**Acceptance Criteria:**

**Given** I run `/lens-bug-reporter`
**When** I provide title, description, and chat log
**Then** exactly one bug artifact is created at `governance_repo/bugs/New/{slug}.md`
**And** the artifact contains valid frontmatter with title, description, status=New, featureId=""

**Given** I run `/lens-bug-reporter`
**When** I omit a required frontmatter field (title, description, or chat log)
**Then** the workflow blocks artifact creation and prompts for correction

**Given** I run `/lens-bug-reporter` a second time with identical inputs
**When** a bug artifact with the same slug already exists
**Then** the workflow does not duplicate the artifact (idempotent rerun)

**Implementation Notes:**
- Create `.github/prompts/lens-bug-reporter.prompt.md` (stub)
- Create `lens.core/_bmad/lens-work/prompts/lens-bug-reporter.prompt.md` (release prompt via bmad-workflow-builder)
- Create `lens.core/_bmad/lens-work/skills/bmad-lens-bug-reporter/SKILL.md` (via bmad-module-builder)
- Create `lens.core/_bmad/lens-work/scripts/bug-reporter-ops.py` (artifact creation + slug generation)
- Bug slug derived from title: lowercase, hyphens, timestamp suffix for uniqueness

---

### Story 1.2: Bug Frontmatter Schema Enforcement

As a Lens developer,
I want the workflow to enforce the bug frontmatter schema strictly,
So that all bug artifacts are machine-parseable and consistent across intake and fix flows.

**Acceptance Criteria:**

**Given** a bug artifact is written by the intake workflow
**When** the artifact is read by any downstream workflow
**Then** frontmatter contains exactly: title (string), description (string), status (enum), featureId (string or empty)

**Given** a status value is set in any operation
**When** the value is not in [New, Inprogress, Fixed]
**Then** the operation is rejected with an explicit validation error

**Given** a status transition is attempted
**When** the transition is not New→Inprogress or Inprogress→Fixed
**Then** the operation is blocked and the prior valid status is preserved

**Implementation Notes:**
- Schema validation in `bug-reporter-ops.py` and `bug-fixer-ops.py`
- State machine enforced in Python; no silent coercion
- Invalid transitions logged as explicit errors in per-item outcome report

---

### Story 1.3: New-Codebase Scope Guard

As a Lens developer,
I want all reads and writes strictly scoped to new-codebase artifacts,
So that bugbash never mutates bugs, features, or governance records outside its authorized domain.

**Acceptance Criteria:**

**Given** any bugbash operation (intake or fix)
**When** a read or write path is resolved
**Then** a path guard validates the path is within `governance_repo/bugs/` or `governance_repo/features/lens-dev/new-codebase/`

**Given** a cross-scope path is detected
**When** the validation runs
**Then** the operation is blocked with an explicit scope violation error and no file is written

**Given** the scope guard is bypassed (simulated for testing)
**When** paths outside new-codebase are evaluated
**Then** test confirms zero mutations occurred outside authorized scope

**Implementation Notes:**
- Path guard function shared across `bug-reporter-ops.py` and `bug-fixer-ops.py`
- Hard-coded scope prefixes; no dynamic override at runtime
- Validated in unit tests via simulated cross-scope inputs

---

## Epic 2: Batch Fix Orchestration

**Goal:** Implement the full fix workflow — discovery of New bugs, N-bugs-to-1-feature batch mapping, expressplan execution, status transitions (New→Inprogress→Fixed), feature-index sync, and completion path.

### Story 2.1: Bug Discovery & Feature Generation

As a fix orchestrator,
I want to run fixbugs which discovers all New bugs and creates a single express-track feature for the batch,
So that all New bugs are routed into remediation in one run without manual per-bug setup.

**Acceptance Criteria:**

**Given** I run `/lens-bug-fixer`
**When** there are N bugs in `governance_repo/bugs/New/`
**Then** the workflow reads all N bug artifacts and groups them into one batch

**Given** a batch is formed
**When** feature generation runs
**Then** one feature is created with featureId = `lens-dev-new-codebase-bugfix-{timestamp}`
**And** the feature's team array is populated with [current_runner, backup_developer]

**Given** feature creation succeeds
**When** publish-to-governance is called
**Then** feature-index.yaml is updated with the new feature entry (explicit BF-3 workaround)

**Given** there are zero New bugs
**When** fixbugs is run
**Then** the workflow exits cleanly with a "no bugs to process" message and makes no mutations

**Implementation Notes:**
- Create `.github/prompts/lens-bug-fixer.prompt.md` (stub)
- Create `lens.core/_bmad/lens-work/prompts/lens-bug-fixer.prompt.md` (release prompt via bmad-workflow-builder)
- Create `lens.core/_bmad/lens-work/skills/bmad-lens-bug-fixer/SKILL.md` (via bmad-module-builder)
- Create `lens.core/_bmad/lens-work/scripts/bug-fixer-ops.py` (discovery, batch formation, feature gen)
- Delegates to bmad-lens-feature-yaml for feature.yaml creation
- Delegates to git-orchestration for branch creation (BF-1 workaround)

---

### Story 2.2: Status Mutations — New → Inprogress → Fixed

As a fix orchestrator,
I want bug status to transition atomically at phase boundaries with git-traceable commits,
So that governance state accurately reflects the current lifecycle phase for every bug.

**Acceptance Criteria:**

**Given** feature generation succeeds for a batch
**When** Phase 2 (transition to Inprogress) runs
**Then** each bug file is moved from `bugs/New/{slug}.md` to `bugs/Inprogress/{slug}.md`
**And** frontmatter is updated: status=Inprogress, featureId={generated featureId}
**And** a git commit is created: `[BUGBASH] Batch {timestamp} moved to Inprogress`

**Given** expressplan execution completes successfully for all bugs
**When** Phase 4 (transition to Fixed) runs
**Then** each bug file is moved from `bugs/Inprogress/{slug}.md` to `bugs/Fixed/{slug}.md`
**And** frontmatter is updated: status=Fixed
**And** a git commit is created: `[BUGBASH] Batch {timestamp} completed (featureId {featureId})`

**Given** a failure occurs during Phase 2 (feature creation) — before the Inprogress commit
**When** the error is detected
**Then** all bugs remain in New (no bugs have been moved yet)
**And** no commits are made for this batch
**And** an explicit error report is written

**Given** a per-bug failure occurs during Phase 3 processing — after the feature-created commit
**When** the per-bug error is detected
**Then** the failed bug remains in New; successfully moved bugs remain in Inprogress
**And** an explicit per-bug error report identifies each failed bug and its error

**Implementation Notes:**
- File moves are atomic (write new, verify, delete old) in `bug-fixer-ops.py`
- Commits use exact message template from architecture
- Status=Inprogress is queryable; supports manual recovery if crash between phases

---

### Story 2.3: Expressplan Execution & Batch Idempotency

As a fix orchestrator,
I want expressplan to execute against each bug's content and reruns to be safe,
So that bugs are planned with their full incident context and no duplicates are created.

**Acceptance Criteria:**

**Given** Phase 3 (expressplan) runs for a batch
**When** expressplan is invoked
**Then** it receives each bug's description + chat log as planning input
**And** all phase artifacts are generated (businessplan, techplan, finalizeplan, expressplan outputs)
**And** sprint-status.yaml is created and valid

**Given** a fixbugs run is re-executed after prior completion
**When** the workflow scans bugs/New/
**Then** previously-processed bugs (Inprogress or Fixed) are not re-discovered
**And** no duplicate feature is created for already-processed bugs

**Given** one bug fails during expressplan
**When** the batch-level error handler runs
**Then** the failed bug remains in Inprogress
**And** the outcome report identifies the failure with the bug slug and error detail
**And** all other bugs in the batch are unaffected (per-item isolation)

**Implementation Notes:**
- Delegates expressplan execution to bmad-lens-expressplan skill (conductor delegation)
- Per-item outcome log written at end of each batch run
- Idempotency enforced by checking status before processing: only status=New is eligible

---

### Story 2.4: Completion Path — Inprogress → Fixed

As a fix orchestrator,
I want to run a completion update that marks linked bugs as Fixed after fix work is done,
So that governance reflects actual completion state and the bug queue stays accurate.

**Acceptance Criteria:**

**Given** I run `/lens-bug-fixer --complete {featureId}`
**When** the workflow resolves bug artifacts linked to {featureId}
**Then** each linked bug's status is updated from Inprogress to Fixed
**And** a git commit is created: `[BUGBASH] Batch {featureId} completed`

**Given** a bug cannot be resolved by featureId
**When** the resolution lookup fails
**Then** the workflow blocks the Fixed promotion and reports the unresolved bug explicitly

**Given** the completion command is run twice for the same featureId
**When** bugs are already Fixed
**Then** the workflow is idempotent and makes no changes (no double-commit)

**Implementation Notes:**
- Completion flag in `bug-fixer-ops.py`: `--complete {featureId}`
- featureId lookup scans Inprogress/ for matching frontmatter field
- Guard: status must be Inprogress before promoting to Fixed

---

## Epic 3: Observability & Conductor Wiring

**Goal:** Wire the main `lens-bugbash` entry conductor, enable per-bug outcome reporting, and ensure the full 3-hop conductor chain is correctly assembled for all three commands.

### Story 3.1: Main Entry Conductor (lens-bugbash)

As a Lens developer,
I want a single `/lens-bugbash` entry point that routes to intake or fix based on flags,
So that I have a unified command surface and do not need to remember separate command names.

**Acceptance Criteria:**

**Given** I run `/lens-bugbash --report`
**When** the conductor evaluates the flag
**Then** it delegates to the bug-reporter workflow

**Given** I run `/lens-bugbash --fix-all-new`
**When** the conductor evaluates the flag
**Then** it delegates to the bug-fixer batch workflow

**Given** I run `/lens-bugbash` with no flags
**When** the conductor runs
**Then** it displays a help menu listing available flags and their descriptions

**Given** I run `/lens-bugbash --status`
**When** the conductor runs
**Then** it prints a summary: count of bugs in each status (New, Inprogress, Fixed)

**Implementation Notes:**
- Create `.github/prompts/lens-bugbash.prompt.md` (stub, runs light-preflight.py)
- Create `lens.core/_bmad/lens-work/prompts/lens-bugbash.prompt.md` (release prompt via bmad-workflow-builder)
- Create `lens.core/_bmad/lens-work/skills/bmad-lens-bugbash/SKILL.md` (via bmad-module-builder)
- Create `lens.core/_bmad/lens-work/scripts/bugbash-ops.py` (status query + routing)
- Delegates to bug-reporter or bug-fixer skill by flag

---

### Story 3.2: Per-Bug Outcome Reporting

As a Lens developer,
I want a per-bug outcome report after every batch run,
So that I can identify which bugs succeeded, which failed, and why without reading git history.

**Acceptance Criteria:**

**Given** a fixbugs batch run completes (success or partial failure)
**When** the run finishes
**Then** a report is printed to the terminal listing each bug slug with its outcome (success/failure)
**And** failed bugs include the error detail (exception type + message)
**And** the report includes totals: N succeeded, M failed

**Given** a batch run where all bugs succeed
**When** the report is printed
**Then** all bugs show status=Inprogress with their assigned featureId

**Given** a batch run where zero bugs are eligible
**When** the report is printed
**Then** the report shows "0 bugs processed" with a clear explanation

**Implementation Notes:**
- Report generated from per-item outcome list in `bug-fixer-ops.py`
- Report format: plain text table; no external dependencies
- Errors include bug slug, attempted operation, exception message

---

### Story 3.3: Conductor Chain Validation (Integration Test)

As a Lens developer,
I want integration tests that verify the full 3-hop conductor chain for all three commands,
So that I can confirm stub → release prompt → SKILL.md wiring is correct before deployment.

**Acceptance Criteria:**

**Given** the stub `.github/prompts/lens-bugbash.prompt.md` exists
**When** a lint/scan validates the file
**Then** it invokes `light-preflight.py` and redirects to the release prompt path

**Given** the release prompt `lens-work/prompts/lens-bugbash.prompt.md` exists
**When** scanned
**Then** it correctly references `skills/bmad-lens-bugbash/SKILL.md`

**Given** all three commands (lens-bugbash, lens-bug-reporter, lens-bug-fixer) are assembled
**When** a scan-path-standards check runs
**Then** all stub/release/skill paths resolve without broken references

**Given** `bugbash-ops.py`, `bug-reporter-ops.py`, and `bug-fixer-ops.py` exist
**When** a scan-scripts check runs
**Then** all scripts execute `uv run` cleanly and accept `--help` without error

**Implementation Notes:**
- Reuse existing `scan-path-standards` and `scan-scripts` lint gates (established pattern from new-codebase features)
- No new test infrastructure required
- Tests run as part of story completion gate before PR merge
