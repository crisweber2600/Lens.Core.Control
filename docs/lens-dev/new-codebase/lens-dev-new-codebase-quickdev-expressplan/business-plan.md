---
feature: lens-dev-new-codebase-quickdev-expressplan
doc_type: business-plan
status: approved
goal: "Define the user-facing need and delivery guardrails for a public lens-quickdev wrapper."
key_decisions:
  - Create a public Lens wrapper for quick development instead of asking users to invoke bmad-quick-dev through lens-bmad-skill directly.
  - Preserve the existing BMAD quick-dev implementation workflow as the implementation engine.
  - Require the wrapper to assess the target codebase, plan the ask, implement the ask, and document both the quickdev session and resulting commit in feature docs.
  - Allow lens-quickdev only once the feature is dev-ready; it must not bypass planning or FinalizePlan readiness.
  - Use direct commits only when an active feature branch is already in progress; otherwise use the standard branch and PR flow.
  - Publish quickdev evidence to governance and store commit evidence inside a generated quickdev-[summaryofrequeststub].md file rather than a separate commit artifact.
open_questions: []
depends_on: []
blocks: []
updated_at: '2026-05-06T15:45:00Z'
---

# Business Plan - lens-quickdev Wrapper

## Problem

The new-codebase module already exposes `bmad-quick-dev` through the internal Lens BMAD wrapper, and it has a bug-specific conductor at `/lens-bug-quickdev`. It does not yet provide a general public `lens-quickdev` command for a normal feature owner who wants to ask for a scoped code change and have Lens handle the full path: assess the current codebase, plan the ask, implement the ask, and document what happened.

Without a wrapper, users must know that quick development is hidden behind `lens-bmad-skill --skill bmad-quick-dev`, must manually supply enough Lens context, and must remember to capture implementation evidence in docs. That creates inconsistent execution, weak auditability, and avoidable handoff friction before code review.

## Desired Outcome

Create a public `lens-quickdev` wrapper that gives users a single governed command for small implementation asks. The wrapper should:

1. Confirm the active feature is dev-ready before any implementation work begins.
2. Resolve the active feature, docs path, governance docs path, target repository, and target branch policy.
3. Assess the current codebase before implementation begins.
4. Produce or refresh a concise implementation plan for the ask.
5. Delegate code changes to the existing `bmad-quick-dev` workflow through Lens context.
6. Commit the implementation in the target repo when changes are made.
7. Write durable quickdev evidence that records the request, plan, validation, branch, commit, changed files, and PR URL when applicable.
8. Publish the same quickdev evidence to governance so the implementation record survives beyond the local control repo branch.

## Users

Primary users are Lens maintainers working on small, well-understood code changes inside `TargetProjects/lens-dev/new-codebase/lens.core.src`. Secondary users are feature owners in other services who need a repeatable quick development route without learning the internal BMAD wrapper shape.

## Scope

In scope:
- Add a public `lens-quickdev` prompt redirect and Lens skill contract in the source module.
- Register `lens-quickdev` in the module command surfaces needed for discovery.
- Route implementation through `lens-bmad-skill --skill bmad-quick-dev` or an equivalent sanctioned Lens wrapper path.
- Define docs output as `quickdev-[summaryofrequeststub].md` under `feature.yaml.docs.path`.
- Publish the generated quickdev document to `feature.yaml.docs.governance_docs_path` through the sanctioned Lens publication path.
- Require codebase assessment, implementation plan, validation summary, commit hash, branch, PR URL when applicable, and changed-files summary in the generated quickdev document.
- Enforce the branch policy: commit directly to an active in-progress feature branch when one exists; otherwise create a working branch and PR through standard Lens git orchestration.
- Define validation-failure behavior before and after commit creation.

Out of scope:
- Rewriting the BMAD quick-dev workflow itself.
- Replacing `lens-dev` or `lens-finalizeplan` lifecycle conductors.
- Creating a bug intake artifact; `/lens-bug-quickdev` remains the bug-specific route.
- Writing directly to `lens.core/` or `.github/prompts/` in the control repo.

## Success Criteria

- Users can invoke the public `lens-quickdev` wrapper for an active feature without manually routing through `lens-bmad-skill`.
- The wrapper refuses to run before the feature is dev-ready.
- The wrapper refuses to run when no target repository can be resolved.
- Codebase assessment and implementation plan are documented before or during the quickdev session.
- Implementation changes are committed in the target repo with a conventional commit message.
- Feature docs include a generated `quickdev-[summaryofrequeststub].md` file with the request, plan, validation, changed files, branch, commit hash, and PR URL when applicable.
- The generated quickdev document is published to governance after the local evidence file is written.
- Existing `/lens-bug-quickdev` behavior remains unchanged.
