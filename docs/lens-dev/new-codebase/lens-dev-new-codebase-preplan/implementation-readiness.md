---
feature: lens-dev-new-codebase-preplan
doc_type: implementation-readiness
status: draft
goal: "Assess readiness to begin implementation of the preplan clean-room rewrite"
key_decisions:
  - Planning artifacts pass-with-warnings; all five finalizeplan-review findings are resolved before their respective sprint stories begin
  - F-5 resolved by B (message-only advance in PP-4.1) — already reflected in sprint-plan.md
  - F-1 resolved — ADR 1 updated to permit scripts/tests/ subdir; PP-1.3 names the canonical test file path
  - F-2 resolved — business-plan success criteria and sprint-plan PP-2.1 and PP-1.3 updated with /next pre-confirmed handoff
  - F-6 resolved — Sprint 3 gate verification command added to sprint-plan.md Sprint 3 prerequisite note
  - F-7 resolved — no-governance-write invariant check added to PP-3.3 acceptance criteria
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-28T00:00:00Z
---

# Implementation Readiness — Preplan Command

## Overall Assessment: READY WITH CONDITIONS

The combined planning set (business plan, tech plan, sprint plan) is implementable. The architecture is sound; shared-utility delegation is well-specified; all five finalizeplan-review findings are resolved at the planning level. Two external prerequisites (baseline stories from `lens-dev-new-codebase-baseline`) must be confirmed green before Sprint 3 begins. One prerequisite (baseline story 3-1) must be confirmed before Sprint 2 stories PP-2.1 and PP-3.3 close.

---

## Readiness Checklist

| Item | Status | Notes |
|---|---|---|
| Business plan reviewed and approved | ✅ Pass-with-warnings | All findings resolved |
| Tech plan reviewed and approved | ✅ Pass-with-warnings | ADR 1 updated for test file location |
| Sprint plan sequenced and gated | ✅ Ready | Sprint 3 gate verification command present |
| FinalizePlan review verdict | ✅ Pass-with-warnings | 5 findings — all resolved |
| Expressplan review findings (carried) | ✅ Resolved | F-1 and F-2 resolved in latest commits |
| Governance cross-check complete | ✅ Done | Impacts documented: businessplan, finalizeplan, next features |
| Feature.yaml phase correct | ✅ expressplan-complete | Ready for finalizeplan bundle |
| External prerequisites documented | ✅ Explicit | Baseline 1-2, 1-3, 3-1 gated in sprint plan |
| No ad hoc branch creation needed | ✅ Confirmed | Both branches exist |
| Definition of Done defined | ✅ Defined | See sprint-plan.md |

---

## Risk Register

| Risk | Probability | Impact | Mitigation | Blocks |
|---|---:|---:|---|---|
| Baseline story 1-2 (validate-phase-artifacts.py) not ready when Sprint 3 starts | Medium | High | Sprint 2 DoD requires Sprint 3 gate verification command to pass; PP-3.1 cannot start until confirmed | PP-3.1 |
| Baseline story 1-3 (bmad-lens-batch) not ready when Sprint 3 starts | Medium | High | Same Sprint 2 DoD gate; PP-3.2 cannot start until confirmed | PP-3.2 |
| Baseline story 3-1 (constitution fix) not ready when PP-2.1 or PP-3.3 begins | Medium | High | Stories PP-2.1 and PP-3.3 have explicit prerequisite notes; story closure blocked without confirmation | PP-2.1, PP-3.3 |
| `bmad-lens-businessplan` command not dev-ready when preplan phase completes | Low | Medium | Resolved by F-5 decision B: PP-4.1 emits a message-only advance; no live routing call; user initiates businessplan | PP-4.1 |
| Test location inconsistency — developer defaults to prohibited scripts/ path | Low | Medium | ADR 1 updated; PP-1.3 names the exact canonical test file path; code review gate | PP-1.3 |
| `/next` pre-confirmed handoff not implemented in PP-2.1 | Low | Medium | Explicit AC in PP-2.1 and parity test in PP-1.3; code review gate | PP-2.1 |
| No-governance-write invariant broken by adversarial review wiring (PP-3.3) | Low | High | Explicit AC in PP-3.3: "no-governance-write invariant test still passes after adversarial review wiring" | PP-3.3 |
| Inline batch or review-ready logic accidentally re-introduced | Low | Medium | Code review gate + parity tests turn red on regression | PP-3.1, PP-3.2 |

---

## External Prerequisite Tracking

The following baseline stories from `lens-dev-new-codebase-baseline` must be confirmed green before their respective sprint gate:

| Baseline Story | What It Provides | Required By | Sprint Gate |
|---|---|---|---|
| 1-2: validate-phase-artifacts shared utility | `validate-phase-artifacts.py` callable with `--phase preplan --contract review-ready` | PP-3.1 | Sprint 2 DoD |
| 1-3: batch 2-pass contract | `bmad-lens-batch --target preplan` callable for pass 1 and pass 2 resume | PP-3.2 | Sprint 2 DoD |
| 3-1: fix-constitution-partial-hierarchy | `bmad-lens-constitution` resolves partial hierarchy without panic | PP-2.1, PP-3.3 | Before PP-2.1 closes |

Sprint 2 cannot close until the following gate verification command passes in `TargetProjects/lens-dev/new-codebase/lens.core.src`:
```bash
uv run --with pytest pytest \
  _bmad/lens-work/skills/bmad-lens-validate-phase-artifacts/scripts/tests/ \
  _bmad/lens-work/skills/bmad-lens-batch/scripts/tests/ \
  _bmad/lens-work/skills/bmad-lens-constitution/scripts/tests/ \
  -q
```

---

## Governance Impacts

| Feature | Phase | Impact | Action Required |
|---|---|---|---|
| `lens-dev-new-codebase-businessplan` | `expressplan-complete` | Preplan's phase completion emits a message naming `/businessplan`; no live routing dependency | Monitor businessplan dev-readiness before wiring any live routing call in the future |
| `lens-dev-new-codebase-finalizeplan` | `preplan` | Preplan routes through finalizeplan at phase end (user action); finalizeplan command availability is a user concern, not a preplan dependency | Document in PP-4.2 scope decision notes |
| `lens-dev-new-codebase-next` | `preplan` | `/next` handoff contract depends on correct delegation from the next command | Coordinate `/next` pre-confirmed handoff with `lens-dev-new-codebase-next` team if that feature is under active development in the same sprint window |
| `lens-dev-new-codebase-baseline` | (split-from) | Three baseline stories are external prerequisites; this feature does not own them | Track via Sprint 2 DoD gate; escalate if baseline is blocked |

---

## Definition of Done (Feature-Level)

- [ ] `preplan` command available through stub and release prompt in `lens.core.src`.
- [ ] SKILL.md conductor delegates all non-authoring operations to shared utilities; no inline duplicated logic.
- [ ] Brainstorm-first ordering enforced with passing parity test.
- [ ] Analyst activation before brainstorm mode selection enforced with passing parity test.
- [ ] Both brainstorm modes (`bmad-brainstorming`, `bmad-cis`) covered by passing parity tests.
- [ ] Batch 2-pass contract delegates to `bmad-lens-batch` with passing parity test.
- [ ] Review-ready fast path delegates to `validate-phase-artifacts.py` with passing parity test.
- [ ] Phase completion gate enforces adversarial review (party mode) with passing parity test.
- [ ] No governance writes during preplan — invariant test passes.
- [ ] `/next` pre-confirmed handoff parity test passes.
- [ ] `module-help.csv` and `lens.agent.md` list `preplan` in the 17-command surface (or ownership delegation documented).
- [ ] An independent end-to-end run of the full preplan flow is recorded in PP-4.1 closure notes.
- [ ] All existing init-feature, create-domain, and create-service parity tests remain green.
- [ ] CI gate passes: no old-codebase file paths appear in any implementation PR diff.
