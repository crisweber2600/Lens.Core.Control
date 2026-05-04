---
feature: lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be
doc_type: tech-plan
status: draft
track: express
updated_at: "2026-05-04T00:08:00Z"
depends_on: []
blocks: []
key_decisions:
  - "B3 (step3 routing): git-orchestration-ops.py step3 route changed from {featureId}-dev to {featureId} — confirmed correct per FinalizePlan SKILL.md line 99"
  - "B4 (pull-request flag): Add --pull-request to update subparser in feature-yaml-ops.py and wire it to set links.pull_request in feature.yaml"
  - "B2 (PowerShell): Prohibit PowerShell heredoc for multi-file text replacement; use Python with explicit encoding"
  - "B6 (branch mismatch): Hard error with structured message — NO --allow-branch-mismatch bypass flag"
  - "B5 (gh pr): Promote from docs-only fix to code fix in git-orchestration-ops.py create-pr (add merge-base check)"
open_questions: []
---

# Tech Plan — Environment, Orchestration and Tooling Fixes

## Architecture Overview

All fixes are self-contained, single-file changes (or AGENTS.md additions) in the source repo. No new files are created. No shared interfaces are changed beyond the `feature-yaml-ops.py` CLI surface addition (additive only, backward-compatible).

| Bug | File to Change | Change Type |
|-----|----------------|-------------|
| B1 — rg not available | `AGENTS.md` | Documentation / wording fix |
| B2 — PowerShell \r\n | Agent skill instruction + AGENTS.md | Instruction fix |
| B3 — step3 wrong branch | `skills/lens-git-orchestration/scripts/git-orchestration-ops.py` | Logic fix |
| B4 — missing --pull-request | `skills/lens-feature-yaml/scripts/feature-yaml-ops.py` | CLI addition |
| B5 — gh pr no history check | `AGENTS.md` + `skills/lens-git-orchestration/scripts/git-orchestration-ops.py` | Documentation + code fix |
| B6 — no branch mismatch warning | `skills/lens-git-orchestration/scripts/git-orchestration-ops.py` | Behavior addition |

---

## B1 — rg command unavailable

**Root cause:** `AGENTS.md` documents `rg is not a command.` but the phrasing is easily missed (bare text, not a formatted rule block). The agent picks `rg` by default before falling back to `grep`.

**Fix:** Reformat the AGENTS.md `Common Terminal Errors & Fixes` section to match the documented example format, making the `rg` unavailability prominent:
```markdown
**Error**: `rg: command not found` (or similar)
**Cause**: `ripgrep (rg) is not installed in this environment`
**Fix**: Use `grep` instead of `rg` for all text searches
```

**Testing:** None required (documentation fix). Verify manually that agent no longer invokes `rg` after the AGENTS.md update is applied.

---

## B2 — PowerShell bulk-replace injects `\r\n`

**Root cause:** When PowerShell `-Command` heredocs or `$ExecutionContext.InvokeCommand` expand replacement strings containing `\r\n`, they treat them as two-char literal tokens, not as actual CR+LF. Files get corrupted with literal `\r\n` text.

**Fix:** Add an explicit AGENTS.md rule prohibiting PowerShell for multi-file text replacement when the replacement string contains escape sequences. Rule: use Python with `pathlib` + `str.replace()` or `re.sub()` and explicit `encoding="utf-8"` for all bulk prompt-file replacement operations.

**AGENTS.md addition:**
```markdown
**Error**: Prompt files contain literal `\r\n` text after bulk replace
**Cause**: PowerShell heredoc replacement does not expand `\r\n` as newlines
**Fix**: Use Python for multi-file text replacement — never PowerShell `-Command` with regex replacements on prompt files
```

**Testing:** No code change; documentation fix. Python replacement pattern:
```python
from pathlib import Path
for p in Path(".github/prompts").glob("*.prompt.md"):
    content = p.read_text(encoding="utf-8")
    content = content.replace("OLD", "NEW")
    p.write_text(content, encoding="utf-8")
```

---

## B3 — commit-artifacts step3 wrong branch (HIGH)

**Root cause:** `branch_for_phase_write()` in `git-orchestration-ops.py` maps `finalizeplan step3` to `{feature_id}-dev`. The FinalizePlan SKILL.md step3 contract specifies the downstream bundle must be committed on the `{featureId}` branch (main feature branch, no suffix). The `-dev` routing is wrong for this step.

**Current routing (line 199):**
```python
if normalized_step in {"step3", "3", "finalizeplan-step3"}:
    return f"{feature_id}-dev", "finalizeplan_step_3_to_dev"
```

**Fix:** Change step3 to route to `feature_id` (no suffix) with an updated rule name:
```python
if normalized_step in {"step3", "3", "finalizeplan-step3"}:
    return feature_id, "finalizeplan_step_3_to_feature"
```

**Verify FinalizePlan contract:** Load `lens-finalizeplan` SKILL.md and confirm step3 target branch before applying.

**Impact:** Only affects `commit-artifacts` calls with `--phase finalizeplan --phase-step step3`. Step1 and step2 routing unchanged.

**Testing:**
- Unit test: `branch_for_phase_write("feat-abc", "finalizeplan", "step3")` → `("feat-abc", "finalizeplan_step_3_to_feature")`
- Unit test: `branch_for_phase_write("feat-abc", "finalizeplan", "step2")` → `("feat-abc-plan", ...)`  still passes

---

## B4 — feature-yaml-ops.py missing --pull-request

**Root cause:** `feature.yaml` schema has `links.pull_request` but the `update` subparser only exposes `--phase`, `--docs-path`, `--governance-docs-path`, `--target-repos`, and `--milestones`. There is no `--pull-request` argument.

**Fix:** Add `--pull-request` to the `update_parser` (line ~557 in `feature-yaml-ops.py`) and handle it in the update logic to set `links.pull_request` in the YAML.

**Addition to update_parser:**
```python
update_parser.add_argument("--pull-request", required=False, help="PR URL to set in feature.yaml links.pull_request")
```

**Handler:** In the `update` command body, check `if args.pull_request: feature_data["links"]["pull_request"] = args.pull_request`

**Backward compatibility:** Additive only. Existing callers unaffected.

**Testing:**
- `feature-yaml-ops.py update --feature-id X --pull-request https://github.com/... --governance-repo Y` → sets `links.pull_request` in feature.yaml

---

## B5 — gh pr create targets main without history check

**Root cause:** The agent (or skill instruction) runs `gh pr create --base main` without first verifying that the current branch shares history with `main`. When the branch was created from `develop`, there is no common ancestor with `main`, causing GitHub to reject the PR.

**Fix:** Add a `create-pr` subcommand enhancement to `git-orchestration-ops.py` that performs a merge-base timestamp comparison before selecting the base branch. Also add an AGENTS.md rule documenting the expected behavior.

**AGENTS.md addition:**
```markdown
**Error**: `gh pr create` fails with no common ancestor / no shared history
**Cause**: Branch was created from `develop` (or another non-main base) but `--base main` was passed
**Fix**: Use the merge-base timestamp comparison in `create-pr` of `git-orchestration-ops.py`; do not call `gh pr create --base main` directly
```

**Algorithm (code fix in `create-pr`):**
```python
import subprocess, sys, json

def pick_base_branch(candidates=("main", "develop")):
    """Return the candidate branch whose merge-base with HEAD is most recent."""
    best_branch, best_ts = None, -1
    for branch in candidates:
        mb = subprocess.run(
            ["git", "merge-base", "HEAD", branch],
            capture_output=True, text=True
        )
        if mb.returncode != 0 or not mb.stdout.strip():
            continue
        sha = mb.stdout.strip()
        ts_out = subprocess.run(
            ["git", "log", "-1", "--format=%ct", sha],
            capture_output=True, text=True
        )
        ts = int(ts_out.stdout.strip()) if ts_out.stdout.strip() else -1
        if ts > best_ts:
            best_ts, best_branch = ts, branch
    if best_branch is None:
        print(json.dumps({"status": "error", "error": "no_common_ancestor",
                          "detail": "No shared history found with any candidate base branch."}))
        sys.exit(1)
    return best_branch
```

This correctly handles the case where a branch was created from `develop`: the merge-base with `develop` will have a more recent commit timestamp than the merge-base with `main`, so `develop` will be selected.

**Testing:** The algorithm can be unit-tested by mocking `subprocess.run` responses for the two `git merge-base` and `git log` calls.

---

## B6 — No branch mismatch warning

**Root cause:** `commit_artifacts` in `git-orchestration-ops.py` resolves an expected branch via `branch_for_phase_write()` and compares it to the current branch. When they differ, it currently returns a generic hard error without a clear, actionable description of the mismatch.

**Revised design (from expressplan review response):** Keep the behavior as a hard error (non-zero exit), but replace the current generic error message with a structured error that includes the current branch, expected branch, and an explicit instruction for the user. Do NOT add a `--allow-branch-mismatch` bypass flag — that flag creates a silent-bypass risk and was rejected in the expressplan adversarial review.

**Fix:** Update the branch-mismatch error path to emit:
```json
{
  "status": "error",
  "error": "branch_mismatch",
  "current_branch": "...",
  "expected_branch": "...",
  "detail": "Phase '{phase}' step '{step}' requires branch '{expected}'. Currently on '{current}'. Checkout '{expected}' before committing."
}
```

**Files changed:** `git-orchestration-ops.py` (update error message format; no new flags)

**Testing:**
- Unit test: resolve step3 to `feat-abc`, check on `feat-abc-plan` → returns `status: error`, `error: branch_mismatch`, with both branch names in the detail
- Acceptance: `commit-artifacts --phase finalizeplan --phase-step step3` on wrong branch exits non-zero with the structured message

---

## Data / Artifact Contracts

- `feature.yaml` schema: `links.pull_request` field already present; no schema change
- `git-orchestration-ops.py` JSON output: existing `status: error` shape preserved and extended with `error`, `current_branch`, `expected_branch`, and `detail` fields for branch mismatch; no new `status: warn` shape
- `AGENTS.md`: 3 new formatted error/fix blocks added to `Common Terminal Errors & Fixes` section

## Rollout

All changes are in the source repo `feature/lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be` branch, targeting `develop`.
Single PR covering all 6 bug fixes; changes are independent and can be reviewed per-file.
