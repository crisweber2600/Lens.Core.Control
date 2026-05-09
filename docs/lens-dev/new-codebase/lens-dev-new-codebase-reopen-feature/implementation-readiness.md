---
feature: lens-dev-new-codebase-reopen-feature
doc_type: implementation-readiness
status: approved
updated_at: '2026-05-08T00:00:00Z'
---

# Implementation Readiness — lens-dev-new-codebase-reopen-feature

## Summary

All express-track planning artifacts are approved and the downstream bundle is complete. This feature is ready for dev handoff pending target_repos registration in `feature.yaml`.

## Planning Artifacts Status

| Artifact | Status | Notes |
|---|---|---|
| `business-plan.md` | ✅ approved | Deferred: /lens-reopen conductor |
| `tech-plan.md` | ✅ approved | Null track guard, unconditional completed_at removal, temp-dir test fixtures |
| `sprint-plan.md` | ✅ approved | Single sprint |
| `expressplan-adversarial-review.md` | ✅ responses-recorded | |
| `finalizeplan-review.md` | ✅ responses-recorded | pass-with-warnings verdict |
| `epics.md` | ✅ approved | E1 - 3 stories |
| `stories.md` | ✅ approved | S1.1, S1.2, S1.3 |
| `sprint-status.yaml` | ✅ approved | Sprint 1, 3 stories |

## Target Implementation

**Primary target file:**
```
TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-feature-yaml/scripts/feature-yaml-ops.py
```

**Tests file:**
```
TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-feature-yaml/scripts/tests/test-feature-yaml-ops.py
```

**Docs file:**
```
TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-feature-yaml/SKILL.md
```

## Open Risks

| Risk | Severity | Mitigation |
|---|---|---|
| `target_repos` not yet populated in `feature.yaml` | High | Must be resolved before dev handoff — see H1 in finalizeplan-review.md |
| `/lens-reopen` conductor absent | Medium | Formally deferred; documented in business-plan.md and finalizeplan-review.md |
| Governance mirror state after reopen | Low | Out-of-scope per finalizeplan-review.md M2; acceptable for this slice |

## Prerequisites for Dev

- [x] Planning bundle complete and committed
- [x] `feature.yaml` phase = `finalizeplan-complete` (after Step 3 completion)
- [ ] `target_repos` populated in `feature.yaml` (H1 — must be resolved)
- [x] Dev branch exists: `lens-dev-new-codebase-reopen-feature-dev`
- [x] Test harness: `uv run python -m pytest` confirmed working in repo

## Test Strategy

- Unit tests with temp-dir `feature.yaml` fixtures
- Assertions cover: phase, status, `completed_at` removal, `phase_transitions` append, `feature-index.yaml` status = `active`
- Guard test: non-terminal feature returns `reopen_not_allowed`
- Run via: `uv run python -m pytest`
