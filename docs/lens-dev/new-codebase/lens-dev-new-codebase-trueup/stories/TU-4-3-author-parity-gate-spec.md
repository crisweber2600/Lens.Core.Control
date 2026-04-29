---
feature: lens-dev-new-codebase-trueup
story_id: "TU-4.3"
story_key: "TU-4-3-author-parity-gate-spec"
epic: "EP-4"
title: "Author parity-gate-spec.md"
status: ready-for-dev
priority: must
story_points: 3
depends_on: []
updated_at: 2026-04-29T00:00:00Z
---

# Story TU-4.3: Author parity-gate-spec.md

Status: ready-for-dev

## Story

As a Lens Workbench maintainer,
I want a parity gate specification authored that defines what "fully migrated" means for any retained command migration in the new-codebase,
so that future migration work (beyond True Up) has a clear, reusable gate definition and teams know exactly what to verify before declaring a command migration complete.

## Acceptance Criteria

1. `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/parity-gate-spec.md` created and committed.

2. Spec defines a "fully migrated" gate with three layers:
   - **Layer 1 (Prompt Stub):** `_bmad/lens-work/prompts/{command}.prompt.md` present and follows stub format
   - **Layer 2 (SKILL.md):** `skills/{skill-name}/SKILL.md` present, authored via BMB channel, command contracts documented
   - **Layer 3 (Script + Tests):** `skills/{skill-name}/scripts/{name}-ops.py` present with `conftest.py` and at least 6 test stubs

3. Spec includes a "How to Apply" section (CF-4):
   - 2–3 paragraphs explaining how to use the gate for a future retained command migration
   - Explains the evaluation sequence (Layer 1 → 2 → 3 — all three must pass for "fully migrated")
   - Explains how partial completions are handled (partial = migration in progress, not blocked, must have a tracking story)

4. Spec references ADR-3 (Python 3.12) and ADR-4 (SAFE_ID_PATTERN) as migration standards that apply to all Layer 3 scripts.

5. **Post-merge action (CF-11):** After this story is committed and the feature PR is merged, add a reference to `parity-gate-spec.md` from the service constitution at `constitutions/lens-dev/new-codebase/constitution.md` under a new "Migration Standards" section. This is a governance commit in the governance repo, not a source commit.

## Tasks / Subtasks

- [ ] Read `architecture.md` Section 3 for the 3-layer structure reference.
- [ ] Read ADR-3 and ADR-4 in `architecture.md` Section 4 for migration standards.
- [ ] Author `parity-gate-spec.md` with three-layer gate definition.
- [ ] Author the "How to Apply" section (2–3 paragraphs, CF-4).
- [ ] Add ADR-3 and ADR-4 references to the migration standards section.
- [ ] Commit with message: `[DEV] TU-4.3 — author parity-gate-spec.md with How to Apply section (FR-14)`.
- [ ] **Post-merge (deferred):** After feature PR merges, add constitution reference in governance repo (CF-11).

## Dev Notes

- **CF-4**: The "How to Apply" section is a mandatory AC. The gate spec without usage instructions is incomplete.
- **CF-11**: The constitution reference is a governance repo write — it goes to `TargetProjects/lens/lens-governance/constitutions/lens-dev/new-codebase/constitution.md`. This is deferred until after the feature PR merges (it references the gate spec by its post-merge path). Do not write the constitution reference as part of this story commit.
- **Audience**: The gate spec is for future maintainers and agents performing command migrations. Write it at a level that a dev agent with the architecture doc can apply it independently.

### References

- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/architecture.md` — Section 3 (3-layer structure), Section 4 (ADR-3, ADR-4)
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/finalizeplan-review.md` — CF-4, CF-11
- `TargetProjects/lens/lens-governance/constitutions/lens-dev/new-codebase/constitution.md` (post-merge governance reference target)

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
