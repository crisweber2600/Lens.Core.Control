---
feature: lens-dev-new-codebase-trueup
story_id: "TU-4.1"
story_key: "TU-4-1-author-parity-audit-report"
epic: "EP-4"
title: "Author parity-audit-report.md"
status: ready-for-dev
priority: must
story_points: 8
depends_on: [TU-3.1, TU-3.2]
updated_at: 2026-04-29T00:00:00Z
---

# Story TU-4.1: Author parity-audit-report.md

Status: ready-for-dev

## Story

As a Lens Workbench maintainer,
I want a consolidated parity audit report authored covering all 5 in-scope features,
so that the gap between old-codebase and new-codebase command surfaces is documented with verdicts, evidence, and governance classifications — and regression blockers are traceable to authoritative findings.

## Acceptance Criteria

1. `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/parity-audit-report.md` created and committed.

2. Per-feature section present for each of the following:
   - `lens-dev-new-codebase-switch`
   - `lens-dev-new-codebase-new-domain`
   - `lens-dev-new-codebase-new-service`
   - `lens-dev-new-codebase-new-feature`
   - `lens-dev-new-codebase-complete`

3. Each per-feature section includes:
   - Verdict: `pass`, `gap`, or `regression` (regression = behavioral capability present in old-codebase but absent in new)
   - Specific gaps or regressions identified: file path, function name, or schema field
   - Governance classification: `structural-gap`, `behavioral-regression`, or `governance-gap`
   - Classification rationale (1–2 sentences)

4. FR-8 section: Python 3.12 requirement documented as an intentional design decision:
   - Reason: `tomllib` (stdlib) availability, structural pattern matching, clean migration
   - Classification: `reviewed-decision` — not a parity gap
   - Reference: ADR-3

5. FR-9 section: SAFE_ID_PATTERN tightening documented with scan evidence (CF-2):
   - Scan scope stated explicitly: all `feature.yaml` and `feature-index.yaml` in `TargetProjects/lens/lens-governance/`
   - Scan date stated
   - Scan result stated (pass = no IDs with dots/underscores found; fail = IDs found, list them)
   - If scan fails: this story is blocking; surface as a critical finding before committing
   - Reference: ADR-4

6. `new-feature` findings must include:
   - `create` subcommand absent from `init-feature-ops.py` → behavioral regression
   - `fetch-context` function absent → behavioral regression
   - `read-context` function absent → behavioral regression

7. `complete` findings must include:
   - Entire `bmad-lens-complete` skill absent (no SKILL.md, no `complete-ops.py`, no tests) → behavioral regression

8. `switch` findings must include:
   - Prompt publishing gap → classified (structural or governance gap, not behavioral regression)

9. **Timing note (CF-5):** This story does NOT write blocker annotations to governance. The annotation is a post-audit governance write in TU-5.1. This timing gap is expected and correct.

## Tasks / Subtasks

- [ ] Confirm TU-3.1 (`adr-complete-prerequisite.md`) is complete before referencing it in the `complete` section.
- [ ] Confirm TU-3.2 (`adr-constitution-tracks.md`) is complete before referencing it in the `new-domain` / `new-service` section.
- [ ] For each of the 5 features, navigate to the new-codebase source tree and inspect the relevant scripts and skills.
  - [ ] Compare against old-codebase counterparts in `TargetProjects/lens-dev/old-codebase/lens.core.src/`.
- [ ] Run the SAFE_ID_PATTERN scan on `TargetProjects/lens/lens-governance/` (CF-2):
  - [ ] Record scan scope, date, and result.
  - [ ] If any IDs fail the pattern, halt and surface as critical finding.
- [ ] Author the parity-audit-report.md with all sections.
- [ ] Commit with message: `[DEV] TU-4.1 — author parity-audit-report.md (FR-8, FR-9, FR-10 to FR-12, FR-15)`.

## Dev Notes

- **Scope boundary**: Only the 5 features listed above are in scope. If the audit reveals gaps in other features, document them in an appendix note but do not expand the True Up scope.
- **SAFE_ID_PATTERN**: ADR-4 was adopted assuming a clean scan. If the scan fails (finds non-compliant IDs), the ADR is invalidated and a re-decision is required before this story can close.
- **Parity audit review window (CF-9)**: After this story commits, stakeholders for the 5 impacted features should have visibility into findings before TU-5.1 (blocker annotations) runs. This is an expected timing gap.
- **No governance writes in this story (CF-5)**: Blocker annotations are written in TU-5.1, not here.
- **Research order**: Audit `switch` and `new-domain` / `new-service` first (expected to be pass/minor gaps), then `new-feature` and `complete` (expected regressions). This ordering allows early wins before the heavy research sections.

### References

- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/architecture.md` — Section 3.4, Section 4 (ADR-3, ADR-4)
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/adr-complete-prerequisite.md` (TU-3.1 output)
- `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/adr-constitution-tracks.md` (TU-3.2 output)
- New-codebase source: `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/`
- Old-codebase source: `TargetProjects/lens-dev/old-codebase/lens.core.src/_bmad/lens-work/skills/`
- `TargetProjects/lens/lens-governance/` (SAFE_ID_PATTERN scan target)
- TU-5.1 story (companion — writes blocker annotations based on this report)

## Dev Agent Record

### Agent Model Used

TBD

### Debug Log References

### Completion Notes List

### File List
