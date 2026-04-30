---
feature: lens-dev-new-codebase-finalizeplan
doc_type: stories
status: draft
goal: "Story list with acceptance criteria for FinalizePlan, ExpressPlan, QuickPlan conductor delivery."
updated_at: '2026-04-30T00:00:00Z'
---

# Stories — FinalizePlan, ExpressPlan, QuickPlan Conductors

## Epic 1 — Foundation Validation

---

### E1-S1 — Validate all conductor infrastructure

**Type:** [confirm]  
**Points:** 5

**Story:** As a Lens module maintainer, I want to confirm that all conductor infrastructure
files created in the prior session are structurally complete and correct so that Sprint 2
and Sprint 3 work can proceed from a verified baseline.

**Acceptance Criteria:**

1. `bmad-lens-finalizeplan/SKILL.md`:
   - [ ] Three-step execution contract is present: `review-and-push`, `plan-pr-readiness`, `downstream-bundle-and-final-pr`
   - [ ] On Activation step 5 validates predecessor as `techplan` OR `expressplan-complete` (not only `techplan`)
   - [ ] No direct governance file creation (all governance writes route through publish CLI or `bmad-lens-git-orchestration`)
   - [ ] Step 3 bundle delegates to `bmad-lens-bmad-skill` in the required order: epics-and-stories → readiness → sprint-planning → story creation
   - [ ] `feature.yaml` update to `finalizeplan-complete` is in Step 3 (not earlier)

2. `bmad-lens-expressplan/SKILL.md`:
   - [ ] Express-only gate present: activation stops for non-express tracks before any delegation
   - [ ] Step 1 delegates via `bmad-lens-bmad-skill --skill bmad-lens-quickplan`
   - [ ] Step 2 invokes `bmad-lens-adversarial-review --phase expressplan --source phase-complete`
   - [ ] Party-mode is required (not optional) in step 2
   - [ ] Step 3 sets `expressplan-complete` and signals `/finalizeplan`

3. `bmad-lens-quickplan/SKILL.md`:
   - [ ] Three-phase pipeline present: business plan (John/PM) → tech plan (Winston/Architect) → sprint plan (Bob/SM)
   - [ ] No public prompt stub — internal only
   - [ ] Called via `bmad-lens-bmad-skill` only

4. Prompt stubs and thin redirects:
   - [ ] `.github/prompts/lens-finalizeplan.prompt.md` uses shared prompt-start preflight pattern
   - [ ] `.github/prompts/lens-expressplan.prompt.md` uses shared prompt-start preflight pattern
   - [ ] `_bmad/lens-work/prompts/lens-finalizeplan.prompt.md` is a thin redirect to SKILL.md
   - [ ] `_bmad/lens-work/prompts/lens-expressplan.prompt.md` is a thin redirect to SKILL.md

5. `module.yaml` and `bmad-lens-bmad-skill`:
   - [ ] `module.yaml` lists `lens-finalizeplan.prompt.md` in the `prompts:` section
   - [ ] `module.yaml` lists `lens-expressplan.prompt.md` in the `prompts:` section
   - [ ] No duplicate entries (check against `lens-dev-new-codebase-expressplan` registrations)
   - [ ] `bmad-lens-bmad-skill/SKILL.md` integration table includes `bmad-lens-quickplan` as internal wrapper target

---

### E1-S2 — Confirm express-track constitution permission

**Type:** [new]  
**Points:** 2

**Story:** As a Lens module maintainer, I want to confirm that the `lens-dev/new-codebase`
domain constitution permits the `express` track so that ExpressPlan activation does not
fail the constitution permission check.

**Acceptance Criteria:**
- [ ] Locate the domain constitution file for `lens-dev/new-codebase`
- [ ] Confirm the `express` track is listed as permitted
- [ ] If absent: add the permission and commit the constitution file
- [ ] Document the constitution file path in this story's completion notes

---

### E1-S3 — Confirm publish CLI handles hyphenated artifact names

**Type:** [confirm]  
**Points:** 1

**Story:** As a Lens module maintainer, I want to confirm whether the `publish-to-governance`
CLI maps express-track artifact names (`tech-plan.md`, `business-plan.md`) correctly so
that FinalizePlan step 1's TechPlan publish is not a silent no-op.

**Acceptance Criteria:**
- [ ] Read `publish-to-governance` CLI source or its artifact mapping config
- [ ] Confirm whether `tech-plan.md` (hyphenated) maps to the `techplan` artifact slug
- [ ] If mismatch: file as tracked item; note that governance publish will be a no-op for express-track features until resolved
- [ ] Document the finding in this story's completion notes

---

### E1-S4 — Run all tests, confirm no regressions

**Type:** [confirm]  
**Points:** 2

**Story:** As a Lens module maintainer, I want all existing tests to continue passing after
the conductor infrastructure was added so that no regressions were introduced.

**Acceptance Criteria:**
- [ ] Run `uv run --with pytest --with pyyaml python -m pytest _bmad/lens-work -q` in the target source repo
- [ ] All tests pass (≥ 34)
- [ ] No new failures introduced by conductor infrastructure files

---

### E1-S5 — Commit untracked infrastructure files to target source repo

**Type:** [confirm]  
**Points:** 2

**Story:** As a Lens module maintainer, I want all conductor infrastructure files to be
committed to the `develop` branch of `lens.core.src` so that the implementation is
tracked in git and visible to other contributors.

**Acceptance Criteria:**
- [ ] `git status` in `lens.core.src` shows no untracked files under `_bmad/lens-work/skills/bmad-lens-finalizeplan/`
- [ ] `git status` shows no untracked files under `_bmad/lens-work/skills/bmad-lens-expressplan/`
- [ ] `git status` shows no untracked files under `_bmad/lens-work/skills/bmad-lens-quickplan/`
- [ ] `git status` shows no untracked files under `_bmad/lens-work/prompts/` for new prompts
- [ ] `git status` shows no untracked files under `.github/prompts/` for new stubs
- [ ] All files committed with message pattern: `[FEAT] lens-dev-new-codebase-finalizeplan — conductor skills and prompt stubs`
- [ ] Committed to `develop` branch (or feature branch if branching is required)

---

## Epic 2 — Discovery and Regressions

---

### E2-S1 — Register commands in discovery surface

**Type:** [new]  
**Points:** 3

**Story:** As a Lens user, I want `/lens-finalizeplan` and `/lens-expressplan` to appear
in the discovery surface (help, module-help.csv, or equivalent) of the new-codebase target
so that the commands are findable.

**Acceptance Criteria:**
- [ ] Identify the new-codebase discovery file (e.g., `_bmad/lens-work/module-help.csv`)
- [ ] Add `lens-finalizeplan` entry following the pattern of other retained commands
- [ ] Add `lens-expressplan` entry following the pattern of other retained commands
- [ ] Entries include: command name, description, available modes, phase applicability
- [ ] No entries for `bmad-lens-quickplan` (it is internal-only, not user-facing)

---

### E2-S2 — Document test coverage gaps

**Type:** [new]  
**Points:** 2

**Story:** As a QA engineer, I want known test coverage gaps documented so that they are
tracked for future hardening sprints and do not create false confidence in current coverage.

**Acceptance Criteria:**
- [ ] Document the following gaps in a `test-coverage-gaps.md` or within story completion notes:
  - FinalizePlan step-2 PR creation path (integration level, not covered by current unit tests)
  - FinalizePlan step-3 bundle + final PR path (integration level, not covered)
  - Constitution check in ExpressPlan (requires mock constitution fixture)
  - End-to-end command activation (behavior-level, explicitly out of scope)
- [ ] Each gap has a severity classification (High/Medium/Low) and a proposed next step

---

### E2-S3 — Confirm prerequisite skills in target project

**Type:** [confirm]  
**Points:** 2

**Story:** As a Lens module maintainer, I want to confirm that all skills delegated by the
conductors are present in the target source repo so that the conductors will not fail at
runtime with missing delegate errors.

**Acceptance Criteria:**
- [ ] `_bmad/lens-work/skills/bmad-lens-adversarial-review/SKILL.md` exists
- [ ] `_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md` exists
- [ ] `_bmad/lens-work/skills/bmad-lens-git-orchestration/SKILL.md` exists
- [ ] `_bmad/lens-work/scripts/validate-phase-artifacts.py` exists
- [ ] For any missing skill: flag as a dependency gap and create a tracked item

---

### E2-S4 — Update feature.yaml target_repos

**Type:** [new]  
**Points:** 1

**Story:** As a Lens governance operator, I want `feature.yaml.target_repos` to reference
`lens.core.src` so that cross-feature tooling can identify the implementation target.

**Acceptance Criteria:**
- [ ] `feature.yaml.target_repos` updated to include `lens.core.src` reference
- [ ] Committed to governance `main` and pushed

---

## Epic 3 — Handoff Readiness

---

### E3-S1 — Verify review findings resolved, open final PR

**Type:** [new]  
**Points:** 3

**Story:** As a Lens feature lead, I want to confirm no open fail-level findings remain
and open the final PR from the feature base branch to `main` so that `/dev` can begin.

**Acceptance Criteria:**
- [ ] No open fail-level findings in `expressplan-adversarial-review.md`
- [ ] No open fail-level findings in `finalizeplan-review.md`
- [ ] Epic 1 H2 remediation confirmed (predecessor gate accepts `expressplan-complete`)
- [ ] All Epic 1 and Epic 2 exit criteria met
- [ ] Final PR `lens-dev-new-codebase-finalizeplan` → `main` opened
- [ ] `feature.yaml` phase updated to `finalizeplan-complete` in governance repo
- [ ] Feature-index.yaml updated
- [ ] `/dev` signalled to implementation agents
