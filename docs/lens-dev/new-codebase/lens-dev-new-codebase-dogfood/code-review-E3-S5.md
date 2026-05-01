---
story: E3-S5
title: Resolve QuickPlan public vs internal surface decision
status: reviewed
reviewed_at: '2026-05-02T00:00:00Z'
---

# Code Review — E3-S5: QuickPlan Classification

## Summary

This story makes the QuickPlan internal classification explicit and consistent across three authoritative sources: `module.yaml`, `lens-bmad-skill-registry.json`, and `SKILL.md`. A regression test suite locks the classification to prevent drift.

---

## Files Changed

| File | Change |
|------|--------|
| `_bmad/lens-work/module.yaml` | Added `internal_skills` section with QuickPlan entry (`classification: internal`) |
| `_bmad/lens-work/assets/lens-bmad-skill-registry.json` | Added `surface: internal` field to `bmad-lens-quickplan` entry |
| `_bmad/lens-work/scripts/tests/test-quickplan-classification.py` | New: 7 consistency tests across all three classification sources |

---

## Acceptance Criteria Review

- [x] QuickPlan classification explicitly recorded in `module.yaml` — `internal_skills` section added with `classification: internal`
- [x] SKILL.md matches the classification decision — already declared internal-only; unchanged and consistent
- [x] Public help output excludes QuickPlan — not in `prompts` or `skills` lists; verified by tests
- [x] Test: `module.yaml` classification consistent with SKILL.md → passes — 7/7 tests pass

---

## Quality Notes

**Strengths:**
- Three-source consistency (module.yaml + registry + SKILL.md) closes the ambiguity the story identified
- Tests are precise and independently assert each source before a cross-check
- `internal_skills` section uses a comment header explaining the contract, improving maintainability

**No issues found.** The classification is unambiguous and locked by tests.
