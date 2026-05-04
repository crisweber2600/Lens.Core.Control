---
feature: lens-dev-new-codebase-bugfix-commit-artifacts-rejects-message-fl
doc_type: implementation-readiness
status: approved
track: express
domain: lens-dev
service: new-codebase
depends_on: []
blocks: []
key_decisions:
  - Four stories are independent; implementation order is S1 → S2 → S3 → S4
  - S4 requires a separate control-repo PR for AGENTS.md changes
  - target_repos confirmed to include TargetProjects/lens-dev/new-codebase/lens.core.src
open_questions: []
updated_at: '2026-05-04T02:20:00Z'
---

# Implementation Readiness — Script Errors and On-the-fly Workflow Scripts

## Readiness Summary

| Check | Status | Notes |
|---|---|---|
| Business plan approved | ✅ | `business-plan.md` status: approved |
| Tech plan approved | ✅ | `tech-plan.md` status: approved |
| Sprint plan approved | ✅ | `sprint-plan.md` status: approved |
| ExpressPlan adversarial review complete | ✅ | Verdict: pass-with-warnings |
| FinalizePlan review complete | ✅ | Verdict: pass-with-warnings |
| Epics and stories defined | ✅ | 4 epics, 4 stories |
| Acceptance criteria complete | ✅ | All stories have full AC |
| Dependencies clear | ✅ | No cross-feature dependencies |
| Write boundary confirmed | ✅ | Source changes in `TargetProjects/lens-dev/new-codebase/lens.core.src/`; AGENTS.md via separate control-repo PR |
| Read-only clone risk addressed | ✅ | All stories target writable paths only |
| Target repos registered | ✅ | `feature.yaml.target_repos` includes `lens.core.src` |

**Overall Readiness: READY FOR DEV** — no blocking findings remain.

## Story Readiness Detail

### Story 1.1 — Feature YAML command aliases
- **Ready:** ✅
- **Scope clear:** `feature-yaml-ops.py` in writable source path
- **AC testable:** Unit tests for alias dispatch and transition validation
- **Risk:** Low — aliases delegate to existing validators

### Story 2.1 — Git orchestration command compatibility and PR body handling
- **Ready:** ✅
- **Scope clear:** `git-orchestration-ops.py` in writable source path
- **AC testable:** Unit tests for flag aliases and PR body generation
- **Risk:** Low — compatibility surface only; existing logic unchanged

### Story 3.1 — Preflight contract alignment
- **Ready:** ✅
- **Scope clear:** `light-preflight.py` and `SKILL.md` in writable source path
- **AC testable:** Root-detection tests and argument acceptance/rejection tests
- **Risk:** Medium — Option A vs Option B decision must be made at dev time; AC forces a concrete choice

### Story 4.1 — Durable lifecycle helpers and AGENTS.md terminal guidance
- **Ready:** ✅
- **Scope clear:** Script helpers in `lens.core.src`; `AGENTS.md` via control-repo PR (separate)
- **AC testable:** Script smoke test; AGENTS.md diff review
- **Risk:** Medium — multi-repo landing; dev agent must not attempt `AGENTS.md` via source-repo branch

## Constraints

- All code changes MUST target `TargetProjects/lens-dev/new-codebase/lens.core.src/`
- `lens.core/` is READ-ONLY — never edit it directly
- `AGENTS.md` control-repo edit MUST land via a separate control-repo PR, not via the `lens.core.src` feature branch
