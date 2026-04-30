---
feature: lens-dev-new-codebase-switch
doc_type: stories
status: approved
goal: "Complete story list with acceptance criteria for the switch command clean-room rewrite"
key_decisions:
  - Stories SW-1 to SW-4 are Sprint 1 (EP-1): prompt parity
  - Stories SW-5 to SW-9 are Sprint 2 (EP-2): operation parity
  - Stories SW-10 to SW-12 are Sprint 3 (EP-3): release surface parity
  - SW-12 is blocked-by SW-9 (test infrastructure must exist first)
open_questions: []
depends_on: [epics.md, tech-plan.md, sprint-plan.md]
blocks: []
updated_at: 2026-04-27T00:00:00Z
---

# Stories — Switch Command

## Sprint 1 — EP-1: Contract Lock And Prompt Parity

### SW-1 — Verify Prompt-Start Gate Parity

**Epic:** EP-1  
**Estimate:** S  
**Status:** not-started

**Description:**  
The published stub (`lens-switch.prompt.md`) must run `uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py` before loading the release-module prompt. No config lookup, skill load, or script call may precede the preflight.

**Acceptance Criteria:**
- [ ] Stub file contains the `uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py` call as the first executable step.
- [ ] Stub stops execution and surfaces the failure message when preflight exits non-zero.
- [ ] Stub loads the release-module prompt only after successful preflight.
- [ ] Release prompt does not read skills or search alternate paths before receiving control from the stub.
- [ ] Smoke test confirms preflight → release prompt handoff flow.

---

### SW-2 — Preserve Config Resolution

**Epic:** EP-1  
**Estimate:** S  
**Status:** not-started

**Description:**  
Config resolution must prefer `.lens/governance-setup.yaml` when present, then fall back to `lens.core/_bmad/lens-work/bmadconfig.yaml`. Missing config files must not crash the command.

**Acceptance Criteria:**
- [ ] When `.lens/governance-setup.yaml` exists, its `governance_repo_path` overrides the module config value.
- [ ] When `.lens/governance-setup.yaml` is absent, the module config value is used.
- [ ] When both files are missing, the command reports a configuration error with actionable guidance.
- [ ] Config resolution happens before any governance file reads.

---

### SW-3 — Lock Numbered Menu Behavior

**Epic:** EP-1  
**Estimate:** M  
**Status:** not-started

**Description:**  
The numbered selection menu must be deterministic. `domains` mode renders the domain/service inventory and stops. `features` mode renders the numbered list and accepts a number or `q`. Invalid input rerenders the menu and stops. No inference from external state.

**Acceptance Criteria:**
- [ ] With no supplied feature id, `list` is invoked automatically.
- [ ] `mode: domains` response renders domain/service inventory and stops without asking for selection.
- [ ] `mode: features` response renders numbered list and asks for a number or `q`.
- [ ] Numeric input in range maps to the correct feature id.
- [ ] `q` cancels without side effects.
- [ ] Non-numeric or out-of-range input rerenders the menu and stops without guessing.
- [ ] No branch name, open file, recent path, or conversation history is used as a substitute for selection.
- [ ] Regression fixture: domains mode produces no feature selection; invalid input produces no feature selection.

---

### SW-4 — Remove Deprecated Public Command References

**Epic:** EP-1  
**Estimate:** S  
**Status:** not-started  
**Carry-Forward:** H1 from expressplan-review

**Description:**  
Switch user-facing messages, fallback help text, and inline guidance must not reference `init-feature` or other deprecated public command names. Replace with the retained command aliases from the 17-command surface.

**Acceptance Criteria:**
- [ ] String scan of the switch prompt, skill references, and switch script output confirms zero occurrences of `init-feature` as a user-facing command name.
- [ ] Any "missing branches" guidance directs users to the retained new-feature command alias.
- [ ] Coordinate with `lens-dev-new-codebase-new-feature` team to confirm the final retained command alias before closing this story.
- [ ] String-scan regression is added to the switch test suite and runs as part of SW-12.

---

## Sprint 2 — EP-2: Switch Operation Parity

### SW-5 — Validate Target Feature Identity

**Epic:** EP-2  
**Estimate:** S  
**Status:** not-started

**Description:**  
Feature id input must be sanitized and validated against `feature-index.yaml` before any path construction or file reads. Unsafe ids must fail fast with actionable errors.

**Acceptance Criteria:**
- [ ] Ids containing path separators, spaces, or non-alphanumeric characters (except hyphens) are rejected before any file I/O.
- [ ] A valid id that is not present in `feature-index.yaml` returns `status: fail` with `"error": "feature_not_found"`.
- [ ] A valid id present in the index proceeds to `feature.yaml` resolution.
- [ ] Malformed `feature-index.yaml` returns `status: fail` with `"error": "index_malformed"`.
- [ ] Test fixtures: invalid id characters, unknown id, malformed index.

---

### SW-6 — Return Complete Feature Context

**Epic:** EP-2  
**Estimate:** M  
**Status:** not-started

**Description:**  
A successful switch response must include all required fields: feature id, phase, track, priority, status, owner, stale flag, context path, target repo state, and context paths for related/depends-on/blocks.

**Acceptance Criteria:**
- [ ] Switch JSON response contains: `feature_id`, `domain`, `service`, `phase`, `track`, `priority`, `status`, `owner`, `stale`, `context_path`, `target_repo_state`, `context_paths`.
- [ ] `stale: true` when `feature.yaml.updated` is more than 30 days old; `stale: false` otherwise.
- [ ] `target_repo_state` is `null` when `target_repos` is empty; includes working branch and PR state when populated.
- [ ] Response passes SW-B6 acceptance criteria from the business plan.

---

### SW-7 — Persist Local Context Only

**Epic:** EP-2  
**Estimate:** S  
**Status:** not-started

**Description:**  
Switch must write `.lens/personal/context.yaml` with domain, service, timestamp, and `updated_by: lens-switch`. It must not modify any governance file.

**Acceptance Criteria:**
- [ ] After a successful switch, `.lens/personal/context.yaml` exists and contains `domain`, `service`, `updated_at` (ISO timestamp), and `updated_by: lens-switch`.
- [ ] No file in `TargetProjects/lens/lens-governance/` is modified by a switch operation.
- [ ] No file in `lens.core/_bmad/` is modified by a switch operation.
- [ ] Context write is idempotent; re-switching the same feature overwrites the context file without error.
- [ ] Governance no-write regression test passes.

---

### SW-8 — Report Branch Checkout Result

**Epic:** EP-2  
**Estimate:** S  
**Status:** not-started

**Description:**  
Switch must attempt `git checkout {featureId}-plan` in the control repo and report the result without guessing or falling back silently.

**Acceptance Criteria:**
- [ ] When the plan branch exists and checkout succeeds, response contains `branch_switched: true`, `checked_out_branch: "{featureId}-plan"`.
- [ ] When the plan branch is missing, response contains `branch_switched: false`, `branch_error: "branch_not_found"`, and a user-facing message: "Run /new-feature to initialize branches."
- [ ] When checkout fails for another reason (dirty tree, lock file, etc.), response contains `branch_switched: false`, `branch_error: "<git stderr>"`.
- [ ] No fallback checkout to an alternate branch is attempted.
- [ ] Test fixtures: branch exists, branch missing, dirty working tree.

---

### SW-9 — Normalize Dependency Context Paths

**Epic:** EP-2  
**Estimate:** S  
**Status:** not-started

**Description:**  
Related features map to summary paths; `depends_on` and `blocks` features map to tech-plan paths. Missing files are skipped with a caller-visible warning.

**Acceptance Criteria:**
- [ ] `related` feature ids each produce a `summary.md` path under `features/{domain}/{service}/{featureId}/`.
- [ ] `depends_on` and `blocks` feature ids each produce a `tech-plan.md` path under `docs/{domain}/{service}/{featureId}/`.
- [ ] For each context path, the response includes a `exists: true/false` field so callers skip missing files.
- [ ] Missing files do not cause switch to fail; they are included with `exists: false`.
- [ ] `test-switch-ops.py` has fixtures covering: no dependencies, one `depends_on`, one `blocks`, mixed missing.

---

## Sprint 3 — EP-3: Release Surface And Documentation Parity

### SW-10 — Align Command Discovery Surfaces

**Epic:** EP-3  
**Estimate:** M  
**Status:** not-started  
**Carry-Forward:** M4 awareness — bounded to switch-visible strings only

**Description:**  
All four command discovery surfaces must consistently advertise `switch` as an available retained command.

**Acceptance Criteria:**
- [ ] `lens.core/_bmad/lens-work/bmadconfig.yaml` (or module help CSV) lists `switch` with the correct description.
- [ ] The prompt manifest for `lens-switch.prompt.md` is present in the release module's prompts directory.
- [ ] Agent menu (AGENTS.md or equivalent) includes the switch command entry.
- [ ] Published docs reference switch with consistent syntax and description.
- [ ] Command discovery scan (uv run or grep-based) returns `switch` in results.
- [ ] Scope is bounded to switch-visible strings; non-switch command surfaces are not modified.

---

### SW-11 — Document JSON Contracts

**Epic:** EP-3  
**Estimate:** M  
**Status:** not-started

**Description:**  
The `bmad-lens-switch` skill reference must document the full JSON contracts for `list`, `domains`, and `switch` responses including stale warnings, target repo state, and context path structure.

**Acceptance Criteria:**
- [ ] Skill reference documents `list` success (mode: features), `list` success (mode: domains), `switch` success, and switch failure shapes.
- [ ] Each contract includes all required fields, types, and nullability notes.
- [ ] Stale warning behavior is documented: threshold, flag name, non-blocking behavior.
- [ ] Target repo state field is documented: structure when populated, null when absent.
- [ ] Context paths field is documented: `related`, `depends_on`, `blocks` entries with `exists` flag.
- [ ] Branch checkout result is documented: `branch_switched`, `checked_out_branch`, `branch_error`.

---

### SW-12 — Wire Focused Regression Command

**Epic:** EP-3  
**Estimate:** S  
**Status:** not-started  
**Blocked-By:** SW-9 (test infrastructure must exist), SW-10 (test file path verified)  
**Carry-Forward:** L1, L2 from expressplan-review

**Description:**  
A single documented `uv run` command must validate the switch script behavior and ExpressPlan artifact readiness end-to-end.

**Acceptance Criteria:**
- [ ] `test-switch-ops.py` exists at the path referenced in the tech-plan validation section.
- [ ] Running `uv run --with pytest test-switch-ops.py -q` from the target repo root exits 0.
- [ ] The test suite covers: feature listing, domain fallback, menu numbering, valid switch, context file write, target repo state, invalid identifiers, dependency context paths, and branch checkout reporting.
- [ ] SW-4's string-scan regression is included in the test suite.
- [ ] The regression command is documented in the feature's README or skill reference so future contributors can run it.
- [ ] This story must not start until SW-9 passes (test infrastructure exists and is verified).
