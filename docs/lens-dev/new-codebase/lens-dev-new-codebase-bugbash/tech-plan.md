---
feature: lens-dev-new-codebase-bugbash
doc_type: tech-plan
status: draft
goal: "Define the technical design for bugbash: conductor pattern wiring, bug artifact schema, status state machine, batch fix orchestration, and BMB-first authoring protocol."
key_decisions:
  - Three commands (lens-bugbash, lens-bug-reporter, lens-bug-fixer) each follow the invariant 3-hop conductor chain
  - SKILL.md authored via bmad-module-builder (BMB-first); release prompts via bmad-workflow-builder
  - Bug storage: status-organized folders (bugs/New/, bugs/Inprogress/, bugs/Fixed/) in governance repo
  - N bugs → 1 feature per batch; featureId = lens-dev-new-codebase-bugfix-{timestamp}
  - Two-commit model: Phase 2 (→Inprogress commit) and Phase 4 (→Fixed commit)
  - publish-to-governance is the sole governance write path; no direct file mutations in governance repo
  - Explicit feature-index sync after feature creation (BF-3 workaround)
  - Per-item failure isolation: failed bugs remain in prior valid state with explicit error reporting
open_questions: []
depends_on:
  - prd.md (this feature)
  - architecture.md (this feature)
  - epics.md (this feature)
  - lens-dev-new-codebase-baseline architecture.md
blocks: []
updated_at: 2026-05-03T00:00:00Z
---

# Technical Plan — Bugbash

**Feature:** lens-dev-new-codebase-bugbash
**Author:** crisweber2600
**Date:** 2026-05-03
**References:** [PRD](./prd.md), [Architecture](./architecture.md), [Epics](./epics.md), [Baseline Architecture](../../lens-dev-new-codebase-baseline/docs/architecture.md)

---

## 1. System Design

All three bugbash commands follow the invariant 3-hop command resolution chain:

```
.github/prompts/lens-{command}.prompt.md          (stub — user entry point)
  → lens.core/_bmad/lens-work/prompts/lens-{command}.prompt.md  (release prompt)
    → skills/bmad-lens-{command}/SKILL.md          (owning skill — thin conductor)
      → shared utilities + scripts/{command}-ops.py
```

Each SKILL.md is a **thin conductor** — it orchestrates scripts and shared utilities by delegation; it does not implement business logic inline.

### 1.1 Commands

| Command | Entry Flag | Conductor Skill | Script |
|---------|-----------|-----------------|--------|
| `lens-bugbash` | `--report`, `--fix-all-new`, `--complete`, `--status`, none | `bmad-lens-bugbash` | `bugbash-ops.py` |
| `lens-bug-reporter` | (none — single intake run) | `bmad-lens-bug-reporter` | `bug-reporter-ops.py` |
| `lens-bug-fixer` | `--fix-all-new`, `--complete {featureId}` | `bmad-lens-bug-fixer` | `bug-fixer-ops.py` |

---

## 2. Bug Artifact Schema

### 2.1 Frontmatter Schema

Every bug artifact is a markdown file with this frontmatter:

```yaml
---
title: "Short descriptive title of the bug"
description: "Concise description of what went wrong and expected behavior"
status: New          # enum: New | Inprogress | Fixed
featureId: ""        # empty at intake; populated on fix kickoff
slug: "descriptive-slug-20260503-120000"
created_at: 2026-05-03T12:00:00Z
updated_at: 2026-05-03T12:00:00Z
---
```

**Constraints:**
- `title`: required, non-empty string
- `description`: required, non-empty string
- `status`: required, must be one of `New`, `Inprogress`, `Fixed` — no other values accepted
- `featureId`: required field (empty string at intake; set at fix kickoff; must be set before status→Inprogress promotion)
- `slug`: derived from title (lowercase, hyphens, timestamp suffix for uniqueness)
- Chat log content follows the frontmatter as markdown body — preserved verbatim

### 2.2 Storage Paths

```
governance_repo/bugs/
  New/
    {slug}.md          ← intake destination; eligible for fix discovery
  Inprogress/
    {slug}.md          ← moved here on fix kickoff; featureId populated
  Fixed/
    {slug}.md          ← moved here on fix completion
```

**Scope guard:** All paths must be within `governance_repo/bugs/` or `governance_repo/features/lens-dev/new-codebase/`. Any other path is rejected with a scope violation error before any file operation.

### 2.3 Status State Machine

```
[created]
    │
    ▼
  New ──── fix kickoff + featureId set ────► Inprogress
                                                 │
                                        fix complete ───► Fixed
```

**Allowed transitions:**
- `intake` → sets `New` (new artifact, no prior state)
- `fix kickoff` → `New` → `Inprogress` (requires featureId assigned first)
- `fix completion` → `Inprogress` → `Fixed` (requires featureId resolves to completed feature)

**Forbidden transitions (hard-blocked):**
- `New` → `Fixed` (must pass through Inprogress)
- `Fixed` → any state (terminal; no re-opening)
- `Inprogress` → `New` (no rollback; manual recovery only)

---

## 3. Conductor Flow Designs

### 3.1 lens-bug-reporter Flow

```
[entry: /lens-bug-reporter]
  → run light-preflight.py (exit non-zero → stop)
  → load release prompt: lens-work/prompts/lens-bug-reporter.prompt.md
  → load SKILL.md: skills/bmad-lens-bug-reporter/SKILL.md
  → prompt user for: title, description, chat log
  → validate required fields (reject if any missing)
  → call: bug-reporter-ops.py create-bug
      --title "{title}"
      --description "{description}"
      --chat-log "{chat_log}"
      --governance-repo {governance_repo}
  → script: validate scope, generate slug, write artifact to bugs/New/{slug}.md
  → report: artifact path + slug to user
[done]
```

### 3.2 lens-bug-fixer --fix-all-new Flow

```
[entry: /lens-bug-fixer --fix-all-new]
  → run light-preflight.py (exit non-zero → stop)
  → load release prompt → load SKILL.md
  → call: bug-fixer-ops.py discover-new
      --governance-repo {governance_repo}
  → if 0 bugs: report "no bugs to process", exit 0

Phase 1: Batch Formation
  → group all New bugs into one batch
  → generate featureId = lens-dev-new-codebase-bugfix-{timestamp}

Phase 2: Status → Inprogress
  → for each bug: move bugs/New/{slug}.md → bugs/Inprogress/{slug}.md
  → update frontmatter: status=Inprogress, featureId={featureId}
  → git commit: "[BUGBASH] Batch {timestamp} moved to Inprogress"

Phase 3: Feature & Expressplan
  → delegate to bmad-lens-feature-yaml: create feature.yaml
      featureId = lens-dev-new-codebase-bugfix-{timestamp}
      team = [current_runner, backup_developer]
  → call: publish-to-governance --update-feature-index (BF-3 workaround)
  → git commit: "[BUGBASH] Batch {timestamp} feature created"
  → delegate to bmad-lens-expressplan: run expressplan on feature
      planning input = concatenated bug descriptions + chat logs

Phase 4: Status → Fixed (after expressplan completes)
  → for each bug: move bugs/Inprogress/{slug}.md → bugs/Fixed/{slug}.md
  → update frontmatter: status=Fixed
  → git commit: "[BUGBASH] Batch {timestamp} completed (featureId {featureId})"

  → print per-bug outcome report
[done]
```

**Error handling (per-item isolation):**
- If any bug fails during Phase 2 or 3: that bug remains in its current state; error is recorded in per-item report
- If expressplan fails: all bugs remain Inprogress; full batch retry required
- No partial commit: Phase 2 commit only lands after all file moves succeed

### 3.3 lens-bug-fixer --complete Flow

```
[entry: /lens-bug-fixer --complete {featureId}]
  → load SKILL.md
  → call: bug-fixer-ops.py resolve-bugs --feature-id {featureId}
  → scan bugs/Inprogress/ for artifacts where frontmatter.featureId == {featureId}
  → if none found: report unresolved, exit with error
  → for each matched bug: move to bugs/Fixed/{slug}.md, update status=Fixed
  → git commit: "[BUGBASH] Batch {featureId} completed"
  → idempotency: if bugs already Fixed, no-op and report "already fixed"
[done]
```

### 3.4 lens-bugbash (Main Entry) Flow

```
[entry: /lens-bugbash {flag}]
  → run light-preflight.py
  → load SKILL.md: skills/bmad-lens-bugbash/SKILL.md
  → evaluate flag:
      --report        → delegate to bmad-lens-bug-reporter
      --fix-all-new   → delegate to bmad-lens-bug-fixer --fix-all-new
      --complete {id} → delegate to bmad-lens-bug-fixer --complete {id}
      --status        → call bugbash-ops.py status-summary
                        print: count of bugs in New, Inprogress, Fixed
      (no flags)      → print help menu with all flags and descriptions
[done]
```

---

## 4. Files to Create

### 4.1 Stubs (`.github/prompts/`)

| File | Action | Channel |
|------|--------|---------|
| `.github/prompts/lens-bugbash.prompt.md` | Create | Direct (established agent-creates pattern) |
| `.github/prompts/lens-bug-reporter.prompt.md` | Create | Direct |
| `.github/prompts/lens-bug-fixer.prompt.md` | Create | Direct |

All stubs follow the invariant pattern:
```
Run: uv run {module_path}/_bmad/lens-work/scripts/light-preflight.py
Exit non-zero → stop.
Load: {module_path}/_bmad/lens-work/prompts/lens-{command}.prompt.md
```

### 4.2 Release Prompts (`lens-work/prompts/`)

| File | Action | Channel |
|------|--------|---------|
| `lens.core/_bmad/lens-work/prompts/lens-bugbash.prompt.md` | Create | `bmad-workflow-builder` |
| `lens.core/_bmad/lens-work/prompts/lens-bug-reporter.prompt.md` | Create | `bmad-workflow-builder` |
| `lens.core/_bmad/lens-work/prompts/lens-bug-fixer.prompt.md` | Create | `bmad-workflow-builder` |

### 4.3 Skill Files (`lens-work/skills/`)

| File | Action | Channel |
|------|--------|---------|
| `lens.core/_bmad/lens-work/skills/bmad-lens-bugbash/SKILL.md` | Create | `bmad-module-builder` (BMB-first) |
| `lens.core/_bmad/lens-work/skills/bmad-lens-bug-reporter/SKILL.md` | Create | `bmad-module-builder` (BMB-first) |
| `lens.core/_bmad/lens-work/skills/bmad-lens-bug-fixer/SKILL.md` | Create | `bmad-module-builder` (BMB-first) |

### 4.4 Scripts (`lens-work/scripts/`)

| File | Action | Channel |
|------|--------|---------|
| `lens.core/_bmad/lens-work/scripts/bugbash-ops.py` | Create | `bmad-module-builder` (BMB-first) |
| `lens.core/_bmad/lens-work/scripts/bug-reporter-ops.py` | Create | `bmad-module-builder` (BMB-first) |
| `lens.core/_bmad/lens-work/scripts/bug-fixer-ops.py` | Create | `bmad-module-builder` (BMB-first) |

### 4.5 Shared Dependencies (read-only — not modified in this feature)

| Dependency | Why Referenced |
|-----------|----------------|
| `scripts/light-preflight.py` | Entry gate for all stubs |
| `scripts/git-orchestration-ops.py` | Branch creation (BF-1), publish-to-governance |
| `skills/bmad-lens-feature-yaml/` | feature.yaml creation delegation |
| `skills/bmad-lens-expressplan/` | Expressplan execution delegation |
| `lifecycle.yaml` | Phase contracts |

---

## 5. Script Design Contracts

### 5.1 bug-reporter-ops.py

**Commands:**

```
bug-reporter-ops.py create-bug
  --title STR
  --description STR
  --chat-log STR
  --governance-repo PATH

  Output (JSON):
    { "slug": str, "path": str, "status": "created" | "duplicate" }
  Exit codes: 0=success, 1=validation failure, 2=scope violation, 3=write error
```

> **Write authority:** `bug-reporter-ops.py` writes **directly** to `governance_repo/bugs/New/{slug}.md`. The `bugs/` folder is operational state — not a feature docs mirror — and does not go through `publish-to-governance`. Status mutations (file moves between `bugs/New/`, `bugs/Inprogress/`, `bugs/Fixed/`) are likewise direct file operations; a publish-CLI layer would make atomic moves impossible. Feature docs mirrors under `features/` continue to use `publish-to-governance` exclusively.

**Internal logic:**
1. Validate required fields (non-empty)
2. Scope guard: assert governance_repo prefix
3. Generate slug: `{title-slug}-{YYYYMMDD-HHMMSS}`
4. Check idempotency: if `bugs/New/{slug}.md` exists → return `"status": "duplicate"`, exit 0
5. Build frontmatter + body
6. Write to `bugs/New/{slug}.md`
7. Return JSON result

### 5.2 bug-fixer-ops.py

**Commands:**

```
bug-fixer-ops.py discover-new
  --governance-repo PATH

  Output (JSON):
    { "bugs": [{ "slug": str, "path": str, "title": str }], "count": int }

bug-fixer-ops.py move-to-inprogress
  --governance-repo PATH
  --slugs [STR, ...]
  --feature-id STR

  Output (JSON):
    { "moved": [str], "failed": [{ "slug": str, "error": str }] }

bug-fixer-ops.py move-to-fixed
  --governance-repo PATH
  --slugs [STR, ...]

  Output (JSON):
    { "moved": [str], "failed": [{ "slug": str, "error": str }] }

bug-fixer-ops.py resolve-bugs
  --governance-repo PATH
  --feature-id STR

  Output (JSON):
    { "resolved": [str], "not_found": [str], "already_fixed": [str] }
```

**All commands:**
- Validate scope before any file operation
- Return structured JSON for SKILL.md to consume
- Never silently skip; always report per-item outcome
- Idempotent: re-running on already-processed slugs is safe

### 5.3 bugbash-ops.py

**Commands:**

```
bugbash-ops.py status-summary
  --governance-repo PATH

  Output (JSON):
    { "New": int, "Inprogress": int, "Fixed": int }
```

---

## 6. SKILL.md Design Contract

Each SKILL.md follows this structural template (per baseline architecture):

```
## Overview        — 2-3 lines: what the command does, scope constraint
## Identity        — 1 paragraph: conductor role, what it does NOT do
## Communication Style — 4-6 bullets
## Principles      — 6-8 bullets: scope-guard-first, no-inline-logic, etc.
## On Activation   — ordered steps: light-preflight → load config → evaluate flag →
                     delegate to script → report result
## Artifacts       — table: artifact name, description, producing agent
## Required Frontmatter — yaml block
## Integration Points — table of delegation targets
```

---

## 7. Regression Coverage

Three categories required before merge:

### 7.1 Schema Validation

| Test | Expected |
|------|----------|
| Intake with all required fields | Artifact created; status=New; featureId="" |
| Intake missing title | Rejected; no file written |
| Intake missing description | Rejected; no file written |
| Status set to invalid value | Operation rejected; prior state preserved |
| Invalid transition (New→Fixed) | Blocked; explicit error |

### 7.2 Scope Guard

| Test | Expected |
|------|----------|
| Write to bugs/New/ within governance_repo | PASS |
| Write to path outside governance_repo | Blocked; scope violation error |
| Write to governance_repo/features/lens-dev/old-codebase/ | Blocked |
| Write to governance_repo/features/lens-dev/new-codebase/ | PASS |

### 7.3 Conductor Chain

| Test | Expected |
|------|----------|
| stub invokes light-preflight.py before redirect | PASS |
| stub redirects to correct release prompt path | PASS |
| release prompt loads correct SKILL.md | PASS |
| scan-path-standards passes for all 3 commands | PASS |
| scan-scripts: all 3 scripts accept --help cleanly | PASS |

### 7.4 Batch Idempotency

| Test | Expected |
|------|----------|
| Re-run discover-new after bugs moved to Inprogress | 0 bugs discovered |
| Re-run create-bug with same title | "duplicate" result; no second file written |
| Re-run resolve-bugs for already-Fixed bugs | "already_fixed" result; no error |

---

## 8. BMB-First Authoring Protocol

Per the `lens-dev/new-codebase` service constitution:

> Anytime `lens.core.src` is being modified, SKILL.md updates must be authored through `.github/skills/bmad-module-builder` and release prompt or workflow artifacts must be authored through `.github/skills/bmad-workflow-builder`.

**Implementation sequence per story:**
1. Author scripts (`bug-reporter-ops.py`, `bug-fixer-ops.py`, `bugbash-ops.py`) directly (Python files, not prompts/skills)
2. Use `bmad-module-builder` to generate each SKILL.md
3. Use `bmad-workflow-builder` to generate each release prompt
4. Verify stub chain integrity (light-preflight.py invoked; correct release prompt path)
5. Run regression coverage gates
6. Commit to feature branch in control repo

---

## 9. Story Implementation Order

Stories are sequenced so each builds on the prior without forward dependencies:

| Sprint | Story | Deliverable |
|--------|-------|-------------|
| 1 | 1.3 Scope Guard | Path validation shared utility — foundation for all writes |
| 1 | 1.2 Schema Enforcement | Frontmatter validator + state machine — foundation for intake and fix |
| 1 | 1.1 Intake Prompt | bug-reporter-ops.py + SKILL.md + prompts — first working command |
| 2 | 2.1 Discovery & Feature Gen | bug-fixer-ops.py discover-new + feature generation |
| 2 | 2.2 Status Mutations | move-to-inprogress + move-to-fixed + two-commit model |
| 2 | 2.3 Expressplan & Idempotency | expressplan delegation + idempotency gates |
| 2 | 2.4 Completion Path | resolve-bugs + --complete flag |
| 3 | 3.1 Main Entry Conductor | lens-bugbash routing + status-summary |
| 3 | 3.2 Per-Bug Reporting | Outcome report format + terminal output |
| 3 | 3.3 Chain Validation | scan-path-standards + scan-scripts integration tests |
