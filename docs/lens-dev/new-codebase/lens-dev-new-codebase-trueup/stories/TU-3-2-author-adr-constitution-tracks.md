---
feature: lens-dev-new-codebase-trueup
story_id: "TU-3.2"
story_key: "TU-3-2-author-adr-constitution-tracks"
epic: "EP-3"
title: "Author adr-constitution-tracks.md"
status: ready-for-dev
priority: must
story_points: 2
depends_on: []
updated_at: 2026-04-29T00:00:00Z
---

# Story TU-3.2: Author adr-constitution-tracks.md

Status: ready-for-dev

## Story

As a Lens Workbench maintainer,
I want the binding ADR confirming the new-codebase constitution template as canonical authored and committed,
so that the `express` and `expressplan` track inclusion is a recorded architectural decision for `create-domain` and `create-service` implementers.

## Acceptance Criteria

1. `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/adr-constitution-tracks.md` created and committed.

2. ADR structure includes all required sections:
   - **Context**: why the constitution template decision was needed (old-codebase had `full` + `express` only; new-codebase adds `expressplan`)
   - **Decision**: new-codebase constitution template is canonical — `permitted_tracks` includes `full`, `express`, `expressplan`
   - **Evidence**: references `lifecycle.yaml` v4, domain constitution files that show `expressplan` usage, the `expressflow` usage pattern
   - **Implications**: `create-domain` and `create-service` commands must use this canonical template; expressplan track is not a gap
   - **Parity audit classification**: `expressplan` track is a resolved design decision, not a parity gap

3. ADR status: `Accepted`.

4. Decision content matches ADR-2 in `architecture.md` Section 4.

## Tasks / Subtasks

- [ ] Read `architecture.md` Section 4 (ADR-2) for the canonical decision summary.
- [ ] Check `lifecycle.yaml` in `lens.core.src/_bmad/lens-work/` for `permitted_tracks` evidence.
- [ ] Check domain constitution files in `TargetProjects/lens/lens-governance/constitutions/` for `expressplan` usage evidence.
- [ ] Author `adr-constitution-tracks.md` in full ADR format.
- [ ] Confirm decision status `Accepted`.
- [ ] Commit with message: `[DEV] TU-3.2 — author adr-constitution-tracks.md (ADR-2)`.

## Dev Notes

- This ADR is binding: it classifies `expressplan` track inclusion as an intentional design decision, not a parity gap. TU-4.1 (parity audit report) must reference this ADR when reporting on `new-domain` and `new-service` parity.
- If `lifecycle.yaml` does not list `expressplan` in `permitted_tracks`, that is a parity finding — surface it in TU-4.1 rather than masking it here.
- This ADR is also referenced by TU-4.3 (`parity-gate-spec.md`) as a migration standard.

### References

- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/architecture.md` — Section 4, ADR-2
- `lens.core.src/_bmad/lens-work/lifecycle.yaml` (permitted_tracks evidence)
- `TargetProjects/lens/lens-governance/constitutions/` (domain constitution evidence)
- TU-4.1 story (companion — references this ADR in the parity classification section)
- TU-4.3 story (companion — references this ADR as a migration standard)

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
