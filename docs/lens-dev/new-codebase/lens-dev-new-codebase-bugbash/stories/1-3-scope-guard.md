---
story_id: "1.3"
epic: "Epic 1"
feature: lens-dev-new-codebase-bugbash
title: New-Codebase Scope Guard
priority: High
size: S
status: not-started
sprint: sprint-1
updated_at: 2026-05-03T23:45:00Z
---

# Story 1.3 — New-Codebase Scope Guard

## Context

All bugbash operations (intake and fix) must be strictly scoped to new-codebase artifacts.
The scope guard is the foundation for all writes in Stories 1.1, 1.2, 2.x, and 3.x.
It must be in place before any intake or fix code is written — a failed guard must make
it impossible to write to unauthorized paths.

Per tech-plan Section 5 and Architecture Key Decision 6: `bugs/` is operational state written
directly by scripts. The scope guard permits direct writes to `governance_repo/bugs/` and
`governance_repo/features/lens-dev/new-codebase/`. All other paths are hard-blocked.

## Tasks

1. Create shared path guard function that validates any path is within the two authorized prefixes.
2. Hard-code the scope prefixes — no dynamic override at runtime.
3. Add scope guard to `bug-reporter-ops.py` (applied before any file operation).
4. Add scope guard to `bug-fixer-ops.py` (applied before any file operation).
5. Add startup validation: if `governance_repo` path does not exist, exit 1 with clear config error (A7).
6. Write unit tests covering the scope guard cases from tech-plan Section 7.2.
7. Commit with message: `[dev:1.3] lens-dev-new-codebase-bugbash — new-codebase scope guard`.

## Acceptance Criteria

- [ ] Path guard validates any read/write path is within `governance_repo/bugs/` or `governance_repo/features/lens-dev/new-codebase/`
- [ ] Cross-scope path is blocked with explicit scope violation error; no file is written
- [ ] Scope guard is shared across `bug-reporter-ops.py` and `bug-fixer-ops.py`
- [ ] Hard-coded scope prefixes; no dynamic override at runtime
- [ ] Unit tests confirm zero mutations outside authorized scope on simulated cross-scope bypass
- [ ] Startup validation: if `governance_repo` path does not exist, exit 1 with clear config error before any file operations (A7)

## Regression Coverage (tech-plan § 7.2)

| Test | Expected |
|------|----------|
| Write to `bugs/New/` within governance_repo | PASS |
| Write to path outside governance_repo | Blocked; scope violation error |
| Write to `governance_repo/features/lens-dev/old-codebase/` | Blocked |
| Write to `governance_repo/features/lens-dev/new-codebase/` | PASS |
| `governance_repo` path does not exist | Exit 1 with config error (startup validation) |
