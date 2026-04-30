---
feature: lens-dev-new-codebase-dev
doc_type: business-plan
status: draft
goal: "Rewrite the dev conductor to preserve target-repo-only code writes, per-task commits, resumable checkpoints, and a final PR across the lens-work rewrite."
key_decisions:
  - "Express track: single focused story (5.1) with three clearly bounded acceptance criteria makes this appropriate for expressplan."
  - "Publish-before-author entry hook is non-negotiable: dev must not start if finalizeplan artifacts are absent or the feature phase is not finalizeplan-complete."
  - "dev-session.yaml schema must remain backward-compatible — no migration path is acceptable for in-flight sessions."
open_questions:
  - "Should the publish-before-author check gate on finalizeplan-complete phase in feature.yaml, or on artifact presence in the governance docs path, or both?"
  - "Is the target repo path resolved at dev start from feature.yaml target_repos, or confirmed interactively when the field is empty?"
depends_on:
  - lens-dev-new-codebase-finalizeplan
blocks: []
updated_at: '2026-04-30T00:00:00Z'
---

# Business Plan — Dev Command Rewrite

## Problem Statement

The `dev` command is the execution-phase conductor in lens-work. It bridges the planning
boundary — where governance and finalizeplan artifacts live — and the implementation
boundary — where code changes land in target repositories. Story 5.1 of the baseline rewrite
program requires that this bridge survive the rewrite without behavioral regression.

The risk is that a rewrite introduces subtle changes to where code is written (control repo
contamination), how progress is recorded (dev-session.yaml drift), or when a session can be
resumed (checkpoint schema incompatibility). Any of these failures breaks active in-progress
development sessions with no safe recovery path.

The `dev` command is the only lens-work command that writes code to a repository. All other
commands write governance artifacts, planning documents, or session state. That asymmetry makes
dev uniquely sensitive to write-path errors: a single misrouted file write during implementation
would contaminate the control repo and corrupt the governance model that all other commands
depend on.

## Business Context

The `dev` command rewrite is the fifth and final command-family tier in the
`lens-dev-new-codebase-baseline` program. Its execution tier (Epic 5) is gated behind the
conductor rewrites in Epic 4 (finalizeplan, Story 4.4), which must complete before any dev work
begins. The split feature `lens-dev-new-codebase-dev` carries Story 5.1 forward as a focused
single-command rewrite with three acceptance criteria.

This is part of the broader lens-work 17-command stable surface rewrite, which rebuilds
lens-work through the same governed lifecycle it enforces for other projects. Preserving the
dev command's behavioral contract is essential to that claim: if dev itself is unreliable after
the rewrite, the governance model breaks down at exactly the phase where users depend on it most.

## Stakeholders

| Role | Name | Responsibility |
|---|---|---|
| Feature lead | crisweber2600 | Planning, implementation oversight, acceptance sign-off |
| Lens user | (all active users) | Execute governed implementation work through the dev conductor |
| Maintainer | crisweber2600 | Validate behavioral parity post-rewrite; confirm dev-session.yaml compat |

## Success Criteria

1. **Target-repo-only writes**: All code changes produced during a dev session land in the
   target repository. No writes occur in the control repo or release repo at any point during
   the implementation loop.

2. **Per-task commits**: Each completed task produces an atomic, scoped commit to the target
   repo branch. The commit record is preserved; no squash or rebase occurs during the session.

3. **Resumable checkpoints**: An interrupted dev session can be resumed from the last valid
   checkpoint in `dev-session.yaml` without schema changes or loss of completed-task state.

4. **Final PR**: The dev session concludes with a single target-repo PR covering all task
   commits from that session. The PR is not opened mid-session.

5. **Publish-before-author entry hook**: The dev conductor confirms that finalizeplan artifacts
   (at minimum `epics.md` and `stories.md`) are present in the feature's governance docs path
   before starting the implementation loop. If artifacts are absent, dev stops with a clear error.

## Out of Scope

- Changes to the `complete` command or archival flow (covered by Story 5.2).
- New capabilities beyond what the old-codebase dev command provided.
- Schema evolution for `dev-session.yaml` — backward compatibility is required, not new fields.
- Any code writes to the control repo or release repo.
- Target repo provisioning (covered by `bmad-lens-target-repo`).

## Constraints

| Constraint | Source |
|---|---|
| Depends on `lens-dev-new-codebase-finalizeplan` being complete (Story 4.4) | Story 5.1 depends_on |
| Clean room: no direct file copy from old-codebase | Project directive |
| Track changed to express: single story, 3 ACs, bounded scope | Expressplan eligibility |
| dev-session.yaml schema unchanged | Backward compatibility requirement |
| Control repo is read-only during dev sessions | Behavioral invariant from baseline |

## Required Outcomes

- `bmad-lens-dev` SKILL.md implements the full conductor chain including the entry hook.
- `lens-dev.prompt.md` (release module redirect) routes correctly to the SKILL.md.
- `.github/prompts/lens-dev.prompt.md` (stub) runs preflight and delegates.
- Regression tests confirm all three acceptance criteria from Story 5.1 pass.
- Feature phase advances to `finalizeplan-complete` after the adversarial review passes.
