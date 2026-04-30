---
feature: lens-dev-new-codebase-new-feature
doc_type: epics
status: approved
goal: "Break new-feature command parity into four delivery epics aligned to sprint sequencing"
key_decisions:
  - Four epics map to the four sprint goals; each epic owns one functional layer.
  - Epic 1 must complete before any Epic 2 implementation starts.
  - Epic 2 builders must complete before Epic 3 git-handoff work starts.
  - Epic 4 fetch-context (NF-4.3) is sequentially dependent on Epic 4 governance git execution (NF-4.1).
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
  - lens-dev-new-codebase-new-service
blocks: []
updated_at: 2026-04-27T15:30:00Z
---

# Epics — New Feature Command

## Epic 1: Command Surface and Parity Foundation

**Goal:** Make `new-feature` visible in the new codebase as a planned command and lock down parity expectations before any implementation begins.

**Acceptance Definition:** The `new-feature` command surface exists (installed stub + release prompt), the skill contract documents the full interaction flow including fetch-context, and parity test skeletons are red-failing for all required behaviors.

**Scope:**
- Installed prompt stub at `.github/prompts/lens-new-feature.prompt.md` and `lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md`
- Release prompt loading `bmad-lens-init-feature/SKILL.md`
- SKILL.md updated with new-feature progressive disclosure contract, fetch-context reference, and `create` invocation
- Test skeleton file covering: `create`, full-track start phase, express PR deferral, invalid IDs, duplicate feature, dry-run no-write, fetch-context failure modes

**Stories:**
- NF-1.1: Add command prompt surfaces (M)
- NF-1.2: Expand skill contract for new-feature (M)
- NF-1.3: Add parity test skeletons (S)

**Dependencies:** None (starting point)

**Blocks:** Epic 2 (parity tests must exist before implementation)

**Risks:**
- Path resolution must stay relative to `lens.core/` in installed use
- Test expectations may expose missing shared helpers not yet ported to new codebase
- SKILL.md must accurately promise fetch-context since it is now in scope

---

## Epic 2: Script-Level Feature Creation

**Goal:** Implement clean-room governance feature creation and core data model parity in `init-feature-ops.py create`.

**Acceptance Definition:** `init-feature-ops.py create` produces governance files (feature.yaml, summary.md, feature-index.yaml entry, domain/service markers) matching the old codebase schema exactly. Dry-run returns planned paths and command groups. Duplicate and validation gates fire before any filesystem write. Parity tests from Epic 1 pass green.

**Scope:**
- `cmd_create` and supporting builder functions in `init-feature-ops.py`
- Canonical ID resolution: `{normalized-domain}-{normalized-service}-{featureSlug}`
- `featureSlug` stored separately in feature.yaml and feature-index.yaml entry
- v4 feature schema compliance (all required fields)
- Duplicate detection against `feature-index.yaml` before writes
- Validation gates for invalid domain/service/slug before writes
- Missing `--track` returns explicit error
- Dry-run path: returns planned paths and command groups, creates no files

**Stories:**
- NF-2.1: Implement identity and feature data builders (L)
- NF-2.2: Implement duplicate and validation gates (M)
- NF-2.3: Preserve dry-run behavior (M)

**Dependencies:** Epic 1 (NF-1.3 parity tests must exist and be red before NF-2.1 starts)

**Blocks:** Epic 3 (data builders required before git handoff)

**Risks:**
- Feature ID drift between old and new codebase is the highest-risk compatibility failure
- Governance write ordering must be atomic — partial writes after validation failure leave dirty state
- Shared helpers (safe ID validation, YAML write utilities) from create-domain must be reused, not reimplemented

---

## Epic 3: Git and Lifecycle Handoff Parity

**Goal:** Restore git command delegation and lifecycle start-phase routing so `new-feature` returns the correct branch creation, activation, and planning PR commands.

**Acceptance Definition:** `init-feature-ops.py create` returns `control_repo_git_commands` routing through `bmad-lens-git-orchestration create-feature-branches`, activation routing through `bmad-lens-switch`, and planning PR commands for non-express tracks. Express track sets `planning_pr_created: false`. Start phase resolves from `lifecycle.yaml` for all tracks including `quickplan` alias.

**Scope:**
- `tracks.{track}.start_phase` resolved from lifecycle.yaml — no hardcoded routing
- `control_repo_git_commands` generated via git-orchestration — no raw `git checkout -b`
- `control_repo_activation_commands` generated via switch — includes `--personal-folder` when configured
- `gh_commands` populated for non-express tracks; empty for express
- `planning_pr_created` boolean reflecting track behavior
- `recommended_command` and `router_command` (/next) returned
- `remaining_git_commands` and `remaining_commands` populated correctly

**Stories:**
- NF-3.1: Add lifecycle start-phase routing (M)
- NF-3.2: Generate control-repo and activation commands (M)
- NF-3.3: Implement track-aware PR command behavior (S)

**Dependencies:** Epic 2 (NF-2.1 identity/data builders must be complete)

**Blocks:** Epic 4 (NF-3.2 command group generation blocks NF-4.1 governance git execution)

**Risks:**
- Manual `git checkout -b` commands would regress the default-branch safety contract
- Express track regression would generate empty planning PRs
- Track alias handling (quickplan → feature) must be deliberate

---

## Epic 4: Context and Release-Surface Parity

**Goal:** Deliver governance git execution, help/manifest alignment, and `fetch-context` subcommand with full parity to the old codebase.

**Acceptance Definition:** `--execute-governance-git` auto-commits and pushes governance artifacts on clean repos; dirty repos fail before writes. `fetch-context` subcommand returns all required output fields (`related`, `depends_on`, `blocks`, `service_refs`, `detected_service_refs`, `missing_service_refs`, `context_paths`, `service_context_paths`) with path-resolution logic matching old codebase exactly. All fetch-context test cases pass. Help/manifests include `new-feature` if this feature owns surface registration.

**Scope:**
- `--execute-governance-git` integration: preflight → write → git add/commit/push → return `governance_commit_sha`
- Dirty repo detection before any file write
- `cmd_fetch_context` in `init-feature-ops.py` — full parity implementation
  - `--depth summaries`: related features → `summary.md` paths
  - `--depth full`: related features → `feature.yaml` + docs paths
  - `depends_on` and `blocks` always full depth regardless of `--depth`
  - `--service-ref`: explicit service names → `service_context_paths`
  - `--service-ref-text`: freeform text detection via `detect_service_refs_from_texts`
  - Missing service refs → `missing_service_refs` (not an error)
- Module help CSV and prompt manifests updated with `new-feature` entry (if owned by this feature)

**Stories:**
- NF-4.1: Implement governance git execution (M)
- NF-4.2: Align help/manifests if owned here (S)
- NF-4.3: Implement fetch-context with full parity (L)

**Dependencies:** Epic 3 (NF-3.2 command group generation blocks NF-4.1; NF-4.1 governance git execution blocks NF-4.3)

**Blocks:** None (final epic)

**Risks:**
- Sprint 4 is the heaviest sprint — three stories, all with hard Epic 3 dependencies
- fetch-context path-resolution logic must exactly match `collect_feature_context_paths`, `collect_service_context_paths`, `available_service_names`, and `detect_service_refs_from_texts` from old codebase
- NF-4.2 may be owned by the broader 17-command sweep; if so, NF-4.2 becomes a backlog item and Sprint 4 is reduced

---

## Cross-Epic Dependency Map

```
Epic 1 ──► Epic 2 ──► Epic 3 ──► Epic 4
NF-1.3     NF-2.1     NF-3.2     NF-4.1
(tests)    (builders) (cmd grps) (gov git)
                                    │
                                    ▼
                                 NF-4.3
                              (fetch-ctx)
```

## Total Complexity

| Epic | Stories | S | M | L | Sprint |
|---|---|---|---|---|---|
| 1 | NF-1.1, NF-1.2, NF-1.3 | 1 | 2 | 0 | 1 |
| 2 | NF-2.1, NF-2.2, NF-2.3 | 0 | 2 | 1 | 2 |
| 3 | NF-3.1, NF-3.2, NF-3.3 | 1 | 2 | 0 | 3 |
| 4 | NF-4.1, NF-4.2, NF-4.3 | 1 | 1 | 1 | 4 |
| **Total** | **12** | **3** | **7** | **2** | — |
