---
feature: lens-dev-new-codebase-constitution
doc_type: stories
status: approved
goal: "Produce a dev-ready story set for the constitution rewrite across express alignment, resolver core, parity restoration, and release hardening."
key_decisions:
  - Epic 1 stories are explicit setup gates for the express path and thin-surface contract.
  - Epic 2 owns the partial-hierarchy fix, merge-rule preservation, and express-track parity.
  - Epic 4 closes the negative safety suite before the feature can claim release readiness.
open_questions:
  - Does any current caller rely on the old accidental omission of `sensing_gate_mode`?
  - Is repo-level fixture coverage needed before the first implementation PR, or can it land in the final hardening story?
depends_on:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-constitution/epics.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-constitution/finalizeplan-review.md
blocks: []
updated_at: 2026-05-01T16:30:00Z
---

# Stories - Constitution Command Rewrite

## Epic 1 - Express Alignment and Command Surface

---

### E1-S1 - Verify and lock express-track feature state alignment

**Type:** [confirm]
**Points:** 2

**Story:** As a Lens maintainer, I want the sanctioned `bmad-lens-feature-yaml` transition
to remain the source of truth for this feature's express state so that the planning packet
and automation route through the same lifecycle path.

**Acceptance Criteria:**

- [ ] Governance `feature.yaml` confirms `track: express` and `phase: expressplan-complete`
- [ ] FinalizePlan artifacts refer to the express packet as the predecessor set, not the
  older full-path assumption
- [ ] No manual governance edits are required outside sanctioned feature-yaml operations
- [ ] Any remaining lifecycle assumptions are called out in the story notes

**Given** the feature was originally initialized on a full-path route,
**When** the implementation packet is reviewed,
**Then** the sanctioned express-state transition is the only lifecycle source of truth.

---

### E1-S2 - Verify the 3-hop prompt chain stays thin

**Type:** [confirm]
**Points:** 2

**Story:** As a Lens maintainer, I want the public prompt, release prompt, and SKILL.md to
stay thin so that all resolver behavior lives in `constitution-ops.py` and not in copied
prompt logic.

**Acceptance Criteria:**

- [ ] `.github/prompts/lens-constitution.prompt.md` keeps the required preflight chain
- [ ] `lens.core/_bmad/lens-work/prompts/lens-constitution.prompt.md` redirects to the skill
- [ ] `bmad-lens-constitution/SKILL.md` delegates logic to the script instead of embedding
  merge behavior inline
- [ ] Any prompt-chain drift is documented before code work starts

**Given** a user invokes the constitution command from the public prompt,
**When** the command surface is inspected,
**Then** the prompt chain delegates without inlining resolver logic.

---

### E1-S3 - Re-state and verify the read-only authority boundary

**Type:** [confirm]
**Points:** 2

**Story:** As a governance maintainer, I want the story packet to make the read-only
authority boundary explicit so that implementation does not accidentally mutate governance,
feature state, or control-doc artifacts.

**Acceptance Criteria:**

- [ ] Story notes state that constitution reads governance but performs no writes
- [ ] Only sanctioned `bmad-lens-feature-yaml` operations may mutate feature state
- [ ] Traversal and malformed-input handling are called out as required negative tests
- [ ] Release hardening story references the no-write guarantee explicitly

**Given** the constitution command is a shared runtime primitive,
**When** implementation stories are handed to dev,
**Then** the packet makes read-only behavior a non-negotiable contract.

---

## Epic 2 - Resolver Core and Merge Contract

---

### E2-S1 - Rewrite partial-hierarchy resolution flow

**Type:** [rewrite]
**Points:** 5

**Story:** As a Lens maintainer, I want `resolve` to skip missing constitution levels with
warnings instead of hard-failing so that partial governance hierarchies remain valid for
all callers.

**Acceptance Criteria:**

- [ ] Missing org/domain/service/repo levels append structured warnings instead of errors
- [ ] Valid partial hierarchies return exit code `0`
- [ ] Empty hierarchies return defaults plus warnings rather than a hard failure
- [ ] Output includes `levels_loaded`, resolved constitution payload, and warnings
- [ ] No caller-specific workaround logic is added to prompts or SKILL.md

**Given** a governance tree with no org constitution,
**When** `resolve` is invoked,
**Then** the script returns a valid merged payload with a `level_absent` warning and exit code `0`.

---

### E2-S2 - Preserve merge rules, defaults, and express-track parity

**Type:** [rewrite]
**Points:** 4

**Story:** As a Lens maintainer, I want the rewritten resolver to preserve merge semantics
while supporting `express` and `sensing_gate_mode` so that callers receive the same shared
governance contract they depend on today.

**Acceptance Criteria:**

- [ ] `permitted_tracks` uses intersection across loaded levels
- [ ] `required_artifacts` unions by phase bucket without duplicates
- [ ] `gate_mode` and `sensing_gate_mode` preserve strongest-wins behavior
- [ ] `enforce_stories` and `enforce_review` preserve true-wins behavior
- [ ] Defaults and known-track handling include `express`

**Given** constitutions that permit `express` at each loaded level,
**When** the merge contract is resolved,
**Then** the merged output still permits `express` without downstream local overrides.

---

### E2-S3 - Harden constitution loading and scope validation

**Type:** [rewrite]
**Points:** 3

**Story:** As a Lens maintainer, I want constitution file loading to surface malformed
frontmatter, unknown keys, and invalid scope inputs safely so that the resolver remains
predictable under hostile or broken input.

**Acceptance Criteria:**

- [ ] Malformed frontmatter returns parse-error details without unsafe fallback behavior
- [ ] Unknown keys are surfaced in payload warnings or metadata
- [ ] Invalid slugs and traversal attempts are rejected with exit code `1`
- [ ] Path construction stays within the configured constitutions root
- [ ] No write path is introduced while handling errors

**Given** an invalid repo or service slug containing traversal segments,
**When** the resolver is invoked,
**Then** the command fails safely without reading or writing outside the constitutions tree.

---

## Epic 3 - Compliance and Progressive Display Parity

---

### E3-S1 - Implement `check-compliance` gate behavior

**Type:** [rewrite]
**Points:** 4

**Story:** As a planning conductor, I want `check-compliance` to distinguish hard-gate and
informational failures correctly so that lifecycle callers can trust its exit codes.

**Acceptance Criteria:**

- [ ] Track, artifact, review, and stories checks are evaluated against the resolved constitution
- [ ] Exit code `2` is reserved for hard-gate failures
- [ ] Informational-only failures return exit code `0` with failure detail in payload
- [ ] Output includes structured `checks`, `hard_failures`, and `informational_failures`
- [ ] Express-track compliance works when the hierarchy permits it

**Given** a hard-gate constitution with a missing required artifact,
**When** `check-compliance` runs,
**Then** the script returns a FAIL payload and exits with code `2`.

---

### E3-S2 - Implement `progressive-display` filters and warning propagation

**Type:** [rewrite]
**Points:** 3

**Story:** As a Lens caller, I want `progressive-display` to return context-filtered rules
with accurate warnings and `full_constitution_available` so that users can understand the
effective governance scope without reading every constitution level manually.

**Acceptance Criteria:**

- [ ] Phase-filtered output exposes `required_artifacts_for_phase`
- [ ] Track-filtered output exposes `track_permitted` and `permitted_tracks`
- [ ] Missing-level warnings from `resolve` are preserved in display output
- [ ] `full_constitution_available` is false whenever org is absent
- [ ] Express-track filtering reports the correct permission state

**Given** a partial hierarchy that permits `express`,
**When** `progressive-display --track express` runs,
**Then** the payload reports `track_permitted=true` and carries any missing-level warnings forward.

---

### E3-S3 - Keep prompt, skill, and references aligned to the script contract

**Type:** [rewrite]
**Points:** 2

**Story:** As a Lens maintainer, I want the prompt chain, SKILL.md, and reference docs to
describe the same script-backed contract so that future changes do not reintroduce inline
logic or stale documentation.

**Acceptance Criteria:**

- [ ] SKILL.md capability descriptions match the implemented script behavior
- [ ] Release prompt remains a thin redirect to the skill
- [ ] Reference docs cover partial hierarchies, express parity, and safety behavior
- [ ] No document claims a write-capable path for constitution operations

**Given** a maintainer reads the prompt, skill, and references together,
**When** they compare them to `constitution-ops.py`,
**Then** all three surfaces describe the same contract and boundaries.

---

## Epic 4 - Regression and Release Hardening

---

### E4-S1 - Build fixture-backed parity coverage

**Type:** [new]
**Points:** 4

**Story:** As a maintainer, I want temp-directory governance fixtures for full and partial
hierarchies so that the rewritten resolver has observable parity coverage rather than prose
claims.

**Acceptance Criteria:**

- [ ] Fixtures cover full hierarchy and representative sparse hierarchies
- [ ] Resolve, compliance, and progressive-display are exercised against the same fixture model
- [ ] Merge-rule scenarios cover intersection, union, and strongest-wins behavior
- [ ] Express-track parity cases are present in the fixture suite
- [ ] Fixture helpers keep tests isolated to temp directories

**Given** a temp-directory governance repo containing only domain and service constitutions,
**When** the parity suite runs,
**Then** the resolver behaves as a valid partial hierarchy and the tests remain isolated.

---

### E4-S2 - Add negative safety tests for malformed and hostile inputs

**Type:** [new]
**Points:** 3

**Story:** As a governance maintainer, I want explicit negative tests for malformed
frontmatter, invalid slugs, traversal attempts, and no-write behavior so that the shared
resolver cannot silently regress on its highest-risk guarantees.

**Acceptance Criteria:**

- [ ] Malformed-frontmatter test asserts parse errors are surfaced cleanly
- [ ] Invalid slug and traversal tests assert exit code `1`
- [ ] No-write test confirms no governance or feature-state mutations occur during reads
- [ ] Negative tests run under the same temp-directory isolation model as parity fixtures
- [ ] Safety coverage is referenced in release-readiness notes

**Given** malformed constitution frontmatter or a traversal input,
**When** the negative suite runs,
**Then** the command fails safely and produces no writes.

---

### E4-S3 - Confirm release readiness and caller follow-ups

**Type:** [confirm]
**Points:** 2

**Story:** As a Lens maintainer, I want a final release-readiness check that records any
remaining caller follow-ups so that the feature can open its final PR with known risks and
without pretending unfinished audit work is complete.

**Acceptance Criteria:**

- [ ] Release notes summarize parity coverage and remaining caller-audit items
- [ ] Any unresolved repo-level fixture or caller follow-up is recorded explicitly
- [ ] Final PR notes do not claim more than the implemented resolver contract
- [ ] The feature is ready to update `feature.yaml` to `finalizeplan-complete`

**Given** all implementation and regression stories are complete,
**When** the release gate is reviewed,
**Then** the final PR can be opened with explicit follow-up notes and no hidden blockers.