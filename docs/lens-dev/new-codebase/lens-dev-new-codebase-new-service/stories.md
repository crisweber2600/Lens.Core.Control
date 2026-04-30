---
feature: lens-dev-new-codebase-new-service
doc_type: stories
status: approved
goal: "Deliver /new-service command parity through 13 sequenced stories across 4 epics"
key_decisions:
  - Test-first: NS-1 through NS-3 write failing tests before NS-4 begins implementation
  - ADR-3 delegation boundary locked in NS-4: create-service calls create-domain helpers
  - NS-13 is a required gate, not optional polish
open_questions: []
depends_on:
  - epics.md
blocks: []
updated_at: 2026-04-27T16:00:00Z
---

# Story List — New Service Command

**Total:** 13 stories | 29 story points | 4 epics

---

## Epic NS-E1: Contract and Test Lock

### NS-1: Define service CLI contract tests

**Story:** As a Lens setup user, I want the `create-service` command to have a documented, tested CLI contract so I can rely on its observable behavior regardless of how the implementation is structured internally.

**Estimate:** 3 points

**Acceptance Criteria:**
1. Failing tests exist for `create-service` success path (standard invocation)
2. Failing test exists for `--dry-run` behavior (no files written)
3. Failing test exists for duplicate service rejection
4. Failing test exists for invalid service ID rejection
5. Failing test exists for scaffold output structure
6. Failing test exists for context file output
7. Failing test exists for governance git behavior (`--execute-governance-git`)
8. **Failing test exists for `--dry-run + --execute-governance-git` mutual exclusion:** passing both flags simultaneously must return a rejected-arguments error without writing any files
9. All tests are in red state — implementation does not yet exist

**Dependencies:** `business-plan.md`, `tech-plan.md`

**Acceptance Gate:** All 9 test scenarios defined and failing.

---

### NS-2: Assert service-not-feature boundary

**Story:** As a governance enforcer, I want tests that prove `create-service` never writes feature lifecycle artifacts so I can be confident the service-container boundary is respected.

**Estimate:** 2 points

**Acceptance Criteria:**
1. Test confirms `create-service` does not create `feature.yaml`
2. Test confirms `create-service` does not create `summary.md`
3. Test confirms `create-service` does not create feature-index entries
4. Test confirms `create-service` does not create control branches
5. **Integration test: pre-existing domain container** — running `create-service` when the domain marker already exists must not overwrite or duplicate the existing domain marker or constitution; the command must detect the existing domain, skip parent-domain creation, and continue successfully

**Dependencies:** NS-1

**Acceptance Gate:** All 5 boundary tests defined and failing (or passing if the existing framework already prevents these actions).

---

### NS-3: Add prompt and help discovery expectations

**Story:** As a Lens user, I want static checks or tests that confirm `/new-service` is discoverable from both the prompt stub and module help surfaces so the command doesn't ship silently.

**Estimate:** 2 points

**Acceptance Criteria:**
1. Test or static check confirms `lens-new-service.prompt.md` exists at the expected release path
2. Test or static check confirms the module help entry for `new-service` exists and names the retained command
3. Checks are in failing/absent state until NS-9 and NS-10 are delivered

**Dependencies:** NS-1

**Acceptance Gate:** Discovery checks defined; expected to fail until NS-E3 delivers the prompt and help surface.

---

## Epic NS-E2: Script Implementation

### NS-4: Add service marker and constitution builders

**Story:** As a script developer, I want small, explicit helper functions for service marker and constitution creation so `create-service` has a clean, testable foundation.

**Estimate:** 3 points

**Acceptance Criteria:**
1. `make_service_yaml(domain, service, name, username)` returns a stable YAML structure matching the contract in `tech-plan.md` Data Model section
2. `make_service_constitution_md(domain, service)` returns an inherited service constitution with stable fields
3. `get_service_marker_path(governance_repo, domain, service)` returns the correct governance path
4. `get_service_constitution_path(governance_repo, domain, service)` returns the correct governance path
5. **ADR-3 delegation boundary documented:** code comments and NS-13 handoff notes record that parent-domain creation delegates to `create-domain` helpers (`make_domain_yaml`, `make_domain_constitution_md`) — no parallel domain-marker path inside `create-service`
6. NS-1 contract tests for these helpers turn green

**Dependencies:** NS-1

**Acceptance Gate:** Helper functions exist; associated NS-1 tests pass.

---

### NS-5: Implement `create-service` parser route

**Story:** As a Lens agent, I want the `init-feature-ops.py create-service` subcommand to accept all specified arguments so I can invoke it with governance, domain, service, and optional flags.

**Estimate:** 3 points

**Acceptance Criteria:**
1. CLI accepts: `--governance-repo`, `--domain`, `--service`, `--name`, `--username`, `--personal-folder`
2. CLI accepts optional: `--target-projects-root`, `--docs-root`, `--execute-governance-git`, `--dry-run`
3. `--dry-run` and `--execute-governance-git` are mutually exclusive; passing both returns a clear rejected-arguments error (NS-1 test 8 turns green)
4. Success JSON payload includes all fields specified in `tech-plan.md` API Contracts section
5. NS-1 success and dry-run contract tests turn green

**Dependencies:** NS-4

**Acceptance Gate:** Parser route works end-to-end; NS-1 success, dry-run, and flag-exclusion tests all green.

---

### NS-6: Extend context writer safely

**Story:** As a domain setup operator, I want context writing to support both domain-only (`new-domain`) and domain-plus-service (`new-service`) activation through one shared helper so there is no schema drift.

**Estimate:** 2 points

**Acceptance Criteria:**
1. Context writer accepts `service: str | None` parameter without breaking existing `new-domain` calls
2. Domain-only context write (service=None) produces unchanged output vs. baseline
3. Domain-plus-service context write includes both `domain` and `service` fields
4. Context YAML path remains `.lens/personal/context.yaml`
5. NS-1 context output tests turn green
6. Existing `new-domain` context tests remain green (no regression)

**Dependencies:** NS-5

**Acceptance Gate:** All context writer tests green; no `new-domain` regression.

---

### NS-7: Preserve governance git behavior

**Story:** As a governance maintainer, I want the `--execute-governance-git` path to be idempotent so a failed and retried run never creates duplicate service markers, domain markers, or constitution files.

**Estimate:** 3 points

**Acceptance Criteria:**
1. `--execute-governance-git` on success path returns a `governance_commit_sha` in JSON output
2. Remaining git commands for workspace scaffolds are included in `remaining_git_commands`
3. **Idempotency test passes:** run `create-service --execute-governance-git`, simulate a partial failure, re-run; governance state must be consistent with no duplicate artifacts
4. NS-1 governance git behavior tests turn green
5. Retry test demonstrates no duplicate service markers, domain markers, or constitution files on second run

**Dependencies:** NS-5

**Acceptance Gate:** All governance git tests green; idempotency test passes.

---

## Epic NS-E3: Skill, Prompt, and Surface Parity

### NS-8: Document `new-service` intent flow in SKILL.md

**Story:** As a Lens agent reading the init-feature skill, I want a documented `new-service` intent flow in `bmad-lens-init-feature/SKILL.md` so I can guide a user through service creation without referencing the script directly.

**Estimate:** 2 points

**Acceptance Criteria:**
1. SKILL.md extended with a `new-service` / `create-service` intent section
2. Intent flow covers: resolve governance_repo, resolve/ask for parent domain, ask for service display name, derive slug, confirm slug, invoke `create-service`, report SHA
3. Intent flow documents the parent-domain auto-establish behavior (ADR-3)
4. **Implementation channel:** change authored through `.github/skills/bmad-module-builder`
5. NS-3 SKILL.md discovery check turns green

**Dependencies:** NS-5

**Acceptance Gate:** SKILL.md intent section exists and passes NS-3 skill check.

---

### NS-9: Add release prompt

**Story:** As a user invoking `/new-service`, I want a release prompt that delegates to `bmad-lens-init-feature` with `create-service` intent so the command works from any Lens-equipped IDE.

**Estimate:** 2 points

**Acceptance Criteria:**
1. `lens-new-service.prompt.md` exists at `_bmad/lens-work/prompts/lens-new-service.prompt.md` in the new-codebase source
2. Prompt loads `bmad-lens-init-feature/SKILL.md` and specifies `create-service` intent
3. Prompt requires config resolution for: `governance_repo`, optional `target_projects_path`, optional `output_folder`, `personal_output_folder`
4. Prompt instructs callers to pass `--execute-governance-git` and report `governance_commit_sha`
5. **Implementation channel:** release prompt authored through `.github/skills/bmad-workflow-builder`
6. NS-3 prompt discovery check turns green
7. Breaking change: false

**Dependencies:** NS-8

**Acceptance Gate:** Prompt exists, NS-3 prompt check green.

---

### NS-10: Align command discovery metadata

**Story:** As a Lens user browsing module help, I want `new-service` to appear in the retained command index so I can find the command without reading the full planning set.

**Estimate:** 2 points

**Acceptance Criteria:**
1. Module help CSV (`module-help.csv`) includes a `new-service` entry consistent with retained command policy
2. `new-service` entry lists the prompt path, command description, and intent
3. No other retained commands are inadvertently removed or renamed
4. **Acceptance gate per finalizeplan-review:** `new-service` must have an explicit acceptance gate confirming it is discoverable from all listed entry points before NS-10 is marked complete (John PM party-mode concern)
5. NS-3 module-help discovery check turns green

**Dependencies:** NS-9

**Acceptance Gate:** Module-help entry verified and NS-3 module-help check green.

---

## Epic NS-E4: Verification and Handoff

### NS-11: Run focused service parity tests

**Story:** As a QA engineer, I want to run the focused service parity test suite in isolation so I can confirm `create-service` behavior before running the full regression.

**Estimate:** 2 points

**Acceptance Criteria:**
1. `uv run --with pytest pytest -k create_service` passes from the new-codebase source root
2. All NS-1, NS-2, NS-3 contract and boundary tests are green
3. No test is skipped without a documented justification
4. Test run output is captured in NS-13 handoff notes

**Dependencies:** NS-7, NS-10

**Acceptance Gate:** Focused service tests pass; output logged.

---

### NS-12: Run full init-feature regression

**Story:** As a release engineer, I want the complete init-feature test suite to pass after `create-service` is added so I can be confident the `new-domain` path is not regressed.

**Estimate:** 2 points

**Acceptance Criteria:**
1. Full init-feature test file passes: `uv run --with pytest pytest _bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/ -q`
2. No `new-domain` tests regress
3. **NS-13 gate check:** before marking NS-12 complete, verify that NS-13 handoff notes exist and reference: correct test commands, file list, implementation channel decisions, clean-room constraint statement
4. NS-12 is the explicit gating check for NS-13 completeness

**Dependencies:** NS-11

**Acceptance Gate:** Full regression passes; NS-13 existence verified by NS-12.

---

### NS-13: Prepare implementation handoff notes (required)

**Story:** As the `/dev` agent picking up implementation, I want a self-contained handoff document that names every file to touch, every test to run, and every constraint to honor so I do not need to re-read the full planning set.

**Estimate:** 1 point

**Acceptance Criteria:**
1. Handoff notes name all files to create or modify (SKILL.md, init-feature-ops.py, context writer, release prompt, module-help.csv)
2. Handoff notes include the exact test runner command for focused service tests
3. Handoff notes include the exact test runner command for full init-feature regression
4. Handoff notes record the clean-room constraint: no copying source from the old codebase
5. Handoff notes record implementation channel decisions: skill updates via `.github/skills/bmad-module-builder`, prompt/workflow artifacts via `.github/skills/bmad-workflow-builder`
6. Handoff notes record accepted deviations: direct `lens.core.src` edits for NS-4–NS-7 are accepted per `gate_mode: informational`
7. **Handoff notes are a required gate** — NS-13 is not optional polish; NS-12 validates its existence

**Dependencies:** NS-12

**Acceptance Gate:** NS-13 file exists and contains all 7 required items above.
