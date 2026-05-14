---
feature: nextlens-src-topdownlens
doc_type: example
story_id: TL-10
title: Bugfix Flow Example
updated_at: 2026-05-14T05:05:00Z
---

# Bugfix Flow Example

## Originating Signal

- Signal ID: `salmon.20260514T045700Z.high_correct_course`
- Severity: high
- Recommended action: `bmad_correct_course`
- Finding: a selected feature is crossing from discovery contracts into release automation without an approved topology boundary.
- Evidence refs: `evidence.tl1_schema_validation`

## Triage

Owner: conductor plus developer.

Route: immediate bugfix branch because the signal is high severity and could invalidate downstream topology assumptions.

## Fix Branches

- Control branch: `nextlens-src-topdownlens-dev` or a dedicated governed bugfix branch if the defect is outside the active feature.
- Target branch: prepared by `lens-git-orchestration` for the resolved target repo.

## Verification

The fix updates only the control docs path or target repo implementation files. It does not write directly to governance feature folders or release paths. Validation evidence is recorded in the feature docs and linked back to the Salmon signal.

## Closure

The signal moves to `resolved` after the PR is recorded and evidence references point to the validation output. If a later signal replaces this one, the original may move to `superseded` instead, with a pointer to the newer signal.