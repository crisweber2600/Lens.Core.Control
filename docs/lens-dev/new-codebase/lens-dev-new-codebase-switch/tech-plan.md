---
feature: lens-dev-new-codebase-switch
doc_type: tech-plan
status: approved
goal: "Specify the clean-room technical design and parity checks for bmad-lens-switch"
key_decisions:
  - Keep the prompt controller thin and path-anchored; the script owns JSON operations.
  - Keep `switch-ops.py` deterministic and side-effect bounded to local context and optional control-repo checkout.
  - Treat `feature-index.yaml` as the list authority and `feature.yaml` as the switch-state authority.
  - Add parity validation around deprecated command-name references and menu no-inference behavior.
open_questions: []
depends_on: [lens-dev-new-codebase-baseline]
blocks: []
updated_at: 2026-04-27T00:00:00Z
---

# Tech Plan - Switch Command

## 1. Architecture Overview

`switch` has three layers:

| Layer | Responsibility |
|---|---|
| Published prompt stub | Run shared light preflight, stop on failure, then load the release-module prompt. |
| Release prompt | Resolve exact config and script paths, decide whether to run `list` or `switch`, own numbered-menu interaction, and load returned context files that exist. |
| `bmad-lens-switch` skill and `switch-ops.py` | Define the command contract and provide deterministic JSON operations for listing, switching, and context path derivation. |

The design keeps user interaction in the prompt layer and deterministic state inspection in the script. The script does not ask questions and does not infer a target. It returns structured data that the prompt can render or act on.

## 2. Data Sources

| Source | Use | Authority |
|---|---|---|
| `lens.core/_bmad/lens-work/bmadconfig.yaml` | Default release root, governance repo path, personal output folder. | Release module config. |
| `.lens/governance-setup.yaml` | Optional governance repo override. | Local control repo setup. |
| `TargetProjects/lens/lens-governance/feature-index.yaml` | Feature listing, menu numbers, owner/status/summary fields. | Governance main. |
| `features/{domain}/{service}/{featureId}/feature.yaml` | Full feature state for selected target. | Governance main. |
| `.lens/personal/context.yaml` | Local domain/service context persistence. | Local personal state. |
| Control repo git branches | Optional checkout to `{featureId}-plan`. | Control repo. |

## 3. Command Flows

### 3.1 List Flow

1. Load `feature-index.yaml` from the resolved governance repo.
2. If the file is missing, scan `features/` for `domain.yaml` and `service.yaml` and return `mode: domains`.
3. If the file exists but is malformed, return `status: fail`.
4. Apply `status-filter`:
   - default/`active`: all non-archived entries.
   - `all`: all entries.
   - `archived`: archived entries only.
5. Add stable 1-based `num` values.
6. When matching `feature.yaml` exists, include normalized target repo execution state.

### 3.2 Numbered Menu Flow

1. The prompt invokes `list` when no explicit feature id is supplied.
2. If response mode is `domains`, render the domain/service inventory and stop.
3. If response mode is `features`, render numbered options.
4. If a question tool is available, ask the user to choose; otherwise render the menu and stop.
5. On `q`, cancel without side effects.
6. On invalid input, rerender the same menu and stop.
7. On valid number, map to that response's `features[num - 1].id` and invoke switch.

No branch name, open file, recent path, previous answer, or current feature may be used as a substitute for explicit selection.

### 3.3 Switch Flow

1. Validate `feature_id`, optional `domain`, and optional `service` against the safe identifier pattern.
2. Resolve personal folder, defaulting to `{control_repo}/.lens/personal` when a control repo is supplied.
3. Load `feature-index.yaml`.
4. If no index exists, support service-context fallback only when an explicit service context can be resolved; otherwise fail.
5. Validate the feature id exists in the index before scanning for `feature.yaml`.
6. Prefer the indexed feature path, fall back to scanning for matching `featureId` only after index validation succeeds.
7. Load full feature state from `feature.yaml`.
8. Compute cross-feature context paths:
   - `related` -> `summary.md`
   - `depends_on` -> `tech-plan.md`
   - `blocks` -> `tech-plan.md`
9. Write `.lens/personal/context.yaml` with domain, service, timestamp, and `updated_by: lens-switch`.
10. Attempt `git checkout {featureId}-plan` in the control repo.
11. Return structured JSON with feature state, stale flag, branch result, target repo state, and context paths.

## 4. JSON Contracts

### 4.1 List Success

```json
{
  "status": "pass",
  "mode": "features",
  "features": [
    {
      "num": 1,
      "id": "lens-dev-new-codebase-switch",
      "domain": "lens-dev",
      "service": "new-codebase",
      "status": "preplan",
      "owner": "crisweber2600",
      "summary": "",
      "target_repo": null
    }
  ],
  "total": 1
}
```

### 4.2 Domain Fallback

```json
{
  "status": "pass",
  "mode": "domains",
  "domains": [],
  "total_domains": 0,
  "total_services": 0,
  "message": "No features initialized yet. Showing domain/service inventory from governance repo."
}
```

### 4.3 Switch Success

```json
{
  "status": "pass",
  "plan_branch": "lens-dev-new-codebase-switch-plan",
  "branch_switched": true,
  "feature": {
    "id": "lens-dev-new-codebase-switch",
    "name": "Switch Command",
    "domain": "lens-dev",
    "service": "new-codebase",
    "phase": "preplan",
    "track": "full",
    "priority": "medium",
    "status": "preplan",
    "owner": "crisweber2600",
    "stale": false,
    "updated": "2026-04-22T05:23:08Z",
    "context_path": ".lens/personal/context.yaml",
    "target_repo": null
  },
  "context_to_load": {
    "summaries": [],
    "full_docs": []
  }
}
```

## 5. Edge Cases

| Edge Case | Expected Behavior |
|---|---|
| Missing `feature-index.yaml` | `list` returns domain inventory; prompt stops. |
| Malformed `feature-index.yaml` | Return fail with parse error. |
| Feature absent from index | Return fail; do not search filesystem for a feature. |
| Indexed feature missing `feature.yaml` | Return fail with missing feature.yaml message. |
| Invalid feature id | Return fail before path construction. |
| Missing plan branch | Return `branch_switched: false` plus `branch_error`; do not guess fallback branch. |
| Stale `updated` timestamp | Return `stale: true`; prompt warns but does not block. |
| Missing returned context files | Caller skips missing files and may warn. |
| Target repo metadata absent | Return `target_repo: null`. |

## 6. Implementation Work Items

1. Keep the current prompt-start controller and path resolution rules anchored to the release module.
2. Keep `switch-ops.py` pure JSON for `list`, `switch`, and `context-paths`.
3. Replace deprecated user-facing references such as `/lens-init-feature` with the retained command name (`/new-feature` or the installed Lens prompt alias used by the release surface).
4. Add or preserve tests for:
   - non-archived default listing,
   - `all` and `archived` filters,
   - domain/service fallback,
   - numbered menu `num` fields,
   - existing feature switch,
   - context yaml write,
   - target repo state normalization,
   - invalid identifiers,
   - dependency context path derivation,
   - checkout success and failure reporting.
5. Add a no-write regression that hashes governance `feature.yaml` and `feature-index.yaml` before and after switch execution, allowing only `.lens/personal/context.yaml` and control-repo branch checkout as side effects.

## 7. Validation Plan

Focused validation for this feature:

```bash
uv run --script lens.core/_bmad/lens-work/skills/bmad-lens-switch/scripts/tests/test-switch-ops.py
uv run --script lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/scripts/quickplan-ops.py validate-frontmatter --file docs/lens-dev/new-codebase/lens-dev-new-codebase-switch/business-plan.md --doc-type business-plan
uv run --script lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/scripts/quickplan-ops.py validate-frontmatter --file docs/lens-dev/new-codebase/lens-dev-new-codebase-switch/tech-plan.md --doc-type tech-plan
uv run ./lens.core/_bmad/lens-work/scripts/validate-phase-artifacts.py --phase expressplan --contract review-ready --lifecycle-path ./lens.core/_bmad/lens-work/lifecycle.yaml --docs-root docs/lens-dev/new-codebase/lens-dev-new-codebase-switch --json
```

Broader rewrite validation should include the baseline retained-command tests listed in the parent feature docs, especially setup/preflight, next routing, init-feature, and git-orchestration regressions.
