---
feature: lens-dev-new-codebase-discover
doc_type: arch-review-note
status: draft
updated_at: 2026-04-30T00:00:00Z
---

# Architecture Isolation Audit - Discover Auto-Commit Exception

## Verdict

`ISOLATED`

The discover auto-commit exception is isolated to `bmad-lens-discover/SKILL.md` in this target branch. No other `SKILL.md` file contains a direct governance-main `git commit` or `git push` sequence under the audited patterns.

## Method

Searched all tracked `SKILL.md` files under `_bmad/lens-work/skills/` in the target worktree after merging the latest `origin/develop` into `feature/lens-dev-new-codebase-discover`. The audit looked for direct `git push`, `git commit`, and governance-main commit references.

Patterns reviewed:

- `git push`
- `git commit`
- `push origin main`
- `commit -m`
- `governance main`

## Files Searched

Count: 11

- `_bmad/lens-work/skills/bmad-lens-adversarial-review/SKILL.md`
- `_bmad/lens-work/skills/bmad-lens-batch/SKILL.md`
- `_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md`
- `_bmad/lens-work/skills/bmad-lens-businessplan/SKILL.md`
- `_bmad/lens-work/skills/bmad-lens-constitution/SKILL.md`
- `_bmad/lens-work/skills/bmad-lens-discover/SKILL.md`
- `_bmad/lens-work/skills/bmad-lens-git-orchestration/SKILL.md`
- `_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md`
- `_bmad/lens-work/skills/bmad-lens-preplan/SKILL.md`
- `_bmad/lens-work/skills/bmad-lens-switch/SKILL.md`
- `_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md`

`bmad-lens-git-orchestration` is present after the develop merge and is evaluated as the expected centralized git-write workflow. It contains generic push guidance but no direct discover-style governance-main commit exception.

## Findings

### Finding 1 - Expected Exception: discover

- File: `_bmad/lens-work/skills/bmad-lens-discover/SKILL.md` lines 134 and 147
- Pattern: direct `git add`, `git commit`, and `git push` for `repo-inventory.yaml`
- Assessment: Expected. The section is labelled `## Auto-Commit (Governance-Main Exception)`, uses pre/post SHA-256 hashes, commits only when hashes differ, and uses the required commit message `[discover] Sync repo-inventory.yaml`.
- Recommended follow-up: Keep this section unique and rerun this audit before dev-complete.

### Finding 2 - Expected Centralized Git Workflow: git-orchestration

- File: `_bmad/lens-work/skills/bmad-lens-git-orchestration/SKILL.md`
- Pattern: generic `git push` guidance and governance main-only description
- Assessment: Expected. This skill is the centralized git-write workflow and does not contain a direct governance-main `repo-inventory.yaml` auto-commit sequence.
- Recommended follow-up: None for this discover story.

## Non-Findings

- `_bmad/lens-work/skills/bmad-lens-adversarial-review/SKILL.md` was searched and did not contain direct `git commit` or `git push` command references under the audited patterns.
- `_bmad/lens-work/skills/bmad-lens-batch/SKILL.md` was searched and did not contain direct `git commit` or `git push` command references under the audited patterns.
- `_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md` was searched and did not contain direct `git commit` or `git push` command references under the audited patterns.
- `_bmad/lens-work/skills/bmad-lens-businessplan/SKILL.md` was searched and did not contain direct `git commit` or `git push` command references under the audited patterns.
- `_bmad/lens-work/skills/bmad-lens-constitution/SKILL.md` was searched and did not contain direct `git commit` or `git push` command references under the audited patterns.
- `_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md` was searched and did not contain direct `git commit` or `git push` command references under the audited patterns.
- `_bmad/lens-work/skills/bmad-lens-preplan/SKILL.md` was searched and did not contain direct `git commit` or `git push` command references under the audited patterns.
- `_bmad/lens-work/skills/bmad-lens-switch/SKILL.md` was searched and did not contain direct `git commit` or `git push` command references under the audited patterns.
- `_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md` was searched and did not contain direct `git commit` or `git push` command references under the audited patterns.

## Sign-Off

Audit complete for the discover sprint DoD. The discover exception is documented and test-covered; no non-discover direct governance-main commit pattern was found in the current target branch.
