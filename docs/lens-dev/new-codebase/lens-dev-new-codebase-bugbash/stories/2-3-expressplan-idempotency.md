---
story_id: "2.3"
epic: "Epic 2"
feature: lens-dev-new-codebase-bugbash
title: Expressplan Execution & Batch Idempotency
priority: High
size: M
status: not-started
sprint: sprint-2
updated_at: 2026-05-03T23:45:00Z
---

# Story 2.3 — Expressplan Execution & Batch Idempotency

## Context

After bugs are moved to Inprogress (Story 2.2), Phase 4 runs expressplan against the
batch's content. The SKILL.md conductor delegates to `bmad-lens-expressplan` as a
separate skill step — it does not invoke expressplan logic inline.

**Planning input to expressplan:** concatenated bug descriptions + chat logs from all
bugs in the batch. Each bug's content is preserved as planning context.

**End of `fix-all-new` flow:** After expressplan, bugs remain Inprogress. The Fixed
transition only happens via `--complete {featureId}` (Story 2.4).

**Idempotency guarantee:** `discover-new` only surfaces bugs with status=New. Bugs already
in Inprogress or Fixed are invisible to it — ensuring no duplicate features are created
for already-processed bugs.

Depends on: Story 2.2 (Inprogress status must be set before expressplan runs).

## Tasks

1. Wire Phase 4 into `bmad-lens-bug-fixer` SKILL.md:
   - After Inprogress commit lands, delegate to `bmad-lens-expressplan`
   - Pass bug descriptions + chat logs as combined planning input
   - Bugs remain in Inprogress during and after expressplan execution
2. Handle expressplan failure: all bugs remain Inprogress; outcome report identifies failure with bug slug and error detail.
3. Verify idempotency: confirm `discover-new` returns 0 bugs after bugs are moved to Inprogress.
4. Write test: re-run `--fix-all-new` after prior run — confirm 0 bugs discovered, no duplicate feature created.
5. Write test: one bug fails expressplan — confirm other bugs unaffected.
6. Commit with message: `[dev:2.3] lens-dev-new-codebase-bugbash — expressplan execution and batch idempotency`.

## Acceptance Criteria

- [ ] Expressplan receives each bug's description + chat log as planning input
- [ ] All phase artifacts generated; sprint-status.yaml created and valid
- [ ] Bugs remain Inprogress throughout and after expressplan execution
- [ ] Per-item outcome log written at end of each batch run
- [ ] Re-run `--fix-all-new` after prior completion: `discover-new` returns 0 bugs; no duplicate feature created
- [ ] One bug fails expressplan: failed bug remains Inprogress; other bugs unaffected; outcome report identifies failure with bug slug and error detail

## Implementation Notes

- Delegation target: `bmad-lens-expressplan` skill — conductor delegates as a separate skill step, not inline logic
- Idempotency enforced by status check: only status=New is eligible for discover-new (Inprogress/Fixed bugs are invisible)
- `fix-all-new` never moves bugs to Fixed; Fixed state only via `--complete {featureId}`
