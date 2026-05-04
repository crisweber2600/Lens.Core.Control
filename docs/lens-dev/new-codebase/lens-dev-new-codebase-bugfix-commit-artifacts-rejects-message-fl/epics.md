---
feature: lens-dev-new-codebase-bugfix-commit-artifacts-rejects-message-fl
doc_type: epics
status: approved
track: express
domain: lens-dev
service: new-codebase
depends_on: []
blocks: []
key_decisions:
  - Four epics aligned to sprint stories S1–S4
  - S4 spans two repos — source tooling in lens.core.src and AGENTS.md in control repo
  - All code changes go to TargetProjects/lens-dev/new-codebase/lens.core.src (except AGENTS.md)
open_questions: []
updated_at: '2026-05-04T02:20:00Z'
stepsCompleted: [epics-and-stories]
---

# Epics — Script Errors and On-the-fly Workflow Scripts

## Epic 1: Feature YAML Command Interface Compatibility

**Goal:** Add backwards-compatible aliases (`set-phase`, `--field phase --value`) to
`feature-yaml-ops.py` so lifecycle conductors can use the command forms observed in
real workflow runs without bypassing existing transition validation.

**Value:** Eliminates conductor retries caused by missing or undocumented command shapes and
prevents follow-on drift where conductors substitute ad hoc Python snippets.

---

### Story 1.1 — Add `set-phase` alias and `--field phase --value` compatibility

**As a** Lens lifecycle conductor  
**I want** `feature-yaml-ops.py` to accept `set-phase <phase>` and `--field phase --value <phase>`  
**So that** the command forms observed in finalize-flow history work without modification

**Acceptance Criteria:**

- [ ] `feature-yaml-ops.py set-phase <phase>` calls the same transition validator as `update --phase <phase>`
- [ ] `feature-yaml-ops.py update --field phase --value <phase>` routes to the same transition validator
- [ ] Unsupported `--field` names produce a structured error listing supported fields
- [ ] Tests cover: valid `set-phase`, valid `--field phase --value`, invalid field rejection, invalid phase rejection
- [ ] No duplicate validation logic introduced — aliases delegate to existing internal functions

**Story Points:** 3  
**Dependencies:** None

---

## Epic 2: Git Orchestration Command Compatibility and PR Body Handling

**Goal:** Harden `git-orchestration-ops.py` so that `commit-artifacts`, `create-pr`, and PR
body generation work with the command forms conductors have used, without requiring temporary
shell heredocs or unsupported flag retries.

**Value:** Removes the most frequent class of lifecycle command failures: unsupported flag
rejections and PR body heredoc workarounds.

---

### Story 2.1 — Add `--message` alias, branch aliases, and PR body handling

**As a** Lens lifecycle conductor  
**I want** `git-orchestration-ops.py` to accept `commit-artifacts --message`, `create-pr --source-branch/--target-branch`, and a direct `--body` string  
**So that** I can run these commands without flag retries or temporary shell heredocs

**Acceptance Criteria:**

- [ ] `commit-artifacts --message <msg>` is accepted as a compatibility alias for `--description`
- [ ] `create-pr --source-branch <branch>` maps to `--head <branch>` without ambiguity
- [ ] `create-pr --target-branch <branch>` maps to `--base <branch>` without ambiguity
- [ ] `create-pr --body <string>` is supported without requiring a temporary file
- [ ] Tests cover: `--message` alias, `--source-branch`/`--target-branch` alias, `--body` string
- [ ] Aliases delegate to the existing command implementation; no duplicate logic

**Story Points:** 3  
**Dependencies:** None

---

## Epic 3: Preflight Contract Alignment

**Goal:** Reconcile `light-preflight.py` behavior and `lens-preflight/SKILL.md` documentation
so they describe and implement the same contract, including argument handling and root detection.

**Value:** Prevents conductor-level preflight failures caused by undocumented argument rejection
and inconsistent working-directory guidance.

---

### Story 3.1 — Align preflight behavior, docs, and argument contract

**As a** Lens lifecycle conductor  
**I want** `light-preflight.py` and `SKILL.md` to agree on supported arguments and invocation paths  
**So that** I can invoke preflight with documented flags from either workspace root or source root

**Acceptance Criteria:**

- [ ] Decision encoded: Option A (lightweight gate, docs corrected) or Option B (full-sync delegation restored)
- [ ] If Option B: `light-preflight.py` accepts `--caller` and `--governance-path` arguments and delegates to `preflight.py`
- [ ] If Option B: `--caller` and `--governance-path` are listed in `SKILL.md` as supported invocation arguments
- [ ] Script invocation examples in docs use `lens.core/...` for workspace-root and `_bmad/...` for source-root contexts consistently
- [ ] Tests cover: root detection from workspace root, root detection from source-repo root, documented argument acceptance/rejection

**Story Points:** 2  
**Dependencies:** None

---

## Epic 4: Durable Lifecycle Helpers and Terminal-Restriction Guidance

**Goal:** Replace recurring one-off Python/PowerShell snippets with repo-owned Lens script
commands for lifecycle state inspection and prompt file repair, and update `AGENTS.md`
to prohibit ad hoc alternatives.

**Value:** Makes lifecycle state inspection and prompt repair reproducible, reviewable, and
conductor-agnostic rather than session-specific.

---

### Story 4.1 — Lifecycle state inspection script and prompt repair helper

**As a** Lens conductor  
**I want** a Lens script command for lifecycle state inspection and a documented helper for prompt repair  
**So that** I never need to write ad hoc Python one-liners or PowerShell heredocs for recurring operations

**Acceptance Criteria (source-repo, `lens.core.src`):**

- [ ] A Lens script command prints feature phase, track, target repos, docs path, and PR links without requiring inline Python
- [ ] A repo-owned prompt normalization helper exists for newline/path repair, or the repair pattern is explicitly prohibited in favour of an existing tool
- [ ] Tests exist for prompt repair (fixture with literal `\r\n` tokens) if a helper is added

**Acceptance Criteria (control-repo, `AGENTS.md`):**

- [ ] `AGENTS.md` terminal-error guidance prohibits PowerShell bulk prompt replacements
- [ ] `AGENTS.md` terminal-error guidance directs conductors to use repo-owned scripts for lifecycle state checks
- [ ] The `AGENTS.md` change is committed and PR'd against the control repo separately from the `lens.core.src` feature branch

**Story Points:** 3  
**Dependencies:** None
