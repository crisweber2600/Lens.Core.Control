---
feature: lens-dev-new-codebase-bugfix-commands-should-have-run-via-lens-4
doc_type: implementation-readiness
status: approved
goal: "Assess implementation readiness for the Phase 4 delegation clarification in bmad-lens-bug-fixer/SKILL.md."
key_decisions:
  - Feature is implementation-complete: fix already committed in source repo.
  - Story E1-S1 is in ready-for-verification state, not development state.
open_questions: []
depends_on: []
blocks: []
updated_at: '2026-05-03T00:00:00Z'
---

# Implementation Readiness — Commands Should Have Run Via Lens

## Readiness Assessment

### Overall Verdict: READY — FIX ALREADY IMPLEMENTED

This feature's sole deliverable has been implemented and committed. The story is in
**ready-for-verification** state, not backlog.

| Item | Status | Detail |
|------|--------|--------|
| Fix committed | ✅ DONE | Commit `56b1be33` on `fix/preflight-old-patterns` in `lens.core.src` |
| Planning bundle complete | ✅ DONE | business-plan, tech-plan, sprint-plan, expressplan-adversarial-review, finalizeplan-review |
| Source PR to `develop` | ⚠️ PENDING | PR not yet opened from `fix/preflight-old-patterns` |
| `feature.yaml.target_repos` | ⚠️ PENDING | Source repo not linked in feature.yaml |

---

## Risk Assessment

### Risk Register

| ID | Risk | Likelihood | Impact | Mitigation | Status |
|----|------|-----------|--------|-----------|--------|
| R1 | Other `bmad-lens-*` SKILL.md files use the same ambiguous "delegate to X skill" pattern | Medium | Low — future sessions may hit the same bug | Out of scope for this fix; tracked as follow-up | OPEN |
| R2 | Fix on `fix/preflight-old-patterns` is not merged to `develop` before next release promotion | Low | Medium — fix ships without release | Open PR before marking feature complete | OPEN |
| R3 | Manual verification not run after fix | Low | Low — fix is a text clarification, low regression risk | Run `/lens-bug-fixer --fix-all-new` after PR merges | OPEN |

### Closed / Mitigated Risks

| ID | Risk | Resolution |
|----|------|-----------|
| R4 | Phase 4 conductor interprets "delegate to skill" as shell command | Fix applied: explicit SKILL.md load language + "do NOT run as shell command" | CLOSED |

---

## Dev Checklist

Before marking E1-S1 done:

- [ ] Verify `bmad-lens-bug-fixer/SKILL.md` Phase 4 steps 18–20 match the spec in `tech-plan.md`
- [ ] Verify Error Recovery step 3 uses explicit SKILL.md load language
- [ ] Open PR: `fix/preflight-old-patterns` → `develop` in `lens.core.src`
- [ ] Link PR in `feature.yaml.links.pull_request` via `lens-feature-yaml`

## Implementation Scope

**Single file:** `_bmad/lens-work/skills/bmad-lens-bug-fixer/SKILL.md`  
**Repository:** `lens.core.src` (`TargetProjects/lens-dev/new-codebase/lens.core.src/`)  
**No database, API, or test infra changes.**
