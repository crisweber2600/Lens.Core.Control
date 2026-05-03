---
feature: lens-dev-new-codebase-bugfix-bug-fixer-uses-excessive-find-probi
doc_type: business-plan
status: approved
goal: "Fix the bug-fixer skill's excessive find-probe behaviour by constraining the writable SKILL.md path and landing the stub truncation fix."
key_decisions:
  - Constrain Story S1 acceptance criteria to name the exact writable SKILL.md path to prevent accidental edits to the read-only lens.core/ clone.
  - Commit the bug-fixer-ops.py stub truncation fix to the feature branch before dev begins.
  - File the feature-yaml-ops.py ModuleNotFoundError as a follow-on bug artifact rather than blocking this feature.
  - Treat lens-bug-reporter/SKILL.md path-probing as out of scope for this batch.
open_questions:
  - Has bug-fixer-ops.py truncation fix been committed to the lens.core.src feature branch?
  - Should feature-yaml-ops.py import bug be filed before FinalizePlan closes?
depends_on:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/prd.md
blocks: []
updated_at: '2026-05-03T19:10:00Z'
---

# Business Plan — Bug Fixer Excessive Find Probe Fix

## Executive Summary

The `lens-dev-new-codebase-bugfix-bug-fixer-uses-excessive-find-probi` feature addresses two concrete defects in the `bmad-lens-bug-fixer` skill:

1. Story S1 acceptance criteria do not name the exact writable SKILL.md path, creating risk that an implementer edits the read-only `lens.core/` clone instead of the correct `TargetProjects/lens-dev/new-codebase/lens.core.src` path.
2. The `bug-fixer-ops.py` stub truncation fix was applied during the expressplan run but not committed to the feature branch, risking loss on branch switch.

Both defects are small in scope and addressable within a single planning cycle.

## Problem Statement

The `bmad-lens-bug-fixer` skill uses excessive `find` probes when locating SKILL.md, and the stub truncation logic in `bug-fixer-ops.py` applies a `MAX_STUB_LEN` constant without a named reference to the `SAFE_ID_PATTERN` constraint. The immediate risk is that an implementer resolving Story S1 edits the wrong file because the acceptance criteria don't pin the exact target path.

## Users And Stakeholders

- Lens feature owners who rely on `bmad-lens-bug-fixer` for regression triage.
- Lens maintainers who need the SKILL.md edit path to be unambiguous.
- Governance reviewers who need all open findings resolved or explicitly deferred before this feature closes.

## Goals

1. Tighten Story S1 AC to assert the exact writable SKILL.md path.
2. Commit `bug-fixer-ops.py` truncation fix to the `lens.core.src` feature branch.
3. File the `feature-yaml-ops.py` import bug as a follow-on artifact.
4. Close this feature with no critical or high findings outstanding.

## Non-Goals

- Addressing `lens-bug-reporter/SKILL.md` path ambiguity (out of scope, explicitly deferred).
- Changing the runtime behaviour of the `bmad-lens-bug-fixer` skill beyond the stub truncation fix.
- Reworking unrelated lifecycle contracts or feature identity rules.

## Required Outcomes

### User Outcomes

- Story S1 AC names the exact target file path so no implementer can accidentally edit the wrong clone.
- The stub truncation fix is durably committed to the feature branch.

### Governance Outcomes

- All findings in this planning packet are either resolved or explicitly deferred with a recorded reason.
- The feature can close without carry-forward blockers.

### Delivery Outcomes

- S1 and S2 are sequenced with an explicit dependency so neither story is dropped.
- The follow-on bug for `feature-yaml-ops.py` is filed as a separate artifact.

## Scope

### In Scope

- Updating Story S1 AC to pin the exact writable SKILL.md path.
- Committing `bug-fixer-ops.py` stub truncation fix to the feature branch.
- Filing the `feature-yaml-ops.py` import bug as a follow-on report.

### Out Of Scope

- `lens-bug-reporter/SKILL.md` path-probing (explicit deferral).
- Any changes to the `lens.core/` read-only release payload.

## Success Criteria

This feature is successful when:

1. Story S1 AC names `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-bug-fixer/SKILL.md` as the only valid edit target.
2. `bug-fixer-ops.py` stub truncation fix is committed and confirmed on the feature branch.
3. `feature-yaml-ops.py` import bug is filed as a follow-on artifact.
4. No critical or high findings remain open at FinalizePlan close.

## Risks And Mitigations

| Risk | Impact | Mitigation |
| --- | --- | --- |
| S1 and S2 executed out of order | S2 depends on S1; dropping S1 leaves path ambiguity unresolved | Add explicit sequencing to sprint-plan delivery slices |
| bug-fixer-ops.py fix lost on branch switch | Regression reintroduced | Commit fix immediately as part of this planning packet |
| feature-yaml-ops.py import bug blocks future ops | Downstream tooling breaks silently | File as follow-on bug before FinalizePlan closes |

## Exit Decision

Proceed on express planning path. All findings are either resolvable within this batch or explicitly deferred with a recorded reason. No critical findings exist.
