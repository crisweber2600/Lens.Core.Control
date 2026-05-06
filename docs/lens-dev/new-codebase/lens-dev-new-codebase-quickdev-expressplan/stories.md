---
feature: lens-dev-new-codebase-quickdev-expressplan
doc_type: stories
status: approved
goal: "Story list with acceptance criteria for the lens-quickdev wrapper delivery."
key_decisions:
  - Stories stay aligned to the three user-value epics: governed entry, scoped execution, and audit/publication.
  - The first story is seeded as ready-for-dev; the remaining approved stories stay in backlog order.
open_questions: []
depends_on:
  - business-plan.md
  - tech-plan.md
  - sprint-plan.md
  - epics.md
blocks: []
updated_at: '2026-05-06T21:05:00Z'
---

# Stories — lens-quickdev Wrapper

## Epic 1 — Governed Quickdev Entry and Planning Gate

### QD-1.1 — Add Public Prompt and Skill Surfaces

**Type:** [new]
**Points:** 3

**Story:** As a Lens maintainer, I want `lens-quickdev` to exist as a public prompt and owned skill surface so that users can invoke the wrapper without manually routing through `lens-bmad-skill`.

**Acceptance Criteria:**

- [ ] `_bmad/lens-work/prompts/lens-quickdev.prompt.md` exists as a redirect-only prompt.
- [ ] `_bmad/lens-work/skills/lens-quickdev/SKILL.md` exists as the owning conductor contract.
- [ ] The prompt contains no business logic beyond preflight and skill loading.
- [ ] The skill delegates implementation behavior to `bmad-quick-dev` rather than duplicating it.

### QD-1.2 — Register Command Discovery and Operator Help

**Type:** [new]
**Points:** 2

**Story:** As a Lens maintainer, I want `lens-quickdev` to be discoverable and clearly documented as dev-ready only so that users can find the command and understand its lifecycle boundary before running it.

**Acceptance Criteria:**

- [ ] `module.yaml` and `module-help.csv` expose `lens-quickdev` exactly once.
- [ ] Operator-facing guidance states that `lens-quickdev` is a dev-ready-only governed wrapper.
- [ ] Discovery/help updates do not silently expand into broader non-source surfaces without an explicit override record.

### QD-1.3 — Implement Feature Resolution and Dev-Ready Gate

**Type:** [new]
**Points:** 3

**Story:** As a Lens maintainer, I want the wrapper to resolve feature context, docs paths, and target repo metadata before delegation so that quickdev runs can fail safely when lifecycle or metadata preconditions are not met.

**Acceptance Criteria:**

- [ ] Active feature context resolves automatically and honors `--feature-id` overrides.
- [ ] Non-dev-ready features block before target-repo assessment.
- [ ] Missing or unresolved `target_repos` blocks the run without guessing a write target.
- [ ] Docs and governance docs paths resolve before quickdev evidence is created.

### QD-1.4 — Create Versioned Quickdev Evidence Scaffold

**Type:** [new]
**Points:** 3

**Story:** As a Lens maintainer, I want every quickdev run to start with a versioned evidence artifact and codebase assessment so that the implementation plan, assumptions, and validation path are captured before any code changes occur.

**Acceptance Criteria:**

- [ ] The wrapper creates `quickdev/quickdev-[summaryofrequeststub]-vNNN.md` for each run.
- [ ] Reruns create the next available version instead of overwriting prior evidence.
- [ ] The artifact records request, assessment, assumptions, validation plan, and implementation plan before delegation.

## Epic 2 — Scoped Implementation Execution and Branch Control

### QD-2.1 — Delegate Implementation Through bmad-quick-dev

**Type:** [new]
**Points:** 3

**Story:** As a Lens maintainer, I want `lens-quickdev` to delegate code changes through the existing `bmad-quick-dev` engine so that the wrapper remains conductor-only and all implementation behavior stays in one place.

**Acceptance Criteria:**

- [ ] The wrapper delegates implementation through the sanctioned Lens quick-dev path.
- [ ] If no script facade exists, the wrapper loads the registered `bmad-quick-dev` skill directly with equivalent Lens context.
- [ ] No second implementation engine is introduced.

### QD-2.2 — Apply Branch Policy and PR Orchestration

**Type:** [new]
**Points:** 3

**Story:** As a Lens maintainer, I want branch and PR behavior to follow the agreed policy automatically so that quickdev runs end up on the right branch without ad hoc git decisions.

**Acceptance Criteria:**

- [ ] Active in-progress feature branches receive direct commits.
- [ ] Runs without an active feature branch prepare a working branch and PR through Lens git orchestration.
- [ ] Dirty or ambiguous branch states block before implementation and are recorded in the quickdev artifact.

### QD-2.3 — Record Validation, Commit, and No-Op Outcomes

**Type:** [new]
**Points:** 3

**Story:** As a Lens maintainer, I want validation and commit outcomes written back into the versioned quickdev artifact so that each run ends with a durable, inspectable implementation record.

**Acceptance Criteria:**

- [ ] Focused validation command and result are recorded in the artifact.
- [ ] Non-empty runs record conventional commit hash, changed files, branch, and PR URL when present.
- [ ] No-op runs record `no-op` and do not create empty commits.

### QD-2.4 — Handle Validation-Failure Branches and Preserve Bug Quickdev

**Type:** [new]
**Points:** 5

**Story:** As a Lens maintainer, I want failure handling and regression coverage to match the approved branch-by-timing contract so that quickdev failures stay recoverable and the bug-specific quickdev route remains intact.

**Acceptance Criteria:**

- [ ] Pre-commit validation failures create no commit and mark the artifact `blocked`.
- [ ] Local post-commit validation failures do not push or create PRs and record `validation-failed` guidance.
- [ ] Pushed or PR validation failures avoid history rewrite and record fix-forward or blocked PR recovery.
- [ ] `/lens-bug-quickdev` routing remains unchanged under regression coverage.

## Epic 3 — Audit Trail, Publication, and Safe Surface Expansion

### QD-3.1 — Publish Versioned Quickdev Evidence to Governance

**Type:** [new]
**Points:** 2

**Story:** As a Lens maintainer, I want the exact versioned quickdev artifact published to governance after each completed run so that the implementation record survives beyond the local feature branch.

**Acceptance Criteria:**

- [ ] The exact versioned artifact is published to `feature.yaml.docs.governance_docs_path/quickdev/`.
- [ ] Publication uses the sanctioned Lens publication path rather than direct governance authoring.
- [ ] Published reruns preserve their unique version suffixes.

### QD-3.2 — Reconcile Target Repos and Dev Handoff Metadata

**Type:** [new]
**Points:** 2

**Story:** As a Lens governance operator, I want FinalizePlan to register and validate the feature's implementation target metadata so that `/dev` can resolve `lens.core.src` without reopening planning questions.

**Acceptance Criteria:**

- [ ] `feature.yaml.target_repos` includes `lens.core.src`.
- [ ] The update is made through the sanctioned `feature-yaml` helper.
- [ ] Implementation-readiness records the versioned quickdev rule and sanctioned publication path.
- [ ] Strict handoff validation confirms the metadata and docs are internally consistent.

### QD-3.3 — Guard Scope Expansion and Final Audit Readiness

**Type:** [confirm]
**Points:** 2

**Story:** As a Lens maintainer, I want broader non-source work to be explicitly gated and documented so that the feature can expand safely when needed without silently swallowing scope creep.

**Acceptance Criteria:**

- [ ] Broader non-source work triggers a scope-creep warning before edits proceed.
- [ ] Any approved override is recorded in the feature docs.
- [ ] The downstream bundle documents command-surface scope, evidence versioning, and publication boundaries in one reviewable packet.