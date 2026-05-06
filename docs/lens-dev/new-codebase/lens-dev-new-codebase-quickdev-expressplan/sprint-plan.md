---
feature: lens-dev-new-codebase-quickdev-expressplan
doc_type: sprint-plan
status: approved
goal: "Sequence implementation slices for the lens-quickdev wrapper."
key_decisions:
  - Deliver the wrapper as an additive public command with no change to the existing BMAD quick-dev workflow.
  - Put evidence documentation requirements in the first implementation slice so they cannot be deferred.
  - Keep bug quickdev regression coverage explicit.
  - Allow quickdev only when the feature is dev-ready.
  - Record quickdev and commit evidence in generated quickdev-[summaryofrequeststub].md files that publish to governance.
  - Commit directly to active feature branches; otherwise create a branch and PR through standard Lens git flow.
open_questions: []
depends_on:
  - business-plan.md
  - tech-plan.md
blocks: []
updated_at: '2026-05-06T15:45:00Z'
---

# Sprint Plan - lens-quickdev Wrapper

## Sprint Goal

Ship a public `lens-quickdev` conductor that lets users provide a scoped implementation ask during dev-ready execution, delegates code changes to existing BMAD quick-dev behavior, and records quickdev plus commit evidence in a generated evidence document that is published to governance.

## Slice 1 - Command Surface and Registration

Goal: Make `lens-quickdev` discoverable without implementing quick-dev logic inline.

Tasks:
- Add `_bmad/lens-work/prompts/lens-quickdev.prompt.md` as a redirect-only prompt with prompt-start preflight.
- Add `_bmad/lens-work/skills/lens-quickdev/SKILL.md` with the conductor contract.
- Register the prompt and skill in `module.yaml`.
- Add `lens-quickdev` to `module-help.csv` with clear action text and output docs.

Acceptance criteria:
- Given the module command surfaces, when a user searches for quickdev, then `lens-quickdev` appears as a public command.
- Given the prompt file, when reviewed, then it contains no business logic beyond preflight and skill loading.
- Given the skill contract, when reviewed, then it delegates implementation to `bmad-quick-dev` rather than duplicating it.
- Given the command help, when reviewed, then it makes clear that `lens-quickdev` is allowed only for dev-ready features.

## Slice 2 - Context, Assessment, and Planning Gate

Goal: Ensure quickdev starts with the right repo, branch, docs path, and implementation plan.

Tasks:
- Resolve active feature state through `lens-feature-yaml`.
- Enforce the dev-ready gate before target-repo assessment.
- Resolve target repo from `feature.yaml.target_repos` and block when absent.
- Inspect target repo branch and dirty status before any implementation.
- Generate `quickdev-[summaryofrequeststub].md` with request, assessment, assumptions, validation plan, and implementation plan.

Acceptance criteria:
- Given a feature that is not dev-ready, when `lens-quickdev` runs, then it blocks before target-repo assessment.
- Given a feature with no target repo, when `lens-quickdev` runs, then it blocks before delegation with a repair instruction.
- Given a dirty target repo with unrelated changes, when `lens-quickdev` runs, then it blocks before implementation.
- Given a valid target repo, when assessment completes, then `quickdev-[summaryofrequeststub].md` exists under `feature.yaml.docs.path`.

## Slice 3 - Implementation Delegation and Commit Evidence

Goal: Execute the ask through the existing quick-dev engine and record the resulting commit.

Tasks:
- Prepare or verify target-repo working branch according to branch policy.
- Invoke `bmad-quick-dev` with Lens context and the user ask.
- Run focused validation selected during assessment.
- Commit implementation changes with a conventional commit message.
- Update the generated quickdev evidence file with branch, commit hash, changed files, validation, PR link when available, and final outcome.
- Publish the generated quickdev evidence file to governance.

Acceptance criteria:
- Given implementation changes are made, when the wrapper completes, then target repo has a non-empty commit hash recorded in `quickdev-[summaryofrequeststub].md`.
- Given validation runs, when the wrapper completes, then validation command and result are recorded in `quickdev-[summaryofrequeststub].md`.
- Given no implementation changes are produced, when the wrapper completes, then the quickdev evidence records a no-op outcome and no empty commit is created.
- Given the quickdev evidence is finalized, when governance publication runs, then the same file is present under `feature.yaml.docs.governance_docs_path`.

## Slice 4 - Branch Policy, Validation Failure, and Regression Coverage

Goal: Make wrapper completion reviewable and protect existing bug quickdev behavior.

Tasks:
- Implement branch policy: direct commit to active in-progress feature branch, otherwise branch and PR through git orchestration.
- Implement validation failure handling before commit, after local commit, and after push/PR.
- Add tests for redirect-only prompt, registration, dev-ready gate, missing target repo, generated evidence docs, governance publication, branch policy, validation failure, and no-op behavior.
- Add a non-regression check for `/lens-bug-quickdev` routing.

Acceptance criteria:
- Given an active in-progress feature branch exists, when the wrapper completes with changes, then it commits directly to that branch and records no new PR unless one already exists.
- Given no active in-progress feature branch exists, when the wrapper completes with changes, then it creates or verifies a PR and records the PR URL in the quickdev evidence.
- Given validation fails before commit, when the wrapper stops, then no commit is created and the quickdev evidence is marked blocked.
- Given validation fails after a local commit, when the wrapper stops, then it does not push or open a PR and asks for fix-forward or revert direction.
- Given validation fails after push or PR, when the wrapper recovers, then it does not rewrite shared history and records either a fix-forward commit or blocked PR state.
- Given existing `/lens-bug-quickdev`, when command registration is checked, then it still routes to the bug-specific conductor.
- Given focused tests run, then the new wrapper tests pass without requiring a real remote PR.

## Definition of Done

- Public command surface is registered.
- The command is gated to dev-ready features.
- Implementation remains conductor-only and delegates to existing quick-dev behavior.
- Codebase assessment, implementation plan, validation, branch, commit, and PR evidence are captured in generated quickdev docs.
- Generated quickdev docs are published to governance.
- Tests cover command discovery, dev-ready and target-repo blockers, branch policy, validation-failure behavior, docs evidence, governance publication, and bug quickdev non-regression.
