---
feature: nextlens-src-topdownlens
doc_type: guide
story_id: TL-10
title: TopDownLens Bugfix Flow
updated_at: 2026-05-14T05:05:00Z
---

# TopDownLens Bugfix Flow

TopDownLens bugfixes reuse the Lens-core-bugfix pattern: capture the defect as governed evidence, create a dedicated fix branch, implement only in approved target surfaces, verify, record the PR, and close the originating Salmon signal.

## Intake From Salmon

| Severity | Route | Owner Role | Expected Timing |
| --- | --- | --- | --- |
| low | Backlog note on the current feature. | Product / conductor | Next normal planning pass. |
| medium | Next sprint candidate or landscape update. | Product / scrum master | Before dependent work expands. |
| high | Correct-course or immediate bugfix branch. | Conductor / developer | Before more dependent work lands. |
| blocking | Block promotion and open immediate bugfix. | Conductor / reviewer | Before release or governance publication. |

High and blocking Salmon signals should include `evidence_refs` and a `recommended_action` of `bmad_correct_course`, `split_feature`, or `block_promotion`.

## Branch Model

- Control repo: use the active feature branch family or a dedicated bugfix branch created by the governed bugfix conductor.
- Target repo: use the repo-scoped dev working branch prepared by `lens-git-orchestration`.
- Governance repo: stay on `main`; no feature-branch governance topology.
- Release repo: read-only local surface; no direct file edits.

## Verification Surface

Fix verification happens in the target repo or active control docs path for docs-only defects. Governance and release are verified through publication commands, not manual mutation.

## Closure Criteria

A bugfix is closed only when:

- The originating Salmon signal is `resolved` or explicitly `superseded`.
- The fix branch has a commit and PR or documented direct-default evidence.
- Evidence references point to validation output.
- Governance bug or signal artifacts record the PR URL and summary.
- No direct writes were made to governance feature folders, governance docs mirrors, or release clone paths.

## Approved Boundaries

Bugfixes may use `publish-to-governance`, `promote-to-release`, `lens-git-orchestration`, and governed bug reporter operations. They must not hand-copy changes into governance or release as a fallback.