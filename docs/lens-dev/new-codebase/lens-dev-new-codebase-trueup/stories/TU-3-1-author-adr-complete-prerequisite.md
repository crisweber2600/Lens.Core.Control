---
feature: lens-dev-new-codebase-trueup
story_id: "TU-3.1"
story_key: "TU-3-1-author-adr-complete-prerequisite"
epic: "EP-3"
title: "Author adr-complete-prerequisite.md"
status: ready-for-dev
priority: must
story_points: 2
depends_on: []
updated_at: 2026-04-29T00:00:00Z
---

# Story TU-3.1: Author adr-complete-prerequisite.md

Status: ready-for-dev

## Story

As a Lens Workbench maintainer,
I want the binding ADR for the `bmad-lens-complete` prerequisite handling strategy authored and committed,
so that the decision to use graceful-degradation (warn-not-fail when retrospective is absent) is recorded and traceable for future implementers.

## Acceptance Criteria

1. `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/adr-complete-prerequisite.md` created and committed.

2. ADR structure includes all required sections:
   - **Context**: why the prerequisite handling decision was needed (old-codebase behavior, `check-preconditions` guards)
   - **Decision**: graceful-degradation — retrospective absent → warn (not fail), document gap
   - **Evidence**: references old-codebase `complete-ops.py` behavior that informed this decision
   - **Implications**: what this means for TU-2.1 implementation (`check-preconditions` return shape must include `missing_artifacts`)
   - **Companion governance action**: reference to TU-5.1 blocker annotation for `lens-dev-new-codebase-complete`

3. ADR status: `Accepted` (accepted on techplan-adversarial-review pass, 2026-04-28T02:20:00Z).

4. Decision content matches ADR-1 in `architecture.md` Section 4.

## Tasks / Subtasks

- [ ] Read `architecture.md` Section 4 (ADR-1) for the canonical decision summary.
- [ ] Read old-codebase `complete-ops.py` (if accessible) for behavioral evidence.
- [ ] Author `adr-complete-prerequisite.md` in full ADR format.
- [ ] Confirm decision status `Accepted` and date.
- [ ] Commit with message: `[DEV] TU-3.1 — author adr-complete-prerequisite.md (ADR-1)`.

## Dev Notes

- This ADR is binding: it constrains how TU-2.1 implements `check-preconditions`. The warn-not-fail behavior must be documented clearly in the decision rationale.
- If old-codebase `complete-ops.py` is not accessible for evidence, use the old-codebase behavioral contract from `TargetProjects/lens-dev/old-codebase/lens.core.src/` as the reference.
- The companion governance action (blocker annotation) is NOT authored in this story — it is authored in TU-5.1.

### References

- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/architecture.md` — Section 4, ADR-1
- Old-codebase `complete-ops.py` in `TargetProjects/lens-dev/old-codebase/lens.core.src/`
- TU-2.1 story (companion — uses this ADR as a constraint)
- TU-5.1 story (companion — governance action referenced from this ADR)

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
