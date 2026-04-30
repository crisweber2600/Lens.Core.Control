---
feature: lens-dev-new-codebase-next
doc_type: stories
status: approved
goal: "Dev-ready story list for Next command rewrite across Slices 2, 3, and 4."
key_decisions:
  - Slice 2 stories carry the M2 precondition — trueup-complete must be confirmed before E1-S4.
  - Paused-state behavior must be documented in E2-S1 before E2-S4 paused-state fixtures are written.
  - Constitution resolver dependency tracked in E3-S3, not left as an implicit note.
open_questions:
  - Will lens-dev-new-codebase-constitution reach expressplan-complete before Slice 4 begins?
  - Who owns the constitution resolver allow-list fix — this feature or constitution feature?
depends_on:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-next/epics.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-next/finalizeplan-review.md
blocks: []
updated_at: 2026-04-30T22:15:00Z
---

# Stories — Next Command Rewrite

## Epic 1 — Prompt Chain and Discovery

---

### E1-S1 — Create public prompt stub

**Type:** [new]
**Points:** 2

**Story:** As a Lens user, I want a `lens-next.prompt.md` stub in `.github/prompts/` that
runs light preflight and then loads the release prompt so that the `next` command is
invokable from any Copilot session and fails gracefully on bad state.

**Acceptance Criteria:**

- [ ] `.github/prompts/lens-next.prompt.md` exists in the target repo
- [ ] Opening the prompt triggers `light-preflight.py`; a non-zero exit stops execution with
  the preflight error message
- [ ] On success the stub loads `_bmad/lens-work/prompts/lens-next.prompt.md`
- [ ] The stub contains no inline routing logic
- [ ] The stub follows the identical structure as other retained-command stubs
  (cross-check against `lens-finalizeplan.prompt.md` or `lens-expressplan.prompt.md`)

**Given** the user opens `/next` in Copilot with a valid preflight state,
**When** the public stub is loaded,
**Then** light preflight passes and the release prompt is loaded without a run-confirmation prompt.

**Given** the user opens `/next` with a broken preflight state,
**When** the public stub is loaded,
**Then** preflight outputs an error and execution stops before any routing occurs.

---

### E1-S2 — Create release prompt redirect

**Type:** [new]
**Points:** 1

**Story:** As a Lens module maintainer, I want a thin release-prompt redirect at
`_bmad/lens-work/prompts/lens-next.prompt.md` that points to the owning SKILL.md so
that future edits to routing logic live in exactly one place.

**Acceptance Criteria:**

- [ ] `_bmad/lens-work/prompts/lens-next.prompt.md` exists in the target repo
- [ ] The file is a thin redirect: it loads `bmad-lens-next/SKILL.md` and passes through
  control
- [ ] No routing logic, no inline heuristics
- [ ] Follows the same redirect pattern as `lens-finalizeplan.prompt.md` →
  `bmad-lens-finalizeplan/SKILL.md`

**Given** a Copilot session loads `lens-next.prompt.md`,
**When** the release prompt is processed,
**Then** control is handed off to `bmad-lens-next/SKILL.md` without branching in the prompt.

---

### E1-S3 — Scaffold `bmad-lens-next/SKILL.md` conductor shell

**Type:** [new]
**Points:** 3

**Story:** As a Lens module maintainer, I want a thin conductor `bmad-lens-next/SKILL.md`
that loads config, resolves the current feature state by invoking `next-ops.py suggest`,
and delegates to the recommended phase skill or surfaces blockers so that the `next`
command produces deterministic routing outcomes.

**Acceptance Criteria:**

- [ ] `_bmad/lens-work/skills/bmad-lens-next/SKILL.md` exists in the target repo
- [ ] On activation, the skill loads `bmadconfig.yaml` and resolves `{governance_repo}`,
  `{control_repo}`, and `{feature_id}`
- [ ] Invokes `next-ops.py suggest --feature-id {feature_id}` and reads the JSON result
- [ ] On `status=fail`: surfaces the error, stops
- [ ] On `status=blocked`: lists blockers, stops; no downstream delegation
- [ ] On `status=unblocked`: delegates to the recommended phase skill via `bmad-lens-bmad-skill`
  without surfacing a second confirmation prompt
- [ ] Contains no inline routing logic (all routing in `next-ops.py`)
- [ ] Contains no governance writes or control-doc writes

**Given** `next-ops.py` returns `status=unblocked, recommendation=/finalizeplan`,
**When** the SKILL.md conductor processes the result,
**Then** `bmad-lens-finalizeplan/SKILL.md` is loaded immediately without a confirmation prompt.

**Given** `next-ops.py` returns `status=blocked`,
**When** the SKILL.md conductor processes the result,
**Then** the blocker list is surfaced and no downstream skill is loaded.

---

### E1-S4 — Register `next` in discovery surfaces

**Type:** [new]
**Points:** 2

**Story:** As a Lens user, I want the `next` command to appear in all retained discovery
surfaces (`module-help.csv`, `module.yaml` prompts section, and any command-list surfaces)
so that the command is discoverable through standard help and auto-complete flows.

**Acceptance Criteria:**

- [ ] `module-help.csv` contains a row for `next` matching the format of other retained commands
- [ ] `module.yaml` prompts section lists `lens-next.prompt.md`
- [ ] No duplicate entries with any other feature (check against current module.yaml)
- [ ] If a `lens.agent.md` or similar command-list surface exists, `next` is present there
  too

**Precondition (M2):** Before completing this story, verify `lens-dev-new-codebase-trueup`
has completed its discovery-surface writes. This story's registration must not produce
conflicts with trueup's changes. Document the outcome of this check in Dev Notes below.

**Given** a user queries the module help surface,
**When** they search for the `next` command,
**Then** `next` appears with a one-line description matching other retained commands.

---

## Epic 2 — Routing Engine Parity

---

### E2-S1 — Implement `next-ops.py` core routing logic

**Type:** [new]
**Points:** 5

**Story:** As a Lens module maintainer, I want `next-ops.py suggest` to read `feature.yaml`
and `lifecycle.yaml` and return a structured JSON recommendation so that the `next`
conductor has a deterministic, script-testable routing engine.

**Acceptance Criteria:**

- [ ] Script at `_bmad/lens-work/skills/bmad-lens-next/scripts/next-ops.py` exists
- [ ] `suggest` subcommand accepts `--feature-id` and optional `--governance-repo` and
  `--control-repo` arguments
- [ ] Reads `feature.yaml` for current `phase` and `track`
- [ ] Reads `lifecycle.yaml` from the installed module path (live file, not stubbed)
- [ ] Resolves `auto_advance_to` from lifecycle.yaml for the current phase; uses that as the
  recommendation if set
- [ ] Returns JSON: `{ "status": "unblocked"|"blocked"|"fail", "recommendation": "/phase",
  "blockers": [], "warnings": [], "phase": "...", "track": "..." }`
- [ ] On unknown phase or track: `status=fail` with descriptive message
- [ ] On missing `feature.yaml`: `status=fail` with descriptive message
- [ ] Produces no side effects — no file writes, no git operations

**Paused-state gate (M1):** Before this story is marked done, document the selected
paused-state behavior in the Dev Notes below (options: report as blocker with instructions,
load internal pause-resume skill route, or fail with descriptive message). This decision
must be recorded here before E2-S4
fixture for paused state is written. No paused-state fixture is written without this
decision being committed to the story file.

**Given** `feature.yaml` has `phase: expressplan-complete, track: express`,
**When** `next-ops.py suggest` is run,
**Then** `lifecycle.yaml` `auto_advance_to` for `expressplan-complete` on the express track
resolves to `/finalizeplan` and the JSON result has `status=unblocked, recommendation=/finalizeplan`.

**Given** `feature.yaml` has an unrecognized `phase` value,
**When** `next-ops.py suggest` is run,
**Then** the JSON result has `status=fail` and a message identifying the unknown phase.

---

### E2-S2 — Write full-track routing parity fixtures

**Type:** [new]
**Points:** 3

**Story:** As a Lens module maintainer, I want a parity test fixture covering all full-track
routing paths so that `next-ops.py` routing correctness is verifiable against the
lifecycle contract without manual inspection.

**Acceptance Criteria:**

- [ ] Fixture file at `_bmad/lens-work/skills/bmad-lens-next/tests/fixtures/next-routing-full-track.yaml`
  (or equivalent test path)
- [ ] Covers: `preplan` → `/preplan`, `preplan-complete` → next phase, all full-track
  phase transitions, final phase → complete, complete phase → `status=blocked` or custom
  message
- [ ] Each fixture entry specifies: `phase`, `track`, `expected_recommendation`,
  `expected_status`
- [ ] Fixtures load `lifecycle.yaml` from the live installed path; no hard-coded lifecycle
  content inside the fixture
- [ ] All fixtures pass against `next-ops.py suggest`

**Given** a fixture defines `phase: preplan, track: full`,
**When** fixtures are executed against `next-ops.py suggest`,
**Then** the result matches `expected_recommendation: /preplan` with `status=unblocked`.

---

### E2-S3 — Write express-track routing parity fixtures

**Type:** [new]
**Points:** 3

**Story:** As a Lens module maintainer, I want parity fixtures for all express-track routing
paths including `expressplan-complete → /finalizeplan` so that express routing is
regression-covered against the actual lifecycle contract.

**Acceptance Criteria:**

- [ ] Fixture file at `_bmad/lens-work/skills/bmad-lens-next/tests/fixtures/next-routing-express-track.yaml`
- [ ] Covers: `expressplan` → `/expressplan`, `expressplan-complete` → `/finalizeplan`,
  missing express phase → track start phase, express track with blockers → `status=blocked`
- [ ] Each fixture entry specifies `phase`, `track`, `expected_recommendation`,
  `expected_status`
- [ ] Fixtures load `lifecycle.yaml` from the live installed path (no stubs)
- [ ] All fixtures pass against `next-ops.py suggest`
- [ ] Fixture for `expressplan-complete → /finalizeplan` specifically exercises the
  `auto_advance_to` field in lifecycle.yaml express track definition

**Given** a fixture defines `phase: expressplan-complete, track: express`,
**When** fixtures are executed against `next-ops.py suggest`,
**Then** the result matches `expected_recommendation: /finalizeplan` with `status=unblocked`.

---

### E2-S4 — Write paused-state and edge-case fixtures

**Type:** [new]
**Points:** 2

**Story:** As a Lens module maintainer, I want parity fixtures for paused-state and edge
cases (warnings-only, unknown phase, missing feature.yaml) so that `next-ops.py` handles
every possible routing input gracefully.

**Precondition (M1):** E2-S1 Dev Notes must record the selected paused-state behavior
before this story begins. Do not write the paused-state fixture until that decision is
recorded in E2-S1.

**Acceptance Criteria:**

- [ ] Paused-state fixture exists; behavior matches the decision in E2-S1
- [ ] Warnings-only fixture: feature has warnings but no blockers; `status=unblocked` with
  `warnings` populated
- [ ] Unknown-phase fixture: `status=fail`, descriptive message
- [ ] Missing-`feature.yaml` fixture: `status=fail`, descriptive message
- [ ] All fixtures pass against `next-ops.py suggest`

**Given** a fixture defines a paused-state feature per the behavior selected in E2-S1,
**When** fixtures are executed against `next-ops.py suggest`,
**Then** the result matches the expected `status` and `recommendation` for that behavior.

---

## Epic 3 — Delegation and Release Hardening

---

### E3-S1 — Implement pre-confirmed handoff in SKILL.md

**Type:** [new]
**Points:** 3

**Story:** As a Lens user, I want the `next` command to immediately load the recommended
phase skill when the recommendation is unblocked, without surfacing a second run-confirmation
prompt, so that `/next` is a seamless handoff conductor and not a two-step confirmation loop.

**Acceptance Criteria:**

- [ ] `bmad-lens-next/SKILL.md` invokes the target skill via `bmad-lens-bmad-skill --skill
  {recommended_skill}` when `status=unblocked`
- [ ] No second "Proceed?" or "Run [phase]?" confirmation prompt appears
- [ ] Handoff passes the resolved `feature_id` and any relevant context state as arguments
- [ ] Blocked recommendations do not invoke any downstream skill
- [ ] Behavior is verified against E1-S3 acceptance criteria (no inline routing, no writes)

**Given** `next-ops.py` returns `status=unblocked, recommendation=/finalizeplan`,
**When** the user runs `/next` with no extra input,
**Then** `bmad-lens-finalizeplan/SKILL.md` activates immediately with `feature_id` already
resolved and no second prompt.

**Given** `next-ops.py` returns `status=blocked`,
**When** the user runs `/next`,
**Then** the blocker list is displayed and no phase skill is loaded.

---

### E3-S2 — Add negative test for no-write behavior

**Type:** [new]
**Points:** 2

**Story:** As a Lens module maintainer, I want a test confirming that `next-ops.py suggest`
and the `bmad-lens-next/SKILL.md` conductor produce no file system writes, no governance
writes, and no control-doc writes so that `next` remains a strictly read-only routing
surface.

**Acceptance Criteria:**

- [ ] Test script (or test case in the existing test suite) asserts that running `next-ops.py
  suggest` with a valid and an invalid `feature.yaml` produces zero file system changes
- [ ] No governance-repo files are modified by `next-ops.py` invocation
- [ ] No control-repo files are modified by `next-ops.py` invocation
- [ ] SKILL.md is audited: confirm no `create_file`, `replace_string_in_file`, or similar
  write-capable tool calls appear in the skill instructions
- [ ] Test passes in CI

**Given** `next-ops.py suggest` is executed with a valid feature,
**When** the test inspects the file system before and after,
**Then** no files are created, modified, or deleted.

---

### E3-S3 — Document and resolve constitution resolver dependency

**Type:** [confirm/new]
**Points:** 2

**Story:** As a Lens module maintainer, I want the constitution resolver dependency on
`lens-dev-new-codebase-constitution` formally confirmed or scoped before Slice 4 closes
so that the express-track allow-list gap does not silently break constitution permission
checks for future express-track features.

**Acceptance Criteria (H1 gate):**

- [ ] Check governance state of `lens-dev-new-codebase-constitution`: has it reached
  at least `expressplan-complete`?
- [ ] If yes: document the confirmed state in Dev Notes and mark gate closed
- [ ] If no: confirm whether the express-track allow-list fix should be scoped into this
  feature's Slice 4 or remain as a dependency on the constitution feature
- [ ] Document the owner decision (this feature vs constitution feature) in Dev Notes
- [ ] If scoped in: write and test the allow-list fix in `next-ops.py` or the relevant
  resolver component
- [ ] `feature.yaml` `depends_on` is updated to reflect the outcome (add or remove
  `lens-dev-new-codebase-constitution`)

**Given** the constitution feature has reached `expressplan-complete`,
**When** this story is executed,
**Then** the gate is marked closed, Dev Notes record the confirmation, and `depends_on` is
updated accordingly.

**Given** the constitution feature has NOT yet reached `expressplan-complete`,
**When** this story is executed,
**Then** the owner decision is documented and the allow-list scope is confirmed before
marking this story done.

---

### E3-S4 — Confirm release readiness and update target_repos

**Type:** [confirm]
**Points:** 1

**Story:** As a Lens module maintainer, I want to confirm that all Slice 4 acceptance
criteria are met and that `feature.yaml` reflects the final release-ready state so that the
feature can be closed and its artifacts published to the governance mirror.

**Acceptance Criteria:**

- [ ] All stories E1-S1 through E3-S3 are marked done
- [ ] `feature.yaml` `target_repos` includes `lens.core.src`
- [ ] `next` command is discoverable and invokable in a clean session
- [ ] No outstanding high or medium open items in `implementation-readiness.md`
- [ ] Paused-state decision is recorded and fixtures pass
- [ ] Constitution resolver outcome is documented (E3-S3)
- [ ] No-write negative test passes (E3-S2)
- [ ] Announce readiness for governance publish via `/publish` or equivalent

**Given** all prior stories are complete and acceptance criteria met,
**When** this story is executed,
**Then** the feature is declared release-ready and the governance publish is triggered.
