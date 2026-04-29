# Story EP-1.BP-3: Regression gate and discovery surface verification

**Feature:** lens-dev-new-codebase-businessplan  
**Epic:** EP-1 — Businessplan Conductor Rewrite  
**Status:** ready-for-dev

---

## Story

As a Lens maintainer,  
I want regression tests to confirm businessplan wrapper-equivalence and governance-audit compliance,  
so that the thin conductor rewrite does not introduce silent behavioral regressions or governance write violations.

## Acceptance Criteria

1. Both regression categories green for businessplan: wrapper-equivalence and governance-audit
2. `module-help.csv` has matching entry to old codebase for `businessplan`
3. `agents/lens.agent.md` command surface unchanged — 17-command surface, `businessplan` still listed

## Tasks / Subtasks

- [ ] Run wrapper-equivalence regression for businessplan (AC: #1)
  - [ ] Verify batch 2-pass equivalence: pass 1 stops, pass 2 resumes with pre-approved context
  - [ ] Verify review-ready fast path: `validate-phase-artifacts.py` invoked before delegation
  - [ ] Verify both delegation routes (PRD → `bmad-create-prd`, UX → `bmad-create-ux-design`) produce same artifacts as old codebase
  - [ ] Verify `/next` auto-delegation path: no run-confirmation prompt appears, phase entry proceeds immediately
- [ ] Run governance-audit regression for businessplan (AC: #1)
  - [ ] Confirm no direct governance writes in businessplan skill
  - [ ] Confirm `publish-to-governance --phase preplan` runs before any PRD or UX authoring
- [ ] Verify `module-help.csv` entry for `businessplan` matches old codebase (AC: #2)
- [ ] Verify `agents/lens.agent.md` 17-command surface unchanged (AC: #3)

## Dev Notes

- **Regression test command:** `uv run --with pytest pytest lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/ -q`
- **On fail:** Block merge to develop; return to BP-1 for fix
- **Scope constraint (FP-4):** Architecture-reference regression applies to full-track techplan invocations only. Express-track does not produce `prd.md`; this test category is **skipped** for express-track test scenarios. Architecture-reference regression belongs to `lens-dev-new-codebase-techplan`.
- **Unblocking signal (FP-5):** When BP-3 green, this unblocks `lens-dev-new-codebase-baseline` stories 4.4 (finalizeplan rewrite) and 4.5 (expressplan rewrite) in `lens.core.src`. Signal the baseline feature lead when BP-3 passes.
- **`/next` path coverage:** Explicitly include a test case for the `/next` auto-delegation path in wrapper-equivalence coverage.

### Project Structure Notes

- `module-help.csv`: `lens.core/_bmad/_config/module-help.csv` (or module-specific path — verify against old codebase)
- `lens.agent.md`: `lens.core/_bmad/lens-work/agents/lens.agent.md` (verify 17-command surface)
- Test location: `lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/tests/` (create if not present)

### References

- [Source: tech-plan.md §5 — Regression test categories]
- [Source: sprint-plan.md — BP-3 Implementation Notes (FP-4, FP-5 notes)]
- [Source: sprint-plan.md — BP-3 Note on `/next` path]

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List
