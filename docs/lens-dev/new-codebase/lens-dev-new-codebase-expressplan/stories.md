---
feature: lens-dev-new-codebase-expressplan
doc_type: stories
status: draft
updated_at: '2026-04-30T00:00:00Z'
---

# Stories — Expressplan Command

## Story E1-S1 — Validate Prompt Stubs

**Epic:** 1 — Foundation Validation
**Priority:** High
**Size:** XS

As a Lens contributor, I want the two expressplan prompt stubs to be confirmed as valid
and committed, so that the retained command surface is verifiably complete.

**Acceptance Criteria:**
- [ ] `.github/prompts/lens-expressplan.prompt.md` exists in the target source tree.
- [ ] Content matches the shared prompt-start preflight pattern from other retained commands.
- [ ] `_bmad/lens-work/prompts/lens-expressplan.prompt.md` exists and delegates correctly.
- [ ] Both files are tracked by git in the target source repo.

---

## Story E1-S2 — Validate SKILL.md Three-Step Contract

**Epic:** 1 — Foundation Validation
**Priority:** High
**Size:** S

As a Lens contributor, I want the `bmad-lens-expressplan` SKILL.md to be confirmed as
implementing the three-step conductor contract (eligibility gate, QuickPlan delegation,
adversarial review + phase advance), so that the command is behaviorally correct.

**Acceptance Criteria:**
- [ ] SKILL.md opens with an express-only eligibility gate that blocks non-express features.
- [ ] SKILL.md delegates to `bmad-lens-bmad-skill --skill bmad-lens-quickplan`.
- [ ] SKILL.md invokes adversarial review with `--phase expressplan --source phase-complete`.
- [ ] SKILL.md advances phase to `expressplan-complete` on `pass` or `pass-with-warnings`.
- [ ] SKILL.md does NOT advance phase on `fail` verdict.
- [ ] Party-mode usage is correctly enforced (if applicable to this skill pattern).

---

## Story E1-S3 — Validate module.yaml Registration

**Epic:** 1 — Foundation Validation
**Priority:** High
**Size:** XS

As a Lens contributor, I want `lens-expressplan.prompt.md` registered in `module.yaml`,
so that the command is discoverable via the retained module manifest.

**Acceptance Criteria:**
- [ ] `_bmad/lens-work/module.yaml` `prompts:` section includes `lens-expressplan.prompt.md`.
- [ ] The entry shape matches other retained-command entries in the same file.

---

## Story E1-S4 — Audit Test Coverage Gaps

**Epic:** 1 — Foundation Validation
**Priority:** Medium
**Size:** S

As a Lens contributor, I want the existing `test-expressplan-ops.py` to be read and its
coverage gaps documented, so that Epic 2 regression work has a baseline.

**Acceptance Criteria:**
- [ ] `test-expressplan-ops.py` is read and each test case is listed.
- [ ] Coverage gaps vs. the three-step contract are listed in a dev note or comment.
- [ ] Any test file not yet tracked by git is committed.

---

## Story E2-S1 — Register Command in Discovery Surface

**Epic:** 2 — Discovery and Regressions
**Priority:** High
**Size:** S

As a Lens contributor, I want `lens-expressplan` registered in the retained command
discovery surface, so that lifecycle tooling and agents can find the command.

**Acceptance Criteria:**
- [ ] The discovery file (same one used by `lens-techplan` and other retained commands) is
  identified.
- [ ] `lens-expressplan` is registered following the identical pattern.
- [ ] Registration change is committed to the target source repo.

---

## Story E2-S2 — Regression Expectations: Eligibility Gate

**Epic:** 2 — Discovery and Regressions
**Priority:** High
**Size:** S

As a Lens contributor, I want regression expectations for the express-only eligibility
gate to pass, so that non-express feature calls are correctly blocked.

**Acceptance Criteria:**
- [ ] Test verifies that a non-express feature triggers the eligibility guard.
- [ ] Test verifies that an express-track feature passes the gate and proceeds.
- [ ] Both tests pass (or clearly document a skip reason if infra not yet available).

---

## Story E2-S3 — Regression Expectations: QuickPlan Delegation

**Epic:** 2 — Discovery and Regressions
**Priority:** High
**Size:** S

As a Lens contributor, I want regression expectations for the QuickPlan delegation route
to pass, so that plan artifacts are correctly produced via `bmad-lens-bmad-skill`.

**Acceptance Criteria:**
- [ ] Test confirms invocation route calls `bmad-lens-bmad-skill --skill bmad-lens-quickplan`.
- [ ] Test passes (or clearly documents a skip reason).

---

## Story E2-S4 — Regression Expectations: Phase Advance Logic

**Epic:** 2 — Discovery and Regressions
**Priority:** High
**Size:** S

As a Lens contributor, I want regression expectations for the phase-advance decision to
pass, so that verdict-gated advancement is reliable.

**Acceptance Criteria:**
- [ ] Test: `pass` verdict → phase advances to `expressplan-complete`.
- [ ] Test: `pass-with-warnings` verdict → phase advances to `expressplan-complete`.
- [ ] Test: `fail` verdict → phase does NOT advance.
- [ ] All three tests pass (or clearly document skip reasons).

---

## Story E2-S5 — Confirm Shared Skill Prerequisites

**Epic:** 2 — Discovery and Regressions
**Priority:** Medium
**Size:** XS

As a Lens contributor, I want the shared skill prerequisites
(`bmad-lens-quickplan`, `bmad-lens-bmad-skill`, `bmad-lens-adversarial-review`) confirmed
as present in the target project, so that runtime dependencies are known.

**Acceptance Criteria:**
- [ ] Each prerequisite skill folder is confirmed to exist (or noted as absent + tracked).
- [ ] `validate-phase-artifacts.py` presence is confirmed.

---

## Story E3-S1 — Merge Planning PR and Signal Handoff

**Epic:** 3 — Handoff Readiness
**Priority:** High
**Size:** XS

As a Lens contributor, I want the planning PR merged and `finalizeplan-complete` reached,
so that `/dev` can begin.

**Acceptance Criteria:**
- [ ] Planning PR #30 is merged into `lens-dev-new-codebase-expressplan`.
- [ ] `feature.yaml` phase is `finalizeplan-complete`.
- [ ] Dev handoff is signalled with a summary of the three epics and entry story.
