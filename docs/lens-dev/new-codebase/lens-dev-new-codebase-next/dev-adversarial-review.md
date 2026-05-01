---
feature: lens-dev-new-codebase-next
doc_type: dev-adversarial-review
status: pass-with-notes
reviewed_at: 2026-05-01T14:48:51Z
branch: feature/lens-dev-new-codebase-next-clean-room
base: develop
target_repo: TargetProjects/lens-dev/new-codebase/lens.core.src.next-clean-room
pull_request: https://github.com/crisweber2600/Lens.Core.Src/pull/16
---

# Dev Adversarial Review - Next Command

## Verdict

`PASS_WITH_NOTES`

All twelve Next command stories are implemented or verified on the clean-room target branch. The final review found no remaining blocking issue after the hardening pass.

## Evidence

- Target branch: `feature/lens-dev-new-codebase-next-clean-room`, created from `origin/develop` in `TargetProjects/lens-dev/new-codebase/lens.core.src.next-clean-room`.
- Target PR: https://github.com/crisweber2600/Lens.Core.Src/pull/16 targeting `develop`.
- Previous implementation PR #13 was already merged into `develop`; this clean-room branch applies closeout fixes on top of current `develop`.
- Target prompt preflight succeeded from the target repo root using `_bmad/lens-work/skills/bmad-lens-preflight/scripts/light-preflight.py`.
- BMB workflow-builder path scan: pass, zero findings.
- BMB workflow-builder script scan: pass, zero findings.
- Python syntax check: pass for `next-ops.py`, `test_next-ops.py`, and `test_next_no_writes.py`.
- Focused tests: `uv run --with pytest --with pyyaml python -m pytest _bmad/lens-work/skills/bmad-lens-next/scripts/tests` -> 32 passed.

## Findings Addressed During Review

### A1 - Public prompt used source-repo `lens.core` paths

The public `lens-next.prompt.md` stub pointed at `./lens.core/_bmad/...`, which does not exist in the target repo. The stub now follows retained-command target repo convention and uses root `_bmad/...` paths.

Status: addressed.

### A2 - BMB script scan did not recognize direct tests for `next-ops.py`

The existing test suite was broad but did not include a filename matching the BMB scanner's script-test heuristic. Added `scripts/tests/test_next-ops.py` with direct unit coverage for the script helper contract.

Status: addressed.

### A3 - Discovery metadata drift

The discovery surface contained a duplicate `lens-expressplan.prompt.md` prompt entry and the `next-action` help row used outdated `--governance-dir` wording. Removed the duplicate prompt and aligned the help args with `next-ops.py` (`--feature-id`, `--governance-repo`).

Status: addressed.

### A4 - YAML shape and dependency edge cases

Adversarial review flagged malformed `warnings`, malformed `dependencies`, malformed `depends_on`, non-mapping lifecycle YAML, invalid feature IDs, and missing dependency records. `next-ops.py` now fails cleanly for malformed inputs and reports missing dependency feature records as blockers instead of silently skipping them. Regression tests cover the new behavior.

Status: addressed.

## Non-Blocking Notes

- `bmad-lens-next/SKILL.md` is a BMad conductor skill, so it remains instruction-driven rather than a standalone executable wrapper. That is consistent with the module's skill architecture.
- The constitution feature dependency remains recorded in governance. `next-ops.py` now blocks when declared dependency records are missing or incomplete.
- Broad module-wide tests were not rerun for the entire Lens payload; focused Next tests and BMB validations passed.

## Decision

Proceed with PR #16. No Next-specific blocker remains after closeout hardening.
