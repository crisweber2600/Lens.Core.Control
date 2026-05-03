---
story_id: "3.3"
epic: "Epic 3"
feature: lens-dev-new-codebase-bugbash
title: Chain Validation & Regression Tests
priority: Medium
size: S
status: not-started
sprint: sprint-3
updated_at: 2026-05-03T23:45:00Z
---

# Story 3.3 — Chain Validation & Regression Tests

## Context

The final story validates that all three conductor chains are correctly assembled and
all regression categories from tech-plan Section 7 are covered and passing.

This story closes the feature implementation and gates dev-complete.

**Regression categories to cover:**
1. § 7.1 Schema Validation (5 test cases)
2. § 7.2 Scope Guard (4 test cases + governance-repo path mismatch from A7)
3. § 7.3 Conductor Chain (5 test cases)
4. § 7.4 Batch Idempotency (3 test cases)

Depends on: All prior stories complete (Sprint 1 + Sprint 2 + Stories 3.1, 3.2).

## Tasks

1. Run `scan-path-standards` for all 3 commands:
   - `lens-bugbash`, `lens-bug-reporter`, `lens-bug-fixer`
   - Confirm each passes the standard path check
2. Run `scan-scripts` for all 3 scripts:
   - `bugbash-ops.py --help`, `bug-reporter-ops.py --help`, `bug-fixer-ops.py --help`
   - Confirm all 3 exit 0 with clean help output
3. Run all schema validation tests (§ 7.1):
   - Intake with all required fields → artifact created; status=New; featureId=""
   - Missing title → rejected; no file written
   - Missing description → rejected; no file written
   - Invalid status value → rejected; prior state preserved
   - Invalid transition (New→Fixed) → blocked; explicit error
4. Run all scope guard tests (§ 7.2):
   - Write to `bugs/New/` within governance_repo → PASS
   - Write outside governance_repo → blocked; scope violation error
   - Write to `features/lens-dev/old-codebase/` → blocked
   - Write to `features/lens-dev/new-codebase/` → PASS
   - governance_repo path does not exist → exit 1 with config error (A7)
5. Run conductor chain tests (§ 7.3):
   - Stub invokes light-preflight.py before redirect → PASS
   - Stub redirects to correct release prompt path → PASS
   - Release prompt loads correct SKILL.md → PASS
   - scan-path-standards passes for all 3 commands → PASS
   - scan-scripts: all 3 scripts accept --help → PASS
6. Run batch idempotency tests (§ 7.4):
   - Re-run discover-new after bugs moved to Inprogress → 0 bugs discovered
   - Re-run create-bug with same title+description → "duplicate" result; no second file
   - Re-run resolve-bugs for already-Fixed bugs → "already_fixed"; no error
7. Fix any failures. Record any deferred items in a dev note.
8. Commit with message: `[dev:3.3] lens-dev-new-codebase-bugbash — chain validation and regression tests`.

## Acceptance Criteria

- [ ] `scan-path-standards` passes for all 3 commands (lens-bugbash, lens-bug-reporter, lens-bug-fixer)
- [ ] `scan-scripts` confirms all 3 scripts accept `--help` cleanly with exit 0
- [ ] All § 7.1 schema validation tests pass (5 cases)
- [ ] All § 7.2 scope guard tests pass (4 cases + A7 governance_repo validation)
- [ ] All § 7.3 conductor chain tests pass (5 cases)
- [ ] All § 7.4 batch idempotency tests pass (3 cases)
- [ ] Governance-repo path mismatch startup validation passes across all 3 scripts (A7)

## Implementation Notes

- A7: Startup validation must be in all three scripts (`bugbash-ops.py`, `bug-reporter-ops.py`, `bug-fixer-ops.py`) — checks that `governance_repo` exists before any file operations
- After all tests pass, this story completes; feature is ready for dev-complete review
