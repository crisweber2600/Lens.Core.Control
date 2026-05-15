---
feature: nextlens-src-dogfoodnext
story_id: NLB-7
doc_type: story
status: ready-for-dev
title: Validation, PR Recording, And Closeout
depends_on: [NLB-3, NLB-6]
implementation_kind: closeout
epic: 3
updated_at: 2026-05-15T20:00:00Z
---

# NLB-7 - Validation, PR Recording, And Closeout

## Goal

Complete the NextLens bugfix conductor loop with validation evidence, PR recording, and stateful closeout under `bugs/nextlens/Fixed/`.

## Scope

- Push the implementation branch and create or reuse a PR before reporting success.
- Record the PR URL on the namespaced bug artifact.
- Invoke or reference NextLens Doctor validation output instead of reimplementing Doctor checks.
- Move the bug artifact to `bugs/nextlens/Fixed/{slug}.md` only after validation and PR evidence exist.

## Acceptance Criteria

- Given implementation has a commit, when completion runs, then the branch is pushed and a PR URL is captured.
- Given PR recording succeeds, when the bug artifact is namespaced, then the PR URL is written to the matching artifact only.
- Given Doctor validation passes, when closeout runs, then Doctor evidence is recorded before the artifact moves to `Fixed`.
- Given Doctor validation is not applicable or deferred, when closeout runs, then the rationale is recorded and policy allows continuation.
- Given validation or PR evidence is missing, when closeout runs, then the artifact remains open and success is blocked.

## Validation

- Add completion tests for PR recording, Doctor pass, Doctor not-applicable rationale, missing validation evidence, missing PR URL, and successful move to `bugs/nextlens/Fixed/`.

## Dev Notes

- High and blocking Salmon signals require evidence that closure or supersession used the approved route.