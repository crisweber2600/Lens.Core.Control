---
feature: lens-dev-new-codebase-complete
doc_type: business-plan
status: draft
goal: "Restore /complete as a clean-room retained command that preserves irreversible archive semantics and final project documentation capture"
key_decisions:
  - Treat complete as one of the 17 retained published commands required for day-1 rewrite parity.
  - Preserve the closure contract as retrospective-first, document-before-archive, and atomic governance archival.
  - Keep the archive record centered on summary.md, feature.yaml, and feature-index.yaml without introducing new lifecycle schemas.
  - Use the old prompt, old discovery artifacts, and baseline rewrite docs as behavioral evidence only; implementation and tests remain clean-room.
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T21:28:18Z
---

# Business Plan - Complete Command

## Executive Summary

The `complete` feature restores the retained Lens command that closes a feature irreversibly. Its business value is not new functionality, but reliable closure: a feature author must be able to capture a retrospective, document the delivered project state, archive governance records atomically, and leave behind a final historical record that downstream commands can trust.

In the rewrite, `complete` is the last step in both the full and express tracks. If it is missing, incomplete, or behaviorally different, the retained command surface is not actually complete. Features would accumulate in `dev` or `dev-complete` states without a stable archival path, and users would lose confidence that Lens can govern work from creation through closure.

## Business Context

The baseline rewrite plan keeps `complete` in the published 17-command surface. The baseline PRD, research traceability matrix, and brainstorming summary all describe the same essential closure contract:

- `complete` is an irreversible lifecycle endpoint.
- It must preserve retrospective-before-archive sequencing.
- It must capture final project documentation before status changes.
- It must update the archive record atomically so downstream readers recognize a terminal archived state.

This matters because the closure path is a trust boundary for the whole product. `switch`, `next`, dashboard-style status views, and archive queries all rely on the final state being coherent. Users also need the completed feature folder to remain a durable historical record, not just a flag flip in YAML.

## Stakeholders

| Stakeholder | Interest | Sign-off Concern |
|---|---|---|
| Feature authors | Need a clear and safe way to close work permanently | The command confirms irreversibility and does not archive incomplete work silently |
| Governance owners | Need the registry and feature record to stay consistent | `feature.yaml`, `feature-index.yaml`, and `summary.md` stay aligned |
| Lens maintainers | Need parity with the retained old behavior without schema churn | The rewrite preserves terminal archive semantics and focused regression coverage |
| Downstream command owners | Need archived features to remain recognizable across readers | `archive-status`, `switch`, `next`, and reporting surfaces continue to interpret completed features correctly |
| Audit and documentation consumers | Need a durable historical record of what shipped | Retrospective and final project docs exist before archival |

## Success Criteria

1. `/complete` remains present in the retained published command surface and resolves to `bmad-lens-complete`.
2. The new skill preserves the user-facing sequence `check-preconditions -> retrospective -> document-project -> finalize`.
3. `check-preconditions` returns `pass`, `warn`, or `fail`, with missing retrospective treated as a warning and invalid lifecycle phase treated as a blocker.
4. Final documentation is captured before archive state changes are applied.
5. Finalize writes `feature.yaml` phase `complete`, `feature-index.yaml` status `archived`, and `summary.md` as one archive record change set.
6. `archive-status` continues to report whether a feature is archived based on terminal phase semantics.
7. A skipped retrospective requires explicit user confirmation and is reflected in the archive summary.
8. No lifecycle, feature, or feature-index schema migration is required.
9. Focused regressions prove archive atomicity, precondition behavior, and terminal-state recognition.
10. The implementation is authored clean-room from behavioral evidence rather than copied source.

## Scope

### In Scope

- Restore or complete the retained `lens-complete` prompt-to-skill path in the new codebase.
- Preserve the `bmad-lens-complete` skill as the closure conductor for retrospective, documentation capture, and archival confirmation.
- Preserve `complete-ops.py` style operations for `check-preconditions`, `finalize`, and `archive-status`.
- Preserve the feature archive outputs: `feature.yaml`, `feature-index.yaml`, `summary.md`, retrospective record, and project docs.
- Preserve dry-run or preflight visibility where the command needs to show what will happen before irreversible execution.
- Preserve focused regression coverage for archive atomicity and terminal-state recognition.

### Out of Scope

- Changing the lifecycle graph, adding new phases, or redefining what `archived` means.
- Replacing retrospective or document-project with a new documentation workflow.
- Introducing new archive schemas or alternate registry files.
- Rewriting unrelated planning or development commands.
- Copying old-codebase implementation files, prompts, or prose directly into the new source tree.

## Risks and Mitigations

| Risk | Probability | Impact | Mitigation |
|---|---:|---:|---|
| Archive semantics drift between prompt, skill, script, and tests | Medium | High | Define the sequence and outputs explicitly, then test the concrete command surface directly. |
| Partial archive updates leave governance in an inconsistent state | Medium | High | Preserve atomic write behavior and focused finalize regressions. |
| Summary artifact naming drifts between older discovery docs and the current implementation | Medium | Medium | Standardize the new implementation on `summary.md`, matching the current script, tests, and feature folders. |
| Retrospective becomes optional in practice because warnings are ignored | Medium | Medium | Keep explicit user confirmation for skips and record the skip in the archive summary. |
| Clean-room work weakens parity because behavior is under-specified | Low | High | Drive the feature from baseline rewrite requirements plus current skill/script/test behavior, not copied code. |

## Dependencies

- Baseline rewrite planning for `lens-dev-new-codebase-baseline`, especially the retained 17-command surface and the requirement that `complete` preserve archive atomicity.
- Old `complete` command behavior as described by the prior prompt chain and the old-codebase discovery artifacts.
- The current `bmad-lens-complete` skill contract and `complete-ops.py` command surface in the release module.
- Internal dependencies `bmad-lens-retrospective` and `bmad-lens-document-project`, which supply the two prerequisite archive artifacts.

## Timeline Expectations

This feature should land before the rewrite claims retained-command parity. A rewrite that can create and develop features but cannot close and archive them is operationally incomplete.

Because `complete` is terminal and irreversible, the implementation bar is stability rather than speed. Focused regression coverage and a conservative archive contract are more important than expanding the workflow beyond proven behavior.

## ExpressPlan Compatibility Note

This feature is currently registered as `track: full` and `phase: preplan`. The formal `lens-expressplan` lifecycle cannot auto-advance a full-track feature. These artifacts are therefore staged as clean-room planning documents in the control-repo docs path while still honoring the ExpressPlan two-document rule and frontmatter contract requested for this planning task.