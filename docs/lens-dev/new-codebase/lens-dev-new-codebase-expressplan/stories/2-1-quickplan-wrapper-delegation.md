---
feature: lens-dev-new-codebase-expressplan
doc_type: story-file
story_id: 2.1
status: ready-for-dev
goal: "Delegate QuickPlan through the Lens wrapper so expressplan stays compressed but governed"
depends_on:
  - 1.2
updated_at: 2026-04-27T22:50:00Z
---

# Story 2.1 - QuickPlan Wrapper Delegation

## User Story

As a feature author, I want expressplan to generate business, technical, and sprint planning docs in one session so I can move quickly without losing structured planning output.

## Acceptance Criteria

- Expressplan delegates to `bmad-lens-bmad-skill --skill bmad-lens-quickplan`.
- The wrapper resolves the active feature docs path and enforces it as the write scope.
- `business-plan.md`, `tech-plan.md`, and `sprint-plan.md` are written to the staged docs path.
- QuickPlan remains internal and does not reappear as a separate published command.

## Implementation Notes

- The wrapper contract is the boundary that keeps planning-doc writes out of governance and `.github/`.
- Preserve feature-context forwarding, not just raw file creation.
- Treat missing QuickPlan registration as a hard implementation defect.

## Validation Targets

- Wrapper invocation test
- Output path enforcement
- Artifact presence check for the three planning docs