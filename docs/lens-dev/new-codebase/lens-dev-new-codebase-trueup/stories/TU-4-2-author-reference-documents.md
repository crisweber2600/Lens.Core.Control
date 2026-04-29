---
feature: lens-dev-new-codebase-trueup
story_id: "TU-4.2"
story_key: "TU-4-2-author-reference-documents"
epic: "EP-4"
title: "Author Reference Documents (auto-context-pull.md, init-feature.md)"
status: ready-for-dev
priority: must
story_points: 3
depends_on: []
updated_at: 2026-04-29T00:00:00Z
---

# Story TU-4.2: Author Reference Documents (auto-context-pull.md, init-feature.md)

Status: ready-for-dev

## Story

As a Lens Workbench maintainer,
I want two reference documents authored for `bmad-lens-init-feature` covering the Auto-Context Pull flow and the init-feature command flow,
so that future contributors and agents have canonical documentation for these behavioral contracts without referencing old-codebase source.

## Acceptance Criteria

1. `lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/references/auto-context-pull.md` created and committed.
   - Documents the Auto-Context Pull flow: how `fetch-context` and `read-context` are invoked
   - Documents expected output contracts for both functions (matching architecture Section 6)
   - Documents error/degradation behavior when context is unavailable
   - Content derived from discovery docs and architecture — clean-room authoring (no old-codebase code blocks)

2. `lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/references/init-feature.md` created and committed.
   - Documents the `init-feature create` command flow: featureId construction, lifecycle track resolution, feature.yaml authoring, 2-branch creation
   - Documents the permitted_tracks list: must include `full`, `express`, `expressplan` (matches ADR-2)
   - Documents governance path: control repo branch → governance repo feature.yaml creation
   - Content derived from discovery docs and architecture — clean-room authoring (no old-codebase code blocks)

3. Both files reflect the canonical `permitted_tracks` list from ADR-2 (includes `express`, `expressplan`).

## Tasks / Subtasks

- [ ] Read `architecture.md` Section 6 (Auto-Context Pull flow) for `auto-context-pull.md` content.
- [ ] Read `architecture.md` Section 3 for `init-feature create` command flow.
- [ ] Read `adr-constitution-tracks.md` (TU-3.2 output) for `permitted_tracks` canonical list.
- [ ] Create `references/` directory in `lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/` if it doesn't exist.
- [ ] Author `auto-context-pull.md`.
- [ ] Author `init-feature.md`.
- [ ] Commit both files with message: `[DEV] TU-4.2 — author auto-context-pull.md and init-feature.md reference docs (FR-13)`.

## Dev Notes

- **Clean-room authoring**: Reference documents must not copy old-codebase code blocks. The content should be derived from discovery docs (brainstorm, research, PRD, architecture) and behavioral contracts.
- **Permitted tracks**: The `init-feature.md` reference must document `expressplan` as a valid track. Omitting it would contradict ADR-2 and create parity confusion for implementers.
- These reference documents serve as the canonical human-readable contract for the two behavioral flows. They are not the SKILL.md — they are supplementary reference material stored in the `references/` subdirectory.

### References

- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/architecture.md` — Section 3 (init-feature flow), Section 6 (Auto-Context Pull flow)
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/adr-constitution-tracks.md` (TU-3.2 output — permitted_tracks)
- `lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/` (target location)

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
