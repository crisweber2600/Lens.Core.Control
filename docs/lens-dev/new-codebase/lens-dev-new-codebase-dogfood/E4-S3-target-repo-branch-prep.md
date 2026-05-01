---
feature: lens-dev-new-codebase-dogfood
epic: 4
story_id: E4-S3
sprint_story_id: S4.3
title: Implement target-repo branch preparation
type: new
points: M
status: ready
phase: dev
updated_at: '2026-05-01T14:30:00Z'
depends_on: [E4-S1]
blocks: [E5-S4]
target_repo: lens.core.src
target_branch: develop
---

# E4-S3 — Implement target-repo branch preparation

## Context

Before dev implementation can begin on a target repo, the correct branch must exist and be checked out. Branch strategy options: `flat` (directly on `develop`), `feature/{featureStub}`, or `feature/{featureStub}-{username}`. Branch prep must be idempotent and must register the target repo in `feature.yaml.target_repos` if not already present (M1 condition applied at runtime).

## Implementation Steps

1. Read the configured `target_branch_strategy` from `feature.yaml` or `bmadconfig.yaml`.
2. Apply the strategy to determine branch name.
3. If branch does not exist: create from `develop` (or configured base); push to remote.
4. If branch exists: verify it is current with remote; pull if behind.
5. Check that `feature.yaml.target_repos` includes the target repo; if not, add it and commit.
6. Make the operation idempotent: running twice on a clean repo produces no changes.
7. Write tests: flat strategy, feature-stub strategy, idempotent re-run.

## Acceptance Criteria

- [ ] Branch strategy read from `feature.yaml` or `bmadconfig.yaml`.
- [ ] Branch created from `develop` if absent; pulled if behind.
- [ ] `feature.yaml.target_repos` includes the target repo after this operation.
- [ ] Idempotent: second run on clean repo produces no changes.
- [ ] Tests cover flat strategy, feature-stub strategy, and idempotent re-run.

## Implementation Channel

**SKILL.md change:** `.github/skills/bmad-module-builder` (BMB module builder skill).

## Governance Coordination Note

This story reproduces reference behavior for clean-room parity testing. First-class Dev command authoring belongs to `lens-dev-new-codebase-dev`; any overlap with that feature's eventual PRD must be surfaced as explicit acceptance criteria alignment before implementation proceeds.

## Dev Notes

- Reference: tech-plan ADR-2, M1 (target_repos pre-Sprint-1 requirement), feature.yaml schema.

## Dev Agent Record

### Agent Model Used
TBD

### Debug Log References

### Completion Notes List

### File List
