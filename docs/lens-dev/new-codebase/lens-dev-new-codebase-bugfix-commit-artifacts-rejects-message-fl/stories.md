---
feature: lens-dev-new-codebase-bugfix-commit-artifacts-rejects-message-fl
doc_type: stories
status: approved
track: express
domain: lens-dev
service: new-codebase
depends_on: []
blocks: []
key_decisions:
  - Four stories aligned to sprint stories S1–S4; no new scope added
  - S4 targets two repos — source tooling (lens.core.src) and control-repo AGENTS.md
  - Aliases must delegate to existing validators; no duplicate logic
open_questions: []
updated_at: '2026-05-04T02:20:00Z'
---

# Stories — Script Errors and On-the-fly Workflow Scripts

## Story 1.1 — Feature YAML command aliases

| Field | Value |
|---|---|
| ID | 1.1 |
| Epic | 1 |
| Points | 3 |
| Status | Ready |
| Dependencies | None |

**As a** Lens lifecycle conductor  
**I want** `feature-yaml-ops.py` to accept `set-phase <phase>` and `--field phase --value <phase>`  
**So that** finalize-flow command forms work without conductor retries

### Acceptance Criteria

- [ ] `set-phase <phase>` calls the same transition validator as `update --phase <phase>`
- [ ] `--field phase --value <phase>` routes to the same transition validator
- [ ] Unsupported `--field` names produce a structured error listing supported fields
- [ ] Tests cover valid aliases, invalid field rejection, and invalid phase rejection
- [ ] No duplicate validation logic — aliases delegate to existing internal functions

### Implementation Notes

Target files in `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-feature-yaml/scripts/feature-yaml-ops.py`.

---

## Story 2.1 — Git orchestration command compatibility and PR body handling

| Field | Value |
|---|---|
| ID | 2.1 |
| Epic | 2 |
| Points | 3 |
| Status | Ready |
| Dependencies | None |

**As a** Lens lifecycle conductor  
**I want** `git-orchestration-ops.py` to accept `commit-artifacts --message`, branch aliases, and `--body`  
**So that** PR operations complete without unsupported-flag retries or temporary shell heredocs

### Acceptance Criteria

- [ ] `commit-artifacts --message <msg>` accepted as alias for `--description`
- [ ] `create-pr --source-branch <branch>` maps to `--head <branch>`
- [ ] `create-pr --target-branch <branch>` maps to `--base <branch>`
- [ ] `create-pr --body <string>` supported without requiring a temporary file
- [ ] Tests cover all aliases and PR body handling
- [ ] Aliases delegate to existing implementation; no duplicate logic

### Implementation Notes

Target files in `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-git-orchestration/scripts/git-orchestration-ops.py`.

---

## Story 3.1 — Preflight contract alignment

| Field | Value |
|---|---|
| ID | 3.1 |
| Epic | 3 |
| Points | 2 |
| Status | Ready |
| Dependencies | None |

**As a** Lens lifecycle conductor  
**I want** `light-preflight.py` and `SKILL.md` to agree on supported arguments and invocation paths  
**So that** I can invoke preflight correctly from workspace root or source root without trial and error

### Acceptance Criteria

- [ ] Implementation decision encoded: Option A (lightweight gate + corrected docs) or Option B (full-sync delegation restored)
- [ ] If Option B: `light-preflight.py` accepts `--caller` and `--governance-path` and delegates to `preflight.py`
- [ ] `SKILL.md` documents supported arguments matching actual script behavior
- [ ] Script invocation examples use consistent root-relative paths
- [ ] Tests cover root detection from workspace root and source-repo root, and documented argument acceptance/rejection

### Implementation Notes

Target files in `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/`. Make the Option A/B decision explicit in the story AC so the dev agent cannot ship a `light-preflight.py` that rejects documented arguments.

---

## Story 4.1 — Durable lifecycle helpers and AGENTS.md terminal guidance

| Field | Value |
|---|---|
| ID | 4.1 |
| Epic | 4 |
| Points | 3 |
| Status | Ready |
| Dependencies | None |

**As a** Lens conductor  
**I want** repo-owned scripts for lifecycle state inspection and prompt repair, and clear AGENTS.md guidance  
**So that** I never need to write ad hoc Python or PowerShell snippets for recurring lifecycle operations

### Acceptance Criteria (source-repo changes — `lens.core.src`)

- [ ] A Lens script command prints feature phase, track, target repos, docs path, and PR links
- [ ] A repo-owned prompt normalization helper exists, or the repair pattern is explicitly prohibited in favour of an existing tool
- [ ] If a prompt repair helper is added: fixture-based tests exist for literal `\r\n` token repair

### Acceptance Criteria (control-repo change — `AGENTS.md`)

- [ ] `AGENTS.md` terminal-error guidance prohibits PowerShell bulk prompt replacements
- [ ] `AGENTS.md` terminal-error guidance directs conductors to repo-owned scripts for lifecycle state checks
- [ ] The `AGENTS.md` change is committed and PR'd against the control repo separately from the `lens.core.src` feature branch

### Implementation Notes

The `lens.core.src` changes go in `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/scripts/`. The `AGENTS.md` change is in the control repo root and must be landed via a separate control-repo PR.
