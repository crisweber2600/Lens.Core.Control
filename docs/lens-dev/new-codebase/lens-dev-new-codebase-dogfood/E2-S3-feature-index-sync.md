---
feature: lens-dev-new-codebase-dogfood
epic: 2
story_id: E2-S3
sprint_story_id: S2.3
title: Resolve BF-3 feature-index synchronization
type: fix
points: M
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E1-S4]
blocks: [E3-S1]
target_repo: lens.core.src
target_branch: develop
---

# E2-S3 — Resolve BF-3 feature-index synchronization

## Context

`feature-index.yaml` goes stale after phase transitions because no sanctioned sync operation exists. This is BF-3 and Defect 5. The dogfood feature itself is a live instance of this bug (feature-index shows `expressplan`, not `expressplan-complete`, as noted in the finalizeplan-review L1 finding). Direct governance edits are prohibited; the sync must route through the publish CLI or a sanctioned operation.

## Implementation Steps

1. Add `sync-feature-index` operation to `bmad-lens-feature-yaml` or `bmad-lens-git-orchestration`.
2. The operation reads `feature.yaml` for the target feature and updates the matching entry in `feature-index.yaml` in the governance repo.
3. Call the operation automatically after every phase transition in `bmad-lens-feature-yaml update`.
4. Governance write must route through the publish CLI or the sanctioned operation — no direct agent file edits.
5. Write fixture test: start with stale feature-index entry, run phase transition, confirm entry is updated.

## Acceptance Criteria

- [ ] `sync-feature-index` operation exists and updates `feature-index.yaml` from `feature.yaml`.
- [ ] Called automatically after every phase transition.
- [ ] Governance writes route through CLI or sanctioned operation; no direct agent file edits.
- [ ] Fixture test: stale entry → phase transition → entry updated passes.
- [ ] Defect 5 / BF-3 regression test passes.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill).

Load the BMB documentation index at `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md` before authoring.

## Dev Notes

- Reference: tech-plan ADR-3, Git Orchestration Contract section.
- L1 response A from finalizeplan-review: sync feature-index as part of the commit sequence after this review is pushed. The dogfood feature's own stale entry should be corrected as a side effect of landing this story.

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
