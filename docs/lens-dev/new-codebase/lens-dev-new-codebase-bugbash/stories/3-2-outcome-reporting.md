---
story_id: "3.2"
epic: "Epic 3"
feature: lens-dev-new-codebase-bugbash
title: Per-Bug Outcome Reporting
priority: Medium
size: S
status: not-started
sprint: sprint-3
updated_at: 2026-05-03T23:45:00Z
---

# Story 3.2 — Per-Bug Outcome Reporting

## Context

After every batch run (success or partial failure), the fix orchestrator must print a
per-bug outcome report to the terminal. This gives the developer immediate visibility
into what happened without having to read git history or scan bug files manually.

The report is printed by the `bmad-lens-bug-fixer` SKILL.md conductor at the end of
every `--fix-all-new` run, regardless of outcome.

Depends on: Story 2.3 (expressplan execution produces per-item outcomes).

## Tasks

1. Define the outcome report format in `bmad-lens-bug-fixer` SKILL.md:
   ```
   === Bugbash Batch Outcome Report ===
   Batch featureId: {featureId}

   ✅ {slug} — success
   ❌ {slug} — failed: {exception_type}: {error_message}
   ...

   Total: {N} succeeded, {M} failed
   ===
   ```
2. Wire report printing in SKILL.md at the end of `--fix-all-new` flow:
   - Collect per-item results from all phases (feature creation, move-to-inprogress, expressplan)
   - Print formatted report to terminal
   - Report is always printed — even on full success or full failure
3. Write test: batch with 2 successes and 1 failure — confirm report includes all 3 slugs with correct outcome.
4. Write test: batch with 0 bugs — confirm report prints "0 bugs discovered. Queue is clean." (per A6).
5. Commit with message: `[dev:3.2] lens-dev-new-codebase-bugbash — per-bug outcome reporting`.

## Acceptance Criteria

- [ ] After fixbugs batch run (success or partial failure): terminal output lists each bug slug with outcome (success/failure)
- [ ] Failed bugs include error detail (exception type + message)
- [ ] Report includes totals: N succeeded, M failed
- [ ] Report is printed regardless of full-success or partial-failure outcome
- [ ] 0-bug run prints "0 bugs discovered. Queue is clean." rather than an empty report (A6)

## Implementation Notes

- Report is a terminal output concern — no file artifact needed for the report itself
- The per-item outcome data is collected in the SKILL.md conductor across all phases
