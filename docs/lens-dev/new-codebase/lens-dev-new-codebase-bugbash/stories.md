---
feature: lens-dev-new-codebase-bugbash
doc_type: stories
status: draft
goal: "Dev-ready story list for bugbash: 3 commands, 10 stories, 3 sprints."
key_decisions:
  - Implementation order follows dependency-safe sequencing from tech-plan Section 9
  - Sprint 1 lays the storage/schema foundation (1.3 → 1.2 → 1.1)
  - Sprint 2 builds the batch fix orchestration on top of Sprint 1
  - Sprint 3 wires the conductor and validates the full chain
  - All scripts authored directly (Python); SKILL.md via bmad-module-builder; release prompts via bmad-workflow-builder
open_questions: []
depends_on:
  - prd.md
  - architecture.md
  - epics.md
  - tech-plan.md
blocks: []
updated_at: 2026-05-03T23:45:00Z
---

# Stories — Bugbash

## Sprint 1: Bug Intake & Storage Foundation

### Story 1.3 — New-Codebase Scope Guard

**Epic:** Epic 1 — Bug Intake & Storage Foundation
**Priority:** High
**Size:** S
**Sprint:** 1

As a Lens developer, I want all reads and writes strictly scoped to new-codebase artifacts,
so that bugbash never mutates bugs, features, or governance records outside its authorized domain.

**Acceptance Criteria:**
- [ ] Path guard validates any read/write path is within `governance_repo/bugs/` or `governance_repo/features/lens-dev/new-codebase/`
- [ ] Cross-scope path is blocked with explicit scope violation error; no file is written
- [ ] Scope guard is shared across `bug-reporter-ops.py` and `bug-fixer-ops.py`
- [ ] Hard-coded scope prefixes; no dynamic override at runtime
- [ ] Unit tests confirm zero mutations occurred outside authorized scope on simulated bypass

**Implementation Notes:**
- Shared path guard function in `bug-reporter-ops.py` and `bug-fixer-ops.py`
- Covered by regression tests in 7.2 Scope Guard

---

### Story 1.2 — Bug Frontmatter Schema Enforcement

**Epic:** Epic 1 — Bug Intake & Storage Foundation
**Priority:** High
**Size:** S
**Sprint:** 1

As a Lens developer, I want the workflow to enforce the bug frontmatter schema strictly,
so that all bug artifacts are machine-parseable and consistent across intake and fix flows.

**Acceptance Criteria:**
- [ ] Frontmatter contains exactly: title (string), description (string), status (enum), featureId (string or empty), slug, created_at, updated_at
- [ ] Status values restricted to `New`, `Inprogress`, `Fixed` — invalid values rejected with explicit error
- [ ] Invalid transitions (New→Fixed, Fixed→any, Inprogress→New) are blocked; prior valid status is preserved
- [ ] Schema validation in both `bug-reporter-ops.py` and `bug-fixer-ops.py`
- [ ] No silent coercion; all violations logged as explicit errors

**Implementation Notes:**
- State machine enforced in Python
- Covered by regression tests in 7.1 Schema Validation

---

### Story 1.1 — Bug Reporter Intake Prompt & Artifact Creation

**Epic:** Epic 1 — Bug Intake & Storage Foundation
**Priority:** High
**Size:** M
**Sprint:** 1

As a Lens developer, I want to run a bug intake prompt that accepts title, description, and chat log input,
so that each bug is captured as a single canonical markdown artifact in the governance bugs folder.

**Acceptance Criteria:**
- [ ] `/lens-bug-reporter` creates exactly one artifact at `governance_repo/bugs/New/{slug}.md`
- [ ] Artifact contains valid frontmatter: title, description, status=New, featureId="", slug, created_at, updated_at
- [ ] Omitting required fields (title, description, chat log) blocks creation and prompts for correction
- [ ] Idempotent: re-run with identical inputs does not duplicate artifact (content-hash slug key)
- [ ] `.github/prompts/lens-bug-reporter.prompt.md` stub exists and invokes light-preflight.py
- [ ] `lens.core/_bmad/lens-work/prompts/lens-bug-reporter.prompt.md` release prompt delegates to SKILL.md
- [ ] `lens.core/_bmad/lens-work/skills/bmad-lens-bug-reporter/SKILL.md` exists (via bmad-module-builder)
- [ ] `lens.core/_bmad/lens-work/scripts/bug-reporter-ops.py` create-bug command works end-to-end
- [ ] Missing parent directories (`bugs/New/`) are created or a clear initialization error is reported (A4)
- [ ] Slug collision hardening: content-hash + millisecond-precision or UUID fallback prevents concurrent duplicates (A5)

**Implementation Notes:**
- Slug = `{title-slug}-{sha256(title+description)[:8]}` — stable across reruns
- `bug-reporter-ops.py create-bug` returns JSON: `{ "slug": str, "path": str, "status": "created" | "duplicate" }`
- Exit codes: 0=success, 1=validation failure, 2=scope violation, 3=write error

---

## Sprint 2: Batch Fix Orchestration

### Story 2.1 — Bug Discovery & Feature Generation

**Epic:** Epic 2 — Batch Fix Orchestration
**Priority:** High
**Size:** L
**Sprint:** 2

As a fix orchestrator, I want to run fixbugs which discovers all New bugs and creates a single express-track feature for the batch,
so that all New bugs are routed into remediation in one run without manual per-bug setup.

**Acceptance Criteria:**
- [ ] `bug-fixer-ops.py discover-new` reads all artifacts in `governance_repo/bugs/New/` and returns slug + title list
- [ ] When 0 bugs found: workflow exits cleanly with "0 bugs to process. Queue is clean." message and makes no mutations (A6)
- [ ] When N bugs found: batch formed; featureId generated as `lens-dev-new-codebase-bugfix-{ms-timestamp}-{random4hex}`
- [ ] Feature created via `init-feature-ops.py create` with track=express; team includes current_runner
- [ ] feature-index.yaml is updated natively by init-feature-ops.py (no separate BF-3 CLI call needed)
- [ ] Feature creation failure: all bugs remain in New; no mutations; explicit error report
- [ ] Story 2.1 AC uses init-feature-ops.py native feature-index sync (NOT `--update-feature-index` flag — confirmed absent as of 2026-05-03) (A3)

**Implementation Notes:**
- Delegates to bmad-lens-init-feature as a separate skill step; `bug-fixer-ops.py` does not call init-feature-ops.py directly
- Verify available init-feature-ops.py flags before implementation

---

### Story 2.2 — Status Mutations — New → Inprogress → Fixed

**Epic:** Epic 2 — Batch Fix Orchestration
**Priority:** High
**Size:** M
**Sprint:** 2

As a fix orchestrator, I want bug status to transition atomically at phase boundaries with git-traceable commits,
so that governance state accurately reflects the current lifecycle phase for every bug.

**Acceptance Criteria:**
- [ ] After feature creation succeeds: each bug moved from `bugs/New/{slug}.md` to `bugs/Inprogress/{slug}.md` with status=Inprogress, featureId set
- [ ] Git commit created: `[BUGBASH] Batch {featureId} moved to Inprogress`
- [ ] After expressplan completes (via --complete): each linked bug moved from Inprogress to Fixed; status=Fixed
- [ ] Git commit created: `[BUGBASH] Batch {featureId} completed`
- [ ] Phase 2 failure (before Inprogress commit): all bugs remain in New; no commits
- [ ] Per-bug failure during Phase 3: failed bug remains in New; successfully moved bugs proceed; explicit per-bug error report

**Implementation Notes:**
- File moves are atomic in `bug-fixer-ops.py` (write new, verify, delete old)
- Commit messages follow exact template from architecture

---

### Story 2.3 — Expressplan Execution & Batch Idempotency

**Epic:** Epic 2 — Batch Fix Orchestration
**Priority:** High
**Size:** M
**Sprint:** 2

As a fix orchestrator, I want expressplan to execute against each bug's content and reruns to be safe,
so that bugs are planned with their full incident context and no duplicates are created.

**Acceptance Criteria:**
- [ ] Expressplan receives each bug's description + chat log as planning input
- [ ] All phase artifacts generated (businessplan, techplan, finalizeplan, expressplan outputs); sprint-status.yaml created and valid
- [ ] Re-run after prior completion: previously-processed bugs (Inprogress or Fixed) not re-discovered by discover-new
- [ ] No duplicate feature created for already-processed bugs
- [ ] One bug fails during expressplan: failed bug remains Inprogress; other bugs unaffected; outcome report identifies failure

**Implementation Notes:**
- Delegates expressplan execution to bmad-lens-expressplan skill (conductor delegation)
- Per-item outcome log written at end of each batch run
- Idempotency enforced by status check: only status=New is eligible for discover-new

---

### Story 2.4 — Completion Path — Inprogress → Fixed

**Epic:** Epic 2 — Batch Fix Orchestration
**Priority:** High
**Size:** S
**Sprint:** 2

As a fix orchestrator, I want to run a completion update that marks linked bugs as Fixed after fix work is done,
so that governance reflects actual completion state and the bug queue stays accurate.

**Acceptance Criteria:**
- [ ] `/lens-bug-fixer --complete {featureId}` resolves bug artifacts linked to featureId
- [ ] Each linked bug's status updated from Inprogress to Fixed with git commit: `[BUGBASH] Batch {featureId} completed`
- [ ] Bug not resolvable by featureId: Fixed promotion is blocked; explicit error report
- [ ] Idempotent: running completion twice for same featureId makes no changes (no double-commit)

**Implementation Notes:**
- `bug-fixer-ops.py resolve-bugs --feature-id {featureId}` scans Inprogress/ for matching frontmatter.featureId
- Guard: status must be Inprogress before promoting to Fixed

---

## Sprint 3: Observability & Conductor Wiring

### Story 3.1 — Main Entry Conductor (lens-bugbash)

**Epic:** Epic 3 — Observability & Conductor Wiring
**Priority:** Medium
**Size:** M
**Sprint:** 3

As a Lens developer, I want a single `/lens-bugbash` entry point that routes to intake or fix based on flags,
so that I have a unified command surface and do not need to remember separate command names.

**Acceptance Criteria:**
- [ ] `/lens-bugbash --report` delegates to bug-reporter workflow
- [ ] `/lens-bugbash --fix-all-new` delegates to bug-fixer batch workflow
- [ ] `/lens-bugbash` with no flags displays a help menu listing available flags and descriptions
- [ ] `/lens-bugbash --status` prints: count of bugs in each status (New, Inprogress, Fixed)
- [ ] `.github/prompts/lens-bugbash.prompt.md` stub exists and invokes light-preflight.py
- [ ] `lens.core/_bmad/lens-work/prompts/lens-bugbash.prompt.md` release prompt exists
- [ ] `lens.core/_bmad/lens-work/skills/bmad-lens-bugbash/SKILL.md` exists (via bmad-module-builder)
- [ ] `lens.core/_bmad/lens-work/scripts/bugbash-ops.py` status-summary command works

**Implementation Notes:**
- `bugbash-ops.py status-summary` returns JSON: `{ "New": int, "Inprogress": int, "Fixed": int }`

---

### Story 3.2 — Per-Bug Outcome Reporting

**Epic:** Epic 3 — Observability & Conductor Wiring
**Priority:** Medium
**Size:** S
**Sprint:** 3

As a Lens developer, I want a per-bug outcome report after every batch run,
so that I can identify which bugs succeeded, which failed, and why without reading git history.

**Acceptance Criteria:**
- [ ] After fixbugs batch run (success or partial failure): terminal output lists each bug slug with outcome (success/failure)
- [ ] Failed bugs include error detail (exception type + message)
- [ ] Report includes totals: N succeeded, M failed
- [ ] Report is printed regardless of full-success or partial-failure outcome

---

### Story 3.3 — Chain Validation & Regression Tests

**Epic:** Epic 3 — Observability & Conductor Wiring
**Priority:** Medium
**Size:** S
**Sprint:** 3

As a Lens contributor, I want all three conductor chains verified and regression tests passing,
so that all commands are behaviorally correct and covered before the feature is marked dev-complete.

**Acceptance Criteria:**
- [ ] `scan-path-standards` passes for all 3 commands (lens-bugbash, lens-bug-reporter, lens-bug-fixer)
- [ ] `scan-scripts` confirms all 3 scripts accept `--help` cleanly
- [ ] Schema validation tests pass: intake with all fields, missing field rejection, invalid status rejection, invalid transition blocking
- [ ] Scope guard tests pass: authorized path PASS, cross-scope path blocked
- [ ] Batch idempotency tests pass: re-run discover-new after Inprogress shows 0 bugs; duplicate create-bug returns "duplicate"
- [ ] governance-repo path mismatch startup validation test passes (A7)
- [ ] All regression categories from tech-plan Section 7 covered and passing

**Implementation Notes:**
- A7: Add startup validation in all three scripts — if governance_repo path does not exist, exit 1 with clear config error before any file operations
