---
feature: lens-dev-new-codebase-trueup
story_id: "TU-5.1"
story_key: "TU-5-1-blocker-annotations-completion-gate"
epic: "EP-5"
title: "Write Blocker Annotations and Confirm 14-FR Completion Gate"
status: ready-for-dev
priority: must
story_points: 2
depends_on: [TU-4.1, TU-4.2, TU-4.3]
updated_at: 2026-04-29T00:00:00Z
---

# Story TU-5.1: Write Blocker Annotations and Confirm 14-FR Completion Gate

Status: ready-for-dev

## Story

As a Lens Workbench maintainer,
I want blocker annotations written to the governance `feature.yaml` files of `lens-dev-new-codebase-new-feature` and `lens-dev-new-codebase-complete`, and the 14-FR completion gate checked,
so that the regression findings from the parity audit are governance-visible records and the True Up feature can be declared dev-complete.

## Pre-Checks (must run before any writes — CF-3)

- [ ] `cd TargetProjects/lens/lens-governance && git pull` — confirm governance repo is current
- [ ] Read `features/lens-dev/new-codebase/lens-dev-new-codebase-new-feature/feature.yaml` before writing
- [ ] Read `features/lens-dev/new-codebase/lens-dev-new-codebase-complete/feature.yaml` before writing
- [ ] If either file already contains a True Up blocker annotation, the write is idempotent — do not add a duplicate (CF-3)

## Acceptance Criteria

### Blocker Annotation: lens-dev-new-codebase-new-feature

- [ ] `features/lens-dev/new-codebase/lens-dev-new-codebase-new-feature/feature.yaml` updated with blocker annotation:
  ```
  True Up parity audit (2026-04-29): create, fetch-context, read-context absent from new-codebase init-feature-ops.py — functional regression. Blocker on dev phase activation until restored per lens-dev-new-codebase-trueup architecture Section 6.
  ```
- [ ] Annotation committed and pushed to governance repo `main`

### Blocker Annotation: lens-dev-new-codebase-complete

- [ ] `features/lens-dev/new-codebase/lens-dev-new-codebase-complete/feature.yaml` updated with blocker annotation:
  ```
  True Up parity audit (2026-04-29): entire bmad-lens-complete skill absent (no SKILL.md, no complete-ops.py, no tests). Blocker on dev phase activation pending True Up dev-complete. See ADR-1 in lens-dev-new-codebase-trueup for prerequisite strategy.
  ```
- [ ] Annotation committed and pushed to governance repo `main`

### 14-FR Completion Gate (CF-7)

All 14 FRs must be verified complete before this story can close:

- [ ] FR-1: `lens-switch.prompt.md` verified in `_bmad/lens-work/prompts/` (TU-1.1 complete)
- [ ] FR-2: `lens-new-feature.prompt.md` authored in `_bmad/lens-work/prompts/` (TU-1.2 complete)
- [ ] FR-3: `lens-complete.prompt.md` authored in `_bmad/lens-work/prompts/` (TU-1.3 complete)
- [ ] FR-4: `bmad-lens-complete/SKILL.md` authored via BMB channel (TU-2.1 complete)
- [ ] FR-5: `test-complete-ops.py` stubs with `conftest.py` scaffold committed (TU-2.2 complete)
- [ ] FR-6: `adr-complete-prerequisite.md` committed (TU-3.1 complete)
- [ ] FR-7: `adr-constitution-tracks.md` committed (TU-3.2 complete)
- [ ] FR-8: Python 3.12 section in `parity-audit-report.md` committed (TU-4.1 complete)
- [ ] FR-9: SAFE_ID_PATTERN scan evidence section committed (TU-4.1 complete)
- [ ] FR-10: switch parity audit section committed (TU-4.1 complete)
- [ ] FR-11: new-domain parity audit section committed (TU-4.1 complete)
- [ ] FR-12: new-service parity audit section committed (TU-4.1 complete)
- [ ] FR-13: `auto-context-pull.md` and `init-feature.md` reference docs committed (TU-4.2 complete)
- [ ] FR-14: `parity-gate-spec.md` with "How to Apply" section committed (TU-4.3 complete)
- [ ] Blocker annotations for new-feature and complete committed to governance (this story)

## Tasks / Subtasks

- [ ] Verify all 14 FR completion gate items above before writing annotations.
- [ ] Pull governance repo to latest (`git pull`).
- [ ] Read both target feature.yaml files before writing.
- [ ] Write blocker annotation to `lens-dev-new-codebase-new-feature/feature.yaml`.
- [ ] Write blocker annotation to `lens-dev-new-codebase-complete/feature.yaml`.
- [ ] Commit with message: `[DEV] TU-5.1 — blocker annotations + 14-FR completion gate (True Up EP-5)`.
- [ ] Push to governance repo `main`.
- [ ] Report final pushed commit SHA.

## Dev Notes

- **CF-3 (idempotency)**: If a blocker annotation is already present (e.g., from a previous partial run), do not duplicate it. Verify the content and close the story with a note that it was already present.
- **CF-5 (timing gap)**: Before this story runs, the regression findings exist only in the parity audit report. The governance annotation is the final governance-visible record. The timing gap between TU-4.1 commit and this story is expected and correct — it is the parity audit review window (CF-9).
- **Governance write path**: This is a governance repo write. Follow the pull → write → commit → push sequence per the control repo git instructions. Never write directly to governance files without a prior pull.
- **14-FR gate**: This is the Master Definition of Done for the True Up feature. All 14 items must be verified before closing this story. If any FR is incomplete, the story is blocked until the corresponding dev story is resolved.
- **Post-merge deferred action (CF-11)**: After the feature PR merges, remember to add the `parity-gate-spec.md` reference to the service constitution (per TU-4.3). This is a separate governance commit, not part of this story.

### References

- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/parity-audit-report.md` (TU-4.1 output — blocker content source)
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/finalizeplan-review.md` — CF-3, CF-5, CF-7, CF-9
- `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-new-feature/feature.yaml` (target)
- `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-complete/feature.yaml` (target)

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
